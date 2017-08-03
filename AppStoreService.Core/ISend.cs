using System.Threading.Tasks;

namespace AppStoreService.Core
{
    public interface ISend<T>
    {
        Task SendAsync(T item);
    }
}