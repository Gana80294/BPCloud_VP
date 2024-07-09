﻿// <auto-generated />
using System;
using BPCloud_VP.ReportService.DBContexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace BPCloud_VP.ReportService.Migrations
{
    [DbContext(typeof(ReportContext))]
    [Migration("20210329103819_PlantAddedInTables")]
    partial class PlantAddedInTables
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.14-servicing-32113")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("BPCloud_VP.ReportService.Models.BPCInvoice", b =>
                {
                    b.Property<string>("Client")
                        .HasMaxLength(3);

                    b.Property<string>("Company")
                        .HasMaxLength(4);

                    b.Property<string>("Type")
                        .HasMaxLength(1);

                    b.Property<string>("PatnerID")
                        .HasMaxLength(12);

                    b.Property<string>("FiscalYear")
                        .HasMaxLength(4);

                    b.Property<string>("InvoiceNo")
                        .HasMaxLength(16);

                    b.Property<string>("ASN");

                    b.Property<DateTime?>("ASNDate");

                    b.Property<string>("AttID");

                    b.Property<string>("CreatedBy");

                    b.Property<DateTime>("CreatedOn");

                    b.Property<string>("Currency");

                    b.Property<DateTime?>("DateofPayment");

                    b.Property<double>("InvoiceAmount");

                    b.Property<DateTime?>("InvoiceDate");

                    b.Property<bool>("IsActive");

                    b.Property<string>("ModifiedBy");

                    b.Property<DateTime?>("ModifiedOn");

                    b.Property<string>("PODConfirmedBy");

                    b.Property<DateTime?>("PODDate");

                    b.Property<double>("PaidAmount");

                    b.Property<string>("Plant");

                    b.Property<string>("PoReference");

                    b.Property<DateTime?>("PostingDate");

                    b.Property<string>("Status");

                    b.HasKey("Client", "Company", "Type", "PatnerID", "FiscalYear", "InvoiceNo");

                    b.ToTable("BPC_INV");
                });

            modelBuilder.Entity("BPCloud_VP.ReportService.Models.BPCInvoiceItem", b =>
                {
                    b.Property<string>("Client")
                        .HasMaxLength(3);

                    b.Property<string>("Company")
                        .HasMaxLength(4);

                    b.Property<string>("Type")
                        .HasMaxLength(1);

                    b.Property<string>("PatnerID")
                        .HasMaxLength(12);

                    b.Property<string>("FiscalYear")
                        .HasMaxLength(4);

                    b.Property<string>("InvoiceNo")
                        .HasMaxLength(16);

                    b.Property<string>("Item")
                        .HasMaxLength(4);

                    b.Property<string>("CreatedBy");

                    b.Property<DateTime>("CreatedOn");

                    b.Property<double>("InvoiceQty");

                    b.Property<bool>("IsActive");

                    b.Property<string>("Material");

                    b.Property<string>("MaterialText");

                    b.Property<string>("ModifiedBy");

                    b.Property<DateTime?>("ModifiedOn");

                    b.Property<double>("PODQty");

                    b.Property<string>("ReasonCode");

                    b.Property<string>("Status");

                    b.HasKey("Client", "Company", "Type", "PatnerID", "FiscalYear", "InvoiceNo", "Item");

                    b.ToTable("BPC_INV_I");
                });

            modelBuilder.Entity("BPCloud_VP.ReportService.Models.BPCPayment", b =>
                {
                    b.Property<string>("Client")
                        .HasMaxLength(3);

                    b.Property<string>("Company")
                        .HasMaxLength(4);

                    b.Property<string>("Type")
                        .HasMaxLength(1);

                    b.Property<string>("PatnerID")
                        .HasMaxLength(12);

                    b.Property<string>("FiscalYear")
                        .HasMaxLength(4);

                    b.Property<string>("PaymentDoc")
                        .HasMaxLength(10);

                    b.Property<double?>("Amount");

                    b.Property<string>("Attachment");

                    b.Property<string>("CreatedBy");

                    b.Property<DateTime>("CreatedOn");

                    b.Property<string>("Currency");

                    b.Property<DateTime?>("Date");

                    b.Property<bool>("IsActive");

                    b.Property<string>("ModifiedBy");

                    b.Property<DateTime?>("ModifiedOn");

                    b.Property<string>("Plant");

                    b.Property<string>("Remark");

                    b.HasKey("Client", "Company", "Type", "PatnerID", "FiscalYear", "PaymentDoc");

                    b.ToTable("BPC_PAY");
                });

            modelBuilder.Entity("BPCloud_VP.ReportService.Models.BPCReportDOL", b =>
                {
                    b.Property<string>("Client")
                        .HasMaxLength(3);

                    b.Property<string>("Company")
                        .HasMaxLength(4);

                    b.Property<string>("Type")
                        .HasMaxLength(1);

                    b.Property<string>("PatnerID")
                        .HasMaxLength(12);

                    b.Property<string>("Material");

                    b.Property<string>("CreatedBy");

                    b.Property<DateTime>("CreatedOn");

                    b.Property<bool>("IsActive");

                    b.Property<string>("MaterialText");

                    b.Property<string>("ModifiedBy");

                    b.Property<DateTime?>("ModifiedOn");

                    b.Property<string>("Status");

                    b.HasKey("Client", "Company", "Type", "PatnerID", "Material");

                    b.ToTable("BPC_REP_DOL");
                });

            modelBuilder.Entity("BPCloud_VP.ReportService.Models.BPCReportFGCPS", b =>
                {
                    b.Property<string>("Client")
                        .HasMaxLength(3);

                    b.Property<string>("Company")
                        .HasMaxLength(4);

                    b.Property<string>("Type")
                        .HasMaxLength(1);

                    b.Property<string>("PatnerID")
                        .HasMaxLength(12);

                    b.Property<string>("Plant");

                    b.Property<string>("Material");

                    b.Property<string>("Batch");

                    b.Property<string>("CreatedBy");

                    b.Property<DateTime>("CreatedOn");

                    b.Property<bool>("IsActive");

                    b.Property<string>("MaterialText");

                    b.Property<string>("ModifiedBy");

                    b.Property<DateTime?>("ModifiedOn");

                    b.Property<double?>("Price");

                    b.Property<string>("Status");

                    b.Property<double?>("StickQty");

                    b.Property<string>("UOM");

                    b.HasKey("Client", "Company", "Type", "PatnerID", "Plant", "Material");

                    b.ToTable("BPC_REP_FGCPS");
                });

            modelBuilder.Entity("BPCloud_VP.ReportService.Models.BPCReportGRR", b =>
                {
                    b.Property<string>("Client")
                        .HasMaxLength(3);

                    b.Property<string>("Company")
                        .HasMaxLength(4);

                    b.Property<string>("Type")
                        .HasMaxLength(1);

                    b.Property<string>("PatnerID")
                        .HasMaxLength(12);

                    b.Property<string>("Material");

                    b.Property<string>("CreatedBy");

                    b.Property<DateTime>("CreatedOn");

                    b.Property<bool>("IsActive");

                    b.Property<string>("MaterialText");

                    b.Property<string>("ModifiedBy");

                    b.Property<DateTime?>("ModifiedOn");

                    b.Property<double?>("OrderQty");

                    b.Property<double?>("ReceivedQty");

                    b.Property<double?>("RejectedPPM");

                    b.Property<double?>("ReworkQty");

                    b.Property<string>("Status");

                    b.HasKey("Client", "Company", "Type", "PatnerID", "Material");

                    b.ToTable("BPC_REP_GRR");
                });

            modelBuilder.Entity("BPCloud_VP.ReportService.Models.BPCReportIP", b =>
                {
                    b.Property<string>("Client")
                        .HasMaxLength(3);

                    b.Property<string>("Company")
                        .HasMaxLength(4);

                    b.Property<string>("Type")
                        .HasMaxLength(1);

                    b.Property<string>("PatnerID")
                        .HasMaxLength(12);

                    b.Property<string>("Material");

                    b.Property<string>("CreatedBy");

                    b.Property<DateTime>("CreatedOn");

                    b.Property<string>("Desc");

                    b.Property<bool>("IsActive");

                    b.Property<double?>("LowerLimit");

                    b.Property<string>("MaterialChar");

                    b.Property<string>("MaterialText");

                    b.Property<string>("Method");

                    b.Property<string>("ModRule");

                    b.Property<string>("ModifiedBy");

                    b.Property<DateTime?>("ModifiedOn");

                    b.Property<string>("Status");

                    b.Property<string>("UOM");

                    b.Property<double?>("UpperLimit");

                    b.HasKey("Client", "Company", "Type", "PatnerID", "Material");

                    b.ToTable("BPC_REP_IP");
                });

            modelBuilder.Entity("BPCloud_VP.ReportService.Models.BPCReportOV", b =>
                {
                    b.Property<string>("Client")
                        .HasMaxLength(3);

                    b.Property<string>("Company")
                        .HasMaxLength(4);

                    b.Property<string>("Type")
                        .HasMaxLength(1);

                    b.Property<string>("PatnerID")
                        .HasMaxLength(12);

                    b.Property<string>("Material");

                    b.Property<double?>("AccQty");

                    b.Property<string>("CreatedBy");

                    b.Property<DateTime>("CreatedOn");

                    b.Property<double?>("InputQty");

                    b.Property<bool>("IsActive");

                    b.Property<string>("MaterialText");

                    b.Property<string>("ModifiedBy");

                    b.Property<DateTime?>("ModifiedOn");

                    b.Property<double?>("RejPercentage");

                    b.Property<double?>("RejQty");

                    b.Property<string>("Status");

                    b.HasKey("Client", "Company", "Type", "PatnerID", "Material");

                    b.ToTable("BPC_REP_OV");
                });

            modelBuilder.Entity("BPCloud_VP.ReportService.Models.BPCReportPPMHeader", b =>
                {
                    b.Property<string>("Client")
                        .HasMaxLength(3);

                    b.Property<string>("Company")
                        .HasMaxLength(4);

                    b.Property<string>("Type")
                        .HasMaxLength(1);

                    b.Property<string>("PatnerID")
                        .HasMaxLength(12);

                    b.Property<DateTime>("Period");

                    b.Property<string>("CreatedBy");

                    b.Property<DateTime>("CreatedOn");

                    b.Property<bool>("IsActive");

                    b.Property<string>("ModifiedBy");

                    b.Property<DateTime?>("ModifiedOn");

                    b.Property<double?>("PPM");

                    b.Property<double?>("ReceiptQty");

                    b.Property<double?>("RejectedQty");

                    b.Property<double?>("TotalPPM");

                    b.HasKey("Client", "Company", "Type", "PatnerID", "Period");

                    b.ToTable("BPC_REP_PPM_H");
                });

            modelBuilder.Entity("BPCloud_VP.ReportService.Models.BPCReportPPMItem", b =>
                {
                    b.Property<string>("Client")
                        .HasMaxLength(3);

                    b.Property<string>("Company")
                        .HasMaxLength(4);

                    b.Property<string>("Type")
                        .HasMaxLength(1);

                    b.Property<string>("PatnerID")
                        .HasMaxLength(12);

                    b.Property<DateTime>("Period");

                    b.Property<string>("Material");

                    b.Property<string>("CreatedBy");

                    b.Property<DateTime>("CreatedOn");

                    b.Property<bool>("IsActive");

                    b.Property<string>("MaterialText");

                    b.Property<string>("ModifiedBy");

                    b.Property<DateTime?>("ModifiedOn");

                    b.Property<double?>("PPM");

                    b.Property<double?>("ReceiptQty");

                    b.Property<double?>("RejectedQty");

                    b.HasKey("Client", "Company", "Type", "PatnerID", "Period", "Material");

                    b.ToTable("BPC_REP_PPM_I");
                });

            modelBuilder.Entity("BPCloud_VP.ReportService.Models.BPCReportVR", b =>
                {
                    b.Property<string>("Client")
                        .HasMaxLength(3);

                    b.Property<string>("Company")
                        .HasMaxLength(4);

                    b.Property<string>("Type")
                        .HasMaxLength(1);

                    b.Property<string>("PatnerID")
                        .HasMaxLength(12);

                    b.Property<string>("Material");

                    b.Property<string>("CreatedBy");

                    b.Property<DateTime>("CreatedOn");

                    b.Property<bool>("IsActive");

                    b.Property<string>("MaterialText");

                    b.Property<string>("ModifiedBy");

                    b.Property<DateTime?>("ModifiedOn");

                    b.Property<double?>("OrderQty");

                    b.Property<double?>("ReceivedQty");

                    b.Property<double?>("RejectedPPM");

                    b.Property<double?>("ReworkQty");

                    b.Property<string>("Status");

                    b.HasKey("Client", "Company", "Type", "PatnerID", "Material");

                    b.ToTable("BPC_REP_VR");
                });

            modelBuilder.Entity("BPCloud_VP.ReportService.Models.BPCSCSTK", b =>
                {
                    b.Property<string>("Client")
                        .HasMaxLength(3);

                    b.Property<string>("Company")
                        .HasMaxLength(4);

                    b.Property<string>("Type")
                        .HasMaxLength(1);

                    b.Property<string>("PatnerID")
                        .HasMaxLength(12);

                    b.Property<string>("PODocument")
                        .HasMaxLength(10);

                    b.Property<string>("Item")
                        .HasMaxLength(4);

                    b.Property<string>("CreatedBy");

                    b.Property<DateTime>("CreatedOn");

                    b.Property<DateTime?>("Date");

                    b.Property<bool>("IsActive");

                    b.Property<double?>("IssuedQty");

                    b.Property<string>("Material");

                    b.Property<string>("ModifiedBy");

                    b.Property<DateTime?>("ModifiedOn");

                    b.Property<string>("Plant");

                    b.Property<double?>("StockQty");

                    b.HasKey("Client", "Company", "Type", "PatnerID", "PODocument", "Item");

                    b.ToTable("BPC_SC_STK");
                });
#pragma warning restore 612, 618
        }
    }
}
