using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using Aloha.Notification.Models;
using AlohaNotificationBAL;

namespace AnohaNotificationService
{
    public partial class Scheduler : ServiceBase
    {
        System.Timers.Timer _timer;
        DateTime _scheduleTime;
        DateTime _lastRun;

        public Scheduler()
        {
            InitializeComponent();
            _timer = new System.Timers.Timer();
            _scheduleTime = DateTime.Today.AddHours(9).AddMinutes(48);

            /*_timer = new System.Timers.Timer();
            _scheduleTime = DateTime.Today.AddDays(1).AddHours(7);
            */

        }

        public void OnDebug()
        {
            //DataTable dt = new GenerateNotificationBAL().TestAdoNet();
            //List<ResultModel> lstNotificationsGenerated = new GenerateNotificationBAL().GenerateNotifications();
            OnStart(null);
        }

        protected override void OnStart(string[] args)
        {
            /*_timer.Enabled = true;
            _timer.Interval = _scheduleTime.Subtract(DateTime.Now).TotalSeconds * 1000;  
            */
            //List<ResultModel> lstNotificationsGenerated = new GenerateNotificationBAL().GenerateNotifications();
            //ActionLogger.WriteLog(lstNotificationsGenerated);
            File.Create(AppDomain.CurrentDomain.BaseDirectory + "onStart.txt");
            _timer = new System.Timers.Timer(1 * 60 * 1000); // every 10 minutes??
            _timer.Elapsed += new System.Timers.ElapsedEventHandler(Timer_Elapsed);
            _timer.Start();
            //List<ResultModel> lstNotificationsGenerated = new GenerateNotificationBAL().GenerateNotifications();
            //ActionLogger.WriteLog(lstNotificationsGenerated);
        }

        protected void Timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            //DateTime startAt = DateTime.Today.AddHours(9).AddMinutes(48);
            DateTime startAt = DateTime.Today.AddHours(11).AddMinutes(48);
            //DateTime startAt = DateTime.Now;
            if (_lastRun < startAt && DateTime.Now >= startAt)
            {
                // stop the timer 
                _timer.Stop();

                try
                {
                    List<ResultModel> lstNotificationsGenerated = new GenerateNotificationBAL().GenerateNotifications();
                    ActionLogger.WriteLog(lstNotificationsGenerated);
                }
                catch (Exception ex)
                {

                }

                _lastRun = DateTime.Now;
                _timer.Start();


                /*
                //If tick for the first time, reset next run to every 24 hours
                if (_timer.Interval != 24 * 60 * 60 * 1000)
                {
                    _timer.Interval = 24 * 60 * 60 * 1000;
                }
                */
            }
        }
        protected override void OnStop()
        {
            File.Create(AppDomain.CurrentDomain.BaseDirectory + "onStop.txt");
        }
    }
}
