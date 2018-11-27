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
        /// this function write Message to log file.  
        /// </summary>  
        /// <param name="Message"></param>  
        public static void WriteLog(List<ResultModel> lstResusltList)
        {
            StreamWriter SW = null;
            try
            {
                SW = new StreamWriter(AppDomain.CurrentDomain.BaseDirectory + "\\LogFile"+ DateTime.Now.ToString() + ".txt", true);
                SW.Write("\r\n\n");
                SW.WriteLine(DateTime.Now.ToString());
                SW.Write("\r\n\n");
                foreach (ResultModel rm in lstResusltList)
                {
                    SW.WriteLine(rm.MessageReason);
                    SW.Write("\n");
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
