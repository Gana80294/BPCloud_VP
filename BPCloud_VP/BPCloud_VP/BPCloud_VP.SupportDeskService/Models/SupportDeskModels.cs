using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BPCloud_VP.SupportDeskService.Models
{
    public class CommonClass
    {
        public bool IsActive { get; set; }
        public DateTime CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public string ModifiedBy { get; set; }
    }

    [Table("BPC_SM")]
    public class SupportMaster : CommonClass
    {
        [MaxLength(3)]
        public string Client { get; set; }
        [MaxLength(4)]
        public string Company { get; set; }
        [MaxLength(1)]
        public string Type { get; set; }
        [MaxLength(24)]
        public string ReasonCode { get; set; }
        public string ReasonText { get; set; }
    }

    public class SupportMasterViews
    {
        public string ReasonCode { get; set; }
        public string ReasonText { get; set; }
    }

    [Table("BPC_SAM")]
    public class SupportAppMaster : CommonClass
    {
        [MaxLength(3)]
        public string Client { get; set; }
        [MaxLength(4)]
        public string Company { get; set; }
        [MaxLength(1)]
        public string Type { get; set; }
        [MaxLength(24)]
        public string AppID { get; set; }
        public string AppName { get; set; }
    }

    [Table("BPC_SH")]
    public class SupportHeader 
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
        public string SupportID { get; set; }
        public string ReasonCode { get; set; }
        public string Plant { get; set; }
        public DateTime? Date { get; set; }
        public string DocumentRefNo { get; set; }
        public string AppID { get; set; }
        public string AttachmentID { get; set; }
        public string Remarks { get; set; }
        public string Status { get; set; }
        public bool IsResolved { get; set; }
        public bool IsActive { get; set; }
        public string DocCount { get; set; }
        public DateTime? CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public string ModifiedBy { get; set; }

    }

    

    [Table("BPC_SL")]
    public class SupportLog : CommonClass
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
        public string SupportID { get; set; }
        [MaxLength(12)]
        public string SupportLogID { get; set; }
        public string Remarks { get; set; }
        public string AttachmentID { get; set; }
        public bool IsResolved { get; set; }
    }

    [Table("BPC_Support_Attachment")]
    public class BPCSupportAttachment : CommonClass
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AttachmentID { get; set; }
        public string SupportID { get; set; }
        public string SupportLogID { get; set; }
        public string AttachmentName { get; set; }
        public string ContentType { get; set; }
        public long ContentLength { get; set; }
        public byte[] AttachmentFile { get; set; }
    }

    [Table("BPC_FAQ_Attachment")]
    public class BPCFAQAttachment : CommonClass
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int FAQAttachmentID { get; set; }
        public string AttachmentName { get; set; }
        public string ContentType { get; set; }
        public long ContentLength { get; set; }
        public byte[] AttachmentFile { get; set; }
    }

    public class SupportDetails
    {
        public SupportHeaderView supportHeader { get; set; }
        public List<SupportLog> supportLogs { get; set; }
        public List<BPCSupportAttachment> supportAttachments { get; set; }
    }

    public class HelpDeskAdminDetails
    {
        public string PatnerID { get; set; }
        public List<string> Companies { get; set; }
        public List<string> Plants { get; set; }
        public List<string> ReasonCodes { get; set; }
    }

    public class UserView
    {
        public Guid UserID { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
    }
    public class SupportHeaderView : CommonClass
    {
        public string SupportID { get; set; }
        public string Client { get; set; }
        public string Company { get; set; }
        public string Type { get; set; }
        public string PatnerID { get; set; }
        public string AppID { get; set; }
        public string Plant { get; set; }
        public string ReasonCode { get; set; }
        public string DocCount { get; set; }
        public DateTime? Date { get; set; }
        public string AssignTo { get; set; }
        public string Status { get; set; }
        public string Remarks { get; set; }
        public string DocumentRefNo { get; set; }
        public bool IsResolved { get; set; }
        public string Reason { get; set; }
        public List<User> Users { get; set; }
    }

    public class SupportLogView : CommonClass
    {
        public int ID { get; set; }
        public string SupportID { get; set; }
        public string Client { get; set; }
        public string Company { get; set; }
        public string Type { get; set; }
        public string PatnerID { get; set; }
        public string Status { get; set; }
        public string Remarks { get; set; }
        public string PatnerEmail { get; set; }
        public bool IsResolved { get; set; }
    }

    public class User : CommonClass
    {
        public Guid UserID { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string ContactNumber { get; set; }

    }
    public class Ticketsearch
    {

        public string DocNumber { get; set; }
        public string Status { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public string PartnerID { get; set; }
        public string[] Plant { get; set; }

    }
}
