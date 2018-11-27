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
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using static Aloha.Notification.Models.NotificationModel;

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
                Properties.Settings.Default["SubscriptionID"] = SubscriptionID;
                Properties.Settings.Default.Save();
                //OperationContext.Current.OutgoingMessageProperties.Add("SubscriptionID", SubscriptionID);

                bool isMonitored = (bool)(from def in DbLink.Definitions where def.CustomListValue.Code == "LKBDAY_EVNTMNTR" select def.IsMonitored).Single();
                ResultModel mdlResult = new ResultModel();
                if (isMonitored)
                {
                    int result = DbLink.usp_InsertStaffBirthDayBySubscriptionID(Guid.Parse(SubscriptionID));
                    DbLink.SaveChanges();
                    if (result > 0)
                    {
                        mdlResult.Message = "Success";
                        mdlResult.MessageReason = "Birthdays pushed successfully.";
                        mdlResult.IsSuccess = true;
                        return mdlResult;
                    }
                    else if (result == 0)
                    {
                        mdlResult.Message = "Success";
                        mdlResult.MessageReason = "Birthday notification service ran successfully.";
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
                    mdlResult.MessageReason = "Birthdays are not monitored";
                    mdlResult.IsSuccess = true;
                    return mdlResult;
                }


            }
            catch (Exception ex)
            {
                ResultModel mdlResult = new ResultModel();
                mdlResult.Message = "UnSuccessfull";
                mdlResult.MessageReason = ex.Message;
                mdlResult.IsSuccess = false;
                return mdlResult;
            }
        }
        public ResultModel InsertQualificationNotifications(string SubscriptionID)
        {
            try
            {
                Properties.Settings.Default["SubscriptionID"] = SubscriptionID;
                Properties.Settings.Default.Save();
                //OperationContext.Current.OutgoingMessageProperties.Add("SubscriptionID", SubscriptionID);

                bool isMonitored = (bool)(from def in DbLink.Definitions where def.CustomListValue.Code == "STFQUALEXP_EVNTMNTR" select def.IsMonitored).Single();
                ResultModel mdlResult = new ResultModel();
                if (isMonitored)
                {
                    string resultMessage = string.Empty;
                    #region old code
                    //List<ParameterValues> lstNotifyVariations = (from PV in DbLink.ParameterValues
                    //                                             join P in DbLink.Parameters on PV.Parameter_Id equals P.Parameter_Id
                    //                                             join def in DbLink.Definitions on P.Definition_Id equals def.Definition_Id
                    //                                             where def.Code == "QA" && PV.IsRemoved == false
                    //                                             select new ParameterValues
                    //                                             {
                    //                                                 ParameterValue_Id = PV.ParameterValue_Id,
                    //                                                 ParameterValue = PV.ParameterValue1
                    //                                             }).ToList();
                    #endregion
                    List<ParameterValues> lstNotifyVariations = (from PV in DbLink.ParameterValues where PV.Parameter.Definition.CustomListValue.Code == "STFQUALEXP_EVNTMNTR" && PV.IsRemoved==false
                                                                  select new ParameterValues
                                                                  {
                                                                      ParameterValue_Id = PV.ParameterValue_Id,
                                                                      ParameterValue = PV.ParameterValue1
                                                                  }).ToList();
                    foreach (ParameterValues pv in lstNotifyVariations)
                    {
                        int iDuration = 1;
                        string strHowOften = "in " + pv.ParameterValue;
                        if (pv.ParameterValue.ToLower().Contains("two"))
                            iDuration = 2;
                        if (pv.ParameterValue.ToLower().Contains("three"))
                            iDuration = 3;
                        if (pv.ParameterValue.ToLower().Contains("six"))
                            iDuration = 6;
                        if (pv.ParameterValue.ToLower().Contains("0"))
                        {
                            iDuration = 0;
                            strHowOften = "today";
                        }
                        int rowsInserted = DbLink.usp_InsertQualificationExpirationBySubscriptionID(Guid.Parse(SubscriptionID), iDuration, strHowOften);
                        if (rowsInserted > 0)
                            resultMessage += " Qulification Expiration notifications pushed successfully for " + pv.ParameterValue + " - " + SubscriptionID + ".";
                        else if(rowsInserted==0)
                            resultMessage += " Qulification Expiration service ran successfully for " + pv.ParameterValue + " - " + SubscriptionID + ".";
                        else
                            resultMessage += " Qulification service failed for " + pv.ParameterValue + " - " + SubscriptionID + ".";
                    }
                    if(resultMessage.Contains("failed") && resultMessage.Contains("successfully"))
                    {
                        mdlResult.Message = "Partial Success";
                        mdlResult.MessageReason = resultMessage;
                        mdlResult.IsSuccess = true;
                    }
                    else if (!resultMessage.Contains("failed"))
                    {
                        mdlResult.Message = "Success";
                        mdlResult.MessageReason = resultMessage;
                        mdlResult.IsSuccess = true;
                    }
                    else if (!resultMessage.Contains("successfully"))
                    {
                        mdlResult.Message = "UnSuccessfull";
                        mdlResult.MessageReason = resultMessage;
                        mdlResult.IsSuccess = false;
                    }
                    return mdlResult;

                }
                else
                {
                    mdlResult.Message = "Success";
                    mdlResult.MessageReason = "Qualification are not monitored";
                    mdlResult.IsSuccess = true;
                    return mdlResult;
                }


            }
            catch (Exception ex)
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
