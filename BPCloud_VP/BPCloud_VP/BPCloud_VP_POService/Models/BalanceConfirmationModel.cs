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
        public double BillAmount { get; set; }
        public double PaidAmont { get; set; }
        public double TDSAmount { get; set; }
        public double TotalPaidAmount { get; set; }
        public double DownPayment { get; set; }
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
        public string DocNumber { get; set; }
        public DateTime? DocDate { get; set; }
        public string InvoiceNumber { get; set; }
        public double InvoiceAmount { get; set; }
        public double BillAmount { get; set; }
        public double PaidAmont { get; set; }
        public double TDSAmount { get; set; }
        public double TotalPaidAmount { get; set; }
        public double DownPayment { get; set; }
        public double NetDueAmount { get; set; }
        public string Currency { get; set; }
        public DateTime? BalDate { get; set; }
    }

    public class ConfirmationDeatils
    {
        public string ConfirmedBy { get; set; }
    }
}
