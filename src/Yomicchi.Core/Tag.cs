namespace Yomicchi.Core
{
    public partial class Tag
    {
        private const string DefaultCategory = "default";
        private const string DefaultTagColor = "#8a8a91";

        public string Name { get; }
        public string Category { get; }
        public string Color { get; }
        public int Order { get; }

        public Tag(string name) : this(name, DefaultCategory, 0)
        {
        }

        public Tag(string name, string category) : this(name, category, 0)
        { 
        }

        public Tag(string name, string category, int order)
        {
            Name = name;
            Category = category;
            Color = Colors.TryGetValue(Category, out var color) ? color : DefaultTagColor;
            Order = order;
        }
    }
}
