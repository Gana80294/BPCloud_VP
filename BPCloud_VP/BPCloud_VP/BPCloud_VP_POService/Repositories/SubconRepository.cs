using BPCloud_VP_POService.DBContexts;
using BPCloud_VP_POService.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace BPCloud_VP_POService.Repositories
{
    public class SubconRepository : ISubconRepository
    {
        private readonly POContext _dbContext;
        IConfiguration _configuration;
        public SubconRepository(POContext dbContext, IConfiguration configuration)
        {
            _dbContext = dbContext;
            _configuration = configuration;
        }
        public async Task CreateSubcon(List<BPCOFSubcon> subcons)
        {
            try
            {
                if (subcons.Count > 0)
                {
                    _dbContext.BPCOFSubcons.Where(x => x.DocNumber == subcons[0].DocNumber && x.Item == subcons[0].Item && x.SlLine == subcons[0].SlLine)
                        .ToList().ForEach(x => _dbContext.BPCOFSubcons.Remove(x));
                }
                foreach (BPCOFSubcon subcon in subcons)
                {
                    subcon.IsActive = true;
                    subcon.CreatedOn = DateTime.Now;
                    _dbContext.BPCOFSubcons.Add(subcon);
                }
                await _dbContext.SaveChangesAsync();
            }
            catch (SqlException ex) { WriteLog.WriteToFile("SubconRepository/CreateSubcon", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("SubconRepository/CreateSubcon", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<BPCOFSubcon> GetSubconByDocAndPartnerID(string DocNumber, string PartnerID)
        {
            try
            {
                return _dbContext.BPCOFSubcons.Where(x => x.DocNumber == DocNumber && x.PatnerID == PartnerID).ToList();
            }
            catch (SqlException ex) { WriteLog.WriteToFile("SubconRepository/GetSubconByDocAndPartnerID", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("SubconRepository/GetSubconByDocAndPartnerID", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<BPCOFSubcon> GetSubconBySLAndPartnerID(string DocNumber, string Item, string SlLine, string PartnerID)
        {
            try
            {
                return _dbContext.BPCOFSubcons.Where(x => x.DocNumber == DocNumber && x.Item == Item && x.SlLine == SlLine && x.PatnerID == PartnerID).ToList();
            }
            catch (SqlException ex) { WriteLog.WriteToFile("SubconRepository/GetSubconBySLAndPartnerID", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("SubconRepository/GetSubconBySLAndPartnerID", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task DeleteSubcon(BPCOFSubcon subcon)
        {
            try
            {
                _dbContext.BPCOFSubcons.Where(x => x.DocNumber == subcon.DocNumber && x.Item == subcon.Item && x.SlLine == subcon.SlLine)
                          .ToList().ForEach(x => _dbContext.BPCOFSubcons.Remove(x));
                await _dbContext.SaveChangesAsync();
            }
            catch (SqlException ex) { WriteLog.WriteToFile("SubconRepository/DeleteSubcon", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("SubconRepository/DeleteSubcon", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<BPCOFSubconView> GetSubconViewByDocAndPartnerID(string DocNumber, string PartnerID)
        {
            try
            {
                List<BPCOFSubconView> subcons = new List<BPCOFSubconView>();
                var result = _dbContext.BPCOFSubcons.Where(x => x.DocNumber == DocNumber && x.PatnerID == PartnerID).GroupBy(x => x.Item).ToList();
                foreach (var res in result)
                {
                    BPCOFSubconView subconView = new BPCOFSubconView();
                    subconView.OrderedQty = 0;
                    foreach (var subcon in res)
                    {
                        subconView.Client = subcon.Client;
                        subconView.Company = subcon.Company;
                        subconView.Type = subcon.Type;
                        subconView.PatnerID = subcon.PatnerID;
                        subconView.DocNumber = subcon.DocNumber;
                        subconView.Item = subcon.Item;
                        subconView.SlLine = subcon.SlLine;
                        subconView.OrderedQty += subcon.OrderedQty;
                    }
                    subcons.Add(subconView);
                }
                return subcons;
            }
            catch (SqlException ex) { WriteLog.WriteToFile("SubconRepository/GetSubconViewByDocAndPartnerID", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("SubconRepository/GetSubconViewByDocAndPartnerID", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
