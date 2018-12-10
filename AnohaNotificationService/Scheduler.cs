using Aloha.Notification.Models;
using AlohaNotificationBAL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace AnohaNotificationService
{
    public partial class Scheduler : ServiceBase
    {
        Timer ManyTimesADayTimer = new Timer();
        Timer OnceADayTimer;
        private string timeString;

        public Scheduler()
        {
            InitializeComponent();
            //Initializing the OnceADayTimer timers
            OnceADayTimer = new System.Timers.Timer();
            double inter = (double)GetNextInterval();
            OnceADayTimer.Interval = inter;
            OnceADayTimer.Elapsed += new ElapsedEventHandler(ServiceTimer_Tick); // adding Event

        }

        public void OnDebug()
        {
            OnStart(null);
        }

        protected override void OnStart(string[] args)
        {
#if DEBUG
            List<ResultModel> lstNotificationsGenerated = new GenerateNotificationBAL().GenerateNotifications();
            ActionLogger.WriteLog(lstNotificationsGenerated);
            new GenerateNotificationBAL().UpdateLastRan();

            List<ResultModel> lstNotificationsGenerate1 = new GenerateNotificationBAL().GenerateNotificationsOnceADay();
            ActionLogger.WriteLog(lstNotificationsGenerate1);

#else
            File.Create(AppDomain.CurrentDomain.BaseDirectory + "onStart.txt");

            OnceADayTimer.AutoReset = true;
            OnceADayTimer.Enabled = true;

            //Initializing ManyTimesADayTimer 
            ManyTimesADayTimer.Elapsed += new ElapsedEventHandler(Timer_Elapsed); // adding Event
            ManyTimesADayTimer.Interval = 600000;//120000; //60000;//Runs for every given intervel 
            ManyTimesADayTimer.Enabled = true;
            ManyTimesADayTimer.Start();
#endif
        }

        /// <summary>
        /// Executing the many times a day service
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                List<ResultModel> lstNotificationsGenerated = new GenerateNotificationBAL().GenerateNotifications();
                ActionLogger.WriteLog(lstNotificationsGenerated);
                new GenerateNotificationBAL().UpdateLastRan();
            }
            catch (Exception ex)
            {
                ActionLogger.WriteErrorLog(ex);
            }
        }

#region Once a day implementation 
        /// <summary>
        /// Gives the time intervel for next execution
        /// </summary>
        /// <returns>Time in milliSec</returns>
        private double GetNextInterval()
        {
            //Getting the time when to start execution
            timeString = ConfigurationManager.AppSettings["StartTime"];
            DateTime t = DateTime.Parse(timeString);
            TimeSpan ts = new TimeSpan();
            ts = t - System.DateTime.Now;
            if (ts.TotalMilliseconds < 0)
            {
                ts = t.AddDays(1) - System.DateTime.Now;  
            }
            return ts.TotalMilliseconds;
        }

        /// <summary>
        /// Setting the timer for next execution
        /// </summary>
        private void SetTimer()
        {
            try
            {
                double inter = (double)GetNextInterval();
                OnceADayTimer.Interval = inter;
                OnceADayTimer.Start();
            }
            catch (Exception ex)
            {
                ActionLogger.WriteErrorLog(ex);
            }
        }

        /// <summary>
        /// Executing once a day service 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ServiceTimer_Tick(object sender, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                OnceADayTimer.Stop();
                List<ResultModel> lstNotificationsGenerated = new GenerateNotificationBAL().GenerateNotificationsOnceADay();
                ActionLogger.WriteLog(lstNotificationsGenerated);
                SetTimer();
            }
            catch(Exception ex)
            {
                ActionLogger.WriteErrorLog(ex);
            }
        }
#endregion

        protected override void OnStop()
        {
            OnceADayTimer.AutoReset = false;
            OnceADayTimer.Enabled = false;

            ManyTimesADayTimer.Enabled = false;
            File.Create(AppDomain.CurrentDomain.BaseDirectory + "onStop.txt");
        }

    }

}