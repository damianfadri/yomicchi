namespace Yomicchi.Core
{
    public record TextResult(string Text, double X, double Y, double Width, double Height)
    {
        public TextResult(string Text) : this(Text, 0, 0, 0, 0) { }
    }
}
