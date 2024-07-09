using BPCloud_VP_POService.Repositories;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BPCloud_VP_POService.Models
{
    [Table("BPC_OF_H")]
    public class BPCOFHeader : CommonClass
    {
        [MaxLength(3)]
        public string Client { get; set; }
        [MaxLength(4)]
        public string Company { get; set; }
        [MaxLength(1)]
        public string Type { get; set; }
        [MaxLength(12)]
        public string PatnerID { get; set; }
        [MaxLength(10)]
        public string DocNumber { get; set; }
        public string DocType { get; set; }
        public DateTime? DocDate { get; set; }
        public string DocVersion { get; set; }
        public string Currency { get; set; }
        public string Status { get; set; }
        public double? CrossAmount { get; set; }
        public double? NetAmount { get; set; }
        public string RefDoc { get; set; }
        public string AckStatus { get; set; }
        public string AckRemark { get; set; }
        public DateTime? AckDate { get; set; }
        public string AckUser { get; set; }
        public string PINNumber { get; set; }
        public string Plant { get; set; }
        public string PlantName { get; set; }
        public string POCreator { get; set; }
        public string Email { get; set; }
        public string ReleasedStatus { get; set; }
        //public string MaterialType { get; set; }
        



    }

    public class BPCOFReleaseView
    {
        public string Client { get; set; }
        public string Company { get; set; }
        public string Type { get; set; }
        public string PatnerID { get; set; }
        public string DocNumber { get; set; }
        public string ReleasedStatus { get; set; }
    }

    public class BPCOFHeaderUserView
    {
        public string DocNumber { get; set; }
        public string POCreator { get; set; }
        public string Email { get; set; }
    }

    [Table("BPC_OF_I")]
    public class BPCOFItem : CommonClass
    {
        [MaxLength(3)]
        public string Client { get; set; }
        [MaxLength(4)]
        public string Company { get; set; }
        [MaxLength(1)]
        public string Type { get; set; }
        [MaxLength(12)]
        public string PatnerID { get; set; }
        [MaxLength(10)]
        public string DocNumber { get; set; }
        [MaxLength(4)]
        public string Item { get; set; }
        public string Material { get; set; }
        public string MaterialText { get; set; }
        public DateTime? DeliveryDate { get; set; }
        public double? OrderedQty { get; set; }
        public double? CompletedQty { get; set; }
        public double? TransitQty { get; set; }
        public double? OpenQty { get; set; }
        public string UOM { get; set; }
        public string HSN { get; set; }
        [Required]
        public bool IsClosed { get; set; }
        public string AckStatus { get; set; }
        public DateTime? AckDeliveryDate { get; set; }
        public string PlantCode { get; set; }
        public double? UnitPrice { get; set; }
        public double? Value { get; set; }
        public double? TaxAmount { get; set; }
        public string TaxCode { get; set; }
        public string MaterialType { get; set; }
        [MaxLength(12)]
        public string Flag { get; set; }
        [Required]
        public bool IsFreightAvailable { get; set; }
        public double? FreightAmount { get; set; }
        public double? ToleranceUpperLimit { get; set; }
        public double? ToleranceLowerLimit { get; set; }
       
    }

    public class BPCOFItemView : CommonClass
    {
        public string Client { get; set; }
        public string Company { get; set; }
        public string Type { get; set; }
        public string PatnerID { get; set; }
        public string DocNumber { get; set; }
        public string Item { get; set; }
        public string Material { get; set; }
        public string MaterialText { get; set; }
        public string MaterialType { get; set; }
        public DateTime? DeliveryDate { get; set; }
        public double? OrderedQty { get; set; }
        public double? CompletedQty { get; set; }
        public double? TransitQty { get; set; }
        public double? OpenQty { get; set; }
        public string UOM { get; set; }
        public string HSN { get; set; }
        public bool IsClosed { get; set; }
        public string AckStatus { get; set; }
        public DateTime? AckDeliveryDate { get; set; }
        public string PlantCode { get; set; }
        public double? UnitPrice { get; set; }
        public double? Value { get; set; }
        public double? TaxAmount { get; set; }
        public string TaxCode { get; set; }
        public string Flag { get; set; }
        public bool IsFreightAvailable { get; set; }
        public double? FreightAmount { get; set; }
        public double? ToleranceUpperLimit { get; set; }
        public double? ToleranceLowerLimit { get; set; }
        public List<BPCOFItemSES> BPCOFItemSESes { get; set; }
    }

    [Table("BPC_OF_I_SES")]
    public class BPCOFItemSES : CommonClass
    {
        [MaxLength(3)]
        public string Client { get; set; }
        [MaxLength(4)]
        public string Company { get; set; }
        [MaxLength(1)]
        public string Type { get; set; }
        [MaxLength(12)]
        public string PatnerID { get; set; }
        [MaxLength(12)]
        public string DocNumber { get; set; }
        [MaxLength(4)]
        public string Item { get; set; }
        [MaxLength(24)]
        public string ServiceNo { get; set; }
        public string ServiceItem { get; set; }
        [Required]
        public double OrderedQty { get; set; }
        [Required]
        public double OpenQty { get; set; }
        [Required]
        public double ServiceQty { get; set; }
    }

    [Table("BPC_OF_SL")]
    public class BPCOFScheduleLine : CommonClass
    {
        [MaxLength(3)]
        public string Client { get; set; }
        [MaxLength(4)]
        public string Company { get; set; }
        [MaxLength(1)]
        public string Type { get; set; }
        [MaxLength(12)]
        public string PatnerID { get; set; }
        [MaxLength(10)]
        public string DocNumber { get; set; }
        [MaxLength(4)]
        public string Item { get; set; }
        [MaxLength(4)]
        public string SlLine { get; set; }
        public DateTime? DeliveryDate { get; set; }
        public double? OrderedQty { get; set; }
        public double? OpenQty { get; set; }
        public string AckStatus { get; set; }
        public DateTime? AckDeliveryDate { get; set; }
    }

    public class BPCOFItemViewFilter
    {
        public string PatnerID { get; set; }
        public List<string> DocNumbers { get; set; }
    }

    [Table("BPC_OF_GRGI")]
    public class BPCOFGRGI : CommonClass
    {
        [MaxLength(3)]
        public string Client { get; set; }
        [MaxLength(4)]
        public string Company { get; set; }
        [MaxLength(1)]
        public string Type { get; set; }
        [MaxLength(12)]
        public string PatnerID { get; set; }
        [MaxLength(10)]
        public string DocNumber { get; set; }
        [MaxLength(10)]
        public string GRGIDoc { get; set; }
        [MaxLength(4)]
        public string Item { get; set; }
        public string Description { get; set; }
        public string Material { get; set; }
        public string MaterialText { get; set; }
        public DateTime? DeliveryDate { get; set; }
        public DateTime? GRIDate { get; set; }
        public double? GRGIQty { get; set; }
        public string Plant { get; set; }
        public string ShippingPartner { get; set; }
        public string ShippingDoc { get; set; }
        [Required]
        public bool IsFinal { get; set; }
    }

    [Table("BPC_OF_QM")]
    public class BPCOFQM : CommonClass
    {
        [MaxLength(3)]
        public string Client { get; set; }
        [MaxLength(4)]
        public string Company { get; set; }
        [MaxLength(1)]
        public string Type { get; set; }
        [MaxLength(12)]
        public string PatnerID { get; set; }
        [MaxLength(12)]
        public string DocNumber { get; set; }
        [MaxLength(12)]
        public string Item { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int SerialNumber { get; set; }
        public string Material { get; set; }
        public string MaterialText { get; set; }
        public double? GRGIQty { get; set; }
        public double? LotQty { get; set; }
        public double? RejQty { get; set; }
        public string RejReason { get; set; }
    }

    [Table("BPC_OF_Subcon")]
    public class BPCOFSubcon : CommonClass
    {
        [MaxLength(3)]
        public string Client { get; set; }
        [MaxLength(4)]
        public string Company { get; set; }
        [MaxLength(1)]
        public string Type { get; set; }
        [MaxLength(12)]
        public string PatnerID { get; set; }
        [MaxLength(10)]
        public string DocNumber { get; set; }
        [MaxLength(4)]
        public string Item { get; set; }
        [MaxLength(4)]
        public string SlLine { get; set; }
        public DateTime? Date { get; set; }
        [Required]
        public double OrderedQty { get; set; }
        public string Batch { get; set; }
        public string Remarks { get; set; }
        public string Status { get; set; }
    }

    [Table("BPC_OF_AI_ACT")]
    public class BPCOFAIACT : CommonClass
    {
        [MaxLength(3)]
        public string Client { get; set; }
        [MaxLength(4)]
        public string Company { get; set; }
        [MaxLength(1)]
        public string Type { get; set; }
        [MaxLength(12)]
        public string PatnerID { get; set; }
        [MaxLength(4)]
        public string SeqNo { get; set; }
        public string AppID { get; set; }
        public string DocNumber { get; set; }
        public string ActionText { get; set; }
        public string Status { get; set; }
        public DateTime? Date { get; set; }
        public string Time { get; set; }
        [Required]
        public bool HasSeen { get; set; }
    }

    public class BPCAIACT : CommonClass
    {
        public string Client { get; set; }
        public string Company { get; set; }
        public string Type { get; set; }
        public string PatnerID { get; set; }
        public int SeqNo { get; set; }
        public string ActType { get; set; }
        public string AppID { get; set; }
        public string DocNumber { get; set; }
        public string Action { get; set; }
        public string ActionText { get; set; }
        public string Status { get; set; }
        public DateTime? Date { get; set; }
        public string Time { get; set; }
        public bool IsSeen { get; set; }
    }

    [Table("BPC_INV")]
    public class BPCInvoice : CommonClass
    {
        [MaxLength(3)]
        public string Client { get; set; }
        [MaxLength(4)]
        public string Company { get; set; }
        [MaxLength(1)]
        public string Type { get; set; }
        [MaxLength(12)]
        public string PatnerID { get; set; }
        [MaxLength(4)]
        public string FiscalYear { get; set; }
        [MaxLength(16)]
        public string InvoiceNo { get; set; }
        public DateTime? InvoiceDate { get; set; }
        [Required]
        public double InvoiceAmount { get; set; }
        public string AWBNumber { get; set; }
        public string PoReference { get; set; }
        [Required]
        public double PaidAmount { get; set; }
        public double? BalanceAmount { get; set; }
        public string Currency { get; set; }
        public DateTime? DateofPayment { get; set; }
        public string Plant { get; set; }
        public string Status { get; set; }
        public string AttID { get; set; }
        public DateTime? PODDate { get; set; }
        public string PODConfirmedBy { get; set; }
    }

    public class BPCInvoicePayView : CommonClass
    { 
        public List<BPCInvoice> Invoices { get; set; }
        public List<BPCPayRecord> PayRecord { get; set; }
        public List<BPCPayPayment> PayPayment { get; set; }
    }

    [Table("BPC_INV_I")]
    public class BPCInvoiceItem : CommonClass
    {
        [MaxLength(3)]
        public string Client { get; set; }
        [MaxLength(4)]
        public string Company { get; set; }
        [MaxLength(1)]
        public string Type { get; set; }
        [MaxLength(12)]
        public string PatnerID { get; set; }
        [MaxLength(4)]
        public string FiscalYear { get; set; }
        [MaxLength(16)]
        public string InvoiceNo { get; set; }
        [MaxLength(12)]
        public string Item { get; set; }
        public string Material { get; set; }
        public string MaterialText { get; set; }
        [Required]
        public double InvoiceQty { get; set; }
        [Required]
        public double PODQty { get; set; }
        public string ReasonCode { get; set; }
        public string Status { get; set; }
    }
    public class SubconItems
    {
        public List<BPCOFSubcon> items { get; set; }
    }
    public class BPCOFSubconView
    {
        public int ID { get; set; }
        public string Client { get; set; }
        public string Company { get; set; }
        public string Type { get; set; }
        public string PatnerID { get; set; }
        public string DocNumber { get; set; }
        public string Item { get; set; }
        public string SlLine { get; set; }
        public double OrderedQty { get; set; }
    }
    public class PODetails
    {
        public string PO { get; set; }
        public string Version { get; set; }
        public string Currency { get; set; }
        public DateTime? PODate { get; set; }
        public string POType { get; set; }
        public string PlantName { get; set; }
        public string Status { get; set; }
        public string Document { get; set; }

    }

    public class POScheduleLineView
    {
        public int ID { get; set; }
        public string Client { get; set; }
        public string Company { get; set; }
        public string Type { get; set; }
        public string PatnerID { get; set; }
        public string DocNumber { get; set; }
        public string Item { get; set; }
        public string Material { get; set; }
        public string MaterialText { get; set; }
        public string SlLine { get; set; }
        public DateTime? DeliveryDate { get; set; }
        public double? OrderedQty { get; set; }
    }

    public class SODetails
    {
        public int ID { get; set; }
        public string PatnerID { get; set; }
        public string SO { get; set; }
        public string PIRNumber { get; set; }
        public string PIRType { get; set; }
        public DateTime? SODate { get; set; }
        public string Status { get; set; }
        public string Document { get; set; }
        public string Version { get; set; }
        public string Currency { get; set; }
        public int DocCount { get; set; }
        public string DocNumber { get; set; }
        public string DocVersion { get; set; }
    }

    public class ASNDetails
    {
        public string ASN { get; set; }
        public DateTime? Date { get; set; }
        public string Truck { get; set; }
        public string Status { get; set; }
    }

    public class ItemDetails
    {
        public string Client { get; set; }
        public string Company { get; set; }
        public string Type { get; set; }
        public string PatnerID { get; set; }
        public string DocNumber { get; set; }
        public string Item { get; set; }
        public string Material { get; set; }
        public string MaterialText { get; set; }
        public string Description { get; set; }
        public DateTime? DeliveryDate { get; set; }
        public DateTime? Proposeddeliverydate { get; set; }
        public double? OrderQty { get; set; }
        public double? GRQty { get; set; }
        public double? PipelineQty { get; set; }
        public double? OpenQty { get; set; }
        public string UOM { get; set; }
        public string PlantCode { get; set; }
        public double? UnitPrice { get; set; }
        public double? Value { get; set; }
        public double? TaxAmount { get; set; }
        public string TaxCode { get; set; }
    }

    public class GRNDetails
    {
        public string GRGIDoc { get; set; }
        public string Item { get; set; }
        public string Material { get; set; }
        public string MaterialText { get; set; }
        public DateTime? GRNDate { get; set; }
        public double? Qty { get; set; }
        public string Status { get; set; }
        //public string DeliveryDate { get; set; }
    }
    public class QADetails
    {
        public string Item { get; set; }
        public int SerialNumber { get; set; }
        public string Material { get; set; }
        public string MaterialText { get; set; }
        public DateTime? Date { get; set; }
        public double? LotQty { get; set; }
        public double? RejQty { get; set; }
        public string RejReason { get; set; }
    }
    public class SLDetails
    {
        public string DocNumber { get; set; }
        public string Item { get; set; }
        public string SlLine { get; set; }
        public DateTime? DeliveryDate { get; set; }
        public double? OrderedQty { get; set; }
        public string Material { get; set; }
        public string Description { get; set; }
        public DateTime? Proposeddeliverydate { get; set; }
        public double? OrderQty { get; set; }
        public double? GRQty { get; set; }
        public double? PipelineQty { get; set; }
        public double? OpenQty { get; set; }
        public string UOM { get; set; }
    }
    public class FlipDetails : CommonClass
    {
        public string Client { get; set; }
        public string Company { get; set; }
        public string Type { get; set; }
        public string PatnerID { get; set; }
        public string FLIPID { get; set; }
        public string DocNumber { get; set; }
        public string InvoiceNumber { get; set; }
        public DateTime? InvoiceDate { get; set; }
        public double? InvoiceAmount { get; set; }
        public string InvoiceCurrency { get; set; }
        public string InvoiceType { get; set; }
        public string InvoiceDocID { get; set; }
        public string InvoiceAttachmentName { get; set; }
        public string IsInvoiceOrCertified { get; set; }
        public string IsInvoiceFlag { get; set; }
    }
    public class DocumentDetails : CommonClass
    {
        public int AttachmentID { get; set; }
        public string Client { get; set; }
        public string Company { get; set; }
        public string Type { get; set; }
        public string PatnerID { get; set; }
        public string ReferenceNo { get; set; }
        public string AttachmentName { get; set; }
        public string ContentType { get; set; }
        public long ContentLength { get; set; }
        public byte[] AttachmentFile { get; set; }
    }
    public class OrderFulfilmentDetails
    {
        public string PONumber { get; set; }
        public DateTime? PODate { get; set; }
        public string Currency { get; set; }
        public string Version { get; set; }
        public DateTime? ACKDate { get; set; }
        public string Status { get; set; }
        public string GateStatus { get; set; }
        public string GRNStatus { get; set; }
        public int ASNCount { get; set; }
        public int ItemCount { get; set; }
        public int GRNCount { get; set; }
        public int QACount { get; set; }
        public int SLCount { get; set; }
        public int DocumentCount { get; set; }
        public int FlipCount { get; set; }

        public int ReturnCount { get; set; }
        public List<ASNDetails> aSNDetails { get; set; }
        public List<ItemDetails> itemDetails { get; set; }
        public List<GRNDetails> gRNDetails { get; set; }
        public List<QADetails> qADetails { get; set; }
        public List<SLDetails> slDetails { get; set; }
        public List<DocumentDetails> documentDetails { get; set; }
        public List<FlipDetails> flipDetails { get; set; }

        public List<GRNDetails> ReturnDetails { get; set; }
    }
    public class Acknowledgement
    {
        //public string DalivaryDate { get; set; }
        public List<ItemDetails> ItemDetails { get; set; }
        public string PONumber { get; set; }
        //public string Status { get; set; }
    }
    public class POSearch
    {
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public string Status { get; set; }
        public string PartnerID { get; set; }
    }
    public class OfOption
    {
        public string DocNumber { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public string Status { get; set; }
        public string DocType { get; set; }
        public string PartnerID { get; set; }
        public string[] Plant { get; set; }
    }
    public class BPCOFHeaderXLSX
    {
        public string Client { get; set; }
        public string Company { get; set; }
        public string Partnerid { get; set; }
        public string Type { get; set; }
        public string Docnumber { get; set; }
        public string Docdate { get; set; }
        public string Docversion { get; set; }
        public string Plantname { get; set; }
        public string Currency { get; set; }
        public string Status { get; set; }
        public string Refdocument { get; set; }
    }
    public class BPCOFItemXLSX
    {
        public string Client { get; set; }
        public string Company { get; set; }
        public string Partnerid { get; set; }
        public string Type { get; set; }
        public string Docnumber { get; set; }
        public string Itemnumber { get; set; }
        public string Materialnumber { get; set; }
        public string Materialtext { get; set; }
        public string UOM { get; set; }
        public double Orderqty { get; set; }
        public string Deliverydate { get; set; }
        public string Plant { get; set; }
        public string Unitprice { get; set; }
        public string Value { get; set; }
        public string Taxamount { get; set; }
        public string Taxcode { get; set; }
    }
    public class BPCOFScheduleLineXLSX
    {
        public string Client { get; set; }
        public string Company { get; set; }
        public string Partnerid { get; set; }
        public string Docnumber { get; set; }
        public string Itemnumber { get; set; }
        public string Sline { get; set; }
        public string Type { get; set; }
        public string Deldate { get; set; }
        public double Orderquantity { get; set; }
    }
    public class BPCOFGRGIXLSX
    {
        public string Client { get; set; }
        public string Company { get; set; }
        public string Partnerid { get; set; }
        public string Docnumber { get; set; }
        public string Type { get; set; }
        public string Item { get; set; }
        public string Material { get; set; }
        public string Materialtext { get; set; }
        public string Grgidoc { get; set; }
        public string Unit { get; set; }
        public double Grgiqty { get; set; }
        public string Deliverydate { get; set; }
        public string Shippingpartner { get; set; }

    }
    public class GRNListFilter
    {
        public List<string> Plants { get; set; }
        public string GRN { get; set; }
        public string Material { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
    }
    public class BPCOFQMXLSX
    {
        public string Client { get; set; }
        public string Company { get; set; }
        public string Patnerid { get; set; }
        public string Type { get; set; }
        public string Docnumber { get; set; }
        public string Item { get; set; }
        public string Material { get; set; }
        public string MaterialText { get; set; }
        public string Insplotno { get; set; }
        public string Unit { get; set; }
        public double Lotqty { get; set; }
        public double Rejqty { get; set; }
        public string Rejreason { get; set; }
        public string Date { get; set; }
        public double Grgiqty { get; set; }
    }
    public class BPCPAYASXLSX
    {
        public string Client { get; set; }
        public string Company { get; set; }
        public string Type { get; set; }
        public string PartnerID { get; set; }
        public string FiscalYear { get; set; }
        public string DocumentNumber { get; set; }
        public DateTime? DocumentDate { get; set; }
        public string InvoiceNumber { get; set; }
        public DateTime? InvoiceDate { get; set; }
        public double InvoiceAmount { get; set; }
        public double PaidAmount { get; set; }
        public string Reference { get; set; }
    }
    public class BPCPAYPAYMENTXLSX
    {
        public string Client { get; set; }
        public string Company { get; set; }
        public string Type { get; set; }
        public string PartnerID { get; set; }
        public string FiscalYear { get; set; }
        public string DocumentNumber { get; set; }
        public string PaymentDate { get; set; }
        public string PaymentType { get; set; }
        public double PaidAmount { get; set; }
        public string BankName { get; set; }
        public string BankAccount { get; set; }
    }

    public class BPCOFHeaderView : CommonClass
    {
        public int ID { get; set; }
        public string Client { get; set; }
        public string Company { get; set; }
        public string Type { get; set; }
        public string PatnerID { get; set; }
        public string DocNumber { get; set; }
        public DateTime? DocDate { get; set; }
        public string DocVersion { get; set; }
        public string Currency { get; set; }
        public string Status { get; set; }
        public string GateStatus { get; set; }
        public string GRNStatus { get; set; }
        public double CrossAmount { get; set; }
        public double NetAmount { get; set; }
        public string RefDoc { get; set; }
        public string AckStatus { get; set; }
        public string AckRemark { get; set; }
        public DateTime? AckDate { get; set; }
        public string AckUser { get; set; }
        public string PINNumber { get; set; }
        public string DocType { get; set; }
        public string Plant { get; set; }
        public string PlantName { get; set; }
        public int DocCount { get; set; }
        public string ReleasedStatus { get; set; }
    }

    public class SOItemCount
    {
        public string Client { get; set; }
        public string Company { get; set; }
        public string Type { get; set; }
        public string PatnerID { get; set; }
        public string DocNumber { get; set; }
        public int ItemCount { get; set; }
        public int GRGICount { get; set; }
        public int PODCount { get; set; }
        public int InvCount { get; set; }
        public int ReturnCount { get; set; }
        public int DocumentCount { get; set; }
        //DocumentCount

    }

    public class BPCSTK : CommonClass
    {
        public string Client { get; set; }
        [MaxLength(4)]
        public string Company { get; set; }
        [MaxLength(1)]
        public string Type { get; set; }
        [MaxLength(12)]
        public string PatnerID { get; set; }
        public string Item { get; set; }
        public string PODocument { get; set; }
        public DateTime? Date { get; set; }

        public string Material { get; set; }
        public string IssuedQty { get; set; }
        public string StockQty { get; set; }
    }
    public class BPCPAYH : CommonClass
    {
        public string Client { get; set; }
        [MaxLength(4)]
        public string Company { get; set; }
        [MaxLength(1)]
        public string Type { get; set; }
        [MaxLength(12)]
        public string PatnerID { get; set; }
        public string FiscalYear { get; set; }

        public string PayDoc { get; set; }

        public DateTime? Date { get; set; }
        public string Amount { get; set; }
        public string Currency { get; set; }
        public string Remark { get; set; }
    }

    public class BPCOFHeader_Gate : CommonClass
    {
        [MaxLength(3)]
        public string Client { get; set; }
        [MaxLength(4)]
        public string Company { get; set; }
        [MaxLength(1)]
        public string Type { get; set; }
        [MaxLength(12)]
        public string PatnerID { get; set; }
        [MaxLength(10)]
        public string DocNumber { get; set; }
    }

    public class BPCOFHeaderWithAttachments : BPCOFHeader
    {
        public List<string> Attachments { get; set; }
    }

    public class QMReportOption
    {
        public string PartnerID { get; set; }
        public string Material { get; set; }
        public string PO { get; set; }
        public string Status { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }

    }
}
