using BPCloud_VP.FactService.DBContexts;
using BPCloud_VP.FactService.Models;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace BPCloud_VP.FactService.Repositories
{
    public class FactRepository : IFactRepository
    {
        private readonly FactContext _dbContext;
        IConfiguration _configuration;

        public FactRepository(FactContext dbContext, IConfiguration configuration)
        {
            _dbContext = dbContext;
            _configuration = configuration;
        }

        public List<BPCFact> GetAllFacts()
        {
            try
            {
                return _dbContext.BPCFacts.ToList();
            }
            catch (SqlException ex) { WriteLog.WriteToFile("FactRepository/GetAllFacts", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("FactRepository/GetAllFacts", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public BPCFact GetFactByPartnerID(string PartnerID)
        {
            try
            {
                return _dbContext.BPCFacts.FirstOrDefault(x => x.PatnerID == PartnerID);
            }
            catch (SqlException ex) { WriteLog.WriteToFile("FactRepository/GetFactByPartnerID", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("FactRepository/GetFactByPartnerID", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        //public BPCFact GetFactSupportByPartnerID(string PartnerID)
        //{
        //    try
        //    {
        //         FactSupportView:BPCFactViewSupport;
        //        _dbContext.BPCFactsSupport.Where(x => x.PatnerID == PartnerID);
        //    }
        //    catch (SqlException ex){ WriteLog.WriteToFile("FactRepository/", ex); throw new Exception("Something went wrong");}catch(InvalidOperationException ex){WriteLog.WriteToFile("FactRepository/", ex); throw new Exception("Something went wrong"); }catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}
        public List<BPCFact> GetFactByPartnerIDAndType(string PartnerID, string Type)
        {
            try
            {
                //return _dbContext.BPCFacts.FirstOrDefault(x => x.PatnerID == PartnerID && x.Type.ToLower() == Type.ToLower());
                return _dbContext.BPCFacts.Where(x => x.PatnerID == PartnerID && x.Type.ToLower() == Type.ToLower()).ToList();
            }
            catch (SqlException ex) { WriteLog.WriteToFile("FactRepository/GetFactByPartnerIDAndType", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("FactRepository/GetFactByPartnerIDAndType", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public BPCFact GetFactByEmailID(string EmailID)
        {
            try
            {
                return _dbContext.BPCFacts.FirstOrDefault(x => x.Email1 == EmailID);
            }
            catch (SqlException ex) { WriteLog.WriteToFile("FactRepository/GetFactByEmailID", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("FactRepository/GetFactByEmailID", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public BPCFact FindFactByTaxNumber(string TaxNumber)
        {
            try
            {
                var result = (from tb in _dbContext.BPCFacts
                              where tb.IsActive &&
                              tb.GSTNumber == TaxNumber || tb.TaxID1 == TaxNumber || tb.TaxID2 == TaxNumber
                              select tb).FirstOrDefault();
                return result;

            }
            catch (SqlException ex) { WriteLog.WriteToFile("FactRepository/FindFactByTaxNumber", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("FactRepository/FindFactByTaxNumber", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<BPCFact> CreateFacts(List<BPCFact> Facts)
        {
            var log = new BPCLog();
            try
            {
                log = await CreateBPCLog("CreateFacts", Facts.Count);
                foreach (var Fact in Facts)
                {
                    Fact.PatnerID = Fact.PatnerID.TrimStart(new Char[] { '0' });
                    var entity = _dbContext.BPCFacts.FirstOrDefault(x => x.Client == Fact.Client && x.Company == Fact.Company && x.Type == Fact.Type && x.PatnerID == Fact.PatnerID);
                    if (entity == null)
                    {
                        Fact.IsActive = true;
                        Fact.CreatedBy = "SAP API";
                        Fact.CreatedOn = DateTime.Now;
                        var result = _dbContext.BPCFacts.Add(Fact);
                        VendorUser vendorUser = new VendorUser();
                        vendorUser.UserName = Fact.PatnerID;
                        vendorUser.Email = Fact.Email1;
                        vendorUser.DisplayName = Fact.LegalName;
                        vendorUser.Phone = Fact.Phone1;
                        vendorUser.IsBlocked = Fact.IsBlocked;
                        var entity1 = _dbContext.BPCFacts.FirstOrDefault(x => x.Type == Fact.Type && x.PatnerID == Fact.PatnerID);
                        if (entity1 == null)
                        {
                            var res = await CreateVendorUser(vendorUser);
                            if (res)
                            {
                                await _dbContext.SaveChangesAsync();
                            }
                        }
                        else
                        {
                            await _dbContext.SaveChangesAsync();
                        }

                    }
                    else
                    {
                        WriteLog.WriteToFile($"FactRepository/CreateFacts:- Fact details already exist for {Fact.PatnerID}");
                        await UpdateFact(Fact);
                    }

                }
                if (log != null)
                {
                    await UpdateBPCSucessLog(log.LogID);
                }
                else
                {
                    WriteLog.WriteToFile("Fact/CreateFacts--" + "Unable to generate Log");
                }
                if (Facts.Count > 0)
                    return Facts[0];
                return null;
            }
            catch (SqlException ex) { WriteLog.WriteToFile("FactRepository/CreateFacts", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("FactRepository/CreateFacts", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                if (log != null)
                {
                    await UpdateBPCFailureLog(log.LogID, ex.Message);
                }
                else
                {
                    WriteLog.WriteToFile("Fact/UpdateFact--" + "Unable to generate Log");
                }
                throw ex;
            }
        }

        public async Task<BPCFact> CreateFact(BPCFactView FactView)
        {
            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                try
                {
                    BPCFact Fact = new BPCFact();
                    Fact.Client = FactView.Client;
                    Fact.Company = FactView.Company;
                    Fact.Type = FactView.Type;
                    Fact.PatnerID = FactView.PatnerID;
                    Fact.LegalName = FactView.LegalName;
                    Fact.AddressLine1 = FactView.AddressLine1;
                    Fact.AddressLine2 = FactView.AddressLine2;
                    Fact.City = FactView.City;
                    Fact.State = FactView.State;
                    Fact.Country = FactView.Country;
                    Fact.PinCode = FactView.PinCode;
                    Fact.Type = FactView.Type;
                    Fact.Phone1 = FactView.Phone1;
                    Fact.Phone2 = FactView.Phone2;
                    Fact.Email1 = FactView.Email1;
                    Fact.Email2 = FactView.Email2;
                    Fact.TaxID1 = FactView.TaxID1;
                    Fact.TaxID2 = FactView.TaxID2;
                    Fact.OutstandingAmount = FactView.OutstandingAmount;
                    Fact.CreditAmount = FactView.CreditAmount;
                    Fact.LastPayment = FactView.LastPayment;
                    Fact.LastPaymentDate = FactView.LastPaymentDate;
                    Fact.Currency = FactView.Currency;
                    Fact.CreditLimit = FactView.CreditLimit;
                    Fact.CreditBalance = FactView.CreditBalance;
                    Fact.CreditEvalDate = FactView.CreditEvalDate;

                    Fact.Name = FactView.Name;
                    Fact.Role = FactView.Role;
                    Fact.PurchaseOrg = FactView.PurchaseOrg;
                    Fact.AccountGroup = FactView.AccountGroup;
                    Fact.CompanyCode = FactView.CompanyCode;
                    Fact.TypeofIndustry = FactView.TypeofIndustry;
                    Fact.VendorCode = FactView.VendorCode;

                    Fact.IsActive = true;
                    Fact.CreatedOn = DateTime.Now;
                    var result = _dbContext.BPCFacts.Add(Fact);
                    await _dbContext.SaveChangesAsync();

                    //IdentityRepository identityRepository = new IdentityRepository(_dbContext);
                    //await identityRepository.CreateIdentities(FactView.bPIdentities, result.Entity.TransID);

                    //BankRepository BankRepository = new BankRepository(_dbContext);
                    //await BankRepository.CreateBanks(FactView.BPCFactBanks, result.Entity.TransID);

                    //ContactRepository ContactRepository = new ContactRepository(_dbContext);
                    //await ContactRepository.CreateContacts(FactView.bPContacts, result.Entity.TransID);

                    //ActivityLogRepository ActivityLogRepository = new ActivityLogRepository(_dbContext);
                    //await ActivityLogRepository.CreateActivityLogs(FactView.bPActivityLogs, result.Entity.TransID);

                    transaction.Commit();
                    transaction.Dispose();
                    return result.Entity;
                }
                catch (SqlException ex) { WriteLog.WriteToFile("FactRepository/CreateFact", ex); throw new Exception("Something went wrong"); }
                catch (InvalidOperationException ex) { WriteLog.WriteToFile("FactRepository/CreateFact", ex); throw new Exception("Something went wrong"); }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    transaction.Dispose();
                    throw ex;
                }

            }
        }
        public async Task<BPCFactViewSupport> UpdateFactSupport(BPCFactViewSupport FactView)
        {
            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                try
                {
                    var Fact = FactView;
                    //BPCFacts
                    var BPCFacts = _dbContext.BPCFactsSupport.Where(x => x.PatnerID == Fact.BPCFact.PatnerID).FirstOrDefault();

                    if (BPCFacts != null)
                    {
                        _dbContext.BPCFactsSupport.Remove(BPCFacts);
                    }
                    _dbContext.BPCFactsSupport.Add(FactView.BPCFact);
                    if (Fact.BPCFactBanks.Count > 0)
                    {
                        UpdateFactBankSupport(Fact.BPCFactBanks);
                    }
                    if (Fact.BPCFactCerificate.Count > 0)
                    {
                        UpdateFactCertificateSupport(Fact.BPCFactCerificate);
                    }
                    await _dbContext.SaveChangesAsync();
                    transaction.Commit();
                    transaction.Dispose();
                    return Fact;
                }
                catch (SqlException ex) { WriteLog.WriteToFile("FactRepository/UpdateFactSupport", ex); throw new Exception("Something went wrong"); }
                catch (InvalidOperationException ex) { 
                    WriteLog.WriteToFile("FactRepository/UpdateFactSupport", ex); 
                    throw new Exception("Something went wrong"); 
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    transaction.Dispose();
                    throw ex;
                }
            }
        }
        public void UpdateFactBankSupport(List<BPCFactBankSupport> FactBank)
        {
            try
            {
                var Bank = _dbContext.BPCFactBanksSupport.Where(x => x.PatnerID == FactBank[0].PatnerID).ToList();

                if (Bank.Count > 0)
                {
                    foreach (BPCFactBankSupport bank in Bank)
                    {
                        _dbContext.BPCFactBanksSupport.Remove(bank);
                    }
                }
                foreach (BPCFactBankSupport bank in FactBank)
                {
                    bank.CreatedOn = DateTime.Now;
                    bank.IsActive = true;
                    _dbContext.BPCFactBanksSupport.Add(bank);
                }
            }
            catch (SqlException ex) { WriteLog.WriteToFile("FactRepository/UpdateFactBankSupport", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("FactRepository/UpdateFactBankSupport", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void UpdateFactCertificateSupport(List<BPCCertificateSupport> FactCertificate)
        {
            try
            {
                var certificates = _dbContext.BPCCertificatesSupport.Where(x => x.PatnerID == FactCertificate[0].PatnerID).ToList();

                //for (var j = 0; j < FactCertificate.Count; j++)
                //{
                //    var certificate = _dbContext.BPCCertificatesSupport.Where(x => x.PatnerID == FactCertificate[j].PatnerID).ToList();
                //    var k = 0;
                //    var count = 0;
                //    if (certificate.Count > 0)
                //    {
                //        while (k > FactCertificate.Count)
                //        {
                //            for (var i = 0; i < certificate.Count; i++)
                //            {
                //                if (FactCertificate[j].CertificateName == certificate[i].CertificateName)
                //                {
                //                    certificate[i].CertificateType = FactCertificate[j].CertificateType;
                //                    certificate[i].CertificateName = FactCertificate[j].CertificateName;
                //                    certificate[i].Validity = FactCertificate[j].Validity;
                //                    certificate[i].Attachment = FactCertificate[j].Attachment;
                //                    count++;
                //                }

                //            }
                //            if (count == 0)
                //            {
                //                _dbContext.BPCCertificatesSupport.Add(FactCertificate[j]);
                //                count = 0;
                //            }
                //            k++;
                //        }
                //    }
                //    else
                //    {
                //        _dbContext.BPCCertificatesSupport.AddRange(FactCertificate);
                //    }
                //}

                if (certificates.Count > 0)
                {
                    foreach (BPCCertificateSupport certificate in certificates)
                    {

                        _dbContext.BPCCertificatesSupport.Remove(certificate);
                    }
                }
                foreach (BPCCertificateSupport certificate in FactCertificate)
                {
                    certificate.CreatedOn = DateTime.Now;
                    certificate.IsActive = true;
                    _dbContext.BPCCertificatesSupport.Add(certificate);
                }
            }
            catch (SqlException ex) { WriteLog.WriteToFile("FactRepository/UpdateFactCertificateSupport", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("FactRepository/UpdateFactCertificateSupport", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<BPCFact> UpdateFact(BPCFact Fact)
        {
            var log = new BPCLog();
            try
            {

                log = await CreateBPCLog("UpdateFact", 1);
                var entity = _dbContext.Set<BPCFact>().FirstOrDefault(x => x.Client == Fact.Client && x.Company == Fact.Company && x.Type == Fact.Type && x.PatnerID == Fact.PatnerID);
                if (entity == null)
                {
                    await UpdateBPCFailureLog(log.LogID, $"No Records found for {Fact.PatnerID}");

                    return entity;
                }
                ////_dbContext.Entry(Fact).State = EntityState.Modified;
                //entity.Name = Fact.Name;
                //entity.Role = Fact.Role;
                entity.LegalName = Fact.LegalName;
                entity.AddressLine1 = Fact.AddressLine1;
                entity.AddressLine2 = Fact.AddressLine2;
                entity.City = Fact.City;
                entity.State = Fact.State;
                entity.Country = Fact.Country;
                entity.PinCode = Fact.PinCode;
                entity.Type = Fact.Type;
                entity.Phone1 = Fact.Phone1;
                entity.Phone2 = Fact.Phone2;
                entity.Email1 = Fact.Email1;
                entity.Email2 = Fact.Email2;
                entity.TaxID1 = Fact.TaxID1;
                entity.TaxID2 = Fact.TaxID2;
                entity.OutstandingAmount = Fact.OutstandingAmount;
                entity.CreditAmount = Fact.CreditAmount;
                entity.LastPayment = Fact.LastPayment;
                entity.LastPaymentDate = Fact.LastPaymentDate;
                entity.Currency = Fact.Currency;
                entity.CreditLimit = Fact.CreditLimit;
                entity.CreditBalance = Fact.CreditBalance;
                entity.CreditEvalDate = Fact.CreditEvalDate;

                entity.MSME = Fact.MSME;
                entity.MSME_TYPE = Fact.MSME_TYPE;
                entity.MSME_Att_ID = Fact.MSME_Att_ID;
                entity.Reduced_TDS = Fact.Reduced_TDS;
                entity.TDS_RATE = Fact.TDS_RATE;
                entity.TDS_Att_ID = Fact.TDS_Att_ID;
                entity.TDS_Cert_No = Fact.TDS_Cert_No;
                entity.RP = Fact.RP;
                entity.RP_Name = Fact.RP_Name;
                entity.RP_Type = Fact.RP_Type;
                entity.RP_Att_ID = Fact.RP_Att_ID;

                entity.ModifiedBy = "SAP API";
                entity.ModifiedOn = DateTime.Now;

                entity.Role = Fact.Role;
                entity.PurchaseOrg = Fact.PurchaseOrg;
                entity.AccountGroup = Fact.AccountGroup;
                entity.CompanyCode = Fact.CompanyCode;
                entity.TypeofIndustry = Fact.TypeofIndustry;
                entity.VendorCode = Fact.VendorCode;
                await _dbContext.SaveChangesAsync();
                if (log != null)
                {
                    await UpdateBPCSucessLog(log.LogID);
                }
                else
                {
                    WriteLog.WriteToFile("Fact/UpdateFact--" + "Unable to generate Log");
                }
                return Fact;
            }
            catch (SqlException ex) { WriteLog.WriteToFile("FactRepository/UpdateFact", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("FactRepository/UpdateFact", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                if (log != null)
                {
                    await UpdateBPCFailureLog(log.LogID, ex.Message);
                }
                else
                {
                    WriteLog.WriteToFile("Fact/UpdateFact--" + "Unable to generate Log");
                }
                throw ex;
            }
        }
        public async Task<BPCFactSupport> UpdateFact_BPCFact(BPCFact Fact)
        {
            var log = new BPCLog();
            try
            {
                log = await CreateBPCLog("UpdateFact", 1);
                var entity = _dbContext.Set<BPCFactSupport>().FirstOrDefault(x => x.Client == Fact.Client && x.Company == Fact.Company && x.Type == Fact.Type && x.PatnerID == Fact.PatnerID);
                if (entity != null)
                {
                    _dbContext.BPCFactsSupport.Remove(entity);
                }
                ////_dbContext.Entry(Fact).State = EntityState.Modified;
                //entity.Name = Fact.Name;
                //entity.Role = Fact.Role;
                BPCFactSupport factView = new BPCFactSupport();
                factView.Client = Fact.Client;
                factView.Company = Fact.Company;
                factView.Type = Fact.Type;
                factView.PatnerID = Fact.PatnerID;
                factView.LegalName = Fact.LegalName;
                factView.AddressLine1 = Fact.AddressLine1;
                factView.AddressLine2 = Fact.AddressLine2;
                factView.City = Fact.City;
                factView.State = Fact.State;
                factView.Country = Fact.Country;
                factView.PinCode = Fact.PinCode;
                factView.Type = Fact.Type;
                factView.Phone1 = Fact.Phone1;
                factView.Phone2 = Fact.Phone2;
                factView.Email1 = Fact.Email1;
                factView.Email2 = Fact.Email2;
                factView.TaxID1 = Fact.TaxID1;
                factView.TaxID2 = Fact.TaxID2;
                factView.OutstandingAmount = Fact.OutstandingAmount;
                factView.CreditAmount = Fact.CreditAmount;
                factView.LastPayment = Fact.LastPayment;
                factView.LastPaymentDate = Fact.LastPaymentDate;
                factView.Currency = Fact.Currency;
                factView.CreditLimit = Fact.CreditLimit;
                factView.CreditBalance = Fact.CreditBalance;
                factView.CreditEvalDate = Fact.CreditEvalDate;
                factView.MSME = Fact.MSME;
                factView.MSME_TYPE = Fact.MSME_TYPE;
                factView.MSME_Att_ID = Fact.MSME_Att_ID;
                factView.Reduced_TDS = Fact.Reduced_TDS;
                factView.TDS_RATE = Fact.TDS_RATE;
                factView.TDS_Att_ID = Fact.TDS_Att_ID;
                factView.TDS_Cert_No = Fact.TDS_Cert_No;
                factView.RP = Fact.RP;
                factView.RP_Name = Fact.RP_Name;
                factView.RP_Type = Fact.RP_Type;
                factView.RP_Att_ID = Fact.RP_Att_ID;
                factView.ModifiedBy = Fact.ModifiedBy;
                factView.CreatedOn = DateTime.Now;
                factView.ModifiedOn = DateTime.Now;
                factView.Plant = Fact.Plant;
                factView.Role = Fact.Role;
                factView.PurchaseOrg = Fact.PurchaseOrg;
                factView.AccountGroup = Fact.AccountGroup;
                factView.CompanyCode = Fact.CompanyCode;
                factView.TypeofIndustry = Fact.TypeofIndustry;
                factView.VendorCode = Fact.VendorCode;

                _dbContext.BPCFactsSupport.Add(factView);
                await _dbContext.SaveChangesAsync();
                if (log != null)
                {
                    await UpdateBPCSucessLog(log.LogID);
                }
                else
                {
                    WriteLog.WriteToFile("Fact/UpdateFact--" + "Unable to generate Log");
                }

                List<BPCAttachments> attachments = new List<BPCAttachments>();
                if (factView.MSME_Att_ID != null && factView.MSME_Att_ID != "")
                {
                    var result = this.GettAttchmentByAttId(int.Parse(factView.MSME_Att_ID));
                    if (result != null)
                    {
                        var FileName = "VX_" + factView.PatnerID + "_" + "MSME" + "_" + result.AttachmentName;
                        result.AttachmentName = FileName;
                        attachments.Add(result);
                    }
                }
                if (factView.RP_Att_ID != null && factView.RP_Att_ID != "")
                {
                    var result = this.GettAttchmentByAttId(int.Parse(factView.RP_Att_ID));
                    if (result != null)
                    {
                        var FileName = "VX_" + factView.PatnerID + "_" + "RP" + "_" + result.AttachmentName;
                        result.AttachmentName = FileName;
                        attachments.Add(result);
                    }
                }
                if (factView.TDS_Att_ID != null && factView.TDS_Att_ID != "")
                {
                    var result = this.GettAttchmentByAttId(int.Parse(factView.TDS_Att_ID));
                    if (result != null)
                    {
                        var FileName = "VX_" + factView.PatnerID + "_" + "TDS" + "_" + result.AttachmentName;
                        result.AttachmentName = FileName;
                        attachments.Add(result);
                    }
                }

                var User = await HelpdeskUserByCompany(factView.Company);
                foreach (var usr in User)
                {
                    if (string.IsNullOrEmpty(usr.Email))
                    {
                        usr.Email = "exalca.plant1@gmail.com";
                    }
                }

                //var Buyerfact = this._dbContext.BPCFacts.Where(x => x.PatnerID == User.UserName).FirstOrDefault();
                //var buyerMailId = "";
                //if (Buyerfact != null)
                //{
                //    if (Buyerfact.Email1 != null && Buyerfact.Email1 != "")
                //    {
                //        buyerMailId = Buyerfact.Email1;
                //    }
                //    else if (Buyerfact.Email2 != null && Buyerfact.Email2 != "")
                //    {
                //        buyerMailId = Buyerfact.Email1;
                //    }
                //}
                //else
                //{
                //    buyerMailId = "exalca.plant1@gmail.com";
                //    //buyerMailId=User.Email;
                //}

                var IsMailSent = await SendMail(factView, attachments, User);
                return factView;
            }
            catch (SqlException ex) { WriteLog.WriteToFile("FactRepository/UpdateFact_BPCFact", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("FactRepository/UpdateFact_BPCFact", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                if (log != null)
                {
                    await UpdateBPCFailureLog(log.LogID, ex.Message);
                }
                else
                {
                    WriteLog.WriteToFile("Fact/UpdateFact--" + "Unable to generate Log");
                }
                throw ex;
            }
        }

        public async Task GetHelpDeskUserAndSendMail(BPCFactSupport fact, string XmlFilePath, List<FTPAttachment> attachments)
        {
            var User = await HelpdeskUserByCompany(fact.Company);
            foreach (var usr in User)
            {
                if (string.IsNullOrEmpty(usr.Email))
                {
                    usr.Email = "exalca.plant1@gmail.com";
                }
            }
            //var Buyerfact = this._dbContext.BPCFacts.Where(x => x.PatnerID == User.UserName).FirstOrDefault();
            //var buyerMailId = "";
            //if (Buyerfact != null)
            //{
            //    if (Buyerfact.Email1 != null && Buyerfact.Email1 != "")
            //    {
            //        buyerMailId = Buyerfact.Email1;
            //    }
            //    else if (Buyerfact.Email2 != null && Buyerfact.Email2 != "")
            //    {
            //        buyerMailId = Buyerfact.Email1;
            //    }
            //}
            //else
            //{
            //    buyerMailId = "exalca.plant1@gmail.com";
            //    //buyerMailId=User.Email;
            //}
            var IsMailSent = await SendMail(fact, XmlFilePath, attachments, User);
        }

        public async Task<User> GetBuyerFactByPlant(string plant)
        {
            try
            {
                User user = new User();
                string BaseAddress = _configuration.GetValue<string>("APIBaseAddress");
                string HostURI = BaseAddress + "/authenticationapi/Master/BuyerFactByPlant?plant=" + plant;
                var uri = new Uri(HostURI);
                var request = (HttpWebRequest)WebRequest.Create(uri);
                request.Method = "GET";
                try
                {
                    using (var response = (HttpWebResponse)await request.GetResponseAsync())
                    {
                        if (response != null && response.StatusCode == HttpStatusCode.OK)
                        {
                            var reader = new StreamReader(response.GetResponseStream());
                            string responseString = await reader.ReadToEndAsync();
                            user = JsonConvert.DeserializeObject<User>(responseString);
                            reader.Close();
                            return user;
                        }
                        else
                        {
                            var reader = new StreamReader(response.GetResponseStream());
                            string responseString = await reader.ReadToEndAsync();
                            reader.Close();
                            return user;
                        }
                    }
                }
                catch (WebException ex)
                {
                    using (var stream = ex.Response.GetResponseStream())
                    using (var reader = new StreamReader(stream))
                    {
                        var errorMessage = reader.ReadToEnd();
                        throw ex;
                    }
                }
                catch (SqlException ex) { WriteLog.WriteToFile("FactRepository/GetBuyerFactByPlant", ex); throw new Exception("Something went wrong"); }
                catch (InvalidOperationException ex) { WriteLog.WriteToFile("FactRepository/GetBuyerFactByPlant", ex); throw new Exception("Something went wrong"); }
                catch (Exception ex)
                {
                    throw ex;
                }

            }
            catch (SqlException ex) { WriteLog.WriteToFile("FactRepository/GetBuyerFactByPlant", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("FactRepository/GetBuyerFactByPlant", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public async Task<List<UserView>> HelpdeskUserByCompany(string Company)
        {
            try
            {
                List<UserView> user = new List<UserView>();
                string BaseAddress = _configuration.GetValue<string>("APIBaseAddress");
                string HostURI = BaseAddress + "/authenticationapi/Master/HelpdeskUserByCompany?Company=" + Company;
                var uri = new Uri(HostURI);
                var request = (HttpWebRequest)WebRequest.Create(uri);
                request.Method = "GET";
                try
                {
                    using (var response = (HttpWebResponse)await request.GetResponseAsync())
                    {
                        if (response != null && response.StatusCode == HttpStatusCode.OK)
                        {
                            var reader = new StreamReader(response.GetResponseStream());
                            string responseString = await reader.ReadToEndAsync();
                            user = JsonConvert.DeserializeObject<List<UserView>>(responseString);
                            reader.Close();
                            return user;
                        }
                        else
                        {
                            var reader = new StreamReader(response.GetResponseStream());
                            string responseString = await reader.ReadToEndAsync();
                            reader.Close();
                            return user;
                        }
                    }
                }
                catch (WebException ex)
                {
                    using (var stream = ex.Response.GetResponseStream())
                    using (var reader = new StreamReader(stream))
                    {
                        var errorMessage = reader.ReadToEnd();
                        throw ex;
                    }
                }
                catch (SqlException ex) { WriteLog.WriteToFile("FactRepository/HelpdeskUserByCompany", ex); throw new Exception("Something went wrong"); }
                catch (InvalidOperationException ex) { WriteLog.WriteToFile("FactRepository/HelpdeskUserByCompany", ex); throw new Exception("Something went wrong"); }
                catch (Exception ex)
                {
                    throw ex;
                }

            }
            catch (SqlException ex) { WriteLog.WriteToFile("FactRepository/HelpdeskUserByCompany", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("FactRepository/HelpdeskUserByCompany", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> SendMail(BPCFactSupport facts, string XmlFilePath, List<FTPAttachment> attachments, List<UserView> users)
        {
            try
            {
                var STMPDetailsConfig = _configuration.GetSection("STMPDetails");
                string hostName = STMPDetailsConfig["Host"];
                string SMTPEmail = STMPDetailsConfig["Email"];
                string SMTPEmailPassword = STMPDetailsConfig["Password"];
                string SMTPPort = STMPDetailsConfig["Port"];
                string siteURL = _configuration.GetValue<string>("SiteURL");

                var message = new MailMessage();
                string subject = "";
                StringBuilder sb = new StringBuilder();
                //UserName = string.IsNullOrEmpty(UserName) ? toEmail.Split('@')[0] : UserName;
                var body = @"<style>
                              h3 {
                               margin: 5px 0px !important;
                             }
                            </style>";
                body = body + $@"<html><head></head><body> <div style='border:1px solid #dbdbdb;'> <div style='padding: 20px 20px; background-color: #fff06769;text-align: center;font-family: Segoe UI;'> <p> 
                              <h2>Emami Vendor Portal</h2> </p> </div> <div style='background-color: #f8f7f7;padding: 20px 20px;font-family: Segoe UI'> <div style='padding: 20px 20px;border:1px solid white;background-color: white !important'> 
                              <p>Dear concern,</p> <p>The Fact Details for Vendor {facts.PatnerID} </p> 
                              <h3><u>Basic details</u></h3> <span>GST : </span>{facts.GSTNumber ?? ""}<br> 
                                                      <span>PAN : </span>{(facts.PANNumber ?? "")}<br> 
                                                      <span>Legal name : </span>{(facts.LegalName ?? "")}<br><br> 
                              <h3><u>Contact details</u></h3> <span>Phone : </span>{facts.Phone1 ?? ""}<br> 
                                                       <span>Email : </span>{(facts.Email1 ?? "")}<br> <br>
                              <h3><u>Address</u></h3> <span>Contry : </span>{facts.Country ?? ""}<br>
                                               <span>Pin code : </span>{(facts.PinCode ?? "")}<br> 
                                               <span>City : </span>{(facts.City ?? "")}<br> 
                                               <span>State : </span>{(facts.State ?? "")}<br>
                                               <span>Address line 1 : </span>{(facts.AddressLine1 ?? "")}<br>
                                               <span>Address line 2 : </span>{(facts.AddressLine2 ?? "")}<br><br>
                              <p>Regards,</p> <p>Admin</p> </div> </div> </div></body></html>";
                sb.Append(@body);
                subject = "Fact details updation";
                SmtpClient client = new SmtpClient();
                client.Port = Convert.ToInt32(SMTPPort);
                client.Host = hostName;
                client.EnableSsl = true;
                client.Timeout = 60000;
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.UseDefaultCredentials = false;
                client.Credentials = new System.Net.NetworkCredential(SMTPEmail, SMTPEmailPassword);
                //MailMessage reportEmail = new MailMessage(SMTPEmail, toEmail, subject, sb.ToString());
                MailMessage reportEmail = new MailMessage();
                reportEmail.From = new MailAddress(SMTPEmail);
                //reportEmail.To.Add(buyerId);
                var Emails = users.Select(x => x.Email).Distinct().ToList();
                foreach (var email in Emails)
                {
                    reportEmail.To.Add(email);
                }

                Stream stream1 = new MemoryStream(File.ReadAllBytes(XmlFilePath));
                var attachment1 = new System.Net.Mail.Attachment(stream1, Path.GetFileName(XmlFilePath), "application/xml");
                reportEmail.Attachments.Add(attachment1);

                foreach (var attach in attachments)
                {
                    Stream stream = new MemoryStream(attach.AttachmentFile);
                    var attachment = new System.Net.Mail.Attachment(stream, attach.AttachmentName, attach.ContentType);
                    reportEmail.Attachments.Add(attachment);
                }
                reportEmail.Subject = subject;
                reportEmail.Body = sb.ToString();
                reportEmail.BodyEncoding = UTF8Encoding.UTF8;
                reportEmail.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;
                reportEmail.IsBodyHtml = true;
                ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, sslPolicyErrors) => true;
                await client.SendMailAsync(reportEmail);
                WriteLog.WriteToFile($"FactRepository Fact updation Details sent successfully to {string.Join(",", Emails)}");
                return true;
            }
            catch (SmtpFailedRecipientsException ex)
            {
                for (int i = 0; i < ex.InnerExceptions.Length; i++)
                {
                    SmtpStatusCode status = ex.InnerExceptions[i].StatusCode;
                    if (status == SmtpStatusCode.MailboxBusy ||
                        status == SmtpStatusCode.MailboxUnavailable)
                    {
                        WriteLog.WriteToFile("FactRepository/SendMail/MailboxBusy/MailboxUnavailable/SmtpFailedRecipientsException:Inner- " + ex.InnerExceptions[i].Message);
                    }
                    else
                    {
                        WriteLog.WriteToFile("FactRepository/SendMail/SmtpFailedRecipientsException:Inner- " + ex.InnerExceptions[i].Message);
                    }
                }
                WriteLog.WriteToFile("FactRepository/SendMail/SmtpFailedRecipientsException:- " + ex.Message, ex);
                return false;
            }
            catch (SmtpException ex)
            {
                WriteLog.WriteToFile("FactRepository/SendMail/SmtpException:- " + ex.Message, ex);
                return false;
            }
            catch (SqlException ex) { WriteLog.WriteToFile("FactRepository/SendMail", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("FactRepository/SendMail", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("FactRepository/SendMail/Exception:- " + ex.Message, ex);
                return false;
            }
        }
        public async Task<bool> SendMail(BPCFactSupport facts, List<BPCAttachments> attachments, List<UserView> users)
        {
            try
            {
                var STMPDetailsConfig = _configuration.GetSection("STMPDetails");
                string hostName = STMPDetailsConfig["Host"];
                string SMTPEmail = STMPDetailsConfig["Email"];
                string SMTPEmailPassword = STMPDetailsConfig["Password"];
                string SMTPPort = STMPDetailsConfig["Port"];
                string siteURL = _configuration.GetValue<string>("SiteURL");

                var message = new MailMessage();
                string subject = "";
                StringBuilder sb = new StringBuilder();
                //UserName = string.IsNullOrEmpty(UserName) ? toEmail.Split('@')[0] : UserName;
                var body = @"<style>
                              h3 {
                               margin: 5px 0px !important;
                             }
                            </style>";


                body = body + $@"<html><head></head><body> <div style='border:1px solid #dbdbdb;'> <div style='padding: 20px 20px; background-color: #fff06769;text-align: center;font-family: Segoe UI;'> <p> 
                              <h2>Emami Vendor Portal</h2> </p> </div> <div style='background-color: #f8f7f7;padding: 20px 20px;font-family: Segoe UI'> <div style='padding: 20px 20px;border:1px solid white;background-color: white !important'> 
                              <p>Dear concern,</p> <p>The Decleration Details for Vendor {facts.PatnerID} </p> 
                              <h3><u>MSME</u></h3> <span>MSME : </span>{facts.MSME}<br> <span>MSME_TYPE : </span>{(facts.MSME_TYPE != null ? facts.MSME_TYPE : "")}<br> 
                                            <span>MSME_Att_ID : </span>{(facts.MSME_Att_ID != null ? facts.MSME_Att_ID : "")}<br><br>  
                              <h3><u>RP</u></h3> <span>RP : </span>{facts.RP}<br> 
                                          <span>RP_Name : </span>{(facts.RP_Name != null ? facts.RP_Name : "")}<br> 
                                          <span>RP_Type : </span>{(facts.RP_Type != null ? facts.RP_Type : "")}<br> 
                                          <span>RP_Att_ID : </span>{(facts.RP_Att_ID != null ? facts.RP_Att_ID : "")}<br><br> 
                              <h3><u>TDS</u></h3> <span>Reduced_TDS : </span>{facts.Reduced_TDS}<br>
                                           <span>TDS_RATE : </span>{(facts.TDS_RATE != null ? facts.TDS_RATE : "")}<br> 
                                           <span>TDS_Cert_No : </span>{(facts.TDS_Cert_No != null ? facts.TDS_Cert_No : "")}<br> 
                                           <span>TDS_Att_ID : </span>{(facts.TDS_Att_ID != null ? facts.TDS_Att_ID : "")}<br><br> 
                             <p>Regards,</p> <p>Admin</p> </div> </div> </div></body></html>";
                sb.Append(@body);
                subject = "Declaration";
                SmtpClient client = new SmtpClient();
                client.Port = Convert.ToInt32(SMTPPort);
                client.Host = hostName;
                client.EnableSsl = true;
                client.Timeout = 60000;
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.UseDefaultCredentials = false;
                client.Credentials = new System.Net.NetworkCredential(SMTPEmail, SMTPEmailPassword);
                //MailMessage reportEmail = new MailMessage(SMTPEmail, toEmail, subject, sb.ToString());
                MailMessage reportEmail = new MailMessage();
                reportEmail.From = new MailAddress(SMTPEmail);
                var Emails = users.Select(x => x.Email).Distinct().ToList();
                foreach (var email in Emails)
                {
                    reportEmail.To.Add(email);
                }


                foreach (var attach in attachments)
                {
                    Stream stream = new MemoryStream(attach.AttachmentFile);
                    var attachment = new System.Net.Mail.Attachment(stream, attach.AttachmentName, attach.ContentType);
                    reportEmail.Attachments.Add(attachment);
                }
                reportEmail.Subject = subject;
                reportEmail.Body = sb.ToString();
                reportEmail.BodyEncoding = UTF8Encoding.UTF8;
                reportEmail.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;
                reportEmail.IsBodyHtml = true;
                ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, sslPolicyErrors) => true;
                await client.SendMailAsync(reportEmail);
                WriteLog.WriteToFile($"Decleration Details sent successfully to {string.Join(",", Emails)}");
                return true;
            }
            catch (SmtpFailedRecipientsException ex)
            {
                for (int i = 0; i < ex.InnerExceptions.Length; i++)
                {
                    SmtpStatusCode status = ex.InnerExceptions[i].StatusCode;
                    if (status == SmtpStatusCode.MailboxBusy ||
                        status == SmtpStatusCode.MailboxUnavailable)
                    {
                        WriteLog.WriteToFile("FactRepository/SendMail/MailboxBusy/MailboxUnavailable/SmtpFailedRecipientsException:Inner- " + ex.InnerExceptions[i].Message);
                    }
                    else
                    {
                        WriteLog.WriteToFile("FactRepository/SendMail/SmtpFailedRecipientsException:Inner- " + ex.InnerExceptions[i].Message);
                    }
                }
                WriteLog.WriteToFile("FactRepository/SendMail/SmtpFailedRecipientsException:- " + ex.Message, ex);
                return false;
            }
            catch (SmtpException ex)
            {
                WriteLog.WriteToFile("FactRepository/SendMail/SmtpException:- " + ex.Message, ex);
                return false;
            }
            catch (SqlException ex) { WriteLog.WriteToFile("FactRepository/SendMail", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("FactRepository/SendMail", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("FactRepository/SendMail/Exception:- " + ex.Message, ex);
                return false;
            }
        }
        #region BPCFactAttachments
        public async Task<List<BPCAttach>> UpdateAttachment(List<BPCAttachments> Attachment, List<string> catogery)
        {
            var log = new BPCLog();
            BPCAttachments BPCAttachs = new BPCAttachments();
            try
            {
                List<BPCAttach> Attachments = new List<BPCAttach>();

                var i = 0;
                foreach (BPCAttachments Attach in Attachment)
                {
                    BPCAttachments Attachmentobj = new BPCAttachments();
                    // var ISattachment = _dbContext.BPCAttachments.Where(x => x.Client == Attach.Client && x.AttachmentID == Attach.AttachmentID).FirstOrDefault();

                    //if (ISattachment != null)
                    //{
                    //    ISattachment.ReferenceNo = Attach.ReferenceNo;
                    //    ISattachment.AttachmentName = Attach.AttachmentName;
                    //    ISattachment.ContentType = Attach.ContentType;
                    //    ISattachment.ContentLength = Attach.ContentLength;
                    //    ISattachment.AttachmentFile = Attach.AttachmentFile;
                    //    ISattachment.ModifiedOn = Attach.ModifiedOn;
                    //    ISattachment.ModifiedBy = Attach.ModifiedBy;

                    //    await _dbContext.SaveChangesAsync();
                    //    BPCAttach Attach_new = new BPCAttach();
                    //    Attach_new.AttachmentID = ISattachment.AttachmentID;
                    //    Attach_new.AttachmentName = Attach.AttachmentName;
                    //    Attach_new.Catogery = catogery[i];
                    //    Attachments.Add(Attach_new);
                    //}
                    //else
                    //{
                    Attachmentobj.Client = Attach.Client;
                    Attachmentobj.Company = Attach.Company;
                    Attachmentobj.Type = Attach.Type;
                    Attachmentobj.PatnerID = Attach.PatnerID;
                    Attachmentobj.ReferenceNo = Attach.ReferenceNo;
                    Attachmentobj.AttachmentName = Attach.AttachmentName;
                    Attachmentobj.ContentType = Attach.ContentType;
                    Attachmentobj.ContentLength = Attach.ContentLength;
                    Attachmentobj.AttachmentFile = Attach.AttachmentFile;
                    Attachmentobj.CreatedOn = DateTime.Now;
                    Attachmentobj.CreatedBy = Attach.CreatedBy;
                    Attachmentobj.IsActive = true;
                    Attachmentobj.ModifiedOn = Attach.ModifiedOn;
                    Attachmentobj.ModifiedBy = Attach.ModifiedBy;
                    var response = _dbContext.BPCAttachments.Add(Attachmentobj);
                    BPCAttachs = response.Entity;
                    await _dbContext.SaveChangesAsync();

                    BPCAttach Attach_new = new BPCAttach();
                    Attach_new.AttachmentID = BPCAttachs.AttachmentID;
                    Attach_new.AttachmentName = BPCAttachs.AttachmentName;
                    Attach_new.Catogery = catogery[i];
                    Attachments.Add(Attach_new);
                    //}
                    i++;
                }
                log = await CreateBPCLog("UpdateFact", 1);


                if (log != null)
                {
                    await UpdateBPCSucessLog(log.LogID);
                }
                else
                {
                    WriteLog.WriteToFile("Fact/UpdateFact--" + "Unable to generate Log");
                }
                return Attachments;
            }
            catch (SqlException ex) { WriteLog.WriteToFile("FactRepository/", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("FactRepository/", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                if (log != null)
                {
                    await UpdateBPCFailureLog(log.LogID, ex.Message);
                }
                else
                {
                    WriteLog.WriteToFile("Fact/UpdateFact--" + "Unable to generate Log");
                }
                throw ex;
            }
        }
        #endregion

        #region GettAttchmentByAttId
        public BPCAttachments GettAttchmentByAttId(int AttachmentID)
        {
            try
            {
                var result = _dbContext.BPCAttachments.Where(x => x.AttachmentID == AttachmentID).FirstOrDefault();

                return result;
            }
            catch (SqlException ex) { WriteLog.WriteToFile("FactRepository/SendMail", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("FactRepository/SendMail", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                throw ex;
            }

        }


        #endregion
        public async Task<BPCFact> UpdateFact(BPCFactView FactView)
        {
            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                try
                {
                    var Fact = _dbContext.Set<BPCFact>().FirstOrDefault(x => x.PatnerID == FactView.PatnerID);
                    if (Fact == null)
                    {
                        return Fact;
                    }
                    //_dbContext.Entry(Fact).State = FactState.Modified;
                    Fact.Client = FactView.Client;
                    Fact.Company = FactView.Company;
                    Fact.Type = FactView.Type;
                    //Fact.PatnerID = FactView.PatnerID;
                    Fact.LegalName = FactView.LegalName;
                    Fact.AddressLine1 = FactView.AddressLine1;
                    Fact.AddressLine2 = FactView.AddressLine2;
                    Fact.City = FactView.City;
                    Fact.State = FactView.State;
                    Fact.Country = FactView.Country;
                    Fact.PinCode = FactView.PinCode;
                    Fact.Type = FactView.Type;
                    Fact.Phone1 = FactView.Phone1;
                    Fact.Phone2 = FactView.Phone2;
                    Fact.Email1 = FactView.Email1;
                    Fact.Email2 = FactView.Email2;
                    Fact.TaxID1 = FactView.TaxID1;
                    Fact.TaxID2 = FactView.TaxID2;
                    Fact.OutstandingAmount = FactView.OutstandingAmount;
                    Fact.CreditAmount = FactView.CreditAmount;
                    Fact.LastPayment = FactView.LastPayment;
                    Fact.LastPaymentDate = FactView.LastPaymentDate;
                    Fact.Currency = FactView.Currency;
                    Fact.CreditLimit = FactView.CreditLimit;
                    Fact.CreditBalance = FactView.CreditBalance;
                    Fact.CreditEvalDate = FactView.CreditEvalDate;



                    Fact.ModifiedBy = FactView.ModifiedBy;
                    Fact.ModifiedOn = DateTime.Now;
                    Fact.GSTNumber = FactView.GSTNumber;
                    Fact.GSTStatus = FactView.GSTStatus;
                    Fact.PANNumber = FactView.PANNumber;
                    Fact.Role = FactView.Role;
                    Fact.PurchaseOrg = FactView.PurchaseOrg;
                    Fact.AccountGroup = FactView.AccountGroup;
                    Fact.CompanyCode = FactView.CompanyCode;
                    Fact.TypeofIndustry = FactView.TypeofIndustry;
                    Fact.VendorCode = FactView.VendorCode;
                    await _dbContext.SaveChangesAsync();

                    //IdentityRepository identityRepository = new IdentityRepository(_dbContext);
                    //await identityRepository.DeleteIdentityByTransID(FactView.TransID);
                    //await identityRepository.CreateIdentities(FactView.bPIdentities, FactView.TransID);

                    //BankRepository BankRepository = new BankRepository(_dbContext);
                    //await BankRepository.DeleteBankByTransID(FactView.TransID);
                    //await BankRepository.CreateBanks(FactView.BPCFactBanks, FactView.TransID);

                    //ContactRepository ContactRepository = new ContactRepository(_dbContext);
                    //await ContactRepository.DeleteContactByTransID(FactView.TransID);
                    //await ContactRepository.CreateContacts(FactView.bPContacts, FactView.TransID);

                    //ActivityLogRepository ActivityLogRepository = new ActivityLogRepository(_dbContext);
                    //await ActivityLogRepository.DeleteActivityLogByTransID(FactView.TransID);
                    //await ActivityLogRepository.CreateActivityLogs(FactView.bPActivityLogs, FactView.TransID);

                    transaction.Commit();
                    transaction.Dispose();
                    return Fact;
                }
                catch (SqlException ex) { WriteLog.WriteToFile("FactRepository/UpdateFact", ex); throw new Exception("Something went wrong"); }
                catch (InvalidOperationException ex) { WriteLog.WriteToFile("FactRepository/UpdateFact", ex); throw new Exception("Something went wrong"); }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    transaction.Dispose();
                    throw ex;
                }
            }
        }
        public async Task UpdateFactSupportDataToMasterData(string partnerId)
        {
            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                try
                {
                    var Fact = _dbContext.BPCFactsSupport.Where(x => x.PatnerID == partnerId).FirstOrDefault();
                    var Factbank = _dbContext.BPCFactBanksSupport.Where(x => x.PatnerID == partnerId).ToList();
                    var FactCertificate = _dbContext.BPCCertificatesSupport.Where(x => x.PatnerID == partnerId).ToList();
                    var index = 0;
                    var count = 0;
                    //Fact
                    if (Fact != null)
                    {
                        var BPCFacts = _dbContext.BPCFacts.Where(x => x.PatnerID == partnerId).FirstOrDefault();
                        BPCFacts.Client = Fact.Client;
                        BPCFacts.Company = Fact.Company;
                        BPCFacts.Type = Fact.Type;
                        BPCFacts.LegalName = Fact.LegalName;
                        BPCFacts.AddressLine1 = Fact.AddressLine1;
                        BPCFacts.AddressLine2 = Fact.AddressLine2;
                        BPCFacts.City = Fact.City;
                        BPCFacts.State = Fact.State;
                        BPCFacts.Country = Fact.Country;
                        BPCFacts.PinCode = Fact.PinCode;
                        BPCFacts.Type = Fact.Type;
                        BPCFacts.Phone1 = Fact.Phone1;
                        BPCFacts.Phone2 = Fact.Phone2;
                        BPCFacts.Email1 = Fact.Email1;
                        BPCFacts.Email2 = Fact.Email2;
                        BPCFacts.ModifiedBy = Fact.ModifiedBy;
                        BPCFacts.ModifiedOn = Fact.ModifiedOn;
                        BPCFacts.GSTNumber = Fact.GSTNumber;
                        BPCFacts.GSTStatus = Fact.GSTStatus;


                        BPCFacts.MSME = Fact.MSME;
                        BPCFacts.MSME_TYPE = Fact.MSME_TYPE;
                        BPCFacts.MSME_Att_ID = Fact.MSME_Att_ID;
                        BPCFacts.Reduced_TDS = Fact.Reduced_TDS;
                        BPCFacts.TDS_RATE = Fact.TDS_RATE;
                        BPCFacts.TDS_Att_ID = Fact.TDS_Att_ID;
                        BPCFacts.TDS_Cert_No = Fact.TDS_Cert_No;
                        BPCFacts.RP = Fact.RP;
                        BPCFacts.RP_Name = Fact.RP_Name;
                        BPCFacts.RP_Type = Fact.RP_Type;
                        BPCFacts.RP_Att_ID = Fact.RP_Att_ID;

                        BPCFacts.PANNumber = Fact.PANNumber;
                        BPCFacts.Role = Fact.Role;
                        BPCFacts.PurchaseOrg = Fact.PurchaseOrg;
                        BPCFacts.AccountGroup = Fact.AccountGroup;
                        BPCFacts.CompanyCode = Fact.CompanyCode;
                        BPCFacts.TypeofIndustry = Fact.TypeofIndustry;
                        BPCFacts.VendorCode = Fact.VendorCode;
                        _dbContext.BPCFactsSupport.Remove(Fact);
                        await _dbContext.SaveChangesAsync();
                    }
                    ////Bank
                    //var BPCFactBank = _dbContext.BPCFactBanks.Where(x => x.PatnerID == partnerId).ToList();
                    //while (index < Factbank.Count)
                    //{
                    //    for (var i = 0; i < BPCFactBank.Count; i++)
                    //    {
                    //        if (Factbank[index].AccountNo == BPCFactBank[i].AccountNo)
                    //        {
                    //            BPCFactBank[i].Name = Factbank[index].Name;
                    //            BPCFactBank[i].AccountNo = Factbank[index].AccountNo;
                    //            BPCFactBank[i].BankID = Factbank[index].BankID;
                    //            BPCFactBank[i].BankName = Factbank[index].BankName;
                    //            count++;
                    //        }
                    //    }
                    //    if (count == 0)
                    //    {
                    //        var bank = JsonConvert.DeserializeObject<BPCFactBank>(JsonConvert.SerializeObject(Factbank[index]));
                    //        _dbContext.BPCFactBanks.Add(bank);
                    //        count = 0;
                    //    }
                    //    index++;
                    //}
                    //_dbContext.BPCFactBanksSupport.RemoveRange(Factbank);
                    //await _dbContext.SaveChangesAsync();
                    ////Certificate
                    //count = 0;
                    //index = 0;
                    //var BPCFactCertificate = _dbContext.BPCCertificates.Where(x => x.PatnerID == partnerId).ToList();
                    //while (index < FactCertificate.Count)
                    //{
                    //    for (var i = 0; i < BPCFactCertificate.Count; i++)
                    //    {
                    //        if (FactCertificate[index].CertificateName == BPCFactCertificate[i].CertificateName)
                    //        {
                    //            BPCFactCertificate[i].CertificateName = FactCertificate[index].CertificateName;
                    //            BPCFactCertificate[i].CertificateType = FactCertificate[index].CertificateType;
                    //            BPCFactCertificate[i].Validity = FactCertificate[index].Validity;
                    //            BPCFactCertificate[i].Attachment = FactCertificate[index].Attachment;
                    //            BPCFactCertificate[i].AttachmentFile = FactCertificate[index].AttachmentFile;
                    //            count++;
                    //        }
                    //    }
                    //    if (count == 0)
                    //    {
                    //        var Certificate = JsonConvert.DeserializeObject<BPCCertificate>(JsonConvert.SerializeObject(FactCertificate[index]));
                    //        _dbContext.BPCCertificates.Add(Certificate);
                    //        count = 0;
                    //    }
                    //    index++;
                    //}
                    //_dbContext.BPCCertificatesSupport.RemoveRange(FactCertificate);

                    //Bank
                    if (Factbank.Count > 0)
                    {
                        foreach (BPCFactBankSupport bank in Factbank)
                        {
                            _dbContext.BPCFactBanksSupport.Remove(bank);
                            var banks = _dbContext.BPCFactBanks.Where(x => x.PatnerID == bank.PatnerID).FirstOrDefault();
                            if (banks != null)
                            {
                                _dbContext.BPCFactBanks.Remove(banks);

                            }
                        }
                    }
                    foreach (BPCFactBankSupport bank in Factbank)
                    {
                        bank.CreatedOn = DateTime.Now;
                        bank.IsActive = true;
                        var banks = JsonConvert.DeserializeObject<BPCFactBank>(JsonConvert.SerializeObject(bank));
                        _dbContext.BPCFactBanks.Add(banks);
                    }
                    //certificate
                    if (FactCertificate.Count > 0)
                    {

                        _dbContext.BPCCertificatesSupport.RemoveRange(FactCertificate);

                        var cert = _dbContext.BPCCertificates.Where(x => x.PatnerID == partnerId).ToList();
                        _dbContext.BPCCertificates.RemoveRange(cert);
                    }

                    foreach (BPCCertificateSupport certificate in FactCertificate)
                    {
                        certificate.CreatedOn = DateTime.Now;
                        certificate.IsActive = true;
                        var Certificate = JsonConvert.DeserializeObject<BPCCertificate>(JsonConvert.SerializeObject(certificate));
                        _dbContext.BPCCertificates.Add(Certificate);
                    }
                    await _dbContext.SaveChangesAsync();
                    transaction.Commit();
                    transaction.Dispose();
                }
                catch (SqlException ex) { WriteLog.WriteToFile("FactRepository/UpdateFactSupportDataToMasterData", ex); throw new Exception("Something went wrong"); }
                catch (InvalidOperationException ex) { WriteLog.WriteToFile("FactRepository/UpdateFactSupportDataToMasterData", ex); throw new Exception("Something went wrong"); }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    transaction.Dispose();
                    throw ex;
                }
            }
        }
        public async Task<BPCFact> DeleteFact(BPCFact Fact)
        {
            try
            {
                //var entity = await _dbContext.Set<BPCFact>().FindAsync(Fact.Fact, Fact.Language);
                var entity = _dbContext.Set<BPCFact>().FirstOrDefault(x => x.PatnerID == Fact.PatnerID);
                if (entity == null)
                {
                    return entity;
                }

                _dbContext.Set<BPCFact>().Remove(entity);
                await _dbContext.SaveChangesAsync();
                return entity;
            }
            catch (SqlException ex) { WriteLog.WriteToFile("FactRepository/DeleteFact", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("FactRepository/DeleteFact", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> CreateVendorUser(VendorUser vendorUser)
        {
            var log = new BPCLog();
            try
            {
                log = await CreateBPCLog("CreateVendorUser", 1);
                WriteLog.WriteToFile($"FactRepository/CreateVendorUser:- Create vendor user called for {vendorUser.UserName}");
                string BaseAddress = _configuration.GetValue<string>("APIBaseAddress");
                string HostURI = BaseAddress + "/authenticationapi/Master/CreateVendorUser";
                var uri = new Uri(HostURI);
                var request = (HttpWebRequest)WebRequest.Create(uri);
                request.Method = "POST";
                request.ContentType = "application/json";
                var SerializedObject = JsonConvert.SerializeObject(vendorUser);
                byte[] requestBody = Encoding.UTF8.GetBytes(SerializedObject);

                using (var postStream = await request.GetRequestStreamAsync())
                {
                    await postStream.WriteAsync(requestBody, 0, requestBody.Length);
                }

                try
                {
                    using (var response = (HttpWebResponse)await request.GetResponseAsync())
                    {
                        if (response != null && response.StatusCode == HttpStatusCode.OK)
                        {
                            var reader = new StreamReader(response.GetResponseStream());
                            string responseString = await reader.ReadToEndAsync();
                            reader.Close();
                            await UpdateBPCSucessLog(log.LogID);
                            return true;
                        }
                        else
                        {
                            var reader = new StreamReader(response.GetResponseStream());
                            string responseString = await reader.ReadToEndAsync();
                            reader.Close();
                            await UpdateBPCFailureLog(log.LogID, "False");
                            return false;
                        }
                    }
                }
                catch (WebException ex)
                {
                    using (var stream = ex.Response.GetResponseStream())
                    using (var reader = new StreamReader(stream))
                    {
                        var errorMessage = reader.ReadToEnd();
                        await UpdateBPCFailureLog(log.LogID, ex.Message);
                        if (!string.IsNullOrEmpty(errorMessage))
                        {
                            await UpdateBPCFailureLog(log.LogID, errorMessage);
                            //throw new Exception(errorMessage);
                            WriteLog.WriteToFile($"FactRepository/CreateVendorUser:- Error {errorMessage} for {vendorUser.UserName}");
                            return false;
                        }
                        //throw ex;
                        WriteLog.WriteToFile($"FactRepository/CreateVendorUser:- Error {ex.Message} for {vendorUser.UserName}");
                        return false;
                    }
                }
                catch (SqlException ex) { WriteLog.WriteToFile("FactRepository/CreateVendorUser", ex); throw new Exception("Something went wrong"); }
                catch (InvalidOperationException ex) { WriteLog.WriteToFile("FactRepository/CreateVendorUser", ex); throw new Exception("Something went wrong"); }
                catch (Exception ex)
                {
                    await UpdateBPCFailureLog(log.LogID, ex.Message);
                    //throw ex;
                    WriteLog.WriteToFile($"FactRepository/CreateVendorUser:- Error {ex.Message} for {vendorUser.UserName}");
                    return false;
                }

            }
            catch (SqlException ex) { WriteLog.WriteToFile("FactRepository/CreateVendorUser", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("FactRepository/CreateVendorUser", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                await UpdateBPCFailureLog(log.LogID, ex.Message);
                //throw ex; 
                WriteLog.WriteToFile($"FactRepository/CreateVendorUser:- Error {ex.Message} for {vendorUser.UserName}");
                return false;
            }
        }



        #region Dashboard 

        public DashboardGraphStatus GetDashboardGraphStatus(string PartnerID)
        {
            try
            {

                DashboardGraphStatus dashboardGraphStatus = new DashboardGraphStatus();
                FulfilmentStatus fulfilmentStatus = new FulfilmentStatus();
                Deliverystatus deliverystatus = new Deliverystatus();
                var entity = (from tb in _dbContext.BPCKRAs
                              where tb.PatnerID == PartnerID && tb.KRA == "01" && tb.IsActive == true
                              select new OTIFStatus
                              {
                                  OTIF = tb.KRAValue
                              }).FirstOrDefault();
                var entity1 = (from tb in _dbContext.BPCKRAs
                               where tb.PatnerID == PartnerID && tb.KRA == "02" && tb.IsActive == true
                               select new QualityStatus
                               {
                                   Quality = tb.KRAValue
                               }).FirstOrDefault();
                var entity2 = (from tb in _dbContext.BPCKRAs
                               where tb.PatnerID == PartnerID && (tb.KRA == "Open" || tb.KRA == "Scheduled" || tb.KRA == "InProgress" || tb.KRA == "Pending") && tb.IsActive == true
                               select tb).ToList();
                if (entity2.Count > 0)
                {
                    FulfilmentDetails OpenDetails = new FulfilmentDetails();
                    FulfilmentDetails ScheduledDetails = new FulfilmentDetails();
                    FulfilmentDetails InProgressDetails = new FulfilmentDetails();
                    FulfilmentDetails PendingDetails = new FulfilmentDetails();
                    //FulfilmentStatus fulfilmentStatus = new FulfilmentStatus();
                    foreach (var item in entity2)
                    {

                        if (item.KRA == "Open")
                        {
                            OpenDetails.Name = item.KRA;
                            OpenDetails.Value = item.KRAValue;
                            OpenDetails.label = item.KRAValue;
                        }
                        else if (item.KRA == "Scheduled")
                        {
                            ScheduledDetails.Name = item.KRA;
                            ScheduledDetails.Value = item.KRAValue;
                            ScheduledDetails.label = item.KRAValue;
                        }
                        else if (item.KRA == "InProgress")
                        {
                            InProgressDetails.Name = item.KRA;
                            InProgressDetails.Value = item.KRAValue;
                            InProgressDetails.label = item.KRAValue;
                        }
                        else if (item.KRA == "Pending")
                        {
                            PendingDetails.Name = item.KRA;
                            PendingDetails.Value = item.KRAValue;
                            PendingDetails.label = item.KRAValue;
                        }
                        fulfilmentStatus.OpenDetails = OpenDetails;
                        fulfilmentStatus.ScheduledDetails = ScheduledDetails;
                        fulfilmentStatus.InProgressDetails = InProgressDetails;
                        fulfilmentStatus.PendingDetails = PendingDetails;
                    }
                }

                var entity3 = (from tb in _dbContext.BPCKRAs
                               where tb.PatnerID == PartnerID && (tb.KRAText == "5-Planed" || tb.KRAText == "5-Actual" || tb.KRAText == "4-Planed" || tb.KRAText == "4-Actual" || tb.KRAText == "3-Planed" || tb.KRAText == "3-Actual" ||
                               tb.KRAText == "2-Planed" || tb.KRAText == "2-Actual" || tb.KRAText == "1-Planed" || tb.KRAText == "1-Actual") && tb.IsActive == true
                               orderby tb.EvalDate descending
                               select tb).Take(10).ToList();
                if (entity3.Count > 0)
                {
                    DeliverystatusDetails Planned1 = new DeliverystatusDetails();
                    DeliverystatusDetails Planned2 = new DeliverystatusDetails();
                    DeliverystatusDetails Planned3 = new DeliverystatusDetails();
                    DeliverystatusDetails Planned4 = new DeliverystatusDetails();
                    DeliverystatusDetails Planned5 = new DeliverystatusDetails();
                    foreach (var item in entity3)
                    {

                        if (item.KRAText == "1-Planed")
                        {
                            Planned1.Planned = item.KRAValue;
                            Planned1.Date = item.EvalDate;
                        }
                        if (item.KRAText == "1-Actual")
                        {
                            Planned1.Actual = item.KRAValue;
                            Planned1.Date = item.EvalDate;
                        }
                        if (item.KRAText == "2-Planed")
                        {
                            Planned2.Planned = item.KRAValue;
                            Planned2.Date = item.EvalDate;
                        }
                        if (item.KRAText == "2-Actual")
                        {
                            Planned2.Actual = item.KRAValue;
                            Planned2.Date = item.EvalDate;
                        }
                        if (item.KRAText == "3-Planed")
                        {
                            Planned3.Planned = item.KRAValue;
                            Planned3.Date = item.EvalDate;
                        }
                        if (item.KRAText == "3-Actual")
                        {
                            Planned3.Actual = item.KRAValue;
                            Planned3.Date = item.EvalDate;
                        }
                        if (item.KRAText == "4-Planed")
                        {
                            Planned4.Planned = item.KRAValue;
                            Planned4.Date = item.EvalDate;
                        }
                        if (item.KRAText == "4-Actual")
                        {
                            Planned4.Actual = item.KRAValue;
                            Planned4.Date = item.EvalDate;
                        }
                        if (item.KRAText == "5-Planed")
                        {
                            Planned5.Planned = item.KRAValue;
                            Planned5.Date = item.EvalDate;
                        }
                        if (item.KRAText == "5-Actual")
                        {
                            Planned5.Actual = item.KRAValue;
                            Planned5.Date = item.EvalDate;
                        }
                        deliverystatus.Planned1 = Planned1;
                        deliverystatus.Planned2 = Planned2;
                        deliverystatus.Planned3 = Planned3;
                        deliverystatus.Planned4 = Planned4;
                        deliverystatus.Planned5 = Planned5;
                    }
                }

                dashboardGraphStatus.oTIFStatus = entity;
                dashboardGraphStatus.qualityStatus = entity1;
                dashboardGraphStatus.fulfilmentStatus = fulfilmentStatus;
                dashboardGraphStatus.deliverystatus = deliverystatus;
                return dashboardGraphStatus;
            }
            catch (SqlException ex) { WriteLog.WriteToFile("FactRepository/GetDashboardGraphStatus", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("FactRepository/GetDashboardGraphStatus", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public BPCKRA GetVendorKRAProcessCircle(string PartnerID)
        {
            try
            {
                var result = (from tb in _dbContext.BPCKRAs
                              where tb.PatnerID == PartnerID && tb.IsActive && tb.KRA == "01"
                              select tb).FirstOrDefault();
                return result;

            }
            catch (SqlException ex) { WriteLog.WriteToFile("FactRepository/GetVendorKRAProcessCircle", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("FactRepository/GetVendorKRAProcessCircle", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        //public List<BPCKRA> GetVendorKRAProcessCircleForSuperUser(string PartnerIDforSuperUser)
        //{
        //    try
        //    {
        //        string[] PartnerID = PartnerIDforSuperUser.Split(',');
        //        for (int i = 0; i < PartnerID.Length; i++)
        //        {
        //            PartnerID[i] = PartnerID[i].Trim();
        //        }
        //        string[] PartnersIDs = PartnerID.Distinct().ToArray();
        //        List<BPCKRA> kra = new List<BPCKRA>();
        //        List<BPCKRA> kras = new List<BPCKRA>();
        //        for (int i = 0; i < PartnersIDs.Length; i++)
        //        {
        //            kra = _dbContext.BPCKRAs.Where(x => x.PatnerID == PartnersIDs[i] && x.IsActive && x.KRA == "01").ToList();
                           
        //            kras.AddRange(kra);
        //        }
        //        return kras;

        //    }
        //    catch (SqlException ex) { WriteLog.WriteToFile("FactRepository/GetVendorKRAProcessCircle", ex); throw new Exception("Something went wrong"); }
        //    catch (InvalidOperationException ex) { WriteLog.WriteToFile("FactRepository/GetVendorKRAProcessCircle", ex); throw new Exception("Something went wrong"); }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        public BPCKRA GetVendorPPMProcessCircle(string PartnerID)
        {
            try
            {
                var result = (from tb in _dbContext.BPCKRAs
                              where tb.PatnerID == PartnerID && tb.IsActive && tb.KRA == "02"
                              select tb).FirstOrDefault();
                return result;

            }
            catch (SqlException ex) { WriteLog.WriteToFile("FactRepository/GetVendorPPMProcessCircle", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("FactRepository/GetVendorPPMProcessCircle", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<BPCKRA> GetCustomerDoughnutChartData(string PartnerID)
        {
            try
            {
                var result = (from tb in _dbContext.BPCKRAs
                              where tb.PatnerID == PartnerID && tb.IsActive &&
                              (tb.KRAText == "Due For ACK" || tb.KRAText == "Scheduled" || tb.KRAText == "InProgress" || tb.KRAText == "Pending")
                              select tb).GroupBy(t => t.KRA).Select(t => t.FirstOrDefault()).ToList();
                return result;

            }
            catch (SqlException ex) { WriteLog.WriteToFile("FactRepository/GetCustomerDoughnutChartData", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("FactRepository/GetCustomerDoughnutChartData", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public BPCKRA GetCustomerOpenProcessCircle(string PartnerID)
        {
            try
            {
                var result = (from tb in _dbContext.BPCKRAs
                              where tb.PatnerID == PartnerID && tb.IsActive && tb.KRAText == "Due For ACK"
                              select tb).FirstOrDefault();
                return result;

            }
            catch (SqlException ex) { WriteLog.WriteToFile("FactRepository/GetCustomerOpenProcessCircle", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("FactRepository/GetCustomerOpenProcessCircle", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public BPCKRA GetCustomerCreditLimitProcessCircle(string PartnerID)
        {
            try
            {
                var result = (from tb in _dbContext.BPCKRAs
                              where tb.PatnerID == PartnerID && tb.IsActive && tb.KRAText == "Credit Limit"
                              select tb).FirstOrDefault();
                return result;

            }
            catch (SqlException ex) { WriteLog.WriteToFile("FactRepository/GetCustomerCreditLimitProcessCircle", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("FactRepository/GetCustomerCreditLimitProcessCircle", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public CustomerBarChartData GetCustomerBarChartData(string PartnerID)
        {
            try
            {
                var toDay = DateTime.Now;
                var fromDay = DateTime.Now.AddDays(-4);
                var result = (from tb in _dbContext.BPCKRAs
                              where tb.PatnerID == PartnerID && tb.IsActive &&
                              (tb.KRAText.Contains("Actual") || tb.KRAText.Contains("Planned")) && tb.EvalDate.HasValue &&
                              tb.EvalDate.Value.Date >= fromDay.Date && tb.EvalDate.Value.Date <= toDay.Date
                              orderby tb.EvalDate
                              select tb).GroupBy(x => x.EvalDate).ToList();
                CustomerBarChartData customerBarChartData = new CustomerBarChartData();
                customerBarChartData.BarChartLabels = new List<string>();
                customerBarChartData.ActualData = new List<string>();
                customerBarChartData.PlannedData = new List<string>();
                foreach (var group in result)
                {
                    customerBarChartData.BarChartLabels.Add(group.Key.Value.ToString("dd/MM/yyyy"));
                    customerBarChartData.ActualData.Add(group.Where(x => x.KRAText.Contains("Actual")).Select(x => x.KRAValue).FirstOrDefault());
                    customerBarChartData.PlannedData.Add(group.Where(x => x.KRAText.Contains("Planned")).Select(x => x.KRAValue).FirstOrDefault());
                }
                //customerBarChartData.BarChartLabels = result.GroupBy(x => x.EvalDate).Select(x => x.FirstOrDefault()).ToList().Select(x => x.EvalDate.Value.ToString("dd/MM/yyyy")).ToList();
                //customerBarChartData.ActualData = result.Where(x => x.KRA == "Actual").Select(x => x.KRAValue).ToList();
                //customerBarChartData.PlannedData = result.Where(x => x.KRA == "Planned").Select(x => x.KRAValue).ToList();
                return customerBarChartData;

            }
            catch (SqlException ex) { WriteLog.WriteToFile("FactRepository/GetCustomerBarChartData", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("FactRepository/GetCustomerBarChartData", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        public async Task DeleteFactByPartnerIDAndType(string PartnerID)
        {
            try
            {
                _dbContext.Set<BPCFact>().Where(x => x.PatnerID == PartnerID && x.Type == "Vendor").ToList().ForEach(x => _dbContext.Set<BPCFact>().Remove(x));
                await _dbContext.SaveChangesAsync();
            }
            catch (SqlException ex) { WriteLog.WriteToFile("FactRepository/DeleteFactByPartnerIDAndType", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("FactRepository/DeleteFactByPartnerIDAndType", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task CreateFacts(List<BPCFactXLSX> Facts)
        {
            try
            {
                if (Facts != null && Facts.Count > 0)
                {
                    foreach (BPCFactXLSX Fact in Facts)
                    {
                        //await DeleteFactByPartnerIDAndType(Fact.PatnerID);
                        BPCFact bPCFact = new BPCFact();
                        //mandatory fields start
                        bPCFact.PatnerID = Fact.Patnerid;
                        bPCFact.Client = Fact.Client;
                        bPCFact.Company = Fact.Company;
                        bPCFact.Type = Fact.Type;
                        bPCFact.LegalName = Fact.Legalname;
                        //mandatory fields end
                        //bPCFact.AddressLine1 = Fact.AddressLine1;
                        //bPCFact.AddressLine2 = Fact.AddressLine2;
                        bPCFact.City = Fact.City;
                        //bPCFact.State = Fact.State;
                        //bPCFact.Country = Fact.Country;
                        bPCFact.PinCode = Fact.Postal;
                        bPCFact.Phone1 = Fact.Phone1;
                        bPCFact.Phone2 = Fact.Phone2;
                        //bPCFact.Email1 = Fact.Email1;
                        //bPCFact.Email2 = Fact.Email2;
                        bPCFact.TaxID1 = Fact.Tax1;
                        bPCFact.TaxID2 = Fact.Tax2;
                        bPCFact.OutstandingAmount = Fact.Outstandingamount;
                        //bPCFact.CreditAmount = Fact.CreditAmount;
                        bPCFact.LastPayment = Fact.Lastpayment;
                        DateTime dateTime;
                        CultureInfo provider = CultureInfo.InvariantCulture;
                        bool isSuccess = DateTime.TryParseExact(Fact.LastPaymentdate, "yyyyMMdd", provider, DateTimeStyles.None, out dateTime);
                        if (isSuccess)
                            bPCFact.LastPaymentDate = dateTime;
                        bPCFact.Currency = Fact.Currency;
                        //bPCFact.CreditLimit = Fact.CreditLimit;
                        //bPCFact.CreditBalance = Fact.CreditBalance;
                        //bPCFact.CreditEvalDate = Fact.CreditEvalDate;
                        bPCFact.IsActive = true;
                        bPCFact.CreatedOn = DateTime.Now;
                        var result = _dbContext.BPCFacts.Add(bPCFact);
                    }
                    await _dbContext.SaveChangesAsync();
                }
            }
            catch (SqlException ex) { WriteLog.WriteToFile("FactRepository/CreateFacts", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("FactRepository/CreateFacts", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<BPCLog> CreateBPCLog(string APIMethod, int NoOfRecords)
        {
            //try
            //{
            //    BPCLog log = new BPCLog();
            //    log.LogDate = DateTime.Now;
            //    log.APIMethod = APIMethod;
            //    log.NoOfRecords = NoOfRecords;
            //    log.Status = "Initiated";
            //    log.CreatedOn = DateTime.Now;
            //    var result = _dbContext.BPCLogs.Add(log);
            //    await _dbContext.SaveChangesAsync();
            //    return result.Entity;
            //}
            //catch (SqlException ex){ WriteLog.WriteToFile("FactRepository/", ex); throw new Exception("Something went wrong");}catch(InvalidOperationException ex){WriteLog.WriteToFile("FactRepository/", ex); throw new Exception("Something went wrong"); }catch (Exception ex)
            //{
            //    throw ex;
            //}
            BPCLog log = new BPCLog();
            try
            {
                string BaseAddress = _configuration.GetValue<string>("APIBaseAddress");
                string HostURI = BaseAddress + "/poapi/PO/CreateBPCLog?APIMethod=" + APIMethod + "&NoOfRecords=" + NoOfRecords;
                var uri = new Uri(HostURI);
                var request = (HttpWebRequest)WebRequest.Create(uri);
                request.Method = "GET";
                try
                {
                    using (var response = (HttpWebResponse)await request.GetResponseAsync())
                    {
                        if (response != null && response.StatusCode == HttpStatusCode.OK)
                        {
                            var reader = new StreamReader(response.GetResponseStream());
                            string responseString = await reader.ReadToEndAsync();
                            reader.Close();
                            log = JsonConvert.DeserializeObject<BPCLog>(responseString);
                            return log;
                        }
                        else
                        {
                            var reader = new StreamReader(response.GetResponseStream());
                            string responseString = await reader.ReadToEndAsync();
                            reader.Close();
                            log = null;
                            return log;
                        }
                    }
                }
                catch (WebException ex)
                {
                    using (var stream = ex.Response.GetResponseStream())
                    using (var reader = new StreamReader(stream))
                    {
                        var errorMessage = reader.ReadToEnd();

                        throw ex;
                    }
                }
                catch (SqlException ex) { WriteLog.WriteToFile("FactRepository/CreateBPCLog", ex); throw new Exception("Something went wrong"); }
                catch (InvalidOperationException ex) { WriteLog.WriteToFile("FactRepository/CreateBPCLog", ex); throw new Exception("Something went wrong"); }
                catch (Exception ex)
                {

                    throw ex;
                }

            }
            catch (SqlException ex) { WriteLog.WriteToFile("FactRepository/CreateBPCLog", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("FactRepository/CreateBPCLog", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public async Task<BPCLog> UpdateBPCSucessLog(int LogID)
        {
            //try
            //{
            //    var result = _dbContext.BPCLogs.FirstOrDefault(x => x.LogID == LogID);
            //    if (result != null)
            //    {
            //        result.Status = "Success";
            //        result.Response = "Data are inserted successfully";
            //        result.ModifiedOn = DateTime.Now;
            //        await _dbContext.SaveChangesAsync();
            //    }
            //    return result;
            //}
            //catch (SqlException ex){ WriteLog.WriteToFile("FactRepository/", ex); throw new Exception("Something went wrong");}catch(InvalidOperationException ex){WriteLog.WriteToFile("FactRepository/", ex); throw new Exception("Something went wrong"); }catch (Exception ex)
            //{
            //    throw ex;
            //}


            BPCLog log = new BPCLog();
            try
            {
                string BaseAddress = _configuration.GetValue<string>("APIBaseAddress");
                string HostURI = BaseAddress + "/poapi/PO/UpdateBPCSucessLog?LogID=" + LogID;
                var uri = new Uri(HostURI);
                var request = (HttpWebRequest)WebRequest.Create(uri);
                request.Method = "GET";
                try
                {
                    using (var response = (HttpWebResponse)await request.GetResponseAsync())
                    {
                        if (response != null && response.StatusCode == HttpStatusCode.OK)
                        {
                            var reader = new StreamReader(response.GetResponseStream());
                            string responseString = await reader.ReadToEndAsync();
                            reader.Close();
                            log = JsonConvert.DeserializeObject<BPCLog>(responseString);
                            return log;
                        }
                        else
                        {
                            var reader = new StreamReader(response.GetResponseStream());
                            string responseString = await reader.ReadToEndAsync();
                            reader.Close();
                            log = null;
                            return log;
                        }
                    }
                }
                catch (WebException ex)
                {
                    using (var stream = ex.Response.GetResponseStream())
                    using (var reader = new StreamReader(stream))
                    {
                        var errorMessage = reader.ReadToEnd();

                        throw ex;
                    }
                }
                catch (SqlException ex) { WriteLog.WriteToFile("FactRepository/UpdateBPCSucessLog", ex); throw new Exception("Something went wrong"); }
                catch (InvalidOperationException ex) { WriteLog.WriteToFile("FactRepository/UpdateBPCSucessLog", ex); throw new Exception("Something went wrong"); }
                catch (Exception ex)
                {

                    throw ex;
                }

            }
            catch (SqlException ex) { WriteLog.WriteToFile("FactRepository/UpdateBPCSucessLog", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("FactRepository/UpdateBPCSucessLog", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public async Task<BPCLog> UpdateBPCFailureLog(int LogID, string ErrorMessage)
        {
            //try
            //{
            //    var result = _dbContext.BPCLogs.FirstOrDefault(x => x.LogID == LogID);
            //    if (result != null)
            //    {
            //        result.Status = "Failed";
            //        result.Response = ErrorMessage;
            //        result.ModifiedOn = DateTime.Now;
            //        await _dbContext.SaveChangesAsync();
            //    }
            //    return result;
            //}
            //catch (SqlException ex){ WriteLog.WriteToFile("FactRepository/", ex); throw new Exception("Something went wrong");}catch(InvalidOperationException ex){WriteLog.WriteToFile("FactRepository/", ex); throw new Exception("Something went wrong"); }catch (Exception ex)
            //{
            //    throw ex;
            //}

            BPCLog log = new BPCLog();
            try
            {
                string BaseAddress = _configuration.GetValue<string>("APIBaseAddress");
                string HostURI = BaseAddress + "/poapi/PO/UpdateBPCFailureLog?LogID=" + LogID + "&ErrorMessage=" + ErrorMessage;
                var uri = new Uri(HostURI);
                var request = (HttpWebRequest)WebRequest.Create(uri);
                request.Method = "GET";
                try
                {
                    using (var response = (HttpWebResponse)await request.GetResponseAsync())
                    {
                        if (response != null && response.StatusCode == HttpStatusCode.OK)
                        {
                            var reader = new StreamReader(response.GetResponseStream());
                            string responseString = await reader.ReadToEndAsync();
                            reader.Close();
                            log = JsonConvert.DeserializeObject<BPCLog>(responseString);
                            return log;
                        }
                        else
                        {
                            var reader = new StreamReader(response.GetResponseStream());
                            string responseString = await reader.ReadToEndAsync();
                            reader.Close();
                            log = null;
                            return log;
                        }
                    }
                }
                catch (WebException ex)
                {
                    using (var stream = ex.Response.GetResponseStream())
                    using (var reader = new StreamReader(stream))
                    {
                        var errorMessage = reader.ReadToEnd();
                        throw ex;
                    }
                }
                catch (SqlException ex) { WriteLog.WriteToFile("FactRepository/UpdateBPCFailureLog", ex); throw new Exception("Something went wrong"); }
                catch (InvalidOperationException ex) { WriteLog.WriteToFile("FactRepository/UpdateBPCFailureLog", ex); throw new Exception("Something went wrong"); }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            catch (SqlException ex) { WriteLog.WriteToFile("FactRepository/UpdateBPCFailureLog", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("FactRepository/UpdateBPCFailureLog", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<BPCFact> GetAllVendors()
        {
            try
            {
                return _dbContext.BPCFacts.Where(x => x.Type == "V").ToList();
            }
            catch (SqlException ex) { WriteLog.WriteToFile("FactRepository/GetAllVendors", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("FactRepository/GetAllVendors", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<BPCFact1> GetAllVendorNames()
        {
            try
            {
                var result = (from tb in _dbContext.BPCFacts
                              where tb.Type == "V"
                              select new BPCFact1()
                              {
                                  PatnerID = tb.PatnerID,
                                  LegalName = tb.LegalName,
                                  Name = tb.Name
                              }).GroupBy(x => x.PatnerID).Select(r => r.FirstOrDefault()).ToList();
                return result;
            }
            catch (SqlException ex) { WriteLog.WriteToFile("FactRepository/GetAllVendors", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("FactRepository/GetAllVendors", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public BPCFactSupport GetFactSupportDetails(string patnerID)
        {
            try
            {
                return _dbContext.BPCFactsSupport.Where(x => x.PatnerID == patnerID).FirstOrDefault();
            }
            catch (SqlException ex) { WriteLog.WriteToFile("FactRepository/GetFactSupportDetails", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("FactRepository/GetFactSupportDetails", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<FTPAttachment> GetCertificatesAttachments(string PatnerID, bool ISDecleration)
        {
            try
            {
                var Certificates = new List<FTPAttachment>();
                if (ISDecleration)
                {
                    Certificates = (from tb in _dbContext.BPCAttachments
                                    join tb1 in _dbContext.BPCCertificates
                                    on tb.AttachmentName equals tb1.Attachment
                                    where tb.PatnerID == tb1.PatnerID
                                    && tb1.AttachmentID.ToString() == tb.AttachmentID.ToString()
                                    select new FTPAttachment()
                                    {
                                        AttachmentID = tb.AttachmentID,
                                        PatnerID = PatnerID,
                                        AttachmentFile = tb.AttachmentFile,
                                        AttachmentName = tb.AttachmentName,
                                        ContentType = tb.ContentType,
                                        Type = tb1.CertificateType
                                    }).ToList();
                }
                else
                {
                    Certificates = (from tb in _dbContext.BPCAttachments
                                    join tb1 in _dbContext.BPCCertificatesSupport
                                    on tb.AttachmentName equals tb1.Attachment
                                    where tb.PatnerID == tb1.PatnerID
                                    && tb1.AttachmentID.ToString() == tb.AttachmentID.ToString()
                                    select new FTPAttachment()
                                    {
                                        AttachmentID = tb.AttachmentID,
                                        PatnerID = PatnerID,
                                        AttachmentFile = tb.AttachmentFile,
                                        AttachmentName = tb.AttachmentName,
                                        ContentType = tb.ContentType,
                                        Type = tb1.CertificateType
                                    }).ToList();
                }
                return Certificates;
            }
            catch (SqlException ex) { WriteLog.WriteToFile("FactRepository/GetCertificatesAttachments", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("FactRepository/GetCertificatesAttachments", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<FTPAttachment> GetAllAttachmentsToFTP(BPCFactSupport Fact, bool ISDecleration)
        {
            try
            {
                List<FTPAttachment> allAttachments = new List<FTPAttachment>();

                var Attachments = GetCertificatesAttachments(Fact.PatnerID, ISDecleration);
                allAttachments.AddRange(Attachments);

                var MSMEAttachmentID = 0;
                if (Fact.MSME_Att_ID != null && Fact.MSME_Att_ID != "")
                {
                    MSMEAttachmentID = int.Parse(Fact.MSME_Att_ID);
                }
                var MSMEAttachment = (from tb in _dbContext.BPCAttachments
                                      where tb.AttachmentID == MSMEAttachmentID && tb.PatnerID == Fact.PatnerID
                                      select new FTPAttachment()
                                      {
                                          AttachmentID = tb.AttachmentID,
                                          PatnerID = Fact.PatnerID,
                                          AttachmentFile = tb.AttachmentFile,
                                          AttachmentName = tb.AttachmentName,
                                          ContentType = tb.ContentType,
                                          Type = "MSME"
                                      }).ToList();

                allAttachments.AddRange(MSMEAttachment);


                var RPAttachmentID = 0;
                if (Fact.RP_Att_ID != null && Fact.RP_Att_ID != "")
                {
                    RPAttachmentID = int.Parse(Fact.RP_Att_ID);
                }
                var RPAttachment = (from tb in _dbContext.BPCAttachments
                                    where tb.AttachmentID == RPAttachmentID && tb.PatnerID == Fact.PatnerID
                                    select new FTPAttachment()
                                    {
                                        AttachmentID = tb.AttachmentID,
                                        PatnerID = Fact.PatnerID,
                                        AttachmentFile = tb.AttachmentFile,
                                        AttachmentName = tb.AttachmentName,
                                        ContentType = tb.ContentType,
                                        Type = "RP"

                                    }).ToList();

                allAttachments.AddRange(RPAttachment);


                var TDSAttachmentID = 0;
                if (Fact.TDS_Att_ID != null && Fact.TDS_Att_ID != "")
                {
                    TDSAttachmentID = int.Parse(Fact.TDS_Att_ID);
                }
                var TDSAttachment = (from tb in _dbContext.BPCAttachments
                                     where tb.AttachmentID == TDSAttachmentID && tb.PatnerID == Fact.PatnerID
                                     select new FTPAttachment()
                                     {
                                         AttachmentID = tb.AttachmentID,
                                         PatnerID = Fact.PatnerID,
                                         AttachmentFile = tb.AttachmentFile,
                                         AttachmentName = tb.AttachmentName,
                                         ContentType = tb.ContentType,
                                         Type = "TDS"

                                     }).ToList();

                allAttachments.AddRange(TDSAttachment);
                return allAttachments;
            }
            catch (SqlException ex) { WriteLog.WriteToFile("FactRepository/GetAllAttachmentsToFTP", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("FactRepository/GetAllAttachmentsToFTP", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
