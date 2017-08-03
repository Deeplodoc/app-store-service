using System.Threading.Tasks;

namespace AppStoreService.Core
{
    /// <summary>
    /// Filtered items.
    /// </summary>
    /// <typeparam name="T">Input filter model.</typeparam>
    /// <typeparam name="U">Output model.</typeparam>
    public interface IFilter<in T, U>
    {
        U Filter(T model);
        Task<U> FilterAsync(T model);
    }
}