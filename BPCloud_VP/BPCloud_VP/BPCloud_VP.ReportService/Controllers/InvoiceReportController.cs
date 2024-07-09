using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using BPCloud_VP.ReportService.Models;
using BPCloud_VP.ReportService.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BPCloud_VP.ReportService.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class InvoiceReportController : ControllerBase
    {
        private readonly IInvoiceReportRepository _InvoiceReportRepository;

        public InvoiceReportController(IInvoiceReportRepository InvoiceReportRepository)
        {
            _InvoiceReportRepository = InvoiceReportRepository;
        }

        [HttpGet]
        public List<BPCInvoice> GetAllInvoices()
        {
            try
            {
                var AllInvoices = _InvoiceReportRepository.GetAllInvoices();
                return AllInvoices;
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("ASN/GetAllInvoices", ex);
                return null;
            }
        }

        [HttpGet]
        public List<BPCInvoice> GetAllInvoicesByPartnerID(string PartnerID)
        {
            try
            {
                var AllInvoices = _InvoiceReportRepository.GetAllInvoicesByPartnerID(PartnerID);
                return AllInvoices;
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("ASN/GetAllInvoicesByPartnerID", ex);
                return null;
            }
        }

        [HttpGet]
        public List<BPCInvoice> GetFilteredInvoices(string InvoiceNo = null, string PoReference = null, DateTime? FromDate = null, DateTime? ToDate = null, string Status = null)
        {
            try
            {
                var AllInvoices = _InvoiceReportRepository.GetFilteredInvoices(InvoiceNo, PoReference, FromDate, ToDate, Status);
                return AllInvoices;
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("ASN/GetFilteredInvoices", ex);
                return null;
            }
        }
        [HttpGet]
        public List<BPCInvoice> GetFilteredInvoicesByPartnerID(string PartnerID, string InvoiceNo = null, string PoReference = null, DateTime? FromDate = null, DateTime? ToDate = null, string Status = null)
        {
            try
            {
                var AllInvoices = _InvoiceReportRepository.GetFilteredInvoicesByPartnerID(PartnerID, InvoiceNo, PoReference, FromDate, ToDate, Status);
                return AllInvoices;
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("ASN/GetFilteredInvoicesByPartnerID", ex);
                return null;
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateInvoices([FromBody] List<BPCInvoiceXLSX> Invoices)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                await _InvoiceReportRepository.CreateInvoices(Invoices);
                return Ok();
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("InvoiceReport/CreateInvoices", ex);
                return BadRequest(ex.Message);
            }
        }

    }
}