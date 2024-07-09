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
    public class ProductRepository : IProductRepository
    {
        private readonly POContext _dbContext;
        private readonly IPORepository _poRepository;
        IConfiguration _configuration;
        public ProductRepository(POContext dbContext, IConfiguration configuration, IPORepository poRepository)
        {
            _dbContext = dbContext;
            _configuration = configuration;
            _poRepository = poRepository;
        }

        public List<BPCProd> GetAllProducts()
        {
            try
            {
                return _dbContext.BPCProds.ToList();
            }
            catch (SqlException ex) { WriteLog.WriteToFile("ProductRepository/GetAllProducts", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("ProductRepository/GetAllProducts", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<BPCProd> CreateProduct(BPCProd prod)
        {
            BPCProd prodResult = new BPCProd();
            try
            {
                BPCProd Product = (from tb in _dbContext.BPCProds
                                   where tb.IsActive && tb.ProductID == prod.ProductID
                                   select tb).FirstOrDefault();
                if (Product == null)
                {
                    prod.CreatedOn = DateTime.Now;
                    prod.IsActive = true;
                    var result = _dbContext.BPCProds.Add(prod);
                    await _dbContext.SaveChangesAsync();
                    prodResult.AttID = prod.AttID;
                    prodResult.Client = prod.Client;
                    prodResult.Company = prod.Company;
                    prodResult.ProductID = prod.ProductID;
                    return prodResult;


                }
                else
                {
                    return prodResult;
                }


            }
            catch (SqlException ex) { WriteLog.WriteToFile("ProductRepository/CreateProduct", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("ProductRepository/CreateProduct", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task CreateProductDetails(List<BPCProd> prods)
        {
            var log = new BPCLog();
            try
            {
                log = await _poRepository.CreateBPCLog("CreateProductDetails", prods.Count);
                foreach (var prod in prods)
                {
                    BPCProd Product = (from tb in _dbContext.BPCProds
                                       where tb.IsActive && tb.ProductID == prod.ProductID
                                       select tb).FirstOrDefault();
                    if (Product == null)
                    {
                        prod.CreatedOn = DateTime.Now;
                        prod.IsActive = true;
                        var result = _dbContext.BPCProds.Add(prod);
                        await _dbContext.SaveChangesAsync();
                    }
                    else
                    {
                        Product.MaterialText = prod.MaterialText;
                        Product.MaterialGroup = prod.MaterialGroup;
                        Product.MaterialType = prod.MaterialType;
                        Product.UOM = prod.UOM;
                        Product.Stock = prod.Stock;
                        Product.BasePrice = prod.BasePrice;
                        Product.StockUpdatedOn = prod.StockUpdatedOn;
                        prod.ModifiedOn = DateTime.Now;
                        await _dbContext.SaveChangesAsync();
                    }
                }
                if (log != null)
                {
                    await _poRepository.UpdateBPCSucessLog(log.LogID);
                }
                else
                {
                    WriteLog.WriteToFile("PO/CreateBPCH--" + "Unable to generate Log");
                }
            }
            catch (SqlException ex) { WriteLog.WriteToFile("ProductRepository/CreateProductDetails", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("ProductRepository/CreateProductDetails", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                if (log != null)
                {
                    await _poRepository.UpdateBPCFailureLog(log.LogID, ex.Message);
                }
                else
                {
                    WriteLog.WriteToFile("PO/CreateBPCH--" + "Unable to generate Log");
                }
                throw ex;
            }
        }
        public async Task<BPCProd> UpdateProduct(BPCProd prod)
        {
            BPCProd prodResult = new BPCProd();
            try
            {
                BPCProd Product = (from tb in _dbContext.BPCProds
                                   where tb.IsActive && tb.ProductID == prod.ProductID
                                   select tb).FirstOrDefault();
                if (Product != null)
                {
                    Product.MaterialType = prod.MaterialType;
                    Product.MaterialGroup = prod.MaterialGroup;
                    Product.MaterialText = prod.MaterialText;
                    Product.BasePrice = prod.BasePrice;
                    Product.Stock = prod.Stock;
                    Product.ModifiedOn = DateTime.Now;
                    Product.ModifiedBy = prod.ModifiedBy;
                    await _dbContext.SaveChangesAsync();
                    prodResult.AttID = prod.AttID;
                    prodResult.Client = prod.Client;
                    prodResult.Company = prod.Company;
                    prodResult.ProductID = prod.ProductID;
                    return prodResult;


                }
                else
                {
                    return prodResult;
                }


            }
            catch (SqlException ex) { WriteLog.WriteToFile("ProductRepository/UpdateProduct", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("ProductRepository/UpdateProduct", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public async Task<BPCProd> DeleteProduct(BPCProd prod)
        {
            BPCProd prodResult = new BPCProd();
            try
            {
                BPCProd Product = (from tb in _dbContext.BPCProds
                                   where tb.IsActive && tb.ProductID == prod.ProductID
                                   select tb).FirstOrDefault();
                if (Product != null)
                {

                    var result = _dbContext.BPCProds.Remove(Product);
                    await _dbContext.SaveChangesAsync();
                    prodResult.AttID = prod.AttID;
                    prodResult.Client = prod.Client;
                    prodResult.Company = prod.Company;
                    prodResult.ProductID = prod.ProductID;
                    return prodResult;


                }
                else
                {
                    return prodResult;
                }


            }
            catch (SqlException ex) { WriteLog.WriteToFile("ProductRepository/DeleteProduct", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("ProductRepository/DeleteProduct", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        #region Product Fav
        public List<BPCProdFav> GetAllProductFav()
        {
            try
            {
                return _dbContext.BPCProdFavs.ToList();
            }
            catch (SqlException ex) { WriteLog.WriteToFile("ProductRepository/GetAllProductFav", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("ProductRepository/GetAllProductFav", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<BPCProdFav> CreateProductFav(BPCProdFav prodFav)
        {
            BPCProdFav prodResult = new BPCProdFav();
            try
            {
                BPCProdFav Product = (from tb in _dbContext.BPCProdFavs
                                      where tb.IsActive && tb.ProductID == prodFav.ProductID && tb.PatnerID == prodFav.PatnerID
                                      select tb).FirstOrDefault();
                if (Product == null)
                {
                    prodFav.CreatedOn = DateTime.Now;
                    prodFav.IsActive = true;
                    var result = _dbContext.BPCProdFavs.Add(prodFav);
                    await _dbContext.SaveChangesAsync();
                    prodResult.Client = prodFav.Client;
                    prodResult.Company = prodFav.Company;
                    prodResult.ProductID = prodFav.ProductID;
                    return prodResult;


                }
                else
                {
                    return prodResult;
                }


            }
            catch (SqlException ex) { WriteLog.WriteToFile("ProductRepository/CreateProductFav", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("ProductRepository/CreateProductFav", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public async Task<BPCProdFav> UpdateProductFav(BPCProdFav prodFav)
        {
            BPCProdFav prodResult = new BPCProdFav();
            try
            {
                BPCProdFav Product = (from tb in _dbContext.BPCProdFavs
                                      where tb.IsActive && tb.ProductID == prodFav.ProductID && tb.PatnerID == prodFav.PatnerID
                                      select tb).FirstOrDefault();
                if (Product != null)
                {
                    Product.Rating = prodFav.Rating;
                    Product.ModifiedBy = prodFav.ModifiedBy;
                    Product.ModifiedOn = prodFav.ModifiedOn;
                    await _dbContext.SaveChangesAsync();
                    prodResult.Client = prodFav.Client;
                    prodResult.Company = prodFav.Company;
                    prodResult.ProductID = prodFav.ProductID;
                    return prodResult;


                }
                else
                {
                    return prodResult;
                }


            }
            catch (SqlException ex) { WriteLog.WriteToFile("ProductRepository/UpdateProductFav", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("ProductRepository/UpdateProductFav", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public async Task<BPCProdFav> DeleteProductFav(BPCProdFav prodFav)
        {
            BPCProdFav prodResult = new BPCProdFav();
            try
            {
                BPCProdFav Product = (from tb in _dbContext.BPCProdFavs
                                      where tb.IsActive && tb.ProductID == prodFav.ProductID && tb.PatnerID == prodFav.PatnerID
                                      select tb).FirstOrDefault();
                if (Product != null)
                {

                    var result = _dbContext.BPCProdFavs.Remove(Product);
                    await _dbContext.SaveChangesAsync();
                    prodResult.Client = prodFav.Client;
                    prodResult.Company = prodFav.Company;
                    prodResult.ProductID = prodFav.ProductID;
                    return prodResult;
                }
                else
                {
                    return prodResult;
                }


            }
            catch (SqlException ex) { WriteLog.WriteToFile("ProductRepository/DeleteProductFav", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("ProductRepository/DeleteProductFav", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                throw ex;
            }
        }



        #endregion
    }
}
