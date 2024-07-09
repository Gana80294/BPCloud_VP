using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using BPCloud_VP_POService.Models;
using BPCloud_VP_POService.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BPCloud_VP_POService.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class InvoiceController : ControllerBase
    {
        private readonly IInvoiceRepository _InvoiceRepository;

        public InvoiceController(IInvoiceRepository bankRepository)
        {
            _InvoiceRepository = bankRepository;
        }

        [HttpGet]
        public List<BPCInvoice> GetAllInvoices()
        {
            try
            {
                var result = _InvoiceRepository.GetAllInvoices();
                return result;
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("Fact/GetAllInvoices", ex);
                return null;
            }
        }
        //
        [HttpGet]
        public List<BPCInvoice> GetInvoiceByPartnerIdAnDocumentNo(string PatnerID)
        {
            try
            {
                var result = _InvoiceRepository.GetInvoiceByPartnerIdAnDocumentNo(PatnerID);
                return result;
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("Fact/GetAllInvoices", ex);
                return null;
            }
        }
        // end

        [HttpPost]
        public async Task<IActionResult> CreateInvoice(BPCInvoice Invoice)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var result = await _InvoiceRepository.CreateInvoice(Invoice);
                return Ok(result);
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("Fact/CreateInvoice", ex);
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateInvoiceDetails([FromBody] List<BPCInvoice> Invoices)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var result = await _InvoiceRepository.CreateInvoices(Invoices);
                if (result != null)
                {
                    return Ok("Data are inserted successfully");
                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("Fact/CreateInvoice", ex);
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> UpdateInvoice(BPCInvoice Invoice)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                await _InvoiceRepository.UpdateInvoice(Invoice);
                return new OkResult();
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("Fact/UpdateInvoice", ex);
                return BadRequest(ex.Message);
            }
        }


        #region payment record

        [HttpGet]
        public List<BPCPayRecord> GetAllPaymentRecord()
        {
            try
            {
                var result = _InvoiceRepository.GetAllPaymentRecord();
                return result;
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("PO/GetAllPaymentRecord", ex);
                return null;
            }
        }
        public List<BPCPayRecord> GetAllRecordDateFilter()
        {
            try
            {
                var result = _InvoiceRepository.GetAllRecordDateFilter();
                return result;
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("PO/GetAllRecordDateFilter", ex);
                return null;
            }
        }
        //[HttpPost]
        //public async Task<IActionResult> CreatePaymentRecord(BPCPayRecord PayRecord)
        //{
        //    try
        //    {
        //        if (!ModelState.IsValid)
        //        {
        //            return BadRequest(ModelState);
        //        }
        //        var result = await _InvoiceRepository.CreatePaymentRecord(PayRecord);
        //        return Ok(result);
        //    }
        //catch (Exception ex)
        //    {
        //        WriteLog.WriteToFile("PO/CreatePaymentRecord", ex);
        //        return BadRequest(ex.Message);
        //    }
        //}



        [HttpPost]
        public async Task<IActionResult> UpdatePaymentRecord(BPCPayRecord PayRecord)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                await _InvoiceRepository.UpdatePaymentRecord(PayRecord);
                return new OkResult();
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("PO/UpdatePaymentRecord", ex);
                return BadRequest(ex.Message);
            }
        }
        #endregion

        //INV Payment

        [HttpPost]
        public async Task<IActionResult> UpdateInvoicePay(BPCInvoicePayView Invoice)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                await _InvoiceRepository.UpdateInvoicePay(Invoice);
                return new OkResult();
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("PO/UpdateInvoicePay", ex);
                return BadRequest(ex.Message);
            }
        }
    }
}
