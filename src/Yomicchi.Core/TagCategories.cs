namespace Yomicchi.Core
{
    public class TagCategories
    {
        private readonly Dictionary<string, Tag> _categories;

        public TagCategories() 
        {
            _categories = new Dictionary<string, Tag>();
        }

        public void AddTags(IEnumerable<Tag> tags)
        {
            foreach (var tag in tags)
            {
                _categories.TryAdd(tag.Name, tag);
            }
        }

        public Tag GetFullTag(string name)
        {
            if (_categories.TryGetValue(name, out var tag))
            {
                return tag;
            }

            return new Tag(name);
        }
    }
}
