using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using AlohaNotificationBAL;

namespace AnohaNotificationService
{
    public partial class Scheduler : ServiceBase
    {
        public Scheduler()
        {
            InitializeComponent();
        }

        public void OnDebug()
        {
            //DataTable dt = new GenerateNotificationBAL().TestAdoNet();
            bool bNotificationsGenerated = new GenerateNotificationBAL().GenerateNotifications();
        }

        protected override void OnStart(string[] args)
        {
            //GenerateNotificationBAL gnBAL = new GenerateNotificationBAL();
        }

        protected override void OnStop()
        {
        }
    }
}
