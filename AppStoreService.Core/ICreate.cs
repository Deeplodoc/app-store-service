using System.Threading.Tasks;

namespace AppStoreService.Core
{
    /// <summary>
    /// Create item.
    /// </summary>
    /// <typeparam name="T">Input model.</typeparam>
    public interface ICreate<T>
    {
        T Create(T item);
        Task<T> CreateAsync(T item);
    }
}