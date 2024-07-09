using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Numerics;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;
using BPCloud_VP_POService.DBContexts;
using BPCloud_VP_POService.Models;
using iTextSharp.text;
using iTextSharp.text.html;
using iTextSharp.text.pdf;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;


namespace BPCloud_VP_POService.Repositories
{
    public class PORepository : IPORepository
    {
        private readonly POContext _dbContext;
        AttachmentRepository attachmentRepository;
        IConfiguration _configuration;
        public PORepository(POContext dbContext, IConfiguration configuration)
        {
            _dbContext = dbContext;
            _configuration = configuration;
            attachmentRepository = new AttachmentRepository(_dbContext, _configuration);
        }

        //GRR table
        public List<BPCOFGRGI> GetAllGRR()
        {
            try
            {
                return _dbContext.BPCOFGRGIs.ToList();
            }
            catch (SqlException ex) { WriteLog.WriteToFile("PORespository/GetAllGRR", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("PORespository/GetAllGRR", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<BPCOFGRGI> FilterGRRListByPartnerID(string PartnerID, string GRGIDoc = null, string DocNumber = null, string Material = null, DateTime? GRIDate = null)
        {
            try
            {
                bool IsDocument = !string.IsNullOrEmpty(GRGIDoc);
                bool IsDocNumber = !string.IsNullOrEmpty(DocNumber);
                bool IsMaterial = !string.IsNullOrEmpty(Material);
                bool IsDate = GRIDate.HasValue;
                var result = (from tb in _dbContext.BPCOFGRGIs
                              where tb.PatnerID == PartnerID &&
                             (!IsDocument || tb.GRGIDoc == GRGIDoc) && (!IsDocNumber || tb.DocNumber == DocNumber) &&
                             (!IsMaterial || tb.Material == Material)
                             && (!IsDate || (tb.GRIDate.HasValue && tb.GRIDate.Value.Date >= GRIDate.Value.Date))
                              orderby tb.GRGIDoc
                              select new BPCOFGRGI()
                              {
                                  Client = tb.Client,
                                  Company = tb.Company,
                                  Type = tb.Type,
                                  PatnerID = tb.PatnerID,
                                  DocNumber = tb.DocNumber,
                                  GRGIDoc = tb.GRGIDoc,
                                  Item = tb.Item,
                                  Description = tb.MaterialText,
                                  Material = tb.Material,
                                  MaterialText = tb.MaterialText,
                                  DeliveryDate = tb.DeliveryDate,
                                  GRIDate = tb.GRIDate,
                                  GRGIQty = tb.GRGIQty,
                                  ShippingPartner = tb.ShippingPartner,
                                  ShippingDoc = tb.ShippingDoc
                              }).ToList();
                return result;

            }
            catch (SqlException ex) { WriteLog.WriteToFile("PORespository/FilterGRRListByPartnerID", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("PORespository/FilterGRRListByPartnerID", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<BPCOFGRGI> GetOfSuperUsergrnDetails(string GetPlantByUser, string GRGIDoc = null, string DocNumber = null, string Material = null, DateTime? GRIDate = null)
        {
            try
            {
                bool IsDocument = !string.IsNullOrEmpty(GRGIDoc);
                bool IsDocNumber = !string.IsNullOrEmpty(DocNumber);
                bool IsMaterial = !string.IsNullOrEmpty(Material);
                bool IsDate = GRIDate.HasValue;
                string[] Plants = GetPlantByUser.Split(',');
                for (int i = 0; i < Plants.Length; i++)
                {
                    Plants[i] = Plants[i].Trim();
                }
                List<BPCOFGRGI> grnlist = new List<BPCOFGRGI>();
                List<BPCOFGRGI> grnlists = new List<BPCOFGRGI>();
                for (int i = 0; i < Plants.Length; i++)
                {
                    grnlist = (from tb in _dbContext.BPCOFGRGIs
                                  where tb.Plant == Plants[i] &&
                                 (!IsDocument || tb.GRGIDoc == GRGIDoc) && (!IsDocNumber || tb.DocNumber == DocNumber) &&
                                 (!IsMaterial || tb.Material == Material)
                                 && (!IsDate || (tb.GRIDate.HasValue && tb.GRIDate.Value.Date >= GRIDate.Value.Date))
                                  orderby tb.GRGIDoc
                                  select new BPCOFGRGI()
                                  {
                                      Client = tb.Client,
                                      Company = tb.Company,
                                      Type = tb.Type,
                                      PatnerID = tb.PatnerID,
                                      DocNumber = tb.DocNumber,
                                      GRGIDoc = tb.GRGIDoc,
                                      Item = tb.Item,
                                      Description = tb.MaterialText,
                                      Material = tb.Material,
                                      MaterialText = tb.MaterialText,
                                      DeliveryDate = tb.DeliveryDate,
                                      GRIDate = tb.GRIDate,
                                      GRGIQty = tb.GRGIQty,
                                      ShippingPartner = tb.ShippingPartner,
                                      ShippingDoc = tb.ShippingDoc
                                  }).ToList();
                    grnlists.AddRange(grnlist);
                }
                return grnlists;

            }
            catch (SqlException ex) { WriteLog.WriteToFile("PORespository/FilterGRRListByPartnerID", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("PORespository/FilterGRRListByPartnerID", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<BPCOFGRGI> FilterGRGIListByPlants(GRNListFilter filter)
        {
            try
            {
                bool IsGRN = !string.IsNullOrEmpty(filter.GRN);
                bool IsMaterial = !string.IsNullOrEmpty(filter.Material);
                bool IsFromDate = filter.FromDate.HasValue;
                bool IsToDate = filter.ToDate.HasValue;
                var result = (from tb in _dbContext.BPCOFGRGIs
                              where (filter.Plants.Any(x => x == tb.Plant)) &&
                              (!IsGRN || tb.GRGIDoc == filter.GRN) && (!IsMaterial || tb.Material == filter.Material)
                              && (!IsFromDate || (tb.GRIDate.HasValue && tb.GRIDate.Value.Date >= filter.ToDate.Value.Date))
                              orderby tb.GRGIDoc
                              select new BPCOFGRGI()
                              {
                                  Client = tb.Client,
                                  Company = tb.Company,
                                  Type = tb.Type,
                                  PatnerID = tb.PatnerID,
                                  DocNumber = tb.DocNumber,
                                  GRGIDoc = tb.GRGIDoc,
                                  Item = tb.Item,
                                  Description = tb.MaterialText,
                                  Material = tb.Material,
                                  MaterialText = tb.MaterialText,
                                  DeliveryDate = tb.DeliveryDate,
                                  GRIDate = tb.GRIDate,
                                  GRGIQty = tb.GRGIQty,
                                  ShippingPartner = tb.ShippingPartner,
                                  ShippingDoc = tb.ShippingDoc
                              }).ToList();
                return result;

            }
            catch (SqlException ex) { WriteLog.WriteToFile("PORespository/FilterGRGIListByPlants", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("PORespository/FilterGRGIListByPlants", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<BPCOFGRGI> FilterGRRListForBuyer(string GRGIDoc = null, string DocNumber = null, string Material = null, DateTime? ASNFromDate = null, DateTime? ASNToDate = null)
        {
            try
            {
                bool IsDocument = !string.IsNullOrEmpty(GRGIDoc);
                bool IsDocNumber = !string.IsNullOrEmpty(DocNumber);
                bool IsMaterial = !string.IsNullOrEmpty(Material);
                bool IsFromDate = ASNFromDate.HasValue;
                bool IsToDate = ASNFromDate.HasValue;
                var result = (from tb in _dbContext.BPCOFGRGIs
                              where (!IsDocument || tb.GRGIDoc == GRGIDoc) && (!IsDocNumber || tb.DocNumber == DocNumber) &&
                                    (!IsMaterial || tb.Material == Material)
                                    && (!IsFromDate || (tb.GRIDate.HasValue && tb.GRIDate.Value.Date >= ASNFromDate.Value.Date))
                                    && (!IsToDate || (tb.GRIDate.HasValue && tb.GRIDate.Value.Date <= ASNToDate.Value.Date))
                              orderby tb.GRGIDoc
                              select new BPCOFGRGI()
                              {
                                  Client = tb.Client,
                                  Company = tb.Company,
                                  Type = tb.Type,
                                  PatnerID = tb.PatnerID,
                                  DocNumber = tb.DocNumber,
                                  GRGIDoc = tb.GRGIDoc,
                                  Item = tb.Item,
                                  Description = tb.MaterialText,
                                  Material = tb.Material,
                                  MaterialText = tb.MaterialText,
                                  DeliveryDate = tb.DeliveryDate,
                                  GRIDate = tb.GRIDate,
                                  GRGIQty = tb.GRGIQty,
                                  ShippingPartner = tb.ShippingPartner,
                                  ShippingDoc = tb.ShippingDoc
                              }).ToList();
                return result;

            }
            catch (SqlException ex) { WriteLog.WriteToFile("PORespository/FilterGRRListForBuyer", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("PORespository/FilterGRRListForBuyer", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<BPCOFHeader> GetAllPOList()
        {
            try
            {
                return _dbContext.BPCOFHeaders.ToList();
            }
            catch (SqlException ex) { WriteLog.WriteToFile("PORespository/GetAllPOList", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("PORespository/GetAllPOList", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<BPCOFHeader> FilterPOList(string VendorCode, string PONumber = null, string Status = null, DateTime? FromDate = null, DateTime? ToDate = null)
        {
            try
            {
                bool IsVendorCode = !string.IsNullOrEmpty(VendorCode);
                bool IsPONumber = !string.IsNullOrEmpty(PONumber);
                bool IsStatus = !string.IsNullOrEmpty(Status);
                bool IsFromDate = FromDate.HasValue;
                bool IsToDate = ToDate.HasValue;
                var result = (from tb in _dbContext.BPCOFHeaders
                              where tb.Type == "V" &&
                              (!IsVendorCode || tb.PatnerID == VendorCode) && (!IsPONumber || tb.DocNumber == PONumber)
                              && (!IsStatus || tb.Status.ToLower() == Status.ToLower())
                              && (!IsFromDate || (tb.DocDate.HasValue && tb.DocDate.Value.Date >= FromDate.Value.Date))
                              && (!IsToDate || (tb.DocDate.HasValue && tb.DocDate.Value.Date <= ToDate.Value.Date))
                              //orderby tb.DocNumber
                              select tb).ToList();
                return result;
            }
            catch (SqlException ex) { WriteLog.WriteToFile("PORespository/FilterPOList", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("PORespository/FilterPOList", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public BPCOFHeader GetPOByDoc(string DocNumber)
        {
            try
            {
                return _dbContext.BPCOFHeaders.FirstOrDefault(x => x.DocNumber == DocNumber);
            }
            catch (SqlException ex) { WriteLog.WriteToFile("PORespository/GetPOByDoc", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("PORespository/GetPOByDoc", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public BPCOFHeader GetPOByDocAndPartnerID(string DocNumber, string PartnerID)
        {
            try
            {
                var po = _dbContext.BPCOFHeaders.FirstOrDefault(x => x.DocNumber == DocNumber && x.PatnerID == PartnerID);
                var items = _dbContext.BPCOFItems.Where(x => x.DocNumber == DocNumber && x.PatnerID == PartnerID).ToList();
                if (po != null && items.Count > 0)
                {
                    po.Plant = items[0].PlantCode;
                    _dbContext.SaveChanges();
                }
                return po;
            }
            catch (SqlException ex) { WriteLog.WriteToFile("PORespository/GetPOByDocAndPartnerID", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("PORespository/GetPOByDocAndPartnerID", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<BPCOFHeader> GetOFByDocAndPartnerID(BPCOFItemViewFilter filter)
        {
            try
            {
                //var po = _dbContext.BPCOFHeaders.Where(x => filter.DocNumbers.Any(y => y == x.DocNumber) && x.PatnerID == filter.PatnerID).ToList();
                
                var po = _dbContext.BPCOFHeaders.Where(x => filter.DocNumbers.Any(y => y == x.DocNumber)).ToList();
                //var items = _dbContext.BPCOFItems.Where(x => x.DocNumber == DocNumber && x.PatnerID == PartnerID).ToList();
                //if (po != null && items.Count > 0)
                //{
                //    po.Plant = items[0].PlantCode;
                //    _dbContext.SaveChanges();
                //}
                return po;
            }
            catch (SqlException ex) { WriteLog.WriteToFile("PORespository/GetOFByDocAndPartnerID", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("PORespository/GetOFByDocAndPartnerID", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<BPCOFItem> GetPOItemsByDoc(string DocNumber)
        {
            try
            {
                return _dbContext.BPCOFItems.Where(x => x.DocNumber == DocNumber).ToList();
            }
            catch (SqlException ex) { WriteLog.WriteToFile("PORespository/GetPOItemsByDoc", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("PORespository/GetPOItemsByDoc", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<BPCOFItemSES> GetOFItemSES()
        {
            try
            {
                return _dbContext.BPCOFItemSESes.ToList();
            }
            catch (SqlException ex) { WriteLog.WriteToFile("PORespository/GetOFItemSES", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("PORespository/GetOFItemSES", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<BPCOFItemSES> GetOFItemSESByItem(string item, string DocumentNo, string PartnerId)
        {
            try
            {
                return _dbContext.BPCOFItemSESes.Where(table => table.Item == item && table.DocNumber == DocumentNo && table.PatnerID == PartnerId).ToList();
            }
            catch (SqlException ex) { WriteLog.WriteToFile("PORespository/GetOFItemSESByItem", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("PORespository/GetOFItemSESByItem", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<BPCOFItemSES> GetOFItemSESByDocNumber(string item, string DocumentNo)
        {
            try
            {
                return _dbContext.BPCOFItemSESes.Where(table => table.Item == item && table.DocNumber == DocumentNo).ToList();
            }
            catch (SqlException ex) { WriteLog.WriteToFile("PORespository/GetOFItemSESByItem", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("PORespository/GetOFItemSESByItem", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        
        public List<BPCOFItem> GetSupportPOItemsByDoc(string DocNumber)
        {
            try
            {
                var result = _dbContext.BPCOFItems.Where(x => x.DocNumber == DocNumber).ToList();
                result.ForEach(y =>
                {
                    y.DeliveryDate = y.DeliveryDate.HasValue ? y.DeliveryDate.Value.Date : y.DeliveryDate;
                    y.AckDeliveryDate = y.AckDeliveryDate.HasValue ? y.AckDeliveryDate.Value.Date : y.AckDeliveryDate;
                });
                return result;
            }
            catch (SqlException ex) { WriteLog.WriteToFile("PORespository/GetSupportPOItemsByDoc", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("PORespository/GetSupportPOItemsByDoc", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<BPCOFItem> GetPOItemsByDocAndPartnerID(string DocNumber, string PartnerID)
        {
            try
            {


                List<BPCOFItem> items = _dbContext.BPCOFItems.Where(x => x.DocNumber == DocNumber && x.PatnerID == PartnerID).ToList();
                foreach (BPCOFItem item in items)
                {
                    List<BPCOFScheduleLine> sls = _dbContext.BPCOFScheduleLines.Where(x => x.DocNumber == item.DocNumber && x.Item == item.Item).ToList();
                    if (sls.Count > 0)
                    {
                        item.OrderedQty = 0;
                        foreach (BPCOFScheduleLine sl in sls)
                        {
                            item.OrderedQty += sl.OrderedQty;
                        }
                    }
                    item.OpenQty = item.OrderedQty - item.TransitQty - item.CompletedQty;

                }
                return items;
            }
            catch (SqlException ex) { WriteLog.WriteToFile("PORespository/GetPOItemsByDocAndPartnerID", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("PORespository/GetPOItemsByDocAndPartnerID", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<BPCOFItemView> GetPOItemViewsByDocAndPartnerID(string DocNumber, string PartnerID)
        {
            try
            {
                List<BPCOFItemView> itemss = new List<BPCOFItemView>();
                List<BPCOFItem> items = _dbContext.BPCOFItems.Where(x => x.DocNumber == DocNumber && x.PatnerID == PartnerID).ToList();
                foreach (BPCOFItem item in items)
                {
                    List<BPCOFScheduleLine> sls = _dbContext.BPCOFScheduleLines.Where(x => x.DocNumber == item.DocNumber && x.Item == item.Item).ToList();
                    if (sls.Count > 0)
                    {
                        item.OrderedQty = 0;
                        foreach (BPCOFScheduleLine sl in sls)
                        {
                            item.OrderedQty += sl.OrderedQty;
                        }
                    }
                    item.OpenQty = item.OrderedQty - item.TransitQty - item.CompletedQty;

                }
                items.ForEach(x =>
                {
                    BPCOFItemView itemView = new BPCOFItemView();
                    itemView.Client = x.Client;
                    itemView.Company = x.Company;
                    itemView.Type = x.Type;
                    itemView.PatnerID = x.PatnerID;
                    itemView.DocNumber = x.DocNumber;
                    itemView.Item = x.Item;
                    itemView.Material = x.Material;
                    itemView.MaterialText = x.MaterialText;
                    itemView.MaterialType = x.MaterialType;
                    itemView.UnitPrice = x.UnitPrice;
                    itemView.DeliveryDate = x.DeliveryDate;
                    itemView.HSN = x.HSN;
                    itemView.OrderedQty = x.OrderedQty;
                    itemView.TransitQty = x.TransitQty;
                    itemView.OpenQty = x.OpenQty;
                    itemView.CompletedQty = x.CompletedQty;
                    itemView.UOM = x.UOM;
                    itemView.PlantCode = x.PlantCode;
                    itemView.TaxAmount = x.TaxAmount;
                    itemView.TaxCode = x.TaxCode;
                    itemView.IsFreightAvailable = x.IsFreightAvailable;
                    itemView.FreightAmount = x.FreightAmount;
                    itemView.ToleranceUpperLimit = x.ToleranceUpperLimit;
                    itemView.ToleranceUpperLimit = x.ToleranceUpperLimit;
                    itemView.Value = x.Value;
                    itemView.AckStatus = x.AckStatus;
                    itemView.AckDeliveryDate = x.AckDeliveryDate;
                    itemView.BPCOFItemSESes = _dbContext.BPCOFItemSESes.Where(y => y.Client == x.Client && y.Company == x.Company && y.Type == x.Type && y.PatnerID == x.PatnerID &&
                                                                              y.DocNumber == x.DocNumber && y.Item == x.Item).ToList();
                    itemss.Add(itemView);
                });
                return itemss;
            }
            catch (SqlException ex) { WriteLog.WriteToFile("PORespository/GetPOItemViewsByDocAndPartnerID", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("PORespository/GetPOItemViewsByDocAndPartnerID", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<BPCOFItemView> GetPOItemViewsByDoc(string DocNumber)
        {
            try
            {
                List<BPCOFItemView> itemss = new List<BPCOFItemView>();
                List<BPCOFItem> items = _dbContext.BPCOFItems.Where(x => x.DocNumber == DocNumber).ToList();
                foreach (BPCOFItem item in items)
                {
                    List<BPCOFScheduleLine> sls = _dbContext.BPCOFScheduleLines.Where(x => x.DocNumber == item.DocNumber && x.Item == item.Item).ToList();
                    if (sls.Count > 0)
                    {
                        item.OrderedQty = 0;
                        foreach (BPCOFScheduleLine sl in sls)
                        {
                            item.OrderedQty += sl.OrderedQty;
                        }
                    }
                    item.OpenQty = item.OrderedQty - item.TransitQty - item.CompletedQty;

                }
                items.ForEach(x =>
                {
                    BPCOFItemView itemView = new BPCOFItemView();
                    itemView.Client = x.Client;
                    itemView.Company = x.Company;
                    itemView.Type = x.Type;
                    itemView.PatnerID = x.PatnerID;
                    itemView.DocNumber = x.DocNumber;
                    itemView.Item = x.Item;
                    itemView.Material = x.Material;
                    itemView.MaterialText = x.MaterialText;
                    itemView.UnitPrice = x.UnitPrice;
                    itemView.DeliveryDate = x.DeliveryDate;
                    itemView.HSN = x.HSN;
                    itemView.OrderedQty = x.OrderedQty;
                    itemView.TransitQty = x.TransitQty;
                    itemView.OpenQty = x.OpenQty;
                    itemView.CompletedQty = x.CompletedQty;
                    itemView.UOM = x.UOM;
                    itemView.PlantCode = x.PlantCode;
                    itemView.TaxAmount = x.TaxAmount;
                    itemView.TaxCode = x.TaxCode;
                    itemView.IsFreightAvailable = x.IsFreightAvailable;
                    itemView.FreightAmount = x.FreightAmount;
                    itemView.ToleranceUpperLimit = x.ToleranceUpperLimit;
                    itemView.ToleranceUpperLimit = x.ToleranceUpperLimit;
                    itemView.Value = x.Value;
                    itemView.AckStatus = x.AckStatus;
                    itemView.AckDeliveryDate = x.AckDeliveryDate;
                    itemView.BPCOFItemSESes = _dbContext.BPCOFItemSESes.Where(y => y.Client == x.Client && y.Company == x.Company && y.Type == x.Type && y.PatnerID == x.PatnerID &&
                                                                              y.DocNumber == x.DocNumber && y.Item == x.Item).ToList();
                    itemss.Add(itemView);
                });
                return itemss;
            }
            catch (SqlException ex) { WriteLog.WriteToFile("PORespository/GetPOItemViewsByDocAndPartnerID", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("PORespository/GetPOItemViewsByDocAndPartnerID", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        
        public List<BPCOFItemView> GetOFItemViewsForASNByDocAndPartnerID(BPCOFItemViewFilter filter)
        {
            try
            {
                List<BPCOFItemView> itemss = new List<BPCOFItemView>();
                //List<BPCOFItem> items = _dbContext.BPCOFItems.Where(x => filter.DocNumbers.Any(y => y == x.DocNumber) && x.PatnerID == filter.PatnerID).ToList();
                List<BPCOFItem> items = _dbContext.BPCOFItems.Where(x => filter.DocNumbers.Any(y => y == x.DocNumber)).ToList();
                foreach (BPCOFItem item in items)
                {
                    List<BPCOFScheduleLine> sls = _dbContext.BPCOFScheduleLines.Where(x => x.DocNumber == item.DocNumber && x.Item == item.Item).ToList();
                    if (sls.Count > 0)
                    {
                        item.OrderedQty = 0;
                        foreach (BPCOFScheduleLine sl in sls)
                        {
                            item.OrderedQty += sl.OrderedQty;
                        }
                    }
                    item.OpenQty = item.OrderedQty - item.TransitQty - item.CompletedQty;

                }
                items.ForEach(x =>
                {
                    BPCOFItemView itemView = new BPCOFItemView();
                    itemView.Client = x.Client;
                    itemView.Company = x.Company;
                    itemView.Type = x.Type;
                    itemView.PatnerID = x.PatnerID;
                    itemView.DocNumber = x.DocNumber;
                    itemView.Item = x.Item;
                    itemView.Material = x.Material;
                    itemView.MaterialText = x.MaterialText;
                   itemView.MaterialType = x.MaterialType;
                    itemView.UnitPrice = x.UnitPrice;
                    itemView.DeliveryDate = x.DeliveryDate;
                    itemView.HSN = x.HSN;
                    itemView.OrderedQty = x.OrderedQty;
                    itemView.TransitQty = x.TransitQty;
                    itemView.OpenQty = x.OpenQty;
                    itemView.CompletedQty = x.CompletedQty;
                    itemView.UOM = x.UOM;
                    itemView.PlantCode = x.PlantCode;
                    itemView.TaxAmount = x.TaxAmount;
                    itemView.TaxCode = x.TaxCode;
                    itemView.IsFreightAvailable = x.IsFreightAvailable;
                    itemView.FreightAmount = x.FreightAmount;
                    itemView.ToleranceUpperLimit = x.ToleranceUpperLimit;
                    itemView.ToleranceLowerLimit = x.ToleranceLowerLimit;
                    itemView.Value = x.Value;
                    itemView.AckStatus = x.AckStatus;
                    itemView.AckDeliveryDate = x.AckDeliveryDate;
                    //itemView.BPCOFItemSESes = _dbContext.BPCOFItemSESes.Where(y => y.Client == x.Client && y.Company == x.Company && y.Type == x.Type && y.PatnerID == x.PatnerID &&
                                                                              //y.DocNumber == x.DocNumber && y.Item == x.Item).ToList();
                    itemView.BPCOFItemSESes = _dbContext.BPCOFItemSESes.Where(y => y.Client == x.Client && y.Company == x.Company && y.Type == x.Type &&
                                                                              y.DocNumber == x.DocNumber && y.Item == x.Item).ToList();
                    itemss.Add(itemView);
                });
                return itemss;
            }
            catch (SqlException ex) { WriteLog.WriteToFile("PORespository/GetPOItemViewsByDocAndPartnerID", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("PORespository/GetPOItemViewsByDocAndPartnerID", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<BPCOFGRGI> GetPOGRGIByDocAndPartnerID(string DocNumber, string PartnerID)
        {
            try
            {
                List<BPCOFGRGI> items = _dbContext.BPCOFGRGIs.Where(x => x.DocNumber == DocNumber && x.PatnerID == PartnerID).ToList();
                return items;
            }
            catch (SqlException ex) { WriteLog.WriteToFile("PORespository/GetPOGRGIByDocAndPartnerID", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("PORespository/GetPOGRGIByDocAndPartnerID", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public SOItemCount GetSOItemCountByDocAndPartnerID(string DocNumber, string PartnerID)
        {
            try
            {
                SOItemCount sOItemCount = new SOItemCount();
                sOItemCount.DocNumber = DocNumber;
                sOItemCount.PatnerID = PartnerID;
                sOItemCount.ItemCount = (from tb in _dbContext.BPCOFItems
                                         where tb.DocNumber == DocNumber && tb.PatnerID == PartnerID
                                         select new { tb.Client, tb.Company, tb.Type, tb.PatnerID, tb.DocNumber, tb.Item }).Count();
                sOItemCount.GRGICount = (from tb in _dbContext.BPCOFGRGIs
                                         where tb.DocNumber == DocNumber && tb.PatnerID == PartnerID
                                         select new { tb.Client, tb.Company, tb.Type, tb.PatnerID, tb.DocNumber, tb.GRGIDoc, tb.Item }).Count();
                sOItemCount.PODCount = (from tb in _dbContext.BPCPODItems
                                        where tb.DocNumber == DocNumber && tb.PatnerID == PartnerID
                                        select tb).Count();
                sOItemCount.InvCount = (from tb in _dbContext.BPCInvoices
                                        where tb.PatnerID == PartnerID
                                        select tb).Count();
                sOItemCount.ReturnCount = (from tb in _dbContext.BPCRetItems
                                           where tb.PatnerID == PartnerID
                                           select tb).Count();
                sOItemCount.DocumentCount = (from tb in _dbContext.BPCAttachments
                                             where tb.ReferenceNo == DocNumber && tb.PatnerID == PartnerID
                                             select tb.AttachmentID).Count();

                return sOItemCount;
            }
            catch (SqlException ex) { WriteLog.WriteToFile("PORespository/GetSOItemCountByDocAndPartnerID", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("PORespository/GetSOItemCountByDocAndPartnerID", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        //mend
        public List<POScheduleLineView> GetPOSLByDocAndPartnerID(string DocNumber, string PartnerID)
        {
            try
            {
                //return _dbContext.BPCOFItems.Where(x => x.DocNumber == DocNumber && x.PatnerID == PartnerID).ToList();
                var result = (from tb in _dbContext.BPCOFItems
                              join tb1 in _dbContext.BPCOFScheduleLines on tb.Item equals tb1.Item
                              where tb.DocNumber == DocNumber && tb1.DocNumber == DocNumber &&
                              tb.PatnerID == PartnerID && tb1.PatnerID == PartnerID
                              select new POScheduleLineView()
                              {
                                  Client = tb.Client,
                                  Company = tb.Company,
                                  Type = tb.Type,
                                  PatnerID = tb.PatnerID,
                                  DocNumber = tb.DocNumber,
                                  Item = tb.Item,
                                  Material = tb.Material,
                                  MaterialText = tb.MaterialText,
                                  SlLine = tb1.SlLine,
                                  DeliveryDate = tb1.DeliveryDate,
                                  OrderedQty = tb1.OrderedQty
                              }).ToList();
                return result;
            }
            catch (SqlException ex) { WriteLog.WriteToFile("PORespository/GetPOSLByDocAndPartnerID", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("PORespository/GetPOSLByDocAndPartnerID", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public byte[] CreatePOPdf(string DocNumber)
        {
            try
            {
                BPCOFHeader header = _dbContext.BPCOFHeaders.Where(x => x.DocNumber == DocNumber).FirstOrDefault();
                List<BPCOFItem> asnItems = _dbContext.BPCOFItems.Where(x => x.DocNumber == DocNumber).ToList();
                int asnItemsCount = asnItems.Count;
                List<BPCASNPack> asnPacks = _dbContext.BPCASNPacks.Where(x => x.ASNNumber == DocNumber).ToList();

                var headerFont = FontFactory.GetFont("Arial", 15, Font.BOLD);
                var titleFont = FontFactory.GetFont("Arial", 10, Font.BOLD);
                var titleBlueFont = FontFactory.GetFont("Arial", 10, Font.NORMAL, BaseColor.BLUE);
                var boldFont = FontFactory.GetFont("Arial", 8, Font.BOLD);
                var boldBlueFont = FontFactory.GetFont("Arial", 8, Font.BOLD, BaseColor.BLUE);
                var normalFont = FontFactory.GetFont("Arial", 8, Font.NORMAL);
                var normalBlueFont = FontFactory.GetFont("Arial", 8, Font.NORMAL, BaseColor.BLUE);

                using (System.IO.MemoryStream memoryStream = new System.IO.MemoryStream())
                {
                    Document document = new Document(PageSize.A4, 10, 10, 10, 10);
                    PdfWriter writer = PdfWriter.GetInstance(document, memoryStream);
                    PdfContentByte cb = new PdfContentByte(writer);
                    document.Open();

                    //Header table
                    //PdfPTable headerTable = new PdfPTable(2) { WidthPercentage = 100 };
                    //headerTable.SetWidths(new float[] { 5f, 95f });
                    //string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Images");
                    //iTextSharp.text.Image img1 = iTextSharp.text.Image.GetInstance(path + "/truck.png");
                    //img1.ScaleAbsolute(15f, 12f);
                    //PdfPCell Imgcell = new PdfPCell(img1);
                    //Imgcell.Padding = 5f;
                    //Imgcell.PaddingBottom = 12f;
                    //Imgcell.Border = Rectangle.NO_BORDER;
                    //Imgcell.VerticalAlignment = Element.ALIGN_BOTTOM;
                    //headerTable.AddCell(Imgcell);
                    //PdfPCell headercell = new PdfPCell(new Phrase("Active ASNs", headerFont));
                    //headercell.Padding = 5f;
                    //headercell.PaddingBottom = 14f;
                    //headercell.Border = Rectangle.NO_BORDER;
                    //headercell.VerticalAlignment = Element.ALIGN_BOTTOM;
                    //headerTable.AddCell(headercell);

                    //document.Add(headerTable);

                    //ASN Table
                    //PdfPTable ASNTable = new PdfPTable(7) { WidthPercentage = 100 };

                    //PdfPCell cell = new PdfPCell(new Phrase("ASN Number", titleBlueFont));
                    //cell.Padding = 5f;
                    //ASNTable.AddCell(cell);
                    //cell = new PdfPCell(new Phrase("Vendor", titleFont));
                    //cell.Padding = 5f;
                    //ASNTable.AddCell(cell);
                    //cell = new PdfPCell(new Phrase("Mode", titleFont));
                    //cell.Padding = 5f;
                    //ASNTable.AddCell(cell);
                    //cell = new PdfPCell(new Phrase("Item", titleFont));
                    //cell.Padding = 5f;
                    //ASNTable.AddCell(cell);
                    //cell = new PdfPCell(new Phrase("Material", titleFont));
                    //cell.Padding = 5f;
                    //ASNTable.AddCell(cell);
                    //cell = new PdfPCell(new Phrase("Material Text", titleFont));
                    //cell.Padding = 5f;
                    //ASNTable.AddCell(cell);
                    //cell = new PdfPCell(new Phrase("Created Date", titleFont));
                    //cell.Padding = 5f;
                    //ASNTable.AddCell(cell);

                    //cell = new PdfPCell(new Phrase(header.ASNNumber, normalBlueFont));
                    //cell.Rowspan = asnItemsCount;
                    //cell.Border = Rectangle.BOTTOM_BORDER | Rectangle.LEFT_BORDER;
                    //cell.Padding = 5f;
                    //ASNTable.AddCell(cell);
                    //cell = new PdfPCell(new Phrase(header.TransporterName, normalFont));
                    //cell.Rowspan = asnItemsCount;
                    //cell.Border = Rectangle.BOTTOM_BORDER;
                    //cell.Padding = 5f;
                    //ASNTable.AddCell(cell);
                    //cell = new PdfPCell(new Phrase(header.TransportMode, normalFont));
                    //cell.Rowspan = asnItemsCount;
                    //cell.Border = Rectangle.BOTTOM_BORDER;
                    //cell.Padding = 5f;
                    //ASNTable.AddCell(cell);
                    PdfPCell cell = new PdfPCell();


                    //POHeader table
                    PdfPTable POHeaderTable = new PdfPTable(3) { WidthPercentage = 100 };
                    POHeaderTable.SetWidths(new float[] { 40f, 30f, 30f });

                    ////Barcode table
                    //PdfPTable BarcodeTable = new PdfPTable(1);
                    //cell = new PdfPCell(new Phrase("Shipment Number", titleFont));
                    //cell.Border = Rectangle.NO_BORDER;
                    //cell.HorizontalAlignment = Element.ALIGN_CENTER;
                    //BarcodeTable.AddCell(cell);

                    //cell = new PdfPCell(new Phrase("1234", normalFont));
                    //cell.Border = Rectangle.NO_BORDER;
                    //cell.HorizontalAlignment = Element.ALIGN_CENTER;
                    //BarcodeTable.AddCell(cell);

                    //Barcode128 code128 = new Barcode128();
                    //code128.CodeType = Barcode.CODE128;
                    //code128.BarHeight = 18f;
                    //code128.ChecksumText = false;
                    //code128.GenerateChecksum = false;
                    //code128.StartStopText = true;
                    //code128.Code = "ABCD";
                    //code128.Font = null;
                    //// Generate barcode image
                    //iTextSharp.text.Image image128 = code128.CreateImageWithBarcode(cb, null, null);
                    //image128.Border = Rectangle.NO_BORDER;
                    //// Add image to table cell
                    //BarcodeTable.AddCell(image128);
                    //POHeaderTable.AddCell(BarcodeTable);

                    //QRCode table
                    //PdfPTable QRCodeTable = new PdfPTable(1);
                    //cell = new PdfPCell(new Phrase("PO Number", titleFont));
                    //cell.Border = Rectangle.NO_BORDER;
                    //cell.HorizontalAlignment = Element.ALIGN_CENTER;
                    //QRCodeTable.AddCell(cell);

                    //cell = new PdfPCell(new Phrase(header.DocNumber, normalFont));
                    //cell.Border = Rectangle.NO_BORDER;
                    //cell.HorizontalAlignment = Element.ALIGN_CENTER;
                    //QRCodeTable.AddCell(cell);

                    //cell = new PdfPCell(new Phrase("Date", titleFont));
                    //cell.Border = Rectangle.NO_BORDER;
                    //cell.HorizontalAlignment = Element.ALIGN_CENTER;
                    //QRCodeTable.AddCell(cell);

                    //cell = new PdfPCell(new Phrase(header.DocDate.HasValue? header.DocDate.Value.ToString("dd/MM/yyyy"):"", normalFont));
                    //cell.Border = Rectangle.NO_BORDER;
                    //cell.HorizontalAlignment = Element.ALIGN_CENTER;
                    //QRCodeTable.AddCell(cell);



                    //ASN details
                    PdfPTable ASNDetailTable = new PdfPTable(1);
                    cell = new PdfPCell(new Phrase($"PO Number : {header.DocNumber}", normalFont));
                    cell.Border = Rectangle.NO_BORDER;
                    ASNDetailTable.AddCell(cell);
                    var dt = header.DocDate.HasValue ? header.DocDate.Value.ToString("dd/MM/yyyy") : "";
                    cell = new PdfPCell(new Phrase($"Date : {dt}", normalFont));
                    cell.Border = Rectangle.NO_BORDER;
                    ASNDetailTable.AddCell(cell);

                    cell = new PdfPCell(new Phrase($"Version : {header.DocVersion}", normalFont));
                    cell.Border = Rectangle.NO_BORDER;
                    ASNDetailTable.AddCell(cell);

                    cell = new PdfPCell(new Phrase($"Currency : {header.Currency}", normalFont));
                    cell.Border = Rectangle.NO_BORDER;
                    ASNDetailTable.AddCell(cell);
                    POHeaderTable.AddCell(ASNDetailTable);

                    //From Address
                    PdfPTable FromAddressTable = new PdfPTable(1);
                    cell = new PdfPCell(new Phrase("From", titleFont));
                    cell.Border = Rectangle.NO_BORDER;
                    FromAddressTable.AddCell(cell);

                    cell = new PdfPCell(new Phrase("Exalca", normalFont));
                    cell.Border = Rectangle.NO_BORDER;
                    FromAddressTable.AddCell(cell);

                    cell = new PdfPCell(new Phrase("Bangalore", normalFont));
                    cell.Border = Rectangle.NO_BORDER;
                    FromAddressTable.AddCell(cell);

                    cell = new PdfPCell(new Phrase("Karnataka", normalFont));
                    cell.Border = Rectangle.NO_BORDER;
                    FromAddressTable.AddCell(cell);
                    POHeaderTable.AddCell(FromAddressTable);

                    //To address
                    if (asnItemsCount > 0)
                    {
                        var PlantCode = asnItems[0].PlantCode;
                        var Plant = _dbContext.BPCPlantMasters.Where(x => x.PlantCode == PlantCode).FirstOrDefault();
                        if (Plant != null)
                        {
                            PdfPTable ToAddressTable = new PdfPTable(1);
                            cell = new PdfPCell(new Phrase(Plant.PlantText, titleFont));
                            cell.Border = Rectangle.NO_BORDER;
                            ToAddressTable.AddCell(cell);

                            cell = new PdfPCell(new Phrase(Plant.AddressLine1, normalFont));
                            cell.Border = Rectangle.NO_BORDER;
                            ToAddressTable.AddCell(cell);

                            cell = new PdfPCell(new Phrase(Plant.City, normalFont));
                            cell.Border = Rectangle.NO_BORDER;
                            ToAddressTable.AddCell(cell);

                            cell = new PdfPCell(new Phrase(Plant.State, normalFont));
                            cell.Border = Rectangle.NO_BORDER;
                            ToAddressTable.AddCell(cell);

                            cell = new PdfPCell(new Phrase(Plant.Country + " , " + Plant.PinCode, normalFont));
                            cell.Border = Rectangle.NO_BORDER;
                            ToAddressTable.AddCell(cell);
                            POHeaderTable.AddCell(ToAddressTable);
                        }
                        else
                        {
                            PdfPTable ToAddressTable = new PdfPTable(1);
                            cell = new PdfPCell(new Phrase("To", titleFont));
                            cell.Border = Rectangle.NO_BORDER;
                            ToAddressTable.AddCell(cell);

                            cell = new PdfPCell(new Phrase("BPCloud", normalFont));
                            cell.Border = Rectangle.NO_BORDER;
                            ToAddressTable.AddCell(cell);

                            cell = new PdfPCell(new Phrase("Bangalore", normalFont));
                            cell.Border = Rectangle.NO_BORDER;
                            ToAddressTable.AddCell(cell);

                            cell = new PdfPCell(new Phrase("Karnataka", normalFont));
                            cell.Border = Rectangle.NO_BORDER;
                            ToAddressTable.AddCell(cell);
                            POHeaderTable.AddCell(ToAddressTable);
                        }
                    }
                    else
                    {
                        PdfPTable ToAddressTable = new PdfPTable(1);
                        cell = new PdfPCell(new Phrase("To", titleFont));
                        cell.Border = Rectangle.NO_BORDER;
                        ToAddressTable.AddCell(cell);

                        cell = new PdfPCell(new Phrase("BPCloud", normalFont));
                        cell.Border = Rectangle.NO_BORDER;
                        ToAddressTable.AddCell(cell);

                        cell = new PdfPCell(new Phrase("Bangalore", normalFont));
                        cell.Border = Rectangle.NO_BORDER;
                        ToAddressTable.AddCell(cell);

                        cell = new PdfPCell(new Phrase("Karnataka", normalFont));
                        cell.Border = Rectangle.NO_BORDER;
                        ToAddressTable.AddCell(cell);
                        POHeaderTable.AddCell(ToAddressTable);
                    }


                    document.Add(POHeaderTable);

                    PdfPTable ASNItemTable = new PdfPTable(9) { WidthPercentage = 100 };
                    ASNItemTable.SpacingBefore = 20f;


                    //cell = new PdfPCell(new Phrase("PO", titleFont));
                    //cell.Padding = 5f;
                    //ASNItemTable.AddCell(cell);

                    cell = new PdfPCell(new Phrase("Item", titleFont));
                    cell.Padding = 5f;
                    ASNItemTable.AddCell(cell);
                    cell = new PdfPCell(new Phrase("Material", titleFont));
                    cell.Padding = 5f;
                    ASNItemTable.AddCell(cell);
                    cell = new PdfPCell(new Phrase("Material Text", titleFont));
                    cell.Padding = 5f;
                    ASNItemTable.AddCell(cell);
                    cell = new PdfPCell(new Phrase("Delivery date", titleFont));
                    cell.Padding = 5f;
                    ASNItemTable.AddCell(cell);
                    cell = new PdfPCell(new Phrase("Order Qty", titleFont));
                    cell.Padding = 5f;
                    ASNItemTable.AddCell(cell);
                    cell = new PdfPCell(new Phrase("Completed Qty", titleFont));
                    cell.Padding = 5f;
                    ASNItemTable.AddCell(cell);
                    cell = new PdfPCell(new Phrase("Transit Qty", titleFont));
                    cell.Padding = 5f;
                    ASNItemTable.AddCell(cell);
                    cell = new PdfPCell(new Phrase("Open Qty", titleFont));
                    cell.Padding = 5f;
                    ASNItemTable.AddCell(cell);
                    cell = new PdfPCell(new Phrase("UOM", titleFont));
                    cell.Padding = 5f;
                    ASNItemTable.AddCell(cell);
                    int i = 1;
                    foreach (var item in asnItems)
                    {
                        //cell = new PdfPCell(new Phrase(header.DocNumber, normalFont));
                        //if (i == asnItemsCount) cell.Border = Rectangle.BOTTOM_BORDER | Rectangle.LEFT_BORDER;
                        //else cell.Border = Rectangle.LEFT_BORDER;
                        //cell.Padding = 5f;
                        //ASNItemTable.AddCell(cell);

                        cell = new PdfPCell(new Phrase(item.Item, normalFont));
                        if (i == asnItemsCount) cell.Border = Rectangle.BOTTOM_BORDER | Rectangle.LEFT_BORDER;
                        else cell.Border = Rectangle.LEFT_BORDER;
                        cell.Padding = 5f;
                        ASNItemTable.AddCell(cell);

                        cell = new PdfPCell(new Phrase(item.Material, normalFont));
                        if (i == asnItemsCount) cell.Border = Rectangle.BOTTOM_BORDER;
                        else cell.Border = Rectangle.NO_BORDER;
                        cell.Padding = 5f;
                        ASNItemTable.AddCell(cell);

                        cell = new PdfPCell(new Phrase(item.MaterialText, normalFont));
                        if (i == asnItemsCount) cell.Border = Rectangle.BOTTOM_BORDER;
                        else cell.Border = Rectangle.NO_BORDER;
                        cell.Padding = 5f;
                        ASNItemTable.AddCell(cell);

                        var del = item.DeliveryDate.HasValue ? item.DeliveryDate.Value.ToString("dd/MM/yyyy") : "";
                        cell = new PdfPCell(new Phrase(del, normalFont));
                        if (i == asnItemsCount) cell.Border = Rectangle.BOTTOM_BORDER;
                        else cell.Border = Rectangle.NO_BORDER;
                        cell.Padding = 5f;
                        ASNItemTable.AddCell(cell);

                        cell = new PdfPCell(new Phrase(item.OrderedQty.ToString(), normalFont));
                        if (i == asnItemsCount) cell.Border = Rectangle.BOTTOM_BORDER;
                        else cell.Border = Rectangle.NO_BORDER;
                        cell.Padding = 5f;
                        ASNItemTable.AddCell(cell);

                        cell = new PdfPCell(new Phrase(item.CompletedQty.ToString(), normalFont));
                        if (i == asnItemsCount) cell.Border = Rectangle.BOTTOM_BORDER;
                        else cell.Border = Rectangle.NO_BORDER;
                        cell.Padding = 5f;
                        ASNItemTable.AddCell(cell);

                        cell = new PdfPCell(new Phrase(item.TransitQty.ToString(), normalFont));
                        if (i == asnItemsCount) cell.Border = Rectangle.BOTTOM_BORDER | Rectangle.RIGHT_BORDER;
                        else cell.Border = Rectangle.RIGHT_BORDER;
                        cell.Padding = 5f;
                        ASNItemTable.AddCell(cell);

                        cell = new PdfPCell(new Phrase(item.OpenQty.ToString(), normalFont));
                        if (i == asnItemsCount) cell.Border = Rectangle.BOTTOM_BORDER | Rectangle.RIGHT_BORDER;
                        else cell.Border = Rectangle.RIGHT_BORDER;
                        cell.Padding = 5f;
                        ASNItemTable.AddCell(cell);

                        cell = new PdfPCell(new Phrase(item.UOM.ToString(), normalFont));
                        if (i == asnItemsCount) cell.Border = Rectangle.BOTTOM_BORDER | Rectangle.RIGHT_BORDER;
                        else cell.Border = Rectangle.RIGHT_BORDER;
                        cell.Padding = 5f;
                        ASNItemTable.AddCell(cell);

                        //if (i == 1)
                        //{
                        //    cell = new PdfPCell(new Phrase(header.CreatedOn.ToString("dd/MM/yyyy"), normalFont));
                        //    cell.Rowspan = asnItemsCount;
                        //    cell.Border = Rectangle.BOTTOM_BORDER | Rectangle.RIGHT_BORDER;
                        //    cell.Padding = 5f;
                        //    ASNItemTable.AddCell(cell);
                        //}
                        i++;
                    }
                    document.Add(ASNItemTable);

                    document.Close();
                    byte[] bytes = memoryStream.ToArray();
                    memoryStream.Close();
                    return bytes;
                }
            }
            catch (SqlException ex) { WriteLog.WriteToFile("PORespository/CreatePOPdf", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("PORespository/CreatePOPdf", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                throw ex;
            }

            //try
            //{
            //    BPCOFHeader header = _dbContext.BPCOFHeaders.Where(x => x.DocNumber == DocNumber).FirstOrDefault();
            //    List<BPCOFItem> Items = _dbContext.BPCOFItems.Where(x => x.DocNumber == DocNumber).ToList();

            //    Document pdfDoc = new Document(PageSize.A4, 10, 10, 10, 10);
            //    using (System.IO.MemoryStream memoryStream = new System.IO.MemoryStream())
            //    {
            //        PdfWriter writer = PdfWriter.GetInstance(pdfDoc, memoryStream);
            //        var titleFont = FontFactory.GetFont("Arial", 12, Font.BOLD);
            //        var titleFontBlue = FontFactory.GetFont("Arial", 14, Font.NORMAL, BaseColor.BLUE);
            //        var boldTableFont = FontFactory.GetFont("Arial", 8, Font.BOLD);
            //        var bodyFont = FontFactory.GetFont("Arial", 8, Font.NORMAL);
            //        var EmailFont = FontFactory.GetFont("Arial", 8, Font.NORMAL, BaseColor.BLUE);
            //        BaseColor TabelHeaderBackGroundColor = WebColors.GetRGBColor("#EEEEEE");

            //        Rectangle pageSize = writer.PageSize;
            //        // Open the Document for writing
            //        pdfDoc.Open();
            //        //Add elements to the document here

            //        #region Top table
            //        // Create the header table 
            //        PdfPTable headertable = new PdfPTable(2);
            //        headertable.HorizontalAlignment = 0;
            //        headertable.WidthPercentage = 100;
            //        headertable.SetWidths(new float[] { 100f, 100f });  // then set the column's __relative__ widths
            //        headertable.DefaultCell.Border = Rectangle.NO_BORDER;
            //        {
            //            PdfPCell middlecell = new PdfPCell();
            //            middlecell.Border = Rectangle.NO_BORDER;
            //            middlecell.BorderColorBottom = new BaseColor(System.Drawing.Color.Black);
            //            middlecell.BorderWidthBottom = 1f;
            //            headertable.AddCell(middlecell);
            //        }
            //        {
            //            PdfPTable nested = new PdfPTable(1);
            //            nested.DefaultCell.Border = Rectangle.NO_BORDER;
            //            PdfPCell nextPostCell1 = new PdfPCell(new Phrase(header.Company, titleFont));
            //            nextPostCell1.Border = Rectangle.NO_BORDER;
            //            nested.AddCell(nextPostCell1);
            //            PdfPCell nextPostCell2 = new PdfPCell(new Phrase("xxx xxx,xxx,xxx,", bodyFont));
            //            nextPostCell2.Border = Rectangle.NO_BORDER;
            //            nested.AddCell(nextPostCell2);
            //            PdfPCell nextPostCell3 = new PdfPCell(new Phrase("(xxx) xxx-xxx", bodyFont));
            //            nextPostCell3.Border = Rectangle.NO_BORDER;
            //            nested.AddCell(nextPostCell3);
            //            PdfPCell nextPostCell4 = new PdfPCell(new Phrase("company@example.com", EmailFont));
            //            nextPostCell4.Border = Rectangle.NO_BORDER;
            //            nested.AddCell(nextPostCell4);
            //            nested.AddCell("");
            //            PdfPCell nesthousing = new PdfPCell(nested);
            //            nesthousing.Border = Rectangle.NO_BORDER;
            //            nesthousing.BorderColorBottom = new BaseColor(System.Drawing.Color.Black);
            //            nesthousing.BorderWidthBottom = 1f;
            //            nesthousing.Rowspan = 5;
            //            nesthousing.PaddingBottom = 10f;
            //            headertable.AddCell(nesthousing);
            //        }

            //        pdfDoc.Add(headertable);

            //        PdfPTable Invoicetable = new PdfPTable(3);
            //        Invoicetable.HorizontalAlignment = 0;
            //        Invoicetable.WidthPercentage = 100;
            //        Invoicetable.SetWidths(new float[] { 100f, 320f, 100f });  // then set the column's __relative__ widths
            //        Invoicetable.DefaultCell.Border = Rectangle.NO_BORDER;

            //        {
            //            PdfPTable nested = new PdfPTable(1);
            //            nested.DefaultCell.Border = Rectangle.NO_BORDER;
            //            PdfPCell nextPostCell1 = new PdfPCell(new Phrase("INVOICE TO:", bodyFont));
            //            nextPostCell1.Border = Rectangle.NO_BORDER;
            //            nested.AddCell(nextPostCell1);
            //            PdfPCell nextPostCell2 = new PdfPCell(new Phrase(header.Client, titleFont));
            //            nextPostCell2.Border = Rectangle.NO_BORDER;
            //            nested.AddCell(nextPostCell2);
            //            PdfPCell nextPostCell3 = new PdfPCell(new Phrase("xxx xxxx xxx xx", bodyFont));
            //            nextPostCell3.Border = Rectangle.NO_BORDER;
            //            nested.AddCell(nextPostCell3);
            //            PdfPCell nextPostCell4 = new PdfPCell(new Phrase("xxxx@example.com", EmailFont));
            //            nextPostCell4.Border = Rectangle.NO_BORDER;
            //            nested.AddCell(nextPostCell4);
            //            nested.AddCell("");
            //            PdfPCell nesthousing = new PdfPCell(nested);
            //            nesthousing.Border = Rectangle.NO_BORDER;
            //            //nesthousing.BorderColorBottom = new BaseColor(System.Drawing.Color.Black);
            //            //nesthousing.BorderWidthBottom = 1f;
            //            nesthousing.Rowspan = 5;
            //            nesthousing.PaddingBottom = 10f;
            //            Invoicetable.AddCell(nesthousing);
            //        }
            //        {
            //            PdfPCell middlecell = new PdfPCell();
            //            middlecell.Border = Rectangle.NO_BORDER;
            //            //middlecell.BorderColorBottom = new BaseColor(System.Drawing.Color.Black);
            //            //middlecell.BorderWidthBottom = 1f;
            //            Invoicetable.AddCell(middlecell);
            //        }


            //        {
            //            PdfPTable nested = new PdfPTable(1);
            //            nested.DefaultCell.Border = Rectangle.NO_BORDER;
            //            PdfPCell nextPostCell1 = new PdfPCell(new Phrase("Doc No:" + header.DocNumber, titleFontBlue));
            //            nextPostCell1.Border = Rectangle.NO_BORDER;
            //            nested.AddCell(nextPostCell1);
            //            PdfPCell nextPostCell2 = new PdfPCell(new Phrase("Date: " + header.DocDate, bodyFont));
            //            nextPostCell2.Border = Rectangle.NO_BORDER;
            //            nested.AddCell(nextPostCell2);
            //            //PdfPCell nextPostCell3 = new PdfPCell(new Phrase("Invoicd Type: " + header.t, bodyFont));
            //            //nextPostCell3.Border = Rectangle.NO_BORDER;
            //            //nested.AddCell(nextPostCell3);
            //            //PdfPCell nextPostCell4 = new PdfPCell(new Phrase("PO Number: " + header.DocNumber, bodyFont));
            //            //nextPostCell3.Border = Rectangle.NO_BORDER;
            //            //nested.AddCell(nextPostCell4);
            //            //nested.AddCell("");
            //            PdfPCell nesthousing = new PdfPCell(nested);
            //            nesthousing.Border = Rectangle.NO_BORDER;
            //            //nesthousing.BorderColorBottom = new BaseColor(System.Drawing.Color.Black);
            //            //nesthousing.BorderWidthBottom = 1f;
            //            nesthousing.Rowspan = 5;
            //            nesthousing.PaddingBottom = 10f;
            //            Invoicetable.AddCell(nesthousing);
            //        }
            //        Invoicetable.PaddingTop = 10f;

            //        pdfDoc.Add(Invoicetable);
            //        #endregion


            //        PdfPTable ItedData = new PdfPTable(9);
            //        {
            //            ItedData.HorizontalAlignment = 0;
            //            ItedData.WidthPercentage = 100;
            //            ItedData.SetWidths(new float[] { 5, 40, 10, 20, 25,25,25,25, 25 });  // then set the column's __relative__ widths
            //            ItedData.SpacingAfter = 40;
            //            ItedData.DefaultCell.Border = Rectangle.BOX;

            //            PdfPCell itemcell1 = new PdfPCell(new Phrase("Item", boldTableFont));
            //            itemcell1.BackgroundColor = TabelHeaderBackGroundColor;
            //            itemcell1.HorizontalAlignment = Element.ALIGN_CENTER;
            //            ItedData.AddCell(itemcell1);

            //            PdfPCell Iteml2 = new PdfPCell(new Phrase("Material Text", boldTableFont));
            //            Iteml2.BackgroundColor = TabelHeaderBackGroundColor;
            //            Iteml2.HorizontalAlignment = 1;
            //            ItedData.AddCell(Iteml2);

            //            PdfPCell Iteml2cell3 = new PdfPCell(new Phrase("Delivery date", boldTableFont));
            //            Iteml2cell3.BackgroundColor = TabelHeaderBackGroundColor;
            //            Iteml2cell3.HorizontalAlignment = Element.ALIGN_CENTER;
            //            ItedData.AddCell(Iteml2cell3);

            //            PdfPCell itemcell4 = new PdfPCell(new Phrase("Order Qty", boldTableFont));
            //            itemcell4.BackgroundColor = TabelHeaderBackGroundColor;
            //            itemcell4.HorizontalAlignment = Element.ALIGN_CENTER;
            //            ItedData.AddCell(itemcell4);

            //            PdfPCell itemcell5 = new PdfPCell(new Phrase("Completed Qty", boldTableFont));
            //            itemcell5.BackgroundColor = TabelHeaderBackGroundColor;
            //            itemcell5.HorizontalAlignment = 1;
            //            ItedData.AddCell(itemcell5);

            //            PdfPCell itemcell6 = new PdfPCell(new Phrase("Transit Qty", boldTableFont));
            //            itemcell5.BackgroundColor = TabelHeaderBackGroundColor;
            //            itemcell5.HorizontalAlignment = 1;
            //            ItedData.AddCell(itemcell5);

            //            PdfPCell itemcell7 = new PdfPCell(new Phrase("Open Qty", boldTableFont));
            //            itemcell6.BackgroundColor = TabelHeaderBackGroundColor;
            //            itemcell6.HorizontalAlignment = Element.ALIGN_CENTER;
            //            ItedData.AddCell(itemcell6);


            //            PdfPCell itemcell8 = new PdfPCell(new Phrase("HSN", boldTableFont));
            //            itemcell8.BackgroundColor = TabelHeaderBackGroundColor;
            //            itemcell8.HorizontalAlignment = 1;
            //            ItedData.AddCell(itemcell8);

            //            PdfPCell itemcell9 = new PdfPCell(new Phrase("Tax Amount", boldTableFont));
            //            itemcell9.BackgroundColor = TabelHeaderBackGroundColor;
            //            itemcell9.HorizontalAlignment = Element.ALIGN_CENTER;
            //            ItedData.AddCell(itemcell9);

            //            foreach (var expensevalue in Items)
            //            {
            //                PdfPCell Type = new PdfPCell(new Phrase(expensevalue.Item, bodyFont));
            //                Type.HorizontalAlignment = 1;
            //                Type.PaddingLeft = 10f;
            //                Type.Border = Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER;
            //                ItedData.AddCell(Type);
            //                PdfPCell amountcell = new PdfPCell(new Phrase("" + expensevalue.MaterialText, bodyFont));
            //                amountcell.HorizontalAlignment = 1;
            //                amountcell.PaddingLeft = 10f;
            //                amountcell.Border = Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER;
            //                ItedData.AddCell(amountcell);
            //                PdfPCell remark = new PdfPCell(new Phrase(expensevalue.DeliveryDate.HasValue ? expensevalue.DeliveryDate.Value.ToString("dd/MM/yyyy") : "", bodyFont));
            //                remark.HorizontalAlignment = 1;
            //                remark.Border = Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER;
            //                ItedData.AddCell(remark);

            //                PdfPCell orderType = new PdfPCell(new Phrase("" + expensevalue.OrderedQty, bodyFont));
            //                orderType.HorizontalAlignment = 1;
            //                orderType.PaddingLeft = 10f;
            //                orderType.Border = Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER;
            //                ItedData.AddCell(orderType);
            //                PdfPCell Opencell = new PdfPCell(new Phrase("" + expensevalue.CompletedQty, bodyFont));
            //                Opencell.HorizontalAlignment = 1;
            //                Opencell.PaddingLeft = 10f;
            //                Opencell.Border = Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER;
            //                ItedData.AddCell(Opencell);
            //                PdfPCell Invoiceqtycell = new PdfPCell(new Phrase("" + expensevalue.TransitQty, bodyFont));
            //                Invoiceqtycell.HorizontalAlignment = 1;
            //                Invoiceqtycell.Border = Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER;
            //                ItedData.AddCell(Invoiceqtycell);

            //                PdfPCell pricecell = new PdfPCell(new Phrase("" + expensevalue.OpenQty, bodyFont));
            //                pricecell.HorizontalAlignment = 1;
            //                pricecell.PaddingLeft = 10f;
            //                pricecell.Border = Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER;
            //                ItedData.AddCell(pricecell);
            //                PdfPCell taxcell = new PdfPCell(new Phrase("" + expensevalue.HSN, bodyFont));
            //                taxcell.HorizontalAlignment = 1;
            //                taxcell.PaddingLeft = 10f;
            //                taxcell.Border = Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER;
            //                ItedData.AddCell(taxcell);
            //                PdfPCell amounttcell = new PdfPCell(new Phrase("" + expensevalue.TaxAmount, bodyFont));
            //                amounttcell.HorizontalAlignment = 1;
            //                amounttcell.Border = Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER;
            //                ItedData.AddCell(amounttcell);
            //            }

            //        }
            //        ItedData.PaddingTop = 10f;
            //        pdfDoc.Add(ItedData);
            //        PdfContentByte cb = new PdfContentByte(writer);
            //        BaseFont bf = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1250, true);
            //        cb = new PdfContentByte(writer);
            //        cb = writer.DirectContent;
            //        cb.BeginText();
            //        cb.SetFontAndSize(bf, 8);
            //        cb.SetTextMatrix(pageSize.GetLeft(120), 20);
            //        cb.ShowText("Invoice was created on a computer and is valid without the signature and seal. ");
            //        cb.EndText();

            //        //Move the pointer and draw line to separate footer section from rest of page
            //        cb.MoveTo(40, pdfDoc.PageSize.GetBottom(50));
            //        cb.LineTo(pdfDoc.PageSize.Width - 40, pdfDoc.PageSize.GetBottom(50));
            //        cb.Stroke();

            //        pdfDoc.Close();
            //        byte[] bytes = memoryStream.ToArray();
            //        memoryStream.Close();
            //        return bytes;
            //    }
            //}
            //catch (SqlException ex){ WriteLog.WriteToFile("PORespository/", ex); throw new Exception("Something went wrong");}catch(InvalidOperationException ex){WriteLog.WriteToFile("PORespository/", ex); throw new Exception("Something went wrong"); }catch (Exception ex)
            //{
            //    throw ex;
            //}
        }

        public BPCAttachment PrintPO(string DocNumber)
        {
            try
            {
                return (from tb in _dbContext.BPCAttachments
                        orderby tb.AttachmentName, tb.CreatedOn
                        where tb.ReferenceNo == DocNumber && tb.AttachmentFile.Length > 0
                        select tb).FirstOrDefault();
            }
            catch (SqlException ex) { WriteLog.WriteToFile("PORespository/PrintPO", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("PORespository/PrintPO", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                throw ex;
            }

            //try
            //{
            //    BPCOFHeader header = _dbContext.BPCOFHeaders.Where(x => x.DocNumber == DocNumber).FirstOrDefault();
            //    List<BPCOFItem> Items = _dbContext.BPCOFItems.Where(x => x.DocNumber == DocNumber).ToList();

            //    Document pdfDoc = new Document(PageSize.A4, 10, 10, 10, 10);
            //    using (System.IO.MemoryStream memoryStream = new System.IO.MemoryStream())
            //    {
            //        PdfWriter writer = PdfWriter.GetInstance(pdfDoc, memoryStream);
            //        var titleFont = FontFactory.GetFont("Arial", 12, Font.BOLD);
            //        var titleFontBlue = FontFactory.GetFont("Arial", 14, Font.NORMAL, BaseColor.BLUE);
            //        var boldTableFont = FontFactory.GetFont("Arial", 8, Font.BOLD);
            //        var bodyFont = FontFactory.GetFont("Arial", 8, Font.NORMAL);
            //        var EmailFont = FontFactory.GetFont("Arial", 8, Font.NORMAL, BaseColor.BLUE);
            //        BaseColor TabelHeaderBackGroundColor = WebColors.GetRGBColor("#EEEEEE");

            //        Rectangle pageSize = writer.PageSize;
            //        // Open the Document for writing
            //        pdfDoc.Open();
            //        //Add elements to the document here

            //        #region Top table
            //        // Create the header table 
            //        PdfPTable headertable = new PdfPTable(2);
            //        headertable.HorizontalAlignment = 0;
            //        headertable.WidthPercentage = 100;
            //        headertable.SetWidths(new float[] { 100f, 100f });  // then set the column's __relative__ widths
            //        headertable.DefaultCell.Border = Rectangle.NO_BORDER;
            //        {
            //            PdfPCell middlecell = new PdfPCell();
            //            middlecell.Border = Rectangle.NO_BORDER;
            //            middlecell.BorderColorBottom = new BaseColor(System.Drawing.Color.Black);
            //            middlecell.BorderWidthBottom = 1f;
            //            headertable.AddCell(middlecell);
            //        }
            //        {
            //            PdfPTable nested = new PdfPTable(1);
            //            nested.DefaultCell.Border = Rectangle.NO_BORDER;
            //            PdfPCell nextPostCell1 = new PdfPCell(new Phrase(header.Company, titleFont));
            //            nextPostCell1.Border = Rectangle.NO_BORDER;
            //            nested.AddCell(nextPostCell1);
            //            PdfPCell nextPostCell2 = new PdfPCell(new Phrase("xxx xxx,xxx,xxx,", bodyFont));
            //            nextPostCell2.Border = Rectangle.NO_BORDER;
            //            nested.AddCell(nextPostCell2);
            //            PdfPCell nextPostCell3 = new PdfPCell(new Phrase("(xxx) xxx-xxx", bodyFont));
            //            nextPostCell3.Border = Rectangle.NO_BORDER;
            //            nested.AddCell(nextPostCell3);
            //            PdfPCell nextPostCell4 = new PdfPCell(new Phrase("company@example.com", EmailFont));
            //            nextPostCell4.Border = Rectangle.NO_BORDER;
            //            nested.AddCell(nextPostCell4);
            //            nested.AddCell("");
            //            PdfPCell nesthousing = new PdfPCell(nested);
            //            nesthousing.Border = Rectangle.NO_BORDER;
            //            nesthousing.BorderColorBottom = new BaseColor(System.Drawing.Color.Black);
            //            nesthousing.BorderWidthBottom = 1f;
            //            nesthousing.Rowspan = 5;
            //            nesthousing.PaddingBottom = 10f;
            //            headertable.AddCell(nesthousing);
            //        }

            //        pdfDoc.Add(headertable);

            //        PdfPTable Invoicetable = new PdfPTable(3);
            //        Invoicetable.HorizontalAlignment = 0;
            //        Invoicetable.WidthPercentage = 100;
            //        Invoicetable.SetWidths(new float[] { 100f, 320f, 100f });  // then set the column's __relative__ widths
            //        Invoicetable.DefaultCell.Border = Rectangle.NO_BORDER;

            //        {
            //            PdfPTable nested = new PdfPTable(1);
            //            nested.DefaultCell.Border = Rectangle.NO_BORDER;
            //            PdfPCell nextPostCell1 = new PdfPCell(new Phrase("INVOICE TO:", bodyFont));
            //            nextPostCell1.Border = Rectangle.NO_BORDER;
            //            nested.AddCell(nextPostCell1);
            //            PdfPCell nextPostCell2 = new PdfPCell(new Phrase(header.Client, titleFont));
            //            nextPostCell2.Border = Rectangle.NO_BORDER;
            //            nested.AddCell(nextPostCell2);
            //            PdfPCell nextPostCell3 = new PdfPCell(new Phrase("xxx xxxx xxx xx", bodyFont));
            //            nextPostCell3.Border = Rectangle.NO_BORDER;
            //            nested.AddCell(nextPostCell3);
            //            PdfPCell nextPostCell4 = new PdfPCell(new Phrase("xxxx@example.com", EmailFont));
            //            nextPostCell4.Border = Rectangle.NO_BORDER;
            //            nested.AddCell(nextPostCell4);
            //            nested.AddCell("");
            //            PdfPCell nesthousing = new PdfPCell(nested);
            //            nesthousing.Border = Rectangle.NO_BORDER;
            //            //nesthousing.BorderColorBottom = new BaseColor(System.Drawing.Color.Black);
            //            //nesthousing.BorderWidthBottom = 1f;
            //            nesthousing.Rowspan = 5;
            //            nesthousing.PaddingBottom = 10f;
            //            Invoicetable.AddCell(nesthousing);
            //        }
            //        {
            //            PdfPCell middlecell = new PdfPCell();
            //            middlecell.Border = Rectangle.NO_BORDER;
            //            //middlecell.BorderColorBottom = new BaseColor(System.Drawing.Color.Black);
            //            //middlecell.BorderWidthBottom = 1f;
            //            Invoicetable.AddCell(middlecell);
            //        }


            //        {
            //            PdfPTable nested = new PdfPTable(1);
            //            nested.DefaultCell.Border = Rectangle.NO_BORDER;
            //            PdfPCell nextPostCell1 = new PdfPCell(new Phrase("Doc No:" + header.DocNumber, titleFontBlue));
            //            nextPostCell1.Border = Rectangle.NO_BORDER;
            //            nested.AddCell(nextPostCell1);
            //            PdfPCell nextPostCell2 = new PdfPCell(new Phrase("Date: " + header.DocDate, bodyFont));
            //            nextPostCell2.Border = Rectangle.NO_BORDER;
            //            nested.AddCell(nextPostCell2);
            //            //PdfPCell nextPostCell3 = new PdfPCell(new Phrase("Invoicd Type: " + header.t, bodyFont));
            //            //nextPostCell3.Border = Rectangle.NO_BORDER;
            //            //nested.AddCell(nextPostCell3);
            //            //PdfPCell nextPostCell4 = new PdfPCell(new Phrase("PO Number: " + header.DocNumber, bodyFont));
            //            //nextPostCell3.Border = Rectangle.NO_BORDER;
            //            //nested.AddCell(nextPostCell4);
            //            //nested.AddCell("");
            //            PdfPCell nesthousing = new PdfPCell(nested);
            //            nesthousing.Border = Rectangle.NO_BORDER;
            //            //nesthousing.BorderColorBottom = new BaseColor(System.Drawing.Color.Black);
            //            //nesthousing.BorderWidthBottom = 1f;
            //            nesthousing.Rowspan = 5;
            //            nesthousing.PaddingBottom = 10f;
            //            Invoicetable.AddCell(nesthousing);
            //        }
            //        Invoicetable.PaddingTop = 10f;

            //        pdfDoc.Add(Invoicetable);
            //        #endregion


            //        PdfPTable ItedData = new PdfPTable(9);
            //        {
            //            ItedData.HorizontalAlignment = 0;
            //            ItedData.WidthPercentage = 100;
            //            ItedData.SetWidths(new float[] { 5, 40, 10, 20, 25,25,25,25, 25 });  // then set the column's __relative__ widths
            //            ItedData.SpacingAfter = 40;
            //            ItedData.DefaultCell.Border = Rectangle.BOX;

            //            PdfPCell itemcell1 = new PdfPCell(new Phrase("Item", boldTableFont));
            //            itemcell1.BackgroundColor = TabelHeaderBackGroundColor;
            //            itemcell1.HorizontalAlignment = Element.ALIGN_CENTER;
            //            ItedData.AddCell(itemcell1);

            //            PdfPCell Iteml2 = new PdfPCell(new Phrase("Material Text", boldTableFont));
            //            Iteml2.BackgroundColor = TabelHeaderBackGroundColor;
            //            Iteml2.HorizontalAlignment = 1;
            //            ItedData.AddCell(Iteml2);

            //            PdfPCell Iteml2cell3 = new PdfPCell(new Phrase("Delivery date", boldTableFont));
            //            Iteml2cell3.BackgroundColor = TabelHeaderBackGroundColor;
            //            Iteml2cell3.HorizontalAlignment = Element.ALIGN_CENTER;
            //            ItedData.AddCell(Iteml2cell3);

            //            PdfPCell itemcell4 = new PdfPCell(new Phrase("Order Qty", boldTableFont));
            //            itemcell4.BackgroundColor = TabelHeaderBackGroundColor;
            //            itemcell4.HorizontalAlignment = Element.ALIGN_CENTER;
            //            ItedData.AddCell(itemcell4);

            //            PdfPCell itemcell5 = new PdfPCell(new Phrase("Completed Qty", boldTableFont));
            //            itemcell5.BackgroundColor = TabelHeaderBackGroundColor;
            //            itemcell5.HorizontalAlignment = 1;
            //            ItedData.AddCell(itemcell5);

            //            PdfPCell itemcell6 = new PdfPCell(new Phrase("Transit Qty", boldTableFont));
            //            itemcell5.BackgroundColor = TabelHeaderBackGroundColor;
            //            itemcell5.HorizontalAlignment = 1;
            //            ItedData.AddCell(itemcell5);

            //            PdfPCell itemcell7 = new PdfPCell(new Phrase("Open Qty", boldTableFont));
            //            itemcell6.BackgroundColor = TabelHeaderBackGroundColor;
            //            itemcell6.HorizontalAlignment = Element.ALIGN_CENTER;
            //            ItedData.AddCell(itemcell6);


            //            PdfPCell itemcell8 = new PdfPCell(new Phrase("HSN", boldTableFont));
            //            itemcell8.BackgroundColor = TabelHeaderBackGroundColor;
            //            itemcell8.HorizontalAlignment = 1;
            //            ItedData.AddCell(itemcell8);

            //            PdfPCell itemcell9 = new PdfPCell(new Phrase("Tax Amount", boldTableFont));
            //            itemcell9.BackgroundColor = TabelHeaderBackGroundColor;
            //            itemcell9.HorizontalAlignment = Element.ALIGN_CENTER;
            //            ItedData.AddCell(itemcell9);

            //            foreach (var expensevalue in Items)
            //            {
            //                PdfPCell Type = new PdfPCell(new Phrase(expensevalue.Item, bodyFont));
            //                Type.HorizontalAlignment = 1;
            //                Type.PaddingLeft = 10f;
            //                Type.Border = Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER;
            //                ItedData.AddCell(Type);
            //                PdfPCell amountcell = new PdfPCell(new Phrase("" + expensevalue.MaterialText, bodyFont));
            //                amountcell.HorizontalAlignment = 1;
            //                amountcell.PaddingLeft = 10f;
            //                amountcell.Border = Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER;
            //                ItedData.AddCell(amountcell);
            //                PdfPCell remark = new PdfPCell(new Phrase(expensevalue.DeliveryDate.HasValue ? expensevalue.DeliveryDate.Value.ToString("dd/MM/yyyy") : "", bodyFont));
            //                remark.HorizontalAlignment = 1;
            //                remark.Border = Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER;
            //                ItedData.AddCell(remark);

            //                PdfPCell orderType = new PdfPCell(new Phrase("" + expensevalue.OrderedQty, bodyFont));
            //                orderType.HorizontalAlignment = 1;
            //                orderType.PaddingLeft = 10f;
            //                orderType.Border = Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER;
            //                ItedData.AddCell(orderType);
            //                PdfPCell Opencell = new PdfPCell(new Phrase("" + expensevalue.CompletedQty, bodyFont));
            //                Opencell.HorizontalAlignment = 1;
            //                Opencell.PaddingLeft = 10f;
            //                Opencell.Border = Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER;
            //                ItedData.AddCell(Opencell);
            //                PdfPCell Invoiceqtycell = new PdfPCell(new Phrase("" + expensevalue.TransitQty, bodyFont));
            //                Invoiceqtycell.HorizontalAlignment = 1;
            //                Invoiceqtycell.Border = Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER;
            //                ItedData.AddCell(Invoiceqtycell);

            //                PdfPCell pricecell = new PdfPCell(new Phrase("" + expensevalue.OpenQty, bodyFont));
            //                pricecell.HorizontalAlignment = 1;
            //                pricecell.PaddingLeft = 10f;
            //                pricecell.Border = Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER;
            //                ItedData.AddCell(pricecell);
            //                PdfPCell taxcell = new PdfPCell(new Phrase("" + expensevalue.HSN, bodyFont));
            //                taxcell.HorizontalAlignment = 1;
            //                taxcell.PaddingLeft = 10f;
            //                taxcell.Border = Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER;
            //                ItedData.AddCell(taxcell);
            //                PdfPCell amounttcell = new PdfPCell(new Phrase("" + expensevalue.TaxAmount, bodyFont));
            //                amounttcell.HorizontalAlignment = 1;
            //                amounttcell.Border = Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER;
            //                ItedData.AddCell(amounttcell);
            //            }

            //        }
            //        ItedData.PaddingTop = 10f;
            //        pdfDoc.Add(ItedData);
            //        PdfContentByte cb = new PdfContentByte(writer);
            //        BaseFont bf = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1250, true);
            //        cb = new PdfContentByte(writer);
            //        cb = writer.DirectContent;
            //        cb.BeginText();
            //        cb.SetFontAndSize(bf, 8);
            //        cb.SetTextMatrix(pageSize.GetLeft(120), 20);
            //        cb.ShowText("Invoice was created on a computer and is valid without the signature and seal. ");
            //        cb.EndText();

            //        //Move the pointer and draw line to separate footer section from rest of page
            //        cb.MoveTo(40, pdfDoc.PageSize.GetBottom(50));
            //        cb.LineTo(pdfDoc.PageSize.Width - 40, pdfDoc.PageSize.GetBottom(50));
            //        cb.Stroke();

            //        pdfDoc.Close();
            //        byte[] bytes = memoryStream.ToArray();
            //        memoryStream.Close();
            //        return bytes;
            //    }
            //}
            //catch (SqlException ex){ WriteLog.WriteToFile("PORespository/", ex); throw new Exception("Something went wrong");}catch(InvalidOperationException ex){WriteLog.WriteToFile("PORespository/", ex); throw new Exception("Something went wrong"); }catch (Exception ex)
            //{
            //    throw ex;
            //}
        }



        #region PO FLIP

        public List<BPCFLIPHeaderView> GetPOFLIPsByPartnerID(string PartnerID)
        {
            try
            {
                List<BPCFLIPHeaderView> bPCFLIPHeaderViews = new List<BPCFLIPHeaderView>();
                if (!string.IsNullOrEmpty(PartnerID))
                {
                    var BPCFLIPHeaders = _dbContext.BPCFLIPHeaders.Where(x => x.PatnerID == PartnerID).ToList();
                    foreach (var FLIPHeader in BPCFLIPHeaders)
                    {
                        BPCFLIPHeaderView bPCFLIPHeaderView = new BPCFLIPHeaderView();
                        bPCFLIPHeaderView.FLIPID = FLIPHeader.FLIPID;
                        bPCFLIPHeaderView.InvoiceAmount = FLIPHeader.InvoiceAmount;
                        bPCFLIPHeaderView.GSTIN = FLIPHeader.GSTIN;
                        bPCFLIPHeaderView.InvoiceDate = FLIPHeader.InvoiceDate;
                        bPCFLIPHeaderView.InvoiceNumber = FLIPHeader.InvoiceNumber;
                        bPCFLIPHeaderView.InvoiceType = FLIPHeader.InvoiceType;
                        bPCFLIPHeaderView.InvoiceCurrency = FLIPHeader.InvoiceCurrency;
                        bPCFLIPHeaderView.InvoiceDocID = FLIPHeader.InvoiceDocID;
                        bPCFLIPHeaderView.IsInvoiceOrCertified = FLIPHeader.IsInvoiceOrCertified;
                        bPCFLIPHeaderView.IsInvoiceFlag = FLIPHeader.IsInvoiceFlag;
                        bPCFLIPHeaderView.InvoiceAttachmentName = FLIPHeader.InvoiceAttachmentName;
                        bPCFLIPHeaderView.Client = FLIPHeader.Client;
                        bPCFLIPHeaderView.Company = FLIPHeader.Company;
                        bPCFLIPHeaderView.Type = FLIPHeader.Type;
                        bPCFLIPHeaderView.PatnerID = FLIPHeader.PatnerID;
                        bPCFLIPHeaderView.DocNumber = FLIPHeader.DocNumber;
                        bPCFLIPHeaderView.IsActive = FLIPHeader.IsActive;
                        bPCFLIPHeaderView.CreatedOn = FLIPHeader.CreatedOn;
                        bPCFLIPHeaderView.CreatedBy = FLIPHeader.CreatedBy;
                        bPCFLIPHeaderView.ProfitCentre = FLIPHeader.ProfitCentre;
                        bPCFLIPHeaderViews.Add(bPCFLIPHeaderView);
                    }
                }
                return bPCFLIPHeaderViews;
            }
            catch (SqlException ex) { WriteLog.WriteToFile("PORespository/GetPOFLIPsByPartnerID", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("PORespository/GetPOFLIPsByPartnerID", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<BPCFLIPHeaderView> GetPOFLIPsBySuperUser(string GetPlantByUser)
        {
            try
            {
                List<BPCFLIPHeaderView> bPCFLIPHeaderViews = new List<BPCFLIPHeaderView>();
                if (!string.IsNullOrEmpty(GetPlantByUser))
                {
                    string[] Plants = GetPlantByUser.Split(',');
                    for (int i = 0; i < Plants.Length; i++)
                    {
                        Plants[i] = Plants[i].Trim();
                    }
                    List<BPCOFHeader> bPCOFHeader = new List<BPCOFHeader>();
                    for (int i = 0; i < Plants.Length; i++)
                    {
                        var BPCFLIPHeaders = _dbContext.BPCFLIPHeaders.Where(x => x.Plant == Plants[i]).ToList();
                        foreach (var FLIPHeader in BPCFLIPHeaders)
                        {
                            BPCFLIPHeaderView bPCFLIPHeaderView = new BPCFLIPHeaderView();
                            bPCFLIPHeaderView.FLIPID = FLIPHeader.FLIPID;
                            bPCFLIPHeaderView.InvoiceAmount = FLIPHeader.InvoiceAmount;
                            bPCFLIPHeaderView.GSTIN = FLIPHeader.GSTIN;
                            bPCFLIPHeaderView.InvoiceDate = FLIPHeader.InvoiceDate;
                            bPCFLIPHeaderView.InvoiceNumber = FLIPHeader.InvoiceNumber;
                            bPCFLIPHeaderView.InvoiceType = FLIPHeader.InvoiceType;
                            bPCFLIPHeaderView.InvoiceCurrency = FLIPHeader.InvoiceCurrency;
                            bPCFLIPHeaderView.InvoiceDocID = FLIPHeader.InvoiceDocID;
                            bPCFLIPHeaderView.IsInvoiceOrCertified = FLIPHeader.IsInvoiceOrCertified;
                            bPCFLIPHeaderView.IsInvoiceFlag = FLIPHeader.IsInvoiceFlag;
                            bPCFLIPHeaderView.InvoiceAttachmentName = FLIPHeader.InvoiceAttachmentName;
                            bPCFLIPHeaderView.Client = FLIPHeader.Client;
                            bPCFLIPHeaderView.Company = FLIPHeader.Company;
                            bPCFLIPHeaderView.Type = FLIPHeader.Type;
                            bPCFLIPHeaderView.PatnerID = FLIPHeader.PatnerID;
                            bPCFLIPHeaderView.DocNumber = FLIPHeader.DocNumber;
                            bPCFLIPHeaderView.IsActive = FLIPHeader.IsActive;
                            bPCFLIPHeaderView.CreatedOn = FLIPHeader.CreatedOn;
                            bPCFLIPHeaderView.CreatedBy = FLIPHeader.CreatedBy;
                            bPCFLIPHeaderView.ProfitCentre = FLIPHeader.ProfitCentre;
                            bPCFLIPHeaderViews.Add(bPCFLIPHeaderView);
                        }
                    }
                }
                return bPCFLIPHeaderViews;
            }
            catch (SqlException ex) { WriteLog.WriteToFile("PORespository/GetPOFLIPsByPartnerID", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("PORespository/GetPOFLIPsByPartnerID", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        
        public List<BPCFLIPHeaderView> GetPOFLIPsByDocAndPartnerID(string DocNumber, string PartnerID)
        {
            try
            {
                List<BPCFLIPHeaderView> bPCFLIPHeaderViews = new List<BPCFLIPHeaderView>();
                if (!string.IsNullOrEmpty(DocNumber) && !string.IsNullOrEmpty(PartnerID))
                {
                    var BPCFLIPHeaders = _dbContext.BPCFLIPHeaders.Where(x => x.DocNumber.ToLower() == DocNumber.ToLower()
                                         && x.PatnerID.ToLower() == PartnerID.ToLower()).ToList();
                    foreach (var FLIPHeader in BPCFLIPHeaders)
                    {
                        BPCFLIPHeaderView bPCFLIPHeaderView = new BPCFLIPHeaderView();
                        bPCFLIPHeaderView.FLIPID = FLIPHeader.FLIPID;
                        bPCFLIPHeaderView.InvoiceAmount = FLIPHeader.InvoiceAmount;
                        bPCFLIPHeaderView.InvoiceDate = FLIPHeader.InvoiceDate;
                        bPCFLIPHeaderView.InvoiceNumber = FLIPHeader.InvoiceNumber;
                        bPCFLIPHeaderView.InvoiceType = FLIPHeader.InvoiceType;
                        bPCFLIPHeaderView.InvoiceCurrency = FLIPHeader.InvoiceCurrency;
                        bPCFLIPHeaderView.InvoiceDocID = FLIPHeader.InvoiceDocID;
                        bPCFLIPHeaderView.IsInvoiceOrCertified = FLIPHeader.IsInvoiceOrCertified;
                        bPCFLIPHeaderView.IsInvoiceFlag = FLIPHeader.IsInvoiceFlag;
                        bPCFLIPHeaderView.InvoiceAttachmentName = FLIPHeader.InvoiceAttachmentName;
                        bPCFLIPHeaderView.Client = FLIPHeader.Client;
                        bPCFLIPHeaderView.Company = FLIPHeader.Company;
                        bPCFLIPHeaderView.Type = FLIPHeader.Type;
                        bPCFLIPHeaderView.PatnerID = FLIPHeader.PatnerID;
                        bPCFLIPHeaderView.DocNumber = FLIPHeader.DocNumber;
                        bPCFLIPHeaderView.IsActive = FLIPHeader.IsActive;
                        bPCFLIPHeaderView.CreatedOn = FLIPHeader.CreatedOn;
                        bPCFLIPHeaderView.CreatedBy = FLIPHeader.CreatedBy;
                        bPCFLIPHeaderViews.Add(bPCFLIPHeaderView);
                    }
                }
                return bPCFLIPHeaderViews;
            }
            catch (SqlException ex) { WriteLog.WriteToFile("PORespository/GetPOFLIPsByDocAndPartnerID", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("PORespository/GetPOFLIPsByDocAndPartnerID", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<BPCFLIPHeader> CreatePOFLIP(BPCFLIPHeaderView FLIPHeader)
        {
            var bPCFLIPHeaderResult = new BPCFLIPHeader();
            var strategy = _dbContext.Database.CreateExecutionStrategy();
            await strategy.ExecuteAsync(async () =>
            {
                using (var transaction = _dbContext.Database.BeginTransaction())
                {
                    try
                    {
                        int ID = 0;
                        var lastFlipID = _dbContext.BPCFLIPHeaders.OrderByDescending(x => x.CreatedOn).Select(t => t.FLIPID).FirstOrDefault();
                        BPCFLIPHeader bPCFLIPHeader = new BPCFLIPHeader();
                        //string randomText = RandomString(10, false);
                        //if (string.IsNullOrEmpty(randomText))
                        //    bPCFLIPHeader.FLIPID = new Random().Next().ToString();
                        if (lastFlipID != null)
                        {
                            int.TryParse(lastFlipID, out ID);
                        }
                        ++ID;
                        bPCFLIPHeader.FLIPID = ID.ToString("D10");
                        bPCFLIPHeader.InvoiceAmount = FLIPHeader.InvoiceAmount;
                        bPCFLIPHeader.ProfitCentre = FLIPHeader.ProfitCentre;
                        bPCFLIPHeader.GSTIN = FLIPHeader.GSTIN;
                        bPCFLIPHeader.InvoiceDate = FLIPHeader.InvoiceDate;
                        bPCFLIPHeader.InvoiceNumber = FLIPHeader.InvoiceNumber;
                        bPCFLIPHeader.InvoiceType = FLIPHeader.InvoiceType;
                        bPCFLIPHeader.InvoiceCurrency = FLIPHeader.InvoiceCurrency;
                        //bPCFLIPHeader.InvoiceDocID = FLIPHeader.InvoiceDocID;
                        bPCFLIPHeader.IsInvoiceOrCertified = FLIPHeader.IsInvoiceOrCertified;
                        bPCFLIPHeader.IsInvoiceFlag = FLIPHeader.IsInvoiceFlag;
                        bPCFLIPHeader.Client = FLIPHeader.Client;
                        bPCFLIPHeader.Company = FLIPHeader.Company;
                        bPCFLIPHeader.Type = FLIPHeader.Type;
                        bPCFLIPHeader.PatnerID = FLIPHeader.PatnerID;
                        bPCFLIPHeader.DocNumber = FLIPHeader.DocNumber;
                        bPCFLIPHeader.InvoiceAttachmentName = FLIPHeader.InvoiceAttachmentName;
                        bPCFLIPHeader.IsActive = true;
                        bPCFLIPHeader.CreatedOn = DateTime.Now;
                        var result = _dbContext.BPCFLIPHeaders.Add(bPCFLIPHeader);
                        if (!string.IsNullOrEmpty(FLIPHeader.InvoiceAttachmentName))
                        {
                            BPCFLIPAttachment FLIPAttachment = new BPCFLIPAttachment();
                            FLIPAttachment.PO = FLIPHeader.DocNumber;
                            FLIPAttachment.FLIPID = bPCFLIPHeader.FLIPID;
                            FLIPAttachment.AttachmentName = FLIPHeader.InvoiceAttachmentName;
                            BPCFLIPAttachment result1 = await attachmentRepository.AddAttachment(FLIPAttachment);
                            result.Entity.InvoiceDocID = result1.AttachmentID.ToString();
                        }
                        await _dbContext.SaveChangesAsync();

                        FLIPCostRepository FLIPCostRepository = new FLIPCostRepository(_dbContext);
                        await FLIPCostRepository.CreateFLIPCosts(FLIPHeader.FLIPCosts, result.Entity.FLIPID);

                        await CreateFLIPItems(FLIPHeader.FLIPItems, result.Entity.FLIPID);

                        CreateInvoiceXmlData(bPCFLIPHeader, FLIPHeader.FLIPItems, FLIPHeader.FLIPCosts);
                        transaction.Commit();
                        transaction.Dispose();
                        bPCFLIPHeaderResult = result.Entity;
                        return bPCFLIPHeaderResult;
                    }
                    catch (SqlException ex)
                    {
                        WriteLog.WriteToFile("PORespository/CreatePOFlip", ex);
                        throw new Exception("Something went wrong");
                    }
                    catch (InvalidOperationException ex)
                    {
                        WriteLog.WriteToFile("PORespository/CreatePOFlip", ex);
                        throw new Exception("Something went wrong");
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        transaction.Dispose();
                        throw ex;
                    }
                }
            });
            return bPCFLIPHeaderResult;
        }

        public async Task CreateFLIPItems(List<BPCFLIPItem> FLIPItems, string FLIPID)
        {
            try
            {
                if (FLIPItems != null && FLIPItems.Count > 0)
                {
                    foreach (BPCFLIPItem FLIPItem in FLIPItems)
                    {
                        BPCFLIPItem bPCFLIPItem = new BPCFLIPItem();
                        bPCFLIPItem.FLIPID = FLIPID;
                        bPCFLIPItem.Client = FLIPItem.Client;
                        bPCFLIPItem.Company = FLIPItem.Company;
                        bPCFLIPItem.PatnerID = FLIPItem.PatnerID;
                        bPCFLIPItem.Type = FLIPItem.Type;
                        //bPCFLIPItem.Item = "10";
                        bPCFLIPItem.Item = FLIPItem.Item;
                        bPCFLIPItem.Material = FLIPItem.Material;
                        bPCFLIPItem.MaterialText = FLIPItem.MaterialText;
                        bPCFLIPItem.DeliveryDate = FLIPItem.DeliveryDate;
                        bPCFLIPItem.OrderedQty = FLIPItem.OrderedQty;
                        bPCFLIPItem.OpenQty = FLIPItem.OpenQty;
                        bPCFLIPItem.InvoiceQty = FLIPItem.InvoiceQty;
                        bPCFLIPItem.UOM = FLIPItem.UOM;
                        bPCFLIPItem.HSN = FLIPItem.HSN;
                        bPCFLIPItem.Price = FLIPItem.Price;
                        bPCFLIPItem.TaxType = FLIPItem.TaxType;
                        bPCFLIPItem.Tax = FLIPItem.Tax;
                        bPCFLIPItem.Amount = FLIPItem.Amount;
                        bPCFLIPItem.IsActive = true;
                        bPCFLIPItem.CreatedOn = DateTime.Now;
                        var result = _dbContext.BPCFLIPItems.Add(bPCFLIPItem);
                    }
                    await _dbContext.SaveChangesAsync();
                }
            }
            catch (SqlException ex)
            {
                WriteLog.WriteToFile("PORespository/CreateFLIPItems", ex);
                throw new Exception("Something went wrong");
            }
            catch (InvalidOperationException ex)
            {
                WriteLog.WriteToFile("PORespository/CreateFLIPItems", ex);
                throw new Exception("Something went wrong");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task DeleteFLIPItemByFLIPID(string FLIPID)
        {
            try
            {
                _dbContext.Set<BPCFLIPItem>().Where(x => x.FLIPID == FLIPID).ToList().ForEach(x => _dbContext.Set<BPCFLIPItem>().Remove(x));
                await _dbContext.SaveChangesAsync();
            }

            catch (SqlException ex) { WriteLog.WriteToFile("PORespository/DeleteFLIPItemByFLIPID", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("PORespository/DeleteFLIPItemByFLIPID", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string RandomString(int size, bool lowerCase)
        {
            try
            {
                StringBuilder builder = new StringBuilder();
                Random random = new Random();
                char ch;
                for (int i = 0; i < size; i++)
                {
                    ch = Convert.ToChar(Convert.ToInt32(Math.Floor(26 * random.NextDouble() + 65)));
                    builder.Append(ch);
                }
                if (lowerCase)
                    return builder.ToString().ToLower();
                return builder.ToString();
            }
            catch (SqlException ex) { WriteLog.WriteToFile("PORespository/RandomString", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("PORespository/RandomString", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<BPCFLIPHeader> UpdatePOFLIP(BPCFLIPHeaderView FLIPHeader)
        {
            var bPCFLIPHeaderResult = new BPCFLIPHeader();
            var strategy = _dbContext.Database.CreateExecutionStrategy();
            await strategy.ExecuteAsync(async () =>
            {
                using (var transaction = _dbContext.Database.BeginTransaction())
                {
                    try
                    {
                        var entity = _dbContext.Set<BPCFLIPHeader>().FirstOrDefault(x => x.FLIPID == FLIPHeader.FLIPID);
                        if (entity == null)
                        {
                            return entity;
                        }
                        //_dbContext.Entry(FLIPHeader).State = EntityState.Modified;
                        entity.InvoiceAmount = FLIPHeader.InvoiceAmount;
                        entity.InvoiceDate = FLIPHeader.InvoiceDate;
                        entity.ProfitCentre = FLIPHeader.ProfitCentre;
                        entity.GSTIN = FLIPHeader.GSTIN;
                        entity.InvoiceNumber = FLIPHeader.InvoiceNumber;
                        entity.InvoiceType = FLIPHeader.InvoiceType;
                        entity.InvoiceCurrency = FLIPHeader.InvoiceCurrency;
                        entity.InvoiceDocID = FLIPHeader.InvoiceDocID;
                        entity.IsInvoiceOrCertified = FLIPHeader.IsInvoiceOrCertified;
                        entity.IsInvoiceFlag = FLIPHeader.IsInvoiceFlag;
                        //entity.Client = FLIPHeader.Client;
                        //entity.Company = FLIPHeader.Company;
                        //entity.Type = FLIPHeader.Type;
                        //entity.PatnerID = FLIPHeader.PatnerID;
                        entity.DocNumber = FLIPHeader.DocNumber;
                        entity.InvoiceAttachmentName = FLIPHeader.InvoiceAttachmentName;
                        entity.ModifiedBy = FLIPHeader.ModifiedBy;
                        entity.ModifiedOn = DateTime.Now;
                        await _dbContext.SaveChangesAsync();

                        FLIPCostRepository FLIPCostRepository = new FLIPCostRepository(_dbContext);
                        await FLIPCostRepository.DeleteFLIPCostByFLIPID(FLIPHeader.FLIPID);
                        await FLIPCostRepository.CreateFLIPCosts(FLIPHeader.FLIPCosts, FLIPHeader.FLIPID);

                        await DeleteFLIPItemByFLIPID(FLIPHeader.FLIPID);
                        await CreateFLIPItems(FLIPHeader.FLIPItems, FLIPHeader.FLIPID);

                        await DeleteFlipAttachmentByDocNumber(FLIPHeader.DocNumber);
                        if (!string.IsNullOrEmpty(FLIPHeader.InvoiceAttachmentName))
                        {
                            BPCFLIPAttachment FLIPAttachment = new BPCFLIPAttachment();
                            FLIPAttachment.PO = FLIPHeader.DocNumber;
                            FLIPAttachment.FLIPID = FLIPHeader.FLIPID;
                            FLIPAttachment.AttachmentName = FLIPHeader.InvoiceAttachmentName;
                            BPCFLIPAttachment result1 = await attachmentRepository.AddAttachment(FLIPAttachment);
                            entity.InvoiceDocID = result1.AttachmentID.ToString();
                        }
                        CreateInvoiceXmlData(entity, FLIPHeader.FLIPItems, FLIPHeader.FLIPCosts);
                        transaction.Commit();
                        transaction.Dispose();
                        bPCFLIPHeaderResult = entity;
                        return bPCFLIPHeaderResult;
                    }
                    catch (SqlException ex)
                    {
                        WriteLog.WriteToFile("PORespository/UpdatePOFLIP", ex); throw new Exception("Something went wrong");
                    }
                    catch (InvalidOperationException ex)
                    {
                        WriteLog.WriteToFile("PORespository/UpdatePOFLIP", ex); throw new Exception("Something went wrong");
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        transaction.Dispose();
                        throw ex;
                    }
                }
            });
            return bPCFLIPHeaderResult;
        }
        public async Task DeleteFlipAttachmentByDocNumber(string DocNumber)
        {
            try
            {
                _dbContext.Set<BPCFLIPAttachment>().Where(x => x.PO == DocNumber).ToList().ForEach(x => _dbContext.Set<BPCFLIPAttachment>().Remove(x));
                await _dbContext.SaveChangesAsync();
            }
            catch (SqlException ex) { WriteLog.WriteToFile("PORespository/DeleteFlipAttachmentByDocNumber", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("PORespository/DeleteFlipAttachmentByDocNumber", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<BPCFLIPHeader> DeletePOFLIP(BPCFLIPHeaderView FLIPHeader)
        {
            var bPCFLIPHeaderResult = new BPCFLIPHeader();
            var strategy = _dbContext.Database.CreateExecutionStrategy();
            await strategy.ExecuteAsync(async () =>
            {
                using (var transaction = _dbContext.Database.BeginTransaction())
                {
                    try
                    {
                        var entity = _dbContext.Set<BPCFLIPHeader>().FirstOrDefault(x => x.FLIPID == FLIPHeader.FLIPID);
                        if (entity == null)
                        {
                            return entity;
                        }
                        _dbContext.Set<BPCFLIPHeader>().Remove(entity);
                        List<BPCFLIPItem> items = _dbContext.BPCFLIPItems.Where(x => x.FLIPID == entity.FLIPID).ToList();
                        foreach (BPCFLIPItem item in items)
                        {
                            _dbContext.BPCFLIPItems.Remove(item);
                        }
                        _dbContext.BPCFLIPCosts.Where(x => x.FLIPID == entity.FLIPID).ToList().ForEach(x => _dbContext.BPCFLIPCosts.Remove(x));
                        await _dbContext.SaveChangesAsync();
                        transaction.Commit();
                        transaction.Dispose();
                        bPCFLIPHeaderResult = entity;
                        return bPCFLIPHeaderResult;
                    }
                    catch (SqlException ex) { WriteLog.WriteToFile("PORespository/DeletePOFLIP", ex); throw new Exception("Something went wrong"); }
                    catch (InvalidOperationException ex) { WriteLog.WriteToFile("PORespository/DeletePOFLIP", ex); throw new Exception("Something went wrong"); }
                    catch (Exception ex)
                    {
                        transaction.Commit();
                        transaction.Dispose();
                        throw ex;
                    }
                }
            });
            return bPCFLIPHeaderResult;
        }

        public List<BPCFLIPItem> GetFLIPItemsByFLIPID(string FLIPID)
        {
            try
            {
                return _dbContext.BPCFLIPItems.Where(x => x.FLIPID == FLIPID).ToList();
            }
            catch (SqlException ex) { WriteLog.WriteToFile("PORespository/GetFLIPItemsByFLIPID", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("PORespository/GetFLIPItemsByFLIPID", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public bool CreateInvoiceXmlData(BPCFLIPHeader header, List<BPCFLIPItem> FLIPItems, List<BPCFLIPCost> FLIPCosts)
        {
            try
            {
                //WriteLog.WriteErrorLog("Enter the GetPRWithVendorDetails method");
                bool status = false;
                CreateOutboxTempFolder();
                Random r = new Random();
                int num = r.Next(1, 9999999);
                string writerFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Outbox");
                var FileName = header.Company + DateTime.Now.ToString("ddMMyyyyhh") + "_" + Regex.Replace(header.InvoiceNumber.Trim(), "[^A-Za-z0-9_. ]+", "") + ".xml";
                //var FileName = header.Company + DateTime.Now.ToString("ddMMyyyyhhmmss") + num + "_" + header.PatnerID + ".xml";
                string writerPath = Path.Combine(writerFolder, FileName);
                using (var sw = new StreamWriter(writerPath))
                {
                    XmlWriter writer = XmlWriter.Create(sw);

                    WriteLog.WriteToFile("Invoice Xml creation started");
                    writer.WriteStartDocument();
                    writer.WriteStartElement("INVOICEDATA");
                    writer.WriteElementString("VID", header.PatnerID);
                    writer.WriteElementString("COMPANYCODE", header.Company);
                    writer.WriteElementString("PO", header.DocNumber);
                    writer.WriteElementString("ProfitCentre", header.ProfitCentre);
                    writer.WriteElementString("GSTIN", header.GSTIN);
                    writer.WriteElementString("InvoiceNumber", header.InvoiceNumber);
                    writer.WriteElementString("InvoiceDate", header.InvoiceDate.HasValue ? header.InvoiceDate.Value.ToString("dd/MM/yyyy") : "");
                    writer.WriteElementString("ScanDate", header.InvoiceDate.HasValue ? header.InvoiceDate.Value.ToString("dd/MM/yyyy") : "");
                    writer.WriteElementString("TIME", header.CreatedOn.ToString("HH:mm"));
                    writer.WriteElementString("InvoiceCurrency", header.InvoiceCurrency);
                    writer.WriteElementString("InvoiceAmount", header.InvoiceAmount.ToString());
                    writer.WriteElementString("InvoiceType", header.InvoiceType);
                    writer.WriteElementString("InvoiceOrCertified", header.IsInvoiceOrCertified);


                    writer.WriteStartElement("INVOICEITEMS");
                    foreach (var item in FLIPItems)
                    {
                        writer.WriteStartElement("ITEM");
                        writer.WriteElementString("ITEM", item.Item);
                        writer.WriteElementString("ITEMText", item.MaterialText);
                        writer.WriteElementString("HSN", item.HSN);
                        //writer.WriteElementString("ORDERQTY", item.OrderedQty.ToString());
                        //writer.WriteElementString("OPENQTY", item.OpenQty.ToString());
                        writer.WriteElementString("INVOICEQTY", item.InvoiceQty.ToString());
                        writer.WriteElementString("PRICE", item.Price.ToString());
                        writer.WriteElementString("TAX", item.Tax.ToString());
                        writer.WriteElementString("AMOUNT", item.Amount.ToString());
                        writer.WriteEndElement();
                    }
                    writer.WriteEndElement();

                    writer.WriteStartElement("INVOICECOSTS");
                    foreach (var cost in FLIPCosts)
                    {
                        writer.WriteStartElement("COSTS");
                        writer.WriteElementString("TYPE", cost.Type);
                        writer.WriteElementString("AMOUNT", cost.Amount.ToString());
                        writer.WriteElementString("REMARKS", cost.Remarks);
                        writer.WriteEndElement();
                    }
                    writer.WriteEndElement();

                    writer.Flush();
                    writer.Close();
                    WriteLog.WriteToFile("PORepository/CreateInvoiceXmlData : - " + "Created Successfully");

                }
                var uploadStatus = UploadFileToVendorOutputFolder(writerFolder, FileName);
                if (uploadStatus == true)
                {
                    status = true;
                    WriteLog.WriteToFile("Registration/CreateXMLFromVendorOnBoarding" + " UploadFileToVendorOutputFolder Success");
                }
                else
                {
                    status = false;
                    WriteLog.WriteToFile("Registration/CreateXMLFromVendorOnBoarding" + " UploadFileToVendorOutputFolder Failure");
                }
                return status;
            }
            catch (SqlException ex) { WriteLog.WriteToFile("PORespository/CreateInvoiceXmlData", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("PORespository/CreateInvoiceXmlData", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("PORepository/CreateInvoiceXmlData/Exception : - " + ex.Message);
                return false;
            }

        }
        public static void CreateOutboxTempFolder()
        {
            try
            {
                //if folder - "\bin\debug\Internal_Folder" not exist,create folder "Internal_Folder" in debug
                string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Outbox");
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                else
                {
                    if (Directory.GetFiles(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Outbox")).Length > 0) //if file found in folder
                    {
                        string[] txtList = Directory.GetFiles(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Outbox"));
                        foreach (string f in txtList)
                        {
                            FileInfo fi = new FileInfo(f);
                            if (fi.CreationTime < DateTime.Now.AddMinutes(-30))
                            {
                                System.GC.Collect();
                                System.GC.WaitForPendingFinalizers();
                                fi.Delete();
                                //File.Delete(f);
                            }
                        }
                    }
                }

            }
            catch (SqlException ex) { WriteLog.WriteToFile("PORespository/CreateOutboxTempFolder", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("PORespository/CreateOutboxTempFolder", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("PORepository/CreateOutboxTempFolder : - ", ex);
            }
        }
        public bool UploadFileToVendorOutputFolder(string filePath, string fileName)
        {
            bool status = false;
            try
            {
                IConfiguration FTPDetailsConfig = _configuration.GetSection("FTPDetails");
                string FTPOutbox = FTPDetailsConfig.GetValue<string>("Outbox");
                string FTPUsername = FTPDetailsConfig.GetValue<string>("Username");
                string FTPPassword = FTPDetailsConfig.GetValue<string>("Password");
                using (WebClient client = new WebClient())
                {
                    if (Directory.GetFiles(filePath).Length > 0) //if file found in folder
                    {
                        DirectoryInfo dir = new DirectoryInfo(filePath);
                        FileInfo[] files = dir.GetFiles();
                        foreach (var file in files)
                        {
                            if (file.Length > 0)
                            {
                                client.Credentials = new NetworkCredential(FTPUsername, FTPPassword);
                                byte[] responseArray = client.UploadFile(FTPOutbox + file.Name, file.FullName);
                                WriteLog.WriteToFile("PORepository/UploadFileToVendorOutputFolder" + " File uploaded to Vendor Output folder");
                                status = true;
                                WriteLog.WriteToFile("PORepository/UploadFileToVendorOutputFolder" + " " + file.Name + " " + FTPOutbox);
                                System.IO.File.Delete(file.FullName);
                                //return status;
                            }
                            else
                            {
                                status = false;
                                WriteLog.WriteToFile("PORepository/UploadFileToVendorOutputFolder" + " " + "File has no contents" + " " + file.Name + " " + FTPOutbox);
                            }
                        }
                    }
                }
                return status;
            }

            catch (SqlException ex) { WriteLog.WriteToFile("PORespository/UploadFileToVendorOutputFolder", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("PORespository/UploadFileToVendorOutputFolder", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("PORepository/UploadFileToVendorOutputFolder/Exception", ex);
                return false;
            }
        }
        #endregion

        #region Data Migration

        public async Task CreateOFHeaders(List<BPCOFHeaderXLSX> OFHeaders)
        {
            try
            {
                if (OFHeaders != null && OFHeaders.Count > 0)
                {
                    foreach (BPCOFHeaderXLSX OFHeader in OFHeaders)
                    {
                        //await DeleteOFHeaderByPartnerIDAndType(OFHeader.PatnerID);
                        BPCOFHeader bPCOFHeader = new BPCOFHeader();
                        //mandatory fields start
                        bPCOFHeader.PatnerID = OFHeader.Partnerid;
                        bPCOFHeader.Client = OFHeader.Client;
                        bPCOFHeader.Company = OFHeader.Company;
                        bPCOFHeader.DocNumber = OFHeader.Docnumber;
                        bPCOFHeader.Type = OFHeader.Type;
                        //mandatory fields end
                        bPCOFHeader.DocVersion = OFHeader.Docversion;
                        bPCOFHeader.PlantName = OFHeader.Plantname;
                        bPCOFHeader.Status = OFHeader.Status;
                        bPCOFHeader.Currency = OFHeader.Currency;
                        bPCOFHeader.RefDoc = OFHeader.Refdocument;
                        DateTime dateTime;
                        CultureInfo provider = CultureInfo.InvariantCulture;
                        bool isSuccess = DateTime.TryParseExact(OFHeader.Docdate, "yyyyMMdd", provider, DateTimeStyles.None, out dateTime);
                        if (isSuccess)
                            bPCOFHeader.DocDate = dateTime;
                        bPCOFHeader.Currency = OFHeader.Currency;
                        bPCOFHeader.IsActive = true;
                        bPCOFHeader.CreatedOn = DateTime.Now;
                        var result = _dbContext.BPCOFHeaders.Add(bPCOFHeader);
                    }
                    await _dbContext.SaveChangesAsync();
                }
            }
            catch (SqlException ex) { WriteLog.WriteToFile("PORespository/CreateOFHeaders", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("PORespository/CreateOFHeaders", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task CreateOFItems(List<BPCOFItemXLSX> OFItems)
        {
            try
            {
                if (OFItems != null && OFItems.Count > 0)
                {
                    foreach (BPCOFItemXLSX OFItem in OFItems)
                    {
                        BPCOFItem bPCOFItem = new BPCOFItem();
                        //mandatory fields start
                        bPCOFItem.PatnerID = OFItem.Partnerid;
                        bPCOFItem.Client = OFItem.Client;
                        bPCOFItem.Company = OFItem.Company;
                        bPCOFItem.DocNumber = OFItem.Docnumber;
                        bPCOFItem.Type = OFItem.Type;
                        bPCOFItem.Item = OFItem.Itemnumber;
                        //mandatory fields end
                        bPCOFItem.Material = OFItem.Materialnumber;
                        bPCOFItem.MaterialText = OFItem.Materialtext;
                        bPCOFItem.UOM = OFItem.UOM;
                        bPCOFItem.OrderedQty = OFItem.Orderqty;
                        DateTime dateTime;
                        CultureInfo provider = CultureInfo.InvariantCulture;
                        bool isSuccess = DateTime.TryParseExact(OFItem.Deliverydate, "yyyyMMdd", provider, DateTimeStyles.None, out dateTime);
                        if (isSuccess)
                            bPCOFItem.DeliveryDate = dateTime;
                        bPCOFItem.PlantCode = OFItem.Plant;
                        if (!string.IsNullOrEmpty(OFItem.Unitprice))
                            bPCOFItem.UnitPrice = Convert.ToDouble(OFItem.Unitprice);
                        if (!string.IsNullOrEmpty(OFItem.Value))
                            bPCOFItem.Value = Convert.ToDouble(OFItem.Value);
                        if (!string.IsNullOrEmpty(OFItem.Taxamount))
                            bPCOFItem.TaxAmount = Convert.ToDouble(OFItem.Taxamount);
                        bPCOFItem.TaxCode = OFItem.Taxcode;
                        bPCOFItem.IsActive = true;
                        bPCOFItem.CreatedOn = DateTime.Now;
                        var result = _dbContext.BPCOFItems.Add(bPCOFItem);
                    }
                    await _dbContext.SaveChangesAsync();
                }
            }
            catch (SqlException ex) { WriteLog.WriteToFile("PORespository/CreateOFItems", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("PORespository/CreateOFItems", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task CreateOFScheduleLines(List<BPCOFScheduleLineXLSX> OFScheduleLines)
        {
            try
            {
                if (OFScheduleLines != null && OFScheduleLines.Count > 0)
                {
                    foreach (BPCOFScheduleLineXLSX OFScheduleLine in OFScheduleLines)
                    {
                        BPCOFScheduleLine bPCOFScheduleLine = new BPCOFScheduleLine();
                        //mandatory fields start
                        bPCOFScheduleLine.PatnerID = OFScheduleLine.Partnerid;
                        bPCOFScheduleLine.Client = OFScheduleLine.Client;
                        bPCOFScheduleLine.Company = OFScheduleLine.Company;
                        bPCOFScheduleLine.DocNumber = OFScheduleLine.Docnumber;
                        bPCOFScheduleLine.Type = OFScheduleLine.Type;
                        bPCOFScheduleLine.SlLine = OFScheduleLine.Sline;
                        bPCOFScheduleLine.Item = OFScheduleLine.Itemnumber;
                        //mandatory fields end
                        bPCOFScheduleLine.OrderedQty = OFScheduleLine.Orderquantity;
                        DateTime dateTime;
                        CultureInfo provider = CultureInfo.InvariantCulture;
                        bool isSuccess = DateTime.TryParseExact(OFScheduleLine.Deldate, "yyyyMMdd", provider, DateTimeStyles.None, out dateTime);
                        if (isSuccess)
                            bPCOFScheduleLine.DeliveryDate = dateTime;
                        bPCOFScheduleLine.IsActive = true;
                        bPCOFScheduleLine.CreatedOn = DateTime.Now;
                        var result = _dbContext.BPCOFScheduleLines.Add(bPCOFScheduleLine);
                    }
                    await _dbContext.SaveChangesAsync();
                }
            }
            catch (SqlException ex) { WriteLog.WriteToFile("PORespository/CreateOFScheduleLines", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("PORespository/CreateOFScheduleLines", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task CreateOFGRGIs(List<BPCOFGRGIXLSX> OFGRGIs)
        {
            try
            {
                if (OFGRGIs != null && OFGRGIs.Count > 0)
                {
                    foreach (BPCOFGRGIXLSX OFGRGI in OFGRGIs)
                    {
                        BPCOFGRGI bPCOFGRGI = new BPCOFGRGI();
                        //mandatory fields start
                        bPCOFGRGI.PatnerID = OFGRGI.Partnerid;
                        bPCOFGRGI.Client = OFGRGI.Client;
                        bPCOFGRGI.Company = OFGRGI.Company;
                        bPCOFGRGI.DocNumber = OFGRGI.Docnumber;
                        bPCOFGRGI.Type = OFGRGI.Type;
                        bPCOFGRGI.GRGIDoc = OFGRGI.Grgidoc;
                        bPCOFGRGI.Item = OFGRGI.Item;
                        //mandatory fields end
                        bPCOFGRGI.Material = OFGRGI.Material;
                        bPCOFGRGI.MaterialText = OFGRGI.Materialtext;
                        //bPCOFGRGI. = OFGRGI.Unit;  // not present in our db
                        bPCOFGRGI.GRGIQty = OFGRGI.Grgiqty;
                        DateTime dateTime;
                        CultureInfo provider = CultureInfo.InvariantCulture;
                        bool isSuccess = DateTime.TryParseExact(OFGRGI.Deliverydate, "yyyyMMdd", provider, DateTimeStyles.None, out dateTime);
                        if (isSuccess)
                            bPCOFGRGI.DeliveryDate = dateTime;
                        bPCOFGRGI.ShippingPartner = OFGRGI.Shippingpartner;
                        bPCOFGRGI.IsActive = true;
                        bPCOFGRGI.CreatedOn = DateTime.Now;
                        var result = _dbContext.BPCOFGRGIs.Add(bPCOFGRGI);
                        //Update the status of BPC_OF_Headers has completed
                        int a = await _dbContext.SaveChangesAsync();
                        if (a > 0)
                        {
                            await UpdateStatusOfOFHeader(OFGRGI);
                        }
                    }
                }
            }
            catch (SqlException ex) { WriteLog.WriteToFile("PORespository/CreateOFGRGIs", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("PORespository/CreateOFGRGIs", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task CreateOFQMs(List<BPCOFQMXLSX> OFQMs)
        {
            try
            {
                if (OFQMs != null && OFQMs.Count > 0)
                {
                    foreach (BPCOFQMXLSX OFQM in OFQMs)
                    {
                        BPCOFQM bPCOFQM = new BPCOFQM();
                        //mandatory fields start
                        bPCOFQM.PatnerID = OFQM.Patnerid;
                        bPCOFQM.Client = OFQM.Client;
                        bPCOFQM.Company = OFQM.Company;
                        bPCOFQM.Type = OFQM.Type;
                        bPCOFQM.Item = OFQM.Item;
                        bPCOFQM.DocNumber = OFQM.Docnumber;
                        //mandatory fields end
                        bPCOFQM.Material = OFQM.Material;
                        bPCOFQM.MaterialText = OFQM.MaterialText;
                        //bPCOFQM.i = OFQM.Insplotno; // not present in our db
                        //bPCOFQM. = OFQM.Unit;// not present in our db
                        bPCOFQM.LotQty = OFQM.Lotqty;
                        bPCOFQM.RejQty = OFQM.Rejqty;
                        bPCOFQM.RejReason = OFQM.Rejreason;
                        bPCOFQM.GRGIQty = OFQM.Grgiqty;
                        //DateTime dateTime;
                        //CultureInfo provider = CultureInfo.InvariantCulture;
                        //bool isSuccess = DateTime.TryParseExact(OFQM.Date, "yyyyMMdd", provider, DateTimeStyles.None, out dateTime);
                        //if (isSuccess)
                        //    bPCOFQM.D = dateTime;
                        bPCOFQM.IsActive = true;
                        bPCOFQM.CreatedOn = DateTime.Now;
                        var result = _dbContext.BPCOFQMs.Add(bPCOFQM);
                    }
                    await _dbContext.SaveChangesAsync();
                }
            }
            catch (SqlException ex) { WriteLog.WriteToFile("PORespository/CreateOFQMs", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("PORespository/CreateOFQMs", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task CreatePAYASs(List<BPCPAYASXLSX> PAYASs)
        {
            try
            {
                if (PAYASs != null && PAYASs.Count > 0)
                {
                    foreach (BPCPAYASXLSX PAYAS in PAYASs)
                    {
                        BPCPayAccountStatement bPCPayAccountStatement = new BPCPayAccountStatement();
                        //mandatory fields start
                        bPCPayAccountStatement.PartnerID = PAYAS.PartnerID;
                        bPCPayAccountStatement.Client = PAYAS.Client;
                        bPCPayAccountStatement.Company = PAYAS.Company;
                        bPCPayAccountStatement.Type = PAYAS.Type;
                        bPCPayAccountStatement.FiscalYear = PAYAS.FiscalYear;
                        //mandatory fields end
                        bPCPayAccountStatement.DocumentNumber = PAYAS.DocumentNumber;
                        bPCPayAccountStatement.DocumentDate = PAYAS.DocumentDate;
                        bPCPayAccountStatement.InvoiceNumber = PAYAS.InvoiceNumber;
                        bPCPayAccountStatement.InvoiceDate = PAYAS.InvoiceDate;
                        bPCPayAccountStatement.InvoiceAmount = PAYAS.InvoiceAmount;
                        bPCPayAccountStatement.PaidAmount = PAYAS.PaidAmount;
                        bPCPayAccountStatement.Reference = PAYAS.Reference;
                        //DateTime dateTime;
                        //CultureInfo provider = CultureInfo.InvariantCulture;
                        //bool isSuccess = DateTime.TryParseExact(PAYAS.DocumentDate, "yyyyMMdd", provider, DateTimeStyles.None, out dateTime);
                        //if (isSuccess)
                        //    bPCPayAccountStatement.DocumentDate = dateTime;
                        //DateTime dateTime1;
                        //bool isSuccess1 = DateTime.TryParseExact(PAYAS.PostingDate, "yyyyMMdd", provider, DateTimeStyles.None, out dateTime1);
                        //if (isSuccess1)
                        //    bPCPayAccountStatement.PostingDate = dateTime1;
                        bPCPayAccountStatement.IsActive = true;
                        bPCPayAccountStatement.CreatedOn = DateTime.Now;
                        var result = _dbContext.BPCPayAccountStatements.Add(bPCPayAccountStatement);
                    }
                    await _dbContext.SaveChangesAsync();
                }
            }
            catch (SqlException ex) { WriteLog.WriteToFile("PORespository/CreatePAYASs", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("PORespository/CreatePAYASs", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task CreatePAYPAYMENTs(List<BPCPAYPAYMENTXLSX> PAYPAYMENTs)
        {
            try
            {
                if (PAYPAYMENTs != null && PAYPAYMENTs.Count > 0)
                {
                    foreach (BPCPAYPAYMENTXLSX PAYPAYMENT in PAYPAYMENTs)
                    {
                        BPCPayPayment bPCPayPayment = new BPCPayPayment();
                        //mandatory fields start
                        bPCPayPayment.PartnerID = PAYPAYMENT.PartnerID;
                        bPCPayPayment.Client = PAYPAYMENT.Client;
                        bPCPayPayment.Company = PAYPAYMENT.Company;
                        bPCPayPayment.Type = PAYPAYMENT.Type;
                        bPCPayPayment.FiscalYear = PAYPAYMENT.FiscalYear;
                        //mandatory fields end
                        bPCPayPayment.DocumentNumber = PAYPAYMENT.DocumentNumber;
                        bPCPayPayment.PaymentType = PAYPAYMENT.PaymentType;
                        bPCPayPayment.PaidAmount = PAYPAYMENT.PaidAmount;
                        bPCPayPayment.BankName = PAYPAYMENT.BankName;
                        bPCPayPayment.BankAccount = PAYPAYMENT.BankAccount;
                        DateTime dateTime;
                        CultureInfo provider = CultureInfo.InvariantCulture;
                        bool isSuccess = DateTime.TryParseExact(PAYPAYMENT.PaymentDate, "yyyyMMdd", provider, DateTimeStyles.None, out dateTime);
                        if (isSuccess)
                            bPCPayPayment.PaymentDate = dateTime;
                        bPCPayPayment.IsActive = true;
                        bPCPayPayment.CreatedOn = DateTime.Now;
                        var result = _dbContext.BPCPayPayments.Add(bPCPayPayment);
                    }
                    await _dbContext.SaveChangesAsync();
                }
            }
            catch (SqlException ex) { WriteLog.WriteToFile("PORespository/CreatePAYPAYMENTs", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("PORespository/CreatePAYPAYMENTs", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task UpdateStatusOfOFHeader(BPCOFGRGIXLSX bPCOFGRGIXLSX)
        {
            try
            {
                if (bPCOFGRGIXLSX != null)
                {
                    var entity = _dbContext.BPCOFHeaders.Where(x => x.DocNumber.ToLower() == bPCOFGRGIXLSX.Docnumber.ToLower()
                   && x.PatnerID.ToLower() == bPCOFGRGIXLSX.Partnerid.ToLower() && x.Client.ToLower() == bPCOFGRGIXLSX.Client.ToLower()
                   && x.Company.ToLower() == bPCOFGRGIXLSX.Company.ToLower() && x.Type.ToLower() == bPCOFGRGIXLSX.Type.ToLower()).FirstOrDefault();
                    // var entity = _dbContext.BPCOFHeaders.Where(x => x.DocNumber.ToLower() == bPCOFGRGIXLSX.Docnumber.ToLower()
                    //&& x.PatnerID.ToLower() == bPCOFGRGIXLSX.Partnerid.ToLower()).FirstOrDefault();
                    if (entity != null)
                    {
                        entity.Status = "Completed";
                        await _dbContext.SaveChangesAsync();
                    }
                }
            }
            catch (SqlException ex) { WriteLog.WriteToFile("PORespository/UpdateStatusOfOFHeader", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("PORespository/UpdateStatusOfOFHeader", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task UpdateGRGIStatusOfOFHeader(BPCOFGRGI bPCOFGRGIXLSX)
        {
            var log = new BPCLog();
            try
            {
                log = await CreateBPCLog("UpdateGRGIStatusOfOFHeader", 1);
                if (bPCOFGRGIXLSX != null)
                {
                    var entity = _dbContext.BPCOFHeaders.Where(x => x.DocNumber.ToLower() == bPCOFGRGIXLSX.DocNumber.ToLower()
                   && x.PatnerID.ToLower() == bPCOFGRGIXLSX.PatnerID.ToLower() && x.Client.ToLower() == bPCOFGRGIXLSX.Client.ToLower()
                   && x.Company.ToLower() == bPCOFGRGIXLSX.Company.ToLower() && x.Type.ToLower() == bPCOFGRGIXLSX.Type.ToLower()).FirstOrDefault();
                    // var entity = _dbContext.BPCOFHeaders.Where(x => x.DocNumber.ToLower() == bPCOFGRGIXLSX.Docnumber.ToLower()
                    //&& x.PatnerID.ToLower() == bPCOFGRGIXLSX.Partnerid.ToLower()).FirstOrDefault();
                    if (entity != null)
                    {
                        bool IsNonZeroOpenQty = false;
                        bool isAllGateEntryCompleted = true;

                        var items = _dbContext.BPCOFItems.Where(x => x.Client == bPCOFGRGIXLSX.Client && x.Type == bPCOFGRGIXLSX.Type && x.Company == bPCOFGRGIXLSX.Company && x.PatnerID == bPCOFGRGIXLSX.PatnerID && x.DocNumber == bPCOFGRGIXLSX.DocNumber).ToList();
                        var ASNLists = _dbContext.BPCASNHeaders.Where(x => x.Client == bPCOFGRGIXLSX.Client && x.Type == bPCOFGRGIXLSX.Type && x.Company == bPCOFGRGIXLSX.Company && x.PatnerID == bPCOFGRGIXLSX.PatnerID && x.DocNumber == bPCOFGRGIXLSX.DocNumber).ToList();
                        var GRNLists = _dbContext.BPCOFGRGIs.Where(x => x.Client == bPCOFGRGIXLSX.Client && x.Type == bPCOFGRGIXLSX.Type && x.Company == bPCOFGRGIXLSX.Company && x.PatnerID == bPCOFGRGIXLSX.PatnerID && x.DocNumber == bPCOFGRGIXLSX.DocNumber).ToList();

                        foreach (var item in items)
                        {
                            if (item.OpenQty.HasValue && item.OpenQty.Value > 0)
                            {
                                IsNonZeroOpenQty = true;
                            }
                        }
                        foreach (var asnl in ASNLists)
                        {
                            if (asnl.Status.ToLower() != "gateentry completed")
                            {
                                isAllGateEntryCompleted = false;
                            }
                        }
                        if (IsNonZeroOpenQty)
                        {
                            entity.Status = "PartialASN";
                        }
                        else
                        {
                            //header.Status = "PartialASN";
                            entity.Status = isAllGateEntryCompleted ? GRNLists.Count > 0 ? bPCOFGRGIXLSX.IsFinal ? "Completed" : "PartialGRN" : "DueForGRN" : "PartialGate";
                        }

                        //if (bPCOFGRGIXLSX.IsFinal)
                        //{
                        //    entity.Status = "Completed";
                        //}
                        //else
                        //{
                        //    entity.Status = "PartialGRN";
                        //}

                        //await UpdateBPCSucessLog(log.LogID);
                        await _dbContext.SaveChangesAsync();
                    }
                }
                if (log != null)
                {
                    await UpdateBPCSucessLog(log.LogID);
                }
                else
                {
                    WriteLog.WriteToGRGIFile("PO/UpdateGRGIStatusOfOFHeader--" + "Unable to generate Log");
                }
            }
            catch (SqlException ex) { WriteLog.WriteToGRGIFile("PORespository/UpdateGRGIStatusOfOFHeader", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToGRGIFile("PORespository/UpdateGRGIStatusOfOFHeader", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                if (log != null)
                {
                    await UpdateBPCFailureLog(log.LogID, ex.Message);
                }
                else
                {
                    WriteLog.WriteToGRGIFile("PO/UpdateGRGIStatusOfOFHeader--" + "Unable to generate Log");
                }

                throw ex;
            }
        }

        public async Task UpdateCancelGRGIStatusOfOFHeader(BPCOFGRGI BPCOFGRGI)
        {
            try
            {
                if (BPCOFGRGI != null)
                {
                    var entity = _dbContext.BPCOFHeaders.Where(x => x.DocNumber.ToLower() == BPCOFGRGI.DocNumber.ToLower()
                   && x.PatnerID.ToLower() == BPCOFGRGI.PatnerID.ToLower() && x.Client.ToLower() == BPCOFGRGI.Client.ToLower()
                   && x.Company.ToLower() == BPCOFGRGI.Company.ToLower() && x.Type.ToLower() == BPCOFGRGI.Type.ToLower()).FirstOrDefault();

                    var grns = _dbContext.BPCOFGRGIs.Where(x => x.Client == BPCOFGRGI.Client && x.Company == BPCOFGRGI.Company && x.Type == BPCOFGRGI.Type
                                                                    && x.DocNumber == BPCOFGRGI.DocNumber).ToList();

                    if (grns.Count > 0)
                    {
                        entity.Status = "PartialGRN";
                    }
                    else
                    {
                        entity.Status = "GueForGRN";
                    }
                    await _dbContext.SaveChangesAsync();
                }
            }
            catch (SqlException ex) { WriteLog.WriteToGRGIFile("PORespository/UpdateCancelGRGIStatusOfOFHeader", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToGRGIFile("PORespository/UpdateCancelGRGIStatusOfOFHeader", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public async Task CreateBPCOFGRGI(List<BPCOFGRGI> data)
        {
            var log = new BPCLog();
            try
            {
                log = await CreateBPCLog("CreateBPCOFGRGI", data.Count);
                WriteLog.WriteToGRGIFile($"PORepository/CreateBPCOFGRGI:-{data.Count} GRGI records found");
                foreach (BPCOFGRGI BPCOFGRGI in data)
                {

                    var grn = _dbContext.BPCOFGRGIs.Where(x => x.Client == BPCOFGRGI.Client && x.Company == BPCOFGRGI.Company && x.Type == BPCOFGRGI.Type
                                                 && x.DocNumber == BPCOFGRGI.DocNumber && x.GRGIDoc == BPCOFGRGI.GRGIDoc && x.Item == BPCOFGRGI.Item).FirstOrDefault();
                    if (grn == null)
                    {
                        WriteLog.WriteToGRGIFile($"PORepository/CreateBPCOFGRGI:- Trying to insert GRGI for {BPCOFGRGI.PatnerID} - {BPCOFGRGI.DocNumber} - {BPCOFGRGI.GRGIDoc} - {BPCOFGRGI.Item}");

                        BPCOFGRGI BPCdata = new BPCOFGRGI();
                        BPCdata.Client = BPCOFGRGI.Client;
                        BPCdata.Company = BPCOFGRGI.Company;
                        BPCdata.Type = BPCOFGRGI.Type;
                        BPCdata.PatnerID = BPCOFGRGI.PatnerID;
                        BPCdata.DocNumber = BPCOFGRGI.DocNumber;
                        BPCdata.GRGIDoc = BPCOFGRGI.GRGIDoc;
                        BPCdata.Item = BPCOFGRGI.Item;
                        BPCdata.Description = BPCOFGRGI.Description;
                        BPCdata.Material = BPCOFGRGI.Material;
                        BPCdata.MaterialText = BPCOFGRGI.MaterialText;
                        BPCdata.DeliveryDate = BPCOFGRGI.DeliveryDate;
                        BPCdata.GRIDate = BPCOFGRGI.GRIDate;
                        BPCdata.GRGIQty = BPCOFGRGI.GRGIQty;
                        BPCdata.ShippingDoc = BPCOFGRGI.ShippingDoc;
                        BPCdata.ShippingPartner = BPCOFGRGI.ShippingPartner;
                        BPCdata.CreatedBy = "SAP API";
                        BPCdata.CreatedOn = DateTime.Now;
                        BPCdata.IsActive = true;
                        _dbContext.BPCOFGRGIs.AddRange(BPCdata);
                        await _dbContext.SaveChangesAsync();
                        await UpdateGRGIStatusOfOFHeader(BPCdata);

                        //await UpdateBPCSucessLog(log.LogID);
                        WriteLog.WriteToGRGIFile($"PORepository/CreateBPCOFGRGI:-GRN inserted for {BPCOFGRGI.PatnerID} - {BPCOFGRGI.DocNumber} - {BPCOFGRGI.GRGIDoc}");
                    }
                    else
                    {
                        await UpdateBPCFailureLog(log.LogID, $"PORepository/CreateBPCOFGRGI:- GRGI details already exist for {BPCOFGRGI.PatnerID} - {BPCOFGRGI.DocNumber} - {BPCOFGRGI.GRGIDoc} - {BPCOFGRGI.Item}");

                        WriteLog.WriteToGRGIFile($"PORepository/CreateBPCOFGRGI:- GRGI details already exist for {BPCOFGRGI.PatnerID} - {BPCOFGRGI.DocNumber} - {BPCOFGRGI.GRGIDoc} - {BPCOFGRGI.Item}");

                        await UpdateBPCOFGRGI(BPCOFGRGI);
                    }

                }
                if (log != null)
                {
                    await UpdateBPCSucessLog(log.LogID);
                }
                else
                {
                    WriteLog.WriteToGRGIFile("PO/CreateBPCOFGRGI--" + "Unable to generate Log");
                }
            }
            catch (SqlException ex) { WriteLog.WriteToGRGIFile("PORespository/CreateBPCOFGRGI", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToGRGIFile("PORespository/CreateBPCOFGRGI", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                if (log != null)
                {
                    await UpdateBPCFailureLog(log.LogID, ex.Message);
                }
                else
                {
                    WriteLog.WriteToGRGIFile("PO/CreateBPCOFGRGI--" + "Unable to generate Log");
                }

                throw ex;
            }

        }
        public async Task UpdateBPCOFGRGI(BPCOFGRGI BPCOFGRGI)
        {
            var log = new BPCLog();
            try
            {
                log = await CreateBPCLog("UpdateBPCOFGRGI", 1);
                var BPCdata = _dbContext.BPCOFGRGIs.Where(x => x.Client == BPCOFGRGI.Client && x.Company == BPCOFGRGI.Company && x.Type == BPCOFGRGI.Type
                                                 && x.DocNumber == BPCOFGRGI.DocNumber && x.GRGIDoc == BPCOFGRGI.GRGIDoc && x.Item == BPCOFGRGI.Item).FirstOrDefault();
                if (BPCdata != null)
                {
                    //BPCdata.Item = BPCOFGRGI.Item;
                    BPCdata.Description = BPCOFGRGI.Description;
                    BPCdata.Material = BPCOFGRGI.Material;
                    BPCdata.MaterialText = BPCOFGRGI.MaterialText;
                    BPCdata.DeliveryDate = BPCOFGRGI.DeliveryDate;
                    BPCdata.GRIDate = BPCOFGRGI.GRIDate;
                    BPCdata.GRGIQty = BPCOFGRGI.GRGIQty;
                    BPCdata.ShippingDoc = BPCOFGRGI.ShippingDoc;
                    BPCdata.ShippingPartner = BPCOFGRGI.ShippingPartner;
                    BPCdata.ModifiedBy = "SAP API";
                    BPCdata.ModifiedOn = DateTime.Now;
                    await _dbContext.SaveChangesAsync();
                    await UpdateGRGIStatusOfOFHeader(BPCdata);
                    //await UpdateBPCSucessLog(log.LogID);
                }
                if (log != null)
                {
                    await UpdateBPCSucessLog(log.LogID);
                }
                else
                {
                    WriteLog.WriteToGRGIFile("PO/UpdateBPCOFGRGI--" + "Unable to generate Log");
                }
            }
            catch (SqlException ex) { WriteLog.WriteToGRGIFile("PORespository/CreateBPCOFGRGI", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToGRGIFile("PORespository/CreateBPCOFGRGI", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                if (log != null)
                {
                    await UpdateBPCFailureLog(log.LogID, ex.Message);
                }
                else
                {
                    WriteLog.WriteToGRGIFile("PO/UpdateBPCOFGRGI--" + "Unable to generate Log");
                }

                throw ex;
            }

        }

        public async Task CancelBPCOFGRGI(List<BPCOFGRGI> data)
        {
            try
            {
                foreach (BPCOFGRGI BPCOFGRGI in data)
                {
                    var grn = _dbContext.BPCOFGRGIs.Where(x => x.Client == BPCOFGRGI.Client && x.Company == BPCOFGRGI.Company && x.Type == BPCOFGRGI.Type
                                                 && x.DocNumber == BPCOFGRGI.DocNumber && x.GRGIDoc == BPCOFGRGI.GRGIDoc).FirstOrDefault();
                    if (grn != null)
                    {
                        _dbContext.BPCOFGRGIs.Remove(grn);
                        await _dbContext.SaveChangesAsync();
                        await UpdateCancelGRGIStatusOfOFHeader(grn);
                    }
                }
            }
            catch (SqlException ex) { WriteLog.WriteToGRGIFile("PORespository/CancelBPCOFGRGI", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToGRGIFile("PORespository/CancelBPCOFGRGI", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public List<BPCOFGRGI> GetGRRByPartnerId(string partnerId)
        {
            return _dbContext.BPCOFGRGIs.Where(x => x.PatnerID == partnerId).ToList();
        }


        public BPCOFScheduleLine GetBPCSLByDocNumber(string DocNumber)
        {
            try
            {
                var result = _dbContext.BPCOFScheduleLines.FirstOrDefault(x => x.DocNumber == DocNumber);
                return result;
            }
            catch (SqlException ex) { WriteLog.WriteToFile("PORespository/GetBPCSLByDocNumber", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("PORespository/GetBPCSLByDocNumber", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task CreateBPCH(List<BPCOFHeaderWithAttachments> data)
        {
            var log = new BPCLog();
            try
            {
                List<BPCAIACT> acts = new List<BPCAIACT>();
                log = await CreateBPCLog("CreateBPCH", data.Count);
                foreach (BPCOFHeader bpc_sl in data)
                {
                    WriteLog.WriteToFile($"data from sap - {bpc_sl}");
                    WriteLog.WriteToFile($"Trying to insert data with Doc Number - {bpc_sl.DocNumber}");
                    BPCOFHeader items = _dbContext.BPCOFHeaders.FirstOrDefault(x => x.Client == bpc_sl.Client && x.Company == bpc_sl.Company && x.Type == bpc_sl.Type && x.PatnerID == bpc_sl.PatnerID && x.DocNumber == bpc_sl.DocNumber);
                    if (items == null)
                    {
                        WriteLog.WriteToFile($"item not found - {bpc_sl.DocNumber}");
                        bpc_sl.IsActive = true;
                        bpc_sl.CreatedOn = DateTime.Now;
                        bpc_sl.CreatedBy = "SAP API";
                        _dbContext.BPCOFHeaders.Add(bpc_sl);
                        await _dbContext.SaveChangesAsync();

                        BPCAIACT bPCAIACT = new BPCAIACT();
                        bPCAIACT.Client = bpc_sl.Client;
                        bPCAIACT.Company = bpc_sl.Company;
                        bPCAIACT.Type = bpc_sl.Type;
                        bPCAIACT.PatnerID = bpc_sl.PatnerID;
                        bPCAIACT.ActType = "01"; // Action
                        bPCAIACT.AppID = "01"; // PO
                        bPCAIACT.DocNumber = bpc_sl.DocNumber;
                        bPCAIACT.Action = "Accept";
                        bPCAIACT.ActionText = $"PO {bpc_sl.DocNumber} need to be accepted";
                        bPCAIACT.Status = "Open";
                        bPCAIACT.Date = DateTime.Now;
                        bPCAIACT.Time = DateTime.Now.ToString("hh:mm tt");
                        bPCAIACT.IsSeen = false;
                        acts.Add(bPCAIACT);
                    }
                    else
                    {
                        WriteLog.WriteToFile($"item  found - {bpc_sl.DocNumber}");
                      //  WriteLog.WriteToFile($"item  found - {bpc_sl.MaterialType}");
                        items.DocDate = bpc_sl.DocDate;
                        items.DocVersion = bpc_sl.DocVersion;
                        items.Currency = bpc_sl.Currency;
                        items.Status = bpc_sl.Status;
                        items.CrossAmount = bpc_sl.CrossAmount;
                        items.NetAmount = bpc_sl.NetAmount;
                        items.RefDoc = bpc_sl.RefDoc;
                        items.AckStatus = bpc_sl.AckStatus;
                        items.AckRemark = bpc_sl.AckRemark;
                        items.AckDate = bpc_sl.AckDate;
                        items.AckUser = bpc_sl.AckUser;
                        items.PINNumber = bpc_sl.PINNumber;
                        items.DocType = bpc_sl.DocType;
                        items.PlantName = bpc_sl.PlantName;
                        items.ReleasedStatus = bpc_sl.ReleasedStatus;
                        items.ModifiedOn = DateTime.Now;
                        items.ModifiedBy = "SAP API";
                       // items.MaterialType = bpc_sl.MaterialType;
                        await _dbContext.SaveChangesAsync();
                    }

                }

                if (acts.Count > 0)
                {
                    await CreateAIACTs(acts);
                }
                foreach (var header in data)
                {
                    //var i = 1;
                    //foreach (var file in header.Attachments)
                    //{
                    //    var fileToUpload = new BPCAttachment();
                    //    var type = GetFileExtension(file);
                    //    fileToUpload.Client = header.Client;
                    //    fileToUpload.Company = header.Company;
                    //    fileToUpload.Type = header.Type;
                    //    fileToUpload.PatnerID = header.PatnerID;
                    //    fileToUpload.ReferenceNo = header.DocNumber;
                    //    fileToUpload.ContentType = "application/" + GetFileExtension(file);
                    //    fileToUpload.AttachmentName = header.DocNumber + $"_{i}.{type}";
                    //    fileToUpload.AttachmentFile = DecodeUrlBase64(file);
                    //    fileToUpload.ContentLength = fileToUpload.AttachmentFile.Length;
                    //    fileToUpload.IsActive = true;
                    //    fileToUpload.CreatedOn = DateTime.Now;
                    //    var result = _dbContext.BPCAttachments.Add(fileToUpload);
                    //    i++;
                    //    await _dbContext.SaveChangesAsync();
                    //}
                }
                if (log != null)
                {
                    await UpdateBPCSucessLog(log.LogID);
                }
                else
                {
                    WriteLog.WriteToFile("PO/CreateBPCH--" + "Unable to generate Log");
                }
            }
            catch (SqlException ex) { WriteLog.WriteToFile("PORespository/CreateBPCH", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("PORespository/CreateBPCH", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                if (log != null)
                {
                    await UpdateBPCFailureLog(log.LogID, ex.Message);
                }
                else
                {
                    WriteLog.WriteToFile("PO/CreateBPCH--" + "Unable to generate Log");
                }
                //await UpdateBPCFailureLog(log.LogID, ex.Message);
                throw ex;
            }
        }

        public async Task UpdateBPCHGateStatus(List<BPCOFHeader_Gate> data)
        {
            var log = new BPCLog();
            try
            {
                log = await CreateBPCLog("UpdateBPCHGateStatus", data.Count);
                foreach (BPCOFHeader_Gate bpc_sl in data)
                {
                    BPCOFHeader items = _dbContext.BPCOFHeaders.FirstOrDefault(x => x.Client == bpc_sl.Client && x.Company == bpc_sl.Company && x.Type == bpc_sl.Type && x.PatnerID == bpc_sl.PatnerID && x.DocNumber == bpc_sl.DocNumber);
                    if (items != null)
                    {
                        items.Status = "DueForGRN";
                        items.ModifiedBy = bpc_sl.ModifiedBy;
                        items.ModifiedOn = DateTime.Now;
                        //await UpdateBPCSucessLog(log.LogID);
                        await _dbContext.SaveChangesAsync();
                    }
                    else
                    {
                        await UpdateBPCFailureLog(log.LogID, $"{bpc_sl.DocNumber} not exists");
                        throw new Exception($"{bpc_sl.DocNumber} not exists");
                    }

                }
                if (log != null)
                {
                    await UpdateBPCSucessLog(log.LogID);
                }
                else
                {
                    WriteLog.WriteToFile("PO/UpdateBPCHGateStatus--" + "Unable to generate Log");
                }
            }
            catch (SqlException ex) { WriteLog.WriteToFile("PORespository/UpdateBPCHGateStatus", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("PORespository/UpdateBPCHGateStatus", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                if (log != null)
                {
                    await UpdateBPCFailureLog(log.LogID, ex.Message);
                }
                else
                {
                    WriteLog.WriteToFile("PO/UpdateBPCHGateStatus--" + "Unable to generate Log");
                }
                throw ex;
            }
        }


        public async Task CreateBPCItems(List<BPCOFItem> data)
        {
            var log = new BPCLog();
            try
            {
                if (data != null)
                {
                    log = await CreateBPCLog("CreateBPCItems", data.Count);
                    foreach (BPCOFItem bpc_item in data)
                    {
                        BPCOFHeader header = _dbContext.BPCOFHeaders.Where(x => x.Client == bpc_item.Client && x.Company == bpc_item.Company && x.Type == bpc_item.Type && x.PatnerID == bpc_item.PatnerID && x.DocNumber == bpc_item.DocNumber).FirstOrDefault();
                        if (header != null)
                        {
                            header.Plant = bpc_item.PlantCode;
                        }
                        BPCOFItem items = _dbContext.BPCOFItems.Where(x => x.Client == bpc_item.Client && x.Company == bpc_item.Company && x.Type == bpc_item.Type && x.PatnerID == bpc_item.PatnerID && x.DocNumber == bpc_item.DocNumber && x.Item == bpc_item.Item).FirstOrDefault();
                        if (items == null)
                        {
                            if (!string.IsNullOrEmpty(bpc_item.Flag) && bpc_item.Flag.ToLower() == "x")
                            {
                                //_dbContext.BPCOFItems.Remove(items);
                                //await _dbContext.SaveChangesAsync();
                            }
                            else
                            {
                                bpc_item.IsActive = true;
                                bpc_item.CreatedOn = DateTime.Now;
                                bpc_item.CreatedBy = "SAP API";
                                _dbContext.BPCOFItems.Add(bpc_item);
                                await _dbContext.SaveChangesAsync();
                            }
                        }
                        else
                        {
                            if (!string.IsNullOrEmpty(bpc_item.Flag) && bpc_item.Flag.ToLower() == "x")
                            {
                                _dbContext.BPCOFItems.Remove(items);
                                await _dbContext.SaveChangesAsync();
                            }
                            else
                            {
                                bool IsDelivery = false;
                                bool IsRate = false;
                                bool IsQty = false;

                                WriteLog.WriteToFile($"item  found - {bpc_item.MaterialType}");
                                items.Material = bpc_item.Material;
                                items.MaterialText = bpc_item.MaterialText;

                                items.MaterialType = bpc_item.MaterialType;
                                IsDelivery = bpc_item.DeliveryDate.HasValue && (items.DeliveryDate != bpc_item.DeliveryDate);
                                items.DeliveryDate = bpc_item.DeliveryDate;

                                IsQty = items.OrderedQty != bpc_item.OrderedQty;
                                items.OrderedQty = bpc_item.OrderedQty;

                                items.CompletedQty = bpc_item.CompletedQty;
                                items.TransitQty = bpc_item.TransitQty;
                                items.OpenQty = bpc_item.OpenQty;
                                items.UOM = bpc_item.UOM;
                                items.HSN = bpc_item.HSN;
                                items.IsClosed = bpc_item.IsClosed;
                                items.AckStatus = bpc_item.AckStatus;
                                items.AckDeliveryDate = bpc_item.AckDeliveryDate;
                                items.PlantCode = bpc_item.PlantCode;

                                IsRate = items.UnitPrice != bpc_item.UnitPrice;
                                items.UnitPrice = bpc_item.UnitPrice;

                                items.Value = bpc_item.Value;
                                items.TaxAmount = bpc_item.TaxAmount;
                                items.TaxCode = bpc_item.TaxCode;
                                items.IsFreightAvailable = bpc_item.IsFreightAvailable;
                                items.FreightAmount = bpc_item.FreightAmount;
                                items.ModifiedOn = DateTime.Now;
                                items.ModifiedBy = "SAP API";
                                await _dbContext.SaveChangesAsync();

                                if (IsDelivery || IsRate || IsQty)
                                    await SendPOItemChangeNotification(items, IsDelivery, IsRate, IsQty);
                            }
                        }
                    }
                    if (log != null)
                    {
                        await UpdateBPCSucessLog(log.LogID);
                    }
                    else
                    {
                        WriteLog.WriteToFile("PO/CreateBPCItems--" + "Unable to generate Log");
                    }

                }
            }
            catch (SqlException ex) { WriteLog.WriteToFile("PORespository/CreateBPCItems", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("PORespository/CreateBPCItems", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                if (log != null)
                {
                    await UpdateBPCFailureLog(log.LogID, ex.Message);
                }
                else
                {
                    WriteLog.WriteToFile("PO/CreateBPCItems--" + "Unable to generate Log");
                }
                throw ex;
            }
        }

        public async Task CreateOFItemSES(List<BPCOFItemSES> Items)
        {
            var log = new BPCLog();
            try
            {
                log = await CreateBPCLog("CreateOFItemSES", Items.Count);
                foreach (BPCOFItemSES bpc_item in Items)
                {
                    BPCOFItemSES items = _dbContext.BPCOFItemSESes.Where(x => x.Client == bpc_item.Client && x.Company == bpc_item.Company && x.Type == bpc_item.Type && x.PatnerID == bpc_item.PatnerID && x.DocNumber == bpc_item.DocNumber && x.Item == bpc_item.Item && x.ServiceNo == bpc_item.ServiceNo).FirstOrDefault();
                    if (items == null)
                    {
                        bpc_item.IsActive = true;
                        bpc_item.CreatedBy = "SAP API";
                        bpc_item.CreatedOn = DateTime.Now;
                        _dbContext.BPCOFItemSESes.Add(bpc_item);
                        await _dbContext.SaveChangesAsync();
                    }
                    else
                    {
                        //if (!string.IsNullOrEmpty(bpc_item.Flag) && bpc_item.Flag.ToLower() == "x")
                        //{
                        //    _dbContext.BPCOFItems.Remove(items);
                        //    await _dbContext.SaveChangesAsync();
                        //}
                        //else
                        //{
                        items.ServiceItem = bpc_item.ServiceItem;
                        items.OrderedQty = bpc_item.OrderedQty;
                        items.OpenQty = bpc_item.OpenQty;
                        items.ServiceQty = bpc_item.ServiceQty;
                        items.ModifiedBy = "SAP API";
                        items.ModifiedOn = DateTime.Now;
                        await _dbContext.SaveChangesAsync();
                        //}
                    }
                }
                if (log != null)
                {
                    await UpdateBPCSucessLog(log.LogID);
                }
                else
                {
                    WriteLog.WriteToFile("PO/CreateOFItemSES--" + "Unable to generate Log");
                }
                //await UpdateBPCSucessLog(log.LogID);
            }
            catch (SqlException ex) { WriteLog.WriteToFile("PORespository/CreateOFItemSES", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("PORespository/CreateOFItemSES", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                if (log != null)
                {
                    await UpdateBPCFailureLog(log.LogID, ex.Message);
                }
                else
                {
                    WriteLog.WriteToFile("PO/CreateOFItemSES--" + "Unable to generate Log");
                }
                throw ex;
            }
        }

        public async Task CreateBPCSL(List<BPCOFScheduleLine> data)
        {
            var log = new BPCLog();
            try
            {
                log = await CreateBPCLog("CreateBPCSL", data.Count);
                foreach (BPCOFScheduleLine bpc_sl in data)
                {
                    BPCOFScheduleLine items = _dbContext.BPCOFScheduleLines.FirstOrDefault(x => x.Client == bpc_sl.Client && x.Company == bpc_sl.Company && x.Type == bpc_sl.Type && x.PatnerID == bpc_sl.PatnerID && x.DocNumber == bpc_sl.DocNumber && x.Item == bpc_sl.Item && x.Type == bpc_sl.Type && x.SlLine == bpc_sl.SlLine);
                    if (items == null)
                    {
                        bpc_sl.IsActive = true;
                        bpc_sl.CreatedBy = "SAP API";
                        bpc_sl.CreatedOn = DateTime.Now;
                        _dbContext.BPCOFScheduleLines.Add(bpc_sl);
                        await _dbContext.SaveChangesAsync();
                    }
                    else
                    {
                        items.DeliveryDate = bpc_sl.DeliveryDate;
                        items.OrderedQty = bpc_sl.OrderedQty;
                        items.AckStatus = bpc_sl.AckStatus;
                        items.OpenQty = bpc_sl.OpenQty;
                        items.AckDeliveryDate = bpc_sl.AckDeliveryDate;
                        items.ModifiedBy = "SAP API";
                        items.ModifiedOn = DateTime.Now;
                        await _dbContext.SaveChangesAsync();
                    }
                }
                if (log != null)
                {
                    await UpdateBPCSucessLog(log.LogID);
                }
                else
                {
                    WriteLog.WriteToFile("PO/CreateBPCSL--" + "Unable to generate Log");
                }

            }
            catch (SqlException ex) { WriteLog.WriteToFile("PORespository/CreateBPCSL", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("PORespository/CreateBPCSL", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                if (log != null)
                {
                    await UpdateBPCFailureLog(log.LogID, ex.Message);
                }
                else
                {
                    WriteLog.WriteToFile("PO/CreateBPCSL--" + "Unable to generate Log");
                }
                throw ex;
            }
        }

        public async Task UpdateBPCH(List<BPCOFHeader> data)
        {
            var log = new BPCLog();
            try
            {
                log = await CreateBPCLog("UpdateBPCH", data.Count);
                foreach (BPCOFHeader bpc_sl in data)
                {
                    BPCOFHeader items = _dbContext.BPCOFHeaders.FirstOrDefault(x => x.Client == bpc_sl.Client && x.Company == bpc_sl.Company && x.Type == bpc_sl.Type && x.PatnerID == bpc_sl.PatnerID && x.DocNumber == bpc_sl.DocNumber);
                    if (items == null)
                    {
                        if (log != null)
                        {
                            await UpdateBPCFailureLog(log.LogID, $"No records found for {bpc_sl.DocNumber} and {bpc_sl.PatnerID}");
                        }
                        else
                        {
                            WriteLog.WriteToFile("PO/UpdateBPCH--" + "Unable to generate Log");
                        }
                        return;
                    }
                    items.DocDate = bpc_sl.DocDate;
                    items.DocVersion = bpc_sl.DocVersion;
                    items.Currency = bpc_sl.Currency;
                    items.Status = bpc_sl.Status;
                    items.CrossAmount = bpc_sl.CrossAmount;
                    items.NetAmount = bpc_sl.NetAmount;
                    items.RefDoc = bpc_sl.RefDoc;
                    items.AckStatus = bpc_sl.AckStatus;
                    items.AckRemark = bpc_sl.AckRemark;
                    items.AckDate = bpc_sl.AckDate;
                    items.AckUser = bpc_sl.AckUser;
                    items.PINNumber = bpc_sl.PINNumber;
                    items.DocType = bpc_sl.DocType;
                    items.PlantName = bpc_sl.PlantName;
                    items.ModifiedBy = "SAP API";
                    items.ModifiedOn = DateTime.Now;
                    await _dbContext.SaveChangesAsync();
                    //await UpdateBPCSucessLog(log.LogID);
                }
                if (log != null)
                {
                    await UpdateBPCSucessLog(log.LogID);
                }
                else
                {
                    WriteLog.WriteToFile("PO/UpdateBPCH--" + "Unable to generate Log");
                }
            }
            catch (SqlException ex) { WriteLog.WriteToFile("PORespository/UpdateBPCH", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("PORespository/UpdateBPCH", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                if (log != null)
                {
                    await UpdateBPCFailureLog(log.LogID, ex.Message);
                }
                else
                {
                    WriteLog.WriteToFile("PO/UpdateBPCH--" + "Unable to generate Log");
                }

                throw ex;
            }
        }

        public async Task UpdateBPCHReleaseStatus(List<BPCOFReleaseView> data)
        {
            var log = new BPCLog();
            try
            {
                log = await CreateBPCLog("UpdateBPCHReleaseStatus", data.Count);
                foreach (BPCOFReleaseView head in data)
                {
                    BPCOFHeader items = _dbContext.BPCOFHeaders.FirstOrDefault(x => x.Client == head.Client && x.Company == head.Company && x.Type == head.Type && x.PatnerID == head.PatnerID && x.DocNumber == head.DocNumber);
                    if (items == null)
                    {
                        if (log != null)
                        {
                            await UpdateBPCFailureLog(log.LogID, $"No records found for {head.DocNumber} and {head.PatnerID}");
                        }
                        else
                        {
                            WriteLog.WriteToFile("PO/UpdateBPCH--" + $"Unable to generate Log for {head.DocNumber} and {head.PatnerID}");
                        }
                        return;
                    }
                    else
                    {
                        items.ReleasedStatus = head.ReleasedStatus;
                        items.ModifiedBy = "SAP API";
                        items.ModifiedOn = DateTime.Now;
                        await _dbContext.SaveChangesAsync();
                    }

                    //await UpdateBPCSucessLog(log.LogID);
                }
                if (log != null)
                {
                    await UpdateBPCSucessLog(log.LogID);
                }
                else
                {
                    WriteLog.WriteToFile("PO/UpdateBPCHReleaseStatus--" + "Unable to generate Log");
                }
            }
            catch (SqlException ex) { WriteLog.WriteToFile("PORespository/UpdateBPCHReleaseStatus", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("PORespository/UpdateBPCHReleaseStatus", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                if (log != null)
                {
                    await UpdateBPCFailureLog(log.LogID, ex.Message);
                }
                else
                {
                    WriteLog.WriteToFile("PO/UpdateBPCHReleaseStatus--" + "Unable to generate Log");
                }

                throw ex;
            }
        }


        public async Task UpdateBPCItems(List<BPCOFItem> data)
        {
            var log = new BPCLog();
            try
            {
                log = await CreateBPCLog("UpdateBPCItems", data.Count);
                foreach (BPCOFItem bpc_item in data)
                {
                    BPCOFItem items = _dbContext.BPCOFItems.Where(x => x.Client == bpc_item.Client && x.Company == bpc_item.Company && x.Type == bpc_item.Type && x.PatnerID == bpc_item.PatnerID && x.DocNumber == bpc_item.DocNumber && x.Item == bpc_item.Item && x.Type == bpc_item.Type).FirstOrDefault();
                    if (items == null)
                    {
                        if (log != null)
                        {
                            await UpdateBPCFailureLog(log.LogID, $"No records Found for {bpc_item.PatnerID} and {bpc_item.Item}");
                        }
                        else
                        {
                            WriteLog.WriteToFile("PORepository/UpdateBPCItems--" + "Unable to generate Log");
                        }

                        return;
                    }
                    if (!string.IsNullOrEmpty(bpc_item.Flag) && bpc_item.Flag.ToLower() == "x")
                    {
                        _dbContext.BPCOFItems.Remove(items);
                        await _dbContext.SaveChangesAsync();
                    }
                    else
                    {
                        items.Material = bpc_item.Material;
                        items.MaterialText = bpc_item.MaterialText;
                        items.DeliveryDate = bpc_item.DeliveryDate;
                        items.OrderedQty = bpc_item.OrderedQty;
                        items.CompletedQty = bpc_item.CompletedQty;
                        items.TransitQty = bpc_item.TransitQty;
                        items.OpenQty = bpc_item.OpenQty;
                        items.UOM = bpc_item.UOM;
                        items.HSN = bpc_item.HSN;
                        items.IsClosed = bpc_item.IsClosed;
                        items.AckStatus = bpc_item.AckStatus;
                        items.AckDeliveryDate = bpc_item.AckDeliveryDate;
                        items.PlantCode = bpc_item.PlantCode;
                        items.UnitPrice = bpc_item.UnitPrice;
                        items.Value = bpc_item.Value;
                        items.TaxAmount = bpc_item.TaxAmount;
                        items.TaxCode = bpc_item.TaxCode;
                        items.IsFreightAvailable = bpc_item.IsFreightAvailable;
                        items.FreightAmount = bpc_item.FreightAmount;
                        items.ModifiedOn = DateTime.Now;
                        items.ModifiedBy = "SAP API";
                        await _dbContext.SaveChangesAsync();
                        //await UpdateBPCSucessLog(log.LogID);
                    }

                }
                if (log != null)
                {
                    await UpdateBPCSucessLog(log.LogID);
                }
                else
                {
                    WriteLog.WriteToFile("PORepository/UpdateBPCItems--" + "Unable to generate Log");
                }
            }
            catch (SqlException ex) { WriteLog.WriteToFile("PORespository/UpdateBPCItems", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("PORespository/UpdateBPCItems", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                if (log != null)
                {
                    await UpdateBPCFailureLog(log.LogID, ex.Message);
                }
                else
                {
                    WriteLog.WriteToFile("PORepository/UpdateBPCItems--" + "Unable to generate Log");
                }

                throw ex;
            }
        }

        public async Task UpdateBPCSL(List<BPCOFScheduleLine> data)
        {
            var log = new BPCLog();
            try
            {
                log = await CreateBPCLog("UpdateBPCSL", data.Count);
                foreach (BPCOFScheduleLine bpc_sl in data)
                {
                    BPCOFScheduleLine items = _dbContext.BPCOFScheduleLines.FirstOrDefault(x => x.Client == bpc_sl.Client && x.Company == bpc_sl.Company && x.Type == bpc_sl.Type && x.PatnerID == bpc_sl.PatnerID && x.DocNumber == bpc_sl.DocNumber && x.Item == bpc_sl.Item && x.Type == bpc_sl.Type && x.SlLine == bpc_sl.SlLine);
                    if (items == null)
                    {
                        return;
                    }
                    items.DeliveryDate = bpc_sl.DeliveryDate;
                    items.OpenQty = bpc_sl.OpenQty;
                    items.AckStatus = bpc_sl.AckStatus;
                    items.OrderedQty = bpc_sl.OrderedQty;
                    items.AckDeliveryDate = bpc_sl.AckDeliveryDate;
                    items.ModifiedBy = "SAP API";
                    items.ModifiedOn = DateTime.Now;
                    await _dbContext.SaveChangesAsync();
                    //await UpdateBPCSucessLog(log.LogID);
                }
                if (log != null)
                {
                    await UpdateBPCSucessLog(log.LogID);
                }
                else
                {
                    WriteLog.WriteToFile("Fact/UpdateBPCSL--" + "Unable to generate Log");
                }
            }
            catch (SqlException ex) { WriteLog.WriteToFile("PORespository/UpdateBPCItems", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("PORespository/UpdateBPCItems", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                if (log != null)
                {
                    await UpdateBPCFailureLog(log.LogID, ex.Message);
                }
                else
                {
                    WriteLog.WriteToFile("Fact/UpdateBPCSL--" + "Unable to generate Log");
                }
                WriteLog.WriteToFile("Fact/UpdateBPCSL--" + ex.Message);

                throw ex;
            }
        }


        public async Task CreateBPCQM(List<BPCOFQM> data)
        {
            var log = new BPCLog();
            try
            {
                log = await CreateBPCLog("CreateBPCQM", data.Count);
                foreach (BPCOFQM QM in data)
                {
                    BPCOFQM QM1 = _dbContext.BPCOFQMs.FirstOrDefault(x => x.Client == QM.Client && x.Company == QM.Company && x.Type == QM.Type && x.PatnerID == QM.PatnerID && x.DocNumber == QM.DocNumber && x.Item == QM.Item && x.SerialNumber == QM.SerialNumber);
                    if (QM1 == null)
                    {
                        BPCOFQM bpc_qm = new BPCOFQM();
                        bpc_qm.Client = QM.Client;
                        bpc_qm.Company = QM.Company;
                        bpc_qm.Type = QM.Type;
                        bpc_qm.PatnerID = QM.PatnerID;
                        bpc_qm.DocNumber = QM.DocNumber;
                        bpc_qm.Item = QM.Item;
                        bpc_qm.Material = QM.Material;
                        bpc_qm.MaterialText = QM.MaterialText;
                        bpc_qm.GRGIQty = QM.GRGIQty;
                        bpc_qm.LotQty = QM.LotQty;
                        bpc_qm.RejQty = QM.RejQty;
                        bpc_qm.RejReason = QM.RejReason;
                        bpc_qm.CreatedOn = DateTime.Now;
                        bpc_qm.CreatedBy = "SAP API";
                        bpc_qm.ModifiedBy = QM.ModifiedBy;
                        bpc_qm.ModifiedOn = DateTime.Now;
                        bpc_qm.IsActive = QM.IsActive;
                        _dbContext.BPCOFQMs.Add(bpc_qm);
                        await _dbContext.SaveChangesAsync();
                    }
                    else
                    {
                        await UpdateBPCQM(QM);
                    }

                }
                if (log != null)
                {
                    await UpdateBPCSucessLog(log.LogID);
                }
                else
                {
                    WriteLog.WriteToFile("PO/CreateBPCQM--" + "Unable to generate Log");
                }
                await UpdateBPCSucessLog(log.LogID);
            }
            catch (SqlException ex) { WriteLog.WriteToFile("PORespository/CreateBPCQM", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("PORespository/CreateBPCQM", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                if (log != null)
                {
                    await UpdateBPCFailureLog(log.LogID, ex.Message);
                }
                else
                {
                    WriteLog.WriteToFile("PO/CreateBPCQM--" + "Unable to generate Log");
                }

                throw ex;
            }
        }

        public async Task UpdateBPCQM(BPCOFQM bpc_qm)
        {
            var log = new BPCLog();
            try
            {
                log = await CreateBPCLog("UpdateBPCQM", 1);
                BPCOFQM QM = _dbContext.BPCOFQMs.FirstOrDefault(x => x.Client == bpc_qm.Client && x.Company == bpc_qm.Company && x.Type == bpc_qm.Type && x.PatnerID == bpc_qm.PatnerID && x.DocNumber == bpc_qm.DocNumber && x.Item == bpc_qm.Item && x.SerialNumber == bpc_qm.SerialNumber);
                if (QM == null)
                {
                    return;
                }
                QM.Material = bpc_qm.Material;
                QM.MaterialText = bpc_qm.MaterialText;
                QM.GRGIQty = bpc_qm.GRGIQty;
                QM.LotQty = bpc_qm.LotQty;
                QM.RejQty = bpc_qm.RejQty;
                QM.RejReason = bpc_qm.RejReason;
                QM.ModifiedBy = "SAP API";
                QM.ModifiedOn = DateTime.Now;
                await _dbContext.SaveChangesAsync();
                if (log != null)
                {
                    await UpdateBPCSucessLog(log.LogID);
                }
                else
                {
                    WriteLog.WriteToFile("PO/UpdateBPCQM--" + "Unable to generate Log");
                }
                //await UpdateBPCSucessLog(log.LogID);
            }
            catch (SqlException ex) { WriteLog.WriteToFile("PORespository/UpdateBPCQM", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("PORespository/UpdateBPCQM", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                if (log != null)
                {
                    await UpdateBPCFailureLog(log.LogID, ex.Message);
                }
                else
                {
                    WriteLog.WriteToFile("PO/UpdateBPCQM--" + "Unable to generate Log");
                }


                throw ex;
            }
        }
        public async Task UpdateBPCQM(List<BPCOFQM> data)
        {
            var log = new BPCLog();
            try
            {
                log = await CreateBPCLog("UpdateBPCQM", data.Count);
                foreach (BPCOFQM bpc_qm in data)
                {
                    BPCOFQM QM = _dbContext.BPCOFQMs.FirstOrDefault(x => x.Client == bpc_qm.Client && x.Company == bpc_qm.Company && x.Type == bpc_qm.Type && x.PatnerID == bpc_qm.PatnerID && x.DocNumber == bpc_qm.DocNumber && x.Item == bpc_qm.Item);
                    if (QM == null)
                    {
                        await UpdateBPCFailureLog(log.LogID, $"No Records found for PatnerID-{bpc_qm.PatnerID} , DocNumber-{bpc_qm.DocNumber} and Item-{bpc_qm.Item}");
                        return;
                    }
                    QM.Material = bpc_qm.Material;
                    QM.MaterialText = bpc_qm.MaterialText;
                    QM.GRGIQty = bpc_qm.GRGIQty;
                    QM.LotQty = bpc_qm.LotQty;
                    QM.RejQty = bpc_qm.RejQty;
                    QM.RejReason = bpc_qm.RejReason;
                    QM.ModifiedBy = "SAP API";
                    QM.ModifiedOn = DateTime.Now;
                    await _dbContext.SaveChangesAsync();
                    await UpdateBPCSucessLog(log.LogID);
                }
            }
            catch (SqlException ex) { WriteLog.WriteToFile("PORespository/UpdateBPCQM", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("PORespository/UpdateBPCQM", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                await UpdateBPCFailureLog(log.LogID, ex.Message);
                throw ex;
            }
        }

        public List<BPCOFQM> GetBPCQMByPartnerID(string PartnerID)
        {
            try
            {
                var result = _dbContext.BPCOFQMs.Where(x => x.PatnerID == PartnerID).ToList();
                return result;
            }
            catch (SqlException ex) { WriteLog.WriteToFile("PORespository/UpdateBPCQM", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("PORespository/UpdateBPCQM", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<BPCOFQM> GetBPCQMByPartnerIDs(string PartnerID)
        {
            try
            {
                string[] PartnerIDs = PartnerID.Split(',');
                for (int i = 0; i < PartnerIDs.Length; i++)
                {
                    PartnerIDs[i] = PartnerIDs[i].Trim();
                }
                List< BPCOFQM> bPCOFQMs = new List< BPCOFQM >();
                List<BPCOFQM> bPCOFQM = new List<BPCOFQM>();
                for (int i = 0; i < PartnerIDs.Length; i++)
                {
                    
                    bPCOFQM = _dbContext.BPCOFQMs.Where(x => x.PatnerID == PartnerIDs[i]).ToList();
                    bPCOFQMs.AddRange(bPCOFQM);
                }
                    return bPCOFQMs;
            }
            catch (SqlException ex) { WriteLog.WriteToFile("PORespository/UpdateBPCQM", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("PORespository/UpdateBPCQM", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<BPCOFQM> GetBPCQMByPartnerIDFilter(string PartnerID)
        {
            try
            {
                var result = _dbContext.BPCOFQMs.Where(x => x.PatnerID == PartnerID).OrderByDescending(x => x.RejQty).ToList();
                return result;
            }
            catch (SqlException ex) { WriteLog.WriteToFile("PORespository/UpdateBPCQM", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("PORespository/UpdateBPCQM", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<BPCOFQM> GetBPCQMByPartnerIDsFilter(string PartnerID)
        {
            try
            {
                string[] PartnerIDs = PartnerID.Split(',');
                for (int i = 0; i < PartnerIDs.Length; i++)
                {
                    PartnerIDs[i] = PartnerIDs[i].Trim();
                }
                string[] Partnersuni = PartnerIDs.Distinct().ToArray();
                List<BPCOFQM> bPCOFQMs = new List<BPCOFQM>();
                List<BPCOFQM> bPCOFQM = new List<BPCOFQM>();
                for (int i = 0; i < Partnersuni.Length; i++)
                {

                    bPCOFQM = _dbContext.BPCOFQMs.Where(x => x.PatnerID == Partnersuni[i]).OrderByDescending(x => x.RejQty).ToList();
                    bPCOFQMs.AddRange(bPCOFQM);
                }
                return bPCOFQMs;
                
            }
            catch (SqlException ex) { WriteLog.WriteToFile("PORespository/UpdateBPCQM", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("PORespository/UpdateBPCQM", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        
        public List<BPCOFQM> GetQMReportByDate(QMReportOption QMOption)
        {
            try
            {
                List<BPCOFQM> QMReports = new List<BPCOFQM>();
                if (QMOption.FromDate.HasValue && QMOption.FromDate != null && QMOption.ToDate.HasValue && QMOption.ToDate != null)
                {
                    QMReports = (from tb in _dbContext.BPCOFQMs
                                 where tb.IsActive && tb.PatnerID.ToLower() == QMOption.PartnerID.ToLower()
                                 && (tb.CreatedOn.Date >= QMOption.FromDate.Value.Date)
                                 && (tb.CreatedOn.Date <= QMOption.ToDate.Value.Date)
                                 select tb).ToList();
                }
                else if (QMOption.FromDate.HasValue && QMOption.FromDate != null && !QMOption.ToDate.HasValue && QMOption.ToDate == null)
                {
                    QMReports = (from tb in _dbContext.BPCOFQMs
                                 where tb.IsActive && tb.PatnerID.ToLower() == QMOption.PartnerID.ToLower()
                                 && (tb.CreatedOn.Date >= QMOption.FromDate.Value.Date)
                                 //&& (tb.CreatedOn.Date <= QMOption.ToDate.Value.Date)
                                 select tb).ToList();
                }
                else if (!QMOption.FromDate.HasValue && QMOption.FromDate == null && QMOption.ToDate.HasValue && QMOption.ToDate != null)
                {
                    QMReports = (from tb in _dbContext.BPCOFQMs
                                 where tb.IsActive && tb.PatnerID.ToLower() == QMOption.PartnerID.ToLower()
                                 //&& (tb.CreatedOn.Date >= QMOption.FromDate.Value.Date)
                                 && (tb.CreatedOn.Date <= QMOption.ToDate.Value.Date)
                                 select tb).ToList();
                }
                return QMReports;
            }
            catch (SqlException ex) { WriteLog.WriteToFile("PORespository/GetQMReportByDate", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("PORespository/GetQMReportByDate", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("PORepository/GetQMReportByDate : - ", ex);
                throw ex;
            }
        }
        public List<BPCOFQM> GetQMReportByOption(QMReportOption QMOption)
        {
            try
            {
                List<BPCOFQM> QMReports = new List<BPCOFQM>();
                bool IsPO = !string.IsNullOrEmpty(QMOption.PO);
                bool IsMaterial = !string.IsNullOrEmpty(QMOption.Material);
                QMReports = (from tb in _dbContext.BPCOFQMs
                             where tb.IsActive && tb.PatnerID.ToLower() == QMOption.PartnerID.ToLower()
                             && (!IsPO || tb.DocNumber.ToLower() == QMOption.PO.ToLower())
                             && (!IsMaterial || tb.Material.ToLower() == QMOption.Material.ToLower())
                             select tb).ToList();
                return QMReports;
            }
            catch (SqlException ex) { WriteLog.WriteToFile("PORespository/GetQMReportByOption", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("PORespository/GetQMReportByOption", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("PORepository/GetQMReportByOption : - ", ex);
                throw ex;
            }
        }
        public List<BPCOFQM> GetQMReportByPatnerID(QMReportOption QMOption)
        {
            try
            {
                string[] PartnerID = QMOption.PartnerID.Split(',');
                for (int i = 0; i < PartnerID.Length; i++)
                {
                    PartnerID[i] = PartnerID[i].Trim();
                }
                List<BPCOFQM> QMReport = new List<BPCOFQM>();
                List<BPCOFQM> QMReports = new List<BPCOFQM>();
                bool IsPO = !string.IsNullOrEmpty(QMOption.PO);
                bool IsMaterial = !string.IsNullOrEmpty(QMOption.Material);
                for (int i = 0; i < PartnerID.Length; i++)
                {
                    QMReport = (from tb in _dbContext.BPCOFQMs
                                where tb.IsActive && tb.PatnerID.ToLower() == PartnerID[i].ToLower()
                                && (!IsPO || tb.DocNumber.ToLower() == QMOption.PO.ToLower())
                                && (!IsMaterial || tb.Material.ToLower() == QMOption.Material.ToLower())
                                select tb).ToList();
                    QMReports.AddRange(QMReport);
                }
                return QMReports;
            }
            catch (SqlException ex) { WriteLog.WriteToFile("PORespository/GetQMReportByOption", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("PORespository/GetQMReportByOption", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("PORepository/GetQMReportByOption : - ", ex);
                throw ex;
            }
        }
        public List<BPCOFQM> GetQMReportByStatus(QMReportOption QMOption)
        {
            try
            {
                List<BPCOFQM> QMReports = new List<BPCOFQM>();
                if (!string.IsNullOrEmpty(QMOption.Status))
                {
                    QMReports = (from tb in _dbContext.BPCOFQMs
                                 where tb.IsActive && tb.PatnerID.ToLower() == QMOption.PartnerID.ToLower()

                                 select tb).ToList();
                }
                return QMReports;
            }
            catch (SqlException ex) { WriteLog.WriteToFile("PORespository/GetQMReportByStatus", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("PORespository/GetQMReportByStatus", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("PORepository/GetQMReportByStatus : - ", ex);
                throw ex;
            }
        }



        public List<BPCOFQM> GetBPCQMByDocNumber(string DocNumber)
        {
            try
            {
                var result = _dbContext.BPCOFQMs.Where(x => x.DocNumber == DocNumber).ToList();
                return result;
            }
            catch (SqlException ex) { WriteLog.WriteToFile("PORespository/GetBPCQMByDocNumber", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("PORespository/GetBPCQMByDocNumber", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region AIACT
        public async Task<bool> CreateAIACTs(List<BPCAIACT> BPCAIACTs)
        {
            var log = new BPCLog();
            try
            {
                log = await CreateBPCLog("CreateAIACTs", BPCAIACTs.Count);
                string BaseAddress = _configuration.GetValue<string>("APIBaseAddress");
                string HostURI = BaseAddress + "/factapi/Fact/CreateAIACTDetails";
                var uri = new Uri(HostURI);
                var request = (HttpWebRequest)WebRequest.Create(uri);
                request.Method = "POST";
                request.ContentType = "application/json";
                var SerializedObject = JsonConvert.SerializeObject(BPCAIACTs);
                byte[] requestBody = Encoding.UTF8.GetBytes(SerializedObject);

                using (var postStream = await request.GetRequestStreamAsync())
                {
                    await postStream.WriteAsync(requestBody, 0, requestBody.Length);
                }

                try
                {
                    using (var response = (HttpWebResponse)await request.GetResponseAsync())
                    {
                        if (response != null && response.StatusCode == HttpStatusCode.OK)
                        {
                            var reader = new StreamReader(response.GetResponseStream());
                            string responseString = await reader.ReadToEndAsync();
                            reader.Close();
                            await UpdateBPCSucessLog(log.LogID);
                            return true;
                        }
                        else
                        {
                            var reader = new StreamReader(response.GetResponseStream());
                            string responseString = await reader.ReadToEndAsync();
                            reader.Close();
                            await UpdateBPCFailureLog(log.LogID, "False");
                            return false;
                        }
                    }
                }
                catch (WebException ex)
                {
                    using (var stream = ex.Response.GetResponseStream())
                    using (var reader = new StreamReader(stream))
                    {
                        var errorMessage = reader.ReadToEnd();
                        await UpdateBPCFailureLog(log.LogID, ex.Message);
                        throw ex;
                    }
                }
                catch (SqlException ex) { WriteLog.WriteToFile("PORespository/CreateAIACTs", ex); throw new Exception("Something went wrong"); }
                catch (InvalidOperationException ex) { WriteLog.WriteToFile("PORespository/CreateAIACTs", ex); throw new Exception("Something went wrong"); }
                catch (Exception ex)
                {
                    await UpdateBPCFailureLog(log.LogID, ex.Message);
                    throw ex;
                }

            }
            catch (SqlException ex) { WriteLog.WriteToFile("PORespository/CreateAIACTs", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("PORespository/CreateAIACTs", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                await UpdateBPCFailureLog(log.LogID, ex.Message);
                throw ex;
            }
        }

        public BPCOFHeader GetPOPartnerID(string PartnerID)
        {
            try
            {
                return _dbContext.BPCOFHeaders.FirstOrDefault(x => x.PatnerID == PartnerID);
            }
            catch (SqlException ex) { WriteLog.WriteToFile("PORespository/GetPOPartnerID", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("PORespository/GetPOPartnerID", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string GetPlantByDocNmber(string DocNumber, string PartnerID)
        {
            try
            {
                return _dbContext.BPCOFHeaders.Where(x => x.PatnerID == PartnerID && x.DocNumber == DocNumber).Select(y => y.Plant).FirstOrDefault();
            }
            catch (SqlException ex) { WriteLog.WriteToFile("PORespository/GetPlantByDocNmber", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("PORespository/GetPlantByDocNmber", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public string GetPlantByASNNmber(string DocNumber, string PartnerID)
        {
            try
            {
                var result = (from tb1 in _dbContext.BPCOFHeaders
                              join tb2 in _dbContext.BPCASNHeaders on tb1.DocNumber equals tb2.DocNumber
                              where tb2.PatnerID == PartnerID && tb1.DocNumber == DocNumber
                              select tb1.Plant).FirstOrDefault();
                return result;
            }
            catch (SqlException ex) { WriteLog.WriteToFile("PORespository/GetPlantByASNNmber", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("PORespository/GetPlantByASNNmber", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static string GetFileExtension(string base64String)
        {
            var data = base64String.Substring(0, 5);

            switch (data.ToUpper())
            {
                case "IVBOR":
                    return "png";
                case "/9J/4":
                    return "jpg";
                case "AAAAF":
                    return "mp4";
                case "JVBER":
                    return "pdf";
                case "AAABA":
                    return "ico";
                case "UMFYI":
                    return "rar";
                case "E1XYD":
                    return "rtf";
                case "U1PKC":
                    return "txt";
                case "MQOWM":
                case "77U/M":
                    return "srt";
                default:
                    return string.Empty;
            }
        }
        public static byte[] DecodeUrlBase64(string s)
        {
            s = s.Replace('-', '+').Replace('_', '/').PadRight(4 * ((s.Length + 3) / 4), '=');
            return Convert.FromBase64String(s);
        }

        public BPCAttachment GetAttachment(string attachmentName)
        {
            return _dbContext.BPCAttachments.FirstOrDefault(t => t.AttachmentName == attachmentName);
        }

        public async Task<BPCLog> CreateBPCLog(string APIMethod, int NoOfRecords)
        {
            try
            {
                BPCLog log = new BPCLog();
                log.LogDate = DateTime.Now;
                log.APIMethod = APIMethod;
                log.NoOfRecords = NoOfRecords;
                log.Status = "Initiated";
                log.CreatedOn = DateTime.Now;
                var result = _dbContext.BPCLogs.Add(log);
                await _dbContext.SaveChangesAsync();
                return result.Entity;
            }
            catch (SqlException ex) { WriteLog.WriteToFile("PORespository/CreateBPCLog", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("PORespository/CreateBPCLog", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<BPCLog> UpdateBPCSucessLog(int LogID)
        {
            try
            {
                var result = _dbContext.BPCLogs.FirstOrDefault(x => x.LogID == LogID);
                if (result != null)
                {
                    result.Status = "Success";
                    result.Response = "Data are inserted successfully";
                    result.ModifiedOn = DateTime.Now;
                    await _dbContext.SaveChangesAsync();
                }
                return result;
            }
            catch (SqlException ex) { WriteLog.WriteToFile("PORespository/UpdateBPCSucessLog", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("PORespository/UpdateBPCSucessLog", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<BPCLog> UpdateBPCFailureLog(int LogID, string ErrorMessage)
        {
            try
            {
                var result = _dbContext.BPCLogs.FirstOrDefault(x => x.LogID == LogID);
                if (result != null)
                {
                    result.Status = "Failed";
                    result.Response = ErrorMessage;
                    result.ModifiedOn = DateTime.Now;
                    await _dbContext.SaveChangesAsync();
                }
                return result;
            }
            catch (SqlException ex) { WriteLog.WriteToFile("PORespository/UpdateBPCFailureLog", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("PORespository/UpdateBPCFailureLog", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task SendDeliveryNotification()
        {
            try
            {
                var today = DateTime.Now;
                var result = (from tb in _dbContext.BPCOFItems
                              where tb.Type.ToLower() == "v" && tb.DeliveryDate.HasValue && tb.DeliveryDate.Value.Date < today.Date
                              select tb).ToList();
                var PatnerIDs = result.Select(x => x.PatnerID).Distinct().ToList();
                var Items = result.GroupBy(x => x.PatnerID).ToList();
                var UserViews = await GetUserViewsByPatnerIDs(PatnerIDs);
                foreach (var item in Items)
                {
                    var email = UserViews.Where(x => x.UserName == item.Key).Select(x => x.Email).FirstOrDefault();
                    if (!string.IsNullOrEmpty(email))
                    {
                        await SendMail(email, item.Select(x => x).ToList());
                    }
                }
            }
            catch (SqlException ex) { WriteLog.WriteToFile("PORespository/SendDeliveryNotification", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("PORespository/SendDeliveryNotification", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task SendPOItemChangeNotification(BPCOFItem item, bool IsDelivery = false, bool IsRate = false, bool IsQty = false)
        {
            try
            {
                var today = DateTime.Now;
                //var result = (from tb in _dbContext.BPCOFItems
                //              where tb.Type.ToLower() == "v" && tb.DeliveryDate.HasValue && tb.DeliveryDate.Value.Date < today.Date
                //              select tb).ToList();
                //var PatnerIDs = result.Select(x => x.PatnerID).Distinct().ToList();
                //var Items = result.GroupBy(x => x.PatnerID).ToList();
                //var UserViews = await GetUserViewsByPatnerIDs(PatnerIDs);

                var UserView = await GetUserViewByPatnerID(item.PatnerID);
                if (UserView!=null && !string.IsNullOrEmpty(UserView.Email))
                {
                    await SendMail1(UserView.Email, item, IsDelivery, IsRate, IsQty);
                }

            }
            catch (SqlException ex) { WriteLog.WriteToFile("PORespository/SendPOItemChangeNotification", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("PORespository/SendPOItemChangeNotification", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public async Task<List<UserView>> GetUserViewsByPatnerIDs(List<string> PatnerIDs)
        {
            try
            {
                string BaseAddress = _configuration.GetValue<string>("APIBaseAddress");
                string HostURI = BaseAddress + "/authenticationapi/Master/GetUserViewsByPatnerIDs";
                var uri = new Uri(HostURI);
                var request = (HttpWebRequest)WebRequest.Create(uri);
                request.Method = "POST";
                request.ContentType = "application/json";
                var SerializedObject = JsonConvert.SerializeObject(PatnerIDs);
                byte[] requestBody = Encoding.UTF8.GetBytes(SerializedObject);

                using (var postStream = await request.GetRequestStreamAsync())
                {
                    await postStream.WriteAsync(requestBody, 0, requestBody.Length);
                }

                try
                {
                    using (var response = (HttpWebResponse)await request.GetResponseAsync())
                    {
                        if (response != null && response.StatusCode == HttpStatusCode.OK)
                        {
                            var reader = new StreamReader(response.GetResponseStream());
                            string responseString = await reader.ReadToEndAsync();
                            reader.Close();
                            var userViews = JsonConvert.DeserializeObject<List<UserView>>(responseString);
                            return userViews;
                        }
                        else
                        {
                            var reader = new StreamReader(response.GetResponseStream());
                            string responseString = await reader.ReadToEndAsync();
                            reader.Close();
                            WriteLog.WriteToFile($"PORespository/GetUserViewsByPatnerIDs : - {responseString}");
                            return null;
                        }
                    }
                }
                catch (WebException ex)
                {
                    using (var stream = ex.Response.GetResponseStream())
                    using (var reader = new StreamReader(stream))
                    {
                        var errorMessage = reader.ReadToEnd();

                        throw ex;
                    }
                }
                catch (SqlException ex) { WriteLog.WriteToFile("PORespository/GetUserViewsByPatnerIDs", ex); throw new Exception("Something went wrong"); }
                catch (InvalidOperationException ex) { WriteLog.WriteToFile("PORespository/GetUserViewsByPatnerIDs", ex); throw new Exception("Something went wrong"); }
                catch (Exception ex)
                {
                    throw ex;
                }

            }
            catch (SqlException ex) { WriteLog.WriteToFile("PORespository/GetUserViewsByPatnerIDs", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("PORespository/GetUserViewsByPatnerIDs", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<UserView> GetUserViewByPatnerID(string PatnerID)
        {
            try
            {
                string BaseAddress = _configuration.GetValue<string>("APIBaseAddress");
                string HostURI = BaseAddress + $"/authenticationapi/Master/GetUserViewByPatnerID?PatnerID={PatnerID}";
                var uri = new Uri(HostURI);
                var request = (HttpWebRequest)WebRequest.Create(uri);
                request.Method = "GET";
                request.ContentType = "application/json";

                try
                {
                    //using (var response = (HttpWebResponse)await request.GetResponseAsync())
                    //{
                    //    if (response != null && response.StatusCode == HttpStatusCode.OK)
                    //    {
                    //        var reader = new StreamReader(response.GetResponseStream());
                    //        string responseString = await reader.ReadToEndAsync();
                    //        reader.Close();
                    //        var userViews = JsonConvert.DeserializeObject<UserView>(responseString);
                    //        return userViews;
                    //    }
                    //    else
                    //    {
                    //        var reader = new StreamReader(response.GetResponseStream());
                    //        string responseString = await reader.ReadToEndAsync();
                    //        reader.Close();
                    //        WriteLog.WriteToFile($"PORespository/GetUserViewByPatnerID : - {responseString}");
                    //        return null;
                    //    }
                    //}
                    using (var response = (HttpWebResponse)await request.GetResponseAsync())
                    {
                        if (response != null && response.StatusCode == HttpStatusCode.OK)
                        {
                            var reader = new StreamReader(response.GetResponseStream());
                            string responseString = await reader.ReadToEndAsync();
                            reader.Close();
                            var userViews = JsonConvert.DeserializeObject<UserView>(responseString);
                            return userViews;
                        }
                        else
                        {
                            var reader = new StreamReader(response.GetResponseStream());
                            string responseString = await reader.ReadToEndAsync();
                            reader.Close();
                            WriteLog.WriteToFile($"PORespository/GetUserViewByPatnerID : - {responseString}");
                            return null;
                        }
                    }
                }
                catch (WebException ex)
                {
                    using (var stream = ex.Response.GetResponseStream())
                    using (var reader = new StreamReader(stream))
                    {
                        var errorMessage = reader.ReadToEnd();

                        throw ex;
                    }
                }
                catch (SqlException ex) { WriteLog.WriteToFile("PORespository/GetUserViewByPatnerID", ex); throw new Exception("Something went wrong"); }
                catch (InvalidOperationException ex) { WriteLog.WriteToFile("PORespository/GetUserViewByPatnerID", ex); throw new Exception("Something went wrong"); }
                catch (Exception ex)
                {
                    throw ex;
                }

            }
            catch (SqlException ex) { WriteLog.WriteToFile("PORespository/GetUserViewByPatnerID", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("PORespository/GetUserViewsByPatnerIDs", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public async Task<bool> SendMail(string toEmail, List<BPCOFItem> items)
        {

            try
            {
                var STMPDetailsConfig = _configuration.GetSection("STMPDetails");
                string hostName = STMPDetailsConfig["Host"];
                string SMTPEmail = STMPDetailsConfig["Email"];
                //string SMTPEmailPassword = STMPDetailsConfig["Password"];
                string SMTPPort = STMPDetailsConfig["Port"];
                string siteURL = _configuration.GetValue<string>("SiteURL");

                var message = new MailMessage();
                string subject = "";
                StringBuilder sb = new StringBuilder();
                //UserName = string.IsNullOrEmpty(UserName) ? toEmail.Split('@')[0] : UserName;
                var body = $@"<html><head></head><body> <div style='border:1px solid #dbdbdb;'> <div style='padding: 20px 20px; background-color: #fff06769;text-align: center;font-family: Segoe UI;'> <p> 
                              <h2>Emami Vendor Portal</h2> </p> </div> <div style='background-color: #f8f7f7;padding: 20px 20px;font-family: Segoe UI'> 
                              <div> 
                              <p> Dear Sir/Madam ,</p>
                              ";
                sb.Append(@body);
                //string styles = @"<style>
                //                        table {
                //                          /*font-family: arial, sans-serif;*/
                //                          border-collapse: collapse;
                //                          width: 100%;
                //                          margin:20px 0px;
                //                        }

                //                        td, th {
                //                          border: 1px solid #dddddd;
                //                          text-align: left;
                //                          padding: 8px;
                //                        }

                //                        tr:nth-child(even) {
                //                          /*background-color: #dddddd;*/
                //                        }
                //                        .lar{
                //                            width: 25%;
                //                         }
                //                        .lar1{
                //                            width: 20%;
                //                         }

                //                      </style>";
                //sb.Append(styles);
                //string table = @"<table>
                //                              <tr>
                //                                <th>Doc Number</th>
                //                                <th>Item</th>
                //                                <th>Material</th>
                //                                <th>Material Text</th>
                //                                <th>Delivery date</th>
                //                                <th>Order Qty</th>
                //                                <th>Completed Qty</th>
                //                              </tr>";
                //foreach (var item in items)
                //{
                //    //var lic = (from tb in _ctx.LicenseMasters where tb.LicenseID == item.LicenseID select tb).FirstOrDefault();
                //    //var des = lic != null ? lic.Description : item.LicenseID.ToString();
                //    table += $@"<tr>
                //                         <td>{item.DocNumber}</td>
                //                         <td>{item.Item}</td>
                //                         <td>{item.Material}</td>
                //                         <td class='lar'>{item.MaterialText}</td>
                //                         <td>{(item.DeliveryDate.HasValue ? item.DeliveryDate.Value.ToString("dd/MM/yyyy") : "")}</td>
                //                         <td>{item.OrderedQty}</td>
                //                         <td>{item.CompletedQty}</td>
                //                        </tr>";
                //};
                //table += @"</table>";
                //sb.Append(table);
                var today = DateTime.Now;
                foreach (var item in items)
                {
                    if (item.DeliveryDate.HasValue)
                    {
                        var dateDifference = (today - item.DeliveryDate.Value).Days;
                        string Plant = item.PlantCode;
                        var plantMaster = _dbContext.BPCPlantMasters.FirstOrDefault(x => x.PlantCode == item.PlantCode);
                        if (plantMaster != null)
                        {
                            Plant = plantMaster.PlantText ?? plantMaster.PlantCode;
                        }
                        var content = $@"<p>PO Number {item.DocNumber}/{item.Item} raised at EMAMI LTD, {Plant}
                                         <p>Delivery date - {item.DeliveryDate.Value.ToString("dd/MM/yyyy")}
                                         for Material {item.Material} - {item.MaterialText} – HEAVY is due by {dateDifference} day/s</p>";
                        sb.Append(content);
                    }

                };

                var content1 = "<br><p><p>Kindly process PO ASAP by sharing dispatch details.</p></p><br>";
                sb.Append(content1);

                var regards = @"<p>Thanks and Regards,</p> <p>Emami Ltd</p> 
                                <p>This mail is generated automatically. Please do not reply</p>
                                </div> </div> </div></body></html>";
                sb.Append(regards);
                subject = "Delivery Due - Alert";
                SmtpClient client = new SmtpClient();
                client.Port = Convert.ToInt32(SMTPPort);
                client.Host = hostName;
                client.EnableSsl = false;
                client.Timeout = 60000;
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                //client.UseDefaultCredentials = false;
                //client.Credentials = new System.Net.NetworkCredential(SMTPEmail, SMTPEmailPassword);
                MailMessage reportEmail = new MailMessage(SMTPEmail, toEmail, subject, sb.ToString());
                reportEmail.BodyEncoding = UTF8Encoding.UTF8;
                reportEmail.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;
                reportEmail.IsBodyHtml = true;
                ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, sslPolicyErrors) => true;
                await client.SendMailAsync(reportEmail);
                WriteLog.WriteToFile($"PORepository Delivery Due Notification Details sent successfully to {toEmail}");
                return true;
            }
            catch (SmtpFailedRecipientsException ex)
            {
                for (int i = 0; i < ex.InnerExceptions.Length; i++)
                {
                    SmtpStatusCode status = ex.InnerExceptions[i].StatusCode;
                    if (status == SmtpStatusCode.MailboxBusy ||
                        status == SmtpStatusCode.MailboxUnavailable)
                    {
                        WriteLog.WriteToFile("PORepository/SendMail/MailboxBusy/MailboxUnavailable/SmtpFailedRecipientsException:Inner- " + ex.InnerExceptions[i].Message);
                    }
                    else
                    {
                        WriteLog.WriteToFile("PORepository/SendMail/SmtpFailedRecipientsException:Inner- " + ex.InnerExceptions[i].Message);
                    }
                }
                WriteLog.WriteToFile("PORepository/SendMail/SmtpFailedRecipientsException:- " + ex.Message, ex);
                return false;
            }
            catch (SmtpException ex)
            {
                WriteLog.WriteToFile("PORepository/SendMail/SmtpException:- " + ex.Message, ex);
                return false;
            }
            catch (SqlException ex) { WriteLog.WriteToFile("FactRepository/SendMail", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("FactRepository/SendMail", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("PORepository/SendMail/Exception:- " + ex.Message, ex);
                return false;
            }





            //string subject = "";
            //string ApplicationUrl = ConfigurationManager.AppSettings["ApplicationUrl"].ToString();
            //StringBuilder sb = new StringBuilder();
            //sb.Append("Dear Sir/Madam,<br/>");
            //string content = "";
            //if (mailModel.CNType == CNType.Compliance)
            //{
            //    content = mailModel.ActivityType == ActivityType.Create ? "New Compliance has been released with the following details:" :
            //              mailModel.ActivityType == ActivityType.Update ? "Compliance has been updated with the following details:" :
            //              "Compliance has been closed. Please find the following details";
            //}
            //else if (mailModel.CNType == CNType.License)
            //{
            //    content = mailModel.ActivityType == ActivityType.Create ? "New License has been released with the following details:" :
            //              mailModel.ActivityType == ActivityType.Update ? "License has been updated with the following details:" :
            //              "License has been closed. Please find the following details";

            //}
            //sb.Append(String.Format("<p>{0}</p>", content));
            //sb.Append(String.Format("<p><b>Unit</b> : {0}</p>", mailModel.Unit));
            //if (mailModel.ActivityType == ActivityType.Create)
            //{
            //    sb.Append(String.Format("<p><b>Created by</b> : {0}</p>", mailModel.PerformedBy));
            //}
            //else if (mailModel.ActivityType == ActivityType.Update)
            //{
            //    sb.Append(String.Format("<p><b>Updated by</b> : {0}</p>", mailModel.PerformedBy));
            //}
            //else if (mailModel.ActivityType == ActivityType.Close)
            //{
            //    sb.Append(String.Format("<p><b>Closed by</b> : {0}</p>", mailModel.PerformedBy));
            //}
            //if (mailModel.CNType == CNType.Compliance)
            //{
            //    sb.Append(String.Format("<p><b>Compliance Request Number</b> : {0}</p>", mailModel.ReferenceNumber));
            //    sb.Append(String.Format("<p><b>Open date</b> : {0}</p>", mailModel.OpenDate.ToString("dd/MM/yyyy")));
            //    sb.Append(String.Format("<p><b>End date</b> : {0}</p>", mailModel.EndDate.ToString("dd/MM/yyyy")));
            //}
            //if (mailModel.CNType == CNType.License)
            //{
            //    sb.Append(String.Format("<p><b>License Request Number</b> : {0}</p>", mailModel.ReferenceNumber));
            //    //sb.Append(String.Format("<p><b>Open date</b> : {0}</p>", mailModel.OpenDate.ToString("dd/MM/yyyy")));
            //    //sb.Append(String.Format("<p><b>End date</b> : {0}</p>", mailModel.EndDate.ToString("dd/MM/yyyy")));
            //}
            //if (mailModel.ActivityType == ActivityType.Update && updatedLicenseItems != null && updatedLicenseItems.Count > 0)
            //{
            //    string styles = @"<style>
            //                        table {
            //                          /*font-family: arial, sans-serif;*/
            //                          border-collapse: collapse;
            //                          width: 100%;
            //                        }

            //                        td, th {
            //                          border: 1px solid #dddddd;
            //                          text-align: left;
            //                          padding: 8px;
            //                        }

            //                        tr:nth-child(even) {
            //                          /*background-color: #dddddd;*/
            //                        }
            //                        .lar{
            //                            width: 25%;
            //                         }
            //                        .lar1{
            //                            width: 20%;
            //                         }

            //                      </style>";
            //    sb.Append(styles);
            //    string table = @"<table>
            //                          <tr>
            //                            <th>License/NOC</th>
            //                            <th>Yes/No</th>
            //                            <th>One Time</th>
            //                            <th>Validity</th>
            //                            <th>Renewal Due Date</th>
            //                            <th>Remarks</th>
            //                          </tr>";
            //    foreach (var item in updatedLicenseItems)
            //    {
            //        //var lic = (from tb in _ctx.LicenseMasters where tb.LicenseID == item.LicenseID select tb).FirstOrDefault();
            //        //var des = lic != null ? lic.Description : item.LicenseID.ToString();
            //        table += $@"<tr>
            //                     <td class='lar'>{item.Description}</td>
            //                     <td>{item.Status}</td>
            //                     <td>{item.OneTime}</td>
            //                     <td>{(item.Validity.HasValue ? item.Validity.Value.ToString("dd/MM/yyyy") : "")}</td>
            //                     <td>{(item.RenewalDueDate.HasValue ? item.RenewalDueDate.Value.ToString("dd/MM/yyyy") : "")}</td>
            //                     <td class='lar1'>{item.Remarks}</td>
            //                    </tr>";
            //    };
            //    table += @"</table>";
            //    sb.Append(table);
            //}

            //sb.Append(String.Format("<p>Click <a href='{0}' target='_blank'>here</a> to login to the application</p>", ApplicationUrl));
            //sb.Append(String.Format("<p>This is a system generated notification, please do not reply.</p>"));
            //sb.Append(String.Format("<p>Regards,</p>"));
            //sb.Append(String.Format("<p>System Administrator</p>"));




            ////sb.Append("</p></div><p>UBL Admin,</p><p style='font-size:14px;color:#ffffff;background-color:#00008B;height:20px;padding:4px;'>© United Breweries Limited</p>");
            //subject = mailModel.Subject;

            //string hostName = ConfigurationManager.AppSettings["HostName"];
            //string SMTPEmail = ConfigurationManager.AppSettings["SMTPEmail"];
            ////string SMTPEmailPassword = ConfigurationManager.AppSettings["SMTPEmailPassword"]; // Comment this line for UBL deployement
            //var message = new MailMessage();
            //SmtpClient client = new SmtpClient();
            //client.Port = Convert.ToInt16(ConfigurationManager.AppSettings["SMTPPort"]);
            //client.Host = ConfigurationManager.AppSettings["HostName"];
            //client.EnableSsl = false;// change this to false for UBL deployement
            //client.Timeout = 60000;
            //client.DeliveryMethod = SmtpDeliveryMethod.Network;
            ////client.UseDefaultCredentials = false; // Comment this line for UBL deployement
            ////client.Credentials = new System.Net.NetworkCredential(SMTPEmail, SMTPEmailPassword);// Comment this line for UBL deployement
            //MailMessage reportEmail = new MailMessage(SMTPEmail, toEmail, subject, sb.ToString());
            //reportEmail.BodyEncoding = UTF8Encoding.UTF8;
            //reportEmail.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;
            //reportEmail.IsBodyHtml = true;
            //await client.SendMailAsync(reportEmail);
            //WriteLog.WriteProcessLog($"License/SendMail : Mail has been sent successfully {toEmail}");
            //string hostName = ConfigurationManager.AppSettings["HostName"];
            //string SMTPEmail = ConfigurationManager.AppSettings["SMTPEmail"];
            //var message = new MailMessage();
            //SmtpClient client = new SmtpClient();
            //client.Port = Convert.ToInt16(ConfigurationManager.AppSettings["SMTPPort"]);
            //client.Host = ConfigurationManager.AppSettings["HostName"];
            //client.EnableSsl = false;
            //client.Timeout = 60000;
            //client.DeliveryMethod = SmtpDeliveryMethod.Network;
            //string SMTPEmailPassword = ConfigurationManager.AppSettings["SMTPEmailPassword"];
            //client.UseDefaultCredentials = false;
            //client.Credentials = new System.Net.NetworkCredential(SMTPEmail, SMTPEmailPassword);
            //MailMessage reportEmail = new MailMessage(SMTPEmail, toEmail, subject, sb.ToString());
            //reportEmail.BodyEncoding = UTF8Encoding.UTF8;
            //reportEmail.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;
            //reportEmail.IsBodyHtml = true;
            //await client.SendMailAsync(reportEmail);

        }

        public async Task<bool> SendMail1(string toEmail, BPCOFItem item, bool IsDelivery = false, bool IsRate = false, bool IsQty = false)
        {

            try
            {
                var STMPDetailsConfig = _configuration.GetSection("STMPDetails");
                string hostName = STMPDetailsConfig["Host"];
                string SMTPEmail = STMPDetailsConfig["Email"];
                //string SMTPEmailPassword = STMPDetailsConfig["Password"];
                string SMTPPort = STMPDetailsConfig["Port"];
                string siteURL = _configuration.GetValue<string>("SiteURL");

                var message = new MailMessage();
                string subject = "";
                StringBuilder sb = new StringBuilder();
                //UserName = string.IsNullOrEmpty(UserName) ? toEmail.Split('@')[0] : UserName;
                var body = $@"<html><head></head><body> <div style='border:1px solid #dbdbdb;'> <div style='padding: 20px 20px; background-color: #fff06769;text-align: center;font-family: Segoe UI;'> <p> 
                              <h2>Emami Vendor Portal</h2> </p> </div> <div style='background-color: #f8f7f7;padding: 20px 20px;font-family: Segoe UI'> 
                              <div> 
                              <p> Dear Sir/Madam ,</p>
                              ";
                sb.Append(@body);
                //string styles = @"<style>
                //                        table {
                //                          /*font-family: arial, sans-serif;*/
                //                          border-collapse: collapse;
                //                          width: 100%;
                //                          margin:20px 0px;
                //                        }

                //                        td, th {
                //                          border: 1px solid #dddddd;
                //                          text-align: left;
                //                          padding: 8px;
                //                        }

                //                        tr:nth-child(even) {
                //                          /*background-color: #dddddd;*/
                //                        }
                //                        .lar{
                //                            width: 25%;
                //                         }
                //                        .lar1{
                //                            width: 20%;
                //                         }

                //                      </style>";
                //sb.Append(styles);
                //string table = @"<table>
                //                              <tr>
                //                                <th>Doc Number</th>
                //                                <th>Item</th>
                //                                <th>Material</th>
                //                                <th>Material Text</th>
                //                                <th>Delivery date</th>
                //                                <th>Order Qty</th>
                //                                <th>Completed Qty</th>
                //                              </tr>";
                //foreach (var item in items)
                //{
                //    //var lic = (from tb in _ctx.LicenseMasters where tb.LicenseID == item.LicenseID select tb).FirstOrDefault();
                //    //var des = lic != null ? lic.Description : item.LicenseID.ToString();
                //    table += $@"<tr>
                //                         <td>{item.DocNumber}</td>
                //                         <td>{item.Item}</td>
                //                         <td>{item.Material}</td>
                //                         <td class='lar'>{item.MaterialText}</td>
                //                         <td>{(item.DeliveryDate.HasValue ? item.DeliveryDate.Value.ToString("dd/MM/yyyy") : "")}</td>
                //                         <td>{item.OrderedQty}</td>
                //                         <td>{item.CompletedQty}</td>
                //                        </tr>";
                //};
                //table += @"</table>";
                //sb.Append(table);
                var today = DateTime.Now;
                string Plant = item.PlantCode;
                var plantMaster = _dbContext.BPCPlantMasters.FirstOrDefault(x => x.PlantCode == item.PlantCode);
                if (plantMaster != null)
                {
                    Plant = plantMaster.PlantText ?? plantMaster.PlantCode;
                }
                var content = $@"<p>Please find the attached PO Number {item.DocNumber}/{item.Item} raised at EMAMI LTD, {Plant}</p>";
                sb.Append(content);

                var Message = "";
                if (IsDelivery)
                {
                    Message += $@"<p>Delivery date hase been revised to {item.DeliveryDate.Value.ToString("dd/MM/yyyy")} for Material {item.Material} - {item.MaterialText}.</p>";
                }
                if (IsRate)
                {
                    Message += $@"<p>Basic rate hase been revised to INR{item.UnitPrice} for Material {item.Material} - {item.MaterialText}.</p>";
                }
                if (IsQty)
                {
                    Message += $@"<p>PO Qty hase been revised to {item.OrderedQty} for Material {item.Material} - {item.MaterialText}.</p>";
                }

                sb.Append(Message);
                var content1 = "<p><p>Kindly acknowledge the PO within 24 hours by return mail and process it ASAP by sharing dispatch details.</p></p><br>";
                sb.Append(content1);

                var regards = @"<p>Thanks and Regards,</p> <p>Emami Ltd</p> 
                                <p>This mail is generated automatically. Please do not reply</p>
                                </div> </div> </div></body></html>";
                sb.Append(regards);
                subject = "PO Update - Alert";
                SmtpClient client = new SmtpClient();
                client.Port = Convert.ToInt32(SMTPPort);
                client.Host = hostName;
                client.EnableSsl = false;
                client.Timeout = 60000;
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                //client.UseDefaultCredentials = false;
                //client.Credentials = new System.Net.NetworkCredential(SMTPEmail, SMTPEmailPassword);
                MailMessage reportEmail = new MailMessage(SMTPEmail, toEmail, subject, sb.ToString());
                reportEmail.BodyEncoding = UTF8Encoding.UTF8;
                reportEmail.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;
                reportEmail.IsBodyHtml = true;
                ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, sslPolicyErrors) => true;
                await client.SendMailAsync(reportEmail);
                WriteLog.WriteToFile($"PO Update Notification details sent successfully to {toEmail}");
                return true;
            }
            catch (SmtpFailedRecipientsException ex)
            {
                for (int i = 0; i < ex.InnerExceptions.Length; i++)
                {
                    SmtpStatusCode status = ex.InnerExceptions[i].StatusCode;
                    if (status == SmtpStatusCode.MailboxBusy ||
                        status == SmtpStatusCode.MailboxUnavailable)
                    {
                        WriteLog.WriteToFile("PORepository/SendMail1/MailboxBusy/MailboxUnavailable/SmtpFailedRecipientsException:Inner- " + ex.InnerExceptions[i].Message);
                    }
                    else
                    {
                        WriteLog.WriteToFile("PORepository/SendMail1/SmtpFailedRecipientsException:Inner- " + ex.InnerExceptions[i].Message);
                    }
                }
                WriteLog.WriteToFile("PORepository/SendMail1/SmtpFailedRecipientsException:- " + ex.Message, ex);
                return false;
            }
            catch (SmtpException ex)
            {
                WriteLog.WriteToFile("PORepository/SendMail1/SmtpException:- " + ex.Message, ex);
                return false;
            }
            catch (SqlException ex) { WriteLog.WriteToFile("FactRepository/SendMail1", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("FactRepository/SendMail1", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("PORepository/SendMail1/Exception:- " + ex.Message, ex);
                return false;
            }





            //string subject = "";
            //string ApplicationUrl = ConfigurationManager.AppSettings["ApplicationUrl"].ToString();
            //StringBuilder sb = new StringBuilder();
            //sb.Append("Dear Sir/Madam,<br/>");
            //string content = "";
            //if (mailModel.CNType == CNType.Compliance)
            //{
            //    content = mailModel.ActivityType == ActivityType.Create ? "New Compliance has been released with the following details:" :
            //              mailModel.ActivityType == ActivityType.Update ? "Compliance has been updated with the following details:" :
            //              "Compliance has been closed. Please find the following details";
            //}
            //else if (mailModel.CNType == CNType.License)
            //{
            //    content = mailModel.ActivityType == ActivityType.Create ? "New License has been released with the following details:" :
            //              mailModel.ActivityType == ActivityType.Update ? "License has been updated with the following details:" :
            //              "License has been closed. Please find the following details";

            //}
            //sb.Append(String.Format("<p>{0}</p>", content));
            //sb.Append(String.Format("<p><b>Unit</b> : {0}</p>", mailModel.Unit));
            //if (mailModel.ActivityType == ActivityType.Create)
            //{
            //    sb.Append(String.Format("<p><b>Created by</b> : {0}</p>", mailModel.PerformedBy));
            //}
            //else if (mailModel.ActivityType == ActivityType.Update)
            //{
            //    sb.Append(String.Format("<p><b>Updated by</b> : {0}</p>", mailModel.PerformedBy));
            //}
            //else if (mailModel.ActivityType == ActivityType.Close)
            //{
            //    sb.Append(String.Format("<p><b>Closed by</b> : {0}</p>", mailModel.PerformedBy));
            //}
            //if (mailModel.CNType == CNType.Compliance)
            //{
            //    sb.Append(String.Format("<p><b>Compliance Request Number</b> : {0}</p>", mailModel.ReferenceNumber));
            //    sb.Append(String.Format("<p><b>Open date</b> : {0}</p>", mailModel.OpenDate.ToString("dd/MM/yyyy")));
            //    sb.Append(String.Format("<p><b>End date</b> : {0}</p>", mailModel.EndDate.ToString("dd/MM/yyyy")));
            //}
            //if (mailModel.CNType == CNType.License)
            //{
            //    sb.Append(String.Format("<p><b>License Request Number</b> : {0}</p>", mailModel.ReferenceNumber));
            //    //sb.Append(String.Format("<p><b>Open date</b> : {0}</p>", mailModel.OpenDate.ToString("dd/MM/yyyy")));
            //    //sb.Append(String.Format("<p><b>End date</b> : {0}</p>", mailModel.EndDate.ToString("dd/MM/yyyy")));
            //}
            //if (mailModel.ActivityType == ActivityType.Update && updatedLicenseItems != null && updatedLicenseItems.Count > 0)
            //{
            //    string styles = @"<style>
            //                        table {
            //                          /*font-family: arial, sans-serif;*/
            //                          border-collapse: collapse;
            //                          width: 100%;
            //                        }

            //                        td, th {
            //                          border: 1px solid #dddddd;
            //                          text-align: left;
            //                          padding: 8px;
            //                        }

            //                        tr:nth-child(even) {
            //                          /*background-color: #dddddd;*/
            //                        }
            //                        .lar{
            //                            width: 25%;
            //                         }
            //                        .lar1{
            //                            width: 20%;
            //                         }

            //                      </style>";
            //    sb.Append(styles);
            //    string table = @"<table>
            //                          <tr>
            //                            <th>License/NOC</th>
            //                            <th>Yes/No</th>
            //                            <th>One Time</th>
            //                            <th>Validity</th>
            //                            <th>Renewal Due Date</th>
            //                            <th>Remarks</th>
            //                          </tr>";
            //    foreach (var item in updatedLicenseItems)
            //    {
            //        //var lic = (from tb in _ctx.LicenseMasters where tb.LicenseID == item.LicenseID select tb).FirstOrDefault();
            //        //var des = lic != null ? lic.Description : item.LicenseID.ToString();
            //        table += $@"<tr>
            //                     <td class='lar'>{item.Description}</td>
            //                     <td>{item.Status}</td>
            //                     <td>{item.OneTime}</td>
            //                     <td>{(item.Validity.HasValue ? item.Validity.Value.ToString("dd/MM/yyyy") : "")}</td>
            //                     <td>{(item.RenewalDueDate.HasValue ? item.RenewalDueDate.Value.ToString("dd/MM/yyyy") : "")}</td>
            //                     <td class='lar1'>{item.Remarks}</td>
            //                    </tr>";
            //    };
            //    table += @"</table>";
            //    sb.Append(table);
            //}

            //sb.Append(String.Format("<p>Click <a href='{0}' target='_blank'>here</a> to login to the application</p>", ApplicationUrl));
            //sb.Append(String.Format("<p>This is a system generated notification, please do not reply.</p>"));
            //sb.Append(String.Format("<p>Regards,</p>"));
            //sb.Append(String.Format("<p>System Administrator</p>"));




            ////sb.Append("</p></div><p>UBL Admin,</p><p style='font-size:14px;color:#ffffff;background-color:#00008B;height:20px;padding:4px;'>© United Breweries Limited</p>");
            //subject = mailModel.Subject;

            //string hostName = ConfigurationManager.AppSettings["HostName"];
            //string SMTPEmail = ConfigurationManager.AppSettings["SMTPEmail"];
            ////string SMTPEmailPassword = ConfigurationManager.AppSettings["SMTPEmailPassword"]; // Comment this line for UBL deployement
            //var message = new MailMessage();
            //SmtpClient client = new SmtpClient();
            //client.Port = Convert.ToInt16(ConfigurationManager.AppSettings["SMTPPort"]);
            //client.Host = ConfigurationManager.AppSettings["HostName"];
            //client.EnableSsl = false;// change this to false for UBL deployement
            //client.Timeout = 60000;
            //client.DeliveryMethod = SmtpDeliveryMethod.Network;
            ////client.UseDefaultCredentials = false; // Comment this line for UBL deployement
            ////client.Credentials = new System.Net.NetworkCredential(SMTPEmail, SMTPEmailPassword);// Comment this line for UBL deployement
            //MailMessage reportEmail = new MailMessage(SMTPEmail, toEmail, subject, sb.ToString());
            //reportEmail.BodyEncoding = UTF8Encoding.UTF8;
            //reportEmail.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;
            //reportEmail.IsBodyHtml = true;
            //await client.SendMailAsync(reportEmail);
            //WriteLog.WriteProcessLog($"License/SendMail : Mail has been sent successfully {toEmail}");
            //string hostName = ConfigurationManager.AppSettings["HostName"];
            //string SMTPEmail = ConfigurationManager.AppSettings["SMTPEmail"];
            //var message = new MailMessage();
            //SmtpClient client = new SmtpClient();
            //client.Port = Convert.ToInt16(ConfigurationManager.AppSettings["SMTPPort"]);
            //client.Host = ConfigurationManager.AppSettings["HostName"];
            //client.EnableSsl = false;
            //client.Timeout = 60000;
            //client.DeliveryMethod = SmtpDeliveryMethod.Network;
            //string SMTPEmailPassword = ConfigurationManager.AppSettings["SMTPEmailPassword"];
            //client.UseDefaultCredentials = false;
            //client.Credentials = new System.Net.NetworkCredential(SMTPEmail, SMTPEmailPassword);
            //MailMessage reportEmail = new MailMessage(SMTPEmail, toEmail, subject, sb.ToString());
            //reportEmail.BodyEncoding = UTF8Encoding.UTF8;
            //reportEmail.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;
            //reportEmail.IsBodyHtml = true;
            //await client.SendMailAsync(reportEmail);

        }


        public class UserView
        {
            public Guid UserID { get; set; }
            public string UserName { get; set; }
            public string Email { get; set; }
        }

        #endregion
    }
}
