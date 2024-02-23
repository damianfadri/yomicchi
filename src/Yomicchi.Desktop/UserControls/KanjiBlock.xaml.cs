using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Yomicchi.Desktop.UserControls
{
    /// <summary>
    /// Interaction logic for KanjiBlock.xaml
    /// </summary>
    public partial class KanjiBlock : UserControl
    {
        public static readonly DependencyProperty FuriganaProperty = DependencyProperty.Register(nameof(Furigana), typeof(string), typeof(KanjiBlock));
        public static readonly DependencyProperty KanjiProperty = DependencyProperty.Register(nameof(Kanji), typeof(string), typeof(KanjiBlock));

        public KanjiBlock()
        {
            InitializeComponent();
        }

        public string Furigana
        {
            get { return (string)GetValue(FuriganaProperty); }
            set { SetValue(FuriganaProperty, value); }
        }

        public string Kanji
        {
            get { return (string)GetValue(KanjiProperty); }
            set { SetValue(KanjiProperty, value); }
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);

            var kanjiFontSize = 32;
            var kanjiLength = !string.IsNullOrWhiteSpace(Kanji) ? Kanji.Length : 1;
            var kanjiWidth = kanjiFontSize * kanjiLength;
            var kanjiWidthPerChar = kanjiFontSize;
            var kanjiMarginPerChar = 0;

            var kanaFontSize = 16;
            var kanaLength = !string.IsNullOrWhiteSpace(Furigana) ? Furigana.Length : 1;
            var kanaWidth = kanaFontSize * kanaLength;
            var kanaWidthPerChar = kanaFontSize;
            var kanaMarginPerChar = 0;

            if (kanaWidth > kanjiWidth)
            {
                kanjiWidthPerChar = kanaWidth / kanjiLength;
                kanjiMarginPerChar = (kanjiWidthPerChar - kanjiFontSize) / 2;
            }
            else
            {
                kanaWidthPerChar = kanjiWidth / kanaLength;
                kanaMarginPerChar = (kanaWidthPerChar - kanaFontSize) / 2;
            }

            RenderText(drawingContext, Furigana, kanaFontSize, kanaWidthPerChar, kanaMarginPerChar, 0);
            RenderText(drawingContext, Kanji, kanjiFontSize, kanjiWidthPerChar, kanjiMarginPerChar, kanaFontSize);
        }

        private void RenderText(DrawingContext drawingContext, string text, int fontSize, int widthPerChar, int marginPerChar, int y)
        {
            if (string.IsNullOrWhiteSpace(text))
                return;

            var currentPos = 0;
            foreach (var ch in text)
            {
                var typeface = new Typeface(FontFamily, FontStyle, FontWeight, FontStretch);
                var formattedText = new FormattedText(ch.ToString(), CultureInfo.CurrentUICulture, FlowDirection.LeftToRight, typeface, fontSize, Foreground, 96);

                drawingContext.DrawText(formattedText, new Point(currentPos + marginPerChar, y));
                currentPos += widthPerChar;
            }
        }
    }
}
