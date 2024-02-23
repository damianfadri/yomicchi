namespace Yomicchi.Core
{
    public class Trie<T>
    {
        public Node<T> Root { get; }

        public Trie()
        {
            Root = new Node<T>(default);
        }

        public void Insert(string keys, T value)
        {
            var curr = Root;
            for (int i = 0; i < keys.Length; i++)
            {
                var key = keys[i];

                var matchingNode = curr.Children.FirstOrDefault(child => child.Key.Equals(key));
                if (matchingNode == null)
                {
                    var newNode = new Node<T>(key);

                    curr.Children.Add(newNode);
                    curr = newNode;
                }
                else
                {
                    curr = matchingNode;
                }
            }

            curr.Values.Add(value);
        }

        public T? Get(string keys)
        {
            return GetAll(keys).FirstOrDefault();
        }

        public List<T> GetAll(string keys)
        {
            var curr = Root;
            for (int i = 0; i < keys.Length; i++)
            {
                var key = keys[i];

                var matchingNode = curr.Children.FirstOrDefault(child => child.Key.Equals(key));
                if (matchingNode == null)
                {
                    return new List<T>();
                }

                curr = matchingNode;
            }

            return curr.Values;
        }
    }
}
