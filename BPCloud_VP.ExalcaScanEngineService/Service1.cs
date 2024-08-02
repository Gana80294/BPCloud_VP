using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace BPCloud_VP.ExalcaScanEngineService
{
    public partial class Service1 : ServiceBase
    {
        private static readonly object SpinLock = new object();
        public static List<string> FolderLocations = new List<string>();
        public static List<string> PlantCodes = new List<string>();
        public static Dictionary<string, string> dictionary = new Dictionary<string, string>();
        public static Random RandomNumber;
        public static string BackOCRFile;
        static string ftpfilename;
        private Timer timer = (Timer)null;
        //private IContainer components = (IContainer)null;

        public static string BackFolder { get; private set; }

        //public Service1() => this.InitializeComponent();

        protected override void OnStart(string[] args)
        {
            try
            {
                int num = Convert.ToInt32(ConfigurationManager.AppSettings["IntervalMinutes"]) * 1000;
                this.timer = new Timer();
                this.timer.Interval = (double)num;
                this.timer.Elapsed += new ElapsedEventHandler(this.timer_Tick);
                this.timer.Enabled = true;
            }
            catch (Exception ex)
            {
                ErrorLog.WriteErrorLog("OnStart:-" + ex.Message);
            }
        }

        protected override void OnStop()
        {
            try
            {
                this.timer.Enabled = false;
                ErrorLog.WriteErrorLog("ScanEngine Service stopped");
            }
            catch (Exception ex)
            {
                ErrorLog.WriteErrorLog("OnStop:-" + ex.Message);
            }
        }

        private void timer_Tick(object sender, ElapsedEventArgs e)
        {
            try
            {
                Service1.ScanEngine();
                ErrorLog.WriteErrorLog("ScanEngine Service started at " + e.SignalTime.ToString());
            }
            catch (Exception ex)
            {
                ErrorLog.WriteErrorLog("timer_Tick:-" + ex.Message);
            }
        }

        public static int RunningNumber()
        {
            try
            {
                lock (Service1.RandomNumber)
                    return Service1.RandomNumber.Next();
            }
            catch (Exception ex)
            {
                ErrorLog.WriteErrorLog("RunningNumber :-" + ex.Message);
                return 4444;
            }
        }

        private static void ScanEngine()
        {
            try
            {
                lock (Service1.SpinLock)
                {
                    Service1.RandomNumber = new Random();
                    ScanCenter.CreateTempFolder();
                    DateTime.Today.ToString("yyyyMMdd");
                    DateTime.Today.AddDays(-60.0);
                    SqlCommand selectCommand = new SqlCommand("SELECT * FROM [dbo].[KeyValues] WHERE IsActive=1", new SqlConnection(ConfigurationManager.ConnectionStrings["AuthContext"].ToString()));
                    DataTable dataTable = new DataTable();
                    using (SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(selectCommand))
                        sqlDataAdapter.Fill(dataTable);
                    for (int index = 0; index < dataTable.Rows.Count; ++index)
                    {
                        Service1.BackFolder = dataTable.Rows[index]["BackFolder"].ToString();
                        if (dataTable.Rows[index]["PFolderLocation"].ToString() != null && dataTable.Rows[index]["PFolderLocation"].ToString() != "")
                            Service1.FolderLocations.Add(dataTable.Rows[index]["PFolderLocation"].ToString());
                        if (dataTable.Rows[index]["PlantCode"].ToString() == null || !(dataTable.Rows[index]["PlantCode"].ToString() != ""));
                    }
                    int result = 1;
                    int NoOfFileParallelProcess = 1;
                    if (!int.TryParse(ConfigurationManager.AppSettings["NoOfFolderParallelProcess"].ToString(), out result))
                        result = 1;
                    EnumerableRowCollection<DataRow> source1 = dataTable.AsEnumerable();
                    ParallelOptions parallelOptions1 = new ParallelOptions();
                    parallelOptions1.MaxDegreeOfParallelism = result;
                    Action<DataRow> body1 = (Action<DataRow>)(CurrentFolder =>
                    {
                        try
                        {
                            if (!int.TryParse(CurrentFolder["NoParallelProcess"].ToString(), out NoOfFileParallelProcess))
                                NoOfFileParallelProcess = 1;
                            IEnumerable<string> source2 = !(ConfigurationManager.AppSettings["FilesToRead"].ToString() == "TP") ? (!(ConfigurationManager.AppSettings["FilesToRead"].ToString() == "TPJP") ? (!(ConfigurationManager.AppSettings["FilesToRead"].ToString() == "TPP") ? Directory.EnumerateFiles(CurrentFolder["PFolderLocation"].ToString(), "*.*", SearchOption.AllDirectories).Where<string>((System.Func<string, bool>)(s => s.ToLower().EndsWith(".tiff") || s.ToLower().EndsWith(".tif") || s.ToLower().EndsWith(".pdf") || s.ToLower().EndsWith(".jpg") || s.ToLower().EndsWith(".jpeg"))) : Directory.EnumerateFiles(CurrentFolder["PFolderLocation"].ToString(), "*.*", SearchOption.AllDirectories).Where<string>((System.Func<string, bool>)(s => s.ToLower().EndsWith(".tiff") || s.ToLower().EndsWith(".tif") || s.ToLower().EndsWith(".pdf") || s.ToLower().EndsWith(".png")))) : Directory.EnumerateFiles(CurrentFolder["PFolderLocation"].ToString(), "*.*", SearchOption.AllDirectories).Where<string>((System.Func<string, bool>)(s => s.ToLower().EndsWith(".tiff") || s.ToLower().EndsWith(".tif") || s.ToLower().EndsWith(".pdf") || s.ToLower().EndsWith(".jpg") || s.ToLower().EndsWith(".png") || s.ToLower().EndsWith(".jpeg")))) : Directory.EnumerateFiles(CurrentFolder["PFolderLocation"].ToString(), "*.*", SearchOption.AllDirectories).Where<string>((System.Func<string, bool>)(s => s.ToLower().EndsWith(".tiff") || s.ToLower().EndsWith(".tif") || s.ToLower().EndsWith(".pdf")));
                            ParallelOptions parallelOptions2 = new ParallelOptions();
                            parallelOptions2.MaxDegreeOfParallelism = NoOfFileParallelProcess;
                            Action<string> body2 = (Action<string>)(file =>
                            {
                                try
                                {
                                    file = Path.ChangeExtension(file, ".pdf");
                                    ErrorLog.WriteErrorLog("Scanning.." + file);
                                    if (Path.GetExtension(file).ToLower() != ".db")
                                    {
                                        string str = "";
                                        Service1.BackOCRFile = Service1.BackFolder;
                                        if (Service1.dictionary.ContainsKey(CurrentFolder["PlantCode"].ToString()))
                                            str = Service1.dictionary[CurrentFolder["PlantCode"].ToString()];
                                        if (!File.Exists(str + "\\" + Path.GetFileName(file)))
                                        {
                                            try
                                            {
                                            }
                                            catch (Exception ex)
                                            {
                                                ErrorLog.WriteErrorLog("ScanEngine :While Taking BackUp.." + file + " Exception:- " + ex.Message);
                                            }
                                        }
                                        int RunningNumber = Service1.RunningNumber();
                                        string scanfilename = System.IO.Path.GetFileNameWithoutExtension(file);
                                        string FTPFileName = "";
                                        string[] scanfile = scanfilename.Split('_');
                                        if (scanfile.Length == 3)
                                        {
                                            ErrorLog.WriteErrorLog("only pdf File Full Path.....");
                                            FTPFileName = ftpfilename;
                                            if (System.IO.Path.GetExtension(file) == ".pdf")
                                            {
                                                string destFileName = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Inbox") + "\\" + CurrentFolder["PlantCode"].ToString() + FTPFileName + "_" + scanfile[2] + ".pdf";
                                                System.IO.File.Copy(file, destFileName);
                                                if (ScanCenter.UploadPDFFile(destFileName, file))
                                                {
                                                    ErrorLog.WriteErrorLog("file deleted successfully");
                                                    // ScanCenter.DeletePDFFile(file);
                                                }
                                            }
                                        }
                                        else
                                        {
                                            FTPFileName = DateTime.Now.ToString("yyyyMMddHHmmssfff") + "_" + RunningNumber;
                                            ftpfilename = FTPFileName;
                                            ScanCenter.GetOCRFile(file, CurrentFolder["PlantCode"].ToString(), DateTime.Now.ToString("yyyyMMddHHmmssfff") + "_" + RunningNumber.ToString());
                                            bool flag = false;
                                            if (ScanCenter.CheckIfOCRFileCreated(RunningNumber))
                                            {
                                                try
                                                {
                                                    if (!Service1.IsFileLocked(new FileInfo(file)))
                                                    {
                                                        flag = true;
                                                        ErrorLog.WriteErrorLog("ScanEngine:Deleted Form Inbox....." + file);
                                                    }
                                                    else
                                                    {
                                                        flag = false;
                                                        ErrorLog.WriteErrorLog("ScanEngine:File Is NOT Able To Delete Form Inbox....." + file);
                                                    }
                                                }
                                                catch (Exception ex)
                                                {
                                                    ErrorLog.WriteErrorLog("ScanEngine:DeletingFile " + ex.Message);
                                                }
                                            }




                                            if (flag)
                                            {
                                                if (ScanCenter.UploadFileToFTPFromRunningNumber(RunningNumber, file))
                                                {
                                                    try
                                                    {
                                                        ErrorLog.WriteErrorLog("ScanEngine:FileUploaded to FTP Successfully!!!!");
                                                        File.Delete(file);
                                                        file = Path.ChangeExtension(file, ".XML");
                                                        if (File.Exists(file))
                                                        {
                                                            File.Delete(file);
                                                        }
                                                        else
                                                        {
                                                            file = Path.ChangeExtension(file, ".xml");
                                                            File.Delete(file);
                                                        }
                                                        ErrorLog.WriteErrorLog("ScanEngine:File Deleted From Internal folder Successfully!!!!");
                                                    }
                                                    catch (Exception ex)
                                                    {
                                                        ErrorLog.WriteErrorLog("ScanEngine:Error While Deleting File......." + ex.Message);
                                                    }
                                                }
                                                else
                                                {

                                                }
                                                //ErrorLog.WriteErrorLog("ScanEngine:FileNotUploaded ");
                                            }
                                            else
                                                ErrorLog.WriteErrorLog("ScanEngine:File Not Deleted So Failed To Upload!!!");
                                        }
                                    }
                                    else
                                        ErrorLog.WriteErrorLog("Error Scanning.." + file);
                                }
                                catch (Exception ex)
                                {
                                    ErrorLog.WriteErrorLog("ScanEngine :ProcessingFile.." + file + " Exception:- " + ex.Message);
                                }
                            });
                            Parallel.ForEach<string>(source2, parallelOptions2, body2);
                        }
                        catch (Exception ex)
                        {
                            ErrorLog.WriteErrorLog("ScanEngine :ProcessingFolder.. " + CurrentFolder?.ToString() + " Exception:-" + ex.Message);
                        }
                    });
                    Parallel.ForEach<DataRow>((IEnumerable<DataRow>)source1, parallelOptions1, body1);
                    if (!(ConfigurationManager.AppSettings["UploadRemainingFilesToFtp"].ToString() == "True"))
                        return;
                    ScanCenter.UploadRemainingFilesToFtp("");
                }
            }
            catch (Exception ex)
            {
                ErrorLog.WriteErrorLog(nameof(ScanEngine) + ex.Message);
            }
        }

        public static bool IsFileLocked(FileInfo file)
        {
            FileStream fileStream = (FileStream)null;
            try
            {
                fileStream = file.Open(FileMode.Open, FileAccess.Read, FileShare.None);
            }
            catch (IOException ex)
            {
                return true;
            }
            finally
            {
                try
                {
                    fileStream?.Close();
                }
                catch (Exception ex)
                {
                    ErrorLog.WriteErrorLog("IsFileLocked : " + ex.Message);
                }
            }
            return false;
        }

        public static void TimerToDailyDirectoryDeletion()
        {
            try
            {
                ErrorLog.WriteErrorLog("TimerToDailyDirectoryDeletion:-TimerToDailyDirectoryDeletion Called!!");
                string[] strArray = ConfigurationManager.AppSettings["DailyTimeToGetSAPData"].ToString().Split(':');
                while (true)
                {
                    DateTime now = DateTime.Now;
                    DateTime dateTime = new DateTime(now.Year, now.Month, now.Day, int.Parse(strArray[0]), int.Parse(strArray[1]), int.Parse(strArray[2]));
                    TimeSpan delay;
                    if (dateTime > now)
                    {
                        delay = dateTime - now;
                    }
                    else
                    {
                        dateTime = dateTime.AddDays(1.0);
                        delay = dateTime - now;
                    }
                    Task.Delay(delay).Wait();
                    Service1.DeleteDirectory();
                }
            }
            catch (Exception ex)
            {
                ErrorLog.WriteErrorLog("TimerToDailyDirectoryDeletion:- " + ex.Message);
            }
        }

        public static void DeleteDirectory()
        {
            try
            {
                string str1 = "";
                List<string> stringList = new List<string>();
                SqlCommand selectCommand = new SqlCommand("SELECT * FROM [dbo].[KeyValues] WHERE IsActive=1", new SqlConnection(ConfigurationManager.ConnectionStrings["AuthContext"].ToString()));
                DataTable dataTable = new DataTable();
                using (SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(selectCommand))
                    sqlDataAdapter.Fill(dataTable);
                for (int index = 0; index < dataTable.Rows.Count; ++index)
                {
                    str1 = dataTable.Rows[index]["BackFolder"].ToString();
                    stringList.Add(dataTable.Rows[index]["PlantCode"].ToString());
                }
                foreach (string str2 in stringList)
                {
                    string str3 = str1;
                    string format = "yyyyMMdd";
                    DirectoryInfo[] directories = new DirectoryInfo(str3 + "\\" + str2).GetDirectories();
                    if (directories != null && directories.Length != 0)
                    {
                        for (int index = directories.Length - 1; index >= 0; --index)
                        {
                            DateTime result;
                            if (DateTime.TryParseExact(directories[index].Name.Trim(), format, (IFormatProvider)CultureInfo.InvariantCulture, DateTimeStyles.None, out result) && DateTime.Today.Subtract(result).TotalDays > 30.0)
                            {
                                directories[index].Delete(true);
                                ErrorLog.WriteErrorLog("DeleteDirectory:- Deleted Directory is" + directories[index]?.ToString());
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLog.WriteErrorLog("DeleteDirectory:- " + ex.Message);
            }
        }

        //protected override void Dispose(bool disposing)
        //{
        //    if (disposing && this.component != null)

        //        this.component.Dispose();
        //    base.Dispose(disposing);
        //}

        //private void InitializeComponent() => this.ServiceName = nameof(Service1);
    }
}
