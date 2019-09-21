using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace HAS.IdentityServer
{
    public class MPYProfileRepository : IMPYProfileRepository
    {
        private readonly CloudSettings _cloudSettings;

        private DbContext _dbContext;
        private IMongoCollection<MPYProfileDAO> _profile;

        public MPYProfileRepository(IOptions<CloudSettings> cloudSettings)
        {
            _cloudSettings = cloudSettings.Value;
            _dbContext = DbContext.Create(_cloudSettings.DBConnectionString_MongoDB_MPY_DatabaseName, _cloudSettings.DBConnectionString_MongoDB_MPY);
            _profile = _dbContext.Database.GetCollection<MPYProfileDAO>(_cloudSettings.DBConnectionString_MongoDB_MPY_CollectionName);
        }

        public async Task<MPYProfile> Find(Expression<Func<MPYProfileDAO, bool>> expression)
        {
            var dao = await Task.Run(() => _profile.AsQueryable().Where(expression).FirstOrDefault());
            if(dao != null)
            {
                return dao.ToEntity();
            }

            return null;
        }
    }
}
