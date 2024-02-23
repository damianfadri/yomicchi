namespace Yomicchi.Core
{
    public interface IRepository<T>
    {
        Task<IEnumerable<T>> FindAllAsync();

        Task<IEnumerable<T>> FindByAsync(string keyword);
    }
}
