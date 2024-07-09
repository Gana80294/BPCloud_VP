using BPCloud_VP_POService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BPCloud_VP_POService.Repositories
{
    public interface IDashboardRepositorie
    {
        #region Order Fulfiment(OF)

        List<BPCOFHeaderView> GetOfsByPartnerID(string PartnerID);
        List<BPCOFHeaderView> GetOfPODetails(string GetOfPODetails);
        List<BPCOFHeaderView> GetOfsByOption(OfOption ofOption);
        List<FulfilmentDetails> GetVendorDoughnutChartData(string PartnerID);
        //List<FulfilmentDetails> GetVendorDoughnutChartDataByUser(string Plant);
        
        List<FulfilmentDetails> GetVendorDoughnutChartDataByPlant(string GetPlantByUser);
        
        FulfilmentStatus GetOfStatusByPartnerID(string PartnerID);
        List<ItemDetails> GetOfItemsByPartnerIDAndDocNumber(string PartnerID, string DocNumber);
        List<ASNDetails> GetOfASNsByPartnerIDAndDocNumber(string PartnerID, string DocNumber);
        List<GRNDetails> GetOfGRGIsByPartnerIDAndDocNumber(string PartnerID, string DocNumber);
        List<GRNDetails> GetGRGIsByPartnerIDAndDocNumber(string PartnerID, string DocNumber);
        List<GRNDetails> GetCancelGRGIsByPartnerIDAndDocNumber(string PartnerID, string DocNumber);
        List<QADetails> GetOfQMsByPartnerIDAndDocNumber(string PartnerID, string DocNumber);
        List<SLDetails> GetOfSLsByPartnerIDAndDocNumber(string PartnerID, string DocNumber);
        List<DocumentDetails> GetOfDocumentsByPartnerIDAndDocNumber(string PartnerID, string DocNumber);
        List<FlipDetails> GetOfFlipsByPartnerIDAndDocNumber(string PartnerID, string DocNumber);
        Task<BPCAttachment> UploadOfAttachment(string PartnerID,BPCAttachment BPAttachment, string DocNumber);
        //Task<BPCAttachment> UploadOfAttachmentByDocNumber(BPCAttachment BPAttachment, string DocNumber);
        
        BPCAttachment GetAttachmentByName(string PartnerID, string AttachmentName, string DocNumber);
        BPCAttachment DownloadOfAttachmentByDocNumber(string AttachmentName, string DocNumber);
        
        BPCInvoiceAttachment GetOfAttachmentByPartnerIDAndDocNumber(string PartnerID, string DocNumber);
        List<BPCInvoiceAttachment> GetOfAttachmentsByPartnerIDAndDocNumber(string PartnerID, string DocNumber);
        List<BPCInvoiceAttachment> GetOfAttachmentsByDocNumber(string DocNumber);
        
        #endregion
        List<PODetails> GetPODetails(string PartnerID);
        List<PODetails> GetAllPOBasedOnDate(POSearch pOSearch);
        OrderFulfilmentDetails GetOrderFulfilmentDetails(string PO, string PartnerID);
        OrderFulfilmentDetails GetOrderFulfilmentDetailsByDocNumber(string PO);
        
        BPCPlantMaster GetItemPlantDetails(string PlantCode);

        //BPCOFHeader CreateAcknowledgement(Acknowledgement acknowledgement);
        Task<BPCOFHeader> CreateAcknowledgement(Acknowledgement acknowledgement);
        Task<Acknowledgement> UpdatePOItems(Acknowledgement acknowledgement);
        BPCOFHeader GetBPCOFHeader(string partnerId,string ReferenceNo);
        BPCOFHeader GetBPCOFHeaderByDocNumber(string ReferenceNo);
        
        List<SODetails> GetSODetails(string Client, string Company, string Type, string PartnerID);
        List<BPCAttachment> GetAttachmentByPatnerIdAndDocNum( string PartnerID,string DocNum);
        List<SODetails> GetFilteredSODetailsByPartnerID(string Type, string PartnerID, DateTime? FromDate = null, DateTime? ToDate = null, string Status = null);
    }
}
