using BPCloud_VP.FactService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BPCloud_VP.FactService.Repositories
{
    public interface IFactRepository
    {

        List<BPCFact> GetAllFacts();
        List<BPCFact> GetAllVendors();
        List<BPCFact1> GetAllVendorNames();
        BPCFact GetFactByPartnerID(string PartnerID);
        //BPCFact GetFactSupportByPartnerID(string PartnerID);
        List<BPCFact> GetFactByPartnerIDAndType(string PartnerID, string Type);
        BPCFact GetFactByEmailID(string EmailID);
        BPCFact FindFactByTaxNumber(string TaxNumber);
        Task<BPCFact> CreateFact(BPCFactView FactView);
        Task<BPCFact> CreateFacts(List<BPCFact> Facts);
        Task<BPCFact> UpdateFact(BPCFactView FactView);
        Task<BPCFact> UpdateFact(BPCFact FactView);
        Task UpdateFactSupportDataToMasterData(string PartnerID);
        Task<BPCFactViewSupport> UpdateFactSupport(BPCFactViewSupport FactView);

        void UpdateFactBankSupport(List<BPCFactBankSupport> FactBank);
        void UpdateFactCertificateSupport(List<BPCCertificateSupport> FactCertificate);

        Task<BPCFact> DeleteFact(BPCFact Fact);
        Task CreateFacts(List<BPCFactXLSX> Facts);
        Task<BPCFactSupport> UpdateFact_BPCFact(BPCFact Fact);
        Task GetHelpDeskUserAndSendMail(BPCFactSupport fact, string XmlFilePath, List<FTPAttachment> attachments);
        Task<List<BPCAttach>> UpdateAttachment(List<BPCAttachments> Attachment, List<string> catogery);
        #region getAllAttachment
        BPCAttachments GettAttchmentByAttId(int AttachmentID);
        #endregion
        //Task UpdateAttachment(List<BPCAttachments> Facts);
        BPCFactSupport GetFactSupportDetails(string patnerID);
        Task<bool> SendMail(BPCFactSupport facts, List<BPCAttachments> attachments, List<UserView> users);
        List<FTPAttachment> GetAllAttachmentsToFTP(BPCFactSupport Fact, bool ISDecleration);
        #region Dashboard Status
        DashboardGraphStatus GetDashboardGraphStatus(string PartnerID);
        BPCKRA GetVendorKRAProcessCircle(string PartnerID);
        //List<BPCKRA> GetVendorKRAProcessCircleForSuperUser(string PartnerIDforSuperUser);
        
        BPCKRA GetVendorPPMProcessCircle(string PartnerID);
        List<BPCKRA> GetCustomerDoughnutChartData(string PartnerID);
        BPCKRA GetCustomerOpenProcessCircle(string PartnerID);
        BPCKRA GetCustomerCreditLimitProcessCircle(string PartnerID);
        CustomerBarChartData GetCustomerBarChartData(string PartnerID);
        #endregion

        public string CreateXMLFromVendor(BPCFactSupport BPCFact, bool IsDecleration = true);
        public List<FTPAttachment> CreateAllAttachments(BPCFactSupport Fact, bool ISDecleration = true);
    }

}
