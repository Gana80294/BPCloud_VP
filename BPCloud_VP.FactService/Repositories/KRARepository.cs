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
using System.Threading.Tasks;

namespace BPCloud_VP.FactService.Repositories
{
    public class KRARepository : IKRARepository
    {
        private readonly FactContext _dbContext;
        IConfiguration _configuration;
        public KRARepository(FactContext dbContext, IConfiguration configuration)
        {
            _dbContext = dbContext;
            _configuration = configuration;
        }

        public List<BPCKRA> GetAllKRAs()
        {
            try
            {
                return _dbContext.BPCKRAs.ToList();
            }
            catch (SqlException ex) { WriteLog.WriteToFile("KRARepository/GetAllKRAs", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("KRARepository/GetAllKRAs", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<BPCKRA> GetKRAsByPartnerID(string PartnerID)
        {
            try
            {
                return _dbContext.BPCKRAs.Where(x => x.PatnerID == PartnerID).ToList();
            }
            catch (SqlException ex) { WriteLog.WriteToFile("KRARepository/GetKRAsByPartnerID", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("KRARepository/GetKRAsByPartnerID", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<BPCKRA> CreateKRA(BPCKRA KRA)
        {
            try
            {
                KRA.IsActive = true;
                KRA.CreatedOn = DateTime.Now;
                var result = _dbContext.BPCKRAs.Add(KRA);
                await _dbContext.SaveChangesAsync();
                return KRA;
            }
            catch (SqlException ex) { WriteLog.WriteToFile("KRARepository/CreateKRA", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("KRARepository/CreateKRA", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<BPCKRA> CreateKRAs(List<BPCKRA> KRAs)
        {
            var log = new BPCLog();
            try
            {
                log = await CreateBPCLog("CreateKRADetails", KRAs.Count);
                foreach (BPCKRA KRA in KRAs)
                {
                    var entity = _dbContext.BPCKRAs.FirstOrDefault(x => x.Client == KRA.Client && x.Company == KRA.Company && x.Type == KRA.Type && x.PatnerID == KRA.PatnerID && x.KRA == KRA.KRA);
                    if (entity == null)
                    {
                        KRA.IsActive = true;
                        KRA.CreatedBy = "SAP API";
                        KRA.CreatedOn = DateTime.Now;
                        var result = _dbContext.BPCKRAs.Add(KRA);
                        await _dbContext.SaveChangesAsync();
                    }
                    else
                    {
                        WriteLog.WriteToFile($"KRARepository/CreateKRAs:- KRA details already exist for {KRA.PatnerID} - {KRA.KRA}");
                        await UpdateKRA(KRA);
                    }

                }
                //await _dbContext.SaveChangesAsync();
                if (log != null)
                {
                    await UpdateBPCSucessLog(log.LogID);
                }
                else
                {
                    WriteLog.WriteToFile("Fact/CreateKRAs--" + "Unable to generate Log");
                }
                if (KRAs.Count > 0)
                    return KRAs[0];
                return null;
            }
            catch (SqlException ex) { WriteLog.WriteToFile("KRARepository/CreateKRAs", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("KRARepository/CreateKRAs", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                if (log != null)
                {
                    await UpdateBPCFailureLog(log.LogID, ex.Message);
                }
                else
                {
                    WriteLog.WriteToFile("Fact/CreateKRAs--" + "Unable to generate Log");
                }
                throw ex;
            }
        }

        public async Task CreateKRAs(List<BPCKRA> KRAs, string PartnerID)
        {
            try
            {
                foreach (BPCKRA KRA in KRAs)
                {
                    KRA.PatnerID = PartnerID;
                    KRA.IsActive = true;
                    KRA.CreatedOn = DateTime.Now;
                    var result = _dbContext.BPCKRAs.Add(KRA);
                }
                await _dbContext.SaveChangesAsync();
            }
            catch (SqlException ex) { WriteLog.WriteToFile("KRARepository/CreateKRAs", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("KRARepository/CreateKRAs", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<BPCKRA> UpdateKRA(BPCKRA KRA)
        {
            var log = new BPCLog();
            try
            {
                log = await CreateBPCLog("UpdateKRA", 1);
                var entity = _dbContext.Set<BPCKRA>().FirstOrDefault(x => x.Client == KRA.Client && x.Company == KRA.Company && x.Type == KRA.Type && x.PatnerID == KRA.PatnerID && x.KRA == KRA.KRA);
                if (entity == null)
                {
                    await UpdateBPCFailureLog(log.LogID, $"No Records found for PatnerID-{KRA.PatnerID} and KRA-{KRA.KRA}");
                    return entity;
                }
                //_dbContext.Entry(KRA).State = EntityState.Modified;
                entity.EvalDate = KRA.EvalDate;
                entity.KRAText = KRA.KRAText;
                entity.KRAValue = KRA.KRAValue;
                entity.ModifiedBy = "SAP API";
                entity.ModifiedOn = DateTime.Now;
                await _dbContext.SaveChangesAsync();
                if (log != null)
                {
                    await UpdateBPCSucessLog(log.LogID);
                }
                else
                {
                    WriteLog.WriteToFile("Fact/UpdateKRA--" + "Unable to generate Log");
                }
                //await UpdateBPCSucessLog(log.LogID);
                return KRA;
            }
            catch (SqlException ex) { WriteLog.WriteToFile("KRARepository/UpdateKRA", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("KRARepository/UpdateKRA", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                if (log != null)
                {
                    await UpdateBPCFailureLog(log.LogID, ex.Message);
                }
                else
                {
                    WriteLog.WriteToFile("Fact/UpdateKRA--" + "Unable to generate Log");
                }
                //await UpdateBPCFailureLog(log.LogID, ex.Message);
                throw ex;
            }
        }

        public async Task<BPCKRA> DeleteKRA(BPCKRA KRA)
        {
            try
            {
                //var entity = await _dbContext.Set<BPCKRA>().FindAsync(KRA.KRA, KRA.Language);
                var entity = _dbContext.Set<BPCKRA>().FirstOrDefault(x => x.PatnerID == KRA.PatnerID && x.KRA == KRA.KRA);
                if (entity == null)
                {
                    return entity;
                }

                _dbContext.Set<BPCKRA>().Remove(entity);
                await _dbContext.SaveChangesAsync();
                return entity;
            }
            catch (SqlException ex) { WriteLog.WriteToFile("KRARepository/DeleteKRA", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("KRARepository/DeleteKRA", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task DeleteKRAByPartnerID(string PartnerID)
        {
            try
            {
                _dbContext.Set<BPCKRA>().Where(x => x.PatnerID == PartnerID).ToList().ForEach(x => _dbContext.Set<BPCKRA>().Remove(x));
                await _dbContext.SaveChangesAsync();
            }
            catch (SqlException ex) { WriteLog.WriteToFile("KRARepository/DeleteKRAByPartnerID", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("KRARepository/DeleteKRAByPartnerID", ex); throw new Exception("Something went wrong"); }
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
            //catch (SqlException ex){ WriteLog.WriteToFile("KRARepository/", ex); throw new Exception("Something went wrong");}catch(InvalidOperationException ex){WriteLog.WriteToFile("KRARepository/", ex); throw new Exception("Something went wrong"); }catch (Exception ex)
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
                catch (SqlException ex) { WriteLog.WriteToFile("KRARepository/CreateBPCLog", ex); throw new Exception("Something went wrong"); }
                catch (InvalidOperationException ex) { WriteLog.WriteToFile("KRARepository/CreateBPCLog", ex); throw new Exception("Something went wrong"); }
                catch (Exception ex)
                {

                    throw ex;
                }

            }
            catch (SqlException ex) { WriteLog.WriteToFile("KRARepository/CreateBPCLog", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("KRARepository/CreateBPCLog", ex); throw new Exception("Something went wrong"); }
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
            //catch (SqlException ex){ WriteLog.WriteToFile("KRARepository/", ex); throw new Exception("Something went wrong");}catch(InvalidOperationException ex){WriteLog.WriteToFile("KRARepository/", ex); throw new Exception("Something went wrong"); }catch (Exception ex)
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
                catch (SqlException ex) { WriteLog.WriteToFile("KRARepository/UpdateBPCSucessLog", ex); throw new Exception("Something went wrong"); }
                catch (InvalidOperationException ex) { WriteLog.WriteToFile("KRARepository/UpdateBPCSucessLog", ex); throw new Exception("Something went wrong"); }
                catch (Exception ex)
                {

                    throw ex;
                }

            }
            catch (SqlException ex) { WriteLog.WriteToFile("KRARepository/UpdateBPCSucessLog", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("KRARepository/UpdateBPCSucessLog", ex); throw new Exception("Something went wrong"); }
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
            //catch (SqlException ex){ WriteLog.WriteToFile("KRARepository/", ex); throw new Exception("Something went wrong");}catch(InvalidOperationException ex){WriteLog.WriteToFile("KRARepository/", ex); throw new Exception("Something went wrong"); }catch (Exception ex)
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
                catch (SqlException ex) { WriteLog.WriteToFile("KRARepository/UpdateBPCFailureLog", ex); throw new Exception("Something went wrong"); }
                catch (InvalidOperationException ex) { WriteLog.WriteToFile("KRARepository/UpdateBPCFailureLog", ex); throw new Exception("Something went wrong"); }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            catch (SqlException ex) { WriteLog.WriteToFile("KRARepository/UpdateBPCFailureLog", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("KRARepository/UpdateBPCFailureLog", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
