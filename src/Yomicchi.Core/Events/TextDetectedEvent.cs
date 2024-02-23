namespace Yomicchi.Core.Events
{
    public record TextDetectedEvent(string Text, double X, double Y, double Width, double Height);
}
