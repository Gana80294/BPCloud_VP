using BPCloud_VP_POService.DBContexts;
using BPCloud_VP_POService.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BPCloud_VP_POService.Repositories
{
    public class AttachmentRepository : IAttachmentRepository
    {
        private readonly POContext _dbContext;
        private IConfiguration _configuration;

        public AttachmentRepository(POContext dbContext, IConfiguration configuration)
        {
            _dbContext = dbContext;
            _configuration = configuration;
        }
        public async Task<BPCFLIPAttachment> AddAttachment(BPCFLIPAttachment BPCFLIPAttachment)
        {
            try
            {
                BPCFLIPAttachment.IsActive = true;
                BPCFLIPAttachment.CreatedOn = DateTime.Now;
                var result = _dbContext.BPCFLIPAttachments.Add(BPCFLIPAttachment);
                await _dbContext.SaveChangesAsync();
                return BPCFLIPAttachment;
            }
            catch (SqlException ex) { WriteLog.WriteToFile("AttachmentRepository/AddAttachment", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("AttachmentRepository/AddAttachment", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<BPCFLIPAttachment> UpdateAttachment(BPCFLIPAttachment BPCFLIPAttachment)
        {
            try
            {
                BPCFLIPAttachment bPAttachment = _dbContext.BPCFLIPAttachments.FirstOrDefault(x => x.FLIPID == BPCFLIPAttachment.FLIPID && x.AttachmentName == BPCFLIPAttachment.AttachmentName);
                if (bPAttachment != null)
                {
                    bPAttachment.AttachmentName = BPCFLIPAttachment.AttachmentName;
                    bPAttachment.AttachmentFile = BPCFLIPAttachment.AttachmentFile;
                    bPAttachment.ContentLength = BPCFLIPAttachment.ContentLength;
                    bPAttachment.ContentType = BPCFLIPAttachment.ContentType;
                    bPAttachment.IsActive = true;
                    bPAttachment.CreatedOn = DateTime.Now;
                    await _dbContext.SaveChangesAsync();
                    var header = _dbContext.BPCFLIPHeaders.Where(x => x.FLIPID == bPAttachment.FLIPID && x.InvoiceAttachmentName == bPAttachment.AttachmentName).FirstOrDefault();
                    if (header != null)
                    {
                        SaveFlipInvAttachment(header, bPAttachment.AttachmentName, bPAttachment.AttachmentFile);
                        string writerFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Outbox");
                        var uploadStatus = UploadFileToVendorOutputFolder(writerFolder, "FTPFiles");

                        if (uploadStatus == true)
                        {
                            WriteLog.WriteToFile("AttachmentRepository/SendAllAttachmentsToFTP" + " UploadFileToVendorOutputFolder Success");
                        }
                        else
                        {
                            WriteLog.WriteToFile("AttachmentRepository/SendAllAttachmentsToFTP" + " UploadFileToVendorOutputFolder Failure");
                        }
                    }
                    //var result = SendAllAttachmentsToFTP(BPCFLIPAttachment);

                }
                return null;
            }
            catch (SqlException ex) { WriteLog.WriteToFile("AttachmentRepository/UpdateAttachment", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("AttachmentRepository/UpdateAttachment", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public bool SaveFlipInvAttachment(BPCFLIPHeader header, string AttachmentName, byte[] AttachmentContent)
        {
            try
            {
                //WriteLog.WriteErrorLog("Enter the GetPRWithVendorDetails method");
                CreateOutboxTempFolder();
                string writerFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Outbox");
                var FileName = header.Company + DateTime.Now.ToString("ddMMyyyyhh") + "_" + Regex.Replace(header.InvoiceNumber.Trim(), "[^A-Za-z0-9_. ]+", "") + Path.GetExtension(AttachmentName);
                //var FileName = header.Company + DateTime.Now.ToString("ddMMyyyyhh") + "_" + header.InvoiceNumber + Path.GetExtension(AttachmentName);
                string writerPath = Path.Combine(writerFolder, FileName);
                File.WriteAllBytes(writerPath, AttachmentContent);
                return true;
            }
            catch (SqlException ex) { WriteLog.WriteToFile("AttachmentRepository/SaveFlipInvAttachment", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("AttachmentRepository/SaveFlipInvAttachment", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("AttachmentRepository/SaveFlipInvAttachment/Exception : - " + ex.Message);
                return false;
            }

        }
        public bool SendAllAttachmentsToFTP(BPCFLIPAttachment BPCFLIPAttachment)
        {
            try
            {
                bool status = false;
                WriteLog.WriteToFile("AttachmentRepository/SendAllAttachmentsToFTP" + "------AttachmentFtp method started------");
                CreateVendorTempFolder();
                Random r = new Random();
                int num = r.Next(1, 9999999);
                string writerFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "VendorTempFolder");
                var Attachment = GetFlipAttachmentByID(BPCFLIPAttachment.FLIPID, BPCFLIPAttachment.AttachmentName);

                if (Attachment != null)
                {
                    var FileName = Attachment.ContentType + "_" + Attachment.AttachmentName;
                    var FileFullPath = Path.Combine(writerFolder, FileName);
                    System.IO.File.WriteAllBytes(FileFullPath, Attachment.AttachmentFile);
                    WriteLog.WriteToFile("AttachmentRepository/SendAllAttachmentsToFTP" + $"------File {FileName} added in VendorTempFolder------");
                }
                else
                {
                    WriteLog.WriteToFile($"File { Attachment.AttachmentName} doesn't have any content");
                }

                WriteLog.WriteToFile("AttachmentRepository/SendAllAttachmentsToFTP" + " FTP File upload about to start");

                var uploadStatus = UploadFileToVendorOutputFolder(writerFolder, "FTPFiles");

                if (uploadStatus == true)
                {
                    status = true;
                    WriteLog.WriteToFile("AttachmentRepository/SendAllAttachmentsToFTP" + " UploadFileToVendorOutputFolder Success");
                }
                else
                {
                    status = false;
                    WriteLog.WriteToFile("AttachmentRepository/SendAllAttachmentsToFTP" + " UploadFileToVendorOutputFolder Failure");
                }
                return status;
            }
            catch (SqlException ex) { WriteLog.WriteToFile("AttachmentRepository/SendAllAttachmentsToFTP", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("AttachmentRepository/SendAllAttachmentsToFTP", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("AttachmentRepository/attachmentFtp/Exception", ex);
                return false;
            }
        }
        public bool UploadFileToVendorOutputFolder(string filePath, string fileName)
        {
            bool status = false;
            try
            {
                IConfiguration FTPDetailsConfig = _configuration.GetSection("FTPDetails");
                string FTPOutbox = FTPDetailsConfig.GetValue<string>("Outbox");
                string FTPUsername = FTPDetailsConfig.GetValue<string>("Username");
                string FTPPassword = FTPDetailsConfig.GetValue<string>("Password");
                using (WebClient client = new WebClient())
                {
                    if (Directory.GetFiles(filePath).Length > 0) //if file found in folder
                    {
                        DirectoryInfo dir = new DirectoryInfo(filePath);
                        FileInfo[] files = dir.GetFiles();
                        foreach (var file in files)
                        {
                            if (file.Length > 0)
                            {
                                client.Credentials = new NetworkCredential(FTPUsername, FTPPassword);
                                byte[] responseArray = client.UploadFile(FTPOutbox + file.Name, file.FullName);
                                WriteLog.WriteToFile("AttachmentRepository/UploadFileToVendorOutputFolder" + " File uploaded to Vendor Output folder");
                                status = true;
                                WriteLog.WriteToFile("AttachmentRepository/UploadFileToVendorOutputFolder" + string.Format("File {0} was successfully uploaded to FTP {1}", file.Name, FTPOutbox));
                                System.IO.File.Delete(file.FullName);
                                //return status;
                            }
                            else
                            {
                                status = false;
                                WriteLog.WriteToFile("AttachmentRepository/UploadFileToVendorOutputFolder" + string.Format("File {0} has no contents", file.FullName));
                            }
                        }
                    }
                }
                return status;
            }

            catch (SqlException ex) { WriteLog.WriteToFile("AttachmentRepository/UploadFileToVendorOutputFolder", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("AttachmentRepository/UploadFileToVendorOutputFolder", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("AttachmentRepository/UploadFileToVendorOutputFolder/Exception", ex);
                return false;
            }
        }
        public void CreateVendorTempFolder()
        {
            try
            {
                string path1 = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "VendorTempFolder");
                if (!Directory.Exists(path1))
                {
                    Directory.CreateDirectory(path1);
                }
                else
                {
                    if (Directory.GetFiles(path1).Length > 0) //if file found in folder
                    {
                        string[] txtList = Directory.GetFiles(path1, "*.xml");
                        foreach (string f in txtList)
                        {
                            System.GC.Collect();
                            System.GC.WaitForPendingFinalizers();
                            System.IO.File.Delete(f);
                        }
                    }
                }
            }
            catch (SqlException ex) { WriteLog.WriteToFile("AttachmentRepository/CreateVendorTempFolder", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("AttachmentRepository/CreateVendorTempFolder", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("Registration/CreateVendorTempFolder/Exception", ex);
            }
        }
        public static void CreateOutboxTempFolder()
        {
            try
            {
                //if folder - "\bin\debug\Internal_Folder" not exist,create folder "Internal_Folder" in debug
                string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Outbox");
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                else
                {
                    if (Directory.GetFiles(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Outbox")).Length > 0) //if file found in folder
                    {
                        string[] txtList = Directory.GetFiles(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Outbox"));
                        foreach (string f in txtList)
                        {
                            FileInfo fi = new FileInfo(f);
                            if (fi.CreationTime < DateTime.Now.AddMinutes(-30))
                            {
                                System.GC.Collect();
                                System.GC.WaitForPendingFinalizers();
                                fi.Delete();
                                //File.Delete(f);
                            }
                        }
                    }
                }

            }
            catch (SqlException ex) { WriteLog.WriteToFile("AttachmentRepository/CreateOutboxTempFolder", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("AttachmentRepository/CreateOutboxTempFolder", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("AAttachmentRepository/CreateOutboxTempFolder : - ", ex);
            }
        }

        //public List<BPCFLIPAttachment> FilterAttachments(string ProjectName, int AppID = 0, string AppNumber = null)
        //{
        //    try
        //    {
        //        bool IsAppID = AppID > 0;
        //        bool IsAppNumber = !string.IsNullOrEmpty(AppNumber);
        //        var result = (from tb in _dbContext.BPCFLIPAttachments
        //                      where tb.PO == ProjectName && (!IsAppID || tb.AppID == AppID) &&
        //                      (!IsAppNumber || tb.AppNumber == AppNumber) && tb.IsActive
        //                      select tb).ToList();
        //        return result;
        //    }
        //    catch (SqlException ex){ WriteLog.WriteToFile("AttachmentRepository/FilterAttachments", ex); throw new Exception("Something went wrong");}
        //    catch(InvalidOperationException ex){WriteLog.WriteToFile("AttachmentRepository/FilterAttachments", ex); throw new Exception("Something went wrong"); }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        //public BPCFLIPAttachment FilterAttachment(string ProjectName, string AttchmentName, int AppID = 0, string AppNumber = null, string HeaderNumber = null)
        //{
        //    try
        //    {
        //        bool IsAppID = AppID > 0;
        //        bool IsAppNumber = !string.IsNullOrEmpty(AppNumber);
        //        bool IsHeaderNumber = !string.IsNullOrEmpty(HeaderNumber);
        //        var result = (from tb in _dbContext.BPCFLIPAttachments
        //                      where tb.ProjectName == ProjectName && tb.AttachmentName == AttchmentName && (!IsAppID || tb.AppID == AppID) &&
        //                      (!IsAppNumber || tb.AppNumber == AppNumber) && (!IsHeaderNumber || tb.HeaderNumber == HeaderNumber) && tb.IsActive
        //                      select tb).FirstOrDefault();
        //        return result;
        //    }
        //    catch (SqlException ex) { WriteLog.WriteToFile("AttachmentRepository/FilterAttachment", ex); throw new Exception("Something went wrong"); }
        //    catch (InvalidOperationException ex) { WriteLog.WriteToFile("AttachmentRepository/FilterAttachment", ex); throw new Exception("Something went wrong"); }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        public BPCFLIPAttachment GetAttachmentByName(string AttachmentName)
        {
            try
            {
                var BPCFLIPAttachment = _dbContext.BPCFLIPAttachments.FirstOrDefault(x => x.AttachmentName == AttachmentName);
                return BPCFLIPAttachment;
            }
            catch (SqlException ex) { WriteLog.WriteToFile("AttachmentRepository/GetAttachmentByName", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("AttachmentRepository/GetAttachmentByName", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public BPCFLIPAttachment GetAttachmentByID(int AttachmentID)
        {
            try
            {
                var BPCFLIPAttachment = _dbContext.BPCFLIPAttachments.FirstOrDefault(x => x.AttachmentID == AttachmentID);
                return BPCFLIPAttachment;
            }
            catch (SqlException ex) { WriteLog.WriteToFile("AttachmentRepository/GetAttachmentByID", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("AttachmentRepository/GetAttachmentByID", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public BPCFLIPAttachment GetFlipAttachmentByID(string FlipID, string AttachmentName)
        {
            try
            {
                var BPCFLIPAttachment = _dbContext.BPCFLIPAttachments.FirstOrDefault(x => x.AttachmentName == AttachmentName && x.FLIPID == FlipID);
                return BPCFLIPAttachment;
            }
            catch (SqlException ex) { WriteLog.WriteToFile("AttachmentRepository/GetFlipAttachmentByID", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("AttachmentRepository/GetFlipAttachmentByID", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
