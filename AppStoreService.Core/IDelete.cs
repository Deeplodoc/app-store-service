namespace AppStoreService.Core
{
    /// <summary>
    /// Remove item.
    /// </summary>
    /// <typeparam name="T">Param for find item.</typeparam>
    public interface IDelete<in T>
    {
        void Delete(T itemIdent);
    }
}