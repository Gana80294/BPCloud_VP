using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using BPCloud_VP_POService.Models;
using BPCloud_VP_POService.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BPCloud_VP_POService.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductRepository _ProductRepository;
        public ProductController(IProductRepository ProductRepository)
        {
            _ProductRepository = ProductRepository;
        }
        [HttpGet]
        public List<BPCProd> GetAllProducts()
        {
            try
            {
                var Products = _ProductRepository.GetAllProducts();
                return Products;
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("Product/GetAllProducts", ex);
                return null;
            }
        }
        [HttpPost]
        public async Task<IActionResult> CreateProduct(BPCProd prod)
        {
            try
            {

                var result = await _ProductRepository.CreateProduct(prod);
                return new OkResult();
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("Product/CreateProduct", ex);
                return BadRequest(ex.Message);
            }
        }
        [HttpPost]
        public async Task<IActionResult> CreateProductDetails([FromBody] List<BPCProd> prods)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                await _ProductRepository.CreateProductDetails(prods);
                return Ok("Data are inserted successfully");
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("Product/CreateProductDetails", ex);
                return BadRequest(ex.Message);
            }
        }
        [HttpPost]
        public async Task<IActionResult> UpdateProduct(BPCProd prod)
        {
            try
            {

                var result = await _ProductRepository.UpdateProduct(prod);
                return new OkResult();
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("Product/UpdateProducts", ex);
                return BadRequest(ex.Message);
            }
        }
        [HttpPost]
        public async Task<IActionResult> DeleteProduct(BPCProd prod)
        {
            try
            {
                //if (!ModelState.IsValid)
                //{
                //    return BadRequest(ModelState);
                //}
                var result = await _ProductRepository.DeleteProduct(prod);
                return new OkResult();
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("Product/DeleteProduct", ex);
                return BadRequest(ex.Message);
            }
        }

        #region product Fav
        [HttpGet]

        public List<BPCProdFav> GetAllProductFav()
        {
            try
            {
                var Products = _ProductRepository.GetAllProductFav();
                return Products;
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("Product/GetAllProductFav", ex);
                return null;
            }
        }
        [HttpPost]
        public async Task<IActionResult> CreateProductFav(BPCProdFav prodFav)
        {
            try
            {

                var result = await _ProductRepository.CreateProductFav(prodFav);
                return new OkResult();
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("Product/CreateProductFav", ex);
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> UpdateProductFav(BPCProdFav prodFav)
        {
            try
            {

                var result = await _ProductRepository.UpdateProductFav(prodFav);
                return new OkResult();
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("Product/UpdateProductFav", ex);
                return BadRequest(ex.Message);
            }
        }
        [HttpPost]
        public async Task<IActionResult> DeleteProductFav(BPCProdFav prodFav)
        {
            try
            {
                //if (!ModelState.IsValid)
                //{
                //    return BadRequest(ModelState);
                //}
                var result = await _ProductRepository.DeleteProductFav(prodFav);
                return new OkResult();
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("Product/DeleteProductFav", ex);
                return BadRequest(ex.Message);
            }
        }
        #endregion
    }

}