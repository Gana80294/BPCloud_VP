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
using System.Timers;

namespace BPCloud_VP.AttachmentMigrationService
{
    public partial class Service1 : ServiceBase
    {

        public Service1()
        {
            InitializeComponent();

        }

        private Timer timer;

        protected override void OnStart(string[] args)
        {
            try
            {
                int interval = Convert.ToInt32(ConfigurationManager.AppSettings["MigrationIntervalMinutes"]) * 1000;
                this.timer = new Timer(interval);
                this.timer.Elapsed += new ElapsedEventHandler(this.OnTimerElapsed);
                this.timer.Start();
                WriteLog.WriteToFile("Attachment Migration Service started");
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("OnStart:-" + ex.Message);
            }
        }

        private void OnTimerElapsed(object sender, ElapsedEventArgs e)
        {
            try
            {
                
                Migration.StartMigration();
                WriteLog.WriteToFile("Attachment Migration Service executed at " + e.SignalTime.ToString());
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("OnTimerElapsed:-" + ex.Message);
            }
        }

        protected override void OnStop()
        {
            try
            {
                if (this.timer != null)
                {
                    this.timer.Stop();
                    this.timer.Dispose();
                }
                WriteLog.WriteToFile("Attachment Migration Service stopped");
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("OnStop:-" + ex.Message);
            }
        }

    }
}
