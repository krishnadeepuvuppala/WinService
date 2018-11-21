using Aloha.Notification.Models;
using AlohaNotificationDAL.Global;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Infrastructure.Interception;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlohaNotificationDAL
{
    public class GenerateNotificationDL : DLBaseClass
    {
        public string connString { get; set; }
        
        public GenerateNotificationDL(string ConnectionString) : base(ConnectionString)
        {
            connString = ConnectionString;
        }
        public DataTable TestAdoNet()
        {
            DataTable dt = new DataTable();
            dt = new SqlHelper().TestAdoNet();
            return dt;
        }

        public ResultModel InsertBirthDayNotification(string SubscriptionID)
        {
            try
            {
                var isMonitored1 = from def in DbLink.Definitions where def.Code == "BDAY" select def.IsMonitored;
                foreach (var v in isMonitored1)
                {
                    bool b =(bool) v;
                }

                bool isMonitored = (bool)(from def in DbLink.Definitions where def.Code == "BDAY" select def.IsMonitored).Single();
                ResultModel mdlResult = new ResultModel();
                if (isMonitored)
                {
                    int result = DbLink.usp_InsertStaffBirthDayBySubscriptionID(Guid.Parse(SubscriptionID));
                    DbLink.SaveChanges();
                    if (result >= 0)
                    {
                        mdlResult.Message = "Success";
                        mdlResult.MessageReason = "Birthdays pushed successfully.";
                        mdlResult.IsSuccess = true;
                        return mdlResult;
                    }

                    else
                    {
                        mdlResult.Message = "UnSuccessfull";
                        mdlResult.MessageReason = "Birthdays did not get pushed. Something went wrong!";
                        mdlResult.IsSuccess = false;
                        return mdlResult;
                    }
                }
                else
                {
                    mdlResult.Message = "Success";
                    mdlResult.MessageReason = "Birthdays are not Monitored";
                    mdlResult.IsSuccess = true;
                    return mdlResult;
                }

                
            }
            catch(Exception ex)
            {
                ResultModel mdlResult = new ResultModel();
                mdlResult.Message = "UnSuccessfull";
                mdlResult.MessageReason = ex.Message;
                mdlResult.IsSuccess = false;
                return mdlResult;
            }
        }
    }

 
}
