using BPCloud_VP_POService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BPCloud_VP_POService.Repositories
{
    public interface IInvoiceRepository
    {
        List<BPCInvoice> GetAllInvoices();
        Task<BPCInvoice> CreateInvoice(BPCInvoice Invoice);
        Task<BPCInvoice> CreateInvoices(List<BPCInvoice> Invoices);
        Task<BPCInvoice> UpdateInvoice(BPCInvoice Invoice);

        #region Payment record
        List<BPCPayRecord> GetAllPaymentRecord();
        List<BPCPayRecord> GetAllRecordDateFilter();
        //Task CreatePaymentRecord(List<BPCPayRecord> PayRecordList);
        Task<BPCPayRecord> UpdatePaymentRecord(BPCPayRecord PayRecord);
        #endregion
        List<BPCInvoice> GetInvoiceByPartnerIdAnDocumentNo(string PatnerID);
      
        
        //inv payment

        //Task<BPCInvoice> CreateInvoicePay(BPCInvoicePayView InvView);
        Task<BPCInvoice> UpdateInvoicePay(BPCInvoicePayView InvView);
    }
        
}
