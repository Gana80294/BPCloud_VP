using BPCloud_VP.ReportService.DBContexts;
using BPCloud_VP.ReportService.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace BPCloud_VP.ReportService.Repositories
{
    public class PaymentReportRepository : IPaymentReportRepository
    {
        private readonly ReportContext _dbContext;

        public PaymentReportRepository(ReportContext context, IConfiguration configuration)
        {
            _dbContext = context;
        }

        public List<BPCPayment> GetAllPayments()
        {
            try
            {
                var data = _dbContext.BPCPayments.ToList();
                return data;
            }
            catch (SqlException ex) { WriteLog.WriteToFile("PaymentReportRepository/GetAllPayments", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("PaymentReportRepository/GetAllPayments", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("PaymentRepository/GetAllPayments : - ", ex);
                throw ex;
            }
        }

        public List<BPCPayment> GetPaymentReport(DateTime? FromDate = null, DateTime? ToDate = null)
        {
            try
            {
                bool IsFromDate = FromDate.HasValue;
                bool IsToDate = ToDate.HasValue;
                var result = (from tb in _dbContext.BPCPayments
                              where tb.IsActive && (!IsFromDate || (tb.Date.HasValue && tb.Date.Value.Date >= FromDate.Value.Date))
                              && (!IsToDate || (tb.Date.HasValue && tb.Date.Value.Date <= ToDate.Value.Date))
                              select tb).ToList();
                return result;
            }
            catch (SqlException ex) { WriteLog.WriteToFile("PaymentReportRepository/GetPaymentReport", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("PaymentReportRepository/GetPaymentReport", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("PaymentRepository/GetPaymentReport : - ", ex);
                throw ex;
            }
        }

        public async Task CreatePayments(List<BPCPaymentXLSX> Payments)
        {
            try
            {
                if (Payments != null && Payments.Count > 0)
                {
                    foreach (BPCPaymentXLSX Payment in Payments)
                    {

                        BPCPayment bPCPayment = new BPCPayment();
                        //mandatory fields start
                        bPCPayment.Client = Payment.Client;
                        bPCPayment.Company = Payment.Company;
                        bPCPayment.PatnerID = Payment.Patnerid;
                        bPCPayment.Type = Payment.Type;
                        bPCPayment.FiscalYear = Payment.Fiscalyear;
                        bPCPayment.PaymentDoc = Payment.Paydoc;
                        //mandatory fields end
                        bPCPayment.Amount = Payment.Amount;
                        bPCPayment.Currency = Payment.Currency;
                        bPCPayment.Remark = Payment.Remark;
                        bPCPayment.Attachment = Payment.Attachment;
                        DateTime dateTime;
                        CultureInfo provider = CultureInfo.InvariantCulture;
                        bool isSuccess = DateTime.TryParseExact(Payment.Date, "yyyyMMdd", provider, DateTimeStyles.None, out dateTime);
                        if (isSuccess)
                            bPCPayment.Date = dateTime;
                        bPCPayment.IsActive = true;
                        bPCPayment.CreatedOn = DateTime.Now;
                        var result = _dbContext.BPCPayments.Add(bPCPayment);
                    }
                    await _dbContext.SaveChangesAsync();
                }
            }
            catch (SqlException ex) { WriteLog.WriteToFile("PaymentReportRepository/CreatePayments", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("PaymentReportRepository/CreatePayments", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
