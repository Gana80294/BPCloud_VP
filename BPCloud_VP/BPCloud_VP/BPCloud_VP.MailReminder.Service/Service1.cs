using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BPCloud_VP.MailReminder.Service
{
    public partial class Service1 : ServiceBase
    {
        private Timer Schedular;
        private static bool Starter = false;
        public Service1()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            WriteLog.WriteToFile("Mail Reminder Service started");
            this.ScheduleService();
        }

        protected override void OnStop()
        {
            WriteLog.WriteToFile("Mail Reminder Service stopped");
        }
        public void ScheduleService() //schdule timing
        {
            try
            {
                //int intervalMinutes = 1;
                //if (Starter)
                //{
                //    ErrorLog.WriteLog("Mail Reminder Service started to check and send mails");
                //    MailReminder.StartMailReminder();
                //    string IntervalMinutes = ConfigurationManager.AppSettings["IntervalMinutes"];
                //    var res = int.TryParse(IntervalMinutes, out intervalMinutes);
                //    if (!res)
                //    {
                //        intervalMinutes = 1;
                //    }
                //}

                if (Starter)
                {
                    WriteLog.WriteToFile("Mail Reminder Service started to check and send mails");
                    MailReminder.StartMailReminder();
                }

                //ErrorLog.WriteLog("Mail Reminder Service started to check and send mails");
                //MailReminder.StartMailReminder();

                Schedular = new Timer(new TimerCallback(SchedularCallback));
                string mode = ConfigurationManager.AppSettings["Mode"].ToUpper();

                DateTime scheduledTime = DateTime.MinValue;

                if (mode.ToUpper() == "DAILY")
                {
                    //Get the Scheduled Time from AppSettings.
                    scheduledTime = DateTime.Parse(System.Configuration.ConfigurationManager.AppSettings["ScheduledTime"]);
                    if (DateTime.Now > scheduledTime)
                    {
                        //If Scheduled Time is passed set Schedule for the next day.
                        scheduledTime = scheduledTime.AddDays(1);
                    }
                }

                if (mode.ToUpper() == "INTERVAL")
                {
                    //Get the Interval in Minutes from AppSettings.
                    int intervalMinutes = Convert.ToInt32(ConfigurationManager.AppSettings["IntervalMinutes"]);

                    //Set the Scheduled Time by adding the Interval to Current Time.
                    scheduledTime = DateTime.Now.AddMinutes(intervalMinutes);
                    if (DateTime.Now > scheduledTime)
                    {
                        //If Scheduled Time is passed set Schedule for the next Interval.
                        scheduledTime = scheduledTime.AddMinutes(intervalMinutes);
                    }
                }

                ////Set the Scheduled Time by adding the Interval to Current Time.
                //scheduledTime = DateTime.Now.AddMinutes(intervalMinutes);
                //if (DateTime.Now > scheduledTime)
                //{
                //    //If Scheduled Time is passed set Schedule for the next Interval.
                //    scheduledTime = scheduledTime.AddMinutes(intervalMinutes);
                //}

                TimeSpan timeSpan = scheduledTime.Subtract(DateTime.Now);
                string schedule = string.Format("{0} day(s) {1} hour(s) {2} minute(s) {3} seconds(s)", timeSpan.Days, timeSpan.Hours, timeSpan.Minutes, timeSpan.Seconds);

                WriteLog.WriteToFile("Mail Reminder Service scheduled to run after " + schedule);
                //Get the difference in Minutes between the Scheduled and Current Time.
                int dueTime = Convert.ToInt32(timeSpan.TotalMilliseconds);

                //Change the Timer's Due Time.
                Schedular.Change(dueTime, Timeout.Infinite);
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile(ex.Message);

                //Stop the Windows Service.
                using (System.ServiceProcess.ServiceController serviceController = new System.ServiceProcess.ServiceController("SimpleService"))
                {
                    serviceController.Stop();
                }
            }
        }
        private void SchedularCallback(object e)
        {
            Starter = true;
            this.ScheduleService();
        }
    }
}
