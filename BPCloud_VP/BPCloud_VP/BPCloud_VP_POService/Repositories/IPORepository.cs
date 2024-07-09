using BPCloud_VP_POService.Models;
using Remotion.Linq.Clauses.ResultOperators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BPCloud_VP_POService.Repositories
{
    public interface IPORepository
    {
        List<BPCOFHeader> GetAllPOList();
        List<BPCOFHeader> FilterPOList(string VendorCode, string PONumber = null, string Status = null, DateTime? FromDate = null, DateTime? ToDate = null);
        BPCOFHeader GetPOByDoc(string DocNumber);
        BPCOFHeader GetPOPartnerID(string PartnerID);
        BPCOFHeader GetPOByDocAndPartnerID(string DocNumber, string PartnerID);
        List<BPCOFHeader> GetOFByDocAndPartnerID(BPCOFItemViewFilter filter);
        List<BPCOFItem> GetPOItemsByDoc(string DocNumber);
        List<BPCOFItemSES> GetOFItemSES();
        List<BPCOFItemSES> GetOFItemSESByItem(string item, string DocumentNo, string PartnerId);
        List<BPCOFItemSES> GetOFItemSESByDocNumber(string item, string DocumentNo);
        List<BPCOFItem> GetSupportPOItemsByDoc(string DocNumber);
        List<BPCOFItem> GetPOItemsByDocAndPartnerID(string DocNumber, string PartnerID);
        List<BPCOFItemView> GetPOItemViewsByDocAndPartnerID(string DocNumber, string PartnerID);
        List<BPCOFItemView> GetPOItemViewsByDoc(string DocNumber);
        List<BPCOFItemView> GetOFItemViewsForASNByDocAndPartnerID(BPCOFItemViewFilter filter);
        Task CreateBPCH(List<BPCOFHeaderWithAttachments> data);
        Task UpdateBPCHGateStatus(List<BPCOFHeader_Gate> data);
        Task CreateBPCItems(List<BPCOFItem> data);
        Task CreateOFItemSES(List<BPCOFItemSES> Items);
        Task CreateBPCSL(List<BPCOFScheduleLine> data);

        Task UpdateBPCH(List<BPCOFHeader> data);
        Task UpdateBPCHReleaseStatus(List<BPCOFReleaseView> data);
        Task UpdateBPCItems(List<BPCOFItem> data);
        Task UpdateBPCSL(List<BPCOFScheduleLine> data);

        BPCOFScheduleLine GetBPCSLByDocNumber(string DocNumber);
        List<BPCOFGRGI> GetPOGRGIByDocAndPartnerID(string DocNumber, string PartnerID);
        SOItemCount GetSOItemCountByDocAndPartnerID(string DocNumber, string PartnerID);
        //GRR table
        List<BPCOFGRGI> GetAllGRR();
        List<BPCOFGRGI> GetOfSuperUsergrnDetails(string GetPlantByUser, string GRGIDoc = null, string DocNumber = null, string Material = null, DateTime? GRIDate = null);

        
        List<BPCOFGRGI> FilterGRRListByPartnerID(string PartnerID, string GRGIDoc = null, string DocNumber = null, string Material = null, DateTime? GRIDate = null);
        List<BPCOFGRGI> FilterGRGIListByPlants(GRNListFilter filter);
        List<BPCOFGRGI> FilterGRRListForBuyer(string GRGIDoc = null, string DocNumber = null, string Material = null, DateTime? ASNFromDate = null, DateTime? ASNToDate = null);

        List<POScheduleLineView> GetPOSLByDocAndPartnerID(string DocNumber, string PartnerID);
        BPCAttachment GetAttachment(string attachmentName);

        byte[] CreatePOPdf(string DocNumber);
        BPCAttachment PrintPO(string DocNumber);
        #region PO FLIP
        
        List<BPCFLIPHeaderView> GetPOFLIPsByPartnerID(string PartnerID);
        List<BPCFLIPHeaderView> GetPOFLIPsBySuperUser(string GetPlantByUser);
        List<BPCFLIPHeaderView> GetPOFLIPsByDocAndPartnerID(string DocNumber, string PartnerID);
        Task<BPCFLIPHeader> CreatePOFLIP(BPCFLIPHeaderView FLIPHeader);
        Task<BPCFLIPHeader> UpdatePOFLIP(BPCFLIPHeaderView FLIPHeader);
        Task<BPCFLIPHeader> DeletePOFLIP(BPCFLIPHeaderView FLIPHeader);
        List<BPCFLIPItem> GetFLIPItemsByFLIPID(string FLIPID);

        #endregion

        #region Data Migration
        Task CreateOFHeaders(List<BPCOFHeaderXLSX> OFHeaders);
        Task CreateOFItems(List<BPCOFItemXLSX> OFItems);
        Task CreateOFScheduleLines(List<BPCOFScheduleLineXLSX> OFScheduleLines);
        Task CreateOFGRGIs(List<BPCOFGRGIXLSX> OFGRGIs);

        Task CreateBPCOFGRGI(List<BPCOFGRGI> data);
        Task CancelBPCOFGRGI(List<BPCOFGRGI> data);
        List<BPCOFGRGI> GetGRRByPartnerId(string partnerId);

        List<BPCOFQM> GetBPCQMByDocNumber(string DocNumber);
        Task CreateOFQMs(List<BPCOFQMXLSX> OFQMs);
        Task CreateBPCQM(List<BPCOFQM> data);
        Task UpdateBPCQM(List<BPCOFQM> data);

        List<BPCOFQM> GetBPCQMByPartnerID(string PartnerID);
        List<BPCOFQM> GetBPCQMByPartnerIDs(string PartnerID);
        List<BPCOFQM> GetBPCQMByPartnerIDFilter(string PartnerID);
        List<BPCOFQM> GetBPCQMByPartnerIDsFilter(string PartnerID);
        
        List<BPCOFQM> GetQMReportByDate(QMReportOption QMReportOption);
        List<BPCOFQM> GetQMReportByOption(QMReportOption QMReportOption);
        List<BPCOFQM> GetQMReportByPatnerID(QMReportOption QMReportOption);
        
        List<BPCOFQM> GetQMReportByStatus(QMReportOption QMReportOption);

        //Task CreateBPCPAYH(List<BPCPAYH> data);
        //Task UpdateBPCPAYH(List<BPCPAYH> data);

        Task CreatePAYASs(List<BPCPAYASXLSX> PAYASs);
        Task CreatePAYPAYMENTs(List<BPCPAYPAYMENTXLSX> PAYPAYMENTs);

        string GetPlantByDocNmber(string DocNumber, string PartnerID);
        string GetPlantByASNNmber(string DocNumber, string PartnerID);
        #endregion


        Task<BPCLog> CreateBPCLog(string APIMethod, int NoOfRecords);
        Task<BPCLog> UpdateBPCSucessLog(int LogID);
        Task<BPCLog> UpdateBPCFailureLog(int LogID, string ErrorMessage);

        Task SendDeliveryNotification();
    }
}
