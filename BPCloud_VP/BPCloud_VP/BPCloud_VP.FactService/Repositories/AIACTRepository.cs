using BPCloud_VP.FactService.DBContexts;
using BPCloud_VP.FactService.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace BPCloud_VP.FactService.Repositories
{
    public class AIACTRepository : IAIACTRepository
    {
        private readonly FactContext _dbContext;

        public AIACTRepository(FactContext dbContext)
        {
            _dbContext = dbContext;
        }

        public List<BPCAIACT> GetAllAIACTs()
        {
            try
            {
                return _dbContext.BPCAIACTs.ToList();
            }
            catch (SqlException ex) { WriteLog.WriteToFile("AIACTRepository/GetAllAIACTs", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("AIACTRepository/GetAllAIACTs", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<BPCAIACT> GetAIACTsByPartnerID(string PartnerID)
        {
            try
            {
                return _dbContext.BPCAIACTs.Where(x => x.PatnerID == PartnerID).ToList();
            }
            catch (SqlException ex) { WriteLog.WriteToFile("AIACTRepository/GetAIACTsByPartnerID", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("AIACTRepository/GetAIACTsByPartnerID", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<BPCAIACT> GetActionsByPartnerID(string PartnerID)
        {
            try
            {
                return _dbContext.BPCAIACTs.Where(x => x.PatnerID == PartnerID && x.ActType == "01" && !x.IsSeen).ToList();
            }
            catch (SqlException ex) { WriteLog.WriteToFile("AIACTRepository/GetActionsByPartnerID", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("AIACTRepository/GetActionsByPartnerID", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<BPCAIACT> GetNotificationsByPartnerID(string PartnerID)
        {
            try
            {
                return _dbContext.BPCAIACTs.Where(x => x.PatnerID == PartnerID && x.ActType == "02" && !x.IsSeen).ToList();
            }
            catch (SqlException ex) { WriteLog.WriteToFile("AIACTRepository/GetNotificationsByPartnerID", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("AIACTRepository/GetNotificationsByPartnerID", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<BPCAIACT> CreateAIACT(BPCAIACT AIACT)
        {
            try
            {
                AIACT.IsActive = true;
                AIACT.CreatedOn = DateTime.Now;
                var result = _dbContext.BPCAIACTs.Add(AIACT);
                await _dbContext.SaveChangesAsync();
                return AIACT;
            }
            catch (SqlException ex) { WriteLog.WriteToFile("AIACTRepository/CreateAIACT", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("AIACTRepository/CreateAIACT", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<BPCAIACT> CreateAIACTDetails(List<BPCAIACT> AIACTs)
        {
            try
            {
                foreach (BPCAIACT AIACT in AIACTs)
                {
                    int lastSeqNo = _dbContext.BPCAIACTs.OrderByDescending(x => x.SeqNo).Select(x => x.SeqNo).FirstOrDefault();
                    AIACT.SeqNo = lastSeqNo + 1;
                    AIACT.IsActive = true;
                    AIACT.CreatedOn = DateTime.Now;
                    var result = _dbContext.BPCAIACTs.Add(AIACT);
                    await _dbContext.SaveChangesAsync();
                }
                if (AIACTs.Count > 0)
                    return AIACTs[0];
                return null;
            }
            catch (SqlException ex) { WriteLog.WriteToFile("AIACTRepository/CreateAIACTDetails", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("AIACTRepository/CreateAIACTDetails", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task CreateAIACTs(List<BPCAIACT> AIACTs, string PatnerID)
        {
            try
            {
                foreach (BPCAIACT AIACT in AIACTs)
                {
                    AIACT.PatnerID = PatnerID;
                    AIACT.IsActive = true;
                    AIACT.CreatedOn = DateTime.Now;
                    var result = _dbContext.BPCAIACTs.Add(AIACT);
                }
                await _dbContext.SaveChangesAsync();
            }
            catch (SqlException ex) { WriteLog.WriteToFile("AIACTRepository/CreateAIACTs", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("AIACTRepository/CreateAIACTs", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<BPCAIACT> UpdateAIACT(BPCAIACT AIACT)
        {
            try
            {
                var entity = _dbContext.Set<BPCAIACT>().FirstOrDefault(x => x.Client == AIACT.Client && x.Company == x.Company && x.Type == AIACT.Type && x.PatnerID == AIACT.PatnerID && x.SeqNo == AIACT.SeqNo);
                if (entity == null)
                {
                    return entity;
                }
                //_dbContext.Entry(AIACT).State = EntityState.Modified;
                //entity.AppID = AIACT.AppID;
                //entity.DocNumber = AIACT.DocNumber;
                //entity.ActionText = AIACT.ActionText;
                //entity.Status = AIACT.Status;
                //entity.Date = AIACT.Date;
                //entity.Time = AIACT.Time;
                entity.Status = AIACT.Status;
                entity.IsSeen = AIACT.IsSeen;
                entity.ModifiedBy = AIACT.ModifiedBy;
                entity.ModifiedOn = DateTime.Now;
                await _dbContext.SaveChangesAsync();
                return AIACT;
            }
            catch (SqlException ex) { WriteLog.WriteToFile("AIACTRepository/UpdateAIACT", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("AIACTRepository/UpdateAIACT", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public async Task UpdateAIACTs(List<int> SeqNos)
        {
            try
            {

                var entities = _dbContext.Set<BPCAIACT>().Where(x => SeqNos.Any(y => y == x.SeqNo)).ToList();
                foreach (var entity in entities)
                {
                    entity.IsSeen = true;
                    //entity.ModifiedBy = AIACT.ModifiedBy;
                    entity.ModifiedOn = DateTime.Now;
                    await _dbContext.SaveChangesAsync();
                }
            }
            catch (SqlException ex) { WriteLog.WriteToFile("AIACTRepository/UpdateAIACTs", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("AIACTRepository/UpdateAIACTs", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public async Task<BPCAIACT> DeleteAIACT(BPCAIACT AIACT)
        {
            try
            {
                //var entity = await _dbContext.Set<BPCAIACT>().FindAsync(AIACT.AIACT, AIACT.Language);
                var entity = _dbContext.Set<BPCAIACT>().FirstOrDefault(x => x.PatnerID == AIACT.PatnerID && x.SeqNo == AIACT.SeqNo);
                if (entity == null)
                {
                    return entity;
                }

                _dbContext.Set<BPCAIACT>().Remove(entity);
                await _dbContext.SaveChangesAsync();
                return entity;
            }
            catch (SqlException ex) { WriteLog.WriteToFile("AIACTRepository/DeleteAIACT", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("AIACTRepository/DeleteAIACT", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task DeleteAIACTByPartnerID(string PartnerID)
        {
            try
            {
                _dbContext.Set<BPCAIACT>().Where(x => x.PatnerID == PartnerID).ToList().ForEach(x => _dbContext.Set<BPCAIACT>().Remove(x));
                await _dbContext.SaveChangesAsync();
            }
            catch (SqlException ex) { WriteLog.WriteToFile("AIACTRepository/DeleteAIACTByPartnerID", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("AIACTRepository/DeleteAIACTByPartnerID", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<BPCAIACT> AcceptAIACT(BPCAIACT AIACT)
        {
            try
            {
                var entity = _dbContext.Set<BPCAIACT>().FirstOrDefault(x => x.PatnerID == AIACT.PatnerID && x.SeqNo == AIACT.SeqNo);
                if (entity == null)
                {
                    return entity;
                }
                entity.ActionText = AIACT.ActionText;
                entity.Status = AIACT.Status;
                entity.ModifiedBy = AIACT.ModifiedBy;
                entity.ModifiedOn = DateTime.Now;
                await _dbContext.SaveChangesAsync();
                return AIACT;
            }
            catch (SqlException ex) { WriteLog.WriteToFile("AIACTRepository/AcceptAIACT", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("AIACTRepository/AcceptAIACT", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<BPCAIACT> AcceptAIACTs(List<BPCAIACT> AIACTs)
        {
            try
            {
                BPCAIACT bPCAIACT = new BPCAIACT();
                if (AIACTs != null && AIACTs.Count > 0)
                {
                    foreach (var AIACT in AIACTs)
                    {
                        var entity = _dbContext.Set<BPCAIACT>().FirstOrDefault(x => x.PatnerID == AIACT.PatnerID && x.SeqNo == AIACT.SeqNo);
                        //if (entity == null)
                        //{
                        //    return entity;
                        //}
                        entity.ActionText = AIACT.ActionText;
                        entity.Status = AIACT.Status;
                        entity.ModifiedBy = AIACT.ModifiedBy;
                        entity.ModifiedOn = DateTime.Now;
                        bPCAIACT = entity;
                    }
                    await _dbContext.SaveChangesAsync();
                }
                return bPCAIACT;
            }
            catch (SqlException ex) { WriteLog.WriteToFile("AIACTRepository/AcceptAIACTs", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("AIACTRepository/AcceptAIACTs", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<BPCAIACT> RejectAIACT(BPCAIACT AIACT)
        {
            try
            {
                var entity = _dbContext.Set<BPCAIACT>().FirstOrDefault(x => x.PatnerID == AIACT.PatnerID && x.SeqNo == AIACT.SeqNo);
                if (entity == null)
                {
                    return entity;
                }
                entity.ActionText = AIACT.ActionText;
                entity.Status = AIACT.Status;
                entity.ModifiedBy = AIACT.ModifiedBy;
                entity.ModifiedOn = DateTime.Now;
                await _dbContext.SaveChangesAsync();
                return AIACT;
            }
            catch (SqlException ex) { WriteLog.WriteToFile("AIACTRepository/RejectAIACT", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("AIACTRepository/RejectAIACT", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
