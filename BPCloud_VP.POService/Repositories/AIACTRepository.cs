using BPCloud_VP_POService.DBContexts;
using BPCloud_VP_POService.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace BPCloud_VP_POService.Repositories
{
    public class AIACTRepository : IAIACTRepository
    {
        private readonly POContext _dbContext;

        public AIACTRepository(POContext dbContext)
        {
            _dbContext = dbContext;
        }

        public List<BPCOFAIACT> GetAllAIACTs()
        {
            try
            {
                return _dbContext.BPCOFAIACTs.ToList();
            }
            catch (SqlException ex) { WriteLog.WriteToFile("AIACTRepository/GetAllAIACTs", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("AIACTRepository/GetAllAIACTs", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<BPCOFAIACT> GetAIACTsByPartnerID(string PartnerID)
        {
            try
            {
                return _dbContext.BPCOFAIACTs.Where(x => x.PatnerID == PartnerID).ToList();
            }
            catch (SqlException ex) { WriteLog.WriteToFile("AIACTRepository/GetAIACTsByPartnerID", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("AIACTRepository/GetAIACTsByPartnerID", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<BPCOFAIACT> GetActionsByPartnerID(string PartnerID)
        {
            try
            {
                return _dbContext.BPCOFAIACTs.Where(x => x.PatnerID.ToLower() == PartnerID.ToLower()
                && x.Type.ToLower() == "action").ToList();
            }
            catch (SqlException ex) { WriteLog.WriteToFile("AIACTRepository/GetActionsByPartnerID", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("AIACTRepository/GetActionsByPartnerID", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<BPCOFAIACT> GetNotificationsByPartnerID(string PartnerID)
        {
            try
            {
                return _dbContext.BPCOFAIACTs.Where(x => x.PatnerID.ToLower() == PartnerID.ToLower()
                && x.Type.ToLower() == "notification").ToList();
            }
            catch (SqlException ex) { WriteLog.WriteToFile("AIACTRepository/GetNotificationsByPartnerID", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("AIACTRepository/GetNotificationsByPartnerID", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<BPCOFAIACT> UpdateNotification(BPCOFAIACT AIACT)
        {
            try
            {
                var entity = _dbContext.Set<BPCOFAIACT>().FirstOrDefault(x => x.PatnerID.ToLower() == AIACT.PatnerID.ToLower() &&
                x.SeqNo == AIACT.SeqNo && x.Type.ToLower() == "notification");
                if (entity == null)
                {
                    return entity;
                }
                entity.HasSeen = false;
                await _dbContext.SaveChangesAsync();
                return entity;
            }
            catch (SqlException ex) { WriteLog.WriteToFile("AIACTRepository/UpdateNotification", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("AIACTRepository/UpdateNotification", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<BPCOFAIACT> CreateAIACT(BPCOFAIACT AIACT)
        {
            try
            {
                AIACT.IsActive = true;
                AIACT.HasSeen = true;
                AIACT.CreatedOn = DateTime.Now;
                var result = _dbContext.BPCOFAIACTs.Add(AIACT);
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

        public async Task CreateAIACTs(List<BPCOFAIACT> AIACTs, string PatnerID)
        {
            try
            {
                foreach (BPCOFAIACT AIACT in AIACTs)
                {
                    AIACT.PatnerID = PatnerID;
                    AIACT.IsActive = true;
                    AIACT.CreatedOn = DateTime.Now;
                    var result = _dbContext.BPCOFAIACTs.Add(AIACT);
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

        public async Task<BPCOFAIACT> UpdateAIACT(BPCOFAIACT AIACT)
        {
            try
            {
                var entity = _dbContext.Set<BPCOFAIACT>().FirstOrDefault(x => x.PatnerID == AIACT.PatnerID && x.SeqNo == AIACT.SeqNo);
                if (entity == null)
                {
                    return entity;
                }
                //_dbContext.Entry(AIACT).State = EntityState.Modified;
                entity.AppID = AIACT.AppID;
                entity.DocNumber = AIACT.DocNumber;
                entity.ActionText = AIACT.ActionText;
                entity.Status = AIACT.Status;
                entity.Date = AIACT.Date;
                entity.Time = AIACT.Time;
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

        public async Task<BPCOFAIACT> DeleteAIACT(BPCOFAIACT AIACT)
        {
            try
            {
                //var entity = await _dbContext.Set<BPCOFAIACT>().FindAsync(AIACT.AIACT, AIACT.Language);
                var entity = _dbContext.Set<BPCOFAIACT>().FirstOrDefault(x => x.PatnerID == AIACT.PatnerID && x.SeqNo == AIACT.SeqNo);
                if (entity == null)
                {
                    return entity;
                }

                _dbContext.Set<BPCOFAIACT>().Remove(entity);
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
                _dbContext.Set<BPCOFAIACT>().Where(x => x.PatnerID == PartnerID).ToList().ForEach(x => _dbContext.Set<BPCOFAIACT>().Remove(x));
                await _dbContext.SaveChangesAsync();
            }
            catch (SqlException ex) { WriteLog.WriteToFile("AIACTRepository/DeleteAIACTByPartnerID", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("AIACTRepository/DeleteAIACTByPartnerID", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<BPCOFAIACT> AcceptAIACT(BPCOFAIACT AIACT)
        {
            try
            {
                var entity = _dbContext.Set<BPCOFAIACT>().FirstOrDefault(x => x.PatnerID == AIACT.PatnerID && x.SeqNo == AIACT.SeqNo);
                if (entity == null)
                {
                    return entity;
                }
                entity.ActionText = AIACT.ActionText;
                entity.Status = AIACT.Status;
                entity.ModifiedBy = AIACT.ModifiedBy;
                entity.ModifiedOn = DateTime.Now;
                await _dbContext.SaveChangesAsync();
                return entity;
            }
            catch (SqlException ex) { WriteLog.WriteToFile("AIACTRepository/AcceptAIACT", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("AIACTRepository/AcceptAIACT", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<BPCOFAIACT> AcceptAIACTs(List<BPCOFAIACT> AIACTs)
        {
            try
            {
                BPCOFAIACT bPCAIACT = new BPCOFAIACT();
                if (AIACTs != null && AIACTs.Count > 0)
                {
                    foreach (var AIACT in AIACTs)
                    {
                        var entity = _dbContext.Set<BPCOFAIACT>().FirstOrDefault(x => x.PatnerID == AIACT.PatnerID && x.SeqNo == AIACT.SeqNo);
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

        public async Task<BPCOFAIACT> RejectAIACT(BPCOFAIACT AIACT)
        {
            try
            {
                var entity = _dbContext.Set<BPCOFAIACT>().FirstOrDefault(x => x.PatnerID == AIACT.PatnerID && x.SeqNo == AIACT.SeqNo);
                if (entity == null)
                {
                    return entity;
                }
                entity.ActionText = AIACT.ActionText;
                entity.Status = AIACT.Status;
                entity.ModifiedBy = AIACT.ModifiedBy;
                entity.ModifiedOn = DateTime.Now;
                await _dbContext.SaveChangesAsync();
                return entity;
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
