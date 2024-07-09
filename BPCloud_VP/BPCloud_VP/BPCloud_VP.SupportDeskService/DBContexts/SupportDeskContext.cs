using BPCloud_VP.SupportDeskService.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BPCloud_VP.SupportDeskService.DBContexts
{
    public class SupportDeskContext : DbContext
    {
        public SupportDeskContext(DbContextOptions<SupportDeskContext> options) : base(options) { }
        public SupportDeskContext() { }
        public DbSet<SupportMaster> SupportMasters { get; set; }
        public DbSet<SupportHeader> SupportHeaders { get; set; }
        //public DbSet<SupportDeskHeader> SupportDeskHeader { get; set; }
        
        public DbSet<SupportLog> SupportLogs { get; set; }
        public DbSet<SupportAppMaster> SupportAppMasters { get; set; }
        public DbSet<BPCSupportAttachment> BPC_Support_Attachment { get; set; }
        public DbSet<BPCFAQAttachment> BPCFAQAttachments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SupportAppMaster>().HasKey(table => new { table.Client, table.Company, table.Type, table.AppID });
            modelBuilder.Entity<SupportHeader>().HasKey(table => new { table.Client, table.Company, table.Type, table.PatnerID, table.SupportID });
            modelBuilder.Entity<SupportLog>().HasKey(table => new { table.Client, table.Company, table.Type, table.PatnerID, table.SupportID, table.SupportLogID });
            modelBuilder.Entity<SupportMaster>().HasKey(table => new { table.Client, table.Company, table.Type, table.ReasonCode });

            modelBuilder.Entity<SupportHeader>().HasIndex(table => new { table.ReasonCode,table.Plant });
        }
    }
}
