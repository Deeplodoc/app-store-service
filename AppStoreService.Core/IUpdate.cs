namespace AppStoreService.Core
{
    /// <summary>
    /// Update item.
    /// </summary>
    /// <typeparam name="T">Input item.</typeparam>
    public interface IUpdate<in T>
    {
        void Update(T item);
    }
}