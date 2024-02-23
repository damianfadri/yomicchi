using Yomicchi.Core;
using Yomicchi.Core.Interfaces;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision.Models;
using Microsoft.Extensions.Options;
using System.Diagnostics;
using System.Drawing;
using System.IO;

namespace Yomicchi.Desktop.Services
{
    public class AzureTextReader : ITextReader
    {
        private const int numberOfCharsInOperationId = 36;

        private readonly AzureOptions _options;

        public AzureTextReader(IOptions<AzureOptions> options)
        {
            _options = options.Value;
        }

        public TextResult Read(string filepath)
        {
            return ReadAsync(filepath).GetAwaiter().GetResult();
        }

        public async Task<TextResult> ReadAsync(string filepath)
        {
            ComputerVisionClient client =
            new ComputerVisionClient(
                new ApiKeyServiceClientCredentials(_options.Key))
            {
                Endpoint = _options.Endpoint
            };

            var textHeaders = await client.ReadInStreamAsync(
                File.OpenRead(ResizeOrDefault(filepath)), "ja");

            string location = textHeaders.OperationLocation;

            string operationId = location
                .Substring(location.Length - numberOfCharsInOperationId);

            ReadOperationResult results;

            do
            {
                Trace.WriteLine("Requesting results...");
                results = await client.GetReadResultAsync(Guid.Parse(operationId));
            }
            while (results.Status == OperationStatusCodes.Running
                || results.Status == OperationStatusCodes.NotStarted);

            var analyzeResult = results.AnalyzeResult.ReadResults.FirstOrDefault();
            if (analyzeResult == null)
            {
                throw new InvalidOperationException("No read results were found.");
            }

            var line = analyzeResult.Lines.FirstOrDefault();
            if (line == null)
            {
                throw new InvalidOperationException("No line was found.");
            }

            var text = line.Text;
            var x = Math.Min(line.BoundingBox[0] ?? 0, line.BoundingBox[6] ?? 0);
            var y = Math.Min(line.BoundingBox[1] ?? 0, line.BoundingBox[3] ?? 0);
            var width = Math.Max(line.BoundingBox[2] ?? 0, line.BoundingBox[4] ?? 0) - x;
            var height = Math.Max(line.BoundingBox[5] ?? 0, line.BoundingBox[7] ?? 0) - y;

            return new TextResult(text, x, y, width, height);
        }

        private string ResizeOrDefault(string filepath)
        {
            using var original = new Bitmap(filepath);

            if (original.Width > 256 && original.Height > 256)
            {
                return filepath;
            }

            var resizedWidth = original.Width > 256 ? original.Width : 256;
            var resizedHeight = original.Height > 256 ? original.Height : 256;

            using var resized = new Bitmap(resizedWidth, resizedHeight);
            using var graphics = Graphics.FromImage(resized);

            var brush = new SolidBrush(original.GetPixel(0, 0));

            graphics.FillRectangle(brush, 0, 0, resizedWidth, resizedHeight);
            graphics.DrawImage(original, 0, 0);

            using var resizedStream = new MemoryStream();

            var directory = Path.GetDirectoryName(filepath) ?? string.Empty;
            var newFilename = Guid.NewGuid() + ".png";

            var newFilepath = Path.Combine(directory, newFilename);
            resized.Save(newFilepath);

            return newFilepath;
        }
    }
}
