using BPCloud_VP.FactService.DBContexts;
using BPCloud_VP.FactService.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace BPCloud_VP.FactService.Repositories
{
    public class CardRepository : ICardRepository
    {
        private readonly FactContext _dbContext;

        public CardRepository(FactContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task SaveDashboardCards(List<BPCDashboardCard> BPCDashboardCards)
        {
            try
            {
                _dbContext.BPCDashboardCards.ToList().ForEach(x => _dbContext.BPCDashboardCards.Remove(x));
                await _dbContext.SaveChangesAsync();
                foreach (var card in BPCDashboardCards)
                {
                    card.IsActive = true;
                    card.CreatedOn = DateTime.Now;
                    var result = _dbContext.BPCDashboardCards.Add(card);
                    await _dbContext.SaveChangesAsync();
                }

            }
            catch (SqlException ex) { WriteLog.WriteToFile("CardRepository/SaveDashboardCards", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("CardRepository/SaveDashboardCards", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public BPCDashboardCard GetDashboardCard1()
        {
            try
            {
                var result = (from tb in _dbContext.BPCDashboardCards
                              orderby tb.AttachmentID
                              select tb).FirstOrDefault();
                return result;
            }
            catch (SqlException ex) { WriteLog.WriteToFile("CardRepository/GetDashboardCard1", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("CardRepository/GetDashboardCard1", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public BPCDashboardCard GetDashboardCard2()
        {
            try
            {
                var result = (from tb in _dbContext.BPCDashboardCards
                              orderby tb.AttachmentID
                              select tb).Skip(1).Take(1).FirstOrDefault();
                return result;
            }
            catch (SqlException ex) { WriteLog.WriteToFile("CardRepository/GetDashboardCard2", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("CardRepository/GetDashboardCard2", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
