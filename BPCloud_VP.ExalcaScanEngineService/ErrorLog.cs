using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BPCloud_VP.ExalcaScanEngineService
{
    public class ErrorLog
    {
        private static readonly object ErrorLogLock = new object();

        public static void WriteErrorLog(string Message)
        {
            try
            {
                lock (ErrorLog.ErrorLogLock)
                {
                    string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ErrorFiles");
                    if (!Directory.Exists(path))
                        Directory.CreateDirectory(path);
                    DateTime today = DateTime.Today;
                    string str1 = DateTime.Today.AddDays(-160.0).ToString("yyyyMMdd");
                    string str2 = today.ToString("yyyyMMdd") + ".txt";
                    if (File.Exists(AppDomain.CurrentDomain.BaseDirectory + "\\ErrorFiles\\Log_" + str1 + ".txt"))
                    {
                        GC.Collect();
                        GC.WaitForPendingFinalizers();
                        File.Delete(AppDomain.CurrentDomain.BaseDirectory + "\\ErrorFiles\\Log_" + str1 + ".txt");
                    }
                    StreamWriter streamWriter = new StreamWriter(AppDomain.CurrentDomain.BaseDirectory + "\\ErrorFiles\\Log_" + str2, true);
                    streamWriter.WriteLine(string.Format(DateTime.Now.ToString()) + ":" + Message);
                    streamWriter.Flush();
                    streamWriter.Close();
                }
            }
            catch
            {
            }
        }
    }
}
