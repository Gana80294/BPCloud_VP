using BPCloud_VP_POService.DBContexts;
using BPCloud_VP_POService.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace BPCloud_VP_POService.Repositories
{
    public class InvoiceRepository : IInvoiceRepository
    {
        private readonly POContext _dbContext;
        IConfiguration _configuration;

        public InvoiceRepository(POContext dbContext, IConfiguration configuration, IAIACTRepository aIACTRepository)
        {
            _dbContext = dbContext;
            _configuration = configuration;
        }
        public async Task<BPCInvoice> CreateInvoices(List<BPCInvoice> Invoices)
        {
            try
            {
                foreach (var Invoice in Invoices)
                {
                    var entity = _dbContext.Set<BPCInvoice>().FirstOrDefault(x => x.Client == Invoice.Client && x.Company == Invoice.Company && x.Type == Invoice.Type && x.PatnerID == Invoice.PatnerID && x.FiscalYear == Invoice.FiscalYear && x.InvoiceNo == Invoice.InvoiceNo);
                    if (entity == null)
                    {
                        Invoice.IsActive = true;
                        Invoice.CreatedBy = "SAP API";
                        Invoice.CreatedOn = DateTime.Now;
                        var result = _dbContext.BPCInvoices.Add(Invoice);
                        await _dbContext.SaveChangesAsync();
                    }
                    else
                    {
                        WriteLog.WriteToFile($"InvoiceRepository/CreateInvoices:- Invoice details already exist for {Invoice.PatnerID} - {Invoice.FiscalYear} - {Invoice.InvoiceNo}");
                        await UpdateInvoice(Invoice);
                    }

                }
                if (Invoices.Count > 0)
                    return Invoices[0];
                return null;
            }
            catch (SqlException ex){ WriteLog.WriteToFile(" ", ex); throw new Exception("Something went wrong");}catch(InvalidOperationException ex){WriteLog.WriteToFile(" ", ex); throw new Exception("Something went wrong"); }catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<BPCInvoice> CreateInvoice(BPCInvoice Invoice)
        {
            try
            {
                Invoice.IsActive = true;
                Invoice.CreatedOn = DateTime.Now;
                var result = _dbContext.BPCInvoices.Add(Invoice);
                await _dbContext.SaveChangesAsync();
                return Invoice;
            }
            catch (SqlException ex){ WriteLog.WriteToFile(" ", ex); throw new Exception("Something went wrong");}catch(InvalidOperationException ex){WriteLog.WriteToFile(" ", ex); throw new Exception("Something went wrong"); }catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<BPCInvoice> UpdateInvoice(BPCInvoice Invoice)
        {
            try
            {
                var entity = _dbContext.Set<BPCInvoice>().FirstOrDefault(x => x.Client == Invoice.Client && x.Company == Invoice.Company && x.Type == Invoice.Type && x.PatnerID == Invoice.PatnerID && x.FiscalYear == Invoice.FiscalYear && x.InvoiceNo == Invoice.InvoiceNo);
                if (entity == null)
                {
                    return entity;
                }
                //_dbContext.Entry(Invoice).State = EntityState.Modified;
                entity.InvoiceDate = Invoice.InvoiceDate;
                entity.InvoiceAmount = Invoice.InvoiceAmount;
                entity.PoReference = Invoice.PoReference;
                entity.PaidAmount = Invoice.PaidAmount;
                entity.ModifiedBy = Invoice.ModifiedBy;
                entity.Currency = Invoice.Currency;
                entity.DateofPayment = Invoice.DateofPayment;
                entity.Status = Invoice.Status;
                entity.ModifiedBy = "SAP API";
                entity.ModifiedOn = DateTime.Now;
                await _dbContext.SaveChangesAsync();
                return Invoice;
            }
            catch (SqlException ex){ WriteLog.WriteToFile(" ", ex); throw new Exception("Something went wrong");}catch(InvalidOperationException ex){WriteLog.WriteToFile(" ", ex); throw new Exception("Something went wrong"); }catch (Exception ex)
            {
                throw ex;
            }
        }

        //mstrat
        public List<BPCInvoice> GetInvoiceByPartnerIdAnDocumentNo(string PatnerID)
        {
            try
            {
                var entity =  _dbContext.BPCInvoices.Where(x => x.PatnerID == PatnerID).ToList();
                return entity;
            }
            catch (SqlException ex){ WriteLog.WriteToFile(" ", ex); throw new Exception("Something went wrong");}catch(InvalidOperationException ex){WriteLog.WriteToFile(" ", ex); throw new Exception("Something went wrong"); }catch (Exception ex)
            {
                throw ex;
            }
        }
        //mend
        public List<BPCInvoice> GetAllInvoices()
        {
            try
            {
                var entity = _dbContext.BPCInvoices.ToList();
                return entity;
            }
            catch (SqlException ex){ WriteLog.WriteToFile(" ", ex); throw new Exception("Something went wrong");}catch(InvalidOperationException ex){WriteLog.WriteToFile(" ", ex); throw new Exception("Something went wrong"); }catch (Exception ex)
            {
                throw ex;
            }
        }



        #region Payment record
        
        public async Task CreatePaymentRecord(BPCPayRecord PayRecord)
        {
            try
            {
               
                    PayRecord.IsActive = true;
                    PayRecord.CreatedOn = DateTime.Now;
                    //List<BPCPayRecord> Records = GetAllPaymentRecord();
                    List<BPCPayRecord> Records = (from record in _dbContext.BPCPayRecords
                                                 orderby record.PayRecordNo descending
                                                 select record).ToList();
                    var LastPayRecNO = Convert.ToInt32(Records[0].PayRecordNo);
                    if (LastPayRecNO > 0)
                    {
                        LastPayRecNO++;
                        PayRecord.PayRecordNo = String.Format("{0:D4}", LastPayRecNO);

                    }
                    else
                    {
                        PayRecord.PayRecordNo = "0001";
                    }
                    var result = _dbContext.BPCPayRecords.Add(PayRecord);
                    await _dbContext.SaveChangesAsync();
                
                //return PayRecord;
            }
            catch (SqlException ex){ WriteLog.WriteToFile(" ", ex); throw new Exception("Something went wrong");}catch(InvalidOperationException ex){WriteLog.WriteToFile(" ", ex); throw new Exception("Something went wrong"); }catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<BPCPayRecord> UpdatePaymentRecord(BPCPayRecord PayRecord)
        {
            try
            {
                var entity = _dbContext.Set<BPCPayRecord>().FirstOrDefault(x => x.Client == PayRecord.Client && x.Company == PayRecord.Company && x.Type == PayRecord.Type && x.PartnerID == PayRecord.PartnerID && x.InvoiceNumber == PayRecord.InvoiceNumber && x.PayRecordNo == PayRecord.PayRecordNo);
                if (entity == null)
                {
                    return entity;
                }
                //_dbContext.Entry(Invoice).State = EntityState.Modified;
                entity.PaidAmount = PayRecord.PaidAmount;
                entity.PaymentDate = PayRecord.PaymentDate;
               // entity.PayRecordNo = PayRecord.PayRecordNo;
                entity.ModifiedOn = DateTime.Now;
                await _dbContext.SaveChangesAsync();
                return PayRecord;
            }
            catch (SqlException ex){ WriteLog.WriteToFile(" ", ex); throw new Exception("Something went wrong");}catch(InvalidOperationException ex){WriteLog.WriteToFile(" ", ex); throw new Exception("Something went wrong"); }catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<BPCPayRecord> GetAllPaymentRecord()
        {
            try
            {
                var entity = _dbContext.BPCPayRecords.ToList();
                return entity;
            }
            catch (SqlException ex){ WriteLog.WriteToFile(" ", ex); throw new Exception("Something went wrong");}catch(InvalidOperationException ex){WriteLog.WriteToFile(" ", ex); throw new Exception("Something went wrong"); }catch (Exception ex)
            {
                throw ex;
            }
        }
       
        public List<BPCPayRecord> GetAllRecordDateFilter()
        {
            try
            {
                List<BPCPayRecord> Records = (from record in _dbContext.BPCPayRecords
                                              orderby record.PaymentDate descending
                                              select record).ToList();
                return Records;
            }
            catch (SqlException ex){ WriteLog.WriteToFile(" ", ex); throw new Exception("Something went wrong");}catch(InvalidOperationException ex){WriteLog.WriteToFile(" ", ex); throw new Exception("Something went wrong"); }catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region inv payment


        public async Task<BPCInvoice> UpdateInvoiceList(List<BPCInvoice> InvoiceList)
        {
            try
            {
                BPCInvoice invoice = new BPCInvoice();
                foreach (BPCInvoice Invoice in InvoiceList)
                {
                    var entity = _dbContext.Set<BPCInvoice>().FirstOrDefault(x => x.Client == Invoice.Client && x.Company == Invoice.Company && x.Type == Invoice.Type && x.PatnerID == Invoice.PatnerID && x.FiscalYear == Invoice.FiscalYear && x.InvoiceNo == Invoice.InvoiceNo);
                    if (entity == null)
                    {
                        return entity;
                    }
                    //_dbContext.Entry(Invoice).State = EntityState.Modified;
                    entity.InvoiceDate = Invoice.InvoiceDate;
                    entity.InvoiceAmount = Invoice.InvoiceAmount;
                    entity.PoReference = Invoice.PoReference;
                    entity.PaidAmount = Invoice.PaidAmount;
                    entity.ModifiedBy = Invoice.ModifiedBy;
                    entity.Currency = Invoice.Currency;
                    entity.DateofPayment = Invoice.DateofPayment;
                    entity.Status = Invoice.Status;
                    entity.ModifiedOn = DateTime.Now;
                    await _dbContext.SaveChangesAsync();
                }
                return invoice;
            }
            catch (SqlException ex){ WriteLog.WriteToFile(" ", ex); throw new Exception("Something went wrong");}catch(InvalidOperationException ex){WriteLog.WriteToFile(" ", ex); throw new Exception("Something went wrong"); }catch (Exception ex)
            {
                throw ex;
            }
        }


        public async Task<BPCInvoice> UpdateInvoicePay(BPCInvoicePayView Invoice)
        {
            BPCInvoice bPCInvoice = new BPCInvoice();
            var strategy = _dbContext.Database.CreateExecutionStrategy();
            await strategy.ExecuteAsync(async () =>
            {
                using (var transaction = _dbContext.Database.BeginTransaction())
                {
                    try
                    {
                        if (Invoice.Invoices.Count != 0)
                        {
                            
                                await UpdateInvoiceList(Invoice.Invoices);
                            
                        }


                        if (Invoice.PayRecord.Count != 0)
                        {
                            foreach (BPCPayRecord item in Invoice.PayRecord)
                            {
                                await CreatePaymentRecord(item);
                            }

                        }

                        if (Invoice.PayPayment.Count != 0)
                        {
                           
                                await CreateInvPayment(Invoice.PayPayment);   

                        }
                        await _dbContext.SaveChangesAsync();
                        transaction.Commit();
                        transaction.Dispose();
                        //return entity;
                    }
                    catch (SqlException ex){ WriteLog.WriteToFile(" ", ex); throw new Exception("Something went wrong");}catch(InvalidOperationException ex){WriteLog.WriteToFile(" ", ex); throw new Exception("Something went wrong"); }catch (Exception ex)
                    {
                        transaction.Rollback();
                        transaction.Dispose();
                        throw ex;
                    }
                }
            });
            return bPCInvoice;
        }
      

        public async Task CreateInvPayment(List<BPCPayPayment> PaymentData)
        {
            try
            {
                foreach (BPCPayPayment paydata in PaymentData)
                {
                    //var data = _dbContext.BPCPayPayments.Where(x => x.PartnerID == paydata.PartnerID).ToList();
                    //if (data.Count==0)
                    //{

                    //}
                    _dbContext.BPCPayPayments.Where(x => x.PartnerID == paydata.PartnerID && x.DocumentNumber == paydata.DocumentNumber).ToList().ForEach(x => _dbContext.BPCPayPayments.Remove(x));

                    paydata.CreatedOn = DateTime.Now;
                    paydata.IsActive = true;
                    var result = _dbContext.BPCPayPayments.Add(paydata);
                      await _dbContext.SaveChangesAsync();

                }
            }
            catch (SqlException ex){ WriteLog.WriteToFile(" ", ex); throw new Exception("Something went wrong");}catch(InvalidOperationException ex){WriteLog.WriteToFile(" ", ex); throw new Exception("Something went wrong"); }catch (Exception ex)
            {
                throw ex;
            }
        }
        //public async Task UpdateInvPayment(List<BPCPayPayment> PaymentData)
        //{
        //    try
        //    {
        //        foreach (BPCPayPayment paydata in PaymentData)
        //        {

        //            paydata.ModifiedOn = DateTime.Now;

        //            //  await _dbContext.SaveChangesAsync();

        //        }
        //    }
        //    catch (SqlException ex){ WriteLog.WriteToFile(" ", ex); throw new Exception("Something went wrong");}catch(InvalidOperationException ex){WriteLog.WriteToFile(" ", ex); throw new Exception("Something went wrong"); }catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}
        #endregion

    }
}
