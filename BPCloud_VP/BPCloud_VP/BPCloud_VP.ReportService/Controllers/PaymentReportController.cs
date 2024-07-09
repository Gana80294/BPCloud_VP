using BPCloud_VP.ReportService.Models;
using BPCloud_VP.ReportService.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace BPCloud_VP.ReportService.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class PaymentReportController : ControllerBase
    {
        private readonly IPaymentReportRepository _paymentReportRepository;

        public PaymentReportController(IPaymentReportRepository paymentReportRepository)
        {
            _paymentReportRepository = paymentReportRepository;
        }

        [HttpGet]
        public List<BPCPayment> GetAllPayments()
        {
            try
            {
                var data = _paymentReportRepository.GetAllPayments();
                return data;
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("Payment/GetAllPayments", ex);
                return null;
            }
        }

        [HttpGet]
        public List<BPCPayment> GetFilteredPayments(DateTime? FromDate = null, DateTime? ToDate = null)
        {
            try
            {
                var data = _paymentReportRepository.GetPaymentReport(FromDate, ToDate);
                return data;
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("Payment/GetBPCPayments", ex);
                return null;
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreatePayments([FromBody] List<BPCPaymentXLSX> Payments)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                await _paymentReportRepository.CreatePayments(Payments);
                return Ok();
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("PaymentReport/CreatePayments", ex);
                return BadRequest(ex.Message);
            }
        }
    }
}
