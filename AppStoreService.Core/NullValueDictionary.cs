using System.Collections.Generic;

namespace AppStoreService.Core
{
    public class NullValueDictionary<T, U> : Dictionary<T, U>
        where U : class
    {
        public new U this[T key]
        {
            get
            {
                if (key == null)
                {
                    return null;
                }

                TryGetValue(key, out U val);
                return val;
            }

            set => base[key] = value;
        }
    }
}