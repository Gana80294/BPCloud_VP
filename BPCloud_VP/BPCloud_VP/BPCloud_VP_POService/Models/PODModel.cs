using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BPCloud_VP_POService.Models
{
    [Table("BPC_POD_H")]
    public class BPCPODHeader : CommonClass
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
        public string InvoiceNumber { get; set; }
        public DateTime? InvoiceDate { get; set; }
        public string TruckNumber { get; set; }
        public string VessleNumber { get; set; }
        public double? Amount { get; set; }
        public string Currency { get; set; }
        public string Transporter { get; set; }
        public string Driver { get; set; }
        public string DriverContactNo { get; set; }
        public string Plant { get; set; }
        public string Status { get; set; }
    }

    [Table("BPC_POD_I")]
    public class BPCPODItem : CommonClass
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
        public string InvoiceNumber { get; set; }
        public string Item { get; set; }
        public string Material { get; set; }
        public string MaterialText { get; set; }
        public double Qty { get; set; }
        public double? ReceivedQty { get; set; }
        public double? BreakageQty { get; set; }
        public double? MissingQty { get; set; }
        public double? AcceptedQty { get; set; }
        public string Reason { get; set; }
        public string Remarks { get; set; }
        public string AttachmentName { get; set; }
        public string AttachmentReferenceNo { get; set; }
    }

    public class BPCPODView : CommonClass
    {
        public string Client { get; set; }
        public string Company { get; set; }
        public string Type { get; set; }
        public string PatnerID { get; set; }
        public string DocNumber { get; set; }
        public string InvoiceNumber { get; set; }
        public DateTime? InvoiceDate { get; set; }
        public string TruckNumber { get; set; }
        public string VessleNumber { get; set; }
        public double? Amount { get; set; }
        public string Currency { get; set; }
        public string Transporter { get; set; }
        public string Status { get; set; }
        public string Recived_status { get; set; }
        public String Doc { get; set; }
        public List<BPCPODItem> PODItems { get; set; }
    }
}
