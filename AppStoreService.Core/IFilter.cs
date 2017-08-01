namespace AppStoreService.Core
{
    /// <summary>
    /// Filtered items.
    /// </summary>
    /// <typeparam name="T">Input filter model.</typeparam>
    /// <typeparam name="U">Output model.</typeparam>
    public interface IFilter<in T, out U>
    {
        U Filter(T model);
    }
}