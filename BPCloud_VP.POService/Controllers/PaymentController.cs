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
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentRepository _paymentRepository;
        public PaymentController(IPaymentRepository PaymentRepository)
        {
            _paymentRepository = PaymentRepository;
        }

        [HttpPost]
        public async Task<IActionResult> CreateAccountStatements([FromBody] List<BPCPayAccountStatement> AccountStatements)
        {
            try
            {

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var result = await _paymentRepository.CreateAccountStatements(AccountStatements);
                if (result != null)
                {
                    return Ok("Data are inserted successfully");
                }
                return Ok(result);

            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("Payment/CreateAccountStatements", ex);
                return null;
            }
        }

        [HttpPost]
        public async Task<IActionResult> UpdateAccountStatement(BPCPayAccountStatement AccountStatement)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var result = await _paymentRepository.UpdateAccountStatement(AccountStatement);
                if (result != null)
                {
                    return Ok("Data are updated successfully");
                }
                return Ok(result);

            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("Payment/UpdateAccountStatement", ex);
                return null;
            }
        }

        [HttpGet]
        public List<BPCPayAccountStatement> GetAccountStatementByPartnerID(string PartnerID)
        {
            try
            {
                var data = _paymentRepository.GetAccountStatementByPartnerID(PartnerID);
                return data;
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("Payment/GetAccountStatementByPartnerID", ex);
                return null;
            }
        }

        [HttpGet]
        public List<BPCPayAccountStatement> FilterAccountStatementByPartnerID(string PartnerID, string DocumentID = null, string ProfitCenter = null, DateTime? FromDate = null, DateTime? ToDate = null)
        {
            try
            {
                var data = _paymentRepository.FilterAccountStatementByPartnerID(PartnerID, DocumentID, ProfitCenter, FromDate, ToDate);
                return data;
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("Payment/FilterAccountStatementByPartnerID", ex);
                return null;
            }
        }
        [HttpGet]
        public List<BPCPayPayable> GetPayableByPartnerID(string PartnerID)
        {
            try
            {
                var data = _paymentRepository.GetPayableByPartnerID(PartnerID);
                return data;
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("Payment/GetPayableByPartnerID", ex);
                return null;
            }
        }

        [HttpGet]
        public List<BPCPayPayable> FilterPayableByPartnerID(string PartnerID, string Invoice = null, DateTime? FromDate = null, DateTime? ToDate = null)
        {
            try
            {
                var data = _paymentRepository.FilterPayableByPartnerID(PartnerID, Invoice, FromDate, ToDate);
                return data;
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("Payment/FilterPayableByPartnerID", ex);
                return null;
            }
        }
        [HttpGet]
        public List<BPCPayPayment> GetPaymentByPartnerID(string PartnerID)
        {
            try
            {
                var data = _paymentRepository.GetPaymentByPartnerID(PartnerID);
                return data;
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("Payment/GetPaymentByPartnerID", ex);
                return null;
            }
        }
        [HttpGet]
        public List<BPCPayPayment> FilterPaymentByPartnerID(string PartnerID, DateTime? FromDate = null, DateTime? ToDate = null)
        {
            try
            {
                var data = _paymentRepository.FilterPaymentByPartnerID(PartnerID, FromDate, ToDate);
                return data;
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("Payment/FilterPaymentByPartnerID", ex);
                return null;
            }
        }
        [HttpGet]
        public List<BPCPayTDS> GetTDSByPartnerID(string PartnerID)
        {
            try
            {
                var data = _paymentRepository.GetTDSByPartnerID(PartnerID);
                return data;
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("Payment/GetTDSByPartnerID", ex);
                return null;
            }
        }
        [HttpGet]
        public List<BPCPayTDS> FilterTDSByPartnerID(string PartnerID, DateTime? FromDate = null, DateTime? ToDate = null)
        {
            try
            {
                var data = _paymentRepository.FilterTDSByPartnerID(PartnerID, FromDate, ToDate);
                return data;
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("Payment/FilterTDSByPartnerID", ex);
                return null;
            }
        }
        [HttpPost]
        public async Task<IActionResult> AcceptBC([FromBody] List<BPCPayAccountStatement> accountStatement)
        {
            try
            {
                var result = await _paymentRepository.AcceptBC(accountStatement);
                return Ok(result);
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("BalanceConfirmation/GetCurrentBCItems", ex);
                return BadRequest(ex);
            }
        }
    }
}