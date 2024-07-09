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
    public class ContactPersonRepository : IContactPersonRepository
    {
        private readonly FactContext _dbContext;
        IConfiguration _configuration;
        public ContactPersonRepository(FactContext dbContext, IConfiguration configuration)
        {
            _dbContext = dbContext;
            _configuration = configuration;
        }

        public List<BPCFactContactPerson> GetAllContactPersons()
        {
            try
            {
                return _dbContext.BPCFactContactPersons.ToList();
            }
            catch (SqlException ex) { WriteLog.WriteToFile("ContactPersonRepository/GetAllContactPersons", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("ContactPersonRepository/GetAllContactPersons", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<BPCFactContactPerson> GetContactPersonsByPartnerID(string PartnerID)
        {
            try
            {
                return _dbContext.BPCFactContactPersons.Where(x => x.PatnerID == PartnerID).ToList();
            }
            catch (SqlException ex) { WriteLog.WriteToFile("ContactPersonRepository/GetContactPersonsByPartnerID", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("ContactPersonRepository/GetContactPersonsByPartnerID", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<BPCFactContactPerson> CreateContactPersons(List<BPCFactContactPerson> FactContactPersons)
        {
            var log = new BPCLog();
            try
            {

                log = await CreateBPCLog("CreateContactPersons", FactContactPersons.Count);
                foreach (var FactContactPerson in FactContactPersons)
                {
                    var entity = _dbContext.Set<BPCFactContactPerson>().FirstOrDefault(x => x.Client == FactContactPerson.Client && x.Company == FactContactPerson.Company && x.Type == FactContactPerson.Type && x.PatnerID == FactContactPerson.PatnerID && x.ContactPersonID == FactContactPerson.ContactPersonID);
                    if (entity == null)
                    {
                        FactContactPerson.IsActive = true;
                        FactContactPerson.CreatedBy = "SAP API";
                        FactContactPerson.CreatedOn = DateTime.Now;
                        var result = _dbContext.BPCFactContactPersons.Add(FactContactPerson);
                        await _dbContext.SaveChangesAsync();

                    }
                    else
                    {
                        WriteLog.WriteToFile($"ContactPersonRepository/CreateContactPersons:- Contact person details already exist for {FactContactPerson.PatnerID} - {FactContactPerson.ContactPersonID}");
                        await UpdateContactPerson(FactContactPerson);
                    }
                }
                if (log != null)
                {
                    await UpdateBPCSucessLog(log.LogID);
                }
                else
                {
                    WriteLog.WriteToFile("Fact/CreateContactPersons--" + "Unable to generate Log");
                }
                if (FactContactPersons.Count > 0)
                    return FactContactPersons[0];
                return null;
            }
            catch (SqlException ex) { WriteLog.WriteToFile("ContactPersonRepository/CreateContactPersons", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("ContactPersonRepository/CreateContactPersons", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                if (log != null)
                {
                    await UpdateBPCFailureLog(log.LogID, ex.Message);
                }
                else
                {
                    WriteLog.WriteToFile("Fact/CreateContactPersons--" + "Unable to generate Log");
                }
                throw ex;
            }
        }
        public async Task<BPCFactContactPerson> CreateContactPerson(BPCFactContactPerson FactContactPerson)
        {
            try
            {
                FactContactPerson.IsActive = true;
                FactContactPerson.CreatedOn = DateTime.Now;
                var result = _dbContext.BPCFactContactPersons.Add(FactContactPerson);
                await _dbContext.SaveChangesAsync();
                return FactContactPerson;
            }
            catch (SqlException ex) { WriteLog.WriteToFile("ContactPersonRepository/CreateContactPerson", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("ContactPersonRepository/CreateContactPerson", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task CreateContactPersons(List<BPCFactContactPerson> FactContactPersons, string PartnerID)
        {
            try
            {
                foreach (BPCFactContactPerson FactContactPerson in FactContactPersons)
                {
                    FactContactPerson.PatnerID = PartnerID;
                    FactContactPerson.IsActive = true;
                    FactContactPerson.CreatedOn = DateTime.Now;
                    var result = _dbContext.BPCFactContactPersons.Add(FactContactPerson);
                }
                await _dbContext.SaveChangesAsync();
            }
            catch (SqlException ex) { WriteLog.WriteToFile("ContactPersonRepository/CreateContactPersons", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("ContactPersonRepository/CreateContactPersons", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<BPCFactContactPerson> UpdateContactPerson(BPCFactContactPerson FactContactPerson)
        {
            var log = new BPCLog();
            try
            {
                log = await CreateBPCLog("UpdateContactPerson", 1);
                var entity = _dbContext.Set<BPCFactContactPerson>().FirstOrDefault(x => x.Client == FactContactPerson.Client && x.Company == FactContactPerson.Company && x.Type == FactContactPerson.Type && x.PatnerID == FactContactPerson.PatnerID && x.ContactPersonID == FactContactPerson.ContactPersonID);
                if (entity == null)
                {
                    await UpdateBPCFailureLog(log.LogID, $"No Records found for {FactContactPerson.PatnerID}");

                    return entity;
                }
                //_dbContext.Entry(FactContactPerson).State = EntityState.Modified;
                entity.Name = FactContactPerson.Name;
                entity.ContactNumber = FactContactPerson.ContactNumber;
                entity.Email = FactContactPerson.Email;
                entity.ModifiedBy = "SAP API";
                entity.ModifiedOn = DateTime.Now;
                await _dbContext.SaveChangesAsync();
                if (log != null)
                {
                    await UpdateBPCSucessLog(log.LogID);
                }
                else
                {
                    WriteLog.WriteToFile("Fact/UpdateContactPerson--" + "Unable to generate Log");
                }

                return FactContactPerson;
            }
            catch (SqlException ex) { WriteLog.WriteToFile("ContactPersonRepository/UpdateContactPerson", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("ContactPersonRepository/UpdateContactPerson", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                if (log != null)
                {
                    await UpdateBPCFailureLog(log.LogID, ex.Message);
                }
                else
                {
                    WriteLog.WriteToFile("Fact/UpdateContactPerson--" + "Unable to generate Log");
                }

                throw ex;
            }
        }

        public async Task<BPCFactContactPerson> DeleteContactPerson(BPCFactContactPerson FactContactPerson)
        {
            try
            {
                //var entity = await _dbContext.Set<BPCFactContactPerson>().FindAsync(FactContactPerson.FactContactPerson, FactContactPerson.Language);
                var entity = _dbContext.Set<BPCFactContactPerson>().FirstOrDefault(x => x.PatnerID == FactContactPerson.PatnerID && x.ContactPersonID == FactContactPerson.ContactPersonID);
                if (entity == null)
                {
                    return entity;
                }

                _dbContext.Set<BPCFactContactPerson>().Remove(entity);
                await _dbContext.SaveChangesAsync();
                return entity;
            }
            catch (SqlException ex) { WriteLog.WriteToFile("ContactPersonRepository/DeleteContactPerson", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("ContactPersonRepository/DeleteContactPerson", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task DeleteContactPersonByPartner(string PartnerID)
        {
            try
            {
                _dbContext.Set<BPCFactContactPerson>().Where(x => x.PatnerID == PartnerID).ToList().ForEach(x => _dbContext.Set<BPCFactContactPerson>().Remove(x));
                await _dbContext.SaveChangesAsync();
            }
            catch (SqlException ex) { WriteLog.WriteToFile("ContactPersonRepository/DeleteContactPersonByPartner", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("ContactPersonRepository/DeleteContactPersonByPartner", ex); throw new Exception("Something went wrong"); }
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
            //catch (SqlException ex){ WriteLog.WriteToFile("ContactPersonRepository/", ex); throw new Exception("Something went wrong");}catch(InvalidOperationException ex){WriteLog.WriteToFile("ContactPersonRepository/", ex); throw new Exception("Something went wrong"); }catch (Exception ex)
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
                catch (SqlException ex) { WriteLog.WriteToFile("ContactPersonRepository/CreateBPCLog", ex); throw new Exception("Something went wrong"); }
                catch (InvalidOperationException ex) { WriteLog.WriteToFile("ContactPersonRepository/CreateBPCLog", ex); throw new Exception("Something went wrong"); }
                catch (Exception ex)
                {

                    throw ex;
                }

            }
            catch (SqlException ex) { WriteLog.WriteToFile("ContactPersonRepository/CreateBPCLog", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("ContactPersonRepository/CreateBPCLog", ex); throw new Exception("Something went wrong"); }
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
            //catch (SqlException ex){ WriteLog.WriteToFile("ContactPersonRepository/", ex); throw new Exception("Something went wrong");}catch(InvalidOperationException ex){WriteLog.WriteToFile("ContactPersonRepository/", ex); throw new Exception("Something went wrong"); }catch (Exception ex)
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
                catch (SqlException ex) { WriteLog.WriteToFile("ContactPersonRepository/UpdateBPCSucessLog", ex); throw new Exception("Something went wrong"); }
                catch (InvalidOperationException ex) { WriteLog.WriteToFile("ContactPersonRepository/UpdateBPCSucessLog", ex); throw new Exception("Something went wrong"); }
                catch (Exception ex)
                {

                    throw ex;
                }

            }
            catch (SqlException ex) { WriteLog.WriteToFile("ContactPersonRepository/", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("ContactPersonRepository/", ex); throw new Exception("Something went wrong"); }
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
            //catch (SqlException ex){ WriteLog.WriteToFile("ContactPersonRepository/", ex); throw new Exception("Something went wrong");}catch(InvalidOperationException ex){WriteLog.WriteToFile("ContactPersonRepository/", ex); throw new Exception("Something went wrong"); }catch (Exception ex)
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
                catch (SqlException ex) { WriteLog.WriteToFile("ContactPersonRepository/UpdateBPCFailureLog", ex); throw new Exception("Something went wrong"); }
                catch (InvalidOperationException ex) { WriteLog.WriteToFile("ContactPersonRepository/UpdateBPCFailureLog", ex); throw new Exception("Something went wrong"); }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            catch (SqlException ex) { WriteLog.WriteToFile("ContactPersonRepository/UpdateBPCFailureLog", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("ContactPersonRepository/UpdateBPCFailureLog", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
