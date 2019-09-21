using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace HAS.IdentityServer
{
    public interface IMPYProfileRepository
    {
        Task<MPYProfile> Find(Expression<Func<MPYProfileDAO, bool>> expression);
    }
}
