namespace Yomicchi.Core
{
    public class Node<T>
    {
        public char Key { get; }
        public List<T> Values { get; }
        public Node<T>? Parent { get; }
        public List<Node<T>> Children { get; }

        public Node(char key)
        {
            Key = key;
            Values = new List<T>();

            Parent = null;
            Children = new List<Node<T>>();
        }

        public Node(char key, params T[] values)
        {
            Key = key;
            Values = values.ToList();

            Parent = null;
            Children = new List<Node<T>>();
        }
    }
}
