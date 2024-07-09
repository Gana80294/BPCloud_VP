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
    public class MessageRepository : IMessageRepository
    {
        private readonly POContext _dbContext;
        IConfiguration _configuration;
        public MessageRepository(POContext dbContext, IConfiguration configuration)
        {
            _dbContext = dbContext;
            _configuration = configuration;
        }
        #region CEOMessage
        public BPCCEOMessage GetCEOMessage()
        {
            try
            {
                var result = (from tb in _dbContext.BPCCEOMessages
                              where tb.IsActive
                              select tb).FirstOrDefault();
                return result;
            }
            catch (SqlException ex) { WriteLog.WriteToFile("MessageRepository/GetCEOMessage", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("MessageRepository/GetCEOMessage", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("CEOMessageRepository/GetCEOMessage : - ", ex);
                throw ex;
            }
        }

        public async Task<BPCCEOMessage> CreateCEOMessage(BPCCEOMessage CEOMessage)
        {
            try
            {
                CEOMessage.CreatedOn = DateTime.Now;
                CEOMessage.IsActive = true;
                var result = _dbContext.BPCCEOMessages.Add(CEOMessage);
                await _dbContext.SaveChangesAsync();
                return result.Entity;
            }
            catch (SqlException ex) { WriteLog.WriteToFile("MessageRepository/CreateCEOMessage", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("MessageRepository/CreateCEOMessage", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("CEOMessageRepository/CreateCEOMessage : - ", ex);
                throw ex;
            }
        }


        public async Task<BPCCEOMessage> UpdateCEOMessage(BPCCEOMessage CEOMessage)
        {
            try
            {
                BPCCEOMessage CEOMessage2 = (from tb in _dbContext.BPCCEOMessages
                                             where tb.IsActive && tb.MessageID == CEOMessage.MessageID
                                             select tb).FirstOrDefault();
                CEOMessage2.CEOMessage = CEOMessage.CEOMessage;
                CEOMessage2.IsReleased = CEOMessage.IsReleased;
                CEOMessage2.IsActive = true;
                CEOMessage2.ModifiedOn = DateTime.Now;
                CEOMessage2.ModifiedBy = CEOMessage.ModifiedBy;
                await _dbContext.SaveChangesAsync();
                return CEOMessage2;
            }
            catch (SqlException ex) { WriteLog.WriteToFile("MessageRepository/UpdateCEOMessage", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("MessageRepository/UpdateCEOMessage", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("CEOMessageRepository/UpdateCEOMessage : - ", ex);
                throw ex;
            }
        }


        public async Task<BPCCEOMessage> DeleteCEOMessage(BPCCEOMessage CEOMessage)
        {
            try
            {
                BPCCEOMessage CEOMessage1 = (from tb in _dbContext.BPCCEOMessages
                                             where tb.IsActive && tb.MessageID == CEOMessage.MessageID
                                             select tb).FirstOrDefault();
                if (CEOMessage1 != null)
                {
                    _dbContext.BPCCEOMessages.Remove(CEOMessage1);
                    await _dbContext.SaveChangesAsync();
                }
                return CEOMessage;
            }
            catch (SqlException ex) { WriteLog.WriteToFile("MessageRepository/DeleteCEOMessage", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("MessageRepository/DeleteCEOMessage", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("CEOMessageRepository/DeleteCEOMessage : - ", ex);
                throw ex;
            }
        }


        #endregion

        #region SCOCMessage
        public BPCSCOCMessage GetSCOCMessage()
        {
            try
            {
                var result = (from tb in _dbContext.BPCSCOCMessages
                              where tb.IsActive
                              select tb).FirstOrDefault();
                return result;
            }
            catch (SqlException ex) { WriteLog.WriteToFile("MessageRepository/GetSCOCMessage", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("MessageRepository/GetSCOCMessage", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("SCOCMessageRepository/GetSCOCMessage : - ", ex);
                throw ex;
            }
        }


        public async Task<BPCSCOCMessage> CreateSCOCMessage(BPCSCOCMessage SCOCMessage)
        {
            try
            {
                SCOCMessage.CreatedOn = DateTime.Now;
                SCOCMessage.IsActive = true;
                var result = _dbContext.BPCSCOCMessages.Add(SCOCMessage);
                await _dbContext.SaveChangesAsync();
                return result.Entity;
            }
            catch (SqlException ex) { WriteLog.WriteToFile("MessageRepository/CreateSCOCMessage", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("MessageRepository/CreateSCOCMessage", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("SCOCMessageRepository/CreateSCOCMessage : - ", ex);
                throw ex;
            }
        }

        public async Task<BPCSCOCMessage> UpdateSCOCMessage(BPCSCOCMessage SCOCMessage)
        {
            try
            {
                BPCSCOCMessage SCOCMessage2 = (from tb in _dbContext.BPCSCOCMessages
                                               where tb.IsActive && tb.MessageID == SCOCMessage.MessageID
                                               select tb).FirstOrDefault();
                SCOCMessage2.SCOCMessage = SCOCMessage.SCOCMessage;
                SCOCMessage2.IsReleased = SCOCMessage.IsReleased;
                SCOCMessage2.IsActive = true;
                SCOCMessage2.ModifiedOn = DateTime.Now;
                SCOCMessage2.ModifiedBy = SCOCMessage.ModifiedBy;
                await _dbContext.SaveChangesAsync();
                return SCOCMessage2;
            }
            catch (SqlException ex) { WriteLog.WriteToFile("MessageRepository/UpdateSCOCMessage", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("MessageRepository/UpdateSCOCMessage", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("SCOCMessageRepository/UpdateSCOCMessage : - ", ex);
                throw ex;
            }
        }

        public async Task<BPCSCOCMessage> DeleteSCOCMessage(BPCSCOCMessage SCOCMessage)
        {
            try
            {
                BPCSCOCMessage SCOCMessage1 = (from tb in _dbContext.BPCSCOCMessages
                                               where tb.IsActive && tb.MessageID == SCOCMessage.MessageID
                                               select tb).FirstOrDefault();
                if (SCOCMessage1 != null)
                {
                    _dbContext.BPCSCOCMessages.Remove(SCOCMessage1);
                    await _dbContext.SaveChangesAsync();
                }
                return SCOCMessage;
            }
            catch (SqlException ex) { WriteLog.WriteToFile("MessageRepository/DeleteSCOCMessage", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("MessageRepository/DeleteSCOCMessage", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("SCOCMessageRepository/DeleteSCOCMessage : - ", ex);
                throw ex;
            }
        }

        #endregion

        #region Welcome meassgae
        public BPCWelcomeMessage GetWelcomeMessage()
        {
            try
            {
                var result = (from tb in _dbContext.BPCWelcomeMessages
                              where tb.IsActive
                              select tb).FirstOrDefault();
                return result;
            }
            catch (SqlException ex) { WriteLog.WriteToFile("MessageRepository/GetWelcomeMessage", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("MessageRepository/GetWelcomeMessage", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("WelcomeMessageRepository/GetWelcomeMessage : - ", ex);
                throw ex;
            }
        }

        public async Task<BPCWelcomeMessage> CreateWelcomeMessage(BPCWelcomeMessage WelcomeMessage)
        {
            try
            {
                WelcomeMessage.CreatedOn = DateTime.Now;
                WelcomeMessage.IsActive = true;
                var result = _dbContext.BPCWelcomeMessages.Add(WelcomeMessage);
                await _dbContext.SaveChangesAsync();
                return result.Entity;
            }
            catch (SqlException ex) { WriteLog.WriteToFile("MessageRepository/CreateWelcomeMessage", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("MessageRepository/CreateWelcomeMessage", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("WelcomeMessageRepository/CreateWelcomeMessage : - ", ex);
                throw ex;
            }
        }
        public async Task<BPCWelcomeMessage> UpdateWelcomeMessage(BPCWelcomeMessage WelcomeMessage)
        {
            try
            {
                BPCWelcomeMessage WelcomeMessage2 = (from tb in _dbContext.BPCWelcomeMessages
                                                     where tb.IsActive && tb.MessageID == WelcomeMessage.MessageID
                                                     select tb).FirstOrDefault();
                WelcomeMessage2.WelcomeMessage = WelcomeMessage.WelcomeMessage;
                WelcomeMessage2.IsReleased = WelcomeMessage.IsReleased;
                WelcomeMessage2.IsActive = true;
                WelcomeMessage2.ModifiedOn = DateTime.Now;
                WelcomeMessage2.ModifiedBy = WelcomeMessage.ModifiedBy;
                await _dbContext.SaveChangesAsync();
                return WelcomeMessage2;
            }
            catch (SqlException ex) { WriteLog.WriteToFile("MessageRepository/UpdateWelcomeMessage", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("MessageRepository/UpdateWelcomeMessage", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("WelcomeMessageRepository/UpdateWelcomeMessage : - ", ex);
                throw ex;
            }
        }

        public async Task<BPCWelcomeMessage> DeleteWelcomeMessage(BPCWelcomeMessage WelcomeMessage)
        {
            try
            {
                BPCWelcomeMessage WelcomeMessage1 = (from tb in _dbContext.BPCWelcomeMessages
                                                     where tb.IsActive && tb.MessageID == WelcomeMessage.MessageID
                                                     select tb).FirstOrDefault();
                if (WelcomeMessage1 != null)
                {
                    _dbContext.BPCWelcomeMessages.Remove(WelcomeMessage1);
                    await _dbContext.SaveChangesAsync();
                }
                return WelcomeMessage;
            }
            catch (SqlException ex) { WriteLog.WriteToFile("MessageRepository/DeleteWelcomeMessage", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("MessageRepository/DeleteWelcomeMessage", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("WelcomeMessageRepository/DeleteCEOMessage : - ", ex);
                throw ex;
            }
        }
        #endregion

    }
}
