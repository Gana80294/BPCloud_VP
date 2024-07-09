using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using BPCloud_VP.ReportService.DBContexts;
using BPCloud_VP.ReportService.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace BPCloud_VP.ReportService.Repositories
{
    public class InvoiceReportRepository : IInvoiceReportRepository
    {
        private readonly ReportContext _dbContext;

        public InvoiceReportRepository(ReportContext dbContext)
        {
            _dbContext = dbContext;
        }


        public List<BPCInvoice> GetAllInvoices()
        {
            try
            {
                return _dbContext.BPCInvoices.ToList();
            }
            catch (SqlException ex) { WriteLog.WriteToFile("InvoiceRepository/GetAllInvoices", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("InvoiceRepository/GetAllInvoices", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<BPCInvoice> GetAllInvoicesByPartnerID(string PartnerID)
        {
            try
            {
                return _dbContext.BPCInvoices.Where(x => x.PatnerID == PartnerID).ToList();
            }
            catch (SqlException ex) { WriteLog.WriteToFile("InvoiceRepository/GetAllInvoicesByPartnerID", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("InvoiceRepository/GetAllInvoicesByPartnerID", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<BPCInvoice> GetFilteredInvoices(string InvoiceNo = null, string PoReference = null, DateTime? FromDate = null, DateTime? ToDate = null, string Status = null)
        {
            try
            {
                bool IsInvoiceNo = !string.IsNullOrEmpty(InvoiceNo);
                bool IsPoReference = !string.IsNullOrEmpty(PoReference);
                bool IsFromDate = FromDate.HasValue;
                bool IsToDate = ToDate.HasValue;
                bool IsStatus = !string.IsNullOrEmpty(Status);
                if (IsStatus)
                {
                    IsStatus = Status.ToLower() != "all";
                }
                var result = (from tb in _dbContext.BPCInvoices
                              where tb.IsActive && (!IsInvoiceNo || tb.InvoiceNo == InvoiceNo) && (!IsPoReference || tb.PoReference == PoReference)
                              && (!IsFromDate || (tb.InvoiceDate.HasValue && tb.InvoiceDate.Value.Date >= FromDate.Value.Date))
                              && (!IsToDate || (tb.InvoiceDate.HasValue && tb.InvoiceDate.Value.Date <= ToDate.Value.Date)) && (!IsStatus || tb.Status == Status)
                              select tb).ToList();
                return result;
            }
            catch (SqlException ex) { WriteLog.WriteToFile("InvoiceRepository/GetFilteredInvoices", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("InvoiceRepository/GetFilteredInvoices", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<BPCInvoice> GetFilteredInvoicesByPartnerID(string PartnerID, string InvoiceNo = null, string PoReference = null, DateTime? FromDate = null, DateTime? ToDate = null, string Status = null)
        {
            try
            {
                bool IsInvoiceNo = !string.IsNullOrEmpty(InvoiceNo);
                bool IsPoReference = !string.IsNullOrEmpty(PoReference);
                bool IsFromDate = FromDate.HasValue;
                bool IsToDate = ToDate.HasValue;
                bool IsStatus = !string.IsNullOrEmpty(Status);
                if (IsStatus)
                {
                    IsStatus = Status.ToLower() != "all";
                }
                var result = (from tb in _dbContext.BPCInvoices
                              where tb.PatnerID == PartnerID && tb.IsActive && (!IsInvoiceNo || tb.InvoiceNo == InvoiceNo) && (!IsPoReference || tb.PoReference == PoReference)
                              && (!IsFromDate || (tb.InvoiceDate.HasValue && tb.InvoiceDate.Value.Date >= FromDate.Value.Date))
                              && (!IsToDate || (tb.InvoiceDate.HasValue && tb.InvoiceDate.Value.Date <= ToDate.Value.Date)) && (!IsStatus || tb.Status == Status)
                              select tb).ToList();
                return result;
            }
            catch (SqlException ex) { WriteLog.WriteToFile("InvoiceRepository/GetFilteredInvoicesByPartnerID", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("InvoiceRepository/GetFilteredInvoicesByPartnerID", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task CreateInvoices(List<BPCInvoiceXLSX> Invoices)
        {
            try
            {
                if (Invoices != null && Invoices.Count > 0)
                {
                    foreach (BPCInvoiceXLSX Invoice in Invoices)
                    {
                        //await DeleteInvoiceByPartnerIDAndType(Invoice.PatnerID);
                        BPCInvoice bPCInvoice = new BPCInvoice();
                        bPCInvoice.Client = Invoice.Client;
                        bPCInvoice.PatnerID = Invoice.Patnerid;
                        bPCInvoice.Company = Invoice.Company;
                        bPCInvoice.Type = Invoice.Type;
                        bPCInvoice.FiscalYear = Invoice.Fiscalyear;
                        bPCInvoice.InvoiceNo = Invoice.Invno;
                        bPCInvoice.PaidAmount = Invoice.Paidamt;
                        bPCInvoice.Currency = Invoice.Currency;
                        DateTime dateTime;
                        CultureInfo provider = CultureInfo.InvariantCulture;
                        bool isSuccess = DateTime.TryParseExact(Invoice.Dateofpayment, "yyyyMMdd", provider, DateTimeStyles.None, out dateTime);
                        if (isSuccess)
                            bPCInvoice.DateofPayment = dateTime;
                        DateTime dateTime1;
                        CultureInfo provider1 = CultureInfo.InvariantCulture;
                        bool isSuccess1 = DateTime.TryParseExact(Invoice.Poddate, "yyyyMMdd", provider1, DateTimeStyles.None, out dateTime1);
                        if (isSuccess1)
                            bPCInvoice.PODDate = dateTime1;
                        DateTime dateTime2;
                        CultureInfo provider2 = CultureInfo.InvariantCulture;
                        bool isSuccess2 = DateTime.TryParseExact(Invoice.Invoicedate, "yyyyMMdd", provider, DateTimeStyles.None, out dateTime2);
                        if (isSuccess)
                            bPCInvoice.InvoiceDate = dateTime2;
                        if (!string.IsNullOrEmpty(Invoice.Invoiceamount))
                            bPCInvoice.InvoiceAmount = Convert.ToDouble(Invoice.Invoiceamount);
                        bPCInvoice.PODConfirmedBy = Invoice.Podconfirmedby;
                        bPCInvoice.PoReference = Invoice.POreference;
                        bPCInvoice.Status = Invoice.Status;
                        bPCInvoice.IsActive = true;
                        bPCInvoice.CreatedOn = DateTime.Now;
                        var result = _dbContext.BPCInvoices.Add(bPCInvoice);
                    }
                    await _dbContext.SaveChangesAsync();
                }
            }
            catch (SqlException ex) { WriteLog.WriteToFile("InvoiceRepository/CreateInvoices", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("InvoiceRepository/CreateInvoices", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
