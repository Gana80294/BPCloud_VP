using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BPCloud_VP.FactService.Models;
using Microsoft.EntityFrameworkCore;

namespace BPCloud_VP.FactService.DBContexts
{
    public class FactContext : DbContext
    {
        public FactContext(DbContextOptions<FactContext> options) : base(options) { }
        public FactContext() { }

        public DbSet<BPCFact> BPCFacts { get; set; }
        public DbSet<BPCFactSupport> BPCFactsSupport { get; set; }
        public DbSet<BPCFactContactPerson> BPCFactContactPersons { get; set; }
        public DbSet<BPCFactBank> BPCFactBanks { get; set; }
        public DbSet<BPCFactBankSupport> BPCFactBanksSupport { get; set; }
        public DbSet<BPCKRA> BPCKRAs { get; set; }
        public DbSet<BPCAIACT> BPCAIACTs { get; set; }
        public DbSet<BPCCertificate> BPCCertificates { get; set; }
        public DbSet<BPCCertificateSupport> BPCCertificatesSupport { get; set; }
        public DbSet<BPCDashboardCard> BPCDashboardCards { get; set; }
        public DbSet<BPCAttachments> BPCAttachments { get; set; }

        //public DbSet<BPCLog> BPCLogs { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BPCFact>().HasKey(table => new { table.Client, table.Company, table.Type, table.PatnerID });
            modelBuilder.Entity<BPCFactSupport>().HasKey(table => new { table.Client, table.Company, table.Type, table.PatnerID });
            modelBuilder.Entity<BPCFactContactPerson>().HasKey(table => new { table.Client, table.Company, table.Type, table.PatnerID, table.ContactPersonID});
            modelBuilder.Entity<BPCFactBank>().HasKey(table => new { table.Client, table.Company, table.Type, table.PatnerID, table.AccountNo });
            modelBuilder.Entity<BPCFactBankSupport>().HasKey(table => new { table.Client, table.Company, table.Type, table.PatnerID, table.AccountNo });
            modelBuilder.Entity<BPCKRA>().HasKey(table => new { table.Client, table.Company, table.Type, table.PatnerID, table.KRA });
            modelBuilder.Entity<BPCAIACT>().HasKey(table => new { table.Client, table.Company, table.Type, table.PatnerID, table.SeqNo });
            modelBuilder.Entity<BPCCertificate>().HasKey(table => new { table.Client, table.Company, table.Type, table.PatnerID,table.CertificateType, table.CertificateName });
            modelBuilder.Entity<BPCCertificateSupport>().HasKey(table => new { table.Client, table.Company, table.Type, table.PatnerID, table.CertificateType, table.CertificateName });
            //modelBuilder.Entity<BPCLog>().HasKey(table => new { table.LogID });
            modelBuilder.Entity<BPCAttachments>().HasIndex(table => new { table.Client, table.Company, table.Type, table.PatnerID });
        }
    }

}
