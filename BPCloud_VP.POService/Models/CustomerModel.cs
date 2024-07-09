using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BPCloud_VP_POService.Models
{
    [Table("BPC_PI_H")]
    public class BPCPIHeader : CommonClass
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
        public string PIRNumber { get; set; }
        public string PIRType { get; set; }
        public string DocumentNumber { get; set; }
        public string Text { get; set; }
        public DateTime? Date { get; set; }
        public string ReferenceDoc { get; set; }
        public double? GrossAmount { get; set; }
        public double? NetAmount { get; set; }
        public string Currency { get; set; }
        //public string Plant { get; set; }
        public string Status { get; set; }
        public string UOM { get; set; }
        public string Material { get; set; }
        public string Description { get; set; }
        [Required]
        public double Qty { get; set; }
        public string Reason { get; set; }
        public string DeliveryNote { get; set; }
    }

    [Table("BPC_PI_I")]
    public class BPCPIItem : CommonClass
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
        public string PIRNumber { get; set; }
        [MaxLength(4)]
        public string Item { get; set; }
        public string ProdcutID { get; set; }
        public string Material { get; set; }
        public string MaterialText { get; set; }
        public DateTime? DeliveryDate { get; set; }
        public double? OrderQty { get; set; }
        public string UOM { get; set; }
        public string HSN { get; set; }
        public double? RetQty { get; set; }
        public string ReasonText { get; set; }
        public string FileName { get; set; }
        public string AttachmentReferenceNo { get; set; }
    }

    public class BPCPIView : CommonClass
    {
        public string Client { get; set; }
        public string Company { get; set; }
        public string Type { get; set; }
        public string PatnerID { get; set; }
        public string PIRNumber { get; set; }
        public string PIRType { get; set; }
        public string DocumentNumber { get; set; }
        public string Text { get; set; }
        public DateTime? Date { get; set; }
        public string ReferenceDoc { get; set; }
        public string Currency { get; set; }
        public string Status { get; set; }
        public double? GrossAmount { get; set; }
        public double? NetAmount { get; set; }
        public string UOM { get; set; }
        public List<BPCPIItem> Items { get; set; }
    }
    //mchng

    [Table("BPC_Ret_H")]
    public class BPCRetHeader : CommonClass
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
        public string RetReqID { get; set; }
        public string DocumentNumber { get; set; }
        public string CreditNote { get; set; }
        public string AWBNumber { get; set; }
        public string Transporter { get; set; }
        public string TruckNumber { get; set; }
        public string Text { get; set; }
        public DateTime? Date { get; set; }
        public string InvoiceDoc { get; set; }
        //public string Plant { get; set; }
        public string Status { get; set; }
    }

    [Table("BPC_Ret_I")]
    public class BPCRetItem : CommonClass
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
        public string RetReqID { get; set; }
        [MaxLength(4)]
        public string Item { get; set; }
        public string ProdcutID { get; set; }
        public string Material { get; set; }
        public string MaterialText { get; set; }
        public string InvoiceNumber { get; set; }
        [Required]
        public double OrderQty { get; set; }
        [Required]
        public double RetQty { get; set; }
        public string ReasonText { get; set; }
        public string FileName { get; set; }
        public string AttachmentReferenceNo { get; set; }
    }

    [Table("BPC_Ret_I_Batch")]
    public class BPCRetItemBatch : CommonClass
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
        public string RetReqID { get; set; }
        [MaxLength(4)]
        public string Item { get; set; }
        [MaxLength(24)]
        public string Batch { get; set; }
        //public double OrderQty { get; set; }
        [Required]
        public double RetQty { get; set; }
    }
    [Table("BPC_Ret_I_Serial")]
    public class BPCRetItemSerial : CommonClass
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
        public string RetReqID { get; set; }
        [MaxLength(4)]
        public string Item { get; set; }
        [MaxLength(24)]
        public string Serial { get; set; }
        //public double OrderQty { get; set; }
        [Required]
        public double RetQty { get; set; }
    }

    public class BPCRetView : CommonClass
    {
        [Key, Column(Order = 1)]
        public int ID { get; set; }
        public string Client { get; set; }
        public string Company { get; set; }
        public string Type { get; set; }
        public string PatnerID { get; set; }
        public string RetReqID { get; set; }
        public string DocumentNumber { get; set; }
        public string Text { get; set; }
        public DateTime? Date { get; set; }
        public string InvoiceDoc { get; set; }
        public string Status { get; set; }
        public List<BPCRetItem> Items { get; set; }
    }
    public class BPCRetView_new : CommonClass
    {
       
        public string Client { get; set; }
        public string Company { get; set; }
        public string Type { get; set; }
        public string PatnerID { get; set; }
        public string RetReqID { get; set; }
        public string DocumentNumber { get; set; }

        public string CreditNote { get; set; }
        public string AWBNumber { get; set; }
        public string Transporter { get; set; }
        public string TruckNumber { get; set; }
        public string Text { get; set; }
        public DateTime? Date { get; set; }
        public string InvoiceDoc { get; set; }
        public string Status { get; set; }
        public List<BPCRetItem> Items { get; set; }
        public List<BPCRetItemBatch> Batch { get; set; }
        public List<BPCRetItemSerial> Serial { get; set; }
    }

    [Table("BPC_Prod")]
    public class BPCProd : CommonClass
    {
        [MaxLength(3)]
        public string Client { get; set; }
        [MaxLength(4)]
        public string Company { get; set; }
        [MaxLength(1)]
        public string Type { get; set; }
        [MaxLength(18)]
        public string ProductID { get; set; }
        public string MaterialText { get; set; }
        public string MaterialType { get; set; }
        public string MaterialGroup { get; set; }
        public string AttID { get; set; }
        public string UOM { get; set; }
        public string Stock { get; set; }
        public string BasePrice { get; set; }
        public DateTime? StockUpdatedOn { get; set; }
    }

    [Table("BPC_Prod_Fav")]
    public class BPCProdFav : CommonClass
    {
        [MaxLength(3)]
        public string Client { get; set; }
        [MaxLength(4)]
        public string Company { get; set; }
        [MaxLength(1)]
        public string Type { get; set; }
        [MaxLength(12)]
        public string PatnerID { get; set; }
        [MaxLength(18)]
        public string ProductID { get; set; }
        [Required]
        public int Rating { get; set; }
    }
    public class BPCRetNew : CommonClass
    {
        public string ReturnOrder { get; set; }
        public DateTime? Date { get; set; }
        public string Material { get; set; }
        public string Text { get; set; }
        public double Qty { get; set; }
        public string Status { get; set; }
        public string Document { get; set; }
    }
}
