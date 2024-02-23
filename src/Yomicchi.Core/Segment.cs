namespace Yomicchi.Core
{
    public class Segment
    {
        public string Text { get; set; }
        public string Furigana { get; set; }

        public Segment(string text) : this(text, string.Empty) { }

        public Segment(string text, string ruby)
        {
            Text = text;
            Furigana = ruby;
        }
    }
}
