using Yomicchi.Core;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO.Compression;

namespace Yomicchi.Desktop.Services
{
    public class IndexLoader
    {
        public Source? Load(string filepath)
        {
            using var archive = ZipFile.OpenRead(filepath);

            var indexFile = archive.Entries.First(entry => entry.Name == "index.json");
            var source = JsonConvert.DeserializeObject<Source>(indexFile.Read(), new SourceJsonConverter(filepath));
            if (source == null)
            {
                return null;
            }

            return source;
        }
    }

    internal class SourceJsonConverter : JsonConverter
    {
        private readonly string _filepath;

        public SourceJsonConverter(string filepath)
        {
            _filepath = filepath;
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(Source);
        }

        public override object? ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
        {
            var obj = JObject.Load(reader);

            var title = obj["title"]?.Value<string>() ?? "Dictionary";
            var revision = obj["revision"]?.Value<string>() ?? "Revision";

            return new Source(_filepath, title, revision);
        }

        public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}
