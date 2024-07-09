﻿// <auto-generated />
using System;
using BPCloud_VP.SupportDeskService.DBContexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace BPCloud_VP.SupportDeskService.Migrations
{
    [DbContext(typeof(SupportDeskContext))]
    partial class SupportDeskContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.14-servicing-32113")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("BPCloud_VP.SupportDeskService.Models.BPCFAQAttachment", b =>
                {
                    b.Property<int>("FAQAttachmentID")
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

                    b.HasKey("FAQAttachmentID");

                    b.ToTable("BPC_FAQ_Attachment");
                });

            modelBuilder.Entity("BPCloud_VP.SupportDeskService.Models.BPCSupportAttachment", b =>
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

                    b.Property<string>("SupportID");

                    b.Property<string>("SupportLogID");

                    b.HasKey("AttachmentID");

                    b.ToTable("BPC_Support_Attachment");
                });

            modelBuilder.Entity("BPCloud_VP.SupportDeskService.Models.SupportAppMaster", b =>
                {
                    b.Property<string>("Client")
                        .HasMaxLength(3);

                    b.Property<string>("Company")
                        .HasMaxLength(4);

                    b.Property<string>("Type")
                        .HasMaxLength(1);

                    b.Property<string>("AppID")
                        .HasMaxLength(24);

                    b.Property<string>("AppName");

                    b.Property<string>("CreatedBy");

                    b.Property<DateTime>("CreatedOn");

                    b.Property<bool>("IsActive");

                    b.Property<string>("ModifiedBy");

                    b.Property<DateTime?>("ModifiedOn");

                    b.HasKey("Client", "Company", "Type", "AppID");

                    b.ToTable("BPC_SAM");
                });

            modelBuilder.Entity("BPCloud_VP.SupportDeskService.Models.SupportHeader", b =>
                {
                    b.Property<string>("Client")
                        .HasMaxLength(3);

                    b.Property<string>("Company")
                        .HasMaxLength(4);

                    b.Property<string>("Type")
                        .HasMaxLength(1);

                    b.Property<string>("PatnerID")
                        .HasMaxLength(12);

                    b.Property<string>("SupportID")
                        .HasMaxLength(12);

                    b.Property<string>("AppID");

                    b.Property<string>("AttachmentID");

                    b.Property<string>("CreatedBy");

                    b.Property<DateTime>("CreatedOn");

                    b.Property<DateTime?>("Date");

                    b.Property<string>("DocumentRefNo");

                    b.Property<bool>("IsActive");

                    b.Property<bool>("IsResolved");

                    b.Property<string>("ModifiedBy");

                    b.Property<DateTime?>("ModifiedOn");

                    b.Property<string>("Plant");

                    b.Property<string>("ReasonCode");

                    b.Property<string>("Remarks");

                    b.Property<string>("Status");

                    b.HasKey("Client", "Company", "Type", "PatnerID", "SupportID");

                    b.HasIndex("ReasonCode", "Plant");

                    b.ToTable("BPC_SH");
                });

            modelBuilder.Entity("BPCloud_VP.SupportDeskService.Models.SupportLog", b =>
                {
                    b.Property<string>("Client")
                        .HasMaxLength(3);

                    b.Property<string>("Company")
                        .HasMaxLength(4);

                    b.Property<string>("Type")
                        .HasMaxLength(1);

                    b.Property<string>("PatnerID")
                        .HasMaxLength(12);

                    b.Property<string>("SupportID")
                        .HasMaxLength(12);

                    b.Property<string>("SupportLogID")
                        .HasMaxLength(12);

                    b.Property<string>("AttachmentID");

                    b.Property<string>("CreatedBy");

                    b.Property<DateTime>("CreatedOn");

                    b.Property<bool>("IsActive");

                    b.Property<bool>("IsResolved");

                    b.Property<string>("ModifiedBy");

                    b.Property<DateTime?>("ModifiedOn");

                    b.Property<string>("Remarks");

                    b.HasKey("Client", "Company", "Type", "PatnerID", "SupportID", "SupportLogID");

                    b.ToTable("BPC_SL");
                });

            modelBuilder.Entity("BPCloud_VP.SupportDeskService.Models.SupportMaster", b =>
                {
                    b.Property<string>("Client")
                        .HasMaxLength(3);

                    b.Property<string>("Company")
                        .HasMaxLength(4);

                    b.Property<string>("Type")
                        .HasMaxLength(1);

                    b.Property<string>("ReasonCode")
                        .HasMaxLength(24);

                    b.Property<string>("CreatedBy");

                    b.Property<DateTime>("CreatedOn");

                    b.Property<bool>("IsActive");

                    b.Property<string>("ModifiedBy");

                    b.Property<DateTime?>("ModifiedOn");

                    b.Property<string>("ReasonText");

                    b.HasKey("Client", "Company", "Type", "ReasonCode");

                    b.ToTable("BPC_SM");
                });
#pragma warning restore 612, 618
        }
    }
}
