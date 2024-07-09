using BPCloud_VP.ReportService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BPCloud_VP.ReportService.Repositories
{
    public interface IPaymentReportRepository
    {
        List<BPCPayment> GetAllPayments();
        List<BPCPayment> GetPaymentReport(DateTime? FromDate = null, DateTime? ToDate = null);
        Task CreatePayments(List<BPCPaymentXLSX> Payments);
    }
}
