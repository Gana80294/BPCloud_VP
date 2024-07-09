using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BPCloud_VP_POService.Models
{
    public class CommonClass
    {
        public bool IsActive { get; set; }
        public DateTime CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public string ModifiedBy { get; set; }
    }

    [Table("BPC_ASN_H")]
    public class BPCASNHeader : CommonClass
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
        public string ASNNumber { get; set; }
        public DateTime? ASNDate { get; set; }
        public string DocNumber { get; set; }
        public string TransportMode { get; set; }
        public string VessleNumber { get; set; }
        public string CountryOfOrigin { get; set; }
        public string AWBNumber { get; set; }
        public DateTime? AWBDate { get; set; }
        public DateTime? DepartureDate { get; set; }
        public DateTime? ArrivalDate { get; set; }
        public string ShippingAgency { get; set; }
        public double? GrossWeight { get; set; }
        public string GrossWeightUOM { get; set; }
        public double? NetWeight { get; set; }
        public string NetWeightUOM { get; set; }
        public double? VolumetricWeight { get; set; }
        public string VolumetricWeightUOM { get; set; }
        public int? NumberOfPacks { get; set; }
        public string InvoiceNumber { get; set; }
        public DateTime? InvoiceDate { get; set; }
        public double? POBasicPrice { get; set; }
        public double? TaxAmount { get; set; }
        public double? InvoiceAmount { get; set; }
        public string InvoiceAmountUOM { get; set; }
        public string InvDocReferenceNo { get; set; }
        public string Plant { get; set; }
        public string Status { get; set; }
        public bool IsSubmitted { get; set; }
        public DateTime? CancelDuration { get; set; }
        public int ArrivalDateInterval { get; set; }
        [StringLength(20)]
        public string BillOfLading { get; set; }
        [StringLength(40)]
        public string TransporterName { get; set; }
        public double? AccessibleValue { get; set; }
        [StringLength(40)]
        public string ContactPerson { get; set; }
        [StringLength(14)]
        public string ContactPersonNo { get; set; }
        public string Field1 { get; set; }
        public string Field2 { get; set; }
        public string Field3 { get; set; }
        public string Field4 { get; set; }
        public string Field5 { get; set; }
        public string Field6 { get; set; }
        public string Field7 { get; set; }
        public string Field8 { get; set; }
        public string Field9 { get; set; }
        public string Field10 { get; set; }
        public string GateEntryNo { get; set; }
        public DateTime? GateEntryDate { get; set; }
        public string GateEntryCreatedBy { get; set; }
        public bool IsBuyerApprovalRequired { get; set; }
        public string BuyerApprovalStatus { get; set; }
        public DateTime? BuyerApprovalOn { get; set; }
    }

    [Table("BPC_ASN_I")]
    public class BPCASNItem : CommonClass
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
        public string ASNNumber { get; set; }
        [MaxLength(4)]
        public string Item { get; set; }
        public string Material { get; set; }
        public string MaterialText { get; set; }
        public DateTime? DeliveryDate { get; set; }
        public double OrderedQty { get; set; }
        public double CompletedQty { get; set; }
        public double TransitQty { get; set; }
        public double OpenQty { get; set; }
        public double ASNQty { get; set; }
        public string UOM { get; set; }
        public string HSN { get; set; }
        public string PlantCode { get; set; }
        public double? UnitPrice { get; set; }
        public double? Value { get; set; }
        public double? TaxAmount { get; set; }
        public string TaxCode { get; set; }
        public bool IsFreightAvailable { get; set; }
        public double? FreightAmount { get; set; }
        public double? ToleranceUpperLimit { get; set; }
        public double? ToleranceLowerLimit { get; set; }
    }

    public class BPCASNItemView : CommonClass
    {
        [MaxLength(3)]
        public string Client { get; set; }
        [MaxLength(4)]
        public string Company { get; set; }
        [MaxLength(1)]
        public string Type { get; set; }
        [MaxLength(12)]
        public string PatnerID { get; set; }
        public string DocNumber { get; set; }
        [MaxLength(12)]
        public string ASNNumber { get; set; }
        [MaxLength(4)]
        public string Item { get; set; }
        public string Material { get; set; }
        public string MaterialText { get; set; }
        public DateTime? DeliveryDate { get; set; }
        public double OrderedQty { get; set; }
        public double CompletedQty { get; set; }
        public double TransitQty { get; set; }
        public double OpenQty { get; set; }
        public double ASNQty { get; set; }
        public string UOM { get; set; }
        public string HSN { get; set; }
        public string PlantCode { get; set; }
        public double? UnitPrice { get; set; }
        public double? Value { get; set; }
        public double? TaxAmount { get; set; }
        public string TaxCode { get; set; }
        public bool IsFreightAvailable { get; set; }
        public double? FreightAmount { get; set; }
        public double? ToleranceUpperLimit { get; set; }
        public double? ToleranceLowerLimit { get; set; }
        public List<BPCASNItemBatch> ASNItemBatches { get; set; }
        public List<BPCASNItemSES> ASNItemSESes { get; set; }
    }

    [Table("BPC_ASN_I_Batch")]
    public class BPCASNItemBatch : CommonClass
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
        public string ASNNumber { get; set; }
        [MaxLength(4)]
        public string Item { get; set; }
        [MaxLength(24)]
        public string Batch { get; set; }
        public double Qty { get; set; }
        public DateTime? ManufactureDate { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public string ManufactureCountry { get; set; }
    }

    [Table("BPC_ASN_I_SES")]
    public class BPCASNItemSES : CommonClass
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
        public string ASNNumber { get; set; }
        [MaxLength(4)]
        public string Item { get; set; }
        [MaxLength(24)]
        public string ServiceNo { get; set; }
        public string ServiceItem { get; set; }
        public double OrderedQty { get; set; }
        public double OpenQty { get; set; }
        public double ServiceQty { get; set; }
    }

    [Table("BPC_ASN_H1")]
    public class BPCASNHeader1 : CommonClass
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
        public string ASNNumber { get; set; }
        public DateTime? ASNDate { get; set; }
        [MaxLength(20)]
        public string DocNumber { get; set; }
        public string TransportMode { get; set; }
        public string VessleNumber { get; set; }
        public string CountryOfOrigin { get; set; }
        public string AWBNumber { get; set; }
        public DateTime? AWBDate { get; set; }
        public DateTime? DepartureDate { get; set; }
        public DateTime? ArrivalDate { get; set; }
        public string ShippingAgency { get; set; }
        public double? GrossWeight { get; set; }
        public string GrossWeightUOM { get; set; }
        public double? NetWeight { get; set; }
        public string NetWeightUOM { get; set; }
        public double? VolumetricWeight { get; set; }
        public string VolumetricWeightUOM { get; set; }
        public int? NumberOfPacks { get; set; }
        public string InvoiceNumber { get; set; }
        public DateTime? InvoiceDate { get; set; }
        public double? POBasicPrice { get; set; }
        public double? TaxAmount { get; set; }
        public double? InvoiceAmount { get; set; }
        public string InvoiceAmountUOM { get; set; }
        public string InvDocReferenceNo { get; set; }
        public string Plant { get; set; }
        public string PurchaseGroup { get; set; }
        public string Status { get; set; }
        public bool IsSubmitted { get; set; }
        public DateTime? CancelDuration { get; set; }
        public int ArrivalDateInterval { get; set; }
        [StringLength(20)]
        public string BillOfLading { get; set; }
        [StringLength(40)]
        public string TransporterName { get; set; }
        public double? AccessibleValue { get; set; }
        [StringLength(40)]
        public string ContactPerson { get; set; }
        [StringLength(14)]
        public string ContactPersonNo { get; set; }
        public string Field1 { get; set; }
        public string Field2 { get; set; }
        public string Field3 { get; set; }
        public string Field4 { get; set; }
        public string Field5 { get; set; }
        public string Field6 { get; set; }
        public string Field7 { get; set; }
        public string Field8 { get; set; }
        public string Field9 { get; set; }
        public string Field10 { get; set; }
        public string GateEntryNo { get; set; }
        public DateTime? GateEntryDate { get; set; }
        public string GateEntryCreatedBy { get; set; }
        public bool IsBuyerApprovalRequired { get; set; }
        public string BuyerApprovalStatus { get; set; }
        public DateTime? BuyerApprovalOn { get; set; }
    }

    [Table("BPC_ASN_OF_Map1")]
    public class BPCASNOFMap1 : CommonClass
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
        public string ASNNumber { get; set; }
        [MaxLength(20)]
        public string DocNumber { get; set; }
    }

    [Table("BPC_ASN_I1")]
    public class BPCASNItem1 : CommonClass
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
        public string ASNNumber { get; set; }
        [MaxLength(20)]
        public string DocNumber { get; set; }
        [MaxLength(4)]
        public string Item { get; set; }
        public string Material { get; set; }
        public string MaterialText { get; set; }
        public DateTime? DeliveryDate { get; set; }
        public double OrderedQty { get; set; }
        public double CompletedQty { get; set; }
        public double TransitQty { get; set; }
        public double OpenQty { get; set; }
        //public double ItemOpenQty { get; set; }
        public double ASNQty { get; set; }
        public string UOM { get; set; }
        public string HSN { get; set; }
        public string PlantCode { get; set; }
        public double? UnitPrice { get; set; }
        public double? Value { get; set; }
        public double? TaxAmount { get; set; }
        public string TaxCode { get; set; }
        public bool IsFreightAvailable { get; set; }
        public double? FreightAmount { get; set; }
        public double? ToleranceUpperLimit { get; set; }
        public double? ToleranceLowerLimit { get; set; }
    }

    public class BPCASNItemView1 : CommonClass
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
        public string ASNNumber { get; set; }
        [MaxLength(20)]
        public string DocNumber { get; set; }
        [MaxLength(4)]
        public string Item { get; set; }
        public string Material { get; set; }
        public string MaterialText { get; set; }
        public DateTime? DeliveryDate { get; set; }
        public double OrderedQty { get; set; }
        public double CompletedQty { get; set; }
        public double TransitQty { get; set; }
        public double OpenQty { get; set; }
        public double? ItemOpenQty { get; set; }
        public double ASNQty { get; set; }
        public string UOM { get; set; }
        public string HSN { get; set; }
        public string PlantCode { get; set; }
        public double? UnitPrice { get; set; }
        public double? Value { get; set; }
        public double? TaxAmount { get; set; }
        public string TaxCode { get; set; }
        public bool IsFreightAvailable { get; set; }
        public double? FreightAmount { get; set; }
        public double? ToleranceUpperLimit { get; set; }
        public double? ToleranceLowerLimit { get; set; }
        public List<BPCASNItemBatch1> ASNItemBatches { get; set; }
        public List<BPCASNItemSES1> ASNItemSESes { get; set; }
    }

    public class BPCASNItemView2
    {
        public string ASNNumber { get; set; }
        public string DocNumber { get; set; }
        public string Item { get; set; }
        public string Material { get; set; }
        public string MaterialText { get; set; }
        public DateTime? DeliveryDate { get; set; }
        public double OrderedQty { get; set; }
        public double ASNQty { get; set; }
        public string UOM { get; set; }
        public string PlantCode { get; set; }

    }

    [Table("BPC_ASN_I_Batch1")]
    public class BPCASNItemBatch1 : CommonClass
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
        public string ASNNumber { get; set; }
        [MaxLength(20)]
        public string DocNumber { get; set; }
        [MaxLength(4)]
        public string Item { get; set; }
        [MaxLength(24)]
        public string Batch { get; set; }
        public double Qty { get; set; }
        public DateTime? ManufactureDate { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public string ManufactureCountry { get; set; }
    }

    [Table("BPC_ASN_I_SES1")]
    public class BPCASNItemSES1 : CommonClass
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
        public string ASNNumber { get; set; }
        [MaxLength(20)]
        public string DocNumber { get; set; }
        [MaxLength(4)]
        public string Item { get; set; }
        [MaxLength(24)]
        public string ServiceNo { get; set; }
        public string ServiceItem { get; set; }
        public double OrderedQty { get; set; }
        public double OpenQty { get; set; }
        public double ServiceQty { get; set; }
    }

    [Table("BPC_ASN_Pack")]
    public class BPCASNPack : CommonClass
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
        public string ASNNumber { get; set; }
        //public string Item { get; set; }
        [MaxLength(12)]
        public string PackageID { get; set; }
        public string ReferenceNumber { get; set; }
        public string Dimension { get; set; }
        public double? GrossWeight { get; set; }
        public string GrossWeightUOM { get; set; }
        public double? NetWeight { get; set; }
        public string NetWeightUOM { get; set; }
        public double? VolumetricWeight { get; set; }
        public string VolumetricWeightUOM { get; set; }
    }

    [Table("BPC_FLIP_H")]
    public class BPCFLIPHeader : CommonClass
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
        public string FLIPID { get; set; }
        public string GSTIN { get; set; }
        public string DocNumber { get; set; }
        public string Plant { get; set; }
        public string ProfitCentre { get; set; }
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

    [Table("BPC_FLIP_I")]
    public class BPCFLIPItem : CommonClass
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
        public string FLIPID { get; set; }
        [MaxLength(4)]
        public string Item { get; set; }
        public string Material { get; set; }
        public string MaterialText { get; set; }
        public DateTime? DeliveryDate { get; set; }
        public double? OrderedQty { get; set; }
        public double? OpenQty { get; set; }
        public double InvoiceQty { get; set; }
        public string UOM { get; set; }
        public string HSN { get; set; }
        public double Price { get; set; } // OpenQty 30 means [20 InvoiceQty(EnterFromHTML)*Price(EnterFromHTML)*Tax(from PO Item)]
        public string TaxType { get; set; }
        public double Tax { get; set; }
        public double Amount { get; set; }
    }

    [Table("BPC_FLIP_Cost")]
    public class BPCFLIPCost : CommonClass
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
        public string FLIPID { get; set; }
        [MaxLength(10)]
        public string ExpenceType { get; set; }
        public double Amount { get; set; }
        public string Remarks { get; set; }
    }

    [Table("BPC_FLIP_Attachment")]
    public class BPCFLIPAttachment : CommonClass
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AttachmentID { get; set; }
        public string PO { get; set; }
        [MaxLength(12)]
        public string FLIPID { get; set; }
        public string AttachmentName { get; set; }
        public string ContentType { get; set; }
        public long ContentLength { get; set; }
        public byte[] AttachmentFile { get; set; }
    }

    public class BPCFLIPHeaderView : CommonClass
    {
        public int ID { get; set; }
        public string Client { get; set; }
        public string Company { get; set; }
        public string Type { get; set; }
        public string PatnerID { get; set; }
        public string FLIPID { get; set; }
        public string ProfitCentre { get; set; }
        public string GSTIN { get; set; }
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
        public string Plant { get; set; }
        public List<BPCFLIPCost> FLIPCosts { get; set; }
        public List<BPCFLIPItem> FLIPItems { get; set; }
    }

    public class BPCASNView : CommonClass
    {
        public string Client { get; set; }
        public string Company { get; set; }
        public string Type { get; set; }
        public string PatnerID { get; set; }
        public string ASNNumber { get; set; }
        public DateTime? ASNDate { get; set; }
        public string DocNumber { get; set; }
        public string TransportMode { get; set; }
        public string VessleNumber { get; set; }
        public string CountryOfOrigin { get; set; }
        public string AWBNumber { get; set; }
        public DateTime? AWBDate { get; set; }
        public DateTime? DepartureDate { get; set; }
        public DateTime? ArrivalDate { get; set; }
        public string ShippingAgency { get; set; }
        public double? GrossWeight { get; set; }
        public string GrossWeightUOM { get; set; }
        public double? NetWeight { get; set; }
        public string NetWeightUOM { get; set; }
        public double? VolumetricWeight { get; set; }
        public string VolumetricWeightUOM { get; set; }
        public int? NumberOfPacks { get; set; }
        public string InvoiceNumber { get; set; }
        public DateTime? InvoiceDate { get; set; }
        public double? POBasicPrice { get; set; }
        public double? TaxAmount { get; set; }
        public double? InvoiceAmount { get; set; }
        public string InvoiceAmountUOM { get; set; }
        public string InvAttachmentName { get; set; }
        public string InvDocReferenceNo { get; set; }
        public string Plant { get; set; }
        public string Status { get; set; }
        public bool IsSubmitted { get; set; }
        public int ArrivalDateInterval { get; set; }
        public string BillOfLading { get; set; }
        public string TransporterName { get; set; }
        public double? AccessibleValue { get; set; }
        public string ContactPerson { get; set; }
        public string ContactPersonNo { get; set; }
        public string Field1 { get; set; }
        public string Field2 { get; set; }
        public string Field3 { get; set; }
        public string Field4 { get; set; }
        public string Field5 { get; set; }
        public string Field6 { get; set; }
        public string Field7 { get; set; }
        public string Field8 { get; set; }
        public string Field9 { get; set; }
        public string Field10 { get; set; }
        public bool IsBuyerApprovalRequired { get; set; }
        public string BuyerApprovalStatus { get; set; }
        public DateTime? BuyerApprovalOn { get; set; }
        public List<BPCASNItemView> ASNItems { get; set; }
        //public List<BPCASNItemBatch> ASNItemBatches { get; set; }
        public List<BPCASNPack> ASNPacks { get; set; }
        public List<BPCDocumentCenter> DocumentCenters { get; set; }
    }

    public class BPCASNView1 : CommonClass
    {
        public string Client { get; set; }
        public string Company { get; set; }
        public string Type { get; set; }
        public string PatnerID { get; set; }
        public string ASNNumber { get; set; }
        public DateTime? ASNDate { get; set; }
        public string DocNumber { get; set; }
        public string TransportMode { get; set; }
        public string VessleNumber { get; set; }
        public string CountryOfOrigin { get; set; }
        public string AWBNumber { get; set; }
        public DateTime? AWBDate { get; set; }
        public DateTime? DepartureDate { get; set; }
        public DateTime? ArrivalDate { get; set; }
        public string ShippingAgency { get; set; }
        public double? GrossWeight { get; set; }
        public string GrossWeightUOM { get; set; }
        public double? NetWeight { get; set; }
        public string NetWeightUOM { get; set; }
        public double? VolumetricWeight { get; set; }
        public string VolumetricWeightUOM { get; set; }
        public int? NumberOfPacks { get; set; }
        public string InvoiceNumber { get; set; }
        public DateTime? InvoiceDate { get; set; }
        public double? POBasicPrice { get; set; }
        public double? TaxAmount { get; set; }
        public double? InvoiceAmount { get; set; }
        public string InvoiceAmountUOM { get; set; }
        public string InvAttachmentName { get; set; }
        public string InvDocReferenceNo { get; set; }
        public string Plant { get; set; }
        public string Status { get; set; }
        public bool IsSubmitted { get; set; }
        public int ArrivalDateInterval { get; set; }
        public string BillOfLading { get; set; }
        public string TransporterName { get; set; }
        public double? AccessibleValue { get; set; }
        public string ContactPerson { get; set; }
        public string ContactPersonNo { get; set; }
        public string Field1 { get; set; }
        public string Field2 { get; set; }
        public string Field3 { get; set; }
        public string Field4 { get; set; }
        public string Field5 { get; set; }
        public string Field6 { get; set; }
        public string Field7 { get; set; }
        public string Field8 { get; set; }
        public string Field9 { get; set; }
        public string Field10 { get; set; }
        public bool IsBuyerApprovalRequired { get; set; }
        public string BuyerApprovalStatus { get; set; }
        public DateTime? BuyerApprovalOn { get; set; }
        public List<string> DocNumbers { get; set; }
        public List<BPCASNItemView1> ASNItems { get; set; }
        //public List<BPCASNItemBatch> ASNItemBatches { get; set; }
        public List<BPCASNPack> ASNPacks { get; set; }
        public List<BPCDocumentCenter> DocumentCenters { get; set; }
    }
    public class BPCASNViewApproval
    {
        public string Client { get; set; }
        public string Company { get; set; }
        public string Type { get; set; }
        public string PatnerID { get; set; }
        public string ASNNumber { get; set; }
        public bool IsBuyerApprovalRequired { get; set; }
        public string BuyerApprovalStatus { get; set; }
        public DateTime? BuyerApprovalOn { get; set; }
    }
    public class BPCASNView2
    {
        public string Vendor { get; set; }
        public string ASNNumber { get; set; }
        public DateTime? ASNDate { get; set; }
        public string DocNumber { get; set; }
        public string VehicleNumber { get; set; }
        public double? GrossWeight { get; set; }
        public double? NetWeight { get; set; }
        public string UOM { get; set; }
        public string AWBNumber { get; set; }
        public DateTime? AWBDate { get; set; }
        public string InvoiceNumber { get; set; }
        public DateTime? InvoiceDate { get; set; }
        public string Plant { get; set; }
        public string GateEntryNo { get; set; }
        public DateTime? GateEntryDate { get; set; }
        public List<BPCASNItemView2> ASNItems { get; set; }
    }

    public class CancelASNView : CommonClass
    {
        public string Client { get; set; }
        public string Company { get; set; }
        public string Type { get; set; }
        public string PatnerID { get; set; }
        public string ASNNumber { get; set; }
        public string DocNumber { get; set; }
    }

    [Table("BPC_DocumentCenter")]
    public class BPCDocumentCenter : CommonClass
    {
        [Key, Column(Order = 1)]
        public int ID { get; set; }
        public string ASNNumber { get; set; }
        public string DocumentType { get; set; }
        public string DocumentTitle { get; set; }
        public string Filename { get; set; }
        public string AttachmentReferenceNo { get; set; }
    }

    [Table("BPC_ASN_PRE_SHIP_MASTER")]
    public class BPCASNPreShipmentMaster : CommonClass
    {
        [Key, Column(Order = 1)]
        public int ID { get; set; }
        public int PreShipmentDay { get; set; }
    }

    public class BPCAttachment : CommonClass
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
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
    [Table("BPC_GSTIN")]
    public class BPC_GSTIN 
    {
        public int ID { get; set; }
        public string GSTIN{ get; set; }
        public string State { get; set; }
        public string Plant { get; set; }

    }
   
    public class BPCInvoiceAttachment
    {
        public int AttachmentID { get; set; }
        public string AttachmentName { get; set; }
        public string ContentType { get; set; }
        public long ContentLength { get; set; }
        public string ReferenceNo { get; set; }
        public string Client { get; set; }
        public string Company { get; set; }
        public string Type { get; set; }
        public string PatnerID { get; set; }

    }

    public class BPCASNAttachment
    {
        public string ASNNumber { get; set; }
        public int AttachmentID { get; set; }
        public string AttachmentName { get; set; }
        public string Type { get; set; }
        public string DocumentType { get; set; }
        public string Title { get; set; }
    }

    public class ASNListView
    {
        public string Client { get; set; }
        public string Company { get; set; }
        public string Type { get; set; }
        public string PatnerID { get; set; }
        public string ASNNumber { get; set; }
        public DateTime? ASNDate { get; set; }
        public DateTime? ArrivalDate { get; set; }
        public DateTime? DepartureDate { get; set; }
        public DateTime? CancelDuration { get; set; }
        public string DocNumber { get; set; }
        public string VessleNumber { get; set; }
        public string AWBNumber { get; set; }
        public string Plant { get; set; }
        //public string Material { get; set; }
        //public string MaterialText { get; set; }
        //public double ASNQty { get; set; }
        public string Status { get; set; }
        public string InvoiceNumber { get; set; }
        public string InvDocReferenceNo { get; set; }
        public string InvoiceAttachmentName { get; set; }
        public double? POBasicPrice { get; set; }
        public double? TaxAmount { get; set; }
        public double? InvoiceAmount { get; set; }
        public int DocCount { get; set; }
        public bool IsBuyerApprovalRequired { get; set; }
        public string BuyerApprovalStatus { get; set; }
        public DateTime? BuyerApprovalOn { get; set; }
    }

    public class GateEntry
    {
        public string ASNNumber { get; set; }
        public string GateEntryNo { get; set; }
        public DateTime? GateEntryDate { get; set; }
        public string GateEntryCreatedBy { get; set; }

    }

    public class ASNListFilter
    {
        public string VendorCode { get; set; }
        public List<string> Plants { get; set; }
        public string ASNNumber { get; set; }
        public string DocNumber { get; set; }
        public string Material { get; set; }
        public string Status { get; set; }
        public DateTime? ASNFromDate { get; set; }
        public DateTime? ASNToDate { get; set; }
    }
    public class OCRValue
    {
        public Boolean GST { get; set; }
        public string Amount { get; set; }
        public string InvoiceNumber {  get; set; }
        public string GstValue { get; set; }
        public string vendor_gst { get; set; }
    }
    public class PythonOCRValue
    {
        public string buyer_gst { get; set; }
        public string amount { get; set; }
        public string invoice_number { get; set; }
        public string vendor_gst { get; set; }
        // public string GstValue { get; set; }
    }
    public class MyPayload
    {
        public string encode { get; set; }
    }
    [Table("BP_Ticket_Status")]
    public class BPTicketStatus
    {
        [Key]
        public string ID { get; set; }
        
        public string Ticket_status { get; set; }
    }
}
