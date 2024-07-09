using BPCloud_VP.FactService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BPCloud_VP.FactService.Repositories
{
    public interface ICertificateRepository
    {
        List<BPCCertificate> GetAllCertificates();
        List<BPCCertificate> GetCertificatesByPartnerID(string PartnerID);
        Task<BPCCertificate> CreateCertificate(BPCCertificate Certificate);
        Task<BPCCertificateSupport> CreateCertificateSupport(BPCCertificateSupport Certificate);
        Task CreateCertificates(List<BPCCertificate> Certificates, string PartnerID);
        Task<BPCCertificate> UpdateCertificate(BPCCertificate Certificate);
        Task<BPCCertificate> DeleteCertificate(BPCCertificate certificate);

        BPCAttachment GetAttachmentByName(string partnerID, string certificateName, string certificateType, int AttachmentID);
        Task DeleteCertificateByPartnerID(string PartnerID);
        Task<BPCCertificateSupport> AddAttachmentTOCertificate(BPCAttachment attachment);

        List<BPCCertificateSupport> GetSupportCertificates(string partnerID);
    }
}
