namespace Yomicchi.Core
{
    public class Tag
    {
        private const string DefaultTagColor = "#8a8a91";

        public string Name { get; }
        public string Color { get; }

        public Tag(string name)
        {
            Name = name;
            Color = DefaultTagColor;
        }
        public Tag(string name, string category)
        {
            Name = name;
            Color = GetColor(category);
        }

        private string GetColor(string category)
        {
            // TODO: Get actual color
            return DefaultTagColor;
        }
    }
}
