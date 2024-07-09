﻿// <auto-generated />
using System;
using BPCloud_VP.AuthenticationService.DbContexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace BPCloud_VP.AuthenticationService.Migrations
{
    [DbContext(typeof(AuthContext))]
    partial class AuthContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.6")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("BPCloud_VP.AuthenticationService.Models.ActionLog", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("ID"));

                    b.Property<string>("Action")
                        .HasColumnType("text");

                    b.Property<string>("ActionText")
                        .HasColumnType("text");

                    b.Property<string>("AppName")
                        .HasColumnType("text");

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

                    b.Property<DateTime>("UsedOn")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid>("UserID")
                        .HasColumnType("uuid");

                    b.HasKey("ID");

                    b.ToTable("ActionLogs");
                });

            modelBuilder.Entity("BPCloud_VP.AuthenticationService.Models.App", b =>
                {
                    b.Property<int>("AppID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("AppID"));

                    b.Property<string>("AppName")
                        .HasColumnType("text");

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

                    b.HasKey("AppID");

                    b.ToTable("Apps");
                });

            modelBuilder.Entity("BPCloud_VP.AuthenticationService.Models.AppUsage", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("ID"));

                    b.Property<string>("AppName")
                        .HasColumnType("text");

                    b.Property<string>("CreatedBy")
                        .HasColumnType("text");

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("timestamp with time zone");

                    b.Property<bool>("IsActive")
                        .HasColumnType("boolean");

                    b.Property<DateTime>("LastUsedOn")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("ModifiedBy")
                        .HasColumnType("text");

                    b.Property<DateTime?>("ModifiedOn")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("UsageCount")
                        .HasColumnType("integer");

                    b.Property<Guid>("UserID")
                        .HasColumnType("uuid");

                    b.HasKey("ID");

                    b.HasIndex("UserID", "AppName");

                    b.ToTable("AppUsages");
                });

            modelBuilder.Entity("BPCloud_VP.AuthenticationService.Models.Client", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text");

                    b.Property<bool>("Active")
                        .HasColumnType("boolean");

                    b.Property<string>("AllowedOrigin")
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.Property<int>("ApplicationType")
                        .HasColumnType("integer");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.Property<int>("RefreshTokenLifeTime")
                        .HasColumnType("integer");

                    b.Property<string>("Secret")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Clients");

                    b.HasData(
                        new
                        {
                            Id = "ngAuthApp",
                            Active = true,
                            AllowedOrigin = "*",
                            ApplicationType = 0,
                            Name = "AngularJS front-end Application",
                            RefreshTokenLifeTime = 7200,
                            Secret = "5YV7M1r981yoGhELyB84aC+KiYksxZf1OY3++C1CtRM="
                        },
                        new
                        {
                            Id = "consoleApp",
                            Active = true,
                            AllowedOrigin = "*",
                            ApplicationType = 1,
                            Name = "Console Application",
                            RefreshTokenLifeTime = 14400,
                            Secret = "lCXDroz4HhR1EIx8qaz3C13z/quTXBkQ3Q5hj7Qx3aA="
                        });
                });

            modelBuilder.Entity("BPCloud_VP.AuthenticationService.Models.Role", b =>
                {
                    b.Property<Guid>("RoleID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

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

                    b.Property<string>("RoleName")
                        .HasColumnType("text");

                    b.HasKey("RoleID");

                    b.ToTable("Roles");
                });

            modelBuilder.Entity("BPCloud_VP.AuthenticationService.Models.RoleAppMap", b =>
                {
                    b.Property<Guid>("RoleID")
                        .HasColumnType("uuid")
                        .HasColumnOrder(1);

                    b.Property<int>("AppID")
                        .HasColumnType("integer")
                        .HasColumnOrder(2);

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

                    b.HasKey("RoleID", "AppID");

                    b.ToTable("RoleAppMaps");
                });

            modelBuilder.Entity("BPCloud_VP.AuthenticationService.Models.SessionMaster", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("ID"));

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

                    b.Property<string>("ProjectName")
                        .HasColumnType("text");

                    b.Property<int>("SessionTimeOut")
                        .HasColumnType("integer");

                    b.HasKey("ID");

                    b.ToTable("SessionMasters");
                });

            modelBuilder.Entity("BPCloud_VP.AuthenticationService.Models.TokenHistory", b =>
                {
                    b.Property<int>("TokenHistoryID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("TokenHistoryID"));

                    b.Property<string>("Comment")
                        .HasColumnType("text");

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("EmailAddress")
                        .HasColumnType("text");

                    b.Property<DateTime>("ExpireOn")
                        .HasColumnType("timestamp with time zone");

                    b.Property<bool>("IsUsed")
                        .HasColumnType("boolean");

                    b.Property<string>("Token")
                        .HasColumnType("text");

                    b.Property<DateTime?>("UsedOn")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid>("UserID")
                        .HasColumnType("uuid");

                    b.Property<string>("UserName")
                        .HasColumnType("text");

                    b.HasKey("TokenHistoryID");

                    b.ToTable("TokenHistories");
                });

            modelBuilder.Entity("BPCloud_VP.AuthenticationService.Models.User", b =>
                {
                    b.Property<Guid>("UserID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime?>("AcceptedOn")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("Attempts")
                        .HasColumnType("integer");

                    b.Property<string>("ContactNumber")
                        .HasColumnType("text");

                    b.Property<string>("CreatedBy")
                        .HasColumnType("text");

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("DisplayName")
                        .HasColumnType("text");

                    b.Property<string>("Email")
                        .HasColumnType("text");

                    b.Property<DateTime?>("ExpiringOn")
                        .HasColumnType("timestamp with time zone");

                    b.Property<bool?>("IsAccepted")
                        .HasColumnType("boolean");

                    b.Property<bool>("IsActive")
                        .HasColumnType("boolean");

                    b.Property<bool>("IsBlocked")
                        .HasColumnType("boolean");

                    b.Property<DateTime?>("IsLockDuration")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("LastChangedPassword")
                        .HasColumnType("text");

                    b.Property<string>("ModifiedBy")
                        .HasColumnType("text");

                    b.Property<DateTime?>("ModifiedOn")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Pass1")
                        .HasColumnType("text");

                    b.Property<string>("Pass2")
                        .HasColumnType("text");

                    b.Property<string>("Pass3")
                        .HasColumnType("text");

                    b.Property<string>("Pass4")
                        .HasColumnType("text");

                    b.Property<string>("Pass5")
                        .HasColumnType("text");

                    b.Property<string>("Password")
                        .HasColumnType("text");

                    b.Property<bool>("TourStatus")
                        .HasColumnType("boolean");

                    b.Property<string>("UserName")
                        .HasColumnType("text");

                    b.HasKey("UserID");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("BPCloud_VP.AuthenticationService.Models.UserCompanyMap", b =>
                {
                    b.Property<Guid>("UserID")
                        .HasColumnType("uuid")
                        .HasColumnOrder(0);

                    b.Property<string>("Company")
                        .HasColumnType("text")
                        .HasColumnOrder(1);

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

                    b.HasKey("UserID", "Company");

                    b.ToTable("UserCompanyMaps");
                });

            modelBuilder.Entity("BPCloud_VP.AuthenticationService.Models.UserLoginHistory", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("ID"));

                    b.Property<DateTime>("LoginTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime?>("LogoutTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid>("UserID")
                        .HasColumnType("uuid");

                    b.Property<string>("UserName")
                        .HasColumnType("text");

                    b.HasKey("ID");

                    b.ToTable("UserLoginHistory");
                });

            modelBuilder.Entity("BPCloud_VP.AuthenticationService.Models.UserPlantMap", b =>
                {
                    b.Property<Guid>("UserID")
                        .HasColumnType("uuid")
                        .HasColumnOrder(0);

                    b.Property<string>("PlantID")
                        .HasColumnType("text")
                        .HasColumnOrder(1);

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

                    b.HasKey("UserID", "PlantID");

                    b.ToTable("UserPlantMaps");
                });

            modelBuilder.Entity("BPCloud_VP.AuthenticationService.Models.UserPreference", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("ID"));

                    b.Property<string>("CreatedBy")
                        .HasColumnType("text");

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("ModifiedBy")
                        .HasColumnType("text");

                    b.Property<DateTime>("ModifiedOn")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("NavbarPrimaryBackground")
                        .HasColumnType("text");

                    b.Property<string>("NavbarSecondaryBackground")
                        .HasColumnType("text");

                    b.Property<string>("ToolbarBackground")
                        .HasColumnType("text");

                    b.Property<Guid>("UserID")
                        .HasColumnType("uuid");

                    b.HasKey("ID");

                    b.ToTable("UserPreferences");
                });

            modelBuilder.Entity("BPCloud_VP.AuthenticationService.Models.UserRoleMap", b =>
                {
                    b.Property<Guid>("UserID")
                        .HasColumnType("uuid")
                        .HasColumnOrder(0);

                    b.Property<Guid>("RoleID")
                        .HasColumnType("uuid")
                        .HasColumnOrder(1);

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

                    b.HasKey("UserID", "RoleID");

                    b.ToTable("UserRoleMaps");
                });

            modelBuilder.Entity("BPCloud_VP.AuthenticationService.Models.UserSupportMasterMap", b =>
                {
                    b.Property<Guid>("UserID")
                        .HasColumnType("uuid")
                        .HasColumnOrder(0);

                    b.Property<string>("ReasonCode")
                        .HasColumnType("text")
                        .HasColumnOrder(1);

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

                    b.HasKey("UserID", "ReasonCode");

                    b.ToTable("UserSupportMasterMaps");
                });
#pragma warning restore 612, 618
        }
    }
}
