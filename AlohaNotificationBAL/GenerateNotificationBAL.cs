using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AlohaNotificationDAL;
using Aloha.Notification.Models;

namespace AlohaNotificationBAL
{
    public class GenerateNotificationBAL
    {
        public DataTable TestAdoNet()
        {
            DataTable dt = new DataTable();
            //dt = new GenerateNotificationDAL().TestAdoNet();
            return dt;
        }
        public bool GenerateNotifications()
        {
            List<SubscriberDetailsModel> lstSubscribers = new List<SubscriberDetailsModel>();
            //lstSubscribers = new SubscriptionDetailsDL().GetSubscribersList();

            //foreach(SubscriberDetails sd in lstSubscribers)
            //{

            //}
            
            ResultModel mdlBirthDayResult = new GenerateNotificationDL(ConstructConnString()).InsertBirthDayNotification("54E720FC-616B-44C6-8485-5F2185FD7B4C");
            //if (lstSubscribers.Count > 0)
                return true;
           // else
                //return false;
        }

        public string ConstructConnString()
        {
           string connectionString = "metadata=res://*/EDM.AlohaModel.csdl|res://*/EDM.AlohaModel.ssdl|res://*/EDM.AlohaModel.msl;provider=System.Data.SqlClient;provider connection string=';data source=104.238.74.145;initial catalog=ALOHA_V1_Dev;persist security info=True;user id=aloha_appuser;password=t0pn0tch0726;multipleactiveresultsets=True;application name=EntityFramework';";
            return connectionString;
        }
    }
}
