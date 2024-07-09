using BPCloud_VP_POService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BPCloud_VP_POService.Repositories
{
    public interface IProductRepository
    {
        List<BPCProd> GetAllProducts();
        Task<BPCProd> CreateProduct(BPCProd prod);
        Task CreateProductDetails(List<BPCProd> prods);
        Task<BPCProd> UpdateProduct(BPCProd prod);
        Task<BPCProd> DeleteProduct(BPCProd prod);

        #region Product Fav
        List<BPCProdFav> GetAllProductFav();
        Task<BPCProdFav> CreateProductFav(BPCProdFav prodFav);
        Task<BPCProdFav> UpdateProductFav(BPCProdFav prodFav);
        Task<BPCProdFav> DeleteProductFav(BPCProdFav prodFav);
        #endregion
    }
}
