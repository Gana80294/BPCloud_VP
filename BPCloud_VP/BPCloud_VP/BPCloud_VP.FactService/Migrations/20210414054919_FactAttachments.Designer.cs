﻿// <auto-generated />
using System;
using BPCloud_VP.FactService.DBContexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace BPCloud_VP.FactService.Migrations
{
    [DbContext(typeof(FactContext))]
    [Migration("20210414054919_FactAttachments")]
    partial class FactAttachments
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.14-servicing-32113")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("BPCloud_VP.FactService.Models.BPCAIACT", b =>
                {
                    b.Property<string>("Client")
                        .HasMaxLength(3);

                    b.Property<string>("Company")
                        .HasMaxLength(4);

                    b.Property<string>("Type")
                        .HasMaxLength(1);

                    b.Property<string>("PatnerID")
                        .HasMaxLength(12);

                    b.Property<int>("SeqNo");

                    b.Property<string>("ActType");

                    b.Property<string>("Action");

                    b.Property<string>("ActionText");

                    b.Property<string>("AppID");

                    b.Property<string>("CreatedBy");

                    b.Property<DateTime>("CreatedOn");

                    b.Property<DateTime?>("Date");

                    b.Property<string>("DocNumber");

                    b.Property<bool>("IsActive");

                    b.Property<bool>("IsSeen");

                    b.Property<string>("ModifiedBy");

                    b.Property<DateTime?>("ModifiedOn");

                    b.Property<string>("Status");

                    b.Property<string>("Time");

                    b.HasKey("Client", "Company", "Type", "PatnerID", "SeqNo");

                    b.ToTable("BPC_AI_ACT");
                });

            modelBuilder.Entity("BPCloud_VP.FactService.Models.BPCAttachments", b =>
                {
                    b.Property<int>("AttachmentID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<byte[]>("AttachmentFile");

                    b.Property<string>("AttachmentName");

                    b.Property<string>("Client")
                        .HasMaxLength(3);

                    b.Property<string>("Company")
                        .HasMaxLength(4);

                    b.Property<long>("ContentLength");

                    b.Property<string>("ContentType");

                    b.Property<string>("CreatedBy");

                    b.Property<DateTime>("CreatedOn");

                    b.Property<bool>("IsActive");

                    b.Property<string>("ModifiedBy");

                    b.Property<DateTime?>("ModifiedOn");

                    b.Property<string>("PatnerID")
                        .HasMaxLength(12);

                    b.Property<string>("ReferenceNo");

                    b.Property<string>("Type")
                        .HasMaxLength(1);

                    b.HasKey("AttachmentID");

                    b.HasIndex("Client", "Company", "Type", "PatnerID");

                    b.ToTable("BPC_Attachments");
                });

            modelBuilder.Entity("BPCloud_VP.FactService.Models.BPCCertificate", b =>
                {
                    b.Property<string>("Client")
                        .HasMaxLength(3);

                    b.Property<string>("Company")
                        .HasMaxLength(4);

                    b.Property<string>("Type")
                        .HasMaxLength(1);

                    b.Property<string>("PatnerID")
                        .HasMaxLength(12);

                    b.Property<string>("CertificateType");

                    b.Property<string>("CertificateName");

                    b.Property<string>("Attachment");

                    b.Property<byte[]>("AttachmentFile");

                    b.Property<string>("CreatedBy");

                    b.Property<DateTime>("CreatedOn");

                    b.Property<bool>("IsActive");

                    b.Property<string>("Mandatory");

                    b.Property<string>("ModifiedBy");

                    b.Property<DateTime?>("ModifiedOn");

                    b.Property<DateTime?>("Validity");

                    b.HasKey("Client", "Company", "Type", "PatnerID", "CertificateType", "CertificateName");

                    b.ToTable("BPC_Cert");
                });

            modelBuilder.Entity("BPCloud_VP.FactService.Models.BPCCertificateSupport", b =>
                {
                    b.Property<string>("Client")
                        .HasMaxLength(3);

                    b.Property<string>("Company")
                        .HasMaxLength(4);

                    b.Property<string>("Type")
                        .HasMaxLength(1);

                    b.Property<string>("PatnerID")
                        .HasMaxLength(12);

                    b.Property<string>("CertificateType");

                    b.Property<string>("CertificateName");

                    b.Property<string>("Attachment");

                    b.Property<byte[]>("AttachmentFile");

                    b.Property<string>("CreatedBy");

                    b.Property<DateTime>("CreatedOn");

                    b.Property<string>("ID");

                    b.Property<bool>("IsActive");

                    b.Property<string>("Mandatory");

                    b.Property<string>("ModifiedBy");

                    b.Property<DateTime?>("ModifiedOn");

                    b.Property<DateTime?>("Validity");

                    b.HasKey("Client", "Company", "Type", "PatnerID", "CertificateType", "CertificateName");

                    b.ToTable("BPC_Cert_Support");
                });

            modelBuilder.Entity("BPCloud_VP.FactService.Models.BPCDashboardCard", b =>
                {
                    b.Property<int>("AttachmentID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<byte[]>("AttachmentFile");

                    b.Property<string>("AttachmentName");

                    b.Property<long>("ContentLength");

                    b.Property<string>("ContentType");

                    b.Property<string>("CreatedBy");

                    b.Property<DateTime>("CreatedOn");

                    b.Property<bool>("IsActive");

                    b.Property<string>("ModifiedBy");

                    b.Property<DateTime?>("ModifiedOn");

                    b.HasKey("AttachmentID");

                    b.ToTable("BPC_Dashboard_Card");
                });

            modelBuilder.Entity("BPCloud_VP.FactService.Models.BPCFact", b =>
                {
                    b.Property<string>("Client")
                        .HasMaxLength(3);

                    b.Property<string>("Company")
                        .HasMaxLength(4);

                    b.Property<string>("Type")
                        .HasMaxLength(1);

                    b.Property<string>("PatnerID")
                        .HasMaxLength(12);

                    b.Property<string>("AccountGroup");

                    b.Property<string>("AddressLine1");

                    b.Property<string>("AddressLine2");

                    b.Property<string>("City");

                    b.Property<string>("CompanyCode");

                    b.Property<string>("Country");

                    b.Property<string>("CreatedBy");

                    b.Property<DateTime>("CreatedOn");

                    b.Property<double>("CreditAmount");

                    b.Property<double>("CreditBalance");

                    b.Property<DateTime?>("CreditEvalDate");

                    b.Property<double>("CreditLimit");

                    b.Property<string>("Currency");

                    b.Property<string>("Email1");

                    b.Property<string>("Email2");

                    b.Property<string>("GSTNumber");

                    b.Property<string>("GSTStatus");

                    b.Property<bool>("IsActive");

                    b.Property<bool>("IsBlocked");

                    b.Property<double>("LastPayment");

                    b.Property<DateTime?>("LastPaymentDate");

                    b.Property<string>("LegalName")
                        .HasMaxLength(40);

                    b.Property<bool>("MSME");

                    b.Property<string>("MSME_Att_ID");

                    b.Property<string>("MSME_TYPE");

                    b.Property<string>("ModifiedBy");

                    b.Property<DateTime?>("ModifiedOn");

                    b.Property<string>("Name");

                    b.Property<double>("OutstandingAmount");

                    b.Property<string>("PANNumber");

                    b.Property<string>("Phone1");

                    b.Property<string>("Phone2");

                    b.Property<string>("PinCode");

                    b.Property<string>("Plant");

                    b.Property<string>("PurchaseOrg");

                    b.Property<bool>("RP");

                    b.Property<string>("RP_Att_ID");

                    b.Property<string>("RP_Name");

                    b.Property<string>("RP_Type");

                    b.Property<bool>("Reduced_TDS");

                    b.Property<string>("Role");

                    b.Property<string>("State");

                    b.Property<string>("TDS_Att_ID");

                    b.Property<string>("TDS_RATE");

                    b.Property<string>("TaxID1");

                    b.Property<string>("TaxID2");

                    b.Property<string>("TypeofIndustry");

                    b.Property<string>("VendorCode");

                    b.HasKey("Client", "Company", "Type", "PatnerID");

                    b.ToTable("BPC_Fact");
                });

            modelBuilder.Entity("BPCloud_VP.FactService.Models.BPCFactBank", b =>
                {
                    b.Property<string>("Client")
                        .HasMaxLength(3);

                    b.Property<string>("Company")
                        .HasMaxLength(4);

                    b.Property<string>("Type")
                        .HasMaxLength(1);

                    b.Property<string>("PatnerID")
                        .HasMaxLength(12);

                    b.Property<string>("AccountNo")
                        .HasMaxLength(20);

                    b.Property<string>("BankID");

                    b.Property<string>("BankName");

                    b.Property<string>("Branch");

                    b.Property<string>("City");

                    b.Property<string>("Country");

                    b.Property<string>("CreatedBy");

                    b.Property<DateTime>("CreatedOn");

                    b.Property<string>("IFSC");

                    b.Property<bool>("IsActive");

                    b.Property<string>("ModifiedBy");

                    b.Property<DateTime?>("ModifiedOn");

                    b.Property<string>("Name");

                    b.HasKey("Client", "Company", "Type", "PatnerID", "AccountNo");

                    b.ToTable("BPC_Fact_Bank");
                });

            modelBuilder.Entity("BPCloud_VP.FactService.Models.BPCFactBankSupport", b =>
                {
                    b.Property<string>("Client")
                        .HasMaxLength(3);

                    b.Property<string>("Company")
                        .HasMaxLength(4);

                    b.Property<string>("Type")
                        .HasMaxLength(1);

                    b.Property<string>("PatnerID")
                        .HasMaxLength(12);

                    b.Property<string>("AccountNo")
                        .HasMaxLength(20);

                    b.Property<string>("BankID");

                    b.Property<string>("BankName");

                    b.Property<string>("Branch");

                    b.Property<string>("City");

                    b.Property<string>("Country");

                    b.Property<string>("CreatedBy");

                    b.Property<DateTime>("CreatedOn");

                    b.Property<string>("ID");

                    b.Property<string>("IFSC");

                    b.Property<bool>("IsActive");

                    b.Property<string>("ModifiedBy");

                    b.Property<DateTime?>("ModifiedOn");

                    b.Property<string>("Name");

                    b.HasKey("Client", "Company", "Type", "PatnerID", "AccountNo");

                    b.ToTable("BPC_Fact_Bank_Support");
                });

            modelBuilder.Entity("BPCloud_VP.FactService.Models.BPCFactContactPerson", b =>
                {
                    b.Property<string>("Client")
                        .HasMaxLength(3);

                    b.Property<string>("Company")
                        .HasMaxLength(4);

                    b.Property<string>("Type")
                        .HasMaxLength(1);

                    b.Property<string>("PatnerID")
                        .HasMaxLength(12);

                    b.Property<string>("ContactPersonID")
                        .HasMaxLength(12);

                    b.Property<string>("ContactNumber");

                    b.Property<string>("CreatedBy");

                    b.Property<DateTime>("CreatedOn");

                    b.Property<string>("Department");

                    b.Property<string>("Email");

                    b.Property<bool>("IsActive");

                    b.Property<string>("Item");

                    b.Property<string>("Mobile");

                    b.Property<string>("ModifiedBy");

                    b.Property<DateTime?>("ModifiedOn");

                    b.Property<string>("Name");

                    b.Property<string>("Title");

                    b.HasKey("Client", "Company", "Type", "PatnerID", "ContactPersonID");

                    b.ToTable("BPC_Fact_CP");
                });

            modelBuilder.Entity("BPCloud_VP.FactService.Models.BPCFactSupport", b =>
                {
                    b.Property<string>("Client")
                        .HasMaxLength(3);

                    b.Property<string>("Company")
                        .HasMaxLength(4);

                    b.Property<string>("Type")
                        .HasMaxLength(1);

                    b.Property<string>("PatnerID")
                        .HasMaxLength(12);

                    b.Property<string>("AccountGroup");

                    b.Property<string>("AddressLine1");

                    b.Property<string>("AddressLine2");

                    b.Property<string>("City");

                    b.Property<string>("CompanyCode");

                    b.Property<string>("Country");

                    b.Property<string>("CreatedBy");

                    b.Property<DateTime>("CreatedOn");

                    b.Property<double>("CreditAmount");

                    b.Property<double>("CreditBalance");

                    b.Property<DateTime?>("CreditEvalDate");

                    b.Property<double>("CreditLimit");

                    b.Property<string>("Currency");

                    b.Property<string>("Email1");

                    b.Property<string>("Email2");

                    b.Property<bool>("IsActive");

                    b.Property<double>("LastPayment");

                    b.Property<DateTime?>("LastPaymentDate");

                    b.Property<string>("LegalName")
                        .HasMaxLength(40);

                    b.Property<string>("ModifiedBy");

                    b.Property<DateTime?>("ModifiedOn");

                    b.Property<string>("Name");

                    b.Property<double>("OutstandingAmount");

                    b.Property<string>("Phone1");

                    b.Property<string>("Phone2");

                    b.Property<string>("PinCode");

                    b.Property<string>("PurchaseOrg");

                    b.Property<string>("Role");

                    b.Property<string>("State");

                    b.Property<string>("TaxID1");

                    b.Property<string>("TaxID2");

                    b.Property<string>("TypeofIndustry");

                    b.Property<string>("VendorCode");

                    b.HasKey("Client", "Company", "Type", "PatnerID");

                    b.ToTable("BPC_Fact_Support");
                });

            modelBuilder.Entity("BPCloud_VP.FactService.Models.BPCKRA", b =>
                {
                    b.Property<string>("Client")
                        .HasMaxLength(3);

                    b.Property<string>("Company")
                        .HasMaxLength(4);

                    b.Property<string>("Type")
                        .HasMaxLength(1);

                    b.Property<string>("PatnerID")
                        .HasMaxLength(12);

                    b.Property<string>("KRA")
                        .HasMaxLength(2);

                    b.Property<string>("CreatedBy");

                    b.Property<DateTime>("CreatedOn");

                    b.Property<DateTime?>("EvalDate");

                    b.Property<bool>("IsActive");

                    b.Property<string>("KRAText");

                    b.Property<string>("KRAValue");

                    b.Property<string>("ModifiedBy");

                    b.Property<DateTime?>("ModifiedOn");

                    b.HasKey("Client", "Company", "Type", "PatnerID", "KRA");

                    b.ToTable("BPC_KRA");
                });
#pragma warning restore 612, 618
        }
    }
}
