﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace BPCloud_VP.AttachmentMigration
{
    public static class Migration
    {
        static string FTPUriValue = ConfigurationManager.AppSettings["FTP_Inbox"];
        static string FTPusrname = ConfigurationManager.AppSettings["FTP_UserName"];
        static string FTPPasswrd = ConfigurationManager.AppSettings["FTP_Password"];
        public static void StartMigration()
        {
            CopyInviocesFromSourceLocation();
        }

        public static void CopyInviocesFromSourceLocation()
        {
            try
            {
                ConfigurationManager.RefreshSection("appSettings");
                string[] files = GetFileList(FTPUriValue, FTPusrname, FTPPasswrd);
                if (files != null)
                {
                    WriteLog.WriteToFile($"{files.Length} file(s) found in FTP");
                    if (files.Length > 0)
                    {
                        System.GC.Collect();
                        System.GC.WaitForPendingFinalizers();
                        CreateTempFolder();//Internal_Folder---Keep original file for temporary use
                        foreach (string file in files)
                        {
                            Download(FTPUriValue, FTPusrname, FTPPasswrd, file);
                            bool status = UploadFileToDatabase(file);
                            if (status)
                            {
                                Delete(FTPUriValue, FTPusrname, FTPPasswrd, file);
                            }
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("CopyInviocesFromSourceLocation :" + ex);
            }
        }

        public static void CreateTempFolder()
        {
            try
            {
                //if folder - "\bin\debug\Internal_Folder" not exist,create folder "Internal_Folder" in debug
                string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Internal_Folder");
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                else
                {
                    if (Directory.GetFiles(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Internal_Folder")).Length > 0) //if file found in folder
                    {
                        string[] txtList = Directory.GetFiles(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Internal_Folder"));
                        foreach (string f in txtList)
                        {
                            System.GC.Collect();
                            System.GC.WaitForPendingFinalizers();
                            File.Delete(f);
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("CreateTempFolder:" + ex.Message);
            }
        }


        public static string[] GetFileList(string FTPUriValue, string FTPusrname, string FTPPasswrd)
        {
            StringBuilder result = new StringBuilder();
            WebResponse response = null;
            StreamReader reader = null;
            FtpWebRequest reqFTP = null;
            try
            {
                reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri(FTPUriValue));
                reqFTP.UseBinary = true;
                reqFTP.Credentials = new NetworkCredential(FTPusrname, FTPPasswrd);
                reqFTP.Method = WebRequestMethods.Ftp.ListDirectory;
                reqFTP.Proxy = null;
                reqFTP.KeepAlive = false;
                reqFTP.UsePassive = true;
                reqFTP.EnableSsl = false;
                response = reqFTP.GetResponse();
                reader = new StreamReader(response.GetResponseStream());
                string line = reader.ReadLine();
                while (line != null)
                {
                    result.Append(line);
                    result.Append("\n");
                    line = reader.ReadLine();
                }

                // to remove the trailing '\n'
                string resultedString = result.ToString();
                if (!string.IsNullOrEmpty(resultedString))
                {
                    int lastIndex = resultedString.LastIndexOf('\n');
                    if (lastIndex > -1)
                    {
                        resultedString = resultedString.Substring(0, resultedString.LastIndexOf('\n'));
                    }
                    var x = resultedString.Split('\n');
                    return resultedString.Split('\n');
                }
                return null;
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("GetFileList :" + ex);
                return null;
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
                if (response != null)
                {
                    response.Close();
                }
            }
        }

        private static void Download(string FTPUriValue, string FTPusrname, string FTPPasswrd, string file)
        {
            try
            {
                // string uri = "ftp://" + ftpServerIP + "/" + remoteDir + "/" + file;
                //Uri serverUri = new Uri(uri);
                //if (serverUri.Scheme != Uri.UriSchemeFtp)
                //{
                //    return;
                //}
                FtpWebRequest reqFTP;
                reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri(FTPUriValue + "/" + file));
                reqFTP.Credentials = new NetworkCredential(FTPusrname, FTPPasswrd);
                reqFTP.KeepAlive = false;
                reqFTP.Method = WebRequestMethods.Ftp.DownloadFile;
                reqFTP.UseBinary = true;
                reqFTP.Proxy = null;
                reqFTP.UsePassive = true;
                FtpWebResponse response = (FtpWebResponse)reqFTP.GetResponse();
                Stream responseStream = response.GetResponseStream();
                string Invoice_Store = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Internal_Folder") + "\\";
                //FileStream fs = new FileStream(Invoice_Store + file, FileMode.Create);
                //StreamReader reader = new StreamReader(responseStream);
                //String buffer = reader.ReadToEnd();
                //StreamWriter writter = new StreamWriter(fs);
                //writter.WriteLineAsync();
                //writter.Flush();
                FileStream writeStream = new FileStream(Invoice_Store + file, FileMode.Create);
                int Length = 2048;
                Byte[] buffer = new Byte[Length];
                int bytesRead = responseStream.Read(buffer, 0, Length);
                while (bytesRead > 0)
                {
                    writeStream.Write(buffer, 0, bytesRead);
                    bytesRead = responseStream.Read(buffer, 0, Length);
                }
                writeStream.Close();
                response.Close();
            }
            catch (WebException ex)
            {
                WriteLog.WriteToFile("Download :" + ex.Message);
                //MessageBox.Show(wEx.Message, "Download Error");
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("Download :" + ex.Message);
                //MessageBox.Show(ex.Message, "Download Error");
            }
        }

        private static void Delete(string FTPUriValue, string FTPusrname, string FTPPasswrd, string file)
        {
            try
            {
                WriteLog.WriteToFile($"Trying to delete {file} from FTP");
                FtpWebRequest reqFTP;
                reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri(FTPUriValue + "/" + file));
                reqFTP.Credentials = new NetworkCredential(FTPusrname, FTPPasswrd);
                reqFTP.KeepAlive = false;
                reqFTP.Method = WebRequestMethods.Ftp.DeleteFile;
                reqFTP.UseBinary = true;
                reqFTP.Proxy = null;
                reqFTP.UsePassive = true;
                FtpWebResponse response = (FtpWebResponse)reqFTP.GetResponse();
                response.Close();
                WriteLog.WriteToFile($"{file} deleted from FTP");
            }
            catch (WebException ex)
            {
                WriteLog.WriteToFile("Delete :", ex);
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("Delete :", ex);
            }
        }

        public static bool UploadFileToDatabase(string file)
        {
            bool status = false;
            try
            {
                try
                {
                    WriteLog.WriteToFile($"Trying to save {file} to the database");
                    string Invoice_Store = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Internal_Folder");
                    string filePath = Path.Combine(Invoice_Store, file);
                    using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                    {
                        BinaryReader br = new BinaryReader(fs);
                        Byte[] buffer = br.ReadBytes((Int32)fs.Length);
                        if (buffer.Length > 0)
                        {
                            string[] SplitedFileName = file.Split('_');
                            if (SplitedFileName.Length > 2)
                            {
                                string connectionString = ConfigurationManager.ConnectionStrings["BPCloud_VP"].ToString();
                                SqlConnection con = new SqlConnection(connectionString);
                                string sql = "UploadBPCAttachments";
                                SqlCommand cmd = new SqlCommand(sql, con);
                                cmd.CommandType = CommandType.StoredProcedure;
                                cmd.Parameters.Add("@Client", SqlDbType.VarChar).Value = SplitedFileName[0];
                                //cmd.Parameters.Add("@Company", SqlDbType.VarChar).Value = InvoiceName;
                                //cmd.Parameters.Add("@Type", SqlDbType.VarChar).Value = InvoiceName;
                                //cmd.Parameters.Add("@PartnerID", SqlDbType.VarChar).Value = InvoiceName;
                                cmd.Parameters.Add("@ReferenceNo", SqlDbType.VarChar).Value = SplitedFileName[1];
                                //var AttachmentName = SplitedFileName.Length > 3 ? SplitedFileName[3] : (SplitedFileName[1] + "_" + SplitedFileName[2]);
                                //var AttachmentName = SplitedFileName.Length > 3 ? (SplitedFileName[2] + "_" + SplitedFileName[3]) : (SplitedFileName[1] + "_" + SplitedFileName[2]);
                                var AttachmentName = "";
                                if (SplitedFileName.Length > 3)
                                {
                                    AttachmentName = SplitedFileName[2];
                                    for (var i = 3; i < SplitedFileName.Length; i++)
                                    {
                                        AttachmentName = AttachmentName + "_" + SplitedFileName[i];
                                    }

                                }
                                else
                                {
                                    AttachmentName = SplitedFileName[1] + "_" + SplitedFileName[2];
                                }
                                cmd.Parameters.Add("@AttachmentName", SqlDbType.VarChar).Value = AttachmentName;
                                cmd.Parameters.Add("@ContentType", SqlDbType.VarChar).Value = GetFileContentType(file);
                                cmd.Parameters.Add("@ContentLength", SqlDbType.BigInt).Value = buffer.Length;
                                cmd.Parameters.Add("@AttachmentFile", SqlDbType.VarBinary).Value = buffer;
                                con.Open();

                                SqlDataReader reader = cmd.ExecuteReader();
                                try
                                {
                                    while (reader.Read())
                                    {
                                        status = true;
                                    }
                                }
                                finally
                                {
                                    // Always call Close when done reading.
                                    reader.Close();
                                }
                                cmd.Dispose();
                                con.Close();
                                WriteLog.WriteToFile(string.Format("file {0} saved in DB", file));
                                status = true;
                            }
                            else
                            {
                                WriteLog.WriteToFile(string.Format("UploadFileToDatabase : -Invalid filename {0}", file));
                            }

                        }
                        else
                        {
                            WriteLog.WriteToFile(string.Format("UploadFileToDatabase : - file {0} has no contents", file));
                        }
                    }

                    if (status)
                    {
                        GC.Collect();
                        GC.WaitForPendingFinalizers();
                        File.Delete(filePath);
                        WriteLog.WriteToFile($"{file} deleted from Internal folder");
                    }
                }
                catch (Exception ex)
                {
                    WriteLog.WriteToFile("UploadFileToDatabase :", ex);
                }
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("UploadFileToDatabase :", ex);
            }
            return status;
        }

        public static string GetFileContentType(string file)
        {
            try
            {
                string fileExtension = Path.GetExtension(file)?.ToLower();
                switch (fileExtension)
                {
                    case ("jpe"):
                        return "image/jpeg";
                    case ("jpeg"):
                        return "image/jpeg";
                    case ("jpg"):
                        return "image/jpeg";
                    case ("png"):
                        return "image/png";
                    case ("tiff"):
                        return "image/tiff";
                    case ("tif"):
                        return "image/tiff";
                    case ("pdf"):
                        return "application/pdf";
                    default:
                        return "application/octet-stream";
                }
            }
            catch (Exception ex)
            {
                return "application/octet-stream";
            }

        }
    }
}
