using Yomicchi.Desktop.Models;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Yomicchi.Desktop.UserControls
{
    /// <summary>
    /// Interaction logic for SnippingTool.xaml
    /// </summary>
    public partial class SnippingTool : UserControl
    {
        public delegate void SnippedEventHandler(object? sender, SnippedEventArgs args);

        private const int BORDER_SIZE = 2;

        public static readonly DependencyProperty SnippedLeftProperty
            = DependencyProperty.Register(
                nameof(SnippedLeft),
                typeof(double),
                typeof(SnippingTool));

        public static readonly DependencyProperty SnippedTopProperty
            = DependencyProperty.Register(
                nameof(SnippedTop),
                typeof(double),
                typeof(SnippingTool));

        public static readonly DependencyProperty SnippedWidthProperty
            = DependencyProperty.Register(
                nameof(SnippedWidth),
                typeof(double),
                typeof(SnippingTool));

        public static readonly DependencyProperty SnippedHeightProperty
            = DependencyProperty.Register(
                nameof(SnippedHeight),
                typeof(double),
                typeof(SnippingTool));

        public static readonly RoutedEvent SnippedEvent =
            EventManager.RegisterRoutedEvent(
                nameof(Snipped), 
                RoutingStrategy.Bubble, 
                typeof(SnippedEventHandler), 
                typeof(SnippingTool));

        public event RoutedEventHandler Snipped
        {
            add { AddHandler(SnippedEvent, value); }
            remove { RemoveHandler(SnippedEvent, value); }
        }

        public double SnippedLeft
        {
            get { return (double)GetValue(SnippedLeftProperty); }
            set { SetValue(SnippedLeftProperty, value); }
        }

        public double SnippedTop
        {
            get { return (double)GetValue(SnippedTopProperty); }
            set { SetValue(SnippedTopProperty, value); }
        }

        public double SnippedWidth
        {
            get { return (double)GetValue(SnippedWidthProperty); }
            set { SetValue(SnippedWidthProperty, value); }
        }

        public double SnippedHeight
        {
            get { return (double)GetValue(SnippedHeightProperty); }
            set { SetValue(SnippedHeightProperty, value); }
        }

        private Point _startPoint;

        public SnippingTool()
        {
            InitializeComponent();

            Canvas.DataContext = this;
        }

        private void OnCanvasMouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Released)
            {
                return;
            }

            var pos = e.GetPosition(Canvas);

            var x = Math.Min(pos.X, _startPoint.X);
            var y = Math.Min(pos.Y, _startPoint.Y);

            var w = Math.Max(pos.X, _startPoint.X) - x;
            var h = Math.Max(pos.Y, _startPoint.Y) - y;

            SnippedWidth = w;
            SnippedHeight = h;
            SnippedLeft = x;
            SnippedTop = y;
        }

        private void OnCanvasMouseUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var x = SnippedLeft + BORDER_SIZE;
            var y = SnippedTop + BORDER_SIZE;
            var w = SnippedWidth - (2 * BORDER_SIZE);
            var h = SnippedHeight - (2 * BORDER_SIZE);

            var ev = new SnippedEventArgs(x, y, w, h);
            ev.RoutedEvent = SnippedEvent;

            RaiseEvent(ev);
        }

        private void OnCanvasMouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            _startPoint = e.GetPosition(Canvas);

            SnippedLeft = _startPoint.X;
            SnippedTop = _startPoint.Y;
        }
    }
}
