using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlohaNotificationDAL.EDM
{
    public partial class ALOHA_V1_DevEntities : DbContext
    {
        //public int AlohaTestProp { get; set; }
        public ALOHA_V1_DevEntities(string connection)
            : base(connection)
        {
        }
    }
}
