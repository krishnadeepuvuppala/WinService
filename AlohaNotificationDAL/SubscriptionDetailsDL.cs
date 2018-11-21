using Aloha.Notification.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlohaNotificationDAL
{
    public class SubscriptionDetailsDL
    {
        /// <summary>
        /// Returns subscription details form global db
        /// </summary>
        /// <returns>List</returns>
        public List<SubscriberDetailsModel> GetSubscribersList()
        {
            var subscribersList = new List<SubscriberDetailsModel>();
            var connection = GetGlobalDbConnection();
            connection.Open();

            string sqlquery = "SELECT [Extent1].[Subscription_Id],[Extent1].[SubscriberDBName],[Extent1].[SubscriberActivityDBName],[Extent1].[SubscriberServer],[Extent1].[SubscriberActivityServer] FROM [SubscriberUrlMapping] AS [Extent1] ";

            SqlCommand query = connection.CreateCommand();
            query.CommandText = sqlquery;

            using (SqlDataReader dr = query.ExecuteReader())
            {
                while (dr.Read())
                {
                    var temp = new SubscriberDetailsModel();

                    temp.SubscriptionId = dr.GetGuid(0);
                    temp.UserDb = dr.GetString(1);
                    temp.ActivityDb = dr.GetString(2);
                    temp.UserDbServer = dr.GetString(3);
                    temp.ActivityDbServer = dr.GetString(4);
                    subscribersList.Add(temp);
                }
            }
            connection.Close();
            return subscribersList;

        }
        #region GetSubscriptionDetails
        /// <summary>
        /// Returns global connection string.
        /// </summary>
        /// <returns>SqlConnection</returns>
        public static SqlConnection GetGlobalDbConnection()
        {
            SqlConnection conn = new SqlConnection();
            //var temp = System.Configuration.ConfigurationSettings.AppSettings["GlobalConnectionString"];
            var temp = "data source=104.238.74.145;initial catalog=ALOHA_V1_Global; persist security info=True;user id=aloha_appuser;password=t0pn0tch0726;MultipleActiveResultSets=True;App=EntityFramework";
            conn.ConnectionString = temp + "&quot;";
            return conn;
        }

        #endregion
    }
}
