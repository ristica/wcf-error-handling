using System.ComponentModel.Composition;
using System.Linq;
using System.ServiceModel;
using Core.Common.Contracts;
using Core.Common.Exceptions;
using Demo.Business.Common;
using Demo.Business.Contracts;
using Demo.Business.Entities;
using Demo.Data.Contracts;
using System.ServiceModel.Dispatcher;
using System;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.Collections.ObjectModel;

namespace Demo.Business.Managers
{
    /// <summary>
    /// do set initialization per call and not per session (default)
    /// because it is not scalable
    /// set concurency mode to multiple (default = single) because we have per call situation
    /// set ReleaseServiceInstanceOnTransactionComplete to true if there will be at least 
    /// one operation with attribute TransactionScopeRequired = true
    /// </summary>
    [ServiceBehavior(
        InstanceContextMode = InstanceContextMode.PerCall, 
        ConcurrencyMode = ConcurrencyMode.Multiple, 
        ReleaseServiceInstanceOnTransactionComplete = false)]
    public class InventoryManager : ManagerBase, IInventoryService, IServiceBehavior
    {
        #region Fields

        [Import]
        private IDataRepositoryFactory _repositoryFactory;

        #endregion

        #region IInventoryManager implementation

        public Product[] GetProducts()
        {
            return ExecuteFaultHandledOperation(() =>
            {
                var productRepository = this._repositoryFactory.GetDataRepository<IProductRepository>();
                var products = productRepository.GetProducts();                

                return products.ToArray();
            });
        }

        [TransactionFlow(TransactionFlowOption.Allowed)]
        [OperationBehavior(TransactionScopeRequired = true)]
        public void DeleteProduct(int productId)
        {
            ExecuteFaultHandledOperation(() =>
            {
                var ex = new ArgumentException("ArgumentException test...");
                throw new FaultException<ArgumentException>(ex, ex.Message);
            });
        }

        [TransactionFlow(TransactionFlowOption.Allowed)]
        [OperationBehavior(TransactionScopeRequired = true)]
        public void ActivateProduct(int productId)
        {
            ExecuteFaultHandledOperation(() =>
            {
                var ex = new NotSupportedException("NotFoundException test...");
                throw new FaultException<NotSupportedException>(ex, ex.Message);
            });
        }

        #endregion

        #region IServiceBehavior implementation

        /// <summary>
        ///  validate per service if the contract has attribute 
        ///  with faultexception of T
        /// </summary>
        /// <param name="serviceDescription"></param>
        /// <param name="serviceHostBase"></param>
        public void Validate(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase)
        {
            foreach (var enpoint in serviceDescription.Endpoints)
            {
                if (!enpoint.Contract.Name.Equals("IInventoryService")) continue;

                foreach (var operationDescription in enpoint.Contract.Operations)
                {
                    if (operationDescription.Name.Equals("DeleteProduct"))
                    {
                        if (operationDescription.Faults.FirstOrDefault(item => item.DetailType.Equals(typeof(ArgumentException))) == null)
                        {
                            throw new InvalidOperationException("DeleteProduct operation requires a fault contract for ArgumentException.");
                        }
                    }

                    if (operationDescription.Name.Equals("ActivateProduct"))
                    {
                        if (operationDescription.Faults.FirstOrDefault(item => item.DetailType.Equals(typeof(NotSupportedException))) == null)
                        {
                            throw new InvalidOperationException("ActivateProduct operation requires a fault contract for NotFoundException.");
                        }
                    }
                }
            }
        }

        public void AddBindingParameters(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase, Collection<ServiceEndpoint> endpoints, BindingParameterCollection bindingParameters)
        {
        }

        public void ApplyDispatchBehavior(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase)
        {
        }

        #endregion
    }
}
