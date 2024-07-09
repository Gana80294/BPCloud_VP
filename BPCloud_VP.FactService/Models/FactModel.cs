using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BPCloud_VP.FactService.Models
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

    [Table("BPC_Fact")]
    public class BPCFact : CommonClass
    {
        
        [MaxLength(3)]
        public string Client { get; set; }
        [MaxLength(4)]
        public string Company { get; set; }
        [MaxLength(1)]
        public string Type { get; set; }
        [MaxLength(12)]
        public string PatnerID { get; set; }
        [MaxLength(40)]
        public string LegalName { get; set; }
        public string Name { get; set; }
        public string Role { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string PinCode { get; set; }
        public string Plant { get; set; }
        public string GSTNumber { get; set; }
        public string GSTStatus { get; set; }
        public string PANNumber { get; set; }
        public string Phone1 { get; set; }
        public string Phone2 { get; set; }
        public string Email1 { get; set; }
        public string Email2 { get; set; }
        public string TaxID1 { get; set; }
        public string TaxID2 { get; set; }
        [Required]
        public double OutstandingAmount { get; set; }
        [Required]
        public double CreditAmount { get; set; }
        [Required]
        public double LastPayment { get; set; }
        public DateTime? LastPaymentDate { get; set; }
        public string Currency { get; set; }
        [Required]
        public double CreditLimit { get; set; }
        [Required]
        public double CreditBalance { get; set; }
        [Required]
        public DateTime? CreditEvalDate { get; set; }
       
        [Required]
        public bool MSME { get; set; }
        public string MSME_TYPE { get; set; }
        public string MSME_Att_ID { get; set; }
        [Required]
        public bool Reduced_TDS { get; set; }
        //public string TDS_CAT { get; set; }
        public string TDS_RATE { get; set; }
        public string TDS_Att_ID { get; set; }
        public string TDS_Cert_No { get; set; }
        [Required]
        public bool RP { get; set; }
        public string RP_Name { get; set; }
        public string RP_Type { get; set; }
        public string RP_Att_ID { get; set; }
        [Required]
        public bool IsBlocked { get; set; }
        public string PurchaseOrg { get; set; }
        public string AccountGroup { get; set; }
        public string CompanyCode { get; set; }
        public string TypeofIndustry { get; set; }
        public string VendorCode { get; set; }


    }

    public class BPCFact1
    {
        public string PatnerID { get; set; }
        public string LegalName { get; set; }
        public string Name { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string City { get; set; }
        public string Phone1 { get; set; }
    }

    [Table("BPC_Fact_Support")]
    public class BPCFactSupport : CommonClass
    {
        [MaxLength(3)]
        public string Client { get; set; }
        [MaxLength(4)]
        public string Company { get; set; }
        [MaxLength(1)]
        public string Type { get; set; }
        [MaxLength(12)]
        public string PatnerID { get; set; }
        [MaxLength(40)]
        public string LegalName { get; set; }
        public string Name { get; set; }
        public string Role { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string PinCode { get; set; }
        public string Plant { get; set; }
        public string GSTNumber { get; set; }
        public string GSTStatus { get; set; }
        public string PANNumber { get; set; }
        public string Phone1 { get; set; }
        public string Phone2 { get; set; }
        public string Email1 { get; set; }
        public string Email2 { get; set; }
        public string TaxID1 { get; set; }
        public string TaxID2 { get; set; }
        [Required]
        public double OutstandingAmount { get; set; }
        [Required]
        public double CreditAmount { get; set; }
        [Required]
        public double LastPayment { get; set; }
        public DateTime? LastPaymentDate { get; set; }
        public string Currency { get; set; }
        public double CreditLimit { get; set; }
        public double CreditBalance { get; set; }
        public DateTime? CreditEvalDate { get; set; }
        [Required]
        public bool MSME { get; set; }
        public string MSME_TYPE { get; set; }
        public string MSME_Att_ID { get; set; }
        [Required]
        public bool Reduced_TDS { get; set; }
        //public string TDS_CAT { get; set; }
        public string TDS_RATE { get; set; }
        public string TDS_Att_ID { get; set; }
        public string TDS_Cert_No { get; set; }
        [Required]
        public bool RP { get; set; }
        public string RP_Name { get; set; }
        public string RP_Type { get; set; }
        public string RP_Att_ID { get; set; }
        [Required]
        public bool IsBlocked { get; set; }

        public string PurchaseOrg { get; set; }
        public string AccountGroup { get; set; }
        public string CompanyCode { get; set; }
        public string TypeofIndustry { get; set; }
        public string VendorCode { get; set; }
      
    }

    public class FTPAttachment
    {
        public int AttachmentID { get; set; }
        public string PatnerID { get; set; }
        public string Type { get; set; }
        public string ContentType { get; set; }
        public string AttachmentName { get; set; }
        public byte[] AttachmentFile { get; set; }
    }
    [Table("BPC_Fact_CP")]
    public class BPCFactContactPerson : CommonClass
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
        public string ContactPersonID { get; set; }
        public string Name { get; set; }
        public string ContactNumber { get; set; }
        public string Email { get; set; }
        public string Item { get; set; }
        public string Department { get; set; }
        public string Title { get; set; }
        public string Mobile { get; set; }
       
    }

    [Table("BPC_Fact_Bank")]
    public class BPCFactBank : CommonClass
    {
        [MaxLength(3)]
        public string Client { get; set; }
        [MaxLength(4)]
        public string Company { get; set; }
        [MaxLength(1)]
        public string Type { get; set; }
        [MaxLength(12)]
        public string PatnerID { get; set; }
        [MaxLength(20)]
        public string AccountNo { get; set; }
        public string Name { get; set; }
        public string BankID { get; set; }
        public string BankName { get; set; }
        public string IFSC { get; set; }
        public string Branch { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
      

    }
    [Table("BPC_Fact_Bank_Support")]
    public class BPCFactBankSupport : CommonClass
    {
        public string ID { get; set; }
        [MaxLength(3)]
        public string Client { get; set; }
        [MaxLength(4)]
        public string Company { get; set; }
        [MaxLength(1)]
        public string Type { get; set; }
        [MaxLength(12)]
        public string PatnerID { get; set; }
        [MaxLength(20)]
        public string AccountNo { get; set; }
        public string Name { get; set; }
        public string BankID { get; set; }
        public string BankName { get; set; }
        public string IFSC { get; set; }
        public string Branch { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
      
    }
    [Table("BPC_KRA")]
    public class BPCKRA : CommonClass
    {
        [MaxLength(3)]
        public string Client { get; set; }
        [MaxLength(4)]
        public string Company { get; set; }
        [MaxLength(1)]
        public string Type { get; set; }
        [MaxLength(12)]
        public string PatnerID { get; set; }
        [MaxLength(2)]
        public string KRA { get; set; }
        public DateTime? EvalDate { get; set; }
        public string KRAText { get; set; }
        public string KRAValue { get; set; }
    }

    [Table("BPC_AI_ACT")]
    public class BPCAIACT : CommonClass
    {
        [MaxLength(3)]
        public string Client { get; set; }
        [MaxLength(4)]
        public string Company { get; set; }
        [MaxLength(1)]
        public string Type { get; set; }
        [MaxLength(12)]
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
        [Required]
        public bool IsSeen { get; set; }
    }

    [Table("BPC_Cert")]
    public class BPCCertificate : CommonClass
    {
        [MaxLength(3)]
        public string Client { get; set; }
        [MaxLength(4)]
        public string Company { get; set; }
        [MaxLength(1)]
        public string Type { get; set; }
        [MaxLength(12)]
        public string PatnerID { get; set; }
        public string CertificateType { get; set; }
        public string CertificateName { get; set; }
        public DateTime? Validity { get; set; }
        public string Mandatory { get; set; }
        public string Attachment { get; set; }
        public byte[] AttachmentFile { get; set; }
        [Required]
        public int AttachmentID { get; set; }

    }
    [Table("BPC_Cert_Support")]
    public class BPCCertificateSupport : CommonClass
    {
        public string ID { get; set; }

        [MaxLength(3)]
        public string Client { get; set; }
        [MaxLength(4)]
        public string Company { get; set; }
        [MaxLength(1)]
        public string Type { get; set; }
        [MaxLength(12)]
        public string PatnerID { get; set; }
        public string CertificateType { get; set; }
        public string CertificateName { get; set; }
        public DateTime? Validity { get; set; }
        public string Mandatory { get; set; }
        public string Attachment { get; set; }
        public byte[] AttachmentFile { get; set; }
        [Required]
        public int AttachmentID { get; set; }
    }
    public class BPCAttachment : CommonClass
    {
        public string Client { get; set; }
        public string Company { get; set; }
        public string Type { get; set; }
        public string PatnerID { get; set; }
        public string CertificateType { get; set; }
        public string CertificateName { get; set; }
        public string AttachmentName { get; set; }
        public byte[] AttachmentFile { get; set; }
        public string ContentType { get; set; }
        public long ContentLength { get; set; }
    }
    [Table("BPC_Dashboard_Card")]
    public class BPCDashboardCard : CommonClass
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AttachmentID { get; set; }
        public string AttachmentName { get; set; }
        public string ContentType { get; set; }
        [Required]
        public long ContentLength { get; set; }
       
        public byte[] AttachmentFile { get; set; }
    }
    [Table("BPC_Attachments")]
    public class BPCAttachments : CommonClass
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AttachmentID { get; set; }
        [MaxLength(3)]
        public string Client { get; set; }
        [MaxLength(4)]
        public string Company { get; set; }
        [MaxLength(1)]
        public string Type { get; set; }
        [MaxLength(12)]
        public string PatnerID { get; set; }
        public string ReferenceNo { get; set; }
        public string AttachmentName { get; set; }
        public string ContentType { get; set; }
        [Required]
        public long ContentLength { get; set; }
        public byte[] AttachmentFile { get; set; }
    }
    public class BPCAttachments_new : CommonClass
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AttachmentID { get; set; }
        [MaxLength(3)]
        public string Client { get; set; }
        [MaxLength(4)]
        public string Company { get; set; }
        [MaxLength(1)]
        public string Type { get; set; }
        [MaxLength(12)]
        public string PatnerID { get; set; }
        public string ReferenceNo { get; set; }
        public string AttachmentName { get; set; }
        public string ContentType { get; set; }
        public long ContentLength { get; set; }
        public string AttachmentFile { get; set; }
    }

    public class BPCAttach
    {

        public int AttachmentID { get; set; }
        public string AttachmentName { get; set; }
        public string Catogery { get; set; }
    }
    public class BPCFactView : CommonClass
    {
        public string Client { get; set; }
        public string Company { get; set; }
        public string Type { get; set; }
        public string PatnerID { get; set; }
        public string LegalName { get; set; }
        public string Name { get; set; }
        public string Role { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string PinCode { get; set; }
        public string Plant { get; set; }
        public string GSTNumber { get; set; }
        public string GSTStatus { get; set; }
        public string PANNumber { get; set; }
        public string Phone1 { get; set; }
        public string Phone2 { get; set; }
        public string Email1 { get; set; }
        public string Email2 { get; set; }
        public string TaxID1 { get; set; }
        public string TaxID2 { get; set; }
        public double OutstandingAmount { get; set; }
        public double CreditAmount { get; set; }
        public double LastPayment { get; set; }
        public DateTime? LastPaymentDate { get; set; }
        public string Currency { get; set; }
        public double CreditLimit { get; set; }
        public double CreditBalance { get; set; }
        public DateTime? CreditEvalDate { get; set; }

        public string PurchaseOrg { get; set; }
        public string AccountGroup { get; set; }
        public string CompanyCode { get; set; }
        public string TypeofIndustry { get; set; }
        public string VendorCode { get; set; }

        public List<BPCFactContactPerson> BPCFactContactPersons { get; set; }
        public List<BPCFactBank> BPCFactBanks { get; set; }

        public List<BPCCertificate> BPCFactCerificate { get; set; }

        public List<BPCKRA> BPCKRAs { get; set; }
        public List<BPCAIACT> BPCAIACTs { get; set; }
    }
    public class User
    {
        [Key]
        public Guid UserID { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string ContactNumber { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public string ModifiedBy { get; set; }
        public string DisplayName { get; set; }
        public bool TourStatus { get; set; }
        public string Pass1 { get; set; }
        public string Pass2 { get; set; }
        public string Pass3 { get; set; }
        public string Pass4 { get; set; }
        public string Pass5 { get; set; }
        public string LastChangedPassword { get; set; }
        public DateTime? ExpiringOn { get; set; }
        public DateTime? IsLockDuration { get; set; }
        public int Attempts { get; set; }
        public bool IsBlocked { get; set; }
    }

    public class UserView
    {
        public Guid UserID { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
    }
    public class BPCFactViewSupport : CommonClass

    {
        public BPCFactSupport BPCFact { get; set; }
        public List<BPCFactBankSupport> BPCFactBanks { get; set; }

        public List<BPCCertificateSupport> BPCFactCerificate { get; set; }
    }
    public class DashboardGraphStatus
    {
        public OTIFStatus oTIFStatus { get; set; }
        public QualityStatus qualityStatus { get; set; }
        public FulfilmentStatus fulfilmentStatus { get; set; }
        public Deliverystatus deliverystatus { get; set; }

    }

    public class OTIFStatus
    {
        public string OTIF { get; set; }
    }

    public class QualityStatus
    {
        public string Quality { get; set; }
    }

    public class FulfilmentDetails
    {
        public string Name { get; set; }
        public string Value { get; set; }
        public string label { get; set; }
    }

    public class FulfilmentStatus
    {
        public FulfilmentDetails OpenDetails { get; set; }
        public FulfilmentDetails ScheduledDetails { get; set; }
        public FulfilmentDetails InProgressDetails { get; set; }
        public FulfilmentDetails PendingDetails { get; set; }
    }

    public class DeliverystatusDetails
    {
        public string Planned { get; set; }
        public string Actual { get; set; }
        public DateTime? Date { get; set; }
    }

    public class Deliverystatus
    {
        public DeliverystatusDetails Planned1 { get; set; }
        public DeliverystatusDetails Planned2 { get; set; }
        public DeliverystatusDetails Planned3 { get; set; }
        public DeliverystatusDetails Planned4 { get; set; }
        public DeliverystatusDetails Planned5 { get; set; }
        //public string Planned5 { get; set; }
        //public string Actual5 { get; set; }
        //public DateTime? Date5 { get; set; }

        //public string Planned4 { get; set; }
        //public string Actual4 { get; set; }
        //public DateTime? Date4 { get; set; }

        //public string Planned3 { get; set; }
        //public string Actual3 { get; set; }
        //public DateTime? Date3 { get; set; }

        //public string Planned2 { get; set; }
        //public string Actual2 { get; set; }
        //public DateTime? Date2 { get; set; }

        //public string Planned1 { get; set; }
        //public string Actual1 { get; set; }
        //public DateTime? Date1 { get; set; }

    }
    //public class VendorDoughnutChartData
    //{
    //    public string Status { get; set; }
    //    public string Count { get; set; }
    //}
    public class CustomerBarChartData
    {
        public List<string> BarChartLabels { get; set; }
        public List<string> PlannedData { get; set; }
        public List<string> ActualData { get; set; }
    }

    public class BPCFactXLSX : CommonClass
    {
        public int ID { get; set; }
        public string Client { get; set; }
        public string Company { get; set; }
        public string Type { get; set; }
        public string Patnerid { get; set; }
        public string Legalname { get; set; }
        public string City { get; set; }
        public string District { get; set; }
        public string Postal { get; set; }
        public string Currency { get; set; }
        public string Phone1 { get; set; }
        public string Phone2 { get; set; }
        public string Tax1 { get; set; }
        public string Tax2 { get; set; }
        public string Accountgroup { get; set; }
        public double Outstandingamount { get; set; }
        public double Lastpayment { get; set; }
        public string LastPaymentdate { get; set; }
    }

    public class BPCFactBankXLSX : CommonClass
    {
        public string Partnerid { get; set; }
        public string Client { get; set; }
        public string Company { get; set; }
        public string Type { get; set; }
        public string Accountnumber { get; set; }
        public string Accountname { get; set; }
        public string Bankid { get; set; }
        public string Bankname { get; set; }
    }

    public class VendorUser
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string DisplayName { get; set; }
        public bool IsBlocked { get; set; }
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
