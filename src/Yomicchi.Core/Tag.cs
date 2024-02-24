namespace Yomicchi.Core
{
    public partial class Tag
    {
        private const string DefaultCategory = "default";
        private const string DefaultTagColor = "#8a8a91";

        public string Name { get; }
        public string Category { get; }
        public string Color { get; }

        public Tag(string name) : this(name, DefaultCategory)
        {
        }

        public Tag(string name, string category)
        {
            Name = name;
            Category = category;
            Color = Colors.TryGetValue(Category, out var color) ? color : DefaultTagColor;
        }
    }
}
