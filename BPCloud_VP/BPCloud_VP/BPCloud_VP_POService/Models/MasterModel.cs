using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BPCloud_VP_POService.Models
{
    [Table("BPC_Country_Master")]
    public class BPCCountryMaster : CommonClass
    {
        [Key, Column(Order = 1)]
        public int ID { get; set; }
        public string CountryCode { get; set; }
        public string CountryName { get; set; }
    }
    [Table("BPC_Currency_Master")]
    public class BPCCurrencyMaster : CommonClass
    {
        [Key, Column(Order = 1)]
        public int ID { get; set; }
        public string CurrencyCode { get; set; }
        public string CurrencyName { get; set; }
    }
    [Table("BPC_DocumentCenter_Master")]
    public class BPCDocumentCenterMaster : CommonClass
    {
        [Key, Column(Order = 1)]
        public int AppID { get; set; }
        public string DocumentType { get; set; }
        public string Text { get; set; }
        public bool Mandatory { get; set; }
        public string Extension { get; set; }
        public double SizeInKB { get; set; }
        public string ForwardMail { get; set; }
    }
    [Table("BPC_Reason_Master")]
    public class BPCReasonMaster : CommonClass
    {
        [Key, Column(Order = 1)]
        public int ID { get; set; }
        public string ReasonCode { get; set; }
        public string ReasonText { get; set; }
    }
    [Table("BPC_ExpenseType_Master")]
    public class BPCExpenseTypeMaster : CommonClass
    {
        [Key, Column(Order = 1)]
        public int ID { get; set; }
        public string Client { get; set; }
        public string Company { get; set; }
        public string Type { get; set; }
        public string PatnerID { get; set; }
        public string ExpenseType { get; set; }
        public string MaxAmount { get; set; }
    }
    [Table("BPC_TaxType_Master")]
    public class BPCTaxTypeMaster : CommonClass
    {
        [Key, Column(Order = 1)]
        public int ID { get; set; }
        //public string Client { get; set; }
        //public string Company { get; set; }
        //public string Type { get; set; }
        //public string PatnerID { get; set; }
        public string TaxType { get; set; }
        //public string MaxAmount { get; set; }
    }

    [Table("BPC_Plant_Master")]
    public class BPCPlantMaster : CommonClass
    {
        [Key, Column(Order = 1)]
        public string PlantCode { get; set; }
        public string PlantText { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string PinCode { get; set; }
    }

    [Table("BPC_Company_Master")]
    public class BPCCompanyMaster : CommonClass
    {
        [Key, Column(Order = 1)]
        public string Company { get; set; }
        public string CompanyName { get; set; }
    }

    [Table("BPC_ProfitCentre_Master")]
    public class BPCProfitCentreMaster : CommonClass
    {
        [Key, Column(Order = 1)]
        public string ProfitCentre { get; set; }
        public string Description { get; set; }
        public string Language { get; set; }
    }

    [Table("BPC_ASN_Field_Master")]
    public class BPCASNFieldMaster : CommonClass
    {
        [Key, Column(Order = 1)]
        public int ID { get; set; }
        public string DocType { get; set; }
        public string Field { get; set; }
        public string FieldName { get; set; }
        public string Text { get; set; }
        public string DefaultValue { get; set; }
        public bool Mandatory { get; set; }
        public bool Invisible { get; set; }
    }
    [Table("BPC_HSN_Master")]
    public class BPCHSNMaster : CommonClass
    {
        [Key, Column(Order = 1)]
        public int ID { get; set; }
        public string HSNCode { get; set; }
        public string Description { get; set; }
    }

    [Table("BPC_LOG")]
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
