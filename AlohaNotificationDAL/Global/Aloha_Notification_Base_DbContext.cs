using AlohaNotificationDAL.EDM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlohaNotificationDAL.Global
{
    public class Aloha_Notification_Base_DbContext : ALOHA_V1_DevEntities
    {
        public Aloha_Notification_Base_DbContext(string connectionString) : base(connectionString)
        {

        }
    }
}
