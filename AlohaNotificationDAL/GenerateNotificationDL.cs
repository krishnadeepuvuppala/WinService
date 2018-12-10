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
                    mdlResult.MessageReason = "Birthdays are not monitored for - " + SubscriptionID;
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
                    List<ParameterValues> lstNotifyVariations = (from PV in DbLink.ParameterValues
                                                                 where PV.Parameter.Definition.CustomListValue.Code == "STFQUALEXP_EVNTMNTR" && PV.IsRemoved == false
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
                        else if (rowsInserted == 0)
                            resultMessage += " Qulification Expiration service ran successfully for " + pv.ParameterValue + " - " + SubscriptionID + ".";
                        else
                            resultMessage += " Qulification service failed for " + pv.ParameterValue + " - " + SubscriptionID + ".";
                    }
                    if (resultMessage.Contains("failed") && resultMessage.Contains("successfully"))
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
                    mdlResult.MessageReason = "Qualification are not monitored for - " + SubscriptionID;
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
        public ResultModel InsertIncompleteAppointmentNotification(string SubscriptionID)
        {
            try
            {
                Properties.Settings.Default["SubscriptionID"] = SubscriptionID;
                Properties.Settings.Default.Save();
                //OperationContext.Current.OutgoingMessageProperties.Add("SubscriptionID", SubscriptionID);

                bool isMonitored = (bool)(from def in DbLink.Definitions where def.CustomListValue.Code == "STFINCMPLAPPT_EVNTMNTR" select def.IsMonitored).Single();
                ResultModel mdlResult = new ResultModel();
                if (isMonitored)
                {
                    var daysResult = (from PV in DbLink.ParameterValues
                                                 where PV.Parameter.Definition.CustomListValue.Code == "STFINCMPLAPPT_EVNTMNTR" && PV.IsRemoved == false
                                                 select PV.ParameterValue1);
                    int settingDays = 0;
                    foreach (var v in daysResult)
                    {
                        settingDays = int.Parse(v);
                    }

                    DateTime minCompareDateTime = DateTime.UtcNow.AddDays(-settingDays);
                    int result = DbLink.usp_InsertIncompleteAppointmentsBySubscriptionID(Guid.Parse(SubscriptionID), settingDays, (long)(minCompareDateTime - new DateTime(1970, 1, 1)).TotalSeconds );
                    DbLink.SaveChanges();
                    if (result > 0)
                    {
                        mdlResult.Message = "Success";
                        mdlResult.MessageReason = "Incomplete Appointment pushed successfully.";
                        mdlResult.IsSuccess = true;
                        return mdlResult;
                    }
                    else if (result == 0)
                    {
                        mdlResult.Message = "Success";
                        mdlResult.MessageReason = "Incomplete Appointment notification service ran successfully.";
                        mdlResult.IsSuccess = true;
                        return mdlResult;
                    }
                    else
                    {
                        mdlResult.Message = "UnSuccessfull";
                        mdlResult.MessageReason = "Incomplete Appointment did not get pushed. Something went wrong!";
                        mdlResult.IsSuccess = false;
                        return mdlResult;
                    }
                }
                else
                {
                    mdlResult.Message = "Success";
                    mdlResult.MessageReason = "Incomplete Appointment are not monitored for - " + SubscriptionID;
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
        public ResultModel InsertIncompleteTimeSheetsNotification(string SubscriptionID)
        {
            try
            {
                Properties.Settings.Default["SubscriptionID"] = SubscriptionID;
                Properties.Settings.Default.Save();
                //OperationContext.Current.OutgoingMessageProperties.Add("SubscriptionID", SubscriptionID);

                bool isMonitored = (bool)(from def in DbLink.Definitions where def.CustomListValue.Code == "STFSBMTS_EVNTMNTR" select def.IsMonitored).Single();
                ResultModel mdlResult = new ResultModel();
                if (isMonitored)
                {
                    var daysResult = (from PV in DbLink.ParameterValues
                                                 where PV.Parameter.Definition.CustomListValue.Code == "STFSBMTS_EVNTMNTR" && PV.IsRemoved == false
                                                 select PV.ParameterValue1);
                    int settingDays = 0;
                    foreach (var v in daysResult)
                    {
                        settingDays = int.Parse(v);
                    }
                    if (DbLink.PayrollProcessBatches.Count() > 0)
                    {
                        var lastObject = DbLink.PayrollProcessBatches.OrderByDescending(item => item.PayrollPeriodEndDate).First();
                        System.DateTime PayrollPeriodEndDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
                        PayrollPeriodEndDateTime = PayrollPeriodEndDateTime.AddSeconds((long)lastObject.PayrollPeriodEndDate).ToLocalTime();
                        TimeSpan difference = DateTime.UtcNow - PayrollPeriodEndDateTime;
                        double days = difference.TotalDays;
                        //if (days <= settingDays)
                        //{
                            int result = DbLink.usp_InsertSubmitTimesheetBySubscriptionID(Guid.Parse(SubscriptionID));
                            DbLink.SaveChanges();
                            if (result > 0)
                            {
                                mdlResult.Message = "Success";
                                mdlResult.MessageReason = "Incomplete timesheets pushed successfully for -"+ SubscriptionID;
                                mdlResult.IsSuccess = true;
                                return mdlResult;
                            }
                            else if (result == 0)
                            {
                                mdlResult.Message = "Success";
                                mdlResult.MessageReason = "Incomplete timesheets notification service ran successfully for -" + SubscriptionID;
                                mdlResult.IsSuccess = true;
                                return mdlResult;
                            }
                            else
                            {
                                mdlResult.Message = "UnSuccessfull";
                                mdlResult.MessageReason = "Incomplete timesheets did not get pushed. Something went wrong! for -" + SubscriptionID;
                                mdlResult.IsSuccess = false;
                                return mdlResult;
                            }
                        //}
                        //else
                        //{
                        //    mdlResult.Message = "Success";
                        //    mdlResult.MessageReason = "Incomplete timesheets service ran successfully for -" + SubscriptionID;
                        //    mdlResult.IsSuccess = true;
                        //    return mdlResult;
                        //}
                    }
                    else
                    {
                        mdlResult.Message = "Success";
                        mdlResult.MessageReason = "Incomplete timesheets did not get pushed as there are no ProcessPayrollBatch records for -" + SubscriptionID;
                        mdlResult.IsSuccess = true;
                        return mdlResult;
                    }

                }
                else
                {
                    mdlResult.Message = "Success";
                    mdlResult.MessageReason = "Incomplete timesheets are not monitored for - " + SubscriptionID;
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

        public ResultModel InsertAddedToClinicalTeamNotification(string SubscriptionID)
        {
            try
            {
                Properties.Settings.Default["SubscriptionID"] = SubscriptionID;
                Properties.Settings.Default.Save();
                //OperationContext.Current.OutgoingMessageProperties.Add("SubscriptionID", SubscriptionID);

                bool isMonitored = (bool)(from def in DbLink.Definitions where def.CustomListValue.Code == "CTADD_EVNTMNTR" select def.IsMonitored).Single();
                ResultModel mdlResult = new ResultModel();
                if (isMonitored)
                {
                    var LastRan =(long)Properties.Settings.Default["LastRan"];
                    //System.DateTime PayrollPeriodEndDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
                    //PayrollPeriodEndDateTime = PayrollPeriodEndDateTime.AddSeconds((long)lastObject.PayrollPeriodEndDate).ToLocalTime();

                    int result = DbLink.usp_InsertClinicalTeamNotificationBySubscriptionID(Guid.Parse(SubscriptionID),LastRan);
                    DbLink.SaveChanges();
                    if (result > 0)
                    {
                        mdlResult.Message = "Success";
                        mdlResult.MessageReason = "Clinical team addition pushed successfully for -" + SubscriptionID;
                        mdlResult.IsSuccess = true;
                        return mdlResult;
                    }
                    else if (result == 0)
                    {
                        mdlResult.Message = "Success";
                        mdlResult.MessageReason = "Clinical team addition notification service ran successfully for -" + SubscriptionID;
                        mdlResult.IsSuccess = true;
                        return mdlResult;
                    }
                    else
                    {
                        mdlResult.Message = "UnSuccessfull";
                        mdlResult.MessageReason = "Clinical team addition did not get pushed. Something went wrong! for -" + SubscriptionID;
                        mdlResult.IsSuccess = false;
                        return mdlResult;
                    }

                }
                else
                {
                    mdlResult.Message = "Success";
                    mdlResult.MessageReason = "Clinical team addition are not monitored for - " + SubscriptionID;
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
        public ResultModel InsertAddedToStaffCaseLoadNotification(string SubscriptionID)
        {
            try
            {
                Properties.Settings.Default["SubscriptionID"] = SubscriptionID;
                Properties.Settings.Default.Save();
                //OperationContext.Current.OutgoingMessageProperties.Add("SubscriptionID", SubscriptionID);

                bool isMonitored = (bool)(from def in DbLink.Definitions where def.CustomListValue.Code == "NEWCLNTJNDTEAM_EVNTMNTR" select def.IsMonitored).Single();
                ResultModel mdlResult = new ResultModel();
                if (isMonitored)
                {
                    var LastRan = (long)Properties.Settings.Default["LastRan"];
                    //System.DateTime PayrollPeriodEndDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
                    //PayrollPeriodEndDateTime = PayrollPeriodEndDateTime.AddSeconds((long)lastObject.PayrollPeriodEndDate).ToLocalTime();

                    int result = DbLink.usp_InsertStaffAssignmentsNotificationBySubscriptionID(Guid.Parse(SubscriptionID), LastRan);
                    DbLink.SaveChanges();
                    if (result > 0)
                    {
                        mdlResult.Message = "Success";
                        mdlResult.MessageReason = "Client assignment for staff pushed successfully for -" + SubscriptionID;
                        mdlResult.IsSuccess = true;
                        return mdlResult;
                    }
                    else if (result == 0)
                    {
                        mdlResult.Message = "Success";
                        mdlResult.MessageReason = "Client assignment for staff notification service ran successfully for -" + SubscriptionID;
                        mdlResult.IsSuccess = true;
                        return mdlResult;
                    }
                    else
                    {
                        mdlResult.Message = "UnSuccessfull";
                        mdlResult.MessageReason = "Client assignment for staff did not get pushed. Something went wrong! for -" + SubscriptionID;
                        mdlResult.IsSuccess = false;
                        return mdlResult;
                    }

                }
                else
                {
                    mdlResult.Message = "Success";
                    mdlResult.MessageReason = "Client assignment for staff are not monitored for - " + SubscriptionID;
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
        public ResultModel InsertAddedToTeamCaseLoadNotification(string SubscriptionID)
        {
            try
            {
                Properties.Settings.Default["SubscriptionID"] = SubscriptionID;
                Properties.Settings.Default.Save();
                //OperationContext.Current.OutgoingMessageProperties.Add("SubscriptionID", SubscriptionID);

                bool isMonitored = (bool)(from def in DbLink.Definitions where def.CustomListValue.Code == "NEWCLNTJNDTEAM_EVNTMNTR" select def.IsMonitored).Single();
                ResultModel mdlResult = new ResultModel();
                if (isMonitored)
                {
                    var LastRan = (long)Properties.Settings.Default["LastRan"];
                    //System.DateTime PayrollPeriodEndDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
                    //PayrollPeriodEndDateTime = PayrollPeriodEndDateTime.AddSeconds((long)lastObject.PayrollPeriodEndDate).ToLocalTime();

                    int result = DbLink.usp_InsertTeamAssignmentsNotificationBySubscriptionID(Guid.Parse(SubscriptionID), LastRan);
                    DbLink.SaveChanges();
                    if (result > 0)
                    {
                        mdlResult.Message = "Success";
                        mdlResult.MessageReason = "Client assignment for Team pushed successfully for -" + SubscriptionID;
                        mdlResult.IsSuccess = true;
                        return mdlResult;
                    }
                    else if (result == 0)
                    {
                        mdlResult.Message = "Success";
                        mdlResult.MessageReason = "Client assignment for Team notification service ran successfully for -" + SubscriptionID;
                        mdlResult.IsSuccess = true;
                        return mdlResult;
                    }
                    else
                    {
                        mdlResult.Message = "UnSuccessfull";
                        mdlResult.MessageReason = "Client assignment for Team did not get pushed. Something went wrong! for -" + SubscriptionID;
                        mdlResult.IsSuccess = false;
                        return mdlResult;
                    }

                }
                else
                {
                    mdlResult.Message = "Success";
                    mdlResult.MessageReason = "Client assignment for Team are not monitored for - " + SubscriptionID;
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
        public ResultModel InsertSupervisorNotification(string SubscriptionID)
        {
            try
            {
                Properties.Settings.Default["SubscriptionID"] = SubscriptionID;
                Properties.Settings.Default.Save();
                //OperationContext.Current.OutgoingMessageProperties.Add("SubscriptionID", SubscriptionID);

                bool isMonitored = (bool)(from def in DbLink.Definitions where def.CustomListValue.Code == "SPVRCHNG_EVNTMNTR" select def.IsMonitored).Single();
                ResultModel mdlResult = new ResultModel();
                if (isMonitored)
                {
                    var LastRan = (long)Properties.Settings.Default["LastRan"];
                    //System.DateTime PayrollPeriodEndDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
                    //PayrollPeriodEndDateTime = PayrollPeriodEndDateTime.AddSeconds((long)lastObject.PayrollPeriodEndDate).ToLocalTime();

                    int result = DbLink.usp_InsertSupervisorNotificationBySubscriptionID(Guid.Parse(SubscriptionID), LastRan);
                    DbLink.SaveChanges();
                    if (result > 0)
                    {
                        mdlResult.Message = "Success";
                        mdlResult.MessageReason = "Supervisor assignments pushed successfully for -" + SubscriptionID;
                        mdlResult.IsSuccess = true;
                        return mdlResult;
                    }
                    else if (result == 0)
                    {
                        mdlResult.Message = "Success";
                        mdlResult.MessageReason = "Supervisor assignment notification service ran successfully for -" + SubscriptionID;
                        mdlResult.IsSuccess = true;
                        return mdlResult;
                    }
                    else
                    {
                        mdlResult.Message = "UnSuccessfull";
                        mdlResult.MessageReason = "Supervisor assignments did not get pushed. Something went wrong! for -" + SubscriptionID;
                        mdlResult.IsSuccess = false;
                        return mdlResult;
                    }

                }
                else
                {
                    mdlResult.Message = "Success";
                    mdlResult.MessageReason = "Supervisor assignments are not monitored for - " + SubscriptionID;
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
        public ResultModel InsertSubordinatNotification(string SubscriptionID)
        {
            try
            {
                Properties.Settings.Default["SubscriptionID"] = SubscriptionID;
                Properties.Settings.Default.Save();
                //OperationContext.Current.OutgoingMessageProperties.Add("SubscriptionID", SubscriptionID);

                bool isMonitored = (bool)(from def in DbLink.Definitions where def.CustomListValue.Code == "SUBORDADD_EVNTMNTR" select def.IsMonitored).Single();
                ResultModel mdlResult = new ResultModel();
                if (isMonitored)
                {
                    var LastRan = (long)Properties.Settings.Default["LastRan"];
                    //System.DateTime PayrollPeriodEndDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
                    //PayrollPeriodEndDateTime = PayrollPeriodEndDateTime.AddSeconds((long)lastObject.PayrollPeriodEndDate).ToLocalTime();

                    int result = DbLink.usp_InsertSubordinateNotificationBySubscriptionID(Guid.Parse(SubscriptionID), LastRan);
                    DbLink.SaveChanges();
                    if (result > 0)
                    {
                        mdlResult.Message = "Success";
                        mdlResult.MessageReason = "Supervisor assignments pushed successfully for -" + SubscriptionID;
                        mdlResult.IsSuccess = true;
                        return mdlResult;
                    }
                    else if (result == 0)
                    {
                        mdlResult.Message = "Success";
                        mdlResult.MessageReason = "Supervisor assignment notification service ran successfully for -" + SubscriptionID;
                        mdlResult.IsSuccess = true;
                        return mdlResult;
                    }
                    else
                    {
                        mdlResult.Message = "UnSuccessfull";
                        mdlResult.MessageReason = "Supervisor assignments did not get pushed. Something went wrong! for -" + SubscriptionID;
                        mdlResult.IsSuccess = false;
                        return mdlResult;
                    }

                }
                else
                {
                    mdlResult.Message = "Success";
                    mdlResult.MessageReason = "Supervisor assignments are not monitored for - " + SubscriptionID;
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
        public static void updateLastRanDAL()
        {
            Properties.Settings.Default["LastRan"] = (long)(DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalSeconds;
            Properties.Settings.Default.Save();
        }
    }


}
