using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlohaNotificationDAL.Global
{
    public class DLBaseClass
    {
        public Aloha_Notification_Base_DbContext DbLink { get; set; }
        public DLBaseClass(string ConnectionString)
        {
            DbLink = new Aloha_Notification_Base_DbContext(ConnectionString);
        }
    }
}
