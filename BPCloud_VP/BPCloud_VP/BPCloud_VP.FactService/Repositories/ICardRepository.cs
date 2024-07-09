using BPCloud_VP.FactService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BPCloud_VP.FactService.Repositories
{
    public interface ICardRepository
    {
        Task SaveDashboardCards(List<BPCDashboardCard> BPCDashboardCards);
        BPCDashboardCard GetDashboardCard1();
        BPCDashboardCard GetDashboardCard2();
    }
}
