using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BPCloud_VP.ReportService.Models
{
    public class CommonClass
    {
        [Required]
        public bool IsActive { get; set; }
        [Required]
        public DateTime CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public string ModifiedBy { get; set; }
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
        public string ASN { get; set; }
        public DateTime? InvoiceDate { get; set; }
        public DateTime? ASNDate { get; set; }
        public DateTime? PostingDate { get; set; }
        [Required]
        public double InvoiceAmount { get; set; }
        public string PoReference { get; set; }
        [Required]
        public double PaidAmount { get; set; }
        public string Currency { get; set; }
        public DateTime? DateofPayment { get; set; }
        public string Plant { get; set; }
        public string Status { get; set; }
        public string AttID { get; set; }
        public DateTime? PODDate { get; set; }
        public string PODConfirmedBy { get; set; }
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
        [MaxLength(4)]
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


    [Table("BPC_PAY")]
    public class BPCPayment : CommonClass
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
        [MaxLength(10)]
        public string PaymentDoc { get; set; }
        public DateTime? Date { get; set; }
        public double? Amount { get; set; }
        public string Currency { get; set; }
        public string Plant { get; set; }
        public string Remark { get; set; }
        public string Attachment { get; set; }
    }

    [Table("BPC_SC_STK")]
    public class BPCSCSTK : CommonClass
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
        public string PODocument { get; set; }
        [MaxLength(4)]
        public string Item { get; set; }
        public DateTime? Date { get; set; }
        public string Material { get; set; }
        public string Plant { get; set; }
        public double? IssuedQty { get; set; }
        public double? StockQty { get; set; }
    }

    public class BPCInvoiceXLSX
    {
        public string Company { get; set; }
        public string Patnerid { get; set; }
        public string Client { get; set; }
        public string Type { get; set; }
        public string Fiscalyear { get; set; }
        public string Invno { get; set; }
        public double Paidamt { get; set; }
        public string Currency { get; set; }
        public string Dateofpayment { get; set; }
        public string Poddate { get; set; }
        public string Invoicedate { get; set; }
        public string Invoiceamount { get; set; }
        public string Podconfirmedby { get; set; }
        public string POreference { get; set; }
        public string Status { get; set; }
    }

    public class BPCPaymentXLSX
    {
        public string Company { get; set; }
        public string Patnerid { get; set; }
        public string Fiscalyear { get; set; }
        public string Type { get; set; }
        public double Amount { get; set; }
        public string Currency { get; set; }
        public string Date { get; set; }
        public string Paydoc { get; set; }
        public string Client { get; set; }
        //public string Company { get; set; }
        //public string Type { get; set; }
        //public string PatnerID { get; set; }
        //public string FiscalYear { get; set; }
        //public string PaymentDoc { get; set; }
        //public string Date { get; set; }
        //public double Amount { get; set; }
        //public string Currency { get; set; }
        public string Remark { get; set; }
        public string Attachment { get; set; }
    }

    [Table("BPC_REP_DOL")]
    public class BPCReportDOL : CommonClass
    {
        [MaxLength(3)]
        public string Client { get; set; }
        [MaxLength(4)]
        public string Company { get; set; }
        [MaxLength(1)]
        public string Type { get; set; }
        [MaxLength(12)]
        public string PatnerID { get; set; }
        public string Material { get; set; }
        public string MaterialText { get; set; }
        public string Status { get; set; }
    }

    [Table("BPC_REP_FGCPS")]
    public class BPCReportFGCPS : CommonClass
    {
        [MaxLength(3)]
        public string Client { get; set; }
        [MaxLength(4)]
        public string Company { get; set; }
        [MaxLength(1)]
        public string Type { get; set; }
        [MaxLength(12)]
        public string PatnerID { get; set; }
        [MaxLength(450)]
        public string Plant { get; set; }
        [MaxLength(450)]
        public string Material { get; set; }
        public string MaterialText { get; set; }
        public double? StickQty { get; set; }
        public string UOM { get; set; }
        public string Batch { get; set; }
        public double? Price { get; set; }
        public string Status { get; set; }
    }

    [Table("BPC_REP_GRR")]
    public class BPCReportGRR : CommonClass
    {
        [MaxLength(3)]
        public string Client { get; set; }
        [MaxLength(4)]
        public string Company { get; set; }
        [MaxLength(1)]
        public string Type { get; set; }
        [MaxLength(12)]
        public string PatnerID { get; set; }
        [MaxLength(450)]
        public string Material { get; set; }
        public string MaterialText { get; set; }
        public double? OrderQty { get; set; }
        public double? ReceivedQty { get; set; }
        public double? RejectedPPM { get; set; }
        public double? ReworkQty { get; set; }
        public string Status { get; set; }
    }

    [Table("BPC_REP_IP")]
    public class BPCReportIP : CommonClass
    {
        [MaxLength(3)]
        public string Client { get; set; }
        [MaxLength(4)]
        public string Company { get; set; }
        [MaxLength(1)]
        public string Type { get; set; }
        [MaxLength(12)]
        public string PatnerID { get; set; }
        [MaxLength(450)]
        public string Material { get; set; }
        public string MaterialText { get; set; }
        public string MaterialChar { get; set; }
        public string Desc { get; set; }
        public double? LowerLimit { get; set; }
        public double? UpperLimit { get; set; }
        public string UOM { get; set; }
        public string Method { get; set; }
        public string ModRule { get; set; }
        public string Status { get; set; }
    }

    [Table("BPC_REP_OV")]
    public class BPCReportOV : CommonClass
    {
        [MaxLength(3)]
        public string Client { get; set; }
        [MaxLength(4)]
        public string Company { get; set; }
        [MaxLength(1)]
        public string Type { get; set; }
        [MaxLength(12)]
        public string PatnerID { get; set; }
        [MaxLength(450)]
        public string Material { get; set; }
        public string MaterialText { get; set; }
        public double? InputQty { get; set; }
        public double? AccQty { get; set; }
        public double? RejQty { get; set; }
        public double? RejPercentage { get; set; }
        public string Status { get; set; }
    }

    [Table("BPC_REP_PPM_H")]
    public class BPCReportPPMHeader : CommonClass
    {
        [MaxLength(3)]
        public string Client { get; set; }
        [MaxLength(4)]
        public string Company { get; set; }
        [MaxLength(1)]
        public string Type { get; set; }
        [MaxLength(12)]
        public string PatnerID { get; set; }
        public DateTime Period { get; set; }
        public double? ReceiptQty { get; set; }
        public double? RejectedQty { get; set; }
        public double? PPM { get; set; }
        public double? TotalPPM { get; set; }
    }

    [Table("BPC_REP_PPM_I")]
    public class BPCReportPPMItem : CommonClass
    {
        [MaxLength(3)]
        public string Client { get; set; }
        [MaxLength(4)]
        public string Company { get; set; }
        [MaxLength(1)]
        public string Type { get; set; }
        [MaxLength(12)]
        public string PatnerID { get; set; }
        public DateTime Period { get; set; }
        [MaxLength(450)]
        public string Material { get; set; }
        public string MaterialText { get; set; }
        public double? ReceiptQty { get; set; }
        public double? RejectedQty { get; set; }
        public double? PPM { get; set; }
    }

    [Table("BPC_REP_VR")]
    public class BPCReportVR : CommonClass
    {
        [MaxLength(3)]
        public string Client { get; set; }
        [MaxLength(4)]
        public string Company { get; set; }
        [MaxLength(1)]
        public string Type { get; set; }
        [MaxLength(12)]
        public string PatnerID { get; set; }
        [MaxLength(450)]
        public string Material { get; set; }
        public string MaterialText { get; set; }
        public double? OrderQty { get; set; }
        public double? ReceivedQty { get; set; }
        public double? RejectedPPM { get; set; }
        public double? ReworkQty { get; set; }
        public string Status { get; set; }
    }

    public class OverviewReportOption
    {
        public string PartnerID { get; set; }
        public string Material { get; set; }
        public string PO { get; set; }
        public string Status { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }

    }
    public class PPMReportOption
    {
        public string PartnerID { get; set; }
        public string Material { get; set; }
        public string PO { get; set; }
        public string Status { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }

    }

    public class VendorRatingReportOption
    {
        public string PartnerID { get; set; }
        public string Material { get; set; }
        public string PO { get; set; }
        public string Status { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }

    }
    public class BPCLog : CommonClass
    {
        [Key]
        public int LogID { get; set; }
        public DateTime LogDate { get; set; }
        public string APIMethod { get; set; }
        public int NoOfRecords { get; set; }
        public string Status { get; set; }
        public string Response { get; set; }
    }
}
