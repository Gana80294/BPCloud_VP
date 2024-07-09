using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BPCloud_VP_POService.Models
{
    [Table("BP_BC_H")]
    public class BalanceConfirmationHeader : CommonClass
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
        //public DateTime Period { get; set; }
        [Required]
        public double BillAmount { get; set; }
        [Required]
        public double PaidAmont { get; set; }
        [Required]
        public double TDSAmount { get; set; }
        [Required]
        public double TotalPaidAmount { get; set; }
        [Required]
        public double DownPayment { get; set; }
        [Required]
        public double NetDueAmount { get; set; }
        public string Currency { get; set; }
        public DateTime? BalDate { get; set; }
        public string Plant { get; set; }
        public string Status { get; set; }
        public DateTime? AcceptedOn { get; set; }
        public string AcceptedBy { get; set; }
    }

    [Table("BP_BC_I")]
    public class BalanceConfirmationItem : CommonClass
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
        //public DateTime Period { get; set; }
        [Required]
        public string DocNumber { get; set; }
        public DateTime? DocDate { get; set; }
        public string InvoiceNumber { get; set; }
        [Required]
        public double InvoiceAmount { get; set; }
        [Required]
        public double BillAmount { get; set; }
        [Required]
        public double PaidAmont { get; set; }
        [Required]
        public double TDSAmount { get; set; }
        [Required]
        public double TotalPaidAmount { get; set; }
        [Required]
        public double DownPayment { get; set; }
        [Required]
        public double NetDueAmount { get; set; }
        public string Currency { get; set; }
        public DateTime? BalDate { get; set; }
    }

    public class ConfirmationDeatils
    {
        public string ConfirmedBy { get; set; }
    }
}
