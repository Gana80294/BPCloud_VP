using Ghostscript.NET;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.parser;
using MessagingToolkit.QRCode.Codec;
using MessagingToolkit.QRCode.Codec.Data;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using Tesseract;
using ZXing;

namespace BPCloud_VP.ExalcaScanEngineService
{
    public static class ScanCenter
    {
        private static readonly object GSLock = new object();
        private static string outputPNGPath = "";
        private static string root = "";
        private static string plntCode = "";
        private static string BarcodeValue = "";
        private static bool IsXmlTrans = false;
        private static string XMLPath = "";
        private static bool qrcodefound = false;
        private static List<FindedBarcodesonPdf> Foundedbarcoesvalues = new List<FindedBarcodesonPdf>();

        public static bool GetOCRFile(string SharedFolder, string plant, string NewFileName)
        {
            ScanCenter.plntCode = plant;
            ScanCenter.root = SharedFolder;
            ScanCenter.CreBackOCRFolder(plant, SharedFolder);
            string str1 = System.IO.Path.ChangeExtension(SharedFolder, ".XML");
            if (System.IO.File.Exists(str1))
            {
                ScanCenter.CreBackOCRFolder(plant, str1);
            }
            else
            {
                string fileList = System.IO.Path.ChangeExtension(SharedFolder, ".xml");
                ScanCenter.CreBackOCRFolder(plant, fileList);
            }
            ErrorLog.WriteErrorLog("GetOCRFile :PDFFile..." + SharedFolder);
            return ScanCenter.ExtractTextFromPdf(SharedFolder, plant, NewFileName);
         
        }

