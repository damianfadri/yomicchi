using Yomicchi.Desktop.Models;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;

namespace Yomicchi.Desktop.UserControls
{
    /// <summary>
    /// Interaction logic for HoverMode.xaml
    /// </summary>
    public partial class HoverMode : UserControl
    {
        private static readonly ModifierKeys FLAG_KEY = ModifierKeys.Shift;

        public delegate void ModeChangedEventHandler(object? sender, ModeChangedEventArgs args);

        private readonly DispatcherTimer _timer;

        public static readonly RoutedEvent ModeChangedEvent =
            EventManager.RegisterRoutedEvent(
                nameof(ModeChanged),
                RoutingStrategy.Bubble,
                typeof(ModeChangedEventHandler),
                typeof(HoverMode));

        public event RoutedEventHandler ModeChanged
        {
            add { AddHandler(ModeChangedEvent, value); }
            remove { RemoveHandler(ModeChangedEvent, value); }
        }

        private volatile bool _enabled;

        public HoverMode()
        {
            InitializeComponent();

            _timer = new DispatcherTimer(DispatcherPriority.Send);
            _timer.Interval = TimeSpan.FromMilliseconds(1000);
            _timer.IsEnabled = true;
            _timer.Tick += OnTick;
            _timer.Start();
        }

        private void OnTick(object? sender, EventArgs e)
        {
            if (!_enabled && Keyboard.Modifiers.HasFlag(FLAG_KEY))
            {
                _enabled = true;

                var ev = new ModeChangedEventArgs(true);
                ev.RoutedEvent = ModeChangedEvent;

                RaiseEvent(ev);

                return;
            }

            if (_enabled && !Keyboard.Modifiers.HasFlag(FLAG_KEY))
            {
                _enabled = false;

                var ev = new ModeChangedEventArgs(false);
                ev.RoutedEvent = ModeChangedEvent;

                RaiseEvent(ev);

                return;
            }
        }
    }
}
