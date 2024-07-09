using BPCloud_VP.ReportService.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BPCloud_VP.ReportService.DBContexts
{
    public class ReportContext : DbContext
    {
        public ReportContext(DbContextOptions<ReportContext> options) : base(options) { }
        public ReportContext() { }

        public DbSet<BPCInvoice> BPCInvoices { get; set; }
        public DbSet<BPCInvoiceItem> BPCInvoiceItems { get; set; }
        public DbSet<BPCPayment> BPCPayments { get; set; }
        public DbSet<BPCSCSTK> BPCSCSTKs { get; set; }

        public DbSet<BPCReportDOL> BPCReportDOLs { get; set; }
        public DbSet<BPCReportFGCPS> BPCReportFGCPS { get; set; }
        public DbSet<BPCReportGRR> BPCReportGRRs { get; set; }
        public DbSet<BPCReportIP> BPCReportIPs { get; set; }

        public DbSet<BPCReportOV> BPCReportOVs { get; set; }
        public DbSet<BPCReportPPMHeader> BPCReportPPMHeaders { get; set; }
        public DbSet<BPCReportPPMItem> BPCReportPPMItems { get; set; }
        public DbSet<BPCReportVR> BPCReportVRs { get; set; }

        //public DbSet<BPCLog> BPCLogs { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BPCInvoice>().HasKey(table => new { table.Client, table.Company, table.Type, table.PatnerID,table.FiscalYear,table.InvoiceNo });
            modelBuilder.Entity<BPCInvoiceItem>().HasKey(table => new { table.Client, table.Company, table.Type, table.PatnerID,table.FiscalYear, table.InvoiceNo, table.Item });
            modelBuilder.Entity<BPCPayment>().HasKey(table => new { table.Client, table.Company, table.Type, table.PatnerID, table.FiscalYear, table.PaymentDoc });
            modelBuilder.Entity<BPCSCSTK>().HasKey(table => new { table.Client, table.Company, table.Type, table.PatnerID, table.PODocument, table.Item });

            modelBuilder.Entity<BPCReportDOL>().HasKey(table => new { table.Client, table.Company, table.Type, table.PatnerID, table.Material });
            modelBuilder.Entity<BPCReportFGCPS>().HasKey(table => new { table.Client, table.Company, table.Type, table.PatnerID, table.Plant, table.Material });
            modelBuilder.Entity<BPCReportGRR>().HasKey(table => new { table.Client, table.Company, table.Type, table.PatnerID, table.Material });
            modelBuilder.Entity<BPCReportIP>().HasKey(table => new { table.Client, table.Company, table.Type, table.PatnerID, table.Material });

            modelBuilder.Entity<BPCReportOV>().HasKey(table => new { table.Client, table.Company, table.Type, table.PatnerID, table.Material });
            modelBuilder.Entity<BPCReportPPMHeader>().HasKey(table => new { table.Client, table.Company, table.Type, table.PatnerID, table.Period });
            modelBuilder.Entity<BPCReportPPMItem>().HasKey(table => new { table.Client, table.Company, table.Type, table.PatnerID, table.Period, table.Material });
            modelBuilder.Entity<BPCReportVR>().HasKey(table => new { table.Client, table.Company, table.Type, table.PatnerID, table.Material });

            //modelBuilder.Entity<BPCLog>().HasKey(table => new { table.LogID });

        }
    }

}
