﻿// <auto-generated />
using System;
using BPCloud_VP.AuthenticatioService.DBContexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace BPCloud_VP.AuthenticatioService.Migrations
{
    [DbContext(typeof(AuthContext))]
    [Migration("20200814055633_UserPreference")]
    partial class UserPreference
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.14-servicing-32113")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("BPCloud_VP.AuthenticatioService.Models.App", b =>
                {
                    b.Property<int>("AppID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("AppName");

                    b.Property<string>("CreatedBy");

                    b.Property<DateTime>("CreatedOn");

                    b.Property<bool>("IsActive");

                    b.Property<string>("ModifiedBy");

                    b.Property<DateTime?>("ModifiedOn");

                    b.HasKey("AppID");

                    b.ToTable("Apps");
                });

            modelBuilder.Entity("BPCloud_VP.AuthenticatioService.Models.AppUsage", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("AppName");

                    b.Property<string>("CreatedBy");

                    b.Property<DateTime>("CreatedOn");

                    b.Property<bool>("IsActive");

                    b.Property<DateTime>("LastUsedOn");

                    b.Property<string>("ModifiedBy");

                    b.Property<DateTime?>("ModifiedOn");

                    b.Property<int>("UsageCount");

                    b.Property<Guid>("UserID");

                    b.HasKey("ID");

                    b.HasIndex("UserID", "AppName");

                    b.ToTable("AppUsages");
                });

            modelBuilder.Entity("BPCloud_VP.AuthenticatioService.Models.Client", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("Active");

                    b.Property<string>("AllowedOrigin")
                        .HasMaxLength(100);

                    b.Property<int>("ApplicationType");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100);

                    b.Property<int>("RefreshTokenLifeTime");

                    b.Property<string>("Secret")
                        .IsRequired();

                    b.HasKey("Id");

                    b.ToTable("Clients");

                    b.HasData(
                        new { Id = "ngAuthApp", Active = true, AllowedOrigin = "*", ApplicationType = 0, Name = "AngularJS front-end Application", RefreshTokenLifeTime = 7200, Secret = "5YV7M1r981yoGhELyB84aC+KiYksxZf1OY3++C1CtRM=" },
                        new { Id = "consoleApp", Active = true, AllowedOrigin = "*", ApplicationType = 1, Name = "Console Application", RefreshTokenLifeTime = 14400, Secret = "lCXDroz4HhR1EIx8qaz3C13z/quTXBkQ3Q5hj7Qx3aA=" }
                    );
                });

            modelBuilder.Entity("BPCloud_VP.AuthenticatioService.Models.Role", b =>
                {
                    b.Property<Guid>("RoleID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("CreatedBy");

                    b.Property<DateTime>("CreatedOn");

                    b.Property<bool>("IsActive");

                    b.Property<string>("ModifiedBy");

                    b.Property<DateTime?>("ModifiedOn");

                    b.Property<string>("RoleName");

                    b.HasKey("RoleID");

                    b.ToTable("Roles");
                });

            modelBuilder.Entity("BPCloud_VP.AuthenticatioService.Models.RoleAppMap", b =>
                {
                    b.Property<Guid>("RoleID");

                    b.Property<int>("AppID");

                    b.Property<string>("CreatedBy");

                    b.Property<DateTime>("CreatedOn");

                    b.Property<bool>("IsActive");

                    b.Property<string>("ModifiedBy");

                    b.Property<DateTime?>("ModifiedOn");

                    b.HasKey("RoleID", "AppID");

                    b.HasAlternateKey("AppID", "RoleID");

                    b.ToTable("RoleAppMaps");
                });

            modelBuilder.Entity("BPCloud_VP.AuthenticatioService.Models.SessionMaster", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("CreatedBy");

                    b.Property<DateTime>("CreatedOn");

                    b.Property<bool>("IsActive");

                    b.Property<string>("ModifiedBy");

                    b.Property<DateTime?>("ModifiedOn");

                    b.Property<string>("ProjectName");

                    b.Property<int>("SessionTimeOut");

                    b.HasKey("ID");

                    b.ToTable("SessionMasters");
                });

            modelBuilder.Entity("BPCloud_VP.AuthenticatioService.Models.TokenHistory", b =>
                {
                    b.Property<int>("TokenHistoryID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Comment");

                    b.Property<DateTime>("CreatedOn");

                    b.Property<string>("EmailAddress");

                    b.Property<DateTime>("ExpireOn");

                    b.Property<bool>("IsUsed");

                    b.Property<string>("Token");

                    b.Property<DateTime?>("UsedOn");

                    b.Property<Guid>("UserID");

                    b.Property<string>("UserName");

                    b.HasKey("TokenHistoryID");

                    b.ToTable("TokenHistories");
                });

            modelBuilder.Entity("BPCloud_VP.AuthenticatioService.Models.User", b =>
                {
                    b.Property<Guid>("UserID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ContactNumber");

                    b.Property<string>("CreatedBy");

                    b.Property<DateTime>("CreatedOn");

                    b.Property<string>("DisplayName");

                    b.Property<string>("Email");

                    b.Property<DateTime?>("ExpiringOn");

                    b.Property<bool>("IsActive");

                    b.Property<string>("ModifiedBy");

                    b.Property<DateTime?>("ModifiedOn");

                    b.Property<string>("Pass1");

                    b.Property<string>("Pass2");

                    b.Property<string>("Pass3");

                    b.Property<string>("Password");

                    b.Property<bool>("TourStatus");

                    b.Property<string>("UserName");

                    b.HasKey("UserID");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("BPCloud_VP.AuthenticatioService.Models.UserLoginHistory", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("LoginTime");

                    b.Property<DateTime?>("LogoutTime");

                    b.Property<Guid>("UserID");

                    b.Property<string>("UserName");

                    b.HasKey("ID");

                    b.ToTable("UserLoginHistory");
                });

            modelBuilder.Entity("BPCloud_VP.AuthenticatioService.Models.UserPreference", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("CreatedBy");

                    b.Property<DateTime>("CreatedOn");

                    b.Property<string>("ModifiedBy");

                    b.Property<DateTime>("ModifiedOn");

                    b.Property<string>("NavbarPrimaryBackground");

                    b.Property<string>("NavbarSecondaryBackground");

                    b.Property<string>("ToolbarBackground");

                    b.Property<Guid>("UserID");

                    b.HasKey("ID");

                    b.ToTable("UserPreferences");
                });

            modelBuilder.Entity("BPCloud_VP.AuthenticatioService.Models.UserRoleMap", b =>
                {
                    b.Property<Guid>("UserID");

                    b.Property<Guid>("RoleID");

                    b.Property<string>("CreatedBy");

                    b.Property<DateTime>("CreatedOn");

                    b.Property<bool>("IsActive");

                    b.Property<string>("ModifiedBy");

                    b.Property<DateTime?>("ModifiedOn");

                    b.HasKey("UserID", "RoleID");

                    b.HasAlternateKey("RoleID", "UserID");

                    b.ToTable("UserRoleMaps");
                });
#pragma warning restore 612, 618
        }
    }
}
