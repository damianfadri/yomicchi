namespace Yomicchi.Core.Interfaces
{
    public interface ITextReader
    {
        TextResult Read(string filepath);

        Task<TextResult> ReadAsync(string filepath);
    }
}
