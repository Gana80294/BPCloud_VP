using BPCloud_VP_POService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BPCloud_VP_POService.Repositories
{
    public interface IASNRepository
    {
        List<BPCASNHeader> GetAllASNs();
        List<BPCASNHeader> GetAllASNByPartnerID(string PartnerID);
        List<BPCASNHeader> GetAllASNBySuperUser(string GetPlantByUser);
        List<BPCASNHeader1> GetAllASN1ByPartnerID(string PartnerID);
        List<BPCASNHeader1> GetOfSuperUserASN1Details(string GetPlantByUser);
        
        List<ASNListView> GetAllASNList();
        List<ASNListView> GetAllASNListByPartnerID(string PartnerID);
        List<ASNListView> FilterASNList(string VendorCode, string ASNNumber = null, string DocNumber = null, string Material = null, string Status = null, DateTime? ASNFromDate = null, DateTime? ASNToDate = null);
        List<ASNListView> FilterASNListByPlants(ASNListFilter filter);
        List<ASNListView> FilterASNList1ByPlants(ASNListFilter filter);
        List<BPCASNAttachment> GetASNAttachmentsASNNumber(string ASNNumber);
        List<BPCASNAttachment> GetASNAttachment1ASNNumber(string ASNNumber);
        List<ASNListView> FilterASNListByPartnerID(string PartnerID, string ASNNumber = null, string DocNumber = null, string Material = null, string Status = null, DateTime? ASNFromDate = null, DateTime? ASNToDate = null);
        List<ASNListView> FilterASNListByPlant(string GetPlantByUser, string ASNNumber = null, string DocNumber = null, string Material = null, string Status = null, DateTime? ASNFromDate = null, DateTime? ASNToDate = null);

        
        List<ASNListView> FilterASNListBySuperUser(string GetPlantByUser, string ASNNumber = null, string DocNumber = null, string Material = null, string Status = null, DateTime? ASNFromDate = null, DateTime? ASNToDate = null);
        
        List<ASNListView> FilterASNList1BySuperUser(string GetPlantByUser, string ASNNumber = null, string DocNumber = null, string Material = null, string Status = null, DateTime? ASNFromDate = null, DateTime? ASNToDate = null);

        List<ASNListView> FilterASNList1ByPartnerID(string PartnerID, string ASNNumber = null, string DocNumber = null, string Material = null, string Status = null, DateTime? ASNFromDate = null, DateTime? ASNToDate = null);
        List<ASNListView> FilterASNListBySER(string PartnerID, string ASNNumber = null, string DocNumber = null, string Material = null, string Status = null, DateTime? ASNFromDate = null, DateTime? ASNToDate = null);
        List<BPCASNHeader> GetASNsByDoc(string DocNumber);
        List<BPCASNHeader> GetASNByDocAndPartnerID(string DocNumber, string PartnerID);
        List<BPCASNHeader1> GetASNByOFAndPartnerID(BPCOFItemViewFilter filter);
        BPCASNHeader GetASNByPartnerID(string PartnerID);
        BPCASNHeader GetASNByDocAndASN(string DocNumber, string ASNNumber);
        BPCASNHeader GetASNByASN(string ASNNumber);
        BPCASNHeader1 GetASN1ByASN(string ASNNumber);
        BPCASNView2 GetASNViewByASN(string ASNNumber);
        Task CreateGateEntry(GateEntry gateEntry);
        Task<BPCASNHeader> CreateASN(BPCASNView ASNView, bool OCRvalidate,string InvoiceAmountOCR, bool shipAndInvAmount,bool ShipInvOcrAmount, bool InvoiceNumber, string InvoiceNumberOCR,string GSTValue,bool GSTcheck,string SupplierGSTValue, bool SupplierGSTCheck);
        Task<BPCASNHeader1> CreateASN1(BPCASNView1 ASNView, bool OCRvalidate, string OCRAmount, bool shipAndInvAmount, bool ShipInvOcrAmount, bool InvoiceNumber, string InvoiceNumberOCR,string GSTValue,bool GSTcheck, string SupplierGSTValue, bool SupplierGSTCheck);
        Task<BPCASNHeader> UpdateASN(BPCASNView ASNView,bool OCRvalidate, string OCRAmount, bool shipAndInvAmount, bool ShipInvOcrAmount, bool InvoiceNumber, string InvoiceNumberOCR,string GSTValue,bool GSTcheck,string SupplierGSTValue, bool SupplierGSTCheck);
        Task<BPCASNHeader1> UpdateASN1(BPCASNView1 ASNView, bool OCRvalidate, string OCRAmount, bool shipAndInvAmount, bool ShipInvOcrAmount, bool InvoiceNumber, string InvoiceNumberOCR,string GSTValue,bool GSTcheck, string SupplierGSTValue, bool SupplierGSTCheck);
        Task UpdateASNApprovalStatus(BPCASNViewApproval viewApproval);
        Task UpdateASN1ApprovalStatus(BPCASNViewApproval viewApproval);
        Task<BPCASNHeader> DeleteASN(BPCASNHeader ASN);
        Task<BPCASNHeader1> DeleteASN1(BPCASNHeader1 ASN);
        Task CancelASN(List<CancelASNView> ASNs);
        Task CancelGate(List<CancelASNView> ASNs);
        List<BPCASNFieldMaster> GetAllASNFieldMaster();
        BPCASNPreShipmentMaster GetASNPreShipmentMaster();
        List<BPCASNPreShipmentMaster> GetAllASNPreShipmentMasters();
        Task<BPCASNPreShipmentMaster> CreateASNPreShipmentMaster(BPCASNPreShipmentMaster ASNPreShipmentMaster);
        Task<BPCASNPreShipmentMaster> UpdateASNPreShipmentMaster(BPCASNPreShipmentMaster ASNPreShipmentMaster);
        Task<BPCASNPreShipmentMaster> DeleteASNPreShipmentMaster(BPCASNPreShipmentMaster ASNPreShipmentMaster);
        List<BPCASNFieldMaster> GetASNFieldMasterByType(string DocType);
        Task<BPCASNFieldMaster> UpdateASNFieldMaster(BPCASNFieldMaster ASNFieldMaster);
        int GetArrivalDateIntervalByPO(string DocNumber);
        int GetArrivalDateIntervalByPOAndPartnerID(string DocNumber);
        //Task CreateASNItems(List<BPCASNItem> ASNitems, string ASNNumber);
        List<string> GetASNOFsByASN(string ASNNumber);
        List<BPCASNItem> GetASNItemsByASN(string ASNNumber);
        List<BPCASNItemView> GetASNItemsWithBatchesByASN(string ASNNumber);
        List<BPCASNItemView1> GetASNItem1WithBatchesByASN(string ASNNumber);
        List<BPCASNItemBatch> GetASNItemBatchesByASN(string ASNNumber);
        List<BPCASNPack> GetASNPacksByASN(string ASNNumber);

        //Task CreateDocumentCenters(List<BPCDocumentCenter> DocumentCenters, string ASNNumber);
        List<BPCDocumentCenter> GetDocumentCentersByASN(string ASNNumber);

        Task AddInvoiceAttachment(BPCAttachment BPAttachment);
        Task AddDocumentCenterAttachment(List<BPCAttachment> BPAttachments,string ASNNumber,string IsSubmitted,string Company);
        BPCAttachment GetAttachmentByName(string AttachmentName, string ASNNumber);
        BPCAttachment DowloandAttachmentByID(int AttachmentID);
        BPCInvoiceAttachment GetInvoiceAttachmentByASN(string ASNNumber, string InvDocReferenceNo);
        BPCInvoiceAttachment GetInvoiceAttachment1ByASN(string ASNNumber, string InvDocReferenceNo);
        bool GSTValidate(string gst, string Plant);
       // bool SupplierGSTValidate (string gst, string PartnerID);
        byte[] CreateASNPdf(string ASNNumber, bool FTPFlag);
        byte[] CreateASNPdf1(string ASNNumber, bool FTPFlag);
    }
}
