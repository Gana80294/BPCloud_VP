using BPCloud_VP_POService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BPCloud_VP_POService.Repositories
{
    public interface IBalanceConfirmationRepository
    {
        List<BalanceConfirmationHeader> GetBalanceConfirmationHeaders();
        List<BalanceConfirmationItem> GetBalanceConfirmationItems();
        Task<BalanceConfirmationHeader> CreateBalConfirmDetails(List<BalanceConfirmationHeader> BalanceConfirmationHeaders);
        Task<BalanceConfirmationItem> CreateBalConfirmItemDetails(List<BalanceConfirmationItem> BalanceConfirmationItems);
        Task<BalanceConfirmationHeader> UpdateBalanceConfirmationHeader(BalanceConfirmationHeader header);
        Task<BalanceConfirmationItem> UpdateBalanceConfirmationItem(BalanceConfirmationItem item);
        BalanceConfirmationHeader GetCurrentHeader();
        List<BalanceConfirmationItem> GetCurrentItems();
        List<BalanceConfirmationItem> GetCurrentBCItemsByPeroid();
        Task UpdateStatus();
        Task AcceptBC(ConfirmationDeatils confirmationDeatils);
    }
}
