using BPCloud_VP_POService.DBContexts;
using BPCloud_VP_POService.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace BPCloud_VP_POService.Repositories
{
    public class GateRepository : IGateRepository
    {
        private readonly POContext _dbContext;
        IConfiguration _configuration;
        private readonly IAIACTRepository _aIACTRepository;

        public GateRepository(POContext dbContext, IConfiguration configuration, IAIACTRepository aIACTRepository)
        {
            _dbContext = dbContext;
            _configuration = configuration;
            _aIACTRepository = aIACTRepository;
        }
        public async Task CreateAllHoveringVechicles(List<BPCGateHoveringVechicles> GateHV)
        {
            try
            {
                _dbContext.GateHV.AddRange(GateHV);
                await _dbContext.SaveChangesAsync();
                //return null;
            }
            catch (SqlException ex) { WriteLog.WriteToFile("GateRepository/CreateAllHoveringVechicles", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("GateRepository/CreateAllHoveringVechicles", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task CreateHoveringVechicles(BPCGateHoveringVechicles GateHV)
        {
            try
            {
                _dbContext.GateHV.Add(GateHV);
                await _dbContext.SaveChangesAsync();
            }
            catch (SqlException ex) { WriteLog.WriteToFile("GateRepository/CreateHoveringVechicles", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("GateRepository/CreateHoveringVechicles", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<BPCGateHoveringVechicles> GetHoveringVechicleByPartnerId(string PartnerId)
        {
            try
            {
                var result = _dbContext.GateHV.Where(x => x.PatnerID == PartnerId).ToList();
                return result;
            }
            catch (SqlException ex) { WriteLog.WriteToFile("GateRepository/GetHoveringVechicleByPartnerId", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("GateRepository/GetHoveringVechicleByPartnerId", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<BPCGateEntry> CreateGateEntryByAsnList(ASNListView Asn)
        {
            try
            {
                bool IsNonZeroOpenQty = false;
                var header = _dbContext.BPCOFHeaders.Where(x => x.Client == Asn.Client && x.Type == Asn.Type && x.Company == Asn.Company && x.PatnerID == Asn.PatnerID && x.DocNumber == Asn.DocNumber).FirstOrDefault();
                var items = _dbContext.BPCOFItems.Where(x => x.Client == Asn.Client && x.Type == Asn.Type && x.Company == Asn.Company && x.PatnerID == Asn.PatnerID && x.DocNumber == Asn.DocNumber).ToList();
                var ASNheader = _dbContext.BPCASNHeaders.Where(x => x.Client == Asn.Client && x.Type == Asn.Type && x.Company == Asn.Company && x.PatnerID == Asn.PatnerID && x.ASNNumber == Asn.ASNNumber && x.DocNumber == Asn.DocNumber).FirstOrDefault();
                var ASNLists = _dbContext.BPCASNHeaders.Where(x => x.Client == Asn.Client && x.Type == Asn.Type && x.Company == Asn.Company && x.PatnerID == Asn.PatnerID && x.DocNumber == Asn.DocNumber).ToList();
                var GRNLists = _dbContext.BPCOFGRGIs.Where(x => x.Client == Asn.Client && x.Type == Asn.Type && x.Company == Asn.Company && x.PatnerID == Asn.PatnerID && x.DocNumber == Asn.DocNumber).ToList();
                bool isAllGateEntryCompleted = true;
                BPCGateEntry vechicles = new BPCGateEntry();
                vechicles.Client = Asn.Client;
                vechicles.Company = Asn.Company;
                vechicles.Type = Asn.Type;
                vechicles.PatnerID = Asn.PatnerID;
                vechicles.ASNNumber = Asn.ASNNumber;
                vechicles.DocNumber = Asn.DocNumber;
                //vechicles.Material = Asn.Material;
                vechicles.VessleNumber = Asn.VessleNumber;
                vechicles.GateEntryTime = DateTime.Now;
                //vechicles.ASNQty = Asn.ASNQty;
                vechicles.DepartureDate = Asn.DepartureDate;
                vechicles.ArrivalDate = Asn.ArrivalDate;
                vechicles.Status = "GateEntry Completed";
                vechicles.Transporter = ASNheader.TransporterName;
                //vechicles.Gate = ASNheader.TransporterName;
                vechicles.Plant = Asn.Plant;

                vechicles.CreatedOn = DateTime.Now;
                vechicles.IsActive = true;
                vechicles.ModifiedOn = DateTime.Now;
                var gate = _dbContext.BPCGateEntries.Where(x => x.Client == Asn.Client && x.Type == Asn.Type && x.Company == Asn.Company && x.PatnerID == Asn.PatnerID && x.DocNumber == Asn.DocNumber && x.ASNNumber == vechicles.ASNNumber).FirstOrDefault();
                if (gate != null)
                {
                    _dbContext.BPCGateEntries.Remove(gate);
                }
                _dbContext.BPCGateEntries.Add(vechicles);
                //header.Status = "DueForGRN";
                ASNheader.Status = "GateEntry Completed";
                var date = DateTime.Now;
                ASNheader.CancelDuration = DateTime.Now.AddHours(3);
                foreach (var item in items)
                {
                    if (item.OpenQty.HasValue && item.OpenQty.Value > 0)
                    {
                        IsNonZeroOpenQty = true;
                    }
                }
                foreach (var asnl in ASNLists)
                {
                    if (asnl.Status.ToLower() != "gateentry completed")
                    {
                        isAllGateEntryCompleted = false;
                    }
                }
                if (header != null)
                {
                    if (IsNonZeroOpenQty)
                    {
                        header.Status = "PartialASN";
                    }
                    else
                    {
                        //header.Status = "PartialASN";
                        header.Status = isAllGateEntryCompleted ? GRNLists.Count > 0 ? "PartialGRN" : "DueForGRN" : "PartialGate";
                    }
                }
                await _dbContext.SaveChangesAsync();
                return vechicles;
            }
            catch (SqlException ex) { WriteLog.WriteToFile("GateRepository/CreateGateEntryByAsnList", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("GateRepository/CreateGateEntryByAsnList", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task CancelGateEntryByAsnList(ASNListView Asn)
        {
            try
            {
                var header = _dbContext.BPCOFHeaders.Where(x => x.Client == Asn.Client && x.Type == Asn.Type && x.Company == Asn.Company && x.PatnerID == Asn.PatnerID && x.DocNumber == Asn.DocNumber).FirstOrDefault();
                var ASNheader = _dbContext.BPCASNHeaders.Where(x => x.Client == Asn.Client && x.Type == Asn.Type && x.Company == Asn.Company && x.PatnerID == Asn.PatnerID && x.ASNNumber == Asn.ASNNumber && x.DocNumber == Asn.DocNumber).FirstOrDefault();
                var Gate = _dbContext.GateHV.Where(x => x.Client == Asn.Client && x.Type == Asn.Type && x.Company == Asn.Company && x.PatnerID == Asn.PatnerID && x.DocNo == Asn.DocNumber).FirstOrDefault();

                _dbContext.GateHV.Remove(Gate);
                header.Status = "DueForGate";
                ASNheader.Status = "GateEntry";
                await _dbContext.SaveChangesAsync();
            }
            catch (SqlException ex) { WriteLog.WriteToFile("GateRepository/CancelGateEntryByAsnList", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("GateRepository/CancelGateEntryByAsnList", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
