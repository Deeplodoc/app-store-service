using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppStoreService.Core;
using AppStoreService.Core.Entities;

namespace AppStoreService.Dal.Repositories
{
    public class UserRepository : ICreate<User>, IUpdate<User>, IDelete<User>
    {
        public User Create(User item)
        {
            throw new NotImplementedException();
        }

        public void Delete(User itemIdent)
        {
            throw new NotImplementedException();
        }

        public void Update(User item)
        {
            throw new NotImplementedException();
        }
    }
}