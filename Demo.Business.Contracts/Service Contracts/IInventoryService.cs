using System;
using System.ServiceModel;
using Core.Common.Exceptions;
using Demo.Business.Entities;

namespace Demo.Business.Contracts
{
    [ServiceContract]
    public interface IInventoryService
    {
        [OperationContract]
        Product[] GetProducts();

        [OperationContract]
        [FaultContract(typeof(ArgumentException))]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        void DeleteProduct(int productId);

        [OperationContract]
        [FaultContract(typeof(NotSupportedException))]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        void ActivateProduct(int productId);
    }
}
