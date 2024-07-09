using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Xml;
using BPCloud_VP.FactService.Models;
using BPCloud_VP.FactService.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace BPCloud_VP.FactService.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class FactController : ControllerBase
    {
        private readonly IFactRepository _FactRepository;
        private readonly IKRARepository _KRARepository;
        private readonly IBankRepository _BankRepository;
        private readonly IContactPersonRepository _ContactPersonRepository;
        private readonly IAIACTRepository _AIACTRepository;
        private readonly ICertificateRepository _CertificateRepository;
        private readonly ICardRepository _CardRespository;
        private readonly IHttpContextAccessor httpContextAccessor;
        private IConfiguration _configuration;

        public FactController(IFactRepository FactRepository, IKRARepository KRARepository,
            IBankRepository bankRepository, IContactPersonRepository ContactPersonRepository, IAIACTRepository AIACTRepository,
            ICertificateRepository CertificateRepository, ICardRepository CardRepository, IHttpContextAccessor httpContextAccessor, IConfiguration configuration)
        {
            _FactRepository = FactRepository;
            _KRARepository = KRARepository;
            _BankRepository = bankRepository;
            _ContactPersonRepository = ContactPersonRepository;
            _AIACTRepository = AIACTRepository;
            _CertificateRepository = CertificateRepository;
            _CardRespository = CardRepository;
            _configuration = configuration;

            this.httpContextAccessor = httpContextAccessor;
        }

        #region Dashboard

        [HttpGet]
        public DashboardGraphStatus GetDashboardGraphStatus(string PartnerID)
        {
            try
            {
                var DashboardGraphStatus = _FactRepository.GetDashboardGraphStatus(PartnerID);
                return DashboardGraphStatus;
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("Fact/GetDashboardGraphStatus", ex);
                return null;
            }
        }

        [HttpGet]
        public BPCKRA GetVendorKRAProcessCircle(string PartnerID)
        {
            try
            {
                var KRAProcessCircle = _FactRepository.GetVendorKRAProcessCircle(PartnerID);
                return KRAProcessCircle;
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("Fact/GetVendorKRAProcessCircle", ex);
                return null;
            }
        }
        //[HttpGet]
        //public List<BPCKRA> GetVendorKRAProcessCircleForSuperUser(string PartnerIDforSuperUser)
        //{
        //    try
        //    {
        //        var KRAProcessCircle = _FactRepository.GetVendorKRAProcessCircleForSuperUser(PartnerIDforSuperUser);
        //        return KRAProcessCircle;
        //    }
        //    catch (Exception ex)
        //    {
        //        WriteLog.WriteToFile("Fact/GetVendorKRAProcessCircle", ex);
        //        return null;
        //    }
        //}

        [HttpGet]
        public BPCKRA GetVendorPPMProcessCircle(string PartnerID)
        {
            try
            {
                var PPMProcessCircle = _FactRepository.GetVendorPPMProcessCircle(PartnerID);
                return PPMProcessCircle;
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("Fact/GetVendorPPMProcessCircle", ex);
                return null;
            }
        }

        [HttpGet]
        public List<BPCKRA> GetCustomerDoughnutChartData(string PartnerID)
        {
            try
            {
                var DashboardGraphStatus = _FactRepository.GetCustomerDoughnutChartData(PartnerID);
                return DashboardGraphStatus;
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("Fact/GetCustomerDoughnutChartData", ex);
                return null;
            }
        }

        [HttpGet]
        public BPCKRA GetCustomerOpenProcessCircle(string PartnerID)
        {
            try
            {
                var DashboardGraphStatus = _FactRepository.GetCustomerOpenProcessCircle(PartnerID);
                return DashboardGraphStatus;
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("Fact/GetCustomerOpenProcessCircle", ex);
                return null;
            }
        }

        [HttpGet]
        public BPCKRA GetCustomerCreditLimitProcessCircle(string PartnerID)
        {
            try
            {
                var DashboardGraphStatus = _FactRepository.GetCustomerCreditLimitProcessCircle(PartnerID);
                return DashboardGraphStatus;
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("Fact/GetCustomerCreditLimitProcessCircle", ex);
                return null;
            }
        }

        [HttpGet]
        public CustomerBarChartData GetCustomerBarChartData(string PartnerID)
        {
            try
            {
                var DashboardGraphStatus = _FactRepository.GetCustomerBarChartData(PartnerID);
                return DashboardGraphStatus;
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("Fact/GetCustomerBarChartData", ex);
                return null;
            }
        }

        #endregion

        #region BPCFact

        [HttpGet]
        public List<BPCFact> GetAllFacts()
        {
            try
            {
                var Facts = _FactRepository.GetAllFacts();
                return Facts;
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("Fact/GetAllFacts", ex);
                return null;
            }
        }
        [HttpGet]
        public List<BPCFact> GetAllVendors()
        {
            try
            {
                var Facts = _FactRepository.GetAllVendors();
                return Facts;
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("Fact/GetAllVendors", ex);
                return null;
            }
        }

        [HttpGet]
        public List<BPCFact1> GetAllVendorNames()
        {
            try
            {
                var Facts = _FactRepository.GetAllVendorNames();
                return Facts;
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("Fact/GetAllVendorNames", ex);
                return null;
            }
        }


        [HttpPost]
        public async Task<IActionResult> UploadOfCertificateAttachment()
        {
            var result = new BPCCertificateSupport();
            BPCAttachment bPCAttachment = new BPCAttachment();
            try
            {
                var request = Request;

                var CertificateName = request.Form["CertificateName"].ToString();
                var PartnerID = request.Form["PartnerID"].ToString();
                var CertificateType = request.Form["CertificateType"].ToString();

                var Client = request.Form["Client"].ToString();
                var Company = request.Form["Company"].ToString();
                var Type = request.Form["Type"].ToString();
                //IFormFileCollection file =
                IFormFileCollection postedfiles = request.Form.Files;
                BPCAttachment BPCAttachment = new BPCAttachment();
                if (postedfiles.Count > 0)
                {
                    for (int i = 0; i < postedfiles.Count; i++)
                    {
                        var FileName = postedfiles[i].FileName;
                        var ContentType = postedfiles[i].ContentType;
                        var ContentLength = postedfiles[i].Length;
                        using (Stream st = postedfiles[i].OpenReadStream())
                        {
                            using (BinaryReader br = new BinaryReader(st))
                            {
                                byte[] fileBytes = br.ReadBytes((Int32)st.Length);
                                if (fileBytes.Length > 0)
                                {
                                    BPCAttachment.Client = Client;
                                    BPCAttachment.Company = Company;
                                    BPCAttachment.Type = Type;
                                    BPCAttachment.CertificateName = CertificateName;
                                    BPCAttachment.CertificateType = CertificateType;
                                    BPCAttachment.AttachmentName = FileName;
                                    BPCAttachment.PatnerID = PartnerID;
                                    BPCAttachment.AttachmentFile = fileBytes;
                                    BPCAttachment.ContentType = ContentType;
                                    BPCAttachment.ContentLength = ContentLength;
                                }
                            }
                        }
                    }
                }
                if (!string.IsNullOrEmpty(BPCAttachment.CertificateName))
                {
                    result = await _CertificateRepository.AddAttachmentTOCertificate(BPCAttachment);
                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("PO/UploadOfCertificateAttachment", ex);
                return BadRequest(ex.Message);
            }
        }
        [HttpGet]
        public BPCFact GetFactByPartnerID(string PartnerID)
        {
            try
            {
                var Facts = _FactRepository.GetFactByPartnerID(PartnerID);
                //var host = Request.HttpContext.Connection.Id.ToString();
                //var ips = HttpContext.Features.Get<IHttpConnectionFeature>()?.RemoteIpAddress?.ToString();
                //IPAddress ip;
                //var headers = Request.Headers.ToList();
                //if (headers.Exists((kvp) => kvp.Key == "X-Forwarded-For"))
                //{
                //    // when running behind a load balancer you can expect this header
                //    var header = headers.First((kvp) => kvp.Key == "X-Forwarded-For").Value.ToString();
                //    ip = IPAddress.Parse(header);
                //}
                //else
                //{
                //    // this will always have a value (running locally in development won't have the header)
                //    ip = Request.HttpContext.Connection.RemoteIpAddress;
                //}
                //string HostName = Dns.GetHostName();
                //Console.WriteLine("Host Name of machine =" + HostName);
                //IPAddress[] ipaddress = Dns.GetHostAddresses(host);
                //Console.WriteLine("IP Address of Machine is");
                return Facts;
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("Fact/GetFactByPartnerID", ex);
                return null;
            }
        }

        [HttpGet]
        public List<BPCFact> GetFactByPartnerIDAndType(string PartnerID, string Type)
        {
            try
            {
                var Facts = _FactRepository.GetFactByPartnerIDAndType(PartnerID, Type);
                return Facts;
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("Fact/GetFactByPartnerIDAndType", ex);
                return null;
            }
        }

        [HttpGet]
        public BPCFact GetFactByEmailID(string EmailID)
        {
            try
            {
                var Facts = _FactRepository.GetFactByEmailID(EmailID);
                return Facts;
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("Fact/GetFactByEmailID", ex);
                return null;
            }
        }
        //[HttpGet]
        //public IActionResult CreateFactSupportXML(string PatnerID)
        //{
        //    try
        //    {
        //        var Facts = _FactRepository.GetFactSupportDetails(PatnerID);
        //        var ISXml = CreateXMLFromVendor(Facts, false);
        //        var FTPAttachment = SendAllAttachmentsToFTP(Facts, false);

        //        if (ISXml && FTPAttachment)
        //        {
        //            return Ok();
        //        }
        //        else
        //        {
        //            throw new Exception("Error in process FTP");
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        WriteLog.WriteToFile("Fact/CreateFactSupportXML", ex);
        //        return BadRequest(ex.Message);
        //    }
        //}
        [HttpGet]
        public async Task<IActionResult> CreateFactSupportXML(string PatnerID)
        {
            try
            {
                var Facts = _FactRepository.GetFactSupportDetails(PatnerID);
                var XmlFilePath = CreateXMLFromVendor(Facts, false);
                var FTPAttachments = CreateAllAttachments(Facts, false);

                if (!string.IsNullOrEmpty(XmlFilePath))
                {
                    await _FactRepository.GetHelpDeskUserAndSendMail(Facts, XmlFilePath, FTPAttachments);
                    return Ok();
                }
                else
                {
                    throw new Exception("Error in creating Xml file");
                }
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("Fact/CreateFactSupportXML", ex);
                return BadRequest(ex.Message);
            }
        }
        [HttpGet]
        public IActionResult FindFactByTaxNumber(string TaxNumber)
        {
            try
            {
                var Facts = _FactRepository.FindFactByTaxNumber(TaxNumber);
                if (Facts != null)
                {
                    return Ok(Facts);
                }
                return BadRequest($"No records found for tax number {TaxNumber}");
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("Fact/FindFactBByTaxNumber", ex);
                return BadRequest($"No records found for tax number {TaxNumber}");
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateFact(BPCFactView Fact)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var result = await _FactRepository.CreateFact(Fact);
                return Ok(result);
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("Fact/CreateFact", ex);
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateFactDetails([FromBody] List<BPCFact> Facts)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var result = await _FactRepository.CreateFacts(Facts);
                if (result != null)
                {
                    return Ok("Data are inserted successfully");
                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("Fact/CreateFact", ex);
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> UpdateFactSupportDataToMasterData(string partnerId)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                await _FactRepository.UpdateFactSupportDataToMasterData(partnerId);
                return new OkResult();
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("Fact/UpdateFactSupportDataToMasterData", ex);
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        public IActionResult DownloadOfAttachment(string PartnerID, string certificateName, string certificateType, int AttachmentID)
        {
            try
            {
                BPCAttachment BPAttachment = _CertificateRepository.GetAttachmentByName(PartnerID, certificateName, certificateType, AttachmentID);
                if (BPAttachment != null && BPAttachment.AttachmentFile != null && BPAttachment.AttachmentFile.Length > 0)
                {
                    Stream stream = new MemoryStream(BPAttachment.AttachmentFile);
                    return File(BPAttachment.AttachmentFile, "application/octet-stream", BPAttachment.CertificateName);
                }
                return NotFound();
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("Fact/DownloadOfAttachment", ex);
                return BadRequest(ex.Message);
            }
        }
        [HttpPost]
        public async Task<IActionResult> UpdateFact(BPCFactView Fact)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var result = await _FactRepository.UpdateFact(Fact);
                return Ok(result);
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("Fact/UpdateFact", ex);
                return BadRequest(ex.Message);
            }
        }


        [HttpPost]
        public async Task<IActionResult> UpdateFact_BPCFact(BPCFact Fact)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var result = await _FactRepository.UpdateFact_BPCFact(Fact);
                //var IsXML = CreateXMLFromVendor(result);
                //var IsAttachmentFTP = SendAllAttachmentsToFTP(result);

                //if (IsXML && IsAttachmentFTP)
                //{
                //    return Ok();
                //}
                //else
                //{
                //    throw new Exception("Error in processing in FTP");
                //}
                return Ok();
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("Fact/UpdateFact_BPCFact", ex);
                return BadRequest(ex.Message);
            }
        }
        public string CreateXMLFromVendor(BPCFactSupport BPCFact, bool IsDecleration = true)
        {
            try
            {
                string writerPath = string.Empty;
                if (BPCFact != null)
                {
                    List<BPCCertificate> Certificates = new List<BPCCertificate>();
                    List<BPCFactBank> Banks = new List<BPCFactBank>();
                    List<BPCFactContactPerson> Contacts = new List<BPCFactContactPerson>();

                    WriteLog.WriteToFile("Fact/CreateXMLFromVendor", "------CreateXMLFromVendor method started------");
                    CreateVendorTempFolder();
                    Random r = new Random();
                    int num = r.Next(1, 9999999);
                    string writerFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "VendorTempFolder");
                    var FileName = "VX_" + BPCFact.PatnerID + ".xml";
                    //var FileName = BPCFact.TransID+"_" + num +"_" + ".xml";

                    writerPath = Path.Combine(writerFolder, FileName);
                    XmlWriter writer = XmlWriter.Create(writerPath);
                    Contacts = _ContactPersonRepository.GetContactPersonsByPartnerID(BPCFact.PatnerID);

                    if (IsDecleration)
                    {
                        Certificates = _CertificateRepository.GetCertificatesByPartnerID(BPCFact.PatnerID);
                        Banks = _BankRepository.GetBanksByPartnerID(BPCFact.PatnerID);
                    }
                    else
                    {
                        var SupportCertificate = _CertificateRepository.GetSupportCertificates(BPCFact.PatnerID);
                        var SupportBanks = _BankRepository.GetSupportBanks(BPCFact.PatnerID);

                        Certificates = JsonConvert.DeserializeObject<List<BPCCertificate>>(JsonConvert.SerializeObject(SupportCertificate));
                        Banks = JsonConvert.DeserializeObject<List<BPCFactBank>>(JsonConvert.SerializeObject(SupportBanks));

                    }
                    WriteLog.WriteToFile("Fact/CreateXMLFromVendor", "XML file fetching the Vendor details");
                    writer.WriteStartDocument();
                    writer.WriteStartElement("Vendor");
                    writer.WriteElementString("NAME", BPCFact.Name);
                    writer.WriteElementString("Client", BPCFact.Client);
                    writer.WriteElementString("Company", BPCFact.Company);
                    writer.WriteElementString("Type", BPCFact.Type);
                    writer.WriteElementString("PatnerID", BPCFact.PatnerID);
                    if (BPCFact.LegalName != null && BPCFact.LegalName.Split().Length > 2)
                    {
                        var names = BPCFact.LegalName.Split();
                        writer.WriteElementString("LEGAL_NAME", names[0] + " " + names[1]);
                    }
                    else
                    {
                        writer.WriteElementString("LEGAL_NAME", BPCFact.LegalName);
                    }
                    writer.WriteElementString("ADDRESS_LINE1", BPCFact.AddressLine1);
                    writer.WriteElementString("ADDRESS_LINE2", BPCFact.AddressLine2);
                    writer.WriteElementString("CITY", BPCFact.City);
                    writer.WriteElementString("STATE", BPCFact.State);
                    writer.WriteElementString("COUNTRY", BPCFact.Country);
                    writer.WriteElementString("PINCODE", BPCFact.PinCode);
                    writer.WriteElementString("PANNumber", BPCFact.PANNumber);
                    writer.WriteElementString("PHONE1", BPCFact.Phone1 ?? "");
                    writer.WriteElementString("PHONE2", BPCFact.Phone2 ?? "");
                    writer.WriteElementString("EMAIL1", BPCFact.Email1 ?? "");
                    writer.WriteElementString("EMAIL2", BPCFact.Email2 ?? "");
                    writer.WriteElementString("ROLE", BPCFact.Role ?? "");
                    writer.WriteElementString("TYPE", BPCFact.Type ?? "");
                    writer.WriteElementString("ACCOUNT_GROUP", BPCFact.AccountGroup ?? "");
                    writer.WriteElementString("PURCHASE_ORG", BPCFact.PurchaseOrg ?? "");
                    writer.WriteElementString("COMPANY_CODE", BPCFact.CompanyCode ?? "");
                    writer.WriteElementString("TypeofIndustry", BPCFact.TypeofIndustry ?? "");
                    writer.WriteElementString("GSTNumber", BPCFact.GSTNumber ?? "");
                    writer.WriteElementString("GSTStatus", BPCFact.GSTStatus ?? "");

                    writer.WriteStartElement("MSME");
                    writer.WriteElementString("MSME", BPCFact.MSME.ToString());
                    writer.WriteElementString("MSME_TYPE", BPCFact.MSME_TYPE ?? "");
                    writer.WriteElementString("MSME_Att_ID", BPCFact.MSME_Att_ID ?? "");
                    writer.WriteEndElement();

                    writer.WriteStartElement("RP");
                    writer.WriteElementString("RP", BPCFact.RP.ToString());
                    writer.WriteElementString("RP_Name", BPCFact.RP_Name ?? "");
                    writer.WriteElementString("RP_Type", BPCFact.RP_Type ?? "");
                    writer.WriteElementString("RP_Att_ID", BPCFact.RP_Att_ID ?? "");
                    writer.WriteEndElement();


                    writer.WriteStartElement("TDS");
                    writer.WriteElementString("Reduced_TDS", BPCFact.Reduced_TDS.ToString());
                    writer.WriteElementString("TDS_Cert_No", BPCFact.TDS_Cert_No ?? "");
                    writer.WriteElementString("TDS_RATE", BPCFact.TDS_RATE ?? "");
                    writer.WriteElementString("TDS_Att_ID", BPCFact.TDS_Att_ID ?? "");
                    writer.WriteEndElement();
                    //writer.WriteElementString("VENDOR_CODE", BPCFact.VendorCode ?? "");
                    //writer.WriteElementString("CREATED_ON", BPCFact.CreatedOn.ToString("yyyyMMdd HH:mm:ss") ?? "");

                    if (Certificates != null && Certificates.Count > 0)
                    {
                        writer.WriteStartElement("Identity");
                        WriteLog.WriteToFile("Fact/CreateXMLFromVendor", "Vendor Certificates count" + Certificates.Count);
                        foreach (var Certificate in Certificates)
                        {
                            writer.WriteStartElement("Item");
                            writer.WriteElementString("CertificateType", Certificate.CertificateType ?? "");
                            writer.WriteElementString("CertificateName", Certificate.CertificateName ?? "");
                            writer.WriteEndElement();
                        }
                        writer.WriteEndElement();
                    }

                    if (Banks != null && Banks.Count > 0)
                    {
                        writer.WriteStartElement("Bank");
                        WriteLog.WriteToFile("Fact/CreateXMLFromVendor", "Vendor Banks count" + Banks.Count);
                        int i = 1;
                        foreach (var bank in Banks)
                        {
                            writer.WriteStartElement("Item");
                            writer.WriteElementString("AccountNo", bank.AccountNo ?? "");
                            writer.WriteElementString("Name", bank.Name ?? "");
                            writer.WriteElementString("IFSC", bank.BankID ?? "");
                            writer.WriteElementString("BankName", bank.BankName ?? "");
                            writer.WriteElementString("Branch", bank.Branch ?? "");
                            writer.WriteElementString("City", bank.City ?? "");
                            writer.WriteEndElement();
                            i++;
                        }
                        writer.WriteEndElement();
                    }

                    if (Contacts != null && Contacts.Count > 0)
                    {
                        writer.WriteStartElement("Contact");
                        WriteLog.WriteToFile("Registration/CreateXMLFromVendor", "Vendor Contacts count" + Contacts.Count);
                        foreach (var contact in Contacts)
                        {
                            writer.WriteStartElement("Item");
                            writer.WriteElementString("ContactPersonID", contact.ContactPersonID ?? "");
                            writer.WriteElementString("Name", contact.Name ?? "");
                            writer.WriteElementString("ContactNumber", contact.ContactNumber ?? "");
                            writer.WriteElementString("Email", contact.Email ?? "");
                            writer.WriteElementString("Department", contact.Department ?? "");
                            writer.WriteEndElement();
                        }
                        writer.WriteEndElement();
                    }

                    writer.WriteEndElement();
                    writer.WriteEndDocument();
                    writer.Flush();
                    writer.Close();
                    WriteLog.WriteToFile("Fact/CreateXMLFromVendor", "Created  XML file with Vendor");

                    //var uploadStatus = UploadFileToVendorOutputFolder(writerFolder, FileName);
                    //if (uploadStatus == true)
                    //{
                    //    status = true;
                    //    WriteLog.WriteToFile("Fact/CreateXMLFromVendor", "UploadFileToVendorOutputFolder Success");
                    //}
                    //else
                    //{
                    //    status = false;
                    //    WriteLog.WriteToFile("Fact/CreateXMLFromVendor", "UploadFileToVendorOutputFolder Failure");
                    //}

                    WriteLog.WriteToFile("Fact/CreateXMLFromVendor", "------CreateXMLFromVendorOnBoarding method ended------");
                }
                return writerPath;
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("Fact/CreateXMLFromVendor/Exception", ex.Message);
                return string.Empty;
            }

        }

        public List<FTPAttachment> CreateAllAttachments(BPCFactSupport Fact, bool ISDecleration = true)
        {
            List<FTPAttachment> Attachments = new List<FTPAttachment>();
            try
            {
                WriteLog.WriteToFile("Fact/CreateAllAttachments", "------Attachment method started------");
                CreateVendorTempFolder();
                Random r = new Random();
                int num = r.Next(1, 9999999);
                string writerFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "VendorTempFolder");
                Attachments = _FactRepository.GetAllAttachmentsToFTP(Fact, ISDecleration);

                for (var i = 0; i < Attachments.Count; i++)
                {
                    if (Attachments[i].AttachmentFile != null && Attachments[i].AttachmentFile.Length > 0)
                    {
                        var FileName = "VX_" + Fact.PatnerID + "_" + Attachments[i].Type + "_" + Attachments[i].AttachmentName;
                        Attachments[i].AttachmentName = FileName;
                        //var FileFullPath = Path.Combine(writerFolder, FileName);
                        //System.IO.File.WriteAllBytes(FileFullPath, Attachment[i].AttachmentFile);
                        WriteLog.WriteToFile("Fact/CreateAllAttachments", $"------File {FileName} added in VendorTempFolder------");
                    }
                    else
                    {
                        WriteLog.WriteToFile($"File { Attachments[i].AttachmentName} doesn't have any content");
                    }

                }

                WriteLog.WriteToFile("Fact/CreateAllAttachments", "------Attachment method ended------");

                //var uploadStatus = UploadFileToVendorOutputFolder(writerFolder, "FTPFiles");

                //if (uploadStatus == true)
                //{
                //    status = true;
                //    WriteLog.WriteToFile("Fact/SendAllAttachmentsToFTP", "UploadFileToVendorOutputFolder Success");
                //}
                //else
                //{
                //    status = false;
                //    WriteLog.WriteToFile("Fact/SendAllAttachmentsToFTP", "UploadFileToVendorOutputFolder Failure");
                //}
                return Attachments;
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("Fact/CreateAllAttachments/Exception", ex.Message);
                return Attachments;
            }
        }
        //public bool CreateXMLFromVendor(BPCFactSupport BPCFact, bool IsDecleration = true)
        //{
        //    try
        //    {
        //        bool status = false;
        //        if (BPCFact != null)
        //        {
        //            List<BPCCertificate> Certificates = new List<BPCCertificate>();
        //            List<BPCFactBank> Banks = new List<BPCFactBank>();
        //            List<BPCFactContactPerson> Contacts = new List<BPCFactContactPerson>();

        //            WriteLog.WriteToFile("Fact/CreateXMLFromVendor", "------CreateXMLFromVendor method started------");
        //            CreateVendorTempFolder();
        //            Random r = new Random();
        //            int num = r.Next(1, 9999999);
        //            string writerFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "VendorTempFolder");
        //            var FileName = "VX_" + BPCFact.PatnerID + ".xml";
        //            //var FileName = BPCFact.TransID+"_" + num +"_" + ".xml";

        //            string writerPath = Path.Combine(writerFolder, FileName);
        //            XmlWriter writer = XmlWriter.Create(writerPath);
        //            Contacts = _ContactPersonRepository.GetContactPersonsByPartnerID(BPCFact.PatnerID);

        //            if (IsDecleration)
        //            {
        //                Certificates = _CertificateRepository.GetCertificatesByPartnerID(BPCFact.PatnerID);
        //                Banks = _BankRepository.GetBanksByPartnerID(BPCFact.PatnerID);
        //            }
        //            else
        //            {
        //                var SupportCertificate = _CertificateRepository.GetSupportCertificates(BPCFact.PatnerID);
        //                var SupportBanks = _BankRepository.GetSupportBanks(BPCFact.PatnerID);

        //                Certificates = JsonConvert.DeserializeObject<List<BPCCertificate>>(JsonConvert.SerializeObject(SupportCertificate));
        //                Banks = JsonConvert.DeserializeObject<List<BPCFactBank>>(JsonConvert.SerializeObject(SupportBanks));

        //            }
        //            WriteLog.WriteToFile("Fact/CreateXMLFromVendor", "XML file fetching the Vendor details");
        //            writer.WriteStartDocument();
        //            writer.WriteStartElement("Vendor");
        //            writer.WriteElementString("NAME", BPCFact.Name);
        //            writer.WriteElementString("Client", BPCFact.Client);
        //            writer.WriteElementString("Company", BPCFact.Company);
        //            writer.WriteElementString("Type", BPCFact.Type);
        //            writer.WriteElementString("PatnerID", BPCFact.PatnerID);
        //            if (BPCFact.LegalName != null && BPCFact.LegalName.Split().Length > 2)
        //            {
        //                var names = BPCFact.LegalName.Split();
        //                writer.WriteElementString("LEGAL_NAME", names[0] + " " + names[1]);
        //            }
        //            else
        //            {
        //                writer.WriteElementString("LEGAL_NAME", BPCFact.LegalName);
        //            }
        //            writer.WriteElementString("ADDRESS_LINE1", BPCFact.AddressLine1);
        //            writer.WriteElementString("ADDRESS_LINE2", BPCFact.AddressLine2);
        //            writer.WriteElementString("CITY", BPCFact.City);
        //            writer.WriteElementString("STATE", BPCFact.State);
        //            writer.WriteElementString("COUNTRY", BPCFact.Country);
        //            writer.WriteElementString("PINCODE", BPCFact.PinCode);
        //            writer.WriteElementString("PANNumber", BPCFact.PANNumber);
        //            writer.WriteElementString("PHONE1", BPCFact.Phone1 ?? "");
        //            writer.WriteElementString("PHONE2", BPCFact.Phone2 ?? "");
        //            writer.WriteElementString("EMAIL1", BPCFact.Email1 ?? "");
        //            writer.WriteElementString("EMAIL2", BPCFact.Email2 ?? "");
        //            writer.WriteElementString("ROLE", BPCFact.Role ?? "");
        //            writer.WriteElementString("TYPE", BPCFact.Type ?? "");
        //            writer.WriteElementString("ACCOUNT_GROUP", BPCFact.AccountGroup ?? "");
        //            writer.WriteElementString("PURCHASE_ORG", BPCFact.PurchaseOrg ?? "");
        //            writer.WriteElementString("COMPANY_CODE", BPCFact.CompanyCode ?? "");
        //            writer.WriteElementString("TypeofIndustry", BPCFact.TypeofIndustry ?? "");
        //            writer.WriteElementString("GSTNumber", BPCFact.GSTNumber ?? "");
        //            writer.WriteElementString("GSTStatus", BPCFact.GSTStatus ?? "");

        //            writer.WriteStartElement("MSME");
        //            writer.WriteElementString("MSME", BPCFact.MSME.ToString());
        //            writer.WriteElementString("MSME_TYPE", BPCFact.MSME_TYPE ?? "");
        //            writer.WriteElementString("MSME_Att_ID", BPCFact.MSME_Att_ID ?? "");
        //            writer.WriteEndElement();

        //            writer.WriteStartElement("RP");
        //            writer.WriteElementString("RP", BPCFact.RP.ToString());
        //            writer.WriteElementString("RP_Name", BPCFact.RP_Name ?? "");
        //            writer.WriteElementString("RP_Type", BPCFact.RP_Type ?? "");
        //            writer.WriteElementString("RP_Att_ID", BPCFact.RP_Att_ID ?? "");
        //            writer.WriteEndElement();


        //            writer.WriteStartElement("TDS");
        //            writer.WriteElementString("Reduced_TDS", BPCFact.Reduced_TDS.ToString());
        //            writer.WriteElementString("TDS_Cert_No", BPCFact.TDS_Cert_No ?? "");
        //            writer.WriteElementString("TDS_RATE", BPCFact.TDS_RATE ?? "");
        //            writer.WriteElementString("TDS_Att_ID", BPCFact.TDS_Att_ID ?? "");
        //            writer.WriteEndElement();
        //            //writer.WriteElementString("VENDOR_CODE", BPCFact.VendorCode ?? "");
        //            //writer.WriteElementString("CREATED_ON", BPCFact.CreatedOn.ToString("yyyyMMdd HH:mm:ss") ?? "");

        //            if (Certificates != null && Certificates.Count > 0)
        //            {
        //                writer.WriteStartElement("Identity");
        //                WriteLog.WriteToFile("Fact/CreateXMLFromVendor", "Vendor Certificates count" + Certificates.Count);
        //                foreach (var Certificate in Certificates)
        //                {
        //                    writer.WriteStartElement("Item");
        //                    writer.WriteElementString("CertificateType", Certificate.CertificateType ?? "");
        //                    writer.WriteElementString("CertificateName", Certificate.CertificateName ?? "");
        //                    writer.WriteEndElement();
        //                }
        //                writer.WriteEndElement();
        //            }

        //            if (Banks != null && Banks.Count > 0)
        //            {
        //                writer.WriteStartElement("Bank");
        //                WriteLog.WriteToFile("Fact/CreateXMLFromVendor", "Vendor Banks count" + Banks.Count);
        //                int i = 1;
        //                foreach (var bank in Banks)
        //                {
        //                    writer.WriteStartElement("Item");
        //                    writer.WriteElementString("AccountNo", bank.AccountNo ?? "");
        //                    writer.WriteElementString("Name", bank.Name ?? "");
        //                    writer.WriteElementString("IFSC", bank.BankID ?? "");
        //                    writer.WriteElementString("BankName", bank.BankName ?? "");
        //                    writer.WriteElementString("Branch", bank.Branch ?? "");
        //                    writer.WriteElementString("City", bank.City ?? "");
        //                    writer.WriteEndElement();
        //                    i++;
        //                }
        //                writer.WriteEndElement();
        //            }

        //            if (Contacts != null && Contacts.Count > 0)
        //            {
        //                writer.WriteStartElement("Contact");
        //                WriteLog.WriteToFile("Registration/CreateXMLFromVendor", "Vendor Contacts count" + Contacts.Count);
        //                foreach (var contact in Contacts)
        //                {
        //                    writer.WriteStartElement("Item");
        //                    writer.WriteElementString("ContactPersonID", contact.ContactPersonID ?? "");
        //                    writer.WriteElementString("Name", contact.Name ?? "");
        //                    writer.WriteElementString("ContactNumber", contact.ContactNumber ?? "");
        //                    writer.WriteElementString("Email", contact.Email ?? "");
        //                    writer.WriteElementString("Department", contact.Department ?? "");
        //                    writer.WriteEndElement();
        //                }
        //                writer.WriteEndElement();
        //            }

        //            writer.WriteEndElement();
        //            writer.WriteEndDocument();
        //            writer.Flush();
        //            writer.Close();
        //            WriteLog.WriteToFile("Fact/CreateXMLFromVendor", "Created  XML file with Vendor");

        //            var uploadStatus = UploadFileToVendorOutputFolder(writerFolder, FileName);
        //            if (uploadStatus == true)
        //            {
        //                status = true;
        //                WriteLog.WriteToFile("Fact/CreateXMLFromVendor", "UploadFileToVendorOutputFolder Success");
        //            }
        //            else
        //            {
        //                status = false;
        //                WriteLog.WriteToFile("Fact/CreateXMLFromVendor", "UploadFileToVendorOutputFolder Failure");
        //            }
        //            WriteLog.WriteToFile("Fact/CreateXMLFromVendor", "------CreateXMLFromVendorOnBoarding method ended------");
        //        }
        //        return status;
        //    }
        //    catch (Exception ex)
        //    {
        //        WriteLog.WriteToFile("Fact/CreateXMLFromVendor/Exception", ex.Message);
        //        return false;
        //    }

        //}

        //public bool SendAllAttachmentsToFTP(BPCFactSupport Fact, bool ISDecleration = true)
        //{
        //    try
        //    {
        //        bool status = false;
        //        WriteLog.WriteToFile("Fact/SendAllAttachmentsToFTP", "------AttachmentFtp method started------");
        //        CreateVendorTempFolder();
        //        Random r = new Random();
        //        int num = r.Next(1, 9999999);
        //        string writerFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "VendorTempFolder");
        //        var Attachment = _FactRepository.GetAllAttachmentsToFTP(Fact, ISDecleration);

        //        for (var i = 0; i < Attachment.Count; i++)
        //        {
        //            if (Attachment[i].AttachmentFile != null && Attachment[i].AttachmentFile.Length > 0)
        //            {
        //                var FileName = "VX_" + Fact.PatnerID + "_" + Attachment[i].Type + "_" + Attachment[i].AttachmentName;
        //                var FileFullPath = Path.Combine(writerFolder, FileName);
        //                System.IO.File.WriteAllBytes(FileFullPath, Attachment[i].AttachmentFile);
        //                WriteLog.WriteToFile("Fact/SendAllAttachmentsToFTP", $"------File {FileName} added in VendorTempFolder------");
        //            }
        //            else
        //            {
        //                WriteLog.WriteToFile($"File { Attachment[i].AttachmentName} doesn't have any content");
        //            }

        //        }

        //        WriteLog.WriteToFile("Fact/SendAllAttachmentsToFTP", "FTP File upload about to start");

        //        var uploadStatus = UploadFileToVendorOutputFolder(writerFolder, "FTPFiles");

        //        if (uploadStatus == true)
        //        {
        //            status = true;
        //            WriteLog.WriteToFile("Fact/SendAllAttachmentsToFTP", "UploadFileToVendorOutputFolder Success");
        //        }
        //        else
        //        {
        //            status = false;
        //            WriteLog.WriteToFile("Fact/SendAllAttachmentsToFTP", "UploadFileToVendorOutputFolder Failure");
        //        }
        //        return status;
        //    }
        //    catch (Exception ex)
        //    {
        //        WriteLog.WriteToFile("Fact/attachmentFtp/Exception", ex.Message);
        //        return false;
        //    }
        //}
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
                                WriteLog.WriteToFile("Fact/UploadFileToVendorOutputFolder", "File uploaded to Vendor Output folder");
                                status = true;
                                WriteLog.WriteToFile("Fact/UploadFileToVendorOutputFolder", string.Format("File {0} was successfully uploaded to FTP {1}", file.Name, FTPOutbox));
                                System.IO.File.Delete(file.FullName);
                                //return status;
                            }
                            else
                            {
                                status = false;
                                WriteLog.WriteToFile("Fact/UploadFileToVendorOutputFolder", string.Format("File {0} has no contents", file.FullName));
                            }
                        }
                    }
                }
                return status;
            }

            catch (Exception ex)
            {
                WriteLog.WriteToFile("Fact/UploadFileToVendorOutputFolder/Exception", ex.Message);
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
                            FileInfo fi = new FileInfo(f);
                            if (fi.CreationTime < DateTime.Now.AddMinutes(-30))
                            {
                                System.GC.Collect();
                                System.GC.WaitForPendingFinalizers();
                                System.IO.File.Delete(f);
                            }

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("Fact/CreateVendorTempFolder/Exception", ex.Message);
            }
        }
        [HttpPost]
        public async Task<List<BPCAttach>> UpdateAttachment()
        {
            try
            {
                var request = Request;
                var Client = request.Form["Client"].ToString();
                var Company = request.Form["Company"].ToString();
                var PatnerID = request.Form["PatnerID"].ToString();
                var ReferenceNo = request.Form["ReferenceNo"].ToString();
                var Type = request.Form["Type"].ToString();
                IFormFileCollection postedfiles = request.Form.Files;
                List<string> catogery = new List<string>();

                List<BPCAttachments> AttachmentList = new List<BPCAttachments>();
                if (postedfiles.Count > 0)
                {
                    for (int i = 0; i < postedfiles.Count; i++)
                    {
                        BPCAttachments Attachment = new BPCAttachments();
                        Attachment.PatnerID = PatnerID;
                        Attachment.Client = Client;
                        Attachment.Company = Company;
                        Attachment.ReferenceNo = ReferenceNo;
                        Attachment.Type = Type;
                        var name = postedfiles[i].Name;
                        //var attachmentId = 0;
                        //if (name[0] != "")
                        //{
                        //    attachmentId = int.Parse(name[0]);
                        //}
                        catogery.Add(name);
                        var FileName = postedfiles[i].FileName;
                        var ContentType = postedfiles[i].ContentType;
                        var ContentLength = postedfiles[i].Length;
                        using (Stream st = postedfiles[i].OpenReadStream())
                        {
                            using (BinaryReader br = new BinaryReader(st))
                            {
                                byte[] fileBytes = br.ReadBytes((Int32)st.Length);
                                if (fileBytes.Length > 0)
                                {
                                    //Attachment.AttachmentID = attachmentId;
                                    Attachment.AttachmentName = FileName;
                                    Attachment.ContentType = ContentType;
                                    Attachment.ContentLength = ContentLength;
                                    Attachment.AttachmentFile = fileBytes;
                                }
                            }

                        }
                        AttachmentList.Add(Attachment);
                    }



                }
                var result = await _FactRepository.UpdateAttachment(AttachmentList, catogery);
                //  var result = await _FactRepository.UpdateAttachment(Attachment);
                return result;
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("Fact/UpdateFactSupportDataToMasterData", ex);
                return null;
            }
        }
        #region GettAttchmentByAttId
        [HttpGet]
        public BPCAttachments GettAttchmentByAttId(int AttachmentID)
        {
            try
            {

                var result = _FactRepository.GettAttchmentByAttId(AttachmentID);
                return result;
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("Fact/GetAllInvoices", ex);
                return null;
            }
        }
        #endregion
        [HttpPost]
        public async Task<IActionResult> UpdateFactDetail(BPCFact Fact)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                await _FactRepository.UpdateFact(Fact);
                return new OkResult();
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("Fact/UpdateFact", ex);
                return BadRequest(ex.Message);
            }
        }
        [HttpPost]
        public async Task<IActionResult> UpdateFactSupport(BPCFactViewSupport Fact)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var result = await _FactRepository.UpdateFactSupport(Fact);
                return Ok(result);
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("Fact/UpdateFactSupport", ex);
                return BadRequest(ex.Message);
            }
        }
        [HttpPost]
        public async Task<IActionResult> DeleteFact(BPCFact Fact)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                await _FactRepository.DeleteFact(Fact);
                return new OkResult();
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("Fact/DeleteFact", ex);
                return BadRequest(ex.Message);
            }
        }

        #endregion

        #region BPCFactContactPerson

        [HttpGet]
        public List<BPCFactContactPerson> GetAllContactPersons()
        {
            try
            {
                var ContactPersons = _ContactPersonRepository.GetAllContactPersons();
                return ContactPersons;
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("Fact/GetAllContactPersons", ex);
                return null;
            }
        }

        [HttpGet]
        public List<BPCFactContactPerson> GetContactPersonsByPartnerID(string PartnerID)
        {
            try
            {
                var ContactPersons = _ContactPersonRepository.GetContactPersonsByPartnerID(PartnerID);
                return ContactPersons;
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("Fact/GetContactPersonsByPartnerID", ex);
                return null;
            }
        }


        [HttpPost]
        public async Task<IActionResult> CreateContactPersonDetails([FromBody] List<BPCFactContactPerson> ContactPersons)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var result = await _ContactPersonRepository.CreateContactPersons(ContactPersons);
                if (result != null)
                {
                    return Ok("Data are inserted successfully");
                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("Fact/CreateContactPersons", ex);
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateContactPerson(BPCFactContactPerson ContactPerson)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var result = await _ContactPersonRepository.CreateContactPerson(ContactPerson);
                return Ok(result);
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("Fact/CreateContactPerson", ex);
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> UpdateContactPerson(BPCFactContactPerson ContactPerson)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                await _ContactPersonRepository.UpdateContactPerson(ContactPerson);
                return new OkResult();
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("Fact/UpdateContactPerson", ex);
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> DeleteContactPerson(BPCFactContactPerson ContactPerson)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                await _ContactPersonRepository.DeleteContactPerson(ContactPerson);
                return new OkResult();
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("Fact/DeleteContactPerson", ex);
                return BadRequest(ex.Message);
            }
        }

        #endregion

        #region BPCFactBank

        [HttpGet]
        public List<BPCFactBank> GetAllBanks()
        {
            try
            {
                var Banks = _BankRepository.GetAllBanks();
                return Banks;
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("Fact/GetAllBanks", ex);
                return null;
            }
        }

        [HttpGet]
        public List<BPCFactBank> GetBanksByPartnerID(string PartnerID)
        {
            try
            {
                var Banks = _BankRepository.GetBanksByPartnerID(PartnerID);
                return Banks;
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("Fact/GetBanksByPartnerID", ex);
                return null;
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateBank(BPCFactBank Bank)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var result = await _BankRepository.CreateBank(Bank);
                return Ok(result);
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("Fact/CreateBank", ex);
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateBankDetails([FromBody] List<BPCFactBank> Banks)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var result = await _BankRepository.CreateBanks(Banks);
                if (result != null)
                {
                    return Ok("Data are inserted successfully");
                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("Fact/CreateBankDetails", ex);
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateSupportBank(BPCFactBankSupport Bank)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var result = await _BankRepository.CreateSupportBank(Bank);
                return Ok(result);
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("Fact/CreateSupportBank", ex);
                return BadRequest(ex.Message);
            }
        }
        [HttpPost]
        public async Task<IActionResult> UpdateBank(BPCFactBank Bank)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                await _BankRepository.UpdateBank(Bank);
                return new OkResult();
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("Fact/UpdateBank", ex);
                return BadRequest(ex.Message);
            }
        }
        [HttpPost]
        public async Task<IActionResult> DeleteBank(BPCFactBank Bank)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                await _BankRepository.DeleteBank(Bank);
                return new OkResult();
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("Fact/DeleteBank", ex);
                return BadRequest(ex.Message);
            }
        }
        [HttpPost]
        public async Task<IActionResult> DeleteCertificate(BPCCertificate certificate)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                await _CertificateRepository.DeleteCertificate(certificate);
                return new OkResult();
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("Fact/DeleteCertificate", ex);
                return BadRequest(ex.Message);
            }
        }
        #endregion

        #region BPCKRA

        [HttpGet]
        public List<BPCKRA> GetAllKRAs()
        {
            try
            {
                var KRAs = _KRARepository.GetAllKRAs();
                return KRAs;
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("Fact/GetAllKRAs", ex);
                return null;
            }
        }

        [HttpGet]
        public List<BPCKRA> GetKRAsByPartnerID(string PartnerID)
        {
            try
            {
                var KRAs = _KRARepository.GetKRAsByPartnerID(PartnerID);
                return KRAs;
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("Fact/GetKRAsByPartnerID", ex);
                return null;
            }
        }


        [HttpPost]
        public async Task<IActionResult> CreateKRA(BPCKRA KRA)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var result = await _KRARepository.CreateKRA(KRA);
                return Ok(result);
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("Fact/CreateKRA", ex);
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateKRADetails([FromBody] List<BPCKRA> KRAs)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var result = await _KRARepository.CreateKRAs(KRAs);
                if (result != null)
                {
                    return Ok("Data are inserted successfully");
                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("Fact/CreateKRADetails", ex);
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateKRAs(object KRAs, string PartnerID)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                List<BPCKRA> KRA = JsonConvert.DeserializeObject<List<BPCKRA>>(JsonConvert.SerializeObject(KRAs));
                await _KRARepository.CreateKRAs(KRA, PartnerID);
                return Ok();
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("Fact/CreateKRAs", ex);
                return BadRequest(ex.Message);
            }
        }
        [HttpPost]
        public async Task<IActionResult> UpdateKRA(BPCKRA KRA)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                await _KRARepository.UpdateKRA(KRA);
                return new OkResult();
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("Fact/UpdateKRA", ex);
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> DeleteKRA(BPCKRA KRA)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                await _KRARepository.DeleteKRA(KRA);
                return new OkResult();
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("Fact/DeleteKRA", ex);
                return BadRequest(ex.Message);
            }
        }
        [HttpPost]
        public async Task<IActionResult> DeleteKRAByPartnerID(string PartnerID)
        {
            try
            {
                if (string.IsNullOrEmpty(PartnerID))
                {
                    return BadRequest(PartnerID);
                }
                await _KRARepository.DeleteKRAByPartnerID(PartnerID);
                return new OkResult();
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("Fact/DeleteKRAByPartnerID", ex);
                return BadRequest(ex.Message);
            }
        }

        #endregion

        #region BPCAIACT

        [HttpGet]
        public List<BPCAIACT> GetAllAIACTs()
        {
            try
            {
                var AIACTs = _AIACTRepository.GetAllAIACTs();
                return AIACTs;
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("Fact/GetAllAIACTs", ex);
                return null;
            }
        }

        [HttpGet]
        public List<BPCAIACT> GetAIACTsByPartnerID(string PartnerID)
        {
            try
            {
                var AIACTs = _AIACTRepository.GetAIACTsByPartnerID(PartnerID);
                return AIACTs;
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("Fact/GetAIACTsByPartnerID", ex);
                return null;
            }
        }

        [HttpGet]
        public List<BPCAIACT> GetActionsByPartnerID(string PartnerID)
        {
            try
            {
                var AIACTs = _AIACTRepository.GetActionsByPartnerID(PartnerID);
                return AIACTs;
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("Fact/GetActionsByPartnerID", ex);
                return null;
            }
        }

        [HttpGet]
        public List<BPCAIACT> GetNotificationsByPartnerID(string PartnerID)
        {
            try
            {
                var AIACTs = _AIACTRepository.GetNotificationsByPartnerID(PartnerID);
                return AIACTs;
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("Fact/GetAIACTsByPartnerID", ex);
                return null;
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateAIACT(BPCAIACT AIACT)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var result = await _AIACTRepository.CreateAIACT(AIACT);
                return Ok(result);
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("Fact/CreateAIACT", ex);
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateAIACTDetails([FromBody] List<BPCAIACT> AIACTs)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var result = await _AIACTRepository.CreateAIACTDetails(AIACTs);
                return Ok(result);
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("Fact/CreateAIACTs", ex);
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> UpdateAIACT(BPCAIACT AIACT)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                await _AIACTRepository.UpdateAIACT(AIACT);
                return new OkResult();
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("Fact/UpdateAIACT", ex);
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> UpdateAIACTs([FromBody] List<int> SeqNos)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                await _AIACTRepository.UpdateAIACTs(SeqNos);
                return new OkResult();
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("Fact/UpdateAIACTs", ex);
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> DeleteAIACT(BPCAIACT AIACT)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                await _AIACTRepository.DeleteAIACT(AIACT);
                return new OkResult();
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("Fact/DeleteAIACT", ex);
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> AcceptAIACT(BPCAIACT AIACT)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                await _AIACTRepository.AcceptAIACT(AIACT);
                return new OkResult();
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("Fact/AcceptAIACT", ex);
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> AcceptAIACTs([FromBody] List<BPCAIACT> AIACTs)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                await _AIACTRepository.AcceptAIACTs(AIACTs);
                return new OkResult();
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("Fact/AcceptAIACTs", ex);
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> RejectAIACT(BPCAIACT AIACT)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                await _AIACTRepository.RejectAIACT(AIACT);
                return new OkResult();
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("Fact/RejectAIACT", ex);
                return BadRequest(ex.Message);
            }
        }

        #endregion

        #region BPCCertificate

        [HttpGet]
        public List<BPCCertificate> GetAllCertificates()
        {
            try
            {
                var Certificates = _CertificateRepository.GetAllCertificates();
                return Certificates;
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("Fact/GetAllCertificates", ex);
                return null;
            }
        }

        [HttpGet]
        public List<BPCCertificate> GetCertificatesByPartnerID(string PartnerID)
        {
            try
            {
                var Certificates = _CertificateRepository.GetCertificatesByPartnerID(PartnerID);
                return Certificates;
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("Fact/GetCertificatesByPartnerID", ex);
                return null;
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateCertificate(BPCCertificate Certificate)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var result = await _CertificateRepository.CreateCertificate(Certificate);
                return Ok(result);
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("Fact/CreateCertificate", ex);
                return BadRequest(ex.Message);
            }
        }
        [HttpPost]
        public async Task<IActionResult> CreateCertificateSupport(BPCCertificateSupport Certificate)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var result = await _CertificateRepository.CreateCertificateSupport(Certificate);
                return Ok(result);
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("Fact/CreateCertificate", ex);
                return BadRequest(ex.Message);
            }
        }
        [HttpPost]
        public async Task<IActionResult> UpdateCertificate(BPCCertificate Certificate)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                await _CertificateRepository.UpdateCertificate(Certificate);
                return new OkResult();
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("Fact/UpdateCertificate", ex);
                return BadRequest(ex.Message);
            }
        }



        #endregion

        #region Data Migration 

        [HttpPost]
        public async Task<IActionResult> CreateFacts([FromBody] List<BPCFactXLSX> Facts)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                await _FactRepository.CreateFacts(Facts);
                return Ok();
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("Fact/CreateFacts", ex);
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateBanks([FromBody] List<BPCFactBankXLSX> Banks)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                await _BankRepository.CreateBanks(Banks);
                return Ok();
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("Fact/CreateBanks", ex);
                return BadRequest(ex.Message);
            }
        }

        #endregion

        #region BPCDashboardCard

        [HttpPost]
        public async Task<IActionResult> SaveDashboardCards()
        {
            try
            {
                var request = Request;
                var CreatedBy = request.Form["CreatedBy"].ToString();
                IFormFileCollection postedfiles = request.Form.Files;
                List<BPCDashboardCard> BPCDashboardCards = new List<BPCDashboardCard>();
                if (postedfiles.Count > 0)
                {
                    for (int i = 0; i < postedfiles.Count; i++)
                    {
                        var FileName = postedfiles[i].FileName;
                        var ContentType = postedfiles[i].ContentType;
                        var ContentLength = postedfiles[i].Length;
                        using (Stream st = postedfiles[i].OpenReadStream())
                        {
                            using (BinaryReader br = new BinaryReader(st))
                            {
                                byte[] fileBytes = br.ReadBytes((Int32)st.Length);
                                if (fileBytes.Length > 0)
                                {
                                    BPCDashboardCard BPCDashboardCard = new BPCDashboardCard();
                                    BPCDashboardCard.AttachmentName = FileName;
                                    BPCDashboardCard.ContentType = ContentType;
                                    BPCDashboardCard.ContentLength = ContentLength;
                                    BPCDashboardCard.AttachmentFile = fileBytes;
                                    BPCDashboardCards.Add(BPCDashboardCard);
                                }
                            }

                        }
                    }
                }
                if (BPCDashboardCards.Count > 0)
                {
                    await _CardRespository.SaveDashboardCards(BPCDashboardCards);
                }

            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("SupportDesk/SaveDashboardCards", ex);
                return BadRequest(ex.Message);
            }
            return Ok();
        }

        [HttpGet]
        public IActionResult GetDashboardCard1()
        {
            try
            {
                BPCDashboardCard BPCDashboardCard = _CardRespository.GetDashboardCard1();
                if (BPCDashboardCard != null && BPCDashboardCard.AttachmentFile.Length > 0)
                {
                    Stream stream = new MemoryStream(BPCDashboardCard.AttachmentFile);
                    return File(BPCDashboardCard.AttachmentFile, BPCDashboardCard.ContentType, BPCDashboardCard.AttachmentName);
                }
                return NotFound();
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("SupportDesk/GetDashboardCard1", ex);
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        public IActionResult GetDashboardCard2()
        {
            try
            {
                BPCDashboardCard BPCDashboardCard = _CardRespository.GetDashboardCard2();
                if (BPCDashboardCard != null && BPCDashboardCard.AttachmentFile.Length > 0)
                {
                    Stream stream = new MemoryStream(BPCDashboardCard.AttachmentFile);
                    return File(BPCDashboardCard.AttachmentFile, BPCDashboardCard.ContentType, BPCDashboardCard.AttachmentName);
                }
                return NotFound();
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("SupportDesk/GetDashboardCard2", ex);
                return BadRequest(ex.Message);
            }
        }

        #endregion

    }
}