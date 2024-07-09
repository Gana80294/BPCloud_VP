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
    public class FLIPCostRepository : IFLIPCostRepository
    {
        private readonly POContext _dbContext;
        //AttachmentRepository attachmentRepository;
        private IConfiguration _configuration;
        public FLIPCostRepository(POContext dbContext)
        {
            _dbContext = dbContext;
            //_configuration = configuration;
            //attachmentRepository = new AttachmentRepository(_dbContext, _configuration);
        }

        public List<BPCFLIPCost> GetAllFLIPCosts()
        {
            try
            {
                return _dbContext.BPCFLIPCosts.ToList();
            }
            catch (SqlException ex) { WriteLog.WriteToFile("FlipCostRepository/GetAllFLIPCosts", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("FlipCostRepository/GetAllFLIPCosts", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<BPCFLIPCost> GetFLIPCostsByFLIPID(string FLIPID)
        {
            try
            {
                return _dbContext.BPCFLIPCosts.Where(x => x.FLIPID == FLIPID).ToList();
            }
            catch (SqlException ex) { WriteLog.WriteToFile("FlipCostRepository/GetFLIPCostsByFLIPID", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("FlipCostRepository/GetFLIPCostsByFLIPID", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<BPCFLIPCost> CreateFLIPCost(BPCFLIPCost FLIPCost)
        {
            try
            {
                FLIPCost.IsActive = true;
                FLIPCost.CreatedOn = DateTime.Now;
                var result = _dbContext.BPCFLIPCosts.Add(FLIPCost);
                await _dbContext.SaveChangesAsync();
                return FLIPCost;
            }
            catch (SqlException ex) { WriteLog.WriteToFile("FlipCostRepository/CreateFLIPCost", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("FlipCostRepository/CreateFLIPCost", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task CreateFLIPCosts(List<BPCFLIPCost> FLIPCosts, string FLIPID)
        {
            try
            {
                if (FLIPCosts != null && FLIPCosts.Count > 0)
                {
                    foreach (BPCFLIPCost FLIPCost in FLIPCosts)
                    {
                        BPCFLIPCost bPCFLIPCost = new BPCFLIPCost();
                        bPCFLIPCost.FLIPID = FLIPID;
                        bPCFLIPCost.Client = FLIPCost.Client;
                        bPCFLIPCost.Company = FLIPCost.Company;
                        bPCFLIPCost.PatnerID = FLIPCost.PatnerID;
                        bPCFLIPCost.Type = FLIPCost.Type;
                        bPCFLIPCost.ExpenceType = FLIPCost.ExpenceType;
                        bPCFLIPCost.Amount = FLIPCost.Amount;
                        bPCFLIPCost.Remarks = FLIPCost.Remarks;
                        bPCFLIPCost.IsActive = true;
                        bPCFLIPCost.CreatedOn = DateTime.Now;
                        var result = _dbContext.BPCFLIPCosts.Add(bPCFLIPCost);
                        //if (!string.IsNullOrEmpty(FLIPCost.AttachmentName))
                        //{
                        //    BPAttachment BPAttachment = new BPAttachment();
                        //    BPAttachment.ProjectName = "BPCloud";
                        //    BPAttachment.AppID = 1;
                        //    BPAttachment.AppNumber = result.Entity.AccountNo;
                        //    BPAttachment.IsHeaderExist = true;
                        //    BPAttachment.HeaderNumber = FLIPID.ToString();
                        //    BPAttachment.AttachmentName = FLIPCost.AttachmentName;
                        //    BPAttachment result1 = await attachmentRepository.AddAttachment(BPAttachment);
                        //    result.Entity.DocID = result1.AttachmentID.ToString();
                        //}
                    }
                    await _dbContext.SaveChangesAsync();
                }
            }
            catch (SqlException ex) { WriteLog.WriteToFile("FlipCostRepository/CreateFLIPCosts", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("FlipCostRepository/CreateFLIPCosts", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<BPCFLIPCost> UpdateFLIPCost(BPCFLIPCost FLIPCost)
        {
            try
            {
                var entity = _dbContext.Set<BPCFLIPCost>().FirstOrDefault(x => x.FLIPID == FLIPCost.FLIPID);
                if (entity == null)
                {
                    return entity;
                }
                //_dbContext.Entry(FLIPCost).State = EntityState.Modified;
                entity.Client = FLIPCost.Client;
                entity.Company = FLIPCost.Company;
                entity.PatnerID = FLIPCost.PatnerID;
                entity.Amount = FLIPCost.Amount;
                entity.ExpenceType = FLIPCost.ExpenceType;
                entity.Amount = FLIPCost.Amount;
                entity.Type = FLIPCost.Type;
                entity.Remarks = FLIPCost.Remarks;
                entity.ModifiedBy = FLIPCost.ModifiedBy;
                entity.ModifiedOn = DateTime.Now;
                await _dbContext.SaveChangesAsync();
                return FLIPCost;
            }
            catch (SqlException ex) { WriteLog.WriteToFile("FlipCostRepository/UpdateFLIPCost", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("FlipCostRepository/UpdateFLIPCost", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<BPCFLIPCost> DeleteFLIPCost(BPCFLIPCost FLIPCost)
        {
            try
            {
                //var entity = await _dbContext.Set<BPCFLIPCost>().FindAsync(FLIPCost.FLIPCost, FLIPCost.Language);
                var entity = _dbContext.Set<BPCFLIPCost>().FirstOrDefault(x => x.FLIPID == FLIPCost.FLIPID);
                if (entity == null)
                {
                    return entity;
                }

                _dbContext.Set<BPCFLIPCost>().Remove(entity);
                await _dbContext.SaveChangesAsync();
                return entity;
            }
            catch (SqlException ex) { WriteLog.WriteToFile("FlipCostRepository/DeleteFLIPCost", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("FlipCostRepository/DeleteFLIPCost", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task DeleteFLIPCostByFLIPID(string FLIPID)
        {
            try
            {
                if (!string.IsNullOrEmpty(FLIPID))
                {
                    _dbContext.Set<BPCFLIPCost>().Where(x => x.FLIPID == FLIPID).ToList().ForEach(x => _dbContext.Set<BPCFLIPCost>().Remove(x));
                    await _dbContext.SaveChangesAsync();
                }
            }
            catch (SqlException ex) { WriteLog.WriteToFile("FlipCostRepository/DeleteFLIPCostByFLIPID", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("FlipCostRepository/DeleteFLIPCostByFLIPID", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
