using BPCloud_VP.ReportService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BPCloud_VP.ReportService.Repositories
{
    public interface IInvoiceReportRepository
    {
        List<BPCInvoice> GetAllInvoices();
        List<BPCInvoice> GetAllInvoicesByPartnerID(string PartnerID);
        List<BPCInvoice> GetFilteredInvoices(string InvoiceNo = null, string PoReference = null, DateTime? FromDate = null, DateTime? ToDate = null, string Status = null);
        List<BPCInvoice> GetFilteredInvoicesByPartnerID(string PartnerID, string InvoiceNo = null, string PoReference = null, DateTime? FromDate = null, DateTime? ToDate = null, string Status = null);
        Task CreateInvoices(List<BPCInvoiceXLSX> Invoices);
    }
}
