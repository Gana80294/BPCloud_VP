using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BPCloud_VP_POService.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.CreateTable(
            //    name: "BP_BC_H",
            //    columns: table => new
            //    {
            //        IsActive = table.Column<bool>(nullable: false),
            //        CreatedOn = table.Column<DateTime>(nullable: false),
            //        CreatedBy = table.Column<string>(nullable: true),
            //        ModifiedOn = table.Column<DateTime>(nullable: true),
            //        ModifiedBy = table.Column<string>(nullable: true),
            //        Client = table.Column<string>(maxLength: 3, nullable: false),
            //        Company = table.Column<string>(maxLength: 4, nullable: false),
            //        Type = table.Column<string>(maxLength: 1, nullable: false),
            //        PatnerID = table.Column<string>(maxLength: 12, nullable: false),
            //        FiscalYear = table.Column<string>(maxLength: 4, nullable: false),
            //        BillAmount = table.Column<double>(nullable: false),
            //        PaidAmont = table.Column<double>(nullable: false),
            //        TDSAmount = table.Column<double>(nullable: false),
            //        TotalPaidAmount = table.Column<double>(nullable: false),
            //        DownPayment = table.Column<double>(nullable: false),
            //        NetDueAmount = table.Column<double>(nullable: false),
            //        Currency = table.Column<string>(nullable: true),
            //        BalDate = table.Column<DateTime>(nullable: true),
            //        Status = table.Column<string>(nullable: true),
            //        AcceptedOn = table.Column<DateTime>(nullable: true),
            //        AcceptedBy = table.Column<string>(nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_BP_BC_H", x => new { x.Client, x.Company, x.Type, x.PatnerID, x.FiscalYear });
            //    });

            //migrationBuilder.CreateTable(
            //    name: "BP_BC_I",
            //    columns: table => new
            //    {
            //        IsActive = table.Column<bool>(nullable: false),
            //        CreatedOn = table.Column<DateTime>(nullable: false),
            //        CreatedBy = table.Column<string>(nullable: true),
            //        ModifiedOn = table.Column<DateTime>(nullable: true),
            //        ModifiedBy = table.Column<string>(nullable: true),
            //        Client = table.Column<string>(maxLength: 3, nullable: false),
            //        Company = table.Column<string>(maxLength: 4, nullable: false),
            //        Type = table.Column<string>(maxLength: 1, nullable: false),
            //        PatnerID = table.Column<string>(maxLength: 12, nullable: false),
            //        FiscalYear = table.Column<string>(maxLength: 4, nullable: false),
            //        DocNumber = table.Column<string>(nullable: false),
            //        DocDate = table.Column<DateTime>(nullable: true),
            //        InvoiceNumber = table.Column<string>(nullable: true),
            //        InvoiceAmount = table.Column<double>(nullable: false),
            //        BillAmount = table.Column<double>(nullable: false),
            //        PaidAmont = table.Column<double>(nullable: false),
            //        TDSAmount = table.Column<double>(nullable: false),
            //        TotalPaidAmount = table.Column<double>(nullable: false),
            //        DownPayment = table.Column<double>(nullable: false),
            //        NetDueAmount = table.Column<double>(nullable: false),
            //        Currency = table.Column<string>(nullable: true),
            //        BalDate = table.Column<DateTime>(nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_BP_BC_I", x => new { x.Client, x.Company, x.Type, x.PatnerID, x.FiscalYear, x.DocNumber });
            //    });

            //migrationBuilder.CreateTable(
            //    name: "BPC_ASN_Field_Master",
            //    columns: table => new
            //    {
            //        IsActive = table.Column<bool>(nullable: false),
            //        CreatedOn = table.Column<DateTime>(nullable: false),
            //        CreatedBy = table.Column<string>(nullable: true),
            //        ModifiedOn = table.Column<DateTime>(nullable: true),
            //        ModifiedBy = table.Column<string>(nullable: true),
            //        ID = table.Column<int>(nullable: false)
            //            .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
            //        Field = table.Column<string>(nullable: true),
            //        FieldName = table.Column<string>(nullable: true),
            //        Text = table.Column<string>(nullable: true),
            //        DefaultValue = table.Column<string>(nullable: true),
            //        Mandatory = table.Column<bool>(nullable: false),
            //        Invisible = table.Column<bool>(nullable: false)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_BPC_ASN_Field_Master", x => x.ID);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "BPC_ASN_H",
            //    columns: table => new
            //    {
            //        IsActive = table.Column<bool>(nullable: false),
            //        CreatedOn = table.Column<DateTime>(nullable: false),
            //        CreatedBy = table.Column<string>(nullable: true),
            //        ModifiedOn = table.Column<DateTime>(nullable: true),
            //        ModifiedBy = table.Column<string>(nullable: true),
            //        Client = table.Column<string>(maxLength: 3, nullable: false),
            //        Company = table.Column<string>(maxLength: 4, nullable: false),
            //        Type = table.Column<string>(maxLength: 1, nullable: false),
            //        PatnerID = table.Column<string>(maxLength: 12, nullable: false),
            //        ASNNumber = table.Column<string>(maxLength: 12, nullable: false),
            //        ASNDate = table.Column<DateTime>(nullable: true),
            //        DocNumber = table.Column<string>(nullable: true),
            //        TransportMode = table.Column<string>(nullable: true),
            //        VessleNumber = table.Column<string>(nullable: true),
            //        CountryOfOrigin = table.Column<string>(nullable: true),
            //        AWBNumber = table.Column<string>(nullable: true),
            //        AWBDate = table.Column<DateTime>(nullable: true),
            //        DepartureDate = table.Column<DateTime>(nullable: true),
            //        ArrivalDate = table.Column<DateTime>(nullable: true),
            //        ShippingAgency = table.Column<string>(nullable: true),
            //        GrossWeight = table.Column<double>(nullable: true),
            //        GrossWeightUOM = table.Column<string>(nullable: true),
            //        NetWeight = table.Column<double>(nullable: true),
            //        NetWeightUOM = table.Column<string>(nullable: true),
            //        VolumetricWeight = table.Column<double>(nullable: true),
            //        VolumetricWeightUOM = table.Column<string>(nullable: true),
            //        NumberOfPacks = table.Column<int>(nullable: true),
            //        InvoiceNumber = table.Column<string>(nullable: true),
            //        InvoiceDate = table.Column<DateTime>(nullable: true),
            //        POBasicPrice = table.Column<double>(nullable: true),
            //        TaxAmount = table.Column<double>(nullable: true),
            //        InvoiceAmount = table.Column<double>(nullable: true),
            //        InvoiceAmountUOM = table.Column<string>(nullable: true),
            //        InvDocReferenceNo = table.Column<string>(nullable: true),
            //        Status = table.Column<string>(nullable: true),
            //        IsSubmitted = table.Column<bool>(nullable: false),
            //        ArrivalDateInterval = table.Column<int>(nullable: false),
            //        BillOfLading = table.Column<string>(maxLength: 20, nullable: true),
            //        TransporterName = table.Column<string>(maxLength: 40, nullable: true),
            //        AccessibleValue = table.Column<double>(nullable: true),
            //        ContactPerson = table.Column<string>(maxLength: 40, nullable: true),
            //        ContactPersonNo = table.Column<string>(maxLength: 14, nullable: true),
            //        Field1 = table.Column<string>(nullable: true),
            //        Field2 = table.Column<string>(nullable: true),
            //        Field3 = table.Column<string>(nullable: true),
            //        Field4 = table.Column<string>(nullable: true),
            //        Field5 = table.Column<string>(nullable: true),
            //        Field6 = table.Column<string>(nullable: true),
            //        Field7 = table.Column<string>(nullable: true),
            //        Field8 = table.Column<string>(nullable: true),
            //        Field9 = table.Column<string>(nullable: true),
            //        Field10 = table.Column<string>(nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_BPC_ASN_H", x => new { x.Client, x.Company, x.Type, x.PatnerID, x.ASNNumber });
            //    });

            //migrationBuilder.CreateTable(
            //    name: "BPC_ASN_I",
            //    columns: table => new
            //    {
            //        IsActive = table.Column<bool>(nullable: false),
            //        CreatedOn = table.Column<DateTime>(nullable: false),
            //        CreatedBy = table.Column<string>(nullable: true),
            //        ModifiedOn = table.Column<DateTime>(nullable: true),
            //        ModifiedBy = table.Column<string>(nullable: true),
            //        Client = table.Column<string>(maxLength: 3, nullable: false),
            //        Company = table.Column<string>(maxLength: 4, nullable: false),
            //        Type = table.Column<string>(maxLength: 1, nullable: false),
            //        PatnerID = table.Column<string>(maxLength: 12, nullable: false),
            //        ASNNumber = table.Column<string>(maxLength: 12, nullable: false),
            //        Item = table.Column<string>(maxLength: 4, nullable: false),
            //        Material = table.Column<string>(nullable: true),
            //        MaterialText = table.Column<string>(nullable: true),
            //        DeliveryDate = table.Column<DateTime>(nullable: true),
            //        OrderedQty = table.Column<double>(nullable: false),
            //        CompletedQty = table.Column<double>(nullable: false),
            //        TransitQty = table.Column<double>(nullable: false),
            //        OpenQty = table.Column<double>(nullable: false),
            //        ASNQty = table.Column<double>(nullable: false),
            //        UOM = table.Column<string>(nullable: true),
            //        HSN = table.Column<string>(nullable: true),
            //        PlantCode = table.Column<string>(nullable: true),
            //        UnitPrice = table.Column<double>(nullable: true),
            //        Value = table.Column<double>(nullable: true),
            //        TaxAmount = table.Column<double>(nullable: true),
            //        TaxCode = table.Column<string>(nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_BPC_ASN_I", x => new { x.Client, x.Company, x.Type, x.PatnerID, x.ASNNumber, x.Item });
            //    });

            //migrationBuilder.CreateTable(
            //    name: "BPC_ASN_I_Batch",
            //    columns: table => new
            //    {
            //        IsActive = table.Column<bool>(nullable: false),
            //        CreatedOn = table.Column<DateTime>(nullable: false),
            //        CreatedBy = table.Column<string>(nullable: true),
            //        ModifiedOn = table.Column<DateTime>(nullable: true),
            //        ModifiedBy = table.Column<string>(nullable: true),
            //        Client = table.Column<string>(maxLength: 3, nullable: false),
            //        Company = table.Column<string>(maxLength: 4, nullable: false),
            //        Type = table.Column<string>(maxLength: 1, nullable: false),
            //        PatnerID = table.Column<string>(maxLength: 12, nullable: false),
            //        ASNNumber = table.Column<string>(maxLength: 12, nullable: false),
            //        Item = table.Column<string>(maxLength: 4, nullable: false),
            //        Batch = table.Column<string>(maxLength: 24, nullable: false),
            //        Qty = table.Column<double>(nullable: false),
            //        ManufactureDate = table.Column<DateTime>(nullable: true),
            //        ExpiryDate = table.Column<DateTime>(nullable: true),
            //        ManufactureCountry = table.Column<string>(nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_BPC_ASN_I_Batch", x => new { x.Client, x.Company, x.Type, x.PatnerID, x.ASNNumber, x.Item, x.Batch });
            //    });

            //migrationBuilder.CreateTable(
            //    name: "BPC_ASN_I_SES",
            //    columns: table => new
            //    {
            //        IsActive = table.Column<bool>(nullable: false),
            //        CreatedOn = table.Column<DateTime>(nullable: false),
            //        CreatedBy = table.Column<string>(nullable: true),
            //        ModifiedOn = table.Column<DateTime>(nullable: true),
            //        ModifiedBy = table.Column<string>(nullable: true),
            //        Client = table.Column<string>(maxLength: 3, nullable: false),
            //        Company = table.Column<string>(maxLength: 4, nullable: false),
            //        Type = table.Column<string>(maxLength: 1, nullable: false),
            //        PatnerID = table.Column<string>(maxLength: 12, nullable: false),
            //        ASNNumber = table.Column<string>(maxLength: 12, nullable: false),
            //        Item = table.Column<string>(maxLength: 4, nullable: false),
            //        ServiceNo = table.Column<string>(maxLength: 24, nullable: false),
            //        ServiceItem = table.Column<string>(nullable: true),
            //        OrderedQty = table.Column<double>(nullable: false),
            //        OpenQty = table.Column<double>(nullable: false),
            //        ServiceQty = table.Column<double>(nullable: false)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_BPC_ASN_I_SES", x => new { x.Client, x.Company, x.Type, x.PatnerID, x.ASNNumber, x.Item, x.ServiceNo });
            //    });

            //migrationBuilder.CreateTable(
            //    name: "BPC_ASN_Pack",
            //    columns: table => new
            //    {
            //        IsActive = table.Column<bool>(nullable: false),
            //        CreatedOn = table.Column<DateTime>(nullable: false),
            //        CreatedBy = table.Column<string>(nullable: true),
            //        ModifiedOn = table.Column<DateTime>(nullable: true),
            //        ModifiedBy = table.Column<string>(nullable: true),
            //        Client = table.Column<string>(maxLength: 3, nullable: false),
            //        Company = table.Column<string>(maxLength: 4, nullable: false),
            //        Type = table.Column<string>(maxLength: 1, nullable: false),
            //        PatnerID = table.Column<string>(maxLength: 12, nullable: false),
            //        ASNNumber = table.Column<string>(maxLength: 12, nullable: false),
            //        PackageID = table.Column<string>(maxLength: 12, nullable: false),
            //        ReferenceNumber = table.Column<string>(nullable: true),
            //        Dimension = table.Column<string>(nullable: true),
            //        GrossWeight = table.Column<double>(nullable: true),
            //        GrossWeightUOM = table.Column<string>(nullable: true),
            //        NetWeight = table.Column<double>(nullable: true),
            //        NetWeightUOM = table.Column<string>(nullable: true),
            //        VolumetricWeight = table.Column<double>(nullable: true),
            //        VolumetricWeightUOM = table.Column<string>(nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_BPC_ASN_Pack", x => new { x.Client, x.Company, x.Type, x.PatnerID, x.ASNNumber, x.PackageID });
            //    });

            //migrationBuilder.CreateTable(
            //    name: "BPC_CEO_Message",
            //    columns: table => new
            //    {
            //        IsActive = table.Column<bool>(nullable: false),
            //        CreatedOn = table.Column<DateTime>(nullable: false),
            //        CreatedBy = table.Column<string>(nullable: true),
            //        ModifiedOn = table.Column<DateTime>(nullable: true),
            //        ModifiedBy = table.Column<string>(nullable: true),
            //        MessageID = table.Column<int>(nullable: false)
            //            .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
            //        CEOMessage = table.Column<string>(nullable: true),
            //        IsReleased = table.Column<bool>(nullable: false)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_BPC_CEO_Message", x => x.MessageID);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "BPC_Country_Master",
            //    columns: table => new
            //    {
            //        IsActive = table.Column<bool>(nullable: false),
            //        CreatedOn = table.Column<DateTime>(nullable: false),
            //        CreatedBy = table.Column<string>(nullable: true),
            //        ModifiedOn = table.Column<DateTime>(nullable: true),
            //        ModifiedBy = table.Column<string>(nullable: true),
            //        ID = table.Column<int>(nullable: false)
            //            .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
            //        CountryCode = table.Column<string>(nullable: true),
            //        CountryName = table.Column<string>(nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_BPC_Country_Master", x => x.ID);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "BPC_Currency_Master",
            //    columns: table => new
            //    {
            //        IsActive = table.Column<bool>(nullable: false),
            //        CreatedOn = table.Column<DateTime>(nullable: false),
            //        CreatedBy = table.Column<string>(nullable: true),
            //        ModifiedOn = table.Column<DateTime>(nullable: true),
            //        ModifiedBy = table.Column<string>(nullable: true),
            //        ID = table.Column<int>(nullable: false)
            //            .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
            //        CurrencyCode = table.Column<string>(nullable: true),
            //        CurrencyName = table.Column<string>(nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_BPC_Currency_Master", x => x.ID);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "BPC_DocumentCenter",
            //    columns: table => new
            //    {
            //        IsActive = table.Column<bool>(nullable: false),
            //        CreatedOn = table.Column<DateTime>(nullable: false),
            //        CreatedBy = table.Column<string>(nullable: true),
            //        ModifiedOn = table.Column<DateTime>(nullable: true),
            //        ModifiedBy = table.Column<string>(nullable: true),
            //        ID = table.Column<int>(nullable: false)
            //            .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
            //        ASNNumber = table.Column<string>(nullable: true),
            //        DocumentType = table.Column<string>(nullable: true),
            //        DocumentTitle = table.Column<string>(nullable: true),
            //        Filename = table.Column<string>(nullable: true),
            //        AttachmentReferenceNo = table.Column<string>(nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_BPC_DocumentCenter", x => x.ID);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "BPC_DocumentCenter_Master",
            //    columns: table => new
            //    {
            //        IsActive = table.Column<bool>(nullable: false),
            //        CreatedOn = table.Column<DateTime>(nullable: false),
            //        CreatedBy = table.Column<string>(nullable: true),
            //        ModifiedOn = table.Column<DateTime>(nullable: true),
            //        ModifiedBy = table.Column<string>(nullable: true),
            //        AppID = table.Column<int>(nullable: false),
            //        DocumentType = table.Column<string>(nullable: false),
            //        Text = table.Column<string>(nullable: true),
            //        Mandatory = table.Column<bool>(nullable: false),
            //        Extension = table.Column<string>(nullable: true),
            //        SizeInKB = table.Column<double>(nullable: false),
            //        ForwardMail = table.Column<string>(nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_BPC_DocumentCenter_Master", x => x.DocumentType);
            //        table.UniqueConstraint("AK_BPC_DocumentCenter_Master_AppID", x => x.AppID);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "BPC_ExpenseType_Master",
            //    columns: table => new
            //    {
            //        IsActive = table.Column<bool>(nullable: false),
            //        CreatedOn = table.Column<DateTime>(nullable: false),
            //        CreatedBy = table.Column<string>(nullable: true),
            //        ModifiedOn = table.Column<DateTime>(nullable: true),
            //        ModifiedBy = table.Column<string>(nullable: true),
            //        ID = table.Column<int>(nullable: false)
            //            .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
            //        Client = table.Column<string>(nullable: true),
            //        Company = table.Column<string>(nullable: true),
            //        Type = table.Column<string>(nullable: true),
            //        PatnerID = table.Column<string>(nullable: true),
            //        ExpenseType = table.Column<string>(nullable: true),
            //        MaxAmount = table.Column<string>(nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_BPC_ExpenseType_Master", x => x.ID);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "BPC_FLIP_Attachment",
            //    columns: table => new
            //    {
            //        IsActive = table.Column<bool>(nullable: false),
            //        CreatedOn = table.Column<DateTime>(nullable: false),
            //        CreatedBy = table.Column<string>(nullable: true),
            //        ModifiedOn = table.Column<DateTime>(nullable: true),
            //        ModifiedBy = table.Column<string>(nullable: true),
            //        AttachmentID = table.Column<int>(nullable: false)
            //            .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
            //        PO = table.Column<string>(nullable: true),
            //        AttachmentName = table.Column<string>(nullable: true),
            //        ContentType = table.Column<string>(nullable: true),
            //        ContentLength = table.Column<long>(nullable: false),
            //        AttachmentFile = table.Column<byte[]>(nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_BPC_FLIP_Attachment", x => x.AttachmentID);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "BPC_FLIP_Cost",
            //    columns: table => new
            //    {
            //        IsActive = table.Column<bool>(nullable: false),
            //        CreatedOn = table.Column<DateTime>(nullable: false),
            //        CreatedBy = table.Column<string>(nullable: true),
            //        ModifiedOn = table.Column<DateTime>(nullable: true),
            //        ModifiedBy = table.Column<string>(nullable: true),
            //        Client = table.Column<string>(maxLength: 3, nullable: false),
            //        Company = table.Column<string>(maxLength: 4, nullable: false),
            //        Type = table.Column<string>(maxLength: 1, nullable: false),
            //        PatnerID = table.Column<string>(maxLength: 12, nullable: false),
            //        FLIPID = table.Column<string>(maxLength: 12, nullable: false),
            //        ExpenceType = table.Column<string>(maxLength: 10, nullable: false),
            //        Amount = table.Column<double>(nullable: false),
            //        Remarks = table.Column<string>(nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_BPC_FLIP_Cost", x => new { x.Client, x.Company, x.Type, x.PatnerID, x.FLIPID, x.ExpenceType });
            //    });

            //migrationBuilder.CreateTable(
            //    name: "BPC_FLIP_H",
            //    columns: table => new
            //    {
            //        IsActive = table.Column<bool>(nullable: false),
            //        CreatedOn = table.Column<DateTime>(nullable: false),
            //        CreatedBy = table.Column<string>(nullable: true),
            //        ModifiedOn = table.Column<DateTime>(nullable: true),
            //        ModifiedBy = table.Column<string>(nullable: true),
            //        Client = table.Column<string>(maxLength: 3, nullable: false),
            //        Company = table.Column<string>(maxLength: 4, nullable: false),
            //        Type = table.Column<string>(maxLength: 1, nullable: false),
            //        PatnerID = table.Column<string>(maxLength: 12, nullable: false),
            //        FLIPID = table.Column<string>(maxLength: 12, nullable: false),
            //        DocNumber = table.Column<string>(nullable: true),
            //        InvoiceNumber = table.Column<string>(nullable: true),
            //        InvoiceDate = table.Column<DateTime>(nullable: true),
            //        InvoiceAmount = table.Column<double>(nullable: true),
            //        InvoiceCurrency = table.Column<string>(nullable: true),
            //        InvoiceType = table.Column<string>(nullable: true),
            //        InvoiceDocID = table.Column<string>(nullable: true),
            //        InvoiceAttachmentName = table.Column<string>(nullable: true),
            //        IsInvoiceOrCertified = table.Column<string>(nullable: true),
            //        IsInvoiceFlag = table.Column<string>(nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_BPC_FLIP_H", x => new { x.Client, x.Company, x.Type, x.PatnerID, x.FLIPID });
            //    });

            //migrationBuilder.CreateTable(
            //    name: "BPC_FLIP_I",
            //    columns: table => new
            //    {
            //        IsActive = table.Column<bool>(nullable: false),
            //        CreatedOn = table.Column<DateTime>(nullable: false),
            //        CreatedBy = table.Column<string>(nullable: true),
            //        ModifiedOn = table.Column<DateTime>(nullable: true),
            //        ModifiedBy = table.Column<string>(nullable: true),
            //        Client = table.Column<string>(maxLength: 3, nullable: false),
            //        Company = table.Column<string>(maxLength: 4, nullable: false),
            //        Type = table.Column<string>(maxLength: 1, nullable: false),
            //        PatnerID = table.Column<string>(maxLength: 12, nullable: false),
            //        FLIPID = table.Column<string>(maxLength: 12, nullable: false),
            //        Item = table.Column<string>(maxLength: 4, nullable: false),
            //        Material = table.Column<string>(nullable: true),
            //        MaterialText = table.Column<string>(nullable: true),
            //        DeliveryDate = table.Column<DateTime>(nullable: true),
            //        OrderedQty = table.Column<double>(nullable: false),
            //        OpenQty = table.Column<double>(nullable: false),
            //        InvoiceQty = table.Column<double>(nullable: false),
            //        UOM = table.Column<string>(nullable: true),
            //        HSN = table.Column<string>(nullable: true),
            //        Price = table.Column<double>(nullable: false),
            //        Tax = table.Column<double>(nullable: false),
            //        Amount = table.Column<double>(nullable: false)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_BPC_FLIP_I", x => new { x.Client, x.Company, x.Type, x.PatnerID, x.FLIPID, x.Item });
            //    });

            //migrationBuilder.CreateTable(
            //    name: "BPC_Gate_HV",
            //    columns: table => new
            //    {
            //        IsActive = table.Column<bool>(nullable: false),
            //        CreatedOn = table.Column<DateTime>(nullable: false),
            //        CreatedBy = table.Column<string>(nullable: true),
            //        ModifiedOn = table.Column<DateTime>(nullable: true),
            //        ModifiedBy = table.Column<string>(nullable: true),
            //        Client = table.Column<string>(maxLength: 3, nullable: false),
            //        Company = table.Column<string>(maxLength: 4, nullable: false),
            //        Type = table.Column<string>(maxLength: 1, nullable: false),
            //        PatnerID = table.Column<string>(maxLength: 12, nullable: false),
            //        Date = table.Column<DateTime>(nullable: true),
            //        Time = table.Column<DateTime>(nullable: true),
            //        Truck = table.Column<string>(nullable: true),
            //        Partner = table.Column<string>(nullable: false),
            //        DocNo = table.Column<string>(nullable: false),
            //        Transporter = table.Column<string>(nullable: true),
            //        Gate = table.Column<string>(nullable: true),
            //        Plant = table.Column<string>(nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_BPC_Gate_HV", x => new { x.Client, x.Company, x.Type, x.PatnerID, x.DocNo, x.Partner });
            //    });

            //migrationBuilder.CreateTable(
            //    name: "BPC_Gate_TA",
            //    columns: table => new
            //    {
            //        IsActive = table.Column<bool>(nullable: false),
            //        CreatedOn = table.Column<DateTime>(nullable: false),
            //        CreatedBy = table.Column<string>(nullable: true),
            //        ModifiedOn = table.Column<DateTime>(nullable: true),
            //        ModifiedBy = table.Column<string>(nullable: true),
            //        Client = table.Column<string>(maxLength: 3, nullable: false),
            //        Company = table.Column<string>(maxLength: 4, nullable: false),
            //        Type = table.Column<string>(maxLength: 1, nullable: false),
            //        PatnerID = table.Column<string>(maxLength: 12, nullable: false),
            //        EntryDate = table.Column<DateTime>(nullable: true),
            //        EntryTime = table.Column<DateTime>(nullable: true),
            //        Truck = table.Column<string>(nullable: true),
            //        Partner = table.Column<string>(nullable: false),
            //        DocNo = table.Column<string>(nullable: false),
            //        Transporter = table.Column<string>(nullable: true),
            //        Gate = table.Column<string>(nullable: true),
            //        ExitDt = table.Column<string>(nullable: true),
            //        ExitTime = table.Column<string>(nullable: true),
            //        TATime = table.Column<string>(nullable: true),
            //        Exception = table.Column<string>(nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_BPC_Gate_TA", x => new { x.Client, x.Company, x.Type, x.PatnerID, x.DocNo, x.Partner });
            //    });

            //migrationBuilder.CreateTable(
            //    name: "BPC_INV",
            //    columns: table => new
            //    {
            //        IsActive = table.Column<bool>(nullable: false),
            //        CreatedOn = table.Column<DateTime>(nullable: false),
            //        CreatedBy = table.Column<string>(nullable: true),
            //        ModifiedOn = table.Column<DateTime>(nullable: true),
            //        ModifiedBy = table.Column<string>(nullable: true),
            //        Client = table.Column<string>(maxLength: 3, nullable: false),
            //        Company = table.Column<string>(maxLength: 4, nullable: false),
            //        Type = table.Column<string>(maxLength: 1, nullable: false),
            //        PatnerID = table.Column<string>(maxLength: 12, nullable: false),
            //        FiscalYear = table.Column<string>(maxLength: 4, nullable: false),
            //        InvoiceNo = table.Column<string>(maxLength: 16, nullable: false),
            //        InvoiceDate = table.Column<DateTime>(nullable: true),
            //        InvoiceAmount = table.Column<double>(nullable: false),
            //        PoReference = table.Column<string>(nullable: true),
            //        PaidAmount = table.Column<double>(nullable: false),
            //        Currency = table.Column<string>(nullable: true),
            //        DateofPayment = table.Column<DateTime>(nullable: true),
            //        Status = table.Column<string>(nullable: true),
            //        AttID = table.Column<string>(nullable: true),
            //        PODDate = table.Column<DateTime>(nullable: true),
            //        PODConfirmedBy = table.Column<string>(nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_BPC_INV", x => new { x.Client, x.Company, x.Type, x.PatnerID, x.FiscalYear, x.InvoiceNo });
            //    });

            //migrationBuilder.CreateTable(
            //    name: "BPC_INV_I",
            //    columns: table => new
            //    {
            //        IsActive = table.Column<bool>(nullable: false),
            //        CreatedOn = table.Column<DateTime>(nullable: false),
            //        CreatedBy = table.Column<string>(nullable: true),
            //        ModifiedOn = table.Column<DateTime>(nullable: true),
            //        ModifiedBy = table.Column<string>(nullable: true),
            //        Client = table.Column<string>(maxLength: 3, nullable: false),
            //        Company = table.Column<string>(maxLength: 4, nullable: false),
            //        Type = table.Column<string>(maxLength: 1, nullable: false),
            //        PatnerID = table.Column<string>(maxLength: 12, nullable: false),
            //        FiscalYear = table.Column<string>(maxLength: 4, nullable: false),
            //        InvoiceNo = table.Column<string>(maxLength: 16, nullable: false),
            //        Item = table.Column<string>(maxLength: 12, nullable: false),
            //        Material = table.Column<string>(nullable: true),
            //        MaterialText = table.Column<string>(nullable: true),
            //        InvoiceQty = table.Column<double>(nullable: false),
            //        PODQty = table.Column<double>(nullable: false),
            //        ReasonCode = table.Column<string>(nullable: true),
            //        Status = table.Column<string>(nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_BPC_INV_I", x => new { x.Client, x.Company, x.Type, x.PatnerID, x.FiscalYear, x.InvoiceNo, x.Item });
            //    });

            //migrationBuilder.CreateTable(
            //    name: "BPC_OF_AI_ACT",
            //    columns: table => new
            //    {
            //        IsActive = table.Column<bool>(nullable: false),
            //        CreatedOn = table.Column<DateTime>(nullable: false),
            //        CreatedBy = table.Column<string>(nullable: true),
            //        ModifiedOn = table.Column<DateTime>(nullable: true),
            //        ModifiedBy = table.Column<string>(nullable: true),
            //        Client = table.Column<string>(maxLength: 3, nullable: false),
            //        Company = table.Column<string>(maxLength: 4, nullable: false),
            //        Type = table.Column<string>(maxLength: 1, nullable: false),
            //        PatnerID = table.Column<string>(maxLength: 12, nullable: false),
            //        SeqNo = table.Column<string>(maxLength: 4, nullable: false),
            //        AppID = table.Column<string>(nullable: true),
            //        DocNumber = table.Column<string>(nullable: true),
            //        ActionText = table.Column<string>(nullable: true),
            //        Status = table.Column<string>(nullable: true),
            //        Date = table.Column<DateTime>(nullable: true),
            //        Time = table.Column<string>(nullable: true),
            //        HasSeen = table.Column<bool>(nullable: false)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_BPC_OF_AI_ACT", x => new { x.Client, x.Company, x.Type, x.PatnerID, x.SeqNo });
            //    });

            //migrationBuilder.CreateTable(
            //    name: "BPC_OF_GRGI",
            //    columns: table => new
            //    {
            //        IsActive = table.Column<bool>(nullable: false),
            //        CreatedOn = table.Column<DateTime>(nullable: false),
            //        CreatedBy = table.Column<string>(nullable: true),
            //        ModifiedOn = table.Column<DateTime>(nullable: true),
            //        ModifiedBy = table.Column<string>(nullable: true),
            //        Client = table.Column<string>(maxLength: 3, nullable: false),
            //        Company = table.Column<string>(maxLength: 4, nullable: false),
            //        Type = table.Column<string>(maxLength: 1, nullable: false),
            //        PatnerID = table.Column<string>(maxLength: 12, nullable: false),
            //        DocNumber = table.Column<string>(maxLength: 10, nullable: false),
            //        GRGIDoc = table.Column<string>(maxLength: 10, nullable: false),
            //        Item = table.Column<string>(maxLength: 4, nullable: false),
            //        Description = table.Column<string>(nullable: true),
            //        Material = table.Column<string>(nullable: true),
            //        MaterialText = table.Column<string>(nullable: true),
            //        DeliveryDate = table.Column<DateTime>(nullable: true),
            //        GRIDate = table.Column<DateTime>(nullable: true),
            //        GRGIQty = table.Column<double>(nullable: true),
            //        ShippingPartner = table.Column<string>(nullable: true),
            //        ShippingDoc = table.Column<string>(nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_BPC_OF_GRGI", x => new { x.Client, x.Company, x.Type, x.PatnerID, x.DocNumber, x.GRGIDoc, x.Item });
            //    });

            //migrationBuilder.CreateTable(
            //    name: "BPC_OF_H",
            //    columns: table => new
            //    {
            //        IsActive = table.Column<bool>(nullable: false),
            //        CreatedOn = table.Column<DateTime>(nullable: false),
            //        CreatedBy = table.Column<string>(nullable: true),
            //        ModifiedOn = table.Column<DateTime>(nullable: true),
            //        ModifiedBy = table.Column<string>(nullable: true),
            //        Client = table.Column<string>(maxLength: 3, nullable: false),
            //        Company = table.Column<string>(maxLength: 4, nullable: false),
            //        Type = table.Column<string>(maxLength: 1, nullable: false),
            //        PatnerID = table.Column<string>(maxLength: 12, nullable: false),
            //        DocNumber = table.Column<string>(maxLength: 10, nullable: false),
            //        DocType = table.Column<string>(nullable: true),
            //        DocDate = table.Column<DateTime>(nullable: true),
            //        DocVersion = table.Column<string>(nullable: true),
            //        Currency = table.Column<string>(nullable: true),
            //        Status = table.Column<string>(nullable: true),
            //        CrossAmount = table.Column<double>(nullable: true),
            //        NetAmount = table.Column<double>(nullable: true),
            //        RefDoc = table.Column<string>(nullable: true),
            //        AckStatus = table.Column<string>(nullable: true),
            //        AckRemark = table.Column<string>(nullable: true),
            //        AckDate = table.Column<DateTime>(nullable: true),
            //        AckUser = table.Column<string>(nullable: true),
            //        PINNumber = table.Column<string>(nullable: true),
            //        PlantName = table.Column<string>(nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_BPC_OF_H", x => new { x.Client, x.Company, x.Type, x.PatnerID, x.DocNumber });
            //    });

            //migrationBuilder.CreateTable(
            //    name: "BPC_OF_I",
            //    columns: table => new
            //    {
            //        IsActive = table.Column<bool>(nullable: false),
            //        CreatedOn = table.Column<DateTime>(nullable: false),
            //        CreatedBy = table.Column<string>(nullable: true),
            //        ModifiedOn = table.Column<DateTime>(nullable: true),
            //        ModifiedBy = table.Column<string>(nullable: true),
            //        Client = table.Column<string>(maxLength: 3, nullable: false),
            //        Company = table.Column<string>(maxLength: 4, nullable: false),
            //        Type = table.Column<string>(maxLength: 1, nullable: false),
            //        PatnerID = table.Column<string>(maxLength: 12, nullable: false),
            //        DocNumber = table.Column<string>(maxLength: 10, nullable: false),
            //        Item = table.Column<string>(maxLength: 4, nullable: false),
            //        Material = table.Column<string>(nullable: true),
            //        MaterialText = table.Column<string>(nullable: true),
            //        DeliveryDate = table.Column<DateTime>(nullable: true),
            //        OrderedQty = table.Column<double>(nullable: true),
            //        CompletedQty = table.Column<double>(nullable: true),
            //        TransitQty = table.Column<double>(nullable: true),
            //        OpenQty = table.Column<double>(nullable: true),
            //        UOM = table.Column<string>(nullable: true),
            //        HSN = table.Column<string>(nullable: true),
            //        IsClosed = table.Column<bool>(nullable: false),
            //        AckStatus = table.Column<string>(nullable: true),
            //        AckDeliveryDate = table.Column<DateTime>(nullable: true),
            //        PlantCode = table.Column<string>(nullable: true),
            //        UnitPrice = table.Column<double>(nullable: true),
            //        Value = table.Column<double>(nullable: true),
            //        TaxAmount = table.Column<double>(nullable: true),
            //        TaxCode = table.Column<string>(nullable: true),
            //        Flag = table.Column<string>(maxLength: 12, nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_BPC_OF_I", x => new { x.Client, x.Company, x.Type, x.PatnerID, x.DocNumber, x.Item });
            //    });

            //migrationBuilder.CreateTable(
            //    name: "BPC_OF_I_SES",
            //    columns: table => new
            //    {
            //        IsActive = table.Column<bool>(nullable: false),
            //        CreatedOn = table.Column<DateTime>(nullable: false),
            //        CreatedBy = table.Column<string>(nullable: true),
            //        ModifiedOn = table.Column<DateTime>(nullable: true),
            //        ModifiedBy = table.Column<string>(nullable: true),
            //        Client = table.Column<string>(maxLength: 3, nullable: false),
            //        Company = table.Column<string>(maxLength: 4, nullable: false),
            //        Type = table.Column<string>(maxLength: 1, nullable: false),
            //        PatnerID = table.Column<string>(maxLength: 12, nullable: false),
            //        DocNumber = table.Column<string>(maxLength: 12, nullable: false),
            //        Item = table.Column<string>(maxLength: 4, nullable: false),
            //        ServiceNo = table.Column<string>(maxLength: 24, nullable: false),
            //        ServiceItem = table.Column<string>(nullable: true),
            //        OrderedQty = table.Column<double>(nullable: false),
            //        OpenQty = table.Column<double>(nullable: false),
            //        ServiceQty = table.Column<double>(nullable: false)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_BPC_OF_I_SES", x => new { x.Client, x.Company, x.Type, x.PatnerID, x.DocNumber, x.Item, x.ServiceNo });
            //    });

            //migrationBuilder.CreateTable(
            //    name: "BPC_OF_QM",
            //    columns: table => new
            //    {
            //        IsActive = table.Column<bool>(nullable: false),
            //        CreatedOn = table.Column<DateTime>(nullable: false),
            //        CreatedBy = table.Column<string>(nullable: true),
            //        ModifiedOn = table.Column<DateTime>(nullable: true),
            //        ModifiedBy = table.Column<string>(nullable: true),
            //        Client = table.Column<string>(maxLength: 3, nullable: false),
            //        Company = table.Column<string>(maxLength: 4, nullable: false),
            //        Type = table.Column<string>(maxLength: 1, nullable: false),
            //        PatnerID = table.Column<string>(maxLength: 12, nullable: false),
            //        DocNumber = table.Column<string>(maxLength: 12, nullable: false),
            //        Item = table.Column<string>(maxLength: 12, nullable: false),
            //        Material = table.Column<string>(nullable: true),
            //        MaterialText = table.Column<string>(nullable: true),
            //        GRGIQty = table.Column<double>(nullable: true),
            //        LotQty = table.Column<double>(nullable: true),
            //        RejQty = table.Column<double>(nullable: true),
            //        RejReason = table.Column<string>(nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_BPC_OF_QM", x => new { x.Client, x.Company, x.Type, x.PatnerID, x.DocNumber, x.Item });
            //    });

            //migrationBuilder.CreateTable(
            //    name: "BPC_OF_SL",
            //    columns: table => new
            //    {
            //        IsActive = table.Column<bool>(nullable: false),
            //        CreatedOn = table.Column<DateTime>(nullable: false),
            //        CreatedBy = table.Column<string>(nullable: true),
            //        ModifiedOn = table.Column<DateTime>(nullable: true),
            //        ModifiedBy = table.Column<string>(nullable: true),
            //        Client = table.Column<string>(maxLength: 3, nullable: false),
            //        Company = table.Column<string>(maxLength: 4, nullable: false),
            //        Type = table.Column<string>(maxLength: 1, nullable: false),
            //        PatnerID = table.Column<string>(maxLength: 12, nullable: false),
            //        DocNumber = table.Column<string>(maxLength: 10, nullable: false),
            //        Item = table.Column<string>(maxLength: 4, nullable: false),
            //        SlLine = table.Column<string>(maxLength: 4, nullable: false),
            //        DeliveryDate = table.Column<DateTime>(nullable: true),
            //        OrderedQty = table.Column<double>(nullable: true),
            //        OpenQty = table.Column<double>(nullable: true),
            //        AckStatus = table.Column<string>(nullable: true),
            //        AckDeliveryDate = table.Column<DateTime>(nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_BPC_OF_SL", x => new { x.Client, x.Company, x.Type, x.PatnerID, x.DocNumber, x.Item, x.SlLine });
            //    });

            //migrationBuilder.CreateTable(
            //    name: "BPC_OF_Subcon",
            //    columns: table => new
            //    {
            //        IsActive = table.Column<bool>(nullable: false),
            //        CreatedOn = table.Column<DateTime>(nullable: false),
            //        CreatedBy = table.Column<string>(nullable: true),
            //        ModifiedOn = table.Column<DateTime>(nullable: true),
            //        ModifiedBy = table.Column<string>(nullable: true),
            //        Client = table.Column<string>(maxLength: 3, nullable: false),
            //        Company = table.Column<string>(maxLength: 4, nullable: false),
            //        Type = table.Column<string>(maxLength: 1, nullable: false),
            //        PatnerID = table.Column<string>(maxLength: 12, nullable: false),
            //        DocNumber = table.Column<string>(maxLength: 10, nullable: false),
            //        Item = table.Column<string>(maxLength: 4, nullable: false),
            //        SlLine = table.Column<string>(maxLength: 4, nullable: false),
            //        Date = table.Column<DateTime>(nullable: true),
            //        OrderedQty = table.Column<double>(nullable: false),
            //        Batch = table.Column<string>(nullable: true),
            //        Remarks = table.Column<string>(nullable: true),
            //        Status = table.Column<string>(nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_BPC_OF_Subcon", x => new { x.Client, x.Company, x.Type, x.PatnerID, x.DocNumber, x.Item, x.SlLine });
            //    });

            migrationBuilder.CreateTable(
                name: "BPC_PAY_AS",
                columns: table => new
                {
                    Client = table.Column<string>(maxLength: 3, nullable: false),
                    Company = table.Column<string>(maxLength: 4, nullable: false),
                    Type = table.Column<string>(maxLength: 1, nullable: false),
                    PartnerID = table.Column<string>(maxLength: 12, nullable: false),
                    FiscalYear = table.Column<string>(maxLength: 4, nullable: false),
                    DocumentNumber = table.Column<string>(maxLength: 10, nullable: false),
                    DocumentDate = table.Column<DateTime>(nullable: true),
                    InvoiceNumber = table.Column<string>(nullable: true),
                    InvoiceDate = table.Column<DateTime>(nullable: true),
                    InvoiceAmount = table.Column<double>(nullable: false),
                    BalanceAmount = table.Column<double>(nullable: false),
                    PaidAmount = table.Column<double>(nullable: false),
                    DueDate = table.Column<DateTime>(nullable: true),
                    ProfitCenter = table.Column<string>(nullable: true),
                    Reference = table.Column<string>(nullable: true),
                    Status = table.Column<string>(nullable: true),
                    AcceptedOn = table.Column<DateTime>(nullable: true),
                    AcceptedBy = table.Column<string>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<string>(nullable: true),
                    ModifiedOn = table.Column<DateTime>(nullable: true),
                    ModifiedBy = table.Column<string>(nullable: true),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BPC_PAY_AS", x => new { x.Client, x.Company, x.Type, x.PartnerID, x.FiscalYear, x.DocumentNumber });
                });

            //migrationBuilder.CreateTable(
            //    name: "BPC_PAY_DIS",
            //    columns: table => new
            //    {
            //        IsActive = table.Column<bool>(nullable: false),
            //        CreatedOn = table.Column<DateTime>(nullable: false),
            //        CreatedBy = table.Column<string>(nullable: true),
            //        ModifiedOn = table.Column<DateTime>(nullable: true),
            //        ModifiedBy = table.Column<string>(nullable: true),
            //        Client = table.Column<string>(maxLength: 3, nullable: false),
            //        Company = table.Column<string>(maxLength: 4, nullable: false),
            //        Type = table.Column<string>(maxLength: 1, nullable: false),
            //        PartnerID = table.Column<string>(maxLength: 12, nullable: false),
            //        FiscalYear = table.Column<string>(maxLength: 4, nullable: false),
            //        DocumentNumber = table.Column<string>(maxLength: 10, nullable: false),
            //        DocumentDate = table.Column<DateTime>(nullable: true),
            //        InvoiceNumber = table.Column<string>(nullable: true),
            //        InvoiceDate = table.Column<DateTime>(nullable: true),
            //        InvoiceAmount = table.Column<double>(nullable: false),
            //        PaidAmount = table.Column<double>(nullable: false),
            //        BalanceAmount = table.Column<double>(nullable: false),
            //        DueDate = table.Column<DateTime>(nullable: true),
            //        ProposedDueDate = table.Column<DateTime>(nullable: true),
            //        ProposedDiscount = table.Column<double>(nullable: false),
            //        PostDiscountAmount = table.Column<double>(nullable: false),
            //        ProfitCenter = table.Column<string>(nullable: true),
            //        Status = table.Column<string>(nullable: true),
            //        ApprovedOn = table.Column<DateTime>(nullable: true),
            //        ApprovedBy = table.Column<string>(nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_BPC_PAY_DIS", x => new { x.Client, x.Company, x.Type, x.PartnerID, x.FiscalYear, x.DocumentNumber });
            //    });

            //migrationBuilder.CreateTable(
            //    name: "BPC_PAY_DIS_MASTER",
            //    columns: table => new
            //    {
            //        IsActive = table.Column<bool>(nullable: false),
            //        CreatedOn = table.Column<DateTime>(nullable: false),
            //        CreatedBy = table.Column<string>(nullable: true),
            //        ModifiedOn = table.Column<DateTime>(nullable: true),
            //        ModifiedBy = table.Column<string>(nullable: true),
            //        ID = table.Column<int>(nullable: false)
            //            .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
            //        FiscalYear = table.Column<string>(maxLength: 4, nullable: true),
            //        Amount = table.Column<double>(nullable: false),
            //        Days = table.Column<int>(nullable: false),
            //        Discount = table.Column<double>(nullable: false),
            //        ProfitCenter = table.Column<string>(nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_BPC_PAY_DIS_MASTER", x => x.ID);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "BPC_PAY_PAYABLE",
            //    columns: table => new
            //    {
            //        IsActive = table.Column<bool>(nullable: false),
            //        CreatedOn = table.Column<DateTime>(nullable: false),
            //        CreatedBy = table.Column<string>(nullable: true),
            //        ModifiedOn = table.Column<DateTime>(nullable: true),
            //        ModifiedBy = table.Column<string>(nullable: true),
            //        Client = table.Column<string>(maxLength: 3, nullable: false),
            //        Company = table.Column<string>(maxLength: 4, nullable: false),
            //        Type = table.Column<string>(maxLength: 1, nullable: false),
            //        PartnerID = table.Column<string>(maxLength: 12, nullable: false),
            //        FiscalYear = table.Column<string>(maxLength: 4, nullable: false),
            //        Invoice = table.Column<string>(maxLength: 16, nullable: false),
            //        InvoiceDate = table.Column<DateTime>(nullable: true),
            //        PostedOn = table.Column<DateTime>(nullable: true),
            //        DueDate = table.Column<DateTime>(nullable: true),
            //        AdvAmount = table.Column<double>(nullable: false),
            //        Amount = table.Column<double>(nullable: false),
            //        Balance = table.Column<double>(nullable: false),
            //        Currency = table.Column<string>(nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_BPC_PAY_PAYABLE", x => new { x.Client, x.Company, x.Type, x.PartnerID, x.FiscalYear, x.Invoice });
            //    });

            //migrationBuilder.CreateTable(
            //    name: "BPC_PAY_PAYMENT",
            //    columns: table => new
            //    {
            //        IsActive = table.Column<bool>(nullable: false),
            //        CreatedOn = table.Column<DateTime>(nullable: false),
            //        CreatedBy = table.Column<string>(nullable: true),
            //        ModifiedOn = table.Column<DateTime>(nullable: true),
            //        ModifiedBy = table.Column<string>(nullable: true),
            //        Client = table.Column<string>(maxLength: 3, nullable: false),
            //        Company = table.Column<string>(maxLength: 4, nullable: false),
            //        Type = table.Column<string>(maxLength: 1, nullable: false),
            //        PartnerID = table.Column<string>(maxLength: 12, nullable: false),
            //        FiscalYear = table.Column<string>(maxLength: 4, nullable: false),
            //        DocumentNumber = table.Column<string>(maxLength: 10, nullable: false),
            //        PaymentDate = table.Column<DateTime>(nullable: true),
            //        PaymentType = table.Column<string>(nullable: true),
            //        PaidAmount = table.Column<double>(nullable: false),
            //        BankName = table.Column<string>(nullable: true),
            //        BankAccount = table.Column<string>(nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_BPC_PAY_PAYMENT", x => new { x.Client, x.Company, x.Type, x.PartnerID, x.FiscalYear, x.DocumentNumber });
            //    });

            //migrationBuilder.CreateTable(
            //    name: "BPC_PAY_TDS",
            //    columns: table => new
            //    {
            //        IsActive = table.Column<bool>(nullable: false),
            //        CreatedOn = table.Column<DateTime>(nullable: false),
            //        CreatedBy = table.Column<string>(nullable: true),
            //        ModifiedOn = table.Column<DateTime>(nullable: true),
            //        ModifiedBy = table.Column<string>(nullable: true),
            //        Client = table.Column<string>(maxLength: 3, nullable: false),
            //        Company = table.Column<string>(maxLength: 4, nullable: false),
            //        Type = table.Column<string>(maxLength: 1, nullable: false),
            //        PartnerID = table.Column<string>(maxLength: 12, nullable: false),
            //        FiscalYear = table.Column<string>(maxLength: 4, nullable: false),
            //        CompanyCode = table.Column<string>(nullable: false),
            //        DocumentID = table.Column<string>(nullable: false),
            //        PostingDate = table.Column<DateTime>(nullable: true),
            //        BaseAmount = table.Column<double>(nullable: false),
            //        TDSCategory = table.Column<string>(nullable: true),
            //        TDSAmount = table.Column<double>(nullable: false),
            //        Currency = table.Column<string>(nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_BPC_PAY_TDS", x => new { x.Client, x.Company, x.Type, x.PartnerID, x.FiscalYear, x.CompanyCode, x.DocumentID });
            //    });

            //migrationBuilder.CreateTable(
            //    name: "BPC_PI_H",
            //    columns: table => new
            //    {
            //        IsActive = table.Column<bool>(nullable: false),
            //        CreatedOn = table.Column<DateTime>(nullable: false),
            //        CreatedBy = table.Column<string>(nullable: true),
            //        ModifiedOn = table.Column<DateTime>(nullable: true),
            //        ModifiedBy = table.Column<string>(nullable: true),
            //        Client = table.Column<string>(maxLength: 3, nullable: false),
            //        Company = table.Column<string>(maxLength: 4, nullable: false),
            //        Type = table.Column<string>(maxLength: 1, nullable: false),
            //        PatnerID = table.Column<string>(maxLength: 12, nullable: false),
            //        PIRNumber = table.Column<string>(maxLength: 12, nullable: false),
            //        PIRType = table.Column<string>(nullable: true),
            //        DocumentNumber = table.Column<string>(nullable: true),
            //        Text = table.Column<string>(nullable: true),
            //        Date = table.Column<DateTime>(nullable: true),
            //        ReferenceDoc = table.Column<string>(nullable: true),
            //        GrossAmount = table.Column<double>(nullable: true),
            //        NetAmount = table.Column<double>(nullable: true),
            //        Currency = table.Column<string>(nullable: true),
            //        Status = table.Column<string>(nullable: true),
            //        UOM = table.Column<string>(nullable: true),
            //        Material = table.Column<string>(nullable: true),
            //        Description = table.Column<string>(nullable: true),
            //        Qty = table.Column<double>(nullable: false),
            //        Reason = table.Column<string>(nullable: true),
            //        DeliveryNote = table.Column<string>(nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_BPC_PI_H", x => new { x.Client, x.Company, x.Type, x.PatnerID, x.PIRNumber });
            //    });

            //migrationBuilder.CreateTable(
            //    name: "BPC_PI_I",
            //    columns: table => new
            //    {
            //        IsActive = table.Column<bool>(nullable: false),
            //        CreatedOn = table.Column<DateTime>(nullable: false),
            //        CreatedBy = table.Column<string>(nullable: true),
            //        ModifiedOn = table.Column<DateTime>(nullable: true),
            //        ModifiedBy = table.Column<string>(nullable: true),
            //        Client = table.Column<string>(maxLength: 3, nullable: false),
            //        Company = table.Column<string>(maxLength: 4, nullable: false),
            //        Type = table.Column<string>(maxLength: 1, nullable: false),
            //        PatnerID = table.Column<string>(maxLength: 12, nullable: false),
            //        PIRNumber = table.Column<string>(maxLength: 12, nullable: false),
            //        Item = table.Column<string>(maxLength: 4, nullable: false),
            //        ProdcutID = table.Column<string>(nullable: true),
            //        MaterialText = table.Column<string>(nullable: true),
            //        DeliveryDate = table.Column<DateTime>(nullable: true),
            //        OrderQty = table.Column<double>(nullable: true),
            //        UOM = table.Column<string>(nullable: true),
            //        HSN = table.Column<string>(nullable: true),
            //        RetQty = table.Column<double>(nullable: true),
            //        ReasonText = table.Column<string>(nullable: true),
            //        FileName = table.Column<string>(nullable: true),
            //        AttachmentReferenceNo = table.Column<string>(nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_BPC_PI_I", x => new { x.Client, x.Company, x.Type, x.PatnerID, x.PIRNumber, x.Item });
            //    });

            //migrationBuilder.CreateTable(
            //    name: "BPC_Plant_Master",
            //    columns: table => new
            //    {
            //        IsActive = table.Column<bool>(nullable: false),
            //        CreatedOn = table.Column<DateTime>(nullable: false),
            //        CreatedBy = table.Column<string>(nullable: true),
            //        ModifiedOn = table.Column<DateTime>(nullable: true),
            //        ModifiedBy = table.Column<string>(nullable: true),
            //        PlantCode = table.Column<string>(nullable: false),
            //        PlantText = table.Column<string>(nullable: true),
            //        AddressLine1 = table.Column<string>(nullable: true),
            //        AddressLine2 = table.Column<string>(nullable: true),
            //        City = table.Column<string>(nullable: true),
            //        State = table.Column<string>(nullable: true),
            //        Country = table.Column<string>(nullable: true),
            //        PinCode = table.Column<string>(nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_BPC_Plant_Master", x => x.PlantCode);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "BPC_POD_H",
            //    columns: table => new
            //    {
            //        IsActive = table.Column<bool>(nullable: false),
            //        CreatedOn = table.Column<DateTime>(nullable: false),
            //        CreatedBy = table.Column<string>(nullable: true),
            //        ModifiedOn = table.Column<DateTime>(nullable: true),
            //        ModifiedBy = table.Column<string>(nullable: true),
            //        Client = table.Column<string>(maxLength: 3, nullable: false),
            //        Company = table.Column<string>(maxLength: 4, nullable: false),
            //        Type = table.Column<string>(maxLength: 1, nullable: false),
            //        PatnerID = table.Column<string>(maxLength: 12, nullable: false),
            //        DocNumber = table.Column<string>(nullable: true),
            //        InvoiceNumber = table.Column<string>(nullable: false),
            //        InvoiceDate = table.Column<DateTime>(nullable: true),
            //        TruckNumber = table.Column<string>(nullable: true),
            //        VessleNumber = table.Column<string>(nullable: true),
            //        Amount = table.Column<double>(nullable: true),
            //        Currency = table.Column<string>(nullable: true),
            //        Transporter = table.Column<string>(nullable: true),
            //        Status = table.Column<string>(nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_BPC_POD_H", x => new { x.Client, x.Company, x.Type, x.PatnerID, x.InvoiceNumber });
            //    });

            //migrationBuilder.CreateTable(
            //    name: "BPC_POD_I",
            //    columns: table => new
            //    {
            //        IsActive = table.Column<bool>(nullable: false),
            //        CreatedOn = table.Column<DateTime>(nullable: false),
            //        CreatedBy = table.Column<string>(nullable: true),
            //        ModifiedOn = table.Column<DateTime>(nullable: true),
            //        ModifiedBy = table.Column<string>(nullable: true),
            //        Client = table.Column<string>(maxLength: 3, nullable: false),
            //        Company = table.Column<string>(maxLength: 4, nullable: false),
            //        Type = table.Column<string>(maxLength: 1, nullable: false),
            //        PatnerID = table.Column<string>(maxLength: 12, nullable: false),
            //        DocNumber = table.Column<string>(nullable: true),
            //        InvoiceNumber = table.Column<string>(nullable: false),
            //        Item = table.Column<string>(nullable: false),
            //        Material = table.Column<string>(nullable: true),
            //        MaterialText = table.Column<string>(nullable: true),
            //        Qty = table.Column<double>(nullable: false),
            //        ReceivedQty = table.Column<double>(nullable: true),
            //        BreakageQty = table.Column<double>(nullable: true),
            //        MissingQty = table.Column<double>(nullable: true),
            //        AcceptedQty = table.Column<double>(nullable: true),
            //        Reason = table.Column<string>(nullable: true),
            //        Remarks = table.Column<string>(nullable: true),
            //        AttachmentName = table.Column<string>(nullable: true),
            //        AttachmentReferenceNo = table.Column<string>(nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_BPC_POD_I", x => new { x.Client, x.Company, x.Type, x.PatnerID, x.InvoiceNumber, x.Item });
            //    });

            //migrationBuilder.CreateTable(
            //    name: "BPC_Prod",
            //    columns: table => new
            //    {
            //        IsActive = table.Column<bool>(nullable: false),
            //        CreatedOn = table.Column<DateTime>(nullable: false),
            //        CreatedBy = table.Column<string>(nullable: true),
            //        ModifiedOn = table.Column<DateTime>(nullable: true),
            //        ModifiedBy = table.Column<string>(nullable: true),
            //        Client = table.Column<string>(maxLength: 3, nullable: false),
            //        Company = table.Column<string>(maxLength: 4, nullable: false),
            //        Type = table.Column<string>(maxLength: 1, nullable: false),
            //        ProductID = table.Column<string>(maxLength: 12, nullable: false),
            //        Text = table.Column<string>(nullable: true),
            //        AttID = table.Column<string>(nullable: true),
            //        UOM = table.Column<string>(nullable: true),
            //        Stock = table.Column<string>(nullable: true),
            //        StockUpdatedOn = table.Column<DateTime>(nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_BPC_Prod", x => new { x.Client, x.Company, x.Type, x.ProductID });
            //    });

            //migrationBuilder.CreateTable(
            //    name: "BPC_Reason_Master",
            //    columns: table => new
            //    {
            //        IsActive = table.Column<bool>(nullable: false),
            //        CreatedOn = table.Column<DateTime>(nullable: false),
            //        CreatedBy = table.Column<string>(nullable: true),
            //        ModifiedOn = table.Column<DateTime>(nullable: true),
            //        ModifiedBy = table.Column<string>(nullable: true),
            //        ID = table.Column<int>(nullable: false)
            //            .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
            //        ReasonCode = table.Column<string>(nullable: true),
            //        ReasonText = table.Column<string>(nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_BPC_Reason_Master", x => x.ID);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "BPC_SCOC_Message",
            //    columns: table => new
            //    {
            //        IsActive = table.Column<bool>(nullable: false),
            //        CreatedOn = table.Column<DateTime>(nullable: false),
            //        CreatedBy = table.Column<string>(nullable: true),
            //        ModifiedOn = table.Column<DateTime>(nullable: true),
            //        ModifiedBy = table.Column<string>(nullable: true),
            //        MessageID = table.Column<int>(nullable: false)
            //            .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
            //        SCOCMessage = table.Column<string>(nullable: true),
            //        IsReleased = table.Column<bool>(nullable: false)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_BPC_SCOC_Message", x => x.MessageID);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "BPCAttachments",
            //    columns: table => new
            //    {
            //        IsActive = table.Column<bool>(nullable: false),
            //        CreatedOn = table.Column<DateTime>(nullable: false),
            //        CreatedBy = table.Column<string>(nullable: true),
            //        ModifiedOn = table.Column<DateTime>(nullable: true),
            //        ModifiedBy = table.Column<string>(nullable: true),
            //        AttachmentID = table.Column<int>(nullable: false)
            //            .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
            //        Client = table.Column<string>(nullable: true),
            //        Company = table.Column<string>(nullable: true),
            //        Type = table.Column<string>(nullable: true),
            //        PatnerID = table.Column<string>(nullable: true),
            //        ReferenceNo = table.Column<string>(nullable: true),
            //        AttachmentName = table.Column<string>(nullable: true),
            //        ContentType = table.Column<string>(nullable: true),
            //        ContentLength = table.Column<long>(nullable: false),
            //        AttachmentFile = table.Column<byte[]>(nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_BPCAttachments", x => x.AttachmentID);
            //    });

            //migrationBuilder.CreateIndex(
            //    name: "IX_BPC_ASN_Field_Master_Field",
            //    table: "BPC_ASN_Field_Master",
            //    column: "Field");

            //migrationBuilder.CreateIndex(
            //    name: "IX_BPC_PAY_DIS_ProfitCenter",
            //    table: "BPC_PAY_DIS",
            //    column: "ProfitCenter");

            //migrationBuilder.CreateIndex(
            //    name: "IX_BPC_PAY_DIS_MASTER_FiscalYear_ProfitCenter",
            //    table: "BPC_PAY_DIS_MASTER",
            //    columns: new[] { "FiscalYear", "ProfitCenter" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BP_BC_H");

            migrationBuilder.DropTable(
                name: "BP_BC_I");

            migrationBuilder.DropTable(
                name: "BPC_ASN_Field_Master");

            migrationBuilder.DropTable(
                name: "BPC_ASN_H");

            migrationBuilder.DropTable(
                name: "BPC_ASN_I");

            migrationBuilder.DropTable(
                name: "BPC_ASN_I_Batch");

            migrationBuilder.DropTable(
                name: "BPC_ASN_I_SES");

            migrationBuilder.DropTable(
                name: "BPC_ASN_Pack");

            migrationBuilder.DropTable(
                name: "BPC_CEO_Message");

            migrationBuilder.DropTable(
                name: "BPC_Country_Master");

            migrationBuilder.DropTable(
                name: "BPC_Currency_Master");

            migrationBuilder.DropTable(
                name: "BPC_DocumentCenter");

            migrationBuilder.DropTable(
                name: "BPC_DocumentCenter_Master");

            migrationBuilder.DropTable(
                name: "BPC_ExpenseType_Master");

            migrationBuilder.DropTable(
                name: "BPC_FLIP_Attachment");

            migrationBuilder.DropTable(
                name: "BPC_FLIP_Cost");

            migrationBuilder.DropTable(
                name: "BPC_FLIP_H");

            migrationBuilder.DropTable(
                name: "BPC_FLIP_I");

            migrationBuilder.DropTable(
                name: "BPC_Gate_HV");

            migrationBuilder.DropTable(
                name: "BPC_Gate_TA");

            migrationBuilder.DropTable(
                name: "BPC_INV");

            migrationBuilder.DropTable(
                name: "BPC_INV_I");

            migrationBuilder.DropTable(
                name: "BPC_OF_AI_ACT");

            migrationBuilder.DropTable(
                name: "BPC_OF_GRGI");

            migrationBuilder.DropTable(
                name: "BPC_OF_H");

            migrationBuilder.DropTable(
                name: "BPC_OF_I");

            migrationBuilder.DropTable(
                name: "BPC_OF_I_SES");

            migrationBuilder.DropTable(
                name: "BPC_OF_QM");

            migrationBuilder.DropTable(
                name: "BPC_OF_SL");

            migrationBuilder.DropTable(
                name: "BPC_OF_Subcon");

            migrationBuilder.DropTable(
                name: "BPC_PAY_AS");

            migrationBuilder.DropTable(
                name: "BPC_PAY_DIS");

            migrationBuilder.DropTable(
                name: "BPC_PAY_DIS_MASTER");

            migrationBuilder.DropTable(
                name: "BPC_PAY_PAYABLE");

            migrationBuilder.DropTable(
                name: "BPC_PAY_PAYMENT");

            migrationBuilder.DropTable(
                name: "BPC_PAY_TDS");

            migrationBuilder.DropTable(
                name: "BPC_PI_H");

            migrationBuilder.DropTable(
                name: "BPC_PI_I");

            migrationBuilder.DropTable(
                name: "BPC_Plant_Master");

            migrationBuilder.DropTable(
                name: "BPC_POD_H");

            migrationBuilder.DropTable(
                name: "BPC_POD_I");

            migrationBuilder.DropTable(
                name: "BPC_Prod");

            migrationBuilder.DropTable(
                name: "BPC_Reason_Master");

            migrationBuilder.DropTable(
                name: "BPC_SCOC_Message");

            migrationBuilder.DropTable(
                name: "BPCAttachments");
        }
    }
}
