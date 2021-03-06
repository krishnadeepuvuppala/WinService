﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.Infrastructure.Interception;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlohaNotificationDAL
{
    public class Interceptor : IDbConnectionInterceptor
    {
        //54E720FC-616B-44C6-8485-5F2185FD7B4C
        //A3F30146-87CD-4A07-AD82-E183F42D5EED
        //F2ECAE0A-AE7C-43A2-94AC-5E7B3BAE90E2
        // public Guid? subscriptionId
        public void Opened(DbConnection connection, DbConnectionInterceptionContext interceptionContext)
        {
            //  Subscription sub = new Subscription();
            var subscriptionId = "";

            //if (HttpContext.Current != null)
            //{
            //    if (HttpContext.Current.Request.Headers.Count > 0)
            //    {
            //        subscriptionId = new Guid(HttpContext.Current.Request.Headers.GetValues("subscription_id").FirstOrDefault().ToString());

            //    }

            //}

            if (subscriptionId != null)
            {

                DbCommand cmd = connection.CreateCommand();
                var value = subscriptionId;
                cmd.CommandText = "EXEC sp_set_session_context @key=N'subscription_id', @value=@subscription_id";
                DbParameter param = cmd.CreateParameter();
                param.ParameterName = "@subscription_id";
                param.Value = value;
                //  db.Database.SqlQuery<dynamic>("EXEC sp_set_session_context @key = N'subscription_id'@value=@subscription_id", idParam);
                cmd.Parameters.Add(param);
                cmd.ExecuteNonQuery();

                DbCommand cmd2 = connection.CreateCommand();
                cmd2.CommandText = "EXEC sp_set_session_context @key=N'IsRemoved', @value=@isRemoved";
                cmd2.Parameters.Add(new SqlParameter("@isRemoved", "0"));
                cmd2.ExecuteNonQuery();



            }


            //  throw new NotImplementedException();
        }


        public void BeganTransaction(DbConnection connection, BeginTransactionInterceptionContext interceptionContext)
        {
            //throw new NotImplementedException();
        }

        public void BeginningTransaction(DbConnection connection, BeginTransactionInterceptionContext interceptionContext)
        {
            // throw new NotImplementedException();
        }

        public void Closed(DbConnection connection, DbConnectionInterceptionContext interceptionContext)
        {
            //  throw new NotImplementedException();
        }

        public void Closing(DbConnection connection, DbConnectionInterceptionContext interceptionContext)
        {
            // throw new NotImplementedException();
        }

        public void ConnectionStringGetting(DbConnection connection, DbConnectionInterceptionContext<string> interceptionContext)
        {
            // throw new NotImplementedException();
        }

        public void ConnectionStringGot(DbConnection connection, DbConnectionInterceptionContext<string> interceptionContext)
        {
            // throw new NotImplementedException();
        }

        public void ConnectionStringSet(DbConnection connection, DbConnectionPropertyInterceptionContext<string> interceptionContext)
        {
            // throw new NotImplementedException();
        }

        public void ConnectionStringSetting(DbConnection connection, DbConnectionPropertyInterceptionContext<string> interceptionContext)
        {
            //   throw new NotImplementedException();
        }

        public void ConnectionTimeoutGetting(DbConnection connection, DbConnectionInterceptionContext<int> interceptionContext)
        {
            // throw new NotImplementedException();
        }

        public void ConnectionTimeoutGot(DbConnection connection, DbConnectionInterceptionContext<int> interceptionContext)
        {
            // throw new NotImplementedException();
        }

        public void DatabaseGetting(DbConnection connection, DbConnectionInterceptionContext<string> interceptionContext)
        {
            // throw new NotImplementedException();
        }

        public void DatabaseGot(DbConnection connection, DbConnectionInterceptionContext<string> interceptionContext)
        {
            // throw new NotImplementedException();
        }

        public void DataSourceGetting(DbConnection connection, DbConnectionInterceptionContext<string> interceptionContext)
        {
            // throw new NotImplementedException();
        }

        public void DataSourceGot(DbConnection connection, DbConnectionInterceptionContext<string> interceptionContext)
        {
            // throw new NotImplementedException();
        }

        public void Disposed(DbConnection connection, DbConnectionInterceptionContext interceptionContext)
        {
            //  throw new NotImplementedException();
        }

        public void Disposing(DbConnection connection, DbConnectionInterceptionContext interceptionContext)
        {
            // throw new NotImplementedException();
        }

        public void EnlistedTransaction(DbConnection connection, EnlistTransactionInterceptionContext interceptionContext)
        {
            // throw new NotImplementedException();
        }

        public void EnlistingTransaction(DbConnection connection, EnlistTransactionInterceptionContext interceptionContext)
        {
            // throw new NotImplementedException();
        }


        public void Opening(DbConnection connection, DbConnectionInterceptionContext interceptionContext)
        {
            //  throw new NotImplementedException();
        }

        public void ServerVersionGetting(DbConnection connection, DbConnectionInterceptionContext<string> interceptionContext)
        {
            //  throw new NotImplementedException();
        }

        public void ServerVersionGot(DbConnection connection, DbConnectionInterceptionContext<string> interceptionContext)
        {
            //  throw new NotImplementedException();
        }

        public void StateGetting(DbConnection connection, DbConnectionInterceptionContext<ConnectionState> interceptionContext)
        {
            //throw new NotImplementedException();
        }

        public void StateGot(DbConnection connection, DbConnectionInterceptionContext<ConnectionState> interceptionContext)
        {
            //throw new NotImplementedException();
        }
    }
    public class initialiser : DbConfiguration
    {


        public initialiser()
        {
            this.AddInterceptor(new Interceptor());

        }
    }
}
