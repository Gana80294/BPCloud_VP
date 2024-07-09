using BPCloud_VP.FactService.DBContexts;
using BPCloud_VP.FactService.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace BPCloud_VP.FactService.Repositories
{
    public class CertificateRepository : ICertificateRepository
    {
        private readonly FactContext _dbContext;

        public CertificateRepository(FactContext dbContext)
        {
            _dbContext = dbContext;
        }

        public List<BPCCertificate> GetAllCertificates()
        {
            try
            {
                return _dbContext.BPCCertificates.ToList();
            }
            catch (SqlException ex) { WriteLog.WriteToFile("CertificateRepository/GetAllCertificates", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("CertificateRepository/GetAllCertificates", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<BPCCertificate> GetCertificatesByPartnerID(string PartnerID)
        {
            try
            {
                var result = _dbContext.BPCCertificates.Where(x => x.PatnerID == PartnerID).ToList();
                return result;
            }
            catch (SqlException ex) { WriteLog.WriteToFile("CertificateRepository/GetCertificatesByPartnerID", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("CertificateRepository/GetCertificatesByPartnerID", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<BPCCertificate> CreateCertificate(BPCCertificate Certificate)
        {
            try
            {
                Certificate.IsActive = true;
                Certificate.CreatedOn = DateTime.Now;
                var result = _dbContext.BPCCertificates.Add(Certificate);
                await _dbContext.SaveChangesAsync();
                return Certificate;
            }
            catch (SqlException ex) { WriteLog.WriteToFile("CertificateRepository/CreateCertificate", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("CertificateRepository/CreateCertificate", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<BPCCertificateSupport> CreateCertificateSupport(BPCCertificateSupport Certificate)
        {
            try
            {
                Certificate.IsActive = true;
                Certificate.CreatedOn = DateTime.Now;
                var result = _dbContext.BPCCertificatesSupport.Add(Certificate);
                await _dbContext.SaveChangesAsync();
                return Certificate;
            }
            catch (SqlException ex) { WriteLog.WriteToFile("CertificateRepository/CreateCertificateSupport", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("CertificateRepository/CreateCertificateSupport", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task CreateCertificates(List<BPCCertificate> Certificates, string PatnerID)
        {
            try
            {
                foreach (BPCCertificate Certificate in Certificates)
                {
                    Certificate.PatnerID = PatnerID;
                    Certificate.IsActive = true;
                    Certificate.CreatedOn = DateTime.Now;
                    var result = _dbContext.BPCCertificates.Add(Certificate);
                }
                await _dbContext.SaveChangesAsync();
            }
            catch (SqlException ex) { WriteLog.WriteToFile("CertificateRepository/CreateCertificates", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("CertificateRepository/CreateCertificates", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<BPCCertificate> UpdateCertificate(BPCCertificate Certificate)
        {
            try
            {
                var entity = _dbContext.Set<BPCCertificate>().FirstOrDefault(x => x.PatnerID == Certificate.PatnerID && x.CertificateName == Certificate.CertificateName);
                if (entity == null)
                {
                    return entity;
                }
                //_dbContext.Entry(Certificate).State = EntityState.Modified;
                entity.CertificateName = Certificate.CertificateName;
                entity.CertificateType = Certificate.CertificateType;
                entity.Validity = Certificate.Validity;
                entity.Mandatory = Certificate.Mandatory;
                entity.Attachment = Certificate.Attachment;
                entity.ModifiedBy = Certificate.ModifiedBy;
                entity.ModifiedOn = DateTime.Now;
                await _dbContext.SaveChangesAsync();
                return Certificate;
            }
            catch (SqlException ex) { WriteLog.WriteToFile("CertificateRepository/UpdateCertificate", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("CertificateRepository/UpdateCertificate", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<BPCCertificate> DeleteCertificate(BPCCertificate certificate)
        {
            try
            {
                //var entity = await _dbContext.Set<BPCFact>().FindAsync(Fact.Fact, Fact.Language);
                var entity = _dbContext.Set<BPCCertificate>().FirstOrDefault(x => x.PatnerID == certificate.PatnerID);
                var CertificateSupport = _dbContext.BPCCertificatesSupport.Where(x => x.PatnerID == certificate.PatnerID && x.CertificateName == certificate.CertificateName).FirstOrDefault();
                if (entity == null)
                {
                    return entity;
                }
                if (CertificateSupport != null)
                {
                    _dbContext.BPCCertificatesSupport.Remove(CertificateSupport);
                }
                _dbContext.Set<BPCCertificate>().Remove(entity);
                await _dbContext.SaveChangesAsync();
                return entity;
            }
            catch (SqlException ex) { WriteLog.WriteToFile("CertificateRepository/DeleteCertificate", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("CertificateRepository/DeleteCertificate", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task DeleteCertificateByPartnerID(string PartnerID)
        {
            try
            {
                _dbContext.Set<BPCCertificate>().Where(x => x.PatnerID == PartnerID).ToList().ForEach(x => _dbContext.Set<BPCCertificate>().Remove(x));
                await _dbContext.SaveChangesAsync();
            }
            catch (SqlException ex) { WriteLog.WriteToFile("CertificateRepository/DeleteCertificateByPartnerID", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("CertificateRepository/DeleteCertificateByPartnerID", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<BPCCertificateSupport> AddAttachmentTOCertificate(BPCAttachment attachment)
        {
            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                try
                {
                    var entity = _dbContext.Set<BPCCertificateSupport>().FirstOrDefault(x => x.PatnerID == attachment.PatnerID && x.CertificateName == attachment.CertificateName);
                    if (entity != null)
                    {
                        BPCAttachments Attachments = new BPCAttachments();
                        Attachments.Client = attachment.Client;
                        Attachments.Company = attachment.Company;
                        Attachments.Type = attachment.Type;
                        Attachments.AttachmentName = attachment.AttachmentName;
                        Attachments.ContentLength = attachment.ContentLength;
                        Attachments.ContentType = attachment.ContentType;
                        Attachments.AttachmentFile = attachment.AttachmentFile;
                        Attachments.PatnerID = attachment.PatnerID;
                        Attachments.CreatedOn = DateTime.Now;
                        var result = _dbContext.BPCAttachments.Add(Attachments);
                        await _dbContext.SaveChangesAsync();
                        entity.AttachmentID = result.Entity.AttachmentID;
                        await _dbContext.SaveChangesAsync();
                        transaction.Commit();
                        transaction.Dispose();
                    }
                    return entity;
                }
                catch (SqlException ex) { WriteLog.WriteToFile("CertificateRepository/AddAttachmentTOCertificate", ex); throw new Exception("Something went wrong"); }
                catch (InvalidOperationException ex) { WriteLog.WriteToFile("CertificateRepository/AddAttachmentTOCertificate", ex); throw new Exception("Something went wrong"); }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    transaction.Dispose();
                    throw ex;
                }
            }
        }

        public BPCAttachment GetAttachmentByName(string partnerID, string certificateName, string certificateType, int AttachmentID)
        {
            try
            {
                var entity = _dbContext.BPCCertificates.Where(x => x.PatnerID == partnerID && x.CertificateName == certificateName && x.CertificateType == certificateType).FirstOrDefault();

                if (entity == null)
                {
                    return null;
                }
                var BPCAttachemnt = new BPCAttachment();
                BPCAttachemnt.CertificateName = entity.CertificateName;
                BPCAttachemnt.CertificateType = entity.CertificateType;

                var attachments = _dbContext.BPCAttachments.Where(x => x.PatnerID == partnerID && x.AttachmentID == AttachmentID).FirstOrDefault();
                if (attachments == null)
                {
                    return null;
                }
                BPCAttachemnt.AttachmentFile = attachments.AttachmentFile;
                BPCAttachemnt.PatnerID = entity.PatnerID;
                return BPCAttachemnt;
            }
            catch (SqlException ex) { WriteLog.WriteToFile("CertificateRepository/GetAttachmentByName", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("CertificateRepository/GetAttachmentByName", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<BPCCertificateSupport> GetSupportCertificates(string partnerID)
        {
            try
            {
                return _dbContext.BPCCertificatesSupport.Where(x => x.PatnerID == partnerID).ToList();
            }
            catch (SqlException ex) { WriteLog.WriteToFile("CertificateRepository/GetSupportCertificates", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("CertificateRepository/GetSupportCertificates", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }

}
