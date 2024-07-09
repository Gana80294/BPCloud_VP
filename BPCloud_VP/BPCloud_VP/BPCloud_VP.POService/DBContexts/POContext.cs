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
        public DbSet<BPCOFGRGI> BPCOFGRGIs { get; set; }
        public DbSet<BPCOFQM> BPCOFQMs { get; set; }
        public DbSet<BPCASNHeader> BPCASNHeaders { get; set; }
        public DbSet<BPCASNItem> BPCASNItems { get; set; }
        public DbSet<BPCASNPack> BPCASNPacks { get; set; }
        public DbSet<BPCFLIPHeader> BPCFLIPHeaders { get; set; }
        public DbSet<BPCFLIPItem> BPCFLIPItems { get; set; }
        public DbSet<BPCFLIPCost> BPCFLIPCosts { get; set; }
        public DbSet<BPCDocumentCenter> BPCDocumentCenters { get; set; }
        public DbSet<BPCFLIPAttachment> BPCFLIPAttachments { get; set; }       
        public DbSet<BPCAttachment> BPCAttachments { get; set; }
        public DbSet<BPCCountryMaster> BPCCountryMasters { get; set; }
        public DbSet<BPCCurrencyMaster> BPCCurrencyMasters { get; set; }
        public DbSet<BPCDocumentCenterMaster> BPCDocumentCenterMasters { get; set; }
        public DbSet<BPCReasonMaster> BPCReasonMasters { get; set; }
        public DbSet<BPCExpenseTypeMaster> BPCExpenseTypeMasters { get; set; }
        public DbSet<BPCPIHeader> BPCPIHeaders { get; set; }
        public DbSet<BPCPIItem> BPCPIItems { get; set; }
        //public DbSet<BPCRetHeader> BPCRetHeaders { get; set; }
        //public DbSet<BPCRetItem> BPCRetItems { get; set; }
        public DbSet<BPCProd> BPCProds { get; set; }
        public DbSet<BPCPODHeader> BPCPODHeaders { get; set; }
        public DbSet<BPCPODItem> BPCPODItems { get; set; }
        public DbSet<BPCOFSubcon> BPCOFSubcons { get; set; }
        public DbSet<BPCPayAccountStatement> BPCPayAccountStatements { get; set; }
        public DbSet<BPCPayTDS> BPCPayTDS { get; set; }
        public DbSet<BPCPayPayable> BPCPayPayables { get; set; }
        public DbSet<BPCPayPayment> BPCPayPayments { get; set; }
        public DbSet<BPCOFAIACT> BPCOFAIACTs { get; set; }
        public DbSet<BPCInvoice> BPCInvoices { get; set; }
        public DbSet<BPCInvoiceItem> BPCInvoiceItems { get; set; }
        public DbSet<BPCPlantMaster> BPCPlantMasters { get; set; }
        public DbSet<BPCASNFieldMaster> BPCASNFieldMasters { get; set; }

        public DbSet<BPCCEOMessage> BPCCEOMessages { get; set; }
        public DbSet<BPCSCOCMessage> BPCSCOCMessages { get; set; }

        public DbSet<BPCOFGRGI> BPCOFGRGI { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BPCOFHeader>().HasKey(table => new { table.Client, table.Company, table.Type, table.PatnerID, table.DocNumber });
            modelBuilder.Entity<BPCOFItem>().HasKey(table => new { table.Client, table.Company, table.Type, table.PatnerID, table.DocNumber, table.Item });
            modelBuilder.Entity<BPCOFScheduleLine>().HasKey(table => new { table.Client, table.Company, table.Type, table.PatnerID, table.DocNumber, table.Item, table.SlLine });
            modelBuilder.Entity<BPCOFGRGI>().HasKey(table => new { table.Client, table.Company, table.Type, table.PatnerID, table.DocNumber, table.GRGIDoc, table.Item });
            modelBuilder.Entity<BPCOFQM>().HasKey(table => new { table.Client, table.Company, table.Type, table.PatnerID, table.DocNumber, table.Item });
            modelBuilder.Entity<BPCASNHeader>().HasKey(table => new { table.Client, table.Company, table.Type, table.PatnerID, table.ASNNumber });
            modelBuilder.Entity<BPCASNItem>().HasKey(table => new { table.Client, table.Company, table.Type, table.PatnerID, table.ASNNumber, table.Item });
            modelBuilder.Entity<BPCASNPack>().HasKey(table => new { table.Client, table.Company, table.Type, table.PatnerID, table.ASNNumber, table.PackageID });
            modelBuilder.Entity<BPCFLIPHeader>().HasKey(table => new { table.Client, table.Company, table.Type, table.PatnerID, table.FLIPID });
            modelBuilder.Entity<BPCFLIPItem>().HasKey(table => new { table.Client, table.Company, table.Type, table.PatnerID, table.FLIPID, table.Item });
            modelBuilder.Entity<BPCFLIPCost>().HasKey(table => new { table.Client, table.Company, table.Type, table.PatnerID, table.FLIPID, table.ExpenceType });
            modelBuilder.Entity<BPCDocumentCenterMaster>().HasKey(table => new { table.DocumentType });
            modelBuilder.Entity<BPCPIHeader>().HasKey(table => new { table.Client, table.Company, table.Type, table.PatnerID, table.PIRNumber});
            modelBuilder.Entity<BPCPIItem>().HasKey(table => new { table.Client, table.Company, table.Type, table.PatnerID, table.PIRNumber, table.Item });
            //modelBuilder.Entity<BPCRetHeader>().HasKey(table => new { table.Client, table.Company, table.Type, table.PatnerID, table.RetReqID });
            //modelBuilder.Entity<BPCRetItem>().HasKey(table => new { table.Client, table.Company, table.Type, table.PatnerID, table.RetReqID, table.Item });
            modelBuilder.Entity<BPCProd>().HasKey(table => new { table.Client, table.Company, table.Type, table.ProductID });
            modelBuilder.Entity<BPCPODHeader>().HasKey(table => new { table.Client, table.Company, table.Type, table.PatnerID, table.InvoiceNumber });
            modelBuilder.Entity<BPCPODItem>().HasKey(table => new { table.Client, table.Company, table.Type, table.PatnerID, table.InvoiceNumber, table.Item });
            modelBuilder.Entity<BPCOFSubcon>().HasKey(table => new { table.Client, table.Company, table.Type, table.PatnerID, table.DocNumber, table.Item, table.SlLine });
            modelBuilder.Entity<BPCPayAccountStatement>().HasKey(table => new { table.Client, table.Company, table.Type, table.PartnerID, table.FiscalYear, table.DocumentID });
            modelBuilder.Entity<BPCPayTDS>().HasKey(table => new { table.Client, table.Company, table.Type, table.PartnerID, table.FiscalYear, table.CompanyCode,table.DocumentID });
            modelBuilder.Entity<BPCPayPayable>().HasKey(table => new { table.Client, table.Company, table.Type, table.PartnerID, table.FiscalYear, table.Invoice });
            modelBuilder.Entity<BPCPayPayment>().HasKey(table => new { table.Client, table.Company, table.Type, table.PartnerID, table.FiscalYear, table.DocumentNumber });
            modelBuilder.Entity<BPCOFAIACT>().HasKey(table => new { table.Client, table.Company, table.Type, table.PatnerID, table.SeqNo });
            modelBuilder.Entity<BPCInvoice>().HasKey(table => new { table.Client, table.Company, table.Type, table.PatnerID, table.FiscalYear, table.InvoiceNo });
            modelBuilder.Entity<BPCInvoiceItem>().HasKey(table => new { table.Client, table.Company, table.Type, table.PatnerID, table.FiscalYear, table.InvoiceNo,table.Item });
            modelBuilder.Entity<BPCPlantMaster>().HasKey(table => new { table.PlantCode });
            modelBuilder.Entity<BPCASNFieldMaster>().HasKey(table => new { table.ID });
            modelBuilder.Entity<BPCCEOMessage>().HasKey(table => new { table.MessageID });
            modelBuilder.Entity<BPCSCOCMessage>().HasKey(table => new { table.MessageID });

            modelBuilder.Entity<BPCASNFieldMaster>().HasIndex(table => new { table.Field });
        }
    }
}
