using Aloha.Notification.Models;
using AlohaNotificationDAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        public List<ResultModel> GenerateNotifications()
        {
            List<ResultModel> lstResultModels = new List<ResultModel>();
            List<SubscriberDetailsModel> lstSubscribers = new List<SubscriberDetailsModel>();
            lstSubscribers = new SubscriptionDetailsDL().GetSubscribersList();
            
            foreach (SubscriberDetailsModel sd in lstSubscribers)
            {
                if ((sd.SubscriptionId.ToString().ToUpper() == "C1FC2C78-42FB-41FC-B42A-E82C9B5EAC52") )
                    continue;
                string subscriberID = sd.SubscriptionId.ToString();
                ResultModel mdlClinicalTeamResult = new GenerateNotificationDL(ConstructConnString(sd.UserDbServer, sd.UserDb)).InsertAddedToClinicalTeamNotification(subscriberID);
                lstResultModels.Add(mdlClinicalTeamResult);
                ResultModel mdlStaffAssignments = new GenerateNotificationDL(ConstructConnString(sd.UserDbServer, sd.UserDb)).InsertAddedToStaffCaseLoadNotification(subscriberID);
                lstResultModels.Add(mdlStaffAssignments);
                ResultModel mdlTeamAssignmentsResult = new GenerateNotificationDL(ConstructConnString(sd.UserDbServer, sd.UserDb)).InsertAddedToTeamCaseLoadNotification(subscriberID);
                lstResultModels.Add(mdlTeamAssignmentsResult);
                ResultModel mdlSupervisorResult = new GenerateNotificationDL(ConstructConnString(sd.UserDbServer, sd.UserDb)).InsertSupervisorNotification(subscriberID);
                lstResultModels.Add(mdlSupervisorResult);
                ResultModel mdlSubordinateResult = new GenerateNotificationDL(ConstructConnString(sd.UserDbServer, sd.UserDb)).InsertSubordinatNotification(subscriberID);
                lstResultModels.Add(mdlSubordinateResult);

                /*Here we can loop through the Subscriber list and send the database details to ConstructConnString() for connection string construction*/
            }



            //if (lstSubscribers.Count > 0)
            return lstResultModels;
            // else
            //return false;
        }

        public List<ResultModel> GenerateNotificationsOnceADay()
        {
            List<ResultModel> lstResultModels = new List<ResultModel>();
            List<SubscriberDetailsModel> lstSubscribers = new List<SubscriberDetailsModel>();
            lstSubscribers = new SubscriptionDetailsDL().GetSubscribersList();
            foreach (SubscriberDetailsModel sd in lstSubscribers)
            {
                if ((sd.SubscriptionId.ToString().ToUpper() == "C1FC2C78-42FB-41FC-B42A-E82C9B5EAC52"))
                    continue;
                string subscriberID = sd.SubscriptionId.ToString();
                ResultModel mdlBirthDayResult = new GenerateNotificationDL(ConstructConnString(sd.UserDbServer, sd.UserDb)).InsertBirthDayNotification(subscriberID);
                lstResultModels.Add(mdlBirthDayResult);
                ResultModel mdlQualExpResult = new GenerateNotificationDL(ConstructConnString(sd.UserDbServer, sd.UserDb)).InsertQualificationNotifications(subscriberID);
                lstResultModels.Add(mdlQualExpResult);
                ResultModel mdlIncompleteAppResult = new GenerateNotificationDL(ConstructConnString(sd.UserDbServer, sd.UserDb)).InsertIncompleteAppointmentNotification(subscriberID);
                lstResultModels.Add(mdlIncompleteAppResult);
                ResultModel mdlIncompleteTSResult = new GenerateNotificationDL(ConstructConnString(sd.UserDbServer, sd.UserDb)).InsertIncompleteTimeSheetsNotification(subscriberID);
                lstResultModels.Add(mdlIncompleteTSResult);
            }
            return lstResultModels;
        }

        public void UpdateLastRan()
        {
            GenerateNotificationDL.updateLastRanDAL();
        }

        public string ConstructConnString(string datasource, string database)
        {
            //string connectionString = "metadata=res://*/EDM.AlohaModel.csdl|res://*/EDM.AlohaModel.ssdl|res://*/EDM.AlohaModel.msl;provider=System.Data.SqlClient;provider connection string=';data source=" + datasource + ";initial catalog=" + database + ";persist security info=True;user id=aloha_appuser;password=t0pn0tch0726;multipleactiveresultsets=True;application name=EntityFramework';";
            string connectionString = "metadata=res://*/EDM.AlohaModel.csdl|res://*/EDM.AlohaModel.ssdl|res://*/EDM.AlohaModel.msl;provider=System.Data.SqlClient;provider connection string=';data source=" + datasource + ";initial catalog=" + database + ";persist security info=True;user id=aloha_appuser;password=t0pn0tch@0726;multipleactiveresultsets=True;application name=EntityFramework';";
            return connectionString;
        }
    }
}
