using Yomicchi.Core.Interfaces;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace Yomicchi.Desktop.Services
{
    public class Win32Screenshot : IScreenshot
    {
        private static readonly string ScreenshotDirectory =
            Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                "Yomicchi",
                "Screenshots");

        public Win32Screenshot()
        {
            Directory.CreateDirectory(ScreenshotDirectory);
        }

        public string CaptureRegion(double x, double y, double width, double height)
        {
            Trace.WriteLine($"SnippingTool::CaptureRegion({x}, {y}, {width}, {height})");

            using var bmp = new Bitmap((int)width, (int)height, PixelFormat.Format32bppArgb);
            using var g = Graphics.FromImage(bmp);

            var dpi = GetCurrentDpi();

            SizeF scale = new SizeF()
            {
                Width = dpi.Width / 96f,
                Height = dpi.Height / 96f
            };

            width *= scale.Width;
            height *= scale.Height;

            g.CopyFromScreen((int)x, (int)y, 0, 0, new Size((int)width, (int)height), CopyPixelOperation.SourceCopy);

            var filename = $"{Guid.NewGuid()}.png";
            var filepath = Path.Combine(ScreenshotDirectory, filename);
            bmp.Save(filepath, ImageFormat.Png);

            return filepath;
        }

        public static SizeF GetCurrentDpi()
        {
            using (Graphics g = Graphics.FromHwnd(Windows.Win32.PInvoke.GetDesktopWindow()))
            {
                var result = new SizeF()
                {
                    Width = g.DpiX,
                    Height = g.DpiY
                };

                return result;
            }
        }
    }
}
