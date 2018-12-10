using Aloha.Notification.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnohaNotificationService
{
    public class ActionLogger
    {
        /// <summary>  
        /// This function write log to LogFile.text when some error occurs.  
        /// </summary>  
        /// <param name="ex"></param> 
        public static void WriteErrorLog(Exception ex)
        {
            StreamWriter sw = null;
            try
            {
                sw = new StreamWriter(AppDomain.CurrentDomain.BaseDirectory + "\\LogFile.txt", true);
                sw.WriteLine(DateTime.Now.ToString() + ": " + ex.Source.ToString().Trim() + "; " + ex.Message.ToString().Trim());
                sw.Flush();
                sw.Close();
            }
            catch
            {
            }
        }

        /// <summary>  
        /// This function will log Actions to LogFile.text  
        /// </summary>  
        /// <param name="Message"></param>  
        public static void WriteLog(List<ResultModel> lstResusltList)
        {
            StreamWriter SW = null;
            try
            {
                TimeSpan ts = new TimeSpan(12, 50, 0);
                SW = new StreamWriter(AppDomain.CurrentDomain.BaseDirectory + "\\LogFile.txt", true);
                SW.Write("\r\n\n");
                SW.WriteLine(DateTime.Now.ToString());
                SW.Write("\r\n\n");
                foreach (ResultModel rm in lstResusltList)
                {
                    SW.WriteLine(rm.MessageReason);
                    SW.Write("\r\n");
                }
                SW.Flush();
                SW.Close();
            }
            catch
            {
            }
        }
    }
}
