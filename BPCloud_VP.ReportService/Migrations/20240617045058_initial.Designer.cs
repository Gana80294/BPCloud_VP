﻿// <auto-generated />
using System;
using BPCloud_VP.ReportService.DBContexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace BPCloud_VP.ReportService.Migrations
{
    [DbContext(typeof(ReportContext))]
    [Migration("20240617045058_initial")]
    partial class initial
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.6")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("BPCloud_VP.ReportService.Models.BPCInvoice", b =>
                {
                    b.Property<string>("Client")
                        .HasMaxLength(3)
                        .HasColumnType("character varying(3)");

                    b.Property<string>("Company")
                        .HasMaxLength(4)
                        .HasColumnType("character varying(4)");

                    b.Property<string>("Type")
                        .HasMaxLength(1)
                        .HasColumnType("character varying(1)");

                    b.Property<string>("PatnerID")
                        .HasMaxLength(12)
                        .HasColumnType("character varying(12)");

                    b.Property<string>("FiscalYear")
                        .HasMaxLength(4)
                        .HasColumnType("character varying(4)");

                    b.Property<string>("InvoiceNo")
                        .HasMaxLength(16)
                        .HasColumnType("character varying(16)");

                    b.Property<string>("ASN")
                        .HasColumnType("text");

                    b.Property<DateTime?>("ASNDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("AttID")
                        .HasColumnType("text");

                    b.Property<string>("CreatedBy")
                        .HasColumnType("text");

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Currency")
                        .HasColumnType("text");

                    b.Property<DateTime?>("DateofPayment")
                        .HasColumnType("timestamp with time zone");

                    b.Property<double>("InvoiceAmount")
                        .HasColumnType("double precision");

                    b.Property<DateTime?>("InvoiceDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<bool>("IsActive")
                        .HasColumnType("boolean");

                    b.Property<string>("ModifiedBy")
                        .HasColumnType("text");

                    b.Property<DateTime?>("ModifiedOn")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("PODConfirmedBy")
                        .HasColumnType("text");

                    b.Property<DateTime?>("PODDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<double>("PaidAmount")
                        .HasColumnType("double precision");

                    b.Property<string>("Plant")
                        .HasColumnType("text");

                    b.Property<string>("PoReference")
                        .HasColumnType("text");

                    b.Property<DateTime?>("PostingDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Status")
                        .HasColumnType("text");

                    b.HasKey("Client", "Company", "Type", "PatnerID", "FiscalYear", "InvoiceNo");

                    b.ToTable("BPC_INV");
                });

            modelBuilder.Entity("BPCloud_VP.ReportService.Models.BPCInvoiceItem", b =>
                {
                    b.Property<string>("Client")
                        .HasMaxLength(3)
                        .HasColumnType("character varying(3)");

                    b.Property<string>("Company")
                        .HasMaxLength(4)
                        .HasColumnType("character varying(4)");

                    b.Property<string>("Type")
                        .HasMaxLength(1)
                        .HasColumnType("character varying(1)");

                    b.Property<string>("PatnerID")
                        .HasMaxLength(12)
                        .HasColumnType("character varying(12)");

                    b.Property<string>("FiscalYear")
                        .HasMaxLength(4)
                        .HasColumnType("character varying(4)");

                    b.Property<string>("InvoiceNo")
                        .HasMaxLength(16)
                        .HasColumnType("character varying(16)");

                    b.Property<string>("Item")
                        .HasMaxLength(4)
                        .HasColumnType("character varying(4)");

                    b.Property<string>("CreatedBy")
                        .HasColumnType("text");

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("timestamp with time zone");

                    b.Property<double>("InvoiceQty")
                        .HasColumnType("double precision");

                    b.Property<bool>("IsActive")
                        .HasColumnType("boolean");

                    b.Property<string>("Material")
                        .HasColumnType("text");

                    b.Property<string>("MaterialText")
                        .HasColumnType("text");

                    b.Property<string>("ModifiedBy")
                        .HasColumnType("text");

                    b.Property<DateTime?>("ModifiedOn")
                        .HasColumnType("timestamp with time zone");

                    b.Property<double>("PODQty")
                        .HasColumnType("double precision");

                    b.Property<string>("ReasonCode")
                        .HasColumnType("text");

                    b.Property<string>("Status")
                        .HasColumnType("text");

                    b.HasKey("Client", "Company", "Type", "PatnerID", "FiscalYear", "InvoiceNo", "Item");

                    b.ToTable("BPC_INV_I");
                });

            modelBuilder.Entity("BPCloud_VP.ReportService.Models.BPCPayment", b =>
                {
                    b.Property<string>("Client")
                        .HasMaxLength(3)
                        .HasColumnType("character varying(3)");

                    b.Property<string>("Company")
                        .HasMaxLength(4)
                        .HasColumnType("character varying(4)");

                    b.Property<string>("Type")
                        .HasMaxLength(1)
                        .HasColumnType("character varying(1)");

                    b.Property<string>("PatnerID")
                        .HasMaxLength(12)
                        .HasColumnType("character varying(12)");

                    b.Property<string>("FiscalYear")
                        .HasMaxLength(4)
                        .HasColumnType("character varying(4)");

                    b.Property<string>("PaymentDoc")
                        .HasMaxLength(10)
                        .HasColumnType("character varying(10)");

                    b.Property<double?>("Amount")
                        .HasColumnType("double precision");

                    b.Property<string>("Attachment")
                        .HasColumnType("text");

                    b.Property<string>("CreatedBy")
                        .HasColumnType("text");

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Currency")
                        .HasColumnType("text");

                    b.Property<DateTime?>("Date")
                        .HasColumnType("timestamp with time zone");

                    b.Property<bool>("IsActive")
                        .HasColumnType("boolean");

                    b.Property<string>("ModifiedBy")
                        .HasColumnType("text");

                    b.Property<DateTime?>("ModifiedOn")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Plant")
                        .HasColumnType("text");

                    b.Property<string>("Remark")
                        .HasColumnType("text");

                    b.HasKey("Client", "Company", "Type", "PatnerID", "FiscalYear", "PaymentDoc");

                    b.ToTable("BPC_PAY");
                });

            modelBuilder.Entity("BPCloud_VP.ReportService.Models.BPCReportDOL", b =>
                {
                    b.Property<string>("Client")
                        .HasMaxLength(3)
                        .HasColumnType("character varying(3)");

                    b.Property<string>("Company")
                        .HasMaxLength(4)
                        .HasColumnType("character varying(4)");

                    b.Property<string>("Type")
                        .HasMaxLength(1)
                        .HasColumnType("character varying(1)");

                    b.Property<string>("PatnerID")
                        .HasMaxLength(12)
                        .HasColumnType("character varying(12)");

                    b.Property<string>("Material")
                        .HasColumnType("text");

                    b.Property<string>("CreatedBy")
                        .HasColumnType("text");

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("timestamp with time zone");

                    b.Property<bool>("IsActive")
                        .HasColumnType("boolean");

                    b.Property<string>("MaterialText")
                        .HasColumnType("text");

                    b.Property<string>("ModifiedBy")
                        .HasColumnType("text");

                    b.Property<DateTime?>("ModifiedOn")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Status")
                        .HasColumnType("text");

                    b.HasKey("Client", "Company", "Type", "PatnerID", "Material");

                    b.ToTable("BPC_REP_DOL");
                });

            modelBuilder.Entity("BPCloud_VP.ReportService.Models.BPCReportFGCPS", b =>
                {
                    b.Property<string>("Client")
                        .HasMaxLength(3)
                        .HasColumnType("character varying(3)");

                    b.Property<string>("Company")
                        .HasMaxLength(4)
                        .HasColumnType("character varying(4)");

                    b.Property<string>("Type")
                        .HasMaxLength(1)
                        .HasColumnType("character varying(1)");

                    b.Property<string>("PatnerID")
                        .HasMaxLength(12)
                        .HasColumnType("character varying(12)");

                    b.Property<string>("Plant")
                        .HasMaxLength(450)
                        .HasColumnType("character varying(450)");

                    b.Property<string>("Material")
                        .HasMaxLength(450)
                        .HasColumnType("character varying(450)");

                    b.Property<string>("Batch")
                        .HasColumnType("text");

                    b.Property<string>("CreatedBy")
                        .HasColumnType("text");

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("timestamp with time zone");

                    b.Property<bool>("IsActive")
                        .HasColumnType("boolean");

                    b.Property<string>("MaterialText")
                        .HasColumnType("text");

                    b.Property<string>("ModifiedBy")
                        .HasColumnType("text");

                    b.Property<DateTime?>("ModifiedOn")
                        .HasColumnType("timestamp with time zone");

                    b.Property<double?>("Price")
                        .HasColumnType("double precision");

                    b.Property<string>("Status")
                        .HasColumnType("text");

                    b.Property<double?>("StickQty")
                        .HasColumnType("double precision");

                    b.Property<string>("UOM")
                        .HasColumnType("text");

                    b.HasKey("Client", "Company", "Type", "PatnerID", "Plant", "Material");

                    b.ToTable("BPC_REP_FGCPS");
                });

            modelBuilder.Entity("BPCloud_VP.ReportService.Models.BPCReportGRR", b =>
                {
                    b.Property<string>("Client")
                        .HasMaxLength(3)
                        .HasColumnType("character varying(3)");

                    b.Property<string>("Company")
                        .HasMaxLength(4)
                        .HasColumnType("character varying(4)");

                    b.Property<string>("Type")
                        .HasMaxLength(1)
                        .HasColumnType("character varying(1)");

                    b.Property<string>("PatnerID")
                        .HasMaxLength(12)
                        .HasColumnType("character varying(12)");

                    b.Property<string>("Material")
                        .HasMaxLength(450)
                        .HasColumnType("character varying(450)");

                    b.Property<string>("CreatedBy")
                        .HasColumnType("text");

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("timestamp with time zone");

                    b.Property<bool>("IsActive")
                        .HasColumnType("boolean");

                    b.Property<string>("MaterialText")
                        .HasColumnType("text");

                    b.Property<string>("ModifiedBy")
                        .HasColumnType("text");

                    b.Property<DateTime?>("ModifiedOn")
                        .HasColumnType("timestamp with time zone");

                    b.Property<double?>("OrderQty")
                        .HasColumnType("double precision");

                    b.Property<double?>("ReceivedQty")
                        .HasColumnType("double precision");

                    b.Property<double?>("RejectedPPM")
                        .HasColumnType("double precision");

                    b.Property<double?>("ReworkQty")
                        .HasColumnType("double precision");

                    b.Property<string>("Status")
                        .HasColumnType("text");

                    b.HasKey("Client", "Company", "Type", "PatnerID", "Material");

                    b.ToTable("BPC_REP_GRR");
                });

            modelBuilder.Entity("BPCloud_VP.ReportService.Models.BPCReportIP", b =>
                {
                    b.Property<string>("Client")
                        .HasMaxLength(3)
                        .HasColumnType("character varying(3)");

                    b.Property<string>("Company")
                        .HasMaxLength(4)
                        .HasColumnType("character varying(4)");

                    b.Property<string>("Type")
                        .HasMaxLength(1)
                        .HasColumnType("character varying(1)");

                    b.Property<string>("PatnerID")
                        .HasMaxLength(12)
                        .HasColumnType("character varying(12)");

                    b.Property<string>("Material")
                        .HasMaxLength(450)
                        .HasColumnType("character varying(450)");

                    b.Property<string>("CreatedBy")
                        .HasColumnType("text");

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Desc")
                        .HasColumnType("text");

                    b.Property<bool>("IsActive")
                        .HasColumnType("boolean");

                    b.Property<double?>("LowerLimit")
                        .HasColumnType("double precision");

                    b.Property<string>("MaterialChar")
                        .HasColumnType("text");

                    b.Property<string>("MaterialText")
                        .HasColumnType("text");

                    b.Property<string>("Method")
                        .HasColumnType("text");

                    b.Property<string>("ModRule")
                        .HasColumnType("text");

                    b.Property<string>("ModifiedBy")
                        .HasColumnType("text");

                    b.Property<DateTime?>("ModifiedOn")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Status")
                        .HasColumnType("text");

                    b.Property<string>("UOM")
                        .HasColumnType("text");

                    b.Property<double?>("UpperLimit")
                        .HasColumnType("double precision");

                    b.HasKey("Client", "Company", "Type", "PatnerID", "Material");

                    b.ToTable("BPC_REP_IP");
                });

            modelBuilder.Entity("BPCloud_VP.ReportService.Models.BPCReportOV", b =>
                {
                    b.Property<string>("Client")
                        .HasMaxLength(3)
                        .HasColumnType("character varying(3)");

                    b.Property<string>("Company")
                        .HasMaxLength(4)
                        .HasColumnType("character varying(4)");

                    b.Property<string>("Type")
                        .HasMaxLength(1)
                        .HasColumnType("character varying(1)");

                    b.Property<string>("PatnerID")
                        .HasMaxLength(12)
                        .HasColumnType("character varying(12)");

                    b.Property<string>("Material")
                        .HasMaxLength(450)
                        .HasColumnType("character varying(450)");

                    b.Property<double?>("AccQty")
                        .HasColumnType("double precision");

                    b.Property<string>("CreatedBy")
                        .HasColumnType("text");

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("timestamp with time zone");

                    b.Property<double?>("InputQty")
                        .HasColumnType("double precision");

                    b.Property<bool>("IsActive")
                        .HasColumnType("boolean");

                    b.Property<string>("MaterialText")
                        .HasColumnType("text");

                    b.Property<string>("ModifiedBy")
                        .HasColumnType("text");

                    b.Property<DateTime?>("ModifiedOn")
                        .HasColumnType("timestamp with time zone");

                    b.Property<double?>("RejPercentage")
                        .HasColumnType("double precision");

                    b.Property<double?>("RejQty")
                        .HasColumnType("double precision");

                    b.Property<string>("Status")
                        .HasColumnType("text");

                    b.HasKey("Client", "Company", "Type", "PatnerID", "Material");

                    b.ToTable("BPC_REP_OV");
                });

            modelBuilder.Entity("BPCloud_VP.ReportService.Models.BPCReportPPMHeader", b =>
                {
                    b.Property<string>("Client")
                        .HasMaxLength(3)
                        .HasColumnType("character varying(3)");

                    b.Property<string>("Company")
                        .HasMaxLength(4)
                        .HasColumnType("character varying(4)");

                    b.Property<string>("Type")
                        .HasMaxLength(1)
                        .HasColumnType("character varying(1)");

                    b.Property<string>("PatnerID")
                        .HasMaxLength(12)
                        .HasColumnType("character varying(12)");

                    b.Property<DateTime>("Period")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("CreatedBy")
                        .HasColumnType("text");

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("timestamp with time zone");

                    b.Property<bool>("IsActive")
                        .HasColumnType("boolean");

                    b.Property<string>("ModifiedBy")
                        .HasColumnType("text");

                    b.Property<DateTime?>("ModifiedOn")
                        .HasColumnType("timestamp with time zone");

                    b.Property<double?>("PPM")
                        .HasColumnType("double precision");

                    b.Property<double?>("ReceiptQty")
                        .HasColumnType("double precision");

                    b.Property<double?>("RejectedQty")
                        .HasColumnType("double precision");

                    b.Property<double?>("TotalPPM")
                        .HasColumnType("double precision");

                    b.HasKey("Client", "Company", "Type", "PatnerID", "Period");

                    b.ToTable("BPC_REP_PPM_H");
                });

            modelBuilder.Entity("BPCloud_VP.ReportService.Models.BPCReportPPMItem", b =>
                {
                    b.Property<string>("Client")
                        .HasMaxLength(3)
                        .HasColumnType("character varying(3)");

                    b.Property<string>("Company")
                        .HasMaxLength(4)
                        .HasColumnType("character varying(4)");

                    b.Property<string>("Type")
                        .HasMaxLength(1)
                        .HasColumnType("character varying(1)");

                    b.Property<string>("PatnerID")
                        .HasMaxLength(12)
                        .HasColumnType("character varying(12)");

                    b.Property<DateTime>("Period")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Material")
                        .HasMaxLength(450)
                        .HasColumnType("character varying(450)");

                    b.Property<string>("CreatedBy")
                        .HasColumnType("text");

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("timestamp with time zone");

                    b.Property<bool>("IsActive")
                        .HasColumnType("boolean");

                    b.Property<string>("MaterialText")
                        .HasColumnType("text");

                    b.Property<string>("ModifiedBy")
                        .HasColumnType("text");

                    b.Property<DateTime?>("ModifiedOn")
                        .HasColumnType("timestamp with time zone");

                    b.Property<double?>("PPM")
                        .HasColumnType("double precision");

                    b.Property<double?>("ReceiptQty")
                        .HasColumnType("double precision");

                    b.Property<double?>("RejectedQty")
                        .HasColumnType("double precision");

                    b.HasKey("Client", "Company", "Type", "PatnerID", "Period", "Material");

                    b.ToTable("BPC_REP_PPM_I");
                });

            modelBuilder.Entity("BPCloud_VP.ReportService.Models.BPCReportVR", b =>
                {
                    b.Property<string>("Client")
                        .HasMaxLength(3)
                        .HasColumnType("character varying(3)");

                    b.Property<string>("Company")
                        .HasMaxLength(4)
                        .HasColumnType("character varying(4)");

                    b.Property<string>("Type")
                        .HasMaxLength(1)
                        .HasColumnType("character varying(1)");

                    b.Property<string>("PatnerID")
                        .HasMaxLength(12)
                        .HasColumnType("character varying(12)");

                    b.Property<string>("Material")
                        .HasMaxLength(450)
                        .HasColumnType("character varying(450)");

                    b.Property<string>("CreatedBy")
                        .HasColumnType("text");

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("timestamp with time zone");

                    b.Property<bool>("IsActive")
                        .HasColumnType("boolean");

                    b.Property<string>("MaterialText")
                        .HasColumnType("text");

                    b.Property<string>("ModifiedBy")
                        .HasColumnType("text");

                    b.Property<DateTime?>("ModifiedOn")
                        .HasColumnType("timestamp with time zone");

                    b.Property<double?>("OrderQty")
                        .HasColumnType("double precision");

                    b.Property<double?>("ReceivedQty")
                        .HasColumnType("double precision");

                    b.Property<double?>("RejectedPPM")
                        .HasColumnType("double precision");

                    b.Property<double?>("ReworkQty")
                        .HasColumnType("double precision");

                    b.Property<string>("Status")
                        .HasColumnType("text");

                    b.HasKey("Client", "Company", "Type", "PatnerID", "Material");

                    b.ToTable("BPC_REP_VR");
                });

            modelBuilder.Entity("BPCloud_VP.ReportService.Models.BPCSCSTK", b =>
                {
                    b.Property<string>("Client")
                        .HasMaxLength(3)
                        .HasColumnType("character varying(3)");

                    b.Property<string>("Company")
                        .HasMaxLength(4)
                        .HasColumnType("character varying(4)");

                    b.Property<string>("Type")
                        .HasMaxLength(1)
                        .HasColumnType("character varying(1)");

                    b.Property<string>("PatnerID")
                        .HasMaxLength(12)
                        .HasColumnType("character varying(12)");

                    b.Property<string>("PODocument")
                        .HasMaxLength(10)
                        .HasColumnType("character varying(10)");

                    b.Property<string>("Item")
                        .HasMaxLength(4)
                        .HasColumnType("character varying(4)");

                    b.Property<string>("CreatedBy")
                        .HasColumnType("text");

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime?>("Date")
                        .HasColumnType("timestamp with time zone");

                    b.Property<bool>("IsActive")
                        .HasColumnType("boolean");

                    b.Property<double?>("IssuedQty")
                        .HasColumnType("double precision");

                    b.Property<string>("Material")
                        .HasColumnType("text");

                    b.Property<string>("ModifiedBy")
                        .HasColumnType("text");

                    b.Property<DateTime?>("ModifiedOn")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Plant")
                        .HasColumnType("text");

                    b.Property<double?>("StockQty")
                        .HasColumnType("double precision");

                    b.HasKey("Client", "Company", "Type", "PatnerID", "PODocument", "Item");

                    b.ToTable("BPC_SC_STK");
                });
#pragma warning restore 612, 618
        }
    }
}
