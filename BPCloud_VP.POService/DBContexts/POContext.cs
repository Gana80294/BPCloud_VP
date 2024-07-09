using BPCloud_VP_POService.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BPCloud_VP_POService.DBContexts
{
    public class POContext : DbContext
    {
        public POContext(DbContextOptions<POContext> options) : base(options) { }
        public POContext() { }

        public DbSet<BPCOFHeader> BPCOFHeaders { get; set; }
        public DbSet<BPCOFItem> BPCOFItems { get; set; }
        public DbSet<BPCOFScheduleLine> BPCOFScheduleLines { get; set; }
        public DbSet<BPCOFItemSES> BPCOFItemSESes { get; set; }
        public DbSet<BPCOFGRGI> BPCOFGRGIs { get; set; }
        public DbSet<BPCOFQM> BPCOFQMs { get; set; }
        public DbSet<BPCASNHeader> BPCASNHeaders { get; set; }
        public DbSet<BPCASNItem> BPCASNItems { get; set; }
        public DbSet<BPCASNItemBatch> BPCASNItemBatches { get; set; }
        public DbSet<BPCASNItemSES> BPCASNItemSESes { get; set; }
        public DbSet<BPCASNHeader1> BPCASNHeader1 { get; set; }
        public DbSet<BPCASNOFMap1> BPCASNOFMap1 { get; set; }
        public DbSet<BPCASNItem1> BPCASNItem1 { get; set; }
        public DbSet<BPCASNItemBatch1> BPCASNItemBatch1 { get; set; }
        public DbSet<BPCASNItemSES1> BPCASNItemSES1 { get; set; }
        public DbSet<BPCASNPack> BPCASNPacks { get; set; }
        public DbSet<BPCASNFieldMaster> BPCASNFieldMasters { get; set; }
        public DbSet<BPCASNPreShipmentMaster> BPCASNPreShipmentMasters { get; set; }
        public DbSet<BPCFLIPHeader> BPCFLIPHeaders { get; set; }
        public DbSet<BPCFLIPItem> BPCFLIPItems { get; set; }
        public DbSet<BPCFLIPCost> BPCFLIPCosts { get; set; }
        public DbSet<BPTicketStatus> BPTicketStatus {  get; set; }
        public DbSet<BPCDocumentCenter> BPCDocumentCenters { get; set; }
        public DbSet<BPCFLIPAttachment> BPCFLIPAttachments { get; set; }
        public DbSet<BPCAttachment> BPCAttachments { get; set; }
        public DbSet<BPCCountryMaster> BPCCountryMasters { get; set; }
        public DbSet<BPCCurrencyMaster> BPCCurrencyMasters { get; set; }
        public DbSet<BPCDocumentCenterMaster> BPCDocumentCenterMasters { get; set; }
        public DbSet<BPCReasonMaster> BPCReasonMasters { get; set; }
        public DbSet<BPCExpenseTypeMaster> BPCExpenseTypeMasters { get; set; }
        public DbSet<BPCTaxTypeMaster> BPCTaxTypeMasters { get; set; }
        public DbSet<BPCPIHeader> BPCPIHeaders { get; set; }
        public DbSet<BPCPIItem> BPCPIItems { get; set; }
        public DbSet<BPC_GSTIN> BPCGSTIN { get; set; }
      

        //mchng
        public DbSet<BPCRetHeader> BPCRetHeaders { get; set; }
        public DbSet<BPCRetItem> BPCRetItems { get; set; }
        public DbSet<BPCRetItemBatch> BPCRetItemBatch { get; set; }
        public DbSet<BPCRetItemSerial> BPCRetItemSerial { get; set; }
        //mend
        public DbSet<BPCProd> BPCProds { get; set; }
        public DbSet<BPCProdFav> BPCProdFavs { get; set; }
        public DbSet<BPCPODHeader> BPCPODHeaders { get; set; }
        public DbSet<BPCPODItem> BPCPODItems { get; set; }
        public DbSet<BPCOFSubcon> BPCOFSubcons { get; set; }
        public DbSet<BPCPayAccountStatement> BPCPayAccountStatements { get; set; }
        public DbSet<BPCPayTDS> BPCPayTDS { get; set; }
        public DbSet<BPCPayPayable> BPCPayPayables { get; set; }
        public DbSet<BPCPayPayment> BPCPayPayments { get; set; }
        public DbSet<BPCPayDiscount> BPCPayDiscounts { get; set; }
        public DbSet<BPCPayDiscountMaster> BPCPayDiscountMasters { get; set; }
        public DbSet<BPCPayRecord> BPCPayRecords { get; set; }
        public DbSet<BPCOFAIACT> BPCOFAIACTs { get; set; }
        public DbSet<BPCInvoice> BPCInvoices { get; set; }
        public DbSet<BPCInvoiceItem> BPCInvoiceItems { get; set; }
        public DbSet<BPCPlantMaster> BPCPlantMasters { get; set; }
        public DbSet<BPCProfitCentreMaster> BPCProfitCentreMasters { get; set; }
        public DbSet<BPCCompanyMaster> BPCCompanyMasters { get; set; }
       
        public DbSet<BPCHSNMaster> BPCHSNMasters { get; set; }

        public DbSet<BPCCEOMessage> BPCCEOMessages { get; set; }
        public DbSet<BPCSCOCMessage> BPCSCOCMessages { get; set; }
        public DbSet<BPCWelcomeMessage> BPCWelcomeMessages { get; set; }

        //public DbSet<BPCOFGRGI> BPCOFGRGI { get; set; }

        public DbSet<BPCGateHoveringVechicles> GateHV { get; set; }

        public DbSet<BPCGateVechicleTurnAroundTime> GateVTA { get; set; }
        public DbSet<BPCGateEntry> BPCGateEntries { get; set; }
        public DbSet<BalanceConfirmationHeader> BalanceConfirmationHeaders { get; set; }
        public DbSet<BalanceConfirmationItem> BalanceConfirmationItems { get; set; }
        public DbSet<BPCLog> BPCLogs { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BPCOFHeader>().HasKey(table => new { table.Client, table.Company, table.Type, table.PatnerID, table.DocNumber });
            modelBuilder.Entity<BPCOFItem>().HasKey(table => new { table.Client, table.Company, table.Type, table.PatnerID, table.DocNumber, table.Item });
            modelBuilder.Entity<BPCOFItemSES>().HasKey(table => new { table.Client, table.Company, table.Type, table.PatnerID, table.DocNumber, table.Item, table.ServiceNo });
            modelBuilder.Entity<BPCOFScheduleLine>().HasKey(table => new { table.Client, table.Company, table.Type, table.PatnerID, table.DocNumber, table.Item, table.SlLine });
            modelBuilder.Entity<BPCOFGRGI>().HasKey(table => new { table.Client, table.Company, table.Type, table.PatnerID, table.DocNumber, table.GRGIDoc, table.Item });
            modelBuilder.Entity<BPCOFQM>().HasKey(table => new { table.Client, table.Company, table.Type, table.PatnerID, table.DocNumber, table.Item, table.SerialNumber });
            modelBuilder.Entity<BPCASNHeader>().HasKey(table => new { table.Client, table.Company, table.Type, table.PatnerID, table.ASNNumber });
            modelBuilder.Entity<BPCASNItem>().HasKey(table => new { table.Client, table.Company, table.Type, table.PatnerID, table.ASNNumber, table.Item });
            modelBuilder.Entity<BPCASNItemBatch>().HasKey(table => new { table.Client, table.Company, table.Type, table.PatnerID, table.ASNNumber, table.Item, table.Batch });
            modelBuilder.Entity<BPCASNItemSES>().HasKey(table => new { table.Client, table.Company, table.Type, table.PatnerID, table.ASNNumber, table.Item, table.ServiceNo });

            modelBuilder.Entity<BPCASNHeader1>().HasKey(table => new { table.Client, table.Company, table.Type, table.PatnerID, table.ASNNumber });
            modelBuilder.Entity<BPCASNOFMap1>().HasKey(table => new { table.Client, table.Company, table.Type, table.PatnerID, table.ASNNumber, table.DocNumber });
            modelBuilder.Entity<BPCASNItem1>().HasKey(table => new { table.Client, table.Company, table.Type, table.PatnerID, table.ASNNumber, table.DocNumber, table.Item });
            modelBuilder.Entity<BPCASNItemBatch1>().HasKey(table => new { table.Client, table.Company, table.Type, table.PatnerID, table.ASNNumber, table.DocNumber, table.Item, table.Batch });
            modelBuilder.Entity<BPCASNItemSES1>().HasKey(table => new { table.Client, table.Company, table.Type, table.PatnerID, table.ASNNumber, table.DocNumber, table.Item, table.ServiceNo });

            modelBuilder.Entity<BPCASNPack>().HasKey(table => new { table.Client, table.Company, table.Type, table.PatnerID, table.ASNNumber, table.PackageID });
            modelBuilder.Entity<BPCASNFieldMaster>().HasKey(table => new { table.ID });
            modelBuilder.Entity<BPCASNPreShipmentMaster>().HasKey(table => new { table.ID });

            modelBuilder.Entity<BPCFLIPHeader>().HasKey(table => new { table.Client, table.Company, table.Type, table.PatnerID, table.FLIPID });
            modelBuilder.Entity<BPCFLIPItem>().HasKey(table => new { table.Client, table.Company, table.Type, table.PatnerID, table.FLIPID, table.Item });
            modelBuilder.Entity<BPCFLIPCost>().HasKey(table => new { table.Client, table.Company, table.Type, table.PatnerID, table.FLIPID, table.ExpenceType });
            modelBuilder.Entity<BPCDocumentCenterMaster>().HasKey(table => new { table.DocumentType });
            modelBuilder.Entity<BPCPIHeader>().HasKey(table => new { table.Client, table.Company, table.Type, table.PatnerID, table.PIRNumber });
            modelBuilder.Entity<BPCPIItem>().HasKey(table => new { table.Client, table.Company, table.Type, table.PatnerID, table.PIRNumber, table.Item });
            modelBuilder.Entity<BPCRetHeader>().HasKey(table => new { table.Client, table.Company, table.Type, table.PatnerID, table.RetReqID });
            modelBuilder.Entity<BPCRetItem>().HasKey(table => new { table.Client, table.Company, table.Type, table.PatnerID, table.RetReqID, table.Item });
            modelBuilder.Entity<BPCRetItemBatch>().HasKey(table => new { table.Client, table.Company, table.Type, table.PatnerID, table.RetReqID, table.Item, table.Batch });
            modelBuilder.Entity<BPCRetItemSerial>().HasKey(table => new { table.Client, table.Company, table.Type, table.PatnerID, table.RetReqID, table.Item, table.Serial });
            modelBuilder.Entity<BPCProd>().HasKey(table => new { table.Client, table.Company, table.Type, table.ProductID });
            modelBuilder.Entity<BPCProdFav>().HasKey(table => new { table.Client, table.Company, table.Type, table.PatnerID, table.ProductID });
            modelBuilder.Entity<BPCPODHeader>().HasKey(table => new { table.Client, table.Company, table.Type, table.PatnerID, table.InvoiceNumber });
            modelBuilder.Entity<BPCPODItem>().HasKey(table => new { table.Client, table.Company, table.Type, table.PatnerID, table.InvoiceNumber, table.Item });
            modelBuilder.Entity<BPCOFSubcon>().HasKey(table => new { table.Client, table.Company, table.Type, table.PatnerID, table.DocNumber, table.Item, table.SlLine });
            modelBuilder.Entity<BPCPayAccountStatement>().HasKey(table => new { table.Client, table.Company, table.Type, table.PartnerID, table.FiscalYear, table.DocumentNumber });
            modelBuilder.Entity<BPCPayTDS>().HasKey(table => new { table.Client, table.Company, table.Type, table.PartnerID, table.FiscalYear, table.CompanyCode, table.DocumentID });
            modelBuilder.Entity<BPCPayPayable>().HasKey(table => new { table.Client, table.Company, table.Type, table.PartnerID, table.FiscalYear, table.Invoice });
            modelBuilder.Entity<BPCPayPayment>().HasKey(table => new { table.Client, table.Company, table.Type, table.PartnerID, table.FiscalYear, table.DocumentNumber });
            modelBuilder.Entity<BPCPayDiscount>().HasKey(table => new { table.Client, table.Company, table.Type, table.PartnerID, table.FiscalYear, table.DocumentNumber });
            modelBuilder.Entity<BPCPayRecord>().HasKey(table => new { table.Client, table.Company, table.Type, table.PartnerID, table.DocumentNumber, table.InvoiceNumber, table.PayRecordNo });
            modelBuilder.Entity<BPCOFAIACT>().HasKey(table => new { table.Client, table.Company, table.Type, table.PatnerID, table.SeqNo });
            modelBuilder.Entity<BPCInvoice>().HasKey(table => new { table.Client, table.Company, table.Type, table.PatnerID, table.FiscalYear, table.InvoiceNo });
            modelBuilder.Entity<BPCInvoiceItem>().HasKey(table => new { table.Client, table.Company, table.Type, table.PatnerID, table.FiscalYear, table.InvoiceNo, table.Item });
            modelBuilder.Entity<BalanceConfirmationHeader>().HasKey(table => new { table.Client, table.Company, table.Type, table.PatnerID, table.FiscalYear });
            modelBuilder.Entity<BalanceConfirmationItem>().HasKey(table => new { table.Client, table.Company, table.Type, table.PatnerID, table.FiscalYear, table.DocNumber });
            modelBuilder.Entity<BPCPlantMaster>().HasKey(table => new { table.PlantCode });
            modelBuilder.Entity<BPCProfitCentreMaster>().HasKey(table => new { table.ProfitCentre });
            modelBuilder.Entity<BPCCEOMessage>().HasKey(table => new { table.MessageID });
            modelBuilder.Entity<BPCSCOCMessage>().HasKey(table => new { table.MessageID });
            modelBuilder.Entity<BPCWelcomeMessage>().HasKey(table => new { table.MessageID });
            modelBuilder.Entity<BPCASNFieldMaster>().HasIndex(table => new { table.Field });
            modelBuilder.Entity<BPCGateEntry>().HasKey(table => new { table.Client, table.Company, table.Type, table.PatnerID, table.ASNNumber, table.DocNumber, table.GateEntryNo });
            modelBuilder.Entity<BPCGateVechicleTurnAroundTime>().HasKey(table => new { table.Client, table.Company, table.Type, table.PatnerID, table.DocNo, table.Partner });
            modelBuilder.Entity<BPCGateHoveringVechicles>().HasKey(table => new { table.Client, table.Company, table.Type, table.PatnerID, table.DocNo, table.Partner });
            modelBuilder.Entity<BPCLog>().HasKey(table => new { table.LogID });

            modelBuilder.Entity<BPCFLIPAttachment>().HasIndex(table => new { table.FLIPID });
            modelBuilder.Entity<BPCPayDiscount>().HasIndex(table => new { table.ProfitCenter });
            modelBuilder.Entity<BPCPayDiscountMaster>().HasIndex(table => new { table.FiscalYear, table.ProfitCenter });

        }
    }
}
