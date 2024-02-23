using System.Windows;

namespace Yomicchi.Desktop.Models
{
    public class SnippedEventArgs : RoutedEventArgs
    {
        public double X { get; }
        public double Y { get; }
        public double Width { get; }
        public double Height { get; }

        public SnippedEventArgs(double x, double y, double w, double h)
        {
            X = x;
            Y = y;
            Width = w;
            Height = h;
        }
    }
}
