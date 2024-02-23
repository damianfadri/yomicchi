using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using Yomicchi.Core.Events;
using Yomicchi.Core.Interfaces;

namespace Yomicchi.Core.ViewModels
{
    public class SnippingViewModel : ObservableObject
    {
        private readonly IScreenshot _screenshot;
        private readonly ITextReader _reader;

        public SnippingViewModel(IScreenshot screenshot, ITextReader reader)
        {
            _screenshot = screenshot;
            _reader = reader;
        }

        public async void SearchRegionForText(double x, double y, double width, double height)
        {
            if (width * height < 500)
            {
                return;
            }

            var imagePath = _screenshot.CaptureRegion(
                x, y, width, height);

            var result = await _reader.ReadAsync(imagePath);

            WeakReferenceMessenger.Default.Send(
                new TextDetectedEvent(
                    result.Text,
                    result.X + x,
                    result.Y + y,
                    result.Width,
                    result.Height));
        }
    }
}
