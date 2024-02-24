using Yomicchi.Core;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;
using System.IO.Compression;
using System.Text.RegularExpressions;

namespace Yomicchi.Desktop.Services
{
    public partial class TermV3Loader
    {
        private static readonly string DictionaryDirectory
            = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                "Yomicchi",
                "Dictionaries");

        [GeneratedRegex("tag_bank_.*\\.json")]
        private static partial Regex TagBankPattern();
        [GeneratedRegex("term_bank_.*\\.json")]
        private static partial Regex TermBankPattern();

        public IEnumerable<Term> Load(string filepath)
        {
            Directory.CreateDirectory(DictionaryDirectory);

            var destination = Path.Combine(DictionaryDirectory, Path.GetFileName(filepath));
            if (!filepath.Equals(destination))
            {
                File.Copy(filepath, destination, true);
            }

            using var archive = ZipFile.OpenRead(filepath);

            var indexFile = archive.Entries.First(entry => entry.Name == "index.json");
            var source = JsonConvert.DeserializeObject<Source>(indexFile.Read(), new SourceJsonConverter(filepath));
            if (source == null)
            {
                yield break;
            }


            var tagCategories = new TagCategories();
            foreach (var tagFile in archive.Entries.Where(
                entry => TagBankPattern().IsMatch(entry.Name)))
            {
                var tags = JsonConvert.DeserializeObject<IEnumerable<Tag>>(tagFile.Read(), new TagJsonConverter())
                    ?? new List<Tag>();

                tagCategories.AddTags(tags);
            }

            foreach (var termFile in archive.Entries.Where(
                entry => TermBankPattern().IsMatch(entry.Name)))
            {
                var terms = JsonConvert.DeserializeObject<IEnumerable<Term>>(
                    termFile.Read(), new TermV3JsonConverter(source, tagCategories));

                if (terms == null)
                {
                    continue;
                }

                foreach (var term in terms.Where(term => term != null))
                {
                    yield return term;
                }
            }
        }
    }

    internal class TagJsonConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(Tag);
        }

        public override object? ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
        {
            var array = JArray.Load(reader);
            if (array.Count != 5)
            {
                throw new ArgumentException("Invalid format for tag bank.");
            }

            var name = array[0]?.Value<string>() ?? "name";
            var category = array[1]?.Value<string>() ?? "default";
            var order = array[2]?.Value<int>() ?? 0;

            return new Tag(name, category, order);
        }

        public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }

    internal class TermV3JsonConverter : JsonConverter
    {
        private readonly Source _source;
        private readonly TagCategories _categories;

        public TermV3JsonConverter(Source source, TagCategories categories)
        {
            _source = source;
            _categories = categories;
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(Term);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
        {
            var array = JArray.Load(reader);
            if (array.Count != 8)
            {
                throw new ArgumentException("Invalid format for term bank.");
            }

            var kanji = array[0]?.Value<string>();
            var reading = array[1]?.Value<string>();

            var partsOfSpeech = array[2]?
                .Value<string>()?
                .Tokenize()
                .Select(strPartOfSpeech => _categories.GetFullTag(strPartOfSpeech))
                .ToList();

            var popularityScore = array[4]?.Value<int>() ?? 0;

            var definitions = array[5]?
                .ToObject<string[]>()?
                .ToList();

            var tags = array[7]?
                .Value<string>()?
                .Tokenize()
                .Select(strTag => _categories.GetFullTag(strTag))
                .ToList();

            var definition = new Definition(
                source: new Tag(_source.Title, "dictionary"),
                definitions: definitions ?? [],
                partsOfSpeech: partsOfSpeech ?? []
            );

            if (kanji == null || reading == null)
            {
                throw new InvalidOperationException("Kanji and/or reading is null.");
            }

            var term = new Term(kanji, reading, popularityScore);
            if (tags != null)
            {
                term.SetTags(tags);
            }

            if (definition != null)
            {
                term.AddDefinition(definition);
            }

            return term;
        }

        public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }

    internal static class StringHelpers
    {
        public static IEnumerable<string> Tokenize(this string text, char delimiter = ' ')
        {
            return text.Split(delimiter).Where(t => !string.IsNullOrWhiteSpace(t));
        }
    }

    internal static class ZipArchiveEntryExtensions
    {
        public static string Read(this ZipArchiveEntry entry)
        {
            using var stream = entry.Open();
            using var reader = new StreamReader(stream);

            return reader.ReadToEnd();
        }
    }
}
