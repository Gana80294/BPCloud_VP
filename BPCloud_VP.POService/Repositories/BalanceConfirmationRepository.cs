using BPCloud_VP_POService.DBContexts;
using BPCloud_VP_POService.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace BPCloud_VP_POService.Repositories
{
    public class BalanceConfirmationRepository : IBalanceConfirmationRepository
    {
        private readonly POContext _dbContext;
        public BalanceConfirmationRepository(POContext dbContext)
        {
            _dbContext = dbContext;
        }
        public List<BalanceConfirmationHeader> GetBalanceConfirmationHeaders()
        {
            try
            {
                return _dbContext.BalanceConfirmationHeaders.ToList();
            }
            catch (SqlException ex) { WriteLog.WriteToFile("BalanceConfirmationRepository/GetBalanceConfirmationHeaders", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("BalanceConfirmationRepository/GetBalanceConfirmationHeaders", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public List<BalanceConfirmationItem> GetBalanceConfirmationItems()
        {
            try
            {
                return _dbContext.BalanceConfirmationItems.ToList();
            }
            catch (SqlException ex) { WriteLog.WriteToFile("BalanceConfirmationRepository/GetBalanceConfirmationItems", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("BalanceConfirmationRepository/GetBalanceConfirmationItems", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {

                throw ex;
            }
        }



        #region BC

        public async Task<BalanceConfirmationHeader> CreateBalConfirmDetails(List<BalanceConfirmationHeader> BalanceConfirmationHeaders)
        {
            var log = new BPCLog();

            try
            {
                log = await CreateBPCLog("CreateBalConfirmDetails", BalanceConfirmationHeaders.Count);
                foreach (var header in BalanceConfirmationHeaders)
                {
                    _dbContext.BalanceConfirmationHeaders.ToList().ForEach(x => _dbContext.BalanceConfirmationHeaders.Remove(x));
                    await _dbContext.SaveChangesAsync();
                    var entity = _dbContext.BalanceConfirmationHeaders.FirstOrDefault(x => x.Client == header.Client && x.Company == header.Company && x.Type == header.Type && x.PatnerID == header.PatnerID && x.FiscalYear == header.FiscalYear);
                    if (entity == null)
                    {
                        header.IsActive = true;
                        header.CreatedOn = DateTime.Now;
                        var result = _dbContext.BalanceConfirmationHeaders.Add(header);
                        await _dbContext.SaveChangesAsync();
                        //await UpdateBPCSucessLog(log.LogID);
                    }
                    else
                    {
                        WriteLog.WriteToFile($"BankRepository/CreateBalConfirmDetails:- Balance Confirmation details already exist for {header.PatnerID} - {header.FiscalYear}");
                        await UpdateBalanceConfirmationHeader(header);
                    }
                }
                if (log != null)
                {
                    await UpdateBPCSucessLog(log.LogID);
                }
                else
                {
                    WriteLog.WriteToFile("Balance/CreateBalConfirmDetails--" + "Unable to generate Log");
                }
                if (BalanceConfirmationHeaders.Count > 0)
                {
                    return BalanceConfirmationHeaders[0];
                }
                //await UpdateBPCFailureLog(log.LogID,"No records Found");
                return null;
            }
            catch (SqlException ex) { WriteLog.WriteToFile("BalanceConfirmationRepository/", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("BalanceConfirmationRepository/", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                if (log != null)
                {
                    await UpdateBPCFailureLog(log.LogID, ex.Message);
                }
                else
                {
                    WriteLog.WriteToFile("Balance/CreateBalConfirmDetails--" + "Unable to generate Log");
                }

                throw ex;
            }
        }

        public async Task<BalanceConfirmationHeader> UpdateBalanceConfirmationHeader(BalanceConfirmationHeader header)
        {
            var log = new BPCLog();
            try
            {
                log = await CreateBPCLog("UpdateBalanceConfirmationHeader", 1);
                var entity = _dbContext.Set<BalanceConfirmationHeader>().FirstOrDefault(x => x.Client == header.Client && x.Company == header.Company && x.Type == header.Type && x.PatnerID == header.PatnerID && x.FiscalYear == header.FiscalYear);
                if (entity == null)
                {
                    await UpdateBPCFailureLog(log.LogID, "No Records Found to Update");
                    return entity;
                }
                //_dbContext.Entry(Bank).State = EntityState.Modified;
                entity.BillAmount = header.BillAmount;
                entity.PaidAmont = header.PaidAmont;
                entity.TDSAmount = header.TDSAmount;
                entity.TotalPaidAmount = header.TotalPaidAmount;
                entity.DownPayment = header.DownPayment;
                entity.NetDueAmount = header.NetDueAmount;
                entity.Currency = header.Currency;
                entity.BalDate = header.BalDate;
                entity.ModifiedBy = header.ModifiedBy;
                entity.ModifiedOn = DateTime.Now;
                await _dbContext.SaveChangesAsync();
                if (log != null)
                {
                    await UpdateBPCSucessLog(log.LogID);
                }
                else
                {
                    WriteLog.WriteToFile("Balance/UpdateBalanceConfirmationHeader--" + "Unable to generate Log");
                }
                return header;
            }
            catch (SqlException ex) { WriteLog.WriteToFile("BalanceConfirmationRepository/", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("BalanceConfirmationRepository/", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                if (log != null)
                {
                    await UpdateBPCFailureLog(log.LogID, ex.Message);
                }
                else
                {
                    WriteLog.WriteToFile("Balance/UpdateBalanceConfirmationHeader--" + "Unable to generate Log");
                }

                throw ex;
            }
        }


        public async Task<BalanceConfirmationItem> CreateBalConfirmItemDetails(List<BalanceConfirmationItem> BalanceConfirmationItems)
        {
            try
            {
                _dbContext.BalanceConfirmationItems.ToList().ForEach(x => _dbContext.BalanceConfirmationItems.Remove(x));
                await _dbContext.SaveChangesAsync();
                foreach (var item in BalanceConfirmationItems)
                {
                    var entity = _dbContext.BalanceConfirmationItems.FirstOrDefault(x => x.Client == item.Client && x.Company == item.Company && x.Type == item.Type && x.PatnerID == item.PatnerID && x.FiscalYear == item.FiscalYear && x.DocNumber == item.DocNumber);
                    if (entity == null)
                    {
                        item.IsActive = true;
                        item.CreatedOn = DateTime.Now;
                        var result = _dbContext.BalanceConfirmationItems.Add(item);
                        await _dbContext.SaveChangesAsync();
                    }
                    else
                    {
                        WriteLog.WriteToFile($"BankRepository/CreateBalConfirmItemDetails:- Balance Confirmation item details already exist for {item.PatnerID} - {item.FiscalYear} - {item.DocNumber}");
                        await UpdateBalanceConfirmationItem(item);
                    }
                }
                if (BalanceConfirmationItems.Count > 0)
                    return BalanceConfirmationItems[0];
                return null;
            }
            catch (SqlException ex) { WriteLog.WriteToFile("BalanceConfirmationRepository/", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("BalanceConfirmationRepository/", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<BalanceConfirmationItem> UpdateBalanceConfirmationItem(BalanceConfirmationItem item)
        {
            try
            {
                var entity = _dbContext.Set<BalanceConfirmationItem>().FirstOrDefault(x => x.Client == item.Client && x.Company == item.Company && x.Type == item.Type && x.PatnerID == item.PatnerID && x.FiscalYear == item.FiscalYear && x.DocNumber == item.DocNumber);
                if (entity == null)
                {
                    return entity;
                }
                //_dbContext.Entry(Bank).State = EntityState.Modified;
                entity.DocDate = item.DocDate;
                entity.InvoiceNumber = item.InvoiceNumber;
                entity.InvoiceAmount = item.InvoiceAmount;
                entity.BillAmount = item.BillAmount;
                entity.PaidAmont = item.PaidAmont;
                entity.TDSAmount = item.TDSAmount;
                entity.TotalPaidAmount = item.TotalPaidAmount;
                entity.DownPayment = item.DownPayment;
                entity.NetDueAmount = item.NetDueAmount;
                entity.Currency = item.Currency;
                entity.BalDate = item.BalDate;
                entity.ModifiedBy = item.ModifiedBy;
                entity.ModifiedOn = DateTime.Now;
                await _dbContext.SaveChangesAsync();
                return item;
            }
            catch (SqlException ex) { WriteLog.WriteToFile("BalanceConfirmationRepository/", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("BalanceConfirmationRepository/", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion
        public BalanceConfirmationHeader GetCurrentHeader()
        {
            try
            {
                //return _dbContext.BalanceConfirmationHeaders.FirstOrDefault(BCH => BCH.Period.Month == DateTime.Now.Month);
                return _dbContext.BalanceConfirmationHeaders.FirstOrDefault();
            }
            catch (SqlException ex) { WriteLog.WriteToFile("BalanceConfirmationRepository/GetCurrentHeader", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("BalanceConfirmationRepository/GetCurrentHeader", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public List<BalanceConfirmationItem> GetCurrentItems()
        {
            try
            {
                return _dbContext.BalanceConfirmationItems.ToList();
            }
            catch (SqlException ex) { WriteLog.WriteToFile("BalanceConfirmationRepository/GetCurrentItems", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("BalanceConfirmationRepository/GetCurrentItems", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public List<BalanceConfirmationItem> GetCurrentBCItemsByPeroid()
        {
            try
            {
                var head = _dbContext.BalanceConfirmationHeaders.FirstOrDefault();
                if (head != null)
                {
                    return _dbContext.BalanceConfirmationItems.Where(BCI => BCI.FiscalYear == head.FiscalYear).ToList();
                }
                return _dbContext.BalanceConfirmationItems.ToList();
            }
            catch (SqlException ex) { WriteLog.WriteToFile("BalanceConfirmationRepository/GetCurrentBCItemsByPeroid", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("BalanceConfirmationRepository/GetCurrentBCItemsByPeroid", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public async Task UpdateStatus()
        {
            try
            {
                var Curr_header = _dbContext.BalanceConfirmationHeaders.FirstOrDefault();
                if (Curr_header != null)
                {
                    Curr_header.AcceptedOn = DateTime.Now;
                    Curr_header.Status = "Accepted";
                    await _dbContext.SaveChangesAsync();
                }
            }
            catch (SqlException ex) { WriteLog.WriteToFile("BalanceConfirmationRepository/UpdateStatus", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("BalanceConfirmationRepository/UpdateStatus", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public async Task AcceptBC(ConfirmationDeatils confirmationDeatils)
        {
            try
            {
                var Curr_header = _dbContext.BalanceConfirmationHeaders.FirstOrDefault();
                if (Curr_header != null)
                {
                    Curr_header.AcceptedBy = confirmationDeatils.ConfirmedBy;
                    Curr_header.AcceptedOn = DateTime.Now;
                    Curr_header.Status = "Accepted";
                    await _dbContext.SaveChangesAsync();
                }

            }
            catch (SqlException ex) { WriteLog.WriteToFile("BalanceConfirmationRepository/AcceptBC", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("BalanceConfirmationRepository/AcceptBC", ex); throw new Exception("Something went wrong"); }
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
            catch (SqlException ex) { WriteLog.WriteToFile("BalanceConfirmationRepository/CreateBPCLog", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("BalanceConfirmationRepository/CreateBPCLog", ex); throw new Exception("Something went wrong"); }
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
            catch (SqlException ex) { WriteLog.WriteToFile("BalanceConfirmationRepository/UpdateBPCSucessLog", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("BalanceConfirmationRepository/UpdateBPCSucessLog", ex); throw new Exception("Something went wrong"); }
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
            catch (SqlException ex) { WriteLog.WriteToFile("BalanceConfirmationRepository/UpdateBPCFailureLog", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("BalanceConfirmationRepository/UpdateBPCFailureLog", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
