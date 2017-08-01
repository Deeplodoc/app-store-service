using System.Collections.Generic;

namespace AppStoreService.Core
{
    /// <summary>
    /// Read items.
    /// </summary>
    /// <typeparam name="T">Output item.</typeparam>
    public interface IRead<out T>
    {
        IEnumerable<T> Read();
    }
}