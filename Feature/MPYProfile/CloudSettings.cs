using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HAS.IdentityServer
{
    public partial class CloudSettings
    {
        public string DBConnectionString_MongoDB_MPY { get; set; }
        public string DBConnectionString_MongoDB_MPY_DatabaseName { get; set; }
        public string DBConnectionString_MongoDB_MPY_CollectionName { get; set; }
    }
}
