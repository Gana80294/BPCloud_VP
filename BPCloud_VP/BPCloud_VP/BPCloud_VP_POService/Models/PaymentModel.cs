using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BPCloud_VP_POService.Models
{
    [Table("BPC_PAY_AS")]
    public class BPCPayAccountStatement : CommonClass
    {
        [MaxLength(3)]
        public string Client { get; set; }
        [MaxLength(4)]
        public string Company { get; set; }
        [MaxLength(1)]
        public string Type { get; set; }
        [MaxLength(12)]
        public string PartnerID { get; set; }
        [MaxLength(4)]
        public string FiscalYear { get; set; }
        [MaxLength(10)]
        public string DocumentNumber { get; set; }
        public DateTime? DocumentDate { get; set; }
        public string InvoiceNumber { get; set; }
        public DateTime? InvoiceDate { get; set; }
        public double InvoiceAmount { get; set; }
        public double BalanceAmount { get; set; }
        public double PaidAmount { get; set; }
        public double? AdvanceAmount { get; set; }
        public double? TDS { get; set; }
        public DateTime? DueDate { get; set; }
        public string Plant { get; set; }
        public string ProfitCenter { get; set; }
        public string Reference { get; set; }
        public string Status { get; set; }
        public DateTime? AcceptedOn { get; set; }
        public string AcceptedBy { get; set; }
    }

    [Table("BPC_PAY_TDS")]
    public class BPCPayTDS : CommonClass
    {
        [MaxLength(3)]
        public string Client { get; set; }
        [MaxLength(4)]
        public string Company { get; set; }
        [MaxLength(1)]
        public string Type { get; set; }
        [MaxLength(12)]
        public string PartnerID { get; set; }
        [MaxLength(4)]
        public string FiscalYear { get; set; }
        public string CompanyCode { get; set; }
        public string DocumentID { get; set; }
        public DateTime? PostingDate { get; set; }
        public double BaseAmount { get; set; }
        public string TDSCategory { get; set; }
        public double TDSAmount { get; set; }
        public string Currency { get; set; }
    }

    [Table("BPC_PAY_PAYABLE")]
    public class BPCPayPayable : CommonClass
    {
        [MaxLength(3)]
        public string Client { get; set; }
        [MaxLength(4)]
        public string Company { get; set; }
        [MaxLength(1)]
        public string Type { get; set; }
        [MaxLength(12)]
        public string PartnerID { get; set; }
        [MaxLength(4)]
        public string FiscalYear { get; set; }
        [MaxLength(16)]
        public string Invoice { get; set; }
        //public string InvoiceBooking { get; set; }
        public DateTime? InvoiceDate { get; set; }
        public DateTime? PostedOn { get; set; }
        public DateTime? DueDate { get; set; }
        public double AdvAmount { get; set; }
        public double Amount { get; set; }
        public double Balance { get; set; }
        public string Currency { get; set; }
    }

    [Table("BPC_PAY_PAYMENT")]
    public class BPCPayPayment : CommonClass
    {
        [MaxLength(3)]
        public string Client { get; set; }
        [MaxLength(4)]
        public string Company { get; set; }
        [MaxLength(1)]
        public string Type { get; set; }
        [MaxLength(12)]
        public string PartnerID { get; set; }
        [MaxLength(4)]
        public string FiscalYear { get; set; }
        [MaxLength(10)]
        public string DocumentNumber { get; set; }
        public DateTime? PaymentDate { get; set; }
        public string PaymentType { get; set; }
        public double PaidAmount { get; set; }
        public string BankName { get; set; }
        public string BankAccount { get; set; }
    }

    [Table("BPC_PAY_RECORD")]
    public class BPCPayRecord : CommonClass
    {
        [MaxLength(3)]
        public string Client { get; set; }
        [MaxLength(4)]
        public string Company { get; set; }
        [MaxLength(1)]
        public string Type { get; set; }
        [MaxLength(12)]
        public string PartnerID { get; set; }
        [MaxLength(10)]
        public string DocumentNumber { get; set; }
        public string InvoiceNumber { get; set; }
        public string PayRecordNo { get; set; }
        public double PaidAmount { get; set; }
        public string UOM { get; set; }
        public string Medium { get; set; }
        public DateTime? PaymentDate { get; set; }
        public string Time { get; set; }
        public string Remarks { get; set; }
        public string RefNumber { get; set; }
        public string PayDoc { get; set; }
    }

    [Table("BPC_PAY_DIS")]
    public class BPCPayDiscount : CommonClass
    {
        [MaxLength(3)]
        public string Client { get; set; }
        [MaxLength(4)]
        public string Company { get; set; }
        [MaxLength(1)]
        public string Type { get; set; }
        [MaxLength(12)]
        public string PartnerID { get; set; }
        [MaxLength(4)]
        public string FiscalYear { get; set; }
        [MaxLength(10)]
        public string DocumentNumber { get; set; }
        public DateTime? DocumentDate { get; set; }
        public string InvoiceNumber { get; set; }
        public DateTime? InvoiceDate { get; set; }
        public double InvoiceAmount { get; set; }
        public double PaidAmount { get; set; }
        public double BalanceAmount { get; set; }
        public DateTime? DueDate { get; set; }
        public DateTime? ProposedDueDate { get; set; }
        public double ProposedDiscount { get; set; }
        public double PostDiscountAmount { get; set; }
        public string ProfitCenter { get; set; }
        public string Status { get; set; }
        public DateTime? ApprovedOn { get; set; }
        public string ApprovedBy { get; set; }
    }
    [Table("BPC_PAY_DIS_MASTER")]
    public class BPCPayDiscountMaster : CommonClass
    {
        [Key]
        public int ID { get; set; }
        [MaxLength(4)]
        public string FiscalYear { get; set; }
        public double Amount { get; set; }
        public int Days { get; set; }
        public double Discount { get; set; }
        public string ProfitCenter { get; set; }
    }

}
