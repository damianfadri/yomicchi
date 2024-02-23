namespace Yomicchi.Core.Interfaces
{
    public interface IScreenshot
    {
        string CaptureRegion(double x, double y, double width, double height);
    }
}
