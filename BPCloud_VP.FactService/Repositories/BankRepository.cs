using BPCloud_VP.FactService.DBContexts;
using BPCloud_VP.FactService.Models;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;

namespace BPCloud_VP.FactService.Repositories
{
    public class BankRepository : IBankRepository
    {
        private readonly FactContext _dbContext;
        IConfiguration _configuration;
        public BankRepository(FactContext dbContext, IConfiguration configuration)
        {
            _dbContext = dbContext;
            _configuration = configuration;
        }

        public List<BPCFactBank> GetAllBanks()
        {
            try
            {
                return _dbContext.BPCFactBanks.ToList();
            }
            catch (SqlException ex) { WriteLog.WriteToFile("BankRepository/GetAllBanks", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("BankRepository/GetAllBanks", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<BPCFactBank> GetBanksByPartnerID(string PartnerID)
        {
            try
            {
                return _dbContext.BPCFactBanks.Where(x => x.PatnerID == PartnerID).ToList();
            }
            catch (SqlException ex) { WriteLog.WriteToFile("BankRepository/GetBanksByPartnerID", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("BankRepository/GetBanksByPartnerID", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<BPCFactBank> CreateBanks(List<BPCFactBank> Banks)
        {
            var log = new BPCLog();
            try
            {
                log = await CreateBPCLog("CreateBanks", Banks.Count);
                foreach (var Bank in Banks)
                {
                    var entity = _dbContext.BPCFactBanks.FirstOrDefault(x => x.Client == Bank.Client && x.Company == Bank.Company && x.Type == Bank.Type && x.PatnerID == Bank.PatnerID && x.AccountNo == Bank.AccountNo);
                    if (entity == null)
                    {
                        Bank.IsActive = true;
                        Bank.CreatedBy = "SAP API";
                        Bank.CreatedOn = DateTime.Now;
                        var result = _dbContext.BPCFactBanks.Add(Bank);
                        await _dbContext.SaveChangesAsync();
                        //await UpdateBPCSucessLog(log.LogID);
                    }
                    else
                    {
                        WriteLog.WriteToFile($"BankRepository/CreateBanks:- Bank details already exist for {Bank.PatnerID} - {Bank.AccountNo}");
                        await UpdateBank(Bank);
                    }
                }
                if (log != null)
                {
                    await UpdateBPCSucessLog(log.LogID);
                }
                else
                {
                    WriteLog.WriteToFile("Fact/CreateBanks--" + "Unable to generate Log");
                }
                if (Banks.Count > 0)
                    return Banks[0];

                return null;
            }
            catch (SqlException ex) { WriteLog.WriteToFile("BankRepository/CreateBanks", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("BankRepository/CreateBanks", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                if (log != null)
                {
                    await UpdateBPCFailureLog(log.LogID, ex.Message);
                }
                else
                {
                    WriteLog.WriteToFile("Fact/CreateBanks--" + "Unable to generate Log");
                }
                throw ex;
            }
        }
        public async Task<BPCFactBank> CreateBank(BPCFactBank Bank)
        {
            try
            {
                Bank.IsActive = true;
                Bank.CreatedOn = DateTime.Now;
                var result = _dbContext.BPCFactBanks.Add(Bank);
                await _dbContext.SaveChangesAsync();
                return Bank;
            }
            catch (SqlException ex) { WriteLog.WriteToFile("BankRepository/CreateBank", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("BankRepository/CreateBank", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<BPCFactBankSupport> CreateSupportBank(BPCFactBankSupport Bank)
        {
            try
            {
                Bank.IsActive = true;
                Bank.CreatedOn = DateTime.Now;
                var result = _dbContext.BPCFactBanksSupport.Add(Bank);
                await _dbContext.SaveChangesAsync();
                return Bank;
            }
            catch (SqlException ex) { WriteLog.WriteToFile("BankRepository/CreateSupportBank", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("BankRepository/CreateSupportBank", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task CreateBanks(List<BPCFactBank> Banks, string PatnerID)
        {
            try
            {
                foreach (BPCFactBank Bank in Banks)
                {
                    Bank.PatnerID = PatnerID;
                    Bank.IsActive = true;
                    Bank.CreatedOn = DateTime.Now;
                    var result = _dbContext.BPCFactBanks.Add(Bank);
                }
                await _dbContext.SaveChangesAsync();
            }
            catch (SqlException ex) { WriteLog.WriteToFile("BankRepository/CreateBanks", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("BankRepository/CreateBanks", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<BPCFactBank> UpdateBank(BPCFactBank Bank)
        {
            var log = new BPCLog();
            try
            {
                log = await CreateBPCLog("UpdateBank", 1);
                var entity = _dbContext.Set<BPCFactBank>().FirstOrDefault(x => x.Client == Bank.Client && x.Company == Bank.Company && x.Type == Bank.Type && x.PatnerID == Bank.PatnerID && x.AccountNo == Bank.AccountNo);
                if (entity == null)
                {
                    await UpdateBPCFailureLog(log.LogID, $"No records found for {Bank.PatnerID}");
                    return entity;
                }
                //_dbContext.Entry(Bank).State = EntityState.Modified;
                entity.BankName = Bank.BankName;
                entity.AccountNo = Bank.AccountNo;
                entity.Name = Bank.Name;
                entity.BankID = Bank.BankID;
                entity.ModifiedBy = "SAP API";
                entity.ModifiedOn = DateTime.Now;
                await _dbContext.SaveChangesAsync();
                if (log != null)
                {
                    await UpdateBPCSucessLog(log.LogID);
                }
                else
                {
                    WriteLog.WriteToFile("Fact/UpdateBank--" + "Unable to generate Log");
                }
                //await UpdateBPCSucessLog(log.LogID);
                return Bank;
            }
            catch (SqlException ex) { WriteLog.WriteToFile("BankRepository/UpdateBank", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("BankRepository/UpdateBank", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                if (log != null)
                {
                    await UpdateBPCFailureLog(log.LogID, ex.Message);
                }
                else
                {
                    WriteLog.WriteToFile("Fact/UpdateBank--" + "Unable to generate Log");
                }
                //await UpdateBPCFailureLog(log.LogID, ex.Message);
                throw ex;
            }
        }

        public async Task<BPCFactBank> DeleteBank(BPCFactBank Bank)
        {
            try
            {
                //var entity = await _dbContext.Set<BPCFact>().FindAsync(Fact.Fact, Fact.Language);
                var entity = _dbContext.Set<BPCFactBank>().FirstOrDefault(x => x.PatnerID == Bank.PatnerID);
                var bankSupport = _dbContext.BPCFactBanksSupport.Where(x => x.PatnerID == Bank.PatnerID && x.AccountNo == Bank.AccountNo).FirstOrDefault();
                if (entity == null)
                {
                    return entity;
                }
                if (bankSupport != null)
                {
                    _dbContext.BPCFactBanksSupport.Remove(bankSupport);
                }
                _dbContext.Set<BPCFactBank>().Remove(entity);
                await _dbContext.SaveChangesAsync();
                return entity;
            }
            catch (SqlException ex) { WriteLog.WriteToFile("BankRepository/DeleteBank", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("BankRepository/DeleteBank", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task DeleteBankByPartnerID(string PartnerID)
        {
            try
            {
                _dbContext.Set<BPCFactBank>().Where(x => x.PatnerID == PartnerID).ToList().ForEach(x => _dbContext.Set<BPCFactBank>().Remove(x));
                await _dbContext.SaveChangesAsync();
            }
            catch (SqlException ex) { WriteLog.WriteToFile("BankRepository/DeleteBankByPartnerID", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("BankRepository/DeleteBankByPartnerID", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task DeleteBankByPartnerIDAndType(string PartnerID)
        {
            try
            {
                _dbContext.Set<BPCFactBank>().Where(x => x.PatnerID == PartnerID && x.Type == "Vendor").ToList().ForEach(x => _dbContext.Set<BPCFactBank>().Remove(x));
                await _dbContext.SaveChangesAsync();
            }
            catch (SqlException ex) { WriteLog.WriteToFile("BankRepository/DeleteBankByPartnerIDAndType", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("BankRepository/DeleteBankByPartnerIDAndType", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task CreateBanks(List<BPCFactBankXLSX> Banks)
        {
            try
            {
                if (Banks != null && Banks.Count() > 0)
                {
                    foreach (BPCFactBankXLSX Bank in Banks)
                    {
                        BPCFactBank bPCFactBank = new BPCFactBank();
                        //await DeleteBankByPartnerIDAndType(Bank.PatnerID);
                        //Convert.ToString(Decimal.Truncate(Convert.ToDecimal()));
                        //mandatory fields start
                        bPCFactBank.PatnerID = Bank.Partnerid;
                        bPCFactBank.Client = Bank.Client;
                        bPCFactBank.Company = Bank.Company;
                        bPCFactBank.Type = Bank.Type;
                        bPCFactBank.AccountNo = Bank.Accountnumber;
                        //mandatory fields end
                        bPCFactBank.Name = Bank.Accountname;
                        bPCFactBank.BankID = Bank.Bankid;
                        bPCFactBank.BankName = Bank.Bankname;
                        bPCFactBank.IsActive = true;
                        bPCFactBank.CreatedOn = DateTime.Now;
                        var result = _dbContext.BPCFactBanks.Add(bPCFactBank);
                    }
                    await _dbContext.SaveChangesAsync();
                }
            }
            catch (SqlException ex) { WriteLog.WriteToFile("BankRepository/CreateBanks", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("BankRepository/CreateBanks", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<BPCLog> CreateBPCLog(string APIMethod, int NoOfRecords)
        {
            //try
            //{
            //    BPCLog log = new BPCLog();
            //    log.LogDate = DateTime.Now;
            //    log.APIMethod = APIMethod;
            //    log.NoOfRecords = NoOfRecords;
            //    log.Status = "Initiated";
            //    log.CreatedOn = DateTime.Now;
            //    var result = _dbContext.BPCLogs.Add(log);
            //    await _dbContext.SaveChangesAsync();
            //    return result.Entity;
            //}
            //catch (SqlException ex){ WriteLog.WriteToFile("BankRepository/", ex); throw new Exception("Something went wrong");}catch(InvalidOperationException ex){WriteLog.WriteToFile("BankRepository/", ex); throw new Exception("Something went wrong"); }catch (Exception ex)
            //{
            //    throw ex;
            //}
            BPCLog log = new BPCLog();
            try
            {
                string BaseAddress = _configuration.GetValue<string>("APIBaseAddress");
                string HostURI = BaseAddress + "/poapi/PO/CreateBPCLog?APIMethod=" + APIMethod + "&NoOfRecords=" + NoOfRecords;
                var uri = new Uri(HostURI);
                var request = (HttpWebRequest)WebRequest.Create(uri);
                request.Method = "GET";
                try
                {
                    using (var response = (HttpWebResponse)await request.GetResponseAsync())
                    {
                        if (response != null && response.StatusCode == HttpStatusCode.OK)
                        {
                            var reader = new StreamReader(response.GetResponseStream());
                            string responseString = await reader.ReadToEndAsync();
                            reader.Close();
                            log = JsonConvert.DeserializeObject<BPCLog>(responseString);
                            return log;
                        }
                        else
                        {
                            var reader = new StreamReader(response.GetResponseStream());
                            string responseString = await reader.ReadToEndAsync();
                            reader.Close();
                            log = null;
                            return log;
                        }
                    }
                }
                catch (WebException ex)
                {
                    using (var stream = ex.Response.GetResponseStream())
                    using (var reader = new StreamReader(stream))
                    {
                        var errorMessage = reader.ReadToEnd();

                        throw ex;
                    }
                }
                catch (SqlException ex) { WriteLog.WriteToFile("BankRepository/CreateBPCLog", ex); throw new Exception("Something went wrong"); }
                catch (InvalidOperationException ex) { WriteLog.WriteToFile("BankRepository/CreateBPCLog", ex); throw new Exception("Something went wrong"); }
                catch (Exception ex)
                {

                    throw ex;
                }

            }
            catch (SqlException ex) { WriteLog.WriteToFile("BankRepository/CreateBPCLog", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("BankRepository/CreateBPCLog", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public async Task<BPCLog> UpdateBPCSucessLog(int LogID)
        {
            //try
            //{
            //    var result = _dbContext.BPCLogs.FirstOrDefault(x => x.LogID == LogID);
            //    if (result != null)
            //    {
            //        result.Status = "Success";
            //        result.Response = "Data are inserted successfully";
            //        result.ModifiedOn = DateTime.Now;
            //        await _dbContext.SaveChangesAsync();
            //    }
            //    return result;
            //}
            //catch (SqlException ex){ WriteLog.WriteToFile("BankRepository/", ex); throw new Exception("Something went wrong");}catch(InvalidOperationException ex){WriteLog.WriteToFile("BankRepository/", ex); throw new Exception("Something went wrong"); }catch (Exception ex)
            //{
            //    throw ex;
            //}


            BPCLog log = new BPCLog();
            try
            {
                string BaseAddress = _configuration.GetValue<string>("APIBaseAddress");
                string HostURI = BaseAddress + "/poapi/PO/UpdateBPCSucessLog?LogID=" + LogID;
                var uri = new Uri(HostURI);
                var request = (HttpWebRequest)WebRequest.Create(uri);
                request.Method = "GET";
                try
                {
                    using (var response = (HttpWebResponse)await request.GetResponseAsync())
                    {
                        if (response != null && response.StatusCode == HttpStatusCode.OK)
                        {
                            var reader = new StreamReader(response.GetResponseStream());
                            string responseString = await reader.ReadToEndAsync();
                            reader.Close();
                            log = JsonConvert.DeserializeObject<BPCLog>(responseString);
                            return log;
                        }
                        else
                        {
                            var reader = new StreamReader(response.GetResponseStream());
                            string responseString = await reader.ReadToEndAsync();
                            reader.Close();
                            log = null;
                            return log;
                        }
                    }
                }
                catch (WebException ex)
                {
                    using (var stream = ex.Response.GetResponseStream())
                    using (var reader = new StreamReader(stream))
                    {
                        var errorMessage = reader.ReadToEnd();

                        throw ex;
                    }
                }
                catch (SqlException ex) { WriteLog.WriteToFile("BankRepository/UpdateBPCSucessLog", ex); throw new Exception("Something went wrong"); }
                catch (InvalidOperationException ex) { WriteLog.WriteToFile("BankRepository/UpdateBPCSucessLog", ex); throw new Exception("Something went wrong"); }
                catch (Exception ex)
                {

                    throw ex;
                }

            }
            catch (SqlException ex) { WriteLog.WriteToFile("BankRepository/UpdateBPCSucessLog", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("BankRepository/UpdateBPCSucessLog", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public async Task<BPCLog> UpdateBPCFailureLog(int LogID, string ErrorMessage)
        {
            //try
            //{
            //    var result = _dbContext.BPCLogs.FirstOrDefault(x => x.LogID == LogID);
            //    if (result != null)
            //    {
            //        result.Status = "Failed";
            //        result.Response = ErrorMessage;
            //        result.ModifiedOn = DateTime.Now;
            //        await _dbContext.SaveChangesAsync();
            //    }
            //    return result;
            //}
            //catch (SqlException ex){ WriteLog.WriteToFile("BankRepository/", ex); throw new Exception("Something went wrong");}catch(InvalidOperationException ex){WriteLog.WriteToFile("BankRepository/", ex); throw new Exception("Something went wrong"); }catch (Exception ex)
            //{
            //    throw ex;
            //}

            BPCLog log = new BPCLog();
            try
            {
                string BaseAddress = _configuration.GetValue<string>("APIBaseAddress");
                string HostURI = BaseAddress + "/poapi/PO/UpdateBPCFailureLog?LogID=" + LogID + "&ErrorMessage=" + ErrorMessage;
                var uri = new Uri(HostURI);
                var request = (HttpWebRequest)WebRequest.Create(uri);
                request.Method = "GET";
                try
                {
                    using (var response = (HttpWebResponse)await request.GetResponseAsync())
                    {
                        if (response != null && response.StatusCode == HttpStatusCode.OK)
                        {
                            var reader = new StreamReader(response.GetResponseStream());
                            string responseString = await reader.ReadToEndAsync();
                            reader.Close();
                            log = JsonConvert.DeserializeObject<BPCLog>(responseString);
                            return log;
                        }
                        else
                        {
                            var reader = new StreamReader(response.GetResponseStream());
                            string responseString = await reader.ReadToEndAsync();
                            reader.Close();
                            log = null;
                            return log;
                        }
                    }
                }
                catch (WebException ex)
                {
                    using (var stream = ex.Response.GetResponseStream())
                    using (var reader = new StreamReader(stream))
                    {
                        var errorMessage = reader.ReadToEnd();
                        throw ex;
                    }
                }
                catch (SqlException ex) { WriteLog.WriteToFile("BankRepository/UpdateBPCFailureLog", ex); throw new Exception("Something went wrong"); }
                catch (InvalidOperationException ex) { WriteLog.WriteToFile("BankRepository/UpdateBPCFailureLog", ex); throw new Exception("Something went wrong"); }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            catch (SqlException ex) { WriteLog.WriteToFile("BankRepository/UpdateBPCFailureLog", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("BankRepository/UpdateBPCFailureLog", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<BPCFactBankSupport> GetSupportBanks(string partnerID)
        {
            try
            {
                return _dbContext.BPCFactBanksSupport.Where(x => x.PatnerID == partnerID).ToList();
            }
            catch (SqlException ex) { WriteLog.WriteToFile("BankRepository/GetSupportBanks", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("BankRepository/GetSupportBanks", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
