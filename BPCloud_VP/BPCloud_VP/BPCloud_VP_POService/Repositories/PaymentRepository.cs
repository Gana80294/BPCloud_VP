using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using BPCloud_VP_POService.DBContexts;
using BPCloud_VP_POService.Models;

namespace BPCloud_VP_POService.Repositories
{
    public class PaymentRepository : IPaymentRepository
    {
        private readonly POContext _dbContext;
        public PaymentRepository(POContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<BPCPayAccountStatement> CreateAccountStatements(List<BPCPayAccountStatement> AccountStatements)
        {
            var log = new BPCLog();
            try
            {

                log = await CreateBPCLog("CreateAccountStatements", AccountStatements.Count);
                WriteLog.WriteToFile($"PaymentRepository/CreateAccountStatements:-{AccountStatements.Count} AccountStatement records found");
                foreach (var AccountStatement in AccountStatements)
                {
                    WriteLog.WriteToFile($"PaymentRepository/CreateAccountStatements:- Trying to insert AccountStatement for {AccountStatement.PartnerID} - {AccountStatement.FiscalYear} - {AccountStatement.DocumentNumber}");

                    var entity = _dbContext.BPCPayAccountStatements.FirstOrDefault(x => x.Client == AccountStatement.Client && x.Company == AccountStatement.Company && x.Type == AccountStatement.Type && x.PartnerID == AccountStatement.PartnerID && x.FiscalYear == AccountStatement.FiscalYear && x.DocumentNumber == AccountStatement.DocumentNumber);
                    if (entity == null)
                    {
                        AccountStatement.IsActive = true;
                        AccountStatement.CreatedBy = "SAP API";
                        AccountStatement.CreatedOn = DateTime.Now;
                        var result = _dbContext.BPCPayAccountStatements.AddAsync(AccountStatement);
                        await _dbContext.SaveChangesAsync();
                        //await UpdateBPCSucessLog(log.LogID);
                        WriteLog.WriteToFile($"PaymentRepository/CreateAccountStatements:-AccountStatement inserted for {AccountStatement.PartnerID} - {AccountStatement.FiscalYear} - {AccountStatement.DocumentNumber}");
                    }
                    else
                    {
                        WriteLog.WriteToFile($"PaymentRepository/CreateAccountStatements:- AccountStatement details already exist for {AccountStatement.PartnerID} - {AccountStatement.FiscalYear} - {AccountStatement.DocumentNumber}");
                        await UpdateAccountStatement(AccountStatement);
                    }

                }
                if (log != null)
                {
                    await UpdateBPCSucessLog(log.LogID);
                }
                else
                {
                    WriteLog.WriteToFile("Payment/CreateAccountStatements--" + "Unable to generate Log");
                }
                if (AccountStatements.Count > 0)
                {
                    return AccountStatements[0];
                }
                return null;
            }
            catch (SqlException ex) { WriteLog.WriteToFile("PaymentRepository/CreateAccountStatements", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("PaymentRepository/CreateAccountStatements", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                if (log != null)
                {
                    await UpdateBPCFailureLog(log.LogID, ex.Message);
                }
                else
                {
                    WriteLog.WriteToFile("Payment/CreateAccountStatements--" + "Unable to generate Log");
                }
                //await UpdateBPCFailureLog(log.LogID, ex.Message);
                throw ex;
            }
        }
        public async Task<BPCPayAccountStatement> UpdateAccountStatement(BPCPayAccountStatement AccountStatement)
        {
            var log = new BPCLog();
            try
            {
                log = await CreateBPCLog("UpdateAccountStatement", 1);
                var entity = _dbContext.BPCPayAccountStatements.FirstOrDefault(x => x.Client == AccountStatement.Client && x.Company == AccountStatement.Company && x.Type == AccountStatement.Type && x.PartnerID == AccountStatement.PartnerID && x.FiscalYear == AccountStatement.FiscalYear && x.DocumentNumber == AccountStatement.DocumentNumber);
                if (entity == null)
                {
                    return entity;
                }
                //_dbContext.Entry(AccountStatement).State = EntityState.Modified;
                entity.DocumentDate = AccountStatement.DocumentDate;
                entity.InvoiceNumber = AccountStatement.InvoiceNumber;
                entity.InvoiceDate = AccountStatement.InvoiceDate;
                entity.InvoiceAmount = AccountStatement.InvoiceAmount;
                entity.BalanceAmount = AccountStatement.BalanceAmount;
                entity.PaidAmount = AccountStatement.PaidAmount;
                entity.AdvanceAmount = AccountStatement.AdvanceAmount;
                entity.DueDate = AccountStatement.DueDate;
                entity.TDS = AccountStatement.TDS;
                entity.Reference = AccountStatement.Reference;
                entity.Plant = AccountStatement.Plant;
                entity.ProfitCenter = AccountStatement.ProfitCenter;
                entity.Status = AccountStatement.Status;
                entity.ModifiedBy = "SAP API";
                entity.ModifiedOn = DateTime.Now;
                await _dbContext.SaveChangesAsync();
                if (log != null)
                {
                    await UpdateBPCSucessLog(log.LogID);
                }
                else
                {
                    WriteLog.WriteToFile("Payment/UpdateAccountStatement--" + "Unable to generate Log");
                }
                return AccountStatement;
            }
            catch (SqlException ex) { WriteLog.WriteToFile("PaymentRepository/UpdateAccountStatement", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("PaymentRepository/UpdateAccountStatement", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                if (log != null)
                {
                    await UpdateBPCFailureLog(log.LogID, ex.Message);
                }
                else
                {
                    WriteLog.WriteToFile("Payment/UpdateAccountStatement--" + "Unable to generate Log");
                }

                throw ex;
            }
        }

        public List<BPCPayAccountStatement> GetAccountStatementByPartnerID(string PartnerID)
        {
            try
            {
                return _dbContext.BPCPayAccountStatements.Where(x => x.PartnerID == PartnerID).ToList();
            }
            catch (SqlException ex) { WriteLog.WriteToFile("PaymentRepository/GetAccountStatementByPartnerID", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("PaymentRepository/GetAccountStatementByPartnerID", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<BPCPayAccountStatement> FilterAccountStatementByPartnerID(string PartnerID, string DocumentID = null, string ProfitCenter = null, DateTime? FromDate = null, DateTime? ToDate = null)
        {
            try
            {
                bool IsDocumentID = !string.IsNullOrEmpty(DocumentID);
                bool IsFromDate = FromDate.HasValue;
                bool IsToDate = ToDate.HasValue;
                bool IsProfitCenter = !string.IsNullOrEmpty(ProfitCenter);
                var result = (from tb in _dbContext.BPCPayAccountStatements
                              where tb.IsActive && (!IsDocumentID || tb.DocumentNumber == DocumentID) && tb.PartnerID == PartnerID &&
                               (!IsProfitCenter || tb.ProfitCenter == ProfitCenter) &&
                              (!IsFromDate || (tb.DocumentDate.HasValue && tb.DocumentDate.Value.Date >= FromDate.Value.Date))
                              && (!IsToDate || (tb.DocumentDate.HasValue && tb.DocumentDate.Value.Date <= ToDate.Value.Date))
                              select tb).ToList();
                return result;
            }
            catch (SqlException ex) { WriteLog.WriteToFile("PaymentRepository/FilterAccountStatementByPartnerID", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("PaymentRepository/FilterAccountStatementByPartnerID", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<BPCPayPayable> GetPayableByPartnerID(string PartnerID)
        {
            try
            {
                return _dbContext.BPCPayPayables.Where(x => x.PartnerID == PartnerID).ToList();
            }
            catch (SqlException ex) { WriteLog.WriteToFile("PaymentRepository/GetPayableByPartnerID", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("PaymentRepository/GetPayableByPartnerID", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<BPCPayPayable> FilterPayableByPartnerID(string PartnerID, string Invoice = null, DateTime? FromDate = null, DateTime? ToDate = null)
        {
            try
            {
                bool IsInvoice = !string.IsNullOrEmpty(Invoice);
                bool IsFromDate = FromDate.HasValue;
                bool IsToDate = ToDate.HasValue;
                var result = (from tb in _dbContext.BPCPayPayables
                              where tb.IsActive && (!IsInvoice || tb.Invoice == Invoice) && tb.PartnerID == PartnerID &&
                              (!IsFromDate || (tb.InvoiceDate.HasValue && tb.InvoiceDate.Value.Date >= FromDate.Value.Date))
                              && (!IsToDate || (tb.InvoiceDate.HasValue && tb.InvoiceDate.Value.Date <= ToDate.Value.Date))
                              select tb).ToList();
                return result;
            }
            catch (SqlException ex) { WriteLog.WriteToFile("PaymentRepository/FilterPayableByPartnerID", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("PaymentRepository/FilterPayableByPartnerID", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<BPCPayPayment> GetPaymentByPartnerID(string PartnerID)
        {
            try
            {
                return _dbContext.BPCPayPayments.Where(x => x.PartnerID == PartnerID).ToList();
            }
            catch (SqlException ex) { WriteLog.WriteToFile("PaymentRepository/GetPaymentByPartnerID", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("PaymentRepository/GetPaymentByPartnerID", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<BPCPayPayment> FilterPaymentByPartnerID(string PartnerID, DateTime? FromDate = null, DateTime? ToDate = null)
        {
            try
            {
                bool IsFromDate = FromDate.HasValue;
                bool IsToDate = ToDate.HasValue;
                var result = (from tb in _dbContext.BPCPayPayments
                              where tb.IsActive && tb.PartnerID == PartnerID &&
                              (!IsFromDate || (tb.PaymentDate.HasValue && tb.PaymentDate.Value.Date >= FromDate.Value.Date))
                              && (!IsToDate || (tb.PaymentDate.HasValue && tb.PaymentDate.Value.Date <= ToDate.Value.Date))
                              select tb).ToList();
                return result;
            }
            catch (SqlException ex) { WriteLog.WriteToFile("PaymentRepository/FilterPaymentByPartnerID", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("PaymentRepository/FilterPaymentByPartnerID", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<BPCPayTDS> GetTDSByPartnerID(string PartnerID)
        {
            try
            {
                return _dbContext.BPCPayTDS.Where(x => x.PartnerID == PartnerID).ToList();
            }
            catch (SqlException ex) { WriteLog.WriteToFile("PaymentRepository/GetTDSByPartnerID", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("PaymentRepository/GetTDSByPartnerID", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<BPCPayTDS> FilterTDSByPartnerID(string PartnerID, DateTime? FromDate = null, DateTime? ToDate = null)
        {
            try
            {
                bool IsFromDate = FromDate.HasValue;
                bool IsToDate = ToDate.HasValue;
                var result = (from tb in _dbContext.BPCPayTDS
                              where tb.IsActive && tb.PartnerID == PartnerID &&
                              (!IsFromDate || (tb.PostingDate.HasValue && tb.PostingDate.Value.Date >= FromDate.Value.Date))
                              && (!IsToDate || (tb.PostingDate.HasValue && tb.PostingDate.Value.Date <= ToDate.Value.Date))
                              select tb).ToList();
                return result;
            }
            catch (SqlException ex) { WriteLog.WriteToFile("PaymentRepository/FilterTDSByPartnerID", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("PaymentRepository/FilterTDSByPartnerID", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<BPCPayAccountStatement>> AcceptBC(List<BPCPayAccountStatement> accountStatement)
        {
            try
            {
                foreach (var AccountStatement in accountStatement)
                {
                    var entity = _dbContext.Set<BPCPayAccountStatement>().FirstOrDefault(x => x.Client == AccountStatement.Client && x.Company == AccountStatement.Company && x.Type == AccountStatement.Type && x.PartnerID == AccountStatement.PartnerID && x.FiscalYear == AccountStatement.FiscalYear && x.DocumentNumber == AccountStatement.DocumentNumber);
                    if (entity != null)
                    {
                        entity.AcceptedOn = DateTime.Now;
                        entity.Status = "Accepted";
                        entity.AcceptedBy = AccountStatement.AcceptedBy;
                    }
                }
                await _dbContext.SaveChangesAsync();
                return accountStatement;
            }
            catch (SqlException ex) { WriteLog.WriteToFile("PaymentRepository/AcceptBC", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("PaymentRepository/AcceptBC", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<BPCLog> CreateBPCLog(string APIMethod, int NoOfRecords)
        {
            try
            {
                BPCLog log = new BPCLog();
                log.LogDate = DateTime.Now;
                log.APIMethod = APIMethod;
                log.NoOfRecords = NoOfRecords;
                log.Status = "Initiated";
                log.CreatedOn = DateTime.Now;
                var result = _dbContext.BPCLogs.Add(log);
                await _dbContext.SaveChangesAsync();
                return result.Entity;
            }
            catch (SqlException ex) { WriteLog.WriteToFile("PaymentRepository/CreateBPCLog", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("PaymentRepository/CreateBPCLog", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<BPCLog> UpdateBPCSucessLog(int LogID)
        {
            try
            {
                var result = _dbContext.BPCLogs.FirstOrDefault(x => x.LogID == LogID);
                if (result != null)
                {
                    result.Status = "Success";
                    result.Response = "Data are inserted successfully";
                    result.ModifiedOn = DateTime.Now;
                    await _dbContext.SaveChangesAsync();
                }
                return result;
            }
            catch (SqlException ex) { WriteLog.WriteToFile("PaymentRepository/UpdateBPCSucessLog", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("PaymentRepository/UpdateBPCSucessLog", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<BPCLog> UpdateBPCFailureLog(int LogID, string ErrorMessage)
        {
            try
            {
                var result = _dbContext.BPCLogs.FirstOrDefault(x => x.LogID == LogID);
                if (result != null)
                {
                    result.Status = "Failed";
                    result.Response = ErrorMessage;
                    result.ModifiedOn = DateTime.Now;
                    await _dbContext.SaveChangesAsync();
                }
                return result;
            }
            catch (SqlException ex) { WriteLog.WriteToFile("PaymentRepository/UpdateBPCFailureLog", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("PaymentRepository/UpdateBPCFailureLog", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
