using System.Windows;

namespace Yomicchi.Desktop.Models
{
    public class ModeChangedEventArgs : RoutedEventArgs
    {
        public bool Enabled { get; }

        public ModeChangedEventArgs(bool enabled) 
        {
            Enabled = enabled;
        }
    }
}