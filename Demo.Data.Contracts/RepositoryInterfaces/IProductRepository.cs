using Core.Common.Contracts;
using Demo.Business.Entities;

namespace Demo.Data.Contracts
{
    public interface IProductRepository : IDataRepository
    {
        Product[] GetProducts();
        void ActivateProduct(int productId);
        void DeactivateProduct(int productId);
    }
}