        public static bool Found_Barcode(string input)
        {
            try
            {
                string pattern = ConfigurationManager.AppSettings["GRNPattern"].ToString();
                if (Regex.Matches(input, pattern).Count >= 1)
                {
                    ErrorLog.WriteErrorLog("Found_Barcode -->Found");
                    return true;
                }
                ErrorLog.WriteErrorLog("Found_Barcode --> Not Found");
                return false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public static void MergeImages(this IEnumerable<System.Drawing.Image> images, FileInfo file)
        {
            if (images == null)
                throw new ArgumentNullException(nameof(images), "Images cannot be null");
            if (file == null)
                throw new ArgumentNullException(nameof(file), "File cannot be null");
            System.Drawing.Image image1 = (System.Drawing.Image)null;
            MemoryStream memoryStream = (MemoryStream)null;
            EncoderParameters encoderParams = (EncoderParameters)null;
            try
            {
                ImageCodecInfo encoder = ((IEnumerable<ImageCodecInfo>)ImageCodecInfo.GetImageEncoders()).Where<ImageCodecInfo>((System.Func<ImageCodecInfo, bool>)(_codec => _codec.MimeType.Equals("image/tiff", StringComparison.OrdinalIgnoreCase))).FirstOrDefault<ImageCodecInfo>();
                if (encoder == null)
                    return;
                bool flag = true;
                encoderParams = new EncoderParameters(2);
                encoderParams.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.Compression, 4L);
                encoderParams.Param[1] = new EncoderParameter(System.Drawing.Imaging.Encoder.SaveFlag, 18L);
                foreach (System.Drawing.Image image2 in images)
                {
                    memoryStream = new MemoryStream();
                    image2.Save((Stream)memoryStream, System.Drawing.Imaging.ImageFormat.Tiff);
                    if (flag)
                    {
                        image1 = System.Drawing.Image.FromStream((Stream)memoryStream);
                        image1.Save(file.FullName, encoder, encoderParams);
                        flag = false;
                    }
                    else
                    {
                        encoderParams.Param[1] = new EncoderParameter(System.Drawing.Imaging.Encoder.SaveFlag, 23L);
                        image1.SaveAdd(System.Drawing.Image.FromStream((Stream)memoryStream), encoderParams);
                    }
                    memoryStream.Dispose();
                    image2.Dispose();
                }
                encoderParams.Param[1] = new EncoderParameter(System.Drawing.Imaging.Encoder.SaveFlag, 20L);
                image1.SaveAdd(encoderParams);
            }
            catch (Exception ex)
            {
                ErrorLog.WriteErrorLog("MergeImages:" + ex.Message);
            }
            finally
            {
                encoderParams.Dispose();
                memoryStream.Dispose();
                image1.Dispose();
            }
        }

        public static string CompressingTiffImages(string SourceFileName)
        {
            string filename = "";
            Stream stream = (Stream)new MemoryStream();
            Bitmap bitmap = (Bitmap)null;
            try
            {
                bitmap = new Bitmap(SourceFileName);
                ImageCodecInfo encoder = ((IEnumerable<ImageCodecInfo>)ImageCodecInfo.GetImageEncoders()).FirstOrDefault<ImageCodecInfo>((System.Func<ImageCodecInfo, bool>)(t => t.MimeType == "image/tiff"));
                System.Drawing.Imaging.Encoder compression = System.Drawing.Imaging.Encoder.Compression;
                EncoderParameters encoderParams = new EncoderParameters(1);
                EncoderParameter encoderParameter1 = new EncoderParameter(compression, 20L);
                EncoderParameter encoderParameter2 = new EncoderParameter(compression, 4L);
                encoderParams.Param[0] = encoderParameter2;
                string str = System.IO.Path.GetFileNameWithoutExtension(SourceFileName) + "_CMP" + System.IO.Path.GetExtension(SourceFileName);
                bitmap.Save(stream, encoder, encoderParams);
                filename = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Splitted_Files\\" + str);
                bitmap.Save(filename, encoder, encoderParams);
            }
            catch (Exception ex)
            {
                ErrorLog.WriteErrorLog("Compressing Tiff:" + ex.Message);
            }
            finally
            {
                try
                {
                    stream.Dispose();
                    stream.Close();
                    bitmap.Dispose();
                }
                catch (Exception ex)
                {
                    ErrorLog.WriteErrorLog("Compressing Tiff:While Disposing.." + ex.Message);
                }
            }
            return filename;
        }

        public static string GetText(Bitmap imgsource, int PageNumber)
        {
            string text = string.Empty;
            try
            {
                if (ConfigurationManager.AppSettings["TesseractVersion"].ToString() == "3.2")
                {
                    ErrorLog.WriteErrorLog("Tesseract 3.2 --> Selected!!");
                    using (TesseractEngine tesseractEngine = new TesseractEngine(System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "tessdata"), "eng", EngineMode.Default))
                    {
                        using (Pix pix = PixConverter.ToPix(imgsource))
                        {
                            using (Page page = tesseractEngine.Process(pix))
                                text = page.GetText() + " Image Quality: " + page.GetMeanConfidence().ToString();
                        }
                    }
                    ErrorLog.WriteErrorLog("Tesseract 3.2-->Success");
                }
                else
                {
                    ErrorLog.WriteErrorLog("Tesseract 3.4 --> Selected!!");
                    string[] configFiles = new string[1]
                    {
                        System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "tessdata\\configs\\config.txt")
                    };
                    using (TesseractEngine tesseractEngine = new TesseractEngine(System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "tessdata"), "eng", EngineMode.TesseractAndCube, configFiles[0]))
                    {
                        using (Pix pix = PixConverter.ToPix(imgsource))
                        {
                            using (Page page = tesseractEngine.Process(pix, new PageSegMode?(PageSegMode.Auto)))
                                text = page.GetText() + " Image Quality: " + page.GetMeanConfidence().ToString();
                        }
                    }
                    ErrorLog.WriteErrorLog("Tesseract 3.4-->Success");
                }
                string str1 = ConfigurationManager.AppSettings["GRNPattern"].ToString();
                bool flag = false;
                foreach (FindedBarcodesonPdf foundedbarcoesvalue in ScanCenter.Foundedbarcoesvalues)
                {
                    if (PageNumber == foundedbarcoesvalue.pagenumber)
                    {
                        text = "\n" + str1 + foundedbarcoesvalue.Barcode + "\n" + text;
                        flag = true;
                    }
                    else
                        flag = false;
                }
                try
                {
                    string str2 = text;
                    string[] separator = new string[3]
                    {
            "\r\n",
            "\r",
            "\n"
                    };
                    foreach (string input in str2.Split(separator, StringSplitOptions.None))
                    {
                        if (input.Contains(str1))
                        {
                            string str3 = Regex.Replace(input, "\\s+", "");
                            int startIndex = str3.IndexOf(str1);
                            str3.Substring(startIndex, 16);
                        }
                    }
                }
                catch (Exception ex)
                {
                    ErrorLog.WriteErrorLog("Tesseract-GetText():" + ex.Message);
                    return text;
                }
            }
            catch (Exception ex)
            {
                ErrorLog.WriteErrorLog("Tesseract -->Failure");
                ErrorLog.WriteErrorLog("Tesseract-GetText():" + ex.Message);
            }
            return text;
        }

        public static bool ExtractTextFromPdf(string path, string Plant, string FileName)
        {
            List<int> PageNumberGRN = new List<int>();
            List<PDFFile> pdfFileList = new List<PDFFile>();
            ConfigurationManager.AppSettings["GRNPattern"].ToString();
            int PageSize = 1;
            StringBuilder stringBuilder = new StringBuilder();
            string extension = System.IO.Path.GetExtension(path);
            string withoutExtension = System.IO.Path.GetFileNameWithoutExtension(path);
            bool flag1 = true;
            string path1 = "";
            bool flag2 = false;
            Dictionary<int, string> CurrentPDFTexts = new Dictionary<int, string>();
            XmlDocument xmlDocument = new XmlDocument();
            string directoryName = System.IO.Path.GetDirectoryName(path);
            try
            {
                using (PdfReader reader = new PdfReader(path))
                {
                    PageSize = reader.NumberOfPages;
                    for (int index = 1; index <= reader.NumberOfPages; ++index)
                    {
                        PdfTextExtractor.GetTextFromPage(reader, index);
                        ErrorLog.WriteErrorLog("Path found or not" + directoryName + "\\" + withoutExtension + ".XML");
                        if (System.IO.File.Exists(directoryName + "\\" + withoutExtension + ".XML"))
                        {
                            ErrorLog.WriteErrorLog("File xml found" + directoryName + "\\" + withoutExtension + ".XML");
                            xmlDocument.Load(directoryName + "\\" + withoutExtension + ".XML");
                            ErrorLog.WriteErrorLog("Document Loaded");
                            Plant = withoutExtension.Substring(0, 4);
                            ErrorLog.WriteErrorLog("Plant Loaded" + Plant);
                            flag2 = true;
                            ScanCenter.IsXmlTrans = true;
                            ScanCenter.XMLPath = directoryName + "\\" + withoutExtension + ".XML";
                        }
                        else if (System.IO.File.Exists(directoryName + "\\" + withoutExtension + ".xml"))
                        {
                            ErrorLog.WriteErrorLog("File xml found" + directoryName + "\\" + withoutExtension + ".xml");
                            xmlDocument.Load(directoryName + "\\" + withoutExtension + ".xml");
                            ErrorLog.WriteErrorLog("Document Loaded");
                            Plant = withoutExtension.Substring(0, 4);
                            ErrorLog.WriteErrorLog("Plant Loaded" + Plant);
                            flag2 = true;
                            ScanCenter.IsXmlTrans = true;
                            ScanCenter.XMLPath = directoryName + "\\" + withoutExtension + ".xml";
                        }
                        string str = (string)null;
                        if (string.IsNullOrEmpty(str))
                        {
                            str = ScanCenter.GhostscriptPDFToImage(path, withoutExtension + index.ToString(), index);
                            CurrentPDFTexts.Add(index, str);
                        }
                        stringBuilder.Append(str);
                        if (ScanCenter.qrcodefound)
                        {
                            ErrorLog.WriteErrorLog("Splite PageNumber adding is:" + index.ToString());
                            PageNumberGRN.Add(index);
                            flag1 = true;
                        }
                        flag1 = true;
                        ScanCenter.outputPNGPath = "";
                    }
                }
                if (PageNumberGRN.Count > 0)
                {
                    int num = 0;
                    foreach (PDFFile pdfFile in ScanCenter.GetAllSplittedPDFFile(PageNumberGRN, PageSize))
                    {
                        string outputPdfPath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Inbox") + "\\" + Plant + FileName + "_" + num.ToString();
                        ErrorLog.WriteErrorLog("ExtractTextFromPdf :Barcode Found--> " + outputPdfPath + extension);
                        if (flag1)
                            ScanCenter.ExtractPages(path, outputPdfPath, extension, pdfFile.PageStart, pdfFile.PageEnd, CurrentPDFTexts);
                        ++num;
                    }
                    try
                    {
                        if (System.IO.File.Exists(path1))
                            System.IO.File.Delete(path1);
                    }
                    catch (Exception ex)
                    {
                        ErrorLog.WriteErrorLog("ExtractTextFromPdf :WhileDeleting From Scan Folder " + ex.Message);
                    }
                }
                else
                {
                    ErrorLog.WriteErrorLog("Enters false condition");
                    string destFileName = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Inbox") + "\\" + Plant + FileName + "_0" + extension;
                    System.IO.File.Copy(path, destFileName);
                    string[] strArray = stringBuilder.ToString().Split(new string[1]
                    {
            "\n"
                    }, StringSplitOptions.RemoveEmptyEntries);
                    string contents = string.Empty;
                    foreach (string str in strArray)
                    {
                        if (!string.IsNullOrEmpty(str.Trim()))
                            contents = contents + str + Environment.NewLine;
                    }
                    if (!flag2)
                    {
                        System.IO.File.WriteAllText(System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Inbox") + "\\" + Plant + FileName + "_0.txt", contents);
                    }
                    else
                    {
                        ErrorLog.WriteErrorLog("XMl file searching");
                        ErrorLog.WriteErrorLog("");
                        XmlNode node = xmlDocument.CreateNode("element", "OCRDATA", "");
                        node.InnerText = contents;
                        xmlDocument.DocumentElement.AppendChild(node);
                        ErrorLog.WriteErrorLog("ExtractTextFromPdf:-Writing file inbox" + System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Inbox") + "\\" + Plant + FileName + "_0.XML");
                        xmlDocument.Save(System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Inbox") + "\\" + Plant + FileName + "_0.XML");
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                ErrorLog.WriteErrorLog("ExtractTextFromPdf " + ex.Message);
                return false;
            }
        }

        public static List<PDFFile> GetAllSplittedPDFFile(List<int> PageNumberGRN, int PageSize)
        {
            List<PDFFile> allSplittedPdfFile = new List<PDFFile>();
            try
            {
                foreach (int num1 in PageNumberGRN)
                {
                    int num2 = 0;
                    int num3 = 1;
                    for (int index = 0; index < PageSize && !PageNumberGRN.Contains(num1 + num3) && num1 + num3 <= PageSize; ++index)
                    {
                        num2 = num1 + num3;
                        ++num3;
                    }
                    PDFFile pdfFile = new PDFFile();
                    if (num2 == 0)
                        num2 = num1;
                    pdfFile.PageStart = num1;
                    pdfFile.PageEnd = num2;
                    allSplittedPdfFile.Add(pdfFile);
                }
                return allSplittedPdfFile;
            }
            catch (Exception ex)
            {
                ErrorLog.WriteErrorLog("GetAllSplittedPDFFile:" + ex.Message);
                return allSplittedPdfFile;
            }
        }

        public static void CreateTempFolder()
        {
            try
            {
                string path1 = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Splitted_Files");
                if (!Directory.Exists(path1))
                {
                    Directory.CreateDirectory(path1);
                }
                else
                {
                    foreach (string file in Directory.GetFiles(path1))
                    {
                        try
                        {
                            System.IO.File.Delete(file);
                        }
                        catch (Exception ex)
                        {
                            ErrorLog.WriteErrorLog("CreateTempFolder:Splitted_Files : ErrorDeletingPath.." + file + " Exception :-" + ex.Message);
                        }
                    }
                }
                string path2 = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Inbox");
                if (!Directory.Exists(path2))
                {
                    Directory.CreateDirectory(path2);
                }
                else
                {
                    foreach (string file in Directory.GetFiles(path2))
                    {
                        try
                        {
                            System.IO.File.Delete(file);
                        }
                        catch (Exception ex)
                        {
                            ErrorLog.WriteErrorLog("CreateTempFolder:Inbox:ErrorDeletingPath.." + file + " Exception :-" + ex.Message);
                        }
                    }
                }
                string path3 = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "TempFolder");
                if (!Directory.Exists(path3))
                {
                    Directory.CreateDirectory(path3);
                }
                else
                {
                    foreach (string file in Directory.GetFiles(path3))
                    {
                        try
                        {
                            System.IO.File.Delete(file);
                        }
                        catch (Exception ex)
                        {
                            ErrorLog.WriteErrorLog("CreateTempFolder:TempFolder:ErrorDeletingPath.." + file + " Exception :-" + ex.Message);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLog.WriteErrorLog("CreateTempFolder:" + ex.Message);
            }
        }

        public static void CreateBackupPlantFolder(string plant_BACKUP)
        {
            try
            {
                if (Directory.Exists(plant_BACKUP))
                    return;
                Directory.CreateDirectory(plant_BACKUP);
            }
            catch (Exception ex)
            {
                ErrorLog.WriteErrorLog("CreateBackupPlantFolder:" + ex.Message);
            }
        }

        public static void DeleteHistoryFolder(string oneWeekBefore)
        {
            try
            {
                DirectoryInfo directoryInfo = new DirectoryInfo(oneWeekBefore);
                if (!directoryInfo.Exists)
                    return;
                directoryInfo.Delete(true);
                ErrorLog.WriteErrorLog("DeleteHistoryFolder:Deleted For.." + oneWeekBefore);
            }
            catch (Exception ex)
            {
                ErrorLog.WriteErrorLog("DeleteHistoryFolder:" + ex.Message);
            }
        }

        public static void CreateBackupFolder(string path_BACKUP)
        {
            try
            {
                if (Directory.Exists(path_BACKUP))
                    return;
                Directory.CreateDirectory(path_BACKUP);
            }
            catch (Exception ex)
            {
                ErrorLog.WriteErrorLog("CreateBackupFolder:" + ex.Message);
            }
        }

        public static string GetRandomFileName(string FileName, string Plant) => "";

        public static bool CheckIfOCRFileCreated(int RunningNumber)
        {
            try
            {
                int num = 0;
                foreach (string file in Directory.GetFiles(System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Inbox")))
                    ++num;
                if (num >= 2)
                {
                    ErrorLog.WriteErrorLog("CheckIfOCRFileCreated:OCR Files Found!!!");
                    return true;
                }
                ErrorLog.WriteErrorLog("CheckIfOCRFileCreated:OCR Files Not Found!!!");
                return true;
            }
            catch (Exception ex)
            {
                ErrorLog.WriteErrorLog("CheckIfOCRFileCreated:" + ex.Message);
                return false;
            }
        }

        public static bool UploadPDFFile(string PDFFileName, string ScanfolderPdf)
        {
            ErrorLog.WriteErrorLog("UploadFileToFTPFromRunningNumber:file");
            bool uploaded = false;
            bool deleted = false;
            bool InboxDeleted = false;
            var FTPUriValue = ConfigurationManager.AppSettings["FTPUriValue"].ToString();
            var FTPusrname = ConfigurationManager.AppSettings["FTPUserName"].ToString();
            var FTPPasswrd = ConfigurationManager.AppSettings["FTPPassword"].ToString();
            using (WebClient client = new WebClient())
            {
                ErrorLog.WriteErrorLog("UploadFileToFTPFromRunningNumber:file");
                string name = System.IO.Path.GetFileName(PDFFileName);
                client.Credentials = new NetworkCredential(FTPusrname, FTPPasswrd);
                byte[] responseArray = client.UploadFile(FTPUriValue + name, PDFFileName);
                uploaded = true;
                ErrorLog.WriteErrorLog("UploadFileToFTPFromRunningNumber:file");
                //filetoback.Add(FileName);
            }

            try
            {
                if (uploaded)
                {
                    ErrorLog.WriteErrorLog("Filename " + PDFFileName);
                    File.Delete(PDFFileName);
                    InboxDeleted = true;
                }
                else
                {
                    ErrorLog.WriteErrorLog("File not deleted from inbox");
                    InboxDeleted = false;
                }
                if (InboxDeleted)
                {
                    string Filename = System.IO.Path.GetFileName(ScanfolderPdf);
                    ErrorLog.WriteErrorLog("Filename " + Filename);
                    var FTPUriValueScanfolder = "ftp://172.17.2.11:24/VendorInbox/";
                    ErrorLog.WriteErrorLog("FTPUriValueScanfolder " + FTPUriValueScanfolder);
                    FtpWebRequest reqFTP;
                    reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri(FTPUriValueScanfolder + "/" + Filename));
                    reqFTP.Credentials = new NetworkCredential(FTPusrname, FTPPasswrd);
                    reqFTP.KeepAlive = false;
                    reqFTP.Method = WebRequestMethods.Ftp.DeleteFile;
                    reqFTP.UseBinary = true;
                    reqFTP.Proxy = null;
                    reqFTP.UsePassive = false;
                    FtpWebResponse response = (FtpWebResponse)reqFTP.GetResponse();
                    ErrorLog.WriteErrorLog("Getting response  " + response);
                    response.Close();
                    deleted = true;
                }
                else
                {
                    ErrorLog.WriteErrorLog("File not deleted from Scan folder");
                }

            }
            catch (Exception wEx)
            {
                deleted = false;
                ErrorLog.WriteErrorLog("DeleteFromFTPConfigurationFolder: " + wEx);
            }
            return deleted;

        }
        public static bool UploadFileToFTPFromRunningNumber(int RunningNumber, string InFileName)
        {
            try
            {
                bool fromRunningNumber = false;
                string[] files = Directory.GetFiles(System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Inbox"));
                foreach (string str in files)
                {
                    if (System.IO.Path.GetExtension(str) == ".pdf")
                    {
                        ErrorLog.WriteErrorLog("UploadFileToFTPFromRunningNumber:file" + str);
                        ErrorLog.WriteErrorLog("UploadFileToFTPFromRunningNumber:Is XML Trnasaction" + ScanCenter.IsXmlTrans.ToString());
                        string TextFile;
                        if (ScanCenter.IsXmlTrans)
                        {
                            TextFile = System.IO.Path.GetDirectoryName(str) + "\\" + System.IO.Path.GetFileNameWithoutExtension(str) + ".XML";
                            ErrorLog.WriteErrorLog("UploadFileToFTPFromRunningNumber:Is XML path is" + TextFile);
                        }
                        else
                            TextFile = System.IO.Path.GetDirectoryName(str) + "\\" + System.IO.Path.GetFileNameWithoutExtension(str) + ".txt";
                        if (System.IO.Path.GetExtension(str) != ".txt" || System.IO.Path.GetExtension(str) != ".XML")
                        {
                            if (((IEnumerable<string>)files).Any<string>(new System.Func<string, bool>(TextFile.Contains)))
                            {
                                ErrorLog.WriteErrorLog("UploadFileToFTPFromRunningNumber:file contains" + str);
                                ErrorLog.WriteErrorLog("UploadFileToFTPFromRunningNumber:file Contains" + TextFile);
                                if (ScanCenter.UplaodFileToFTP(str, TextFile))
                                {
                                    fromRunningNumber = true;
                                    ScanCenter.StoreScanEntryInDB(InFileName, str);
                                }
                                else
                                    ErrorLog.WriteErrorLog("UploadFileToFTPFromRunningNumber:Failed To Uplaod FTP " + str);
                            }
                            else
                                ErrorLog.WriteErrorLog("UploadFileToFTPFromRunningNumber:TextFile Not Present " + TextFile);
                        }
                    }
                }
                return fromRunningNumber;
            }
            catch (Exception ex)
            {
                ErrorLog.WriteErrorLog("UploadFileToFTPFromRunningNumber: " + ex.Message);
                return false;
            }
        }

        public static void UploadRemainingFilesToFtp(string InFileName)
        {
            try
            {
                string[] files = Directory.GetFiles(System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Inbox"), "*.*");
                foreach (string str in files)
                {
                    string TextFile = System.IO.Path.GetDirectoryName(str) + "\\" + System.IO.Path.GetFileNameWithoutExtension(str) + ".txt";
                    if (System.IO.Path.GetExtension(str) != ".txt" && ((IEnumerable<string>)files).Any<string>(new System.Func<string, bool>(TextFile.Contains)) && ScanCenter.UplaodFileToFTP(str, TextFile))
                        ScanCenter.StoreScanEntryInDB(InFileName, str);
                }
            }
            catch (Exception ex)
            {
                ErrorLog.WriteErrorLog("UploadRemainingFilesToFtp: " + ex.Message);
            }
        }

        public static bool UplaodFileToFTP(string FileName, string TextFile)
        {
            try
            {
                string str = System.IO.File.ReadAllText(TextFile);
                if (str != null && str != "")
                {
                    if (ConfigurationManager.AppSettings["FinalUplaodPDF"].ToString() == "1" && System.IO.Path.GetExtension(FileName).ToLower() != ".pdf")
                    {
                        if (ImageToPDF.ConvertImageToPDF(FileName, 1000))
                        {
                            try
                            {
                                FileName = System.IO.Path.ChangeExtension(FileName, ".pdf");
                            }
                            catch (Exception ex)
                            {
                                ErrorLog.WriteErrorLog("UplaodFileToFTP:Error In Changing extension: " + ex.Message + " File :" + FileName);
                            }
                        }
                    }
                    bool flag1 = false;
                    bool flag2 = false;
                    string ftpUri = ConfigurationManager.AppSettings["FTPUriValue"].ToString();
                    string ftpUser = ConfigurationManager.AppSettings["FTPUserName"].ToString();
                    string password = ConfigurationManager.AppSettings["FTPPassword"].ToString();
                    string empty = string.Empty;
                    if (ConfigurationManager.AppSettings["GetFTPDetailsFromDB"].ToString() == "1")
                    {
                        if (FileName.Length > 5)
                        {
                            int length = int.Parse(ConfigurationManager.AppSettings["PlantCodeLength"]);
                            string Plant = System.IO.Path.GetFileNameWithoutExtension(FileName).Substring(0, length);
                            FTPDetails detailsFromPlant = ScanCenter.GetFTPDetailsFromPlant(Plant);
                            if (detailsFromPlant.FTPUri != null && detailsFromPlant.FTPUser != null && detailsFromPlant.Password != null)
                            {
                                ftpUri = detailsFromPlant.FTPUri;
                                ftpUser = detailsFromPlant.FTPUser;
                                password = detailsFromPlant.Password;
                            }
                            else
                                ErrorLog.WriteErrorLog("UplaodFileToFTP:FTP Details Not Found For PlantCode: " + Plant);
                        }
                        else
                            ErrorLog.WriteErrorLog("UplaodFileToFTP:plant Code Not Found: " + FileName);
                    }
                    if (System.IO.File.Exists(FileName))
                    {
                        ErrorLog.WriteErrorLog("UplaodFileToFTP:FileExists: " + FileName);
                        using (WebClient webClient = new WebClient())
                        {
                            string fileName = System.IO.Path.GetFileName(FileName);
                            webClient.Credentials = (ICredentials)new NetworkCredential(ftpUser, password);
                            webClient.UploadFile(ftpUri + fileName, FileName);
                            flag1 = true;
                        }
                    }
                    if (System.IO.File.Exists(TextFile))
                    {
                        ErrorLog.WriteErrorLog("UplaodFileToFTP:TextFileExists: " + TextFile);
                        using (WebClient webClient = new WebClient())
                        {
                            string fileName = System.IO.Path.GetFileName(TextFile);
                            webClient.Credentials = (ICredentials)new NetworkCredential(ftpUser, password);
                            webClient.UploadFile(ftpUri + fileName, TextFile);
                            flag2 = true;
                        }
                        if (flag1)
                        {
                            if (System.IO.File.Exists(FileName))
                            {
                                try
                                {
                                    System.IO.File.Delete(FileName);
                                    System.IO.File.Delete(TextFile);
                                }
                                catch (Exception ex)
                                {
                                    ErrorLog.WriteErrorLog("UplaodFileToFTP:DeletingFile " + ex.Message);
                                }
                            }
                        }
                        if (flag2)
                        {
                            if (System.IO.File.Exists(TextFile))
                            {
                                try
                                {
                                    System.IO.File.Delete(TextFile);
                                }
                                catch (Exception ex)
                                {
                                    ErrorLog.WriteErrorLog("UplaodFileToFTP:DeletingTextFile " + ex.Message);
                                }
                            }
                        }
                    }
                    if (flag1 & flag2)
                    {
                        ErrorLog.WriteErrorLog("Both File And TextFile Uploaded :True ");
                        return true;
                    }
                    ErrorLog.WriteErrorLog(string.Format("Both File And TextFile Uploaded to ftp {0} :False ", (object)ftpUri));
                    return false;
                }
                ErrorLog.WriteErrorLog("UplaodFileToFTP:0kb file generated ");
                return false;
            }
            catch (Exception ex)
            {
                ErrorLog.WriteErrorLog("UplaodFileToFTP " + ex.Message);
                return false;
            }
        }

        public static void StoreScanEntryInDB(string InFileName, string OutFileName)
        {
            string str1;
            try
            {
                str1 = Dns.GetHostByName(Dns.GetHostName()).AddressList[0].ToString();
            }
            catch (Exception ex)
            {
                str1 = "";
            }
            try
            {
                string fileName = System.IO.Path.GetFileName(OutFileName);
                string str2 = fileName.Substring(0, 4);
                DateTime creationTime = System.IO.File.GetCreationTime(ScanCenter.root);
                string directoryName = System.IO.Path.GetDirectoryName(InFileName);
                string connectionString = ConfigurationManager.ConnectionStrings["AuthContext"].ToString();
                string str3 = ConfigurationManager.AppSettings["FTPUriValue"].ToString();
                InFileName = System.IO.Path.GetFileName(InFileName);
                SqlConnection connection = new SqlConnection(connectionString);
                SqlCommand sqlCommand = new SqlCommand("INSERT INTO [dbo].[ScannedLogs] (ScannedHistroyId,TransID,Plant,PlantIP,InFileName,OutFileName,InFolder,OutFolder,InvoiceType,InputType,NoOfApprovers,CreatedOn,LastmodifiedOn,IsActive) VALUES(@ScannedHistroyId,@TransID,@Plant,@PlantIP,@InFileName,@OutFileName,@InFolder,@OutFolder,@InvoiceType,@InputType,@NoOfApprovers,@CreatedOn,@LastmodifiedOn,@IsActive)", connection);
                sqlCommand.Parameters.AddWithValue("@ScannedHistroyId", (object)Guid.NewGuid());
                sqlCommand.Parameters.Add("@TransID", SqlDbType.Int).Value = (object)1;
                sqlCommand.Parameters.Add("@Plant", SqlDbType.VarChar).Value = (object)str2;
                sqlCommand.Parameters.Add("@PlantIP", SqlDbType.VarChar).Value = (object)str1;
                sqlCommand.Parameters.Add("@InFileName", SqlDbType.VarChar).Value = (object)InFileName;
                sqlCommand.Parameters.Add("@OutFileName", SqlDbType.VarChar).Value = (object)fileName;
                sqlCommand.Parameters.Add("@InFolder", SqlDbType.VarChar).Value = (object)directoryName;
                sqlCommand.Parameters.Add("@OutFolder", SqlDbType.VarChar).Value = (object)str3;
                sqlCommand.Parameters.Add("@InvoiceType", SqlDbType.VarChar).Value = (object)"";
                sqlCommand.Parameters.Add("@InputType", SqlDbType.Int).Value = (object)1;
                sqlCommand.Parameters.Add("@NoOfApprovers", SqlDbType.Int).Value = (object)0;
                sqlCommand.Parameters.Add("@CreatedOn", SqlDbType.DateTime).Value = (object)DateTime.Now;
                sqlCommand.Parameters.Add("@LastmodifiedOn", SqlDbType.DateTime).Value = (object)creationTime;
                sqlCommand.Parameters.Add("@IsActive", SqlDbType.Bit).Value = (object)1;
                connection.Open();
                sqlCommand.ExecuteNonQuery();
                sqlCommand.Dispose();
                connection.Close();
            }
            catch (Exception ex)
            {
                ErrorLog.WriteErrorLog("StoreScanEntryInDB:" + ex.Message);
            }
        }

        public static void ExtractPages(
          string sourcePdfPath,
          string outputPdfPath,
        string Extension,
        int startPage,
        int endPage,
          Dictionary<int, string> CurrentPDFTexts)
        {
            PdfReader reader = (PdfReader)null;
            Document document = (Document)null;
            StringBuilder stringBuilder = new StringBuilder();
            try
            {
                reader = new PdfReader(sourcePdfPath);
                document = new Document(reader.GetPageSizeWithRotation(startPage));
                PdfCopy pdfCopy = new PdfCopy(document, (Stream)new FileStream(outputPdfPath + Extension, FileMode.Create));
                document.Open();
                for (int index = startPage; index <= endPage; ++index)
                {
                    PdfImportedPage importedPage = pdfCopy.GetImportedPage(reader, index);
                    pdfCopy.AddPage(importedPage);
                    string str = PdfTextExtractor.GetTextFromPage(reader, index);
                    if (ConfigurationManager.AppSettings["ExtractTextFromPDFItSelf"].ToString() == "0")
                        str = (string)null;
                    if (string.IsNullOrEmpty(str))
                        stringBuilder.Append(CurrentPDFTexts[index]);
                    else
                        stringBuilder.Append(str);
                }
                string[] strArray = stringBuilder.ToString().Split(new string[1]
                {
          "\n"
                }, StringSplitOptions.RemoveEmptyEntries);
                string contents = string.Empty;
                foreach (string str in strArray)
                {
                    if (!string.IsNullOrEmpty(str.Trim()))
                        contents = contents + str + Environment.NewLine;
                }
                System.IO.File.WriteAllText(outputPdfPath + ".txt", contents);
            }
            catch (Exception ex)
            {
                ErrorLog.WriteErrorLog("ExtractPages:" + ex.Message);
            }
            finally
            {
                document.Close();
                reader.Close();
            }
        }

        private static string GhostscriptPDFToImage(
          string inputFile,
          string outputFileName,
          int PageNumber)
        {
            try
            {
                int int32_1 = Convert.ToInt32(ConfigurationManager.AppSettings["GSXDPI"]);
                int int32_2 = Convert.ToInt32(ConfigurationManager.AppSettings["GSYDPI"]);
                lock (ScanCenter.GSLock)
                {
                    ScanCenter.outputPNGPath = System.IO.Path.Combine(System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "TempFolder"), string.Format("{0}.png", (object)(Guid.NewGuid().ToString() + outputFileName)));
                    GhostscriptPngDevice ghostscriptPngDevice = new GhostscriptPngDevice((GhostscriptPngDeviceType)2);
                    ((GhostscriptImageDevice)ghostscriptPngDevice).GraphicsAlphaBits = new GhostscriptImageDeviceAlphaBits?((GhostscriptImageDeviceAlphaBits)2);
                    ((GhostscriptImageDevice)ghostscriptPngDevice).TextAlphaBits = new GhostscriptImageDeviceAlphaBits?((GhostscriptImageDeviceAlphaBits)2);
                    ((GhostscriptImageDevice)ghostscriptPngDevice).ResolutionXY = new GhostscriptImageDeviceResolution(int32_1, int32_2);
                    ((GhostscriptDevice)ghostscriptPngDevice).InputFiles.Add(inputFile);
                    ((GhostscriptDevice)ghostscriptPngDevice).Pdf.FirstPage = new int?(PageNumber);
                    ((GhostscriptDevice)ghostscriptPngDevice).Pdf.LastPage = new int?(PageNumber);
                    ((GhostscriptDevice)ghostscriptPngDevice).PostScript = string.Empty;
                    ((GhostscriptDevice)ghostscriptPngDevice).CustomSwitches.Add("-dDOINTERPOLATE");
                    ((GhostscriptDevice)ghostscriptPngDevice).OutputPath = ScanCenter.outputPNGPath;
                    ((GhostscriptDevice)ghostscriptPngDevice).Process();
                }
                return ScanCenter.GetTextFromPNGFile(ScanCenter.outputPNGPath, PageNumber);
            }
            catch (Exception ex)
            {
                ErrorLog.WriteErrorLog("GhostscriptPDFToImage: " + ex.Message);
                return "";
            }
        }

        public static string GetTextFromPNGFile(string outputPNGPath, int PageNumber)
        {
            string textFromPngFile = string.Empty;
            Bitmap img = new Bitmap(outputPNGPath);
            try
            {
                if (ConfigurationManager.AppSettings["TesseractVersion"].ToString() == "3.2")
                {
                    ErrorLog.WriteErrorLog("Tesseract 3.2 --> Selected!!");
                    using (TesseractEngine tesseractEngine = new TesseractEngine(System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "tessdata"), "eng", EngineMode.Default))
                    {
                        using (Pix pix = PixConverter.ToPix(img))
                        {
                            using (Page page = tesseractEngine.Process(pix))
                                textFromPngFile = page.GetText() + " Image Quality: " + page.GetMeanConfidence().ToString();
                        }
                    }
                    ErrorLog.WriteErrorLog("GetTextFromPNGFileInPDFConversion:Tesseract 3.2-->Success");
                }
                else
                {
                    ErrorLog.WriteErrorLog("Tesseract 3.4 --> Selected!!");
                    string[] configFiles = new string[1]
                    {
            System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "tessdata\\configs\\config.txt")
                    };
                    using (TesseractEngine tesseractEngine = new TesseractEngine(System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "tessdata"), "eng", EngineMode.TesseractAndCube, configFiles[0]))
                    {
                        using (Pix pix = PixConverter.ToPix(img))
                        {
                            using (Page page = tesseractEngine.Process(pix, new PageSegMode?(PageSegMode.Auto)))
                                textFromPngFile = page.GetText() + " Image Quality: " + page.GetMeanConfidence().ToString();
                        }
                    }
                    ErrorLog.WriteErrorLog("GetTextFromPNGFileInPDFConversion:Tesseract 3.4 -->Success");
                }
            }
            catch (Exception ex)
            {
                ErrorLog.WriteErrorLog("Tesseract -->Failure");
                ErrorLog.WriteErrorLog("Tesseract-GetTextFromPNGFileInPDFConversion():" + ex.Message);
            }
            finally
            {
                try
                {
                    img.Dispose();
                }
                catch (Exception ex)
                {
                    ErrorLog.WriteErrorLog("Tesseract-GetTextFromPNGFileInPDFConversion while Disposing:" + ex.Message);
                }
            }
            try
            {
                string str1 = ConfigurationManager.AppSettings["GRNPattern"].ToString();
                Bitmap path = (Bitmap)System.Drawing.Image.FromFile(outputPNGPath);
                ScanCenter.qrcodefound = false;
                ScanCenter.qrcodefound = ScanCenter.scanQRCode(path);
                path.Dispose();
                ErrorLog.WriteErrorLog("GetTextFromPNGFileInPDFConversion:- Writing Barcode if found" + ScanCenter.BarcodeValue);
                outputPNGPath = "";
                textFromPngFile = "\n" + ScanCenter.BarcodeValue + "\n" + textFromPngFile;
                foreach (FindedBarcodesonPdf foundedbarcoesvalue in ScanCenter.Foundedbarcoesvalues)
                {
                    if (PageNumber == foundedbarcoesvalue.pagenumber)
                    {
                        string str2 = foundedbarcoesvalue.Barcode.Substring(0, 12);
                        if (str2.Length == 12)
                            textFromPngFile = "\n##" + str2 + "\n" + textFromPngFile;
                    }
                }
                string str3 = textFromPngFile;
                string[] separator = new string[3]
                {
          "\r\n",
          "\r",
          "\n"
                };
                foreach (string input in str3.Split(separator, StringSplitOptions.None))
                {
                    if (input.Contains(str1))
                    {
                        Regex.Replace(input, "\\s+", "");
                        textFromPngFile = "\n\n" + textFromPngFile;
                    }
                }
                return textFromPngFile;
            }
            catch (Exception ex)
            {
                return textFromPngFile;
            }
        }

        private static FTPDetails GetFTPDetailsFromPlant(string Plant)
        {
            FTPDetails detailsFromPlant = new FTPDetails();
            SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["AuthContext"].ToString());
            SqlCommand selectCommand = new SqlCommand("SELECT * FROM [dbo].[KeyValues] WHERE IsActive=1 AND PlantCode ='" + Plant + "'", connection);
            DataTable dataTable = new DataTable();
            try
            {
                using (SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(selectCommand))
                    sqlDataAdapter.Fill(dataTable);
                if (dataTable.Rows.Count > 0)
                {
                    detailsFromPlant.FTPUri = dataTable.Rows[0]["FTPURL"].ToString();
                    detailsFromPlant.FTPUser = dataTable.Rows[0]["FTPUserName"].ToString();
                    detailsFromPlant.Password = dataTable.Rows[0]["FTPPassword"].ToString();
                }
                return detailsFromPlant;
            }
            catch (Exception ex)
            {
                ErrorLog.WriteErrorLog("GetFTPDetailsFromPlant:" + ex.Message);
                return (FTPDetails)null;
            }
            finally
            {
                try
                {
                    selectCommand.Dispose();
                    connection.Close();
                }
                catch (Exception ex)
                {
                    ErrorLog.WriteErrorLog("GetFTPDetailsFromPlant:While Disposing." + ex.Message);
                }
            }
        }

        public static string GhostscritpImage(string inputFile, string outputFileName, int PageNumber)
        {
            try
            {
                int int32_1 = Convert.ToInt32(ConfigurationManager.AppSettings["GSXDPI"]);
                int int32_2 = Convert.ToInt32(ConfigurationManager.AppSettings["GSYDPI"]);
                string str = System.IO.Path.Combine(System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "TempFolder"), string.Format("{0}.png", (object)(DateTime.Now.ToString("yyyyMMddHHmmss") + outputFileName)));
                GhostscriptPngDevice ghostscriptPngDevice = new GhostscriptPngDevice((GhostscriptPngDeviceType)2);
                ((GhostscriptImageDevice)ghostscriptPngDevice).GraphicsAlphaBits = new GhostscriptImageDeviceAlphaBits?((GhostscriptImageDeviceAlphaBits)2);
                ((GhostscriptImageDevice)ghostscriptPngDevice).TextAlphaBits = new GhostscriptImageDeviceAlphaBits?((GhostscriptImageDeviceAlphaBits)2);
                ((GhostscriptImageDevice)ghostscriptPngDevice).ResolutionXY = new GhostscriptImageDeviceResolution(int32_1, int32_2);
                ((GhostscriptDevice)ghostscriptPngDevice).InputFiles.Add(inputFile);
                ((GhostscriptDevice)ghostscriptPngDevice).Pdf.FirstPage = new int?(PageNumber);
                ((GhostscriptDevice)ghostscriptPngDevice).Pdf.LastPage = new int?(PageNumber);
                ((GhostscriptDevice)ghostscriptPngDevice).PostScript = string.Empty;
                ((GhostscriptDevice)ghostscriptPngDevice).CustomSwitches.Add("-dDOINTERPOLATE");
                ((GhostscriptDevice)ghostscriptPngDevice).OutputPath = str;
                ((GhostscriptDevice)ghostscriptPngDevice).Process();
                return str;
            }
            catch (Exception ex)
            {
                ErrorLog.WriteErrorLog("GhostscritpImage:-" + ex.Message);
                return (string)null;
            }
        }

        public static void CreBackOCRFolder(string rooFolder, string fileList)
        {
            try
            {
                string backOcrFile = Service1.BackOCRFile;
                string str1 = fileList.ToString();
                System.IO.Path.GetFileName(str1);
                string path2_1 = rooFolder;
                string str2 = System.IO.Path.Combine(backOcrFile, path2_1);
                if (!Directory.Exists(str2))
                    Directory.CreateDirectory(str2);
                string path2_2 = DateTime.Now.ToString("yyyyMMdd");
                string str3 = System.IO.Path.Combine(str2, path2_2);
                if (!Directory.Exists(str3))
                    Directory.CreateDirectory(str3);
                try
                {
                    string str4 = System.IO.Path.Combine(str3, System.IO.Path.GetFileName(str1));
                    if (System.IO.File.Exists(str4))
                        return;
                    System.IO.File.Copy(str1, str4, true);
                    ErrorLog.WriteErrorLog("CreBackOCRFolder:- Backup Successfully" + str4);
                }
                catch (Exception ex)
                {
                    ErrorLog.WriteErrorLog("CreBackOCRFolder:- " + ex.Message);
                }
            }
            catch (Exception ex)
            {
                ErrorLog.WriteErrorLog("CreBackOCRFolder:- " + ex.Message);
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

        public static bool scanQRCode(string path)
        {
            try
            {
                Bitmap bitmap = (Bitmap)System.Drawing.Image.FromFile(path);
                bool flag = false;
                using (bitmap)
                {
                    ErrorLog.WriteErrorLog("scanQRCode function called:-");
                    BarcodeReader barcodeReader = new BarcodeReader();
                    ((BarcodeReaderGeneric)barcodeReader).AutoRotate = true;
                    ((BarcodeReaderGeneric)barcodeReader).Options.TryHarder = true;
                    ((BarcodeReaderGeneric)barcodeReader).Options.PureBarcode = false;
                    ((BarcodeReaderGeneric)barcodeReader).Options.PossibleFormats = (IList<BarcodeFormat>)new List<BarcodeFormat>();
                    ((BarcodeReaderGeneric)barcodeReader).Options.PossibleFormats.Add((BarcodeFormat)2048);
                    foreach (Result result in ((BarcodeReader<Bitmap>)barcodeReader).DecodeMultiple(bitmap))
                    {
                        ErrorLog.WriteErrorLog("scanQRCode:- Commplete=>" + result.ToString());
                        ErrorLog.WriteErrorLog("scanQRCode:-Checking for plant Commplete=>" + ScanCenter.plntCode);
                        if (!string.IsNullOrEmpty(result.ToString()))
                        {
                            if (result.ToString().Length < 15 && result.ToString().Contains(ScanCenter.plntCode))
                            {
                                ScanCenter.BarcodeValue = result.ToString();
                                ErrorLog.WriteErrorLog("scanQRCode:- QrcodeFound=>" + ScanCenter.BarcodeValue);
                                flag = true;
                            }
                            else
                            {
                                ErrorLog.WriteErrorLog("scanQRCode:- Not found =>");
                                flag = false;
                            }
                        }
                        else
                        {
                            ErrorLog.WriteErrorLog("scanQRCode:- QrcodeFound Not Found");
                            flag = false;
                        }
                    }
                }
                return flag;
            }
            catch (Exception ex)
            {
                ErrorLog.WriteErrorLog("scanQRCode:-" + ex.Message);
                return false;
            }
        }

        public static bool scanQRCode(Bitmap path)
        {
            try
            {
                Bitmap bitmap = path;
                bool flag = false;
                using (bitmap)
                {
                    ErrorLog.WriteErrorLog("scanQRCode function called:-");
                    BarcodeReader barcodeReader = new BarcodeReader();
                    ((BarcodeReaderGeneric)barcodeReader).AutoRotate = true;
                    ((BarcodeReaderGeneric)barcodeReader).Options.TryHarder = true;
                    ((BarcodeReaderGeneric)barcodeReader).Options.PureBarcode = false;
                    ((BarcodeReaderGeneric)barcodeReader).Options.PossibleFormats = (IList<BarcodeFormat>)new List<BarcodeFormat>();
                    ((BarcodeReaderGeneric)barcodeReader).Options.PossibleFormats.Add((BarcodeFormat)2048);
                    Result[] resultArray = ((BarcodeReader<Bitmap>)barcodeReader).DecodeMultiple(bitmap);
                    if (resultArray != null)
                    {
                        foreach (Result result in resultArray)
                        {
                            ErrorLog.WriteErrorLog("scanQRCode:- Commplete=>" + result.ToString());
                            ErrorLog.WriteErrorLog("scanQRCode:-Checking for plant Commplete=>" + ScanCenter.plntCode);
                            if (!string.IsNullOrEmpty(result.ToString()))
                            {
                                if (result.ToString().Length < 15)
                                {
                                    ScanCenter.BarcodeValue = result.ToString();
                                    ErrorLog.WriteErrorLog("scanQRCode:- QrcodeFound=>" + ScanCenter.BarcodeValue);
                                    return flag = true;
                                }
                                ErrorLog.WriteErrorLog("scanQRCode:- Not found =>");
                                flag = false;
                            }
                            else
                            {
                                ErrorLog.WriteErrorLog("scanQRCode:- QrcodeFound Not Found");
                                flag = false;
                            }
                        }
                    }
                    else
                    {
                        string str = new QRCodeDecoder().Decode((QRCodeImage)new QRCodeBitmapImage(bitmap));
                        ErrorLog.WriteErrorLog("scanQRCode:- Commplete=>" + str);
                        ErrorLog.WriteErrorLog("scanQRCode:-Checking for plant Commplete=>" + ScanCenter.plntCode);
                        if (!string.IsNullOrEmpty(str))
                        {
                            if (str.ToString().Length < 15)
                            {
                                ScanCenter.BarcodeValue = str.ToString();
                                ErrorLog.WriteErrorLog("scanQRCode:- QrcodeFound=>" + ScanCenter.BarcodeValue);
                                flag = true;
                            }
                            else
                            {
                                ErrorLog.WriteErrorLog("scanQRCode:- Not found =>");
                                flag = false;
                            }
                        }
                        else
                        {
                            ErrorLog.WriteErrorLog("scanQRCode:- QrcodeFound Not Found");
                            flag = false;
                        }
                    }
                }
                return flag;
            }
            catch (Exception ex)
            {
                ErrorLog.WriteErrorLog("scanQRCode:-" + ex.Message);
                return false;
            }
        }
    }
}

