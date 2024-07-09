using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using BPCloud_VP.ReportService.DBContexts;
using BPCloud_VP.ReportService.Models;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace BPCloud_VP.ReportService.Repositories
{
    public class IPRepository : IIPRepository
    {
        private readonly ReportContext _dbContext;
        IConfiguration _configuration;
        public IPRepository(ReportContext dbContext,IConfiguration configuration)
        {
            _dbContext = dbContext;
            _configuration = configuration;
        }
        public List<BPCReportIP> GetAllReportIPByPartnerID(string PartnerID)
        {
            try
            {
                return _dbContext.BPCReportIPs.Where(x => x.PatnerID == PartnerID).ToList();
            }
            catch (SqlException ex){ WriteLog.WriteToFile(" ", ex); throw new Exception("Something went wrong");}catch(InvalidOperationException ex){WriteLog.WriteToFile(" ", ex); throw new Exception("Something went wrong"); }catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<BPCReportIP> GetFilteredReportIPByPartnerID(string PartnerID, string Material = null, string Method = null)
        {
            try
            {
                bool IsMaterial = !string.IsNullOrEmpty(Material);
                bool IsMethod = !string.IsNullOrEmpty(Method);
                var result = (from tb in _dbContext.BPCReportIPs
                              where tb.PatnerID == PartnerID && tb.IsActive 
                              && (!IsMaterial || tb.Material == Material) && (!IsMethod || tb.Method == Method)
                              select tb).ToList();
                return result;
            }
            catch (SqlException ex){ WriteLog.WriteToFile(" ", ex); throw new Exception("Something went wrong");}catch(InvalidOperationException ex){WriteLog.WriteToFile(" ", ex); throw new Exception("Something went wrong"); }catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task CreateBPCPAYH(List<BPCPayment> data)
        {
            try
            {
                foreach (BPCPayment PAYH in data)
                {
                    BPCPayment bpc_payh = new BPCPayment();

                    bpc_payh.Client = PAYH.Client;
                    bpc_payh.Company = PAYH.Company;
                    bpc_payh.Type = PAYH.Type;
                    bpc_payh.PatnerID = PAYH.PatnerID;
                    bpc_payh.FiscalYear = PAYH.FiscalYear;
                    bpc_payh.PaymentDoc = PAYH.PaymentDoc;
                    bpc_payh.Date = PAYH.Date;
                    bpc_payh.Amount = PAYH.Amount;
                    bpc_payh.Currency = PAYH.Currency;
                    bpc_payh.Remark = PAYH.Remark;
                    bpc_payh.CreatedBy = "SAP API";
                    bpc_payh.CreatedOn = DateTime.Now;
                    bpc_payh.ModifiedBy = PAYH.ModifiedBy;
                    bpc_payh.ModifiedOn = DateTime.Now;
                    bpc_payh.IsActive = PAYH.IsActive;
                    _dbContext.BPCPayments.Add(bpc_payh);
                    await _dbContext.SaveChangesAsync();
                }
            }
            catch (SqlException ex){ WriteLog.WriteToFile(" ", ex); throw new Exception("Something went wrong");}catch(InvalidOperationException ex){WriteLog.WriteToFile(" ", ex); throw new Exception("Something went wrong"); }catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task UpdateBPCPAYH(List<BPCPayment> data)
        {
            try
            {
                foreach (BPCPayment PAYH in data)
                {
                    BPCPayment bpc_payh = _dbContext.BPCPayments.FirstOrDefault(x => x.Client == PAYH.Client && x.Company == PAYH.Company && x.Type == PAYH.Type && x.PatnerID == PAYH.PatnerID && x.FiscalYear == PAYH.FiscalYear && x.PaymentDoc == PAYH.PaymentDoc);
                    //bpc_payh.Client = PAYH.Client;
                    //bpc_payh.Company = PAYH.Company;
                    //bpc_payh.Type = PAYH.Type;
                    //bpc_payh.PatnerID = PAYH.PatnerID;
                    //bpc_payh.FiscalYear = PAYH.FiscalYear;
                    //bpc_payh.PaymentDoc = PAYH.PaymentDoc;
                    bpc_payh.Date = PAYH.Date;
                    bpc_payh.Amount = PAYH.Amount;
                    bpc_payh.Currency = PAYH.Currency;
                    bpc_payh.Remark = PAYH.Remark;
                    bpc_payh.CreatedBy = PAYH.CreatedBy;
                    bpc_payh.CreatedOn = PAYH.CreatedOn;
                    bpc_payh.ModifiedBy = "SAP API";
                    bpc_payh.ModifiedOn = DateTime.Now;
                    bpc_payh.IsActive = PAYH.IsActive;
                    await _dbContext.SaveChangesAsync();
                }
            }
            catch (SqlException ex){ WriteLog.WriteToFile(" ", ex); throw new Exception("Something went wrong");}catch(InvalidOperationException ex){WriteLog.WriteToFile(" ", ex); throw new Exception("Something went wrong"); }catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task CreateBPCSCSTK(List<BPCSCSTK> data)
        {
            var log = new BPCLog();
            try
            {
                log = await CreateBPCLog("CreateBPCSCSTK", data.Count);
                foreach (BPCSCSTK STK in data)
                {
                    BPCSCSTK BPC_SC_STK = new BPCSCSTK();

                    BPC_SC_STK.Client = STK.Client;
                    BPC_SC_STK.Company = STK.Company;
                    BPC_SC_STK.Type = STK.Type;
                    BPC_SC_STK.PatnerID = STK.PatnerID;
                    BPC_SC_STK.PODocument = STK.PODocument;
                    BPC_SC_STK.Item = STK.Item;
                    BPC_SC_STK.Date = STK.Date;
                    BPC_SC_STK.Material = STK.Material;
                    BPC_SC_STK.IssuedQty = STK.IssuedQty;
                    BPC_SC_STK.StockQty = STK.StockQty;
                    BPC_SC_STK.CreatedBy = "SAP API";
                    BPC_SC_STK.CreatedOn = DateTime.Now;
                    BPC_SC_STK.ModifiedBy = STK.ModifiedBy;
                    BPC_SC_STK.ModifiedOn = DateTime.Now;
                    BPC_SC_STK.IsActive = STK.IsActive;
                    _dbContext.BPCSCSTKs.Add(BPC_SC_STK);
                    await _dbContext.SaveChangesAsync();
                    //await UpdateBPCSucessLog(log.LogID);
                }
                if (log != null)
                {
                    await UpdateBPCSucessLog(log.LogID);
                }
                else
                {
                    WriteLog.WriteToFile("Report/CreateBPCSCSTK--" + "Unable to generate Log");
                }
            }
            catch (SqlException ex){ WriteLog.WriteToFile(" ", ex); throw new Exception("Something went wrong");}catch(InvalidOperationException ex){WriteLog.WriteToFile(" ", ex); throw new Exception("Something went wrong"); }catch (Exception ex)
            {
                if (log != null)
                {
                    await UpdateBPCFailureLog(log.LogID, ex.Message);
                }
                else
                {
                    WriteLog.WriteToFile("Report/CreateBPCSCSTK--" + "Unable to generate Log");
                }
                //await UpdateBPCFailureLog(log.LogID, Ex.Message);
                throw ex;
            }
        }

        public async Task UpdateBPCSCSTK(List<BPCSCSTK> data)
        {
            var log = new BPCLog();
            try
            {
                log = await CreateBPCLog("UpdateBPCSCSTK", data.Count);
                foreach (BPCSCSTK stk in data)
                {
                    BPCSCSTK bpc_stk = _dbContext.BPCSCSTKs.FirstOrDefault(x => x.Client == stk.Client && x.Company == stk.Company && x.Type == stk.Type && x.PatnerID == stk.PatnerID && x.PODocument == stk.PODocument && x.Item == stk.Item);
                    //bpc_payh.Client = PAYH.Client;
                    //bpc_payh.Company = PAYH.Company;
                    //bpc_payh.Type = PAYH.Type;
                    //bpc_payh.PatnerID = PAYH.PatnerID;
                    bpc_stk.Date = stk.Date;
                    bpc_stk.IssuedQty = stk.IssuedQty;
                    bpc_stk.Material = stk.Material;
                    bpc_stk.StockQty = stk.StockQty;
                    bpc_stk.CreatedBy = stk.CreatedBy;
                    bpc_stk.CreatedOn = stk.CreatedOn;
                    bpc_stk.ModifiedBy = "SAP API";
                    bpc_stk.ModifiedOn = DateTime.Now;
                    bpc_stk.IsActive = stk.IsActive;
                    await _dbContext.SaveChangesAsync();
                    //await UpdateBPCSucessLog(log.LogID);
                }
                if (log != null)
                {
                    await UpdateBPCSucessLog(log.LogID);
                }
                else
                {
                    WriteLog.WriteToFile("Report/UpdateBPCSCSTK--" + "Unable to generate Log");
                }
            }
            catch (SqlException ex){ WriteLog.WriteToFile(" ", ex); throw new Exception("Something went wrong");}catch(InvalidOperationException ex){WriteLog.WriteToFile(" ", ex); throw new Exception("Something went wrong"); }catch (Exception ex)
            {
                if (log != null)
                {
                    await UpdateBPCFailureLog(log.LogID, ex.Message);
                }
                else
                {
                    WriteLog.WriteToFile("Report/UpdateBPCSCSTK--" + "Unable to generate Log");
                }
                //await UpdateBPCFailureLog(log.LogID, Ex.Message);
                throw ex;
            }
        }
        public List<BPCPayment> GetBPCPAYHByPartnerId(string partnerID)
        {
            try
            {
                return _dbContext.BPCPayments.Where(x => x.PatnerID == partnerID).ToList();
            }
            catch(Exception ex)
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
            //catch (SqlException ex){ WriteLog.WriteToFile(" ", ex); throw new Exception("Something went wrong");}catch(InvalidOperationException ex){WriteLog.WriteToFile(" ", ex); throw new Exception("Something went wrong"); }catch (Exception ex)
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
                catch (SqlException ex){ WriteLog.WriteToFile(" ", ex); throw new Exception("Something went wrong");}catch(InvalidOperationException ex){WriteLog.WriteToFile(" ", ex); throw new Exception("Something went wrong"); }catch (Exception ex)
                {

                    throw ex;
                }

            }
            catch (SqlException ex){ WriteLog.WriteToFile(" ", ex); throw new Exception("Something went wrong");}catch(InvalidOperationException ex){WriteLog.WriteToFile(" ", ex); throw new Exception("Something went wrong"); }catch (Exception ex)
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
            //catch (SqlException ex){ WriteLog.WriteToFile(" ", ex); throw new Exception("Something went wrong");}catch(InvalidOperationException ex){WriteLog.WriteToFile(" ", ex); throw new Exception("Something went wrong"); }catch (Exception ex)
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
                catch (SqlException ex){ WriteLog.WriteToFile(" ", ex); throw new Exception("Something went wrong");}catch(InvalidOperationException ex){WriteLog.WriteToFile(" ", ex); throw new Exception("Something went wrong"); }catch (Exception ex)
                {

                    throw ex;
                }

            }
            catch (SqlException ex){ WriteLog.WriteToFile(" ", ex); throw new Exception("Something went wrong");}catch(InvalidOperationException ex){WriteLog.WriteToFile(" ", ex); throw new Exception("Something went wrong"); }catch (Exception ex)
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
            //catch (SqlException ex){ WriteLog.WriteToFile(" ", ex); throw new Exception("Something went wrong");}catch(InvalidOperationException ex){WriteLog.WriteToFile(" ", ex); throw new Exception("Something went wrong"); }catch (Exception ex)
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
                catch (SqlException ex){ WriteLog.WriteToFile(" ", ex); throw new Exception("Something went wrong");}catch(InvalidOperationException ex){WriteLog.WriteToFile(" ", ex); throw new Exception("Something went wrong"); }catch (Exception ex)
                {
                    throw ex;
                }
            }
            catch (SqlException ex){ WriteLog.WriteToFile(" ", ex); throw new Exception("Something went wrong");}catch(InvalidOperationException ex){WriteLog.WriteToFile(" ", ex); throw new Exception("Something went wrong"); }catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
