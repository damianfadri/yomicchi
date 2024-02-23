using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Yomicchi.Desktop.UserControls
{
    /// <summary>
    /// Interaction logic for Badge.xaml
    /// </summary>
    public partial class Badge : UserControl
    {
        public static readonly DependencyProperty TextProperty = DependencyProperty.Register(nameof(Text), typeof(string), typeof(Badge));
        public static readonly DependencyProperty ColorProperty = DependencyProperty.Register(nameof(Color), typeof(Brush), typeof(Badge));
        public static readonly DependencyProperty ContrastColorProperty = DependencyProperty.Register(nameof(ContrastColor), typeof(Brush), typeof(Badge),
                new PropertyMetadata(new SolidColorBrush(System.Windows.Media.Color.FromRgb(255, 255, 255))));

        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }
        public Brush Color
        {
            get { return (Brush)GetValue(ColorProperty); }
            set { SetValue(ColorProperty, value); }
        }

        public Brush ContrastColor
        {
            get { return (Brush)GetValue(ContrastColorProperty); }
            set { SetValue(ContrastColorProperty, value); }
        }

        public Badge()
        {
            InitializeComponent();
        }
    }
}
