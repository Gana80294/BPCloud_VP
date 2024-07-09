using BPCloud_VP.SupportDeskService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BPCloud_VP.SupportDeskService.Repositories
{
    public interface ISupportDeskRepository
    {
        List<SupportMaster> GetSupportMasters();
        List<SupportMasterViews> GetSupportMasterViews();
        List<string> GetReasonCodes();
        List<SupportAppMaster> GetSupportAppMasters();
        List<SupportMaster> GetSupportMastersByPartnerID(string PartnerID);
        Task<SupportMaster> CreateSupportMaster(SupportMaster supportMaster);
        Task<SupportMaster> UpdateSupportMaster(SupportMaster supportMaster);
        Task<SupportMaster> DeleteSupportMaster(SupportMaster supportMaster);
        SupportDetails GetSupportDetailsByPartnerAndSupportID(string SupportID, string PartnerID);
        SupportDetails GetSupportDetailsByPartnerIDAndDocRefNo(string PartnerID, string DocRefNo);
        List<SupportHeaderView> GetSupportTicketsByPartnerID(string PartnerID);
        List<SupportHeaderView> GetSupportTicketsByPlants(string GetPlantByUser);
        List<SupportHeaderView> GetTicketSearch(Ticketsearch Ticketsearch);
        
        SupportDetails GetSupportDetailsByPartnerAndSupportIDAndType(string SupportID, string PartnerID, string Type);
        SupportDetails GetSupportDetailsBySupportID(string SupportID);
        SupportDetails GetSupportDetailsByPartnerIDAndDocRefNoAndType(string PartnerID, string DocRefNo, string Type);
        List<SupportHeaderView> GetSupportTicketsByPartnerIDAndType(string PartnerID, string Type);
        List<SupportHeaderView> GetBuyerSupportTickets(string PartnerID, string Plant);
        List<SupportHeaderView> GetHelpDeskAdminSupportTickets(HelpDeskAdminDetails helpDeskAdminDetails);
        Task<SupportHeader> CreateSupportTicket(SupportHeaderView supportHeader);
        List<SupportLog> GetSupportLogsByPartnerAndSupportID(string supportID, string PartnerID);
        Task<SupportLog> CreateSupportLog(SupportLog supportLog);
        Task<SupportLog> UpdateSupportLog(SupportLogView supportLog);
        Task<SupportLog> ReOpenSupportTicket(SupportLogView supportLog);
        Task<SupportHeader> CloseSupportTicket(SupportHeader supportHeader);
        Task AddSupportAttachment(List<BPCSupportAttachment> BPAttachments, string SupportID);
        Task AddSupportLogAttachment(List<BPCSupportAttachment> BPAttachments, string SupportID, string SupportLogID);
        Task SaveFAQAttachment(BPCFAQAttachment BPCFAQAttachment);
        BPCFAQAttachment GetFAQAttachment();
        BPCSupportAttachment GetAttachmentByName(string AttachmentName, string SupportID);
        List<BPCSupportAttachment> GetSupportAttachments();

    }
}
