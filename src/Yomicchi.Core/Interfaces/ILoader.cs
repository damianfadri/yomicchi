namespace Yomicchi.Core.Interfaces
{
    public interface ILoader<T>
    {
        Task<T> LoadAsync(string source);
    }
}
