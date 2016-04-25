using System.ComponentModel.Composition;
using System.Linq;
using Demo.Business.Entities;
using Demo.Data.Contracts;
using System.Collections.Generic;

namespace Demo.Data.DataRepositories
{
    [Export(typeof(IProductRepository))]
    public class ProductRepository : IProductRepository
    {
        #region Db

        public List<Product> Products = new List<Product>
        {
            new Product
            {
                ProductId = 1,
                ArticleNumber = "syrgsr",
                Description = "Test product 1",
                IsActive = true,
                Name = "Product 1",
                Price = 100
            },
            new Product
            {
                ProductId = 2,
                ArticleNumber = "vhjukjuh",
                Description = "Test product 2",
                IsActive = false,
                Name = "Product 2",
                Price = 200
            },
            new Product
            {
                ProductId = 3,
                ArticleNumber = "23545",
                Description = "Test product 3",
                IsActive = true,
                Name = "Product 3",
                Price = 300
            },
            new Product
            {
                ProductId = 4,
                ArticleNumber = "25545",
                Description = "Test product 4",
                IsActive = false,
                Name = "Product 4",
                Price = 400
            }
        };

        #endregion

        #region IProductRepository implementation

        public Product[] GetProducts()
        {
            return Products.ToArray();
        }

        public void ActivateProduct(int productId)
        {
            Products.FirstOrDefault(p => p.IsActive).IsActive = true;
        }

        public void DeactivateProduct(int productId)
        {
            Products.FirstOrDefault(p => p.IsActive).IsActive = false;
        }

        #endregion
    }
}
