﻿using OrderExtension.Web.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using VirtoCommerce.Domain.Cart.Services;
using VirtoCommerce.Domain.Catalog.Model;
using VirtoCommerce.Domain.Catalog.Services;
using VirtoCommerce.Domain.Commerce.Model.Search;
using VirtoCommerce.Domain.Common;
using VirtoCommerce.Domain.Order.Events;
using VirtoCommerce.Domain.Order.Model;
using VirtoCommerce.Domain.Order.Services;
using VirtoCommerce.Domain.Payment.Services;
using VirtoCommerce.Domain.Shipping.Services;
using VirtoCommerce.Domain.Store.Services;
using VirtoCommerce.OrderModule.Data.Model;
using VirtoCommerce.OrderModule.Data.Repositories;
using VirtoCommerce.OrderModule.Data.Services;
using VirtoCommerce.Platform.Core.ChangeLog;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.DynamicProperties;
using VirtoCommerce.Platform.Core.Events;
using VirtoCommerce.Platform.Core.Settings;
using VirtoCommerce.Platform.Data.Infrastructure;

namespace OrderExtension.Web.Services {
    public class CustomerOrderServiceExtImpl : CustomerOrderServiceImpl, IOrderShipmentService {

        protected ICustomerOrderSearchService _orderSearchService;

        private ICustomerOrderService _customerOrderService;
        private IStoreService _storeService;
        private IEventPublisher<OrderChangeEvent> _orderChangingPublisher;
        private Func<IOrderExtensionRepository> _orderRepositoryFactoryEx;
        public CustomerOrderServiceExtImpl(Func<IOrderExtensionRepository> orderRepositoryFactoryEx, IUniqueNumberGenerator uniqueNumberGenerator, IEventPublisher<OrderChangeEvent> orderChangingPublisher, IDynamicPropertyService dynamicPropertyService, IShippingMethodsService shippingMethodsService, IPaymentMethodsService paymentMethodsService, IStoreService storeService, IChangeLogService changeLogService, IEventPublisher<OrderChangedEvent> orderChangedPublisher)
            :base(orderRepositoryFactoryEx, uniqueNumberGenerator, orderChangingPublisher, dynamicPropertyService, shippingMethodsService, paymentMethodsService, storeService, changeLogService, orderChangedPublisher)
        {
            _orderChangingPublisher = orderChangingPublisher;
            _orderRepositoryFactoryEx = orderRepositoryFactoryEx;
        }
        #region ICustomerOrderService Members

        public override void SaveChanges(CustomerOrder[] orders) {
            var pkMap = new PrimaryKeyResolvingMap();

            using (var repository = RepositoryFactory())
            using (var changeTracker = GetChangeTracker(repository)) {
                var dataExistOrders = repository.GetCustomerOrdersByIds(orders.Where(x => !x.IsTransient()).Select(x => x.Id).ToArray(), CustomerOrderResponseGroup.Full);
                foreach (var order in orders) {
                    EnsureThatAllOperationsHaveNumber(order);

                    var originalEntity = dataExistOrders.FirstOrDefault(x => x.Id == order.Id);
                    var originalOrder = originalEntity != null ? (CustomerOrder)originalEntity.ToModel(AbstractTypeFactory<CustomerOrder>.TryCreateInstance()) : order;

                    var changeEvent = new OrderChangeEvent(originalEntity == null ? EntryState.Added : EntryState.Modified, originalOrder, order);
                    _orderChangingPublisher.Publish(changeEvent);

                    var modifiedEntity = AbstractTypeFactory<CustomerOrderEntity>.TryCreateInstance()
                                                                                 .FromModel(order, pkMap) as CustomerOrderEntity;
                    if (originalEntity != null) {
                        changeTracker.Attach(originalEntity);
                        modifiedEntity.Patch(originalEntity);
                    } else {
                        repository.Add(modifiedEntity);
                    }
                }
                CommitChanges(repository);
                pkMap.ResolvePrimaryKeys();
            }
            //Save dynamic properties
            foreach (var order in orders) {
                DynamicPropertyService.SaveDynamicPropertyValues(order);
            }

        }

        public override CustomerOrder[] GetByIds(string[] orderIds, string responseGroup = null)
        {
            var retVal = new List<CustomerOrder>();
            var orderResponseGroup = EnumUtility.SafeParse(responseGroup, CustomerOrderResponseGroup.Full);

            using (var repository = RepositoryFactory())
            {
                var orderEntities = repository.GetCustomerOrdersByIds(orderIds, orderResponseGroup);
                foreach (var orderEntity in orderEntities)
                {
                    var customerOrder = AbstractTypeFactory<CustomerOrder>.TryCreateInstance();
                    if (customerOrder != null)
                    {
                        var shippingMethods = ShippingMethodsService.GetAllShippingMethods();
                        var paymentMethods = PaymentMethodsService.GetAllPaymentMethods();

                        customerOrder = orderEntity.ToModel(customerOrder) as CustomerOrder;
                        if (!shippingMethods.IsNullOrEmpty())
                        {
                            foreach (var shipment in customerOrder.Shipments)
                            {
                                shipment.ShippingMethod = shippingMethods.FirstOrDefault(x => x.Code.EqualsInvariant(shipment.ShipmentMethodCode));
                            }
                        }
                        if (!paymentMethods.IsNullOrEmpty())
                        {
                            foreach (var payment in customerOrder.InPayments)
                            {
                                payment.PaymentMethod = paymentMethods.FirstOrDefault(x => x.Code.EqualsInvariant(payment.GatewayCode));
                            }
                        }
                        retVal.Add(customerOrder);
                    }
                }
            }
            DynamicPropertyService.LoadDynamicPropertyValues(retVal.ToArray());

            return retVal.ToArray();
        }

        public override void Delete(string[] ids)
        {
            var orders = GetByIds(ids, CustomerOrderResponseGroup.Full.ToString());
            using (var repository = RepositoryFactory())
            {
                var dbOrders = repository.GetCustomerOrdersByIds(ids, CustomerOrderResponseGroup.Full);
                foreach (var order in orders)
                {
                    _orderChangingPublisher.Publish(new OrderChangeEvent(VirtoCommerce.Platform.Core.Common.EntryState.Deleted, order, order));
                }
                repository.RemoveOrdersByIds(ids);
                foreach (var order in orders)
                {
                    DynamicPropertyService.DeleteDynamicPropertyValues(order);
                }
                repository.UnitOfWork.Commit();
            }
        }

        #endregion
        public GenericSearchResult<CustomerOrder> SearchCustomerOrdersExt(CustomerOrderSearchCriteria criteria)
        {
            var retVal = new GenericSearchResult<CustomerOrder>();

            using (var repository = _orderRepositoryFactoryEx())
            {
                var query = repository.CustomerOrders;

                var queryShipment = repository.ShipmentExtended.Where(s => s.IsApproved == true);
                var orderIds = queryShipment.Select(x => x.CustomerOrderId).ToArray();
                query = query.Where(x => orderIds.Contains(x.Id));

                //by default does not return prototypes
                if (!criteria.WithPrototypes)
                {
                    query = query.Where(x => !x.IsPrototype);
                }
                if (!criteria.Numbers.IsNullOrEmpty())
                {
                    query = query.Where(x => criteria.Numbers.Contains(x.Number));
                }
                else if (!string.IsNullOrEmpty(criteria.Keyword))
                {
                    query = query.Where(x => x.Number.Contains(criteria.Keyword) || x.CustomerName.Contains(criteria.Keyword));
                }
                if (criteria.OnlyRecurring)
                {
                    query = query.Where(x => x.SubscriptionId != null);
                }
                if (!criteria.SubscriptionIds.IsNullOrEmpty())
                {
                    query = query.Where(x => criteria.SubscriptionIds.Contains(x.SubscriptionId));
                }
                if (criteria.CustomerId != null)
                {
                    query = query.Where(x => x.CustomerId == criteria.CustomerId);
                }
                if (criteria.Statuses != null && criteria.Statuses.Any())
                {
                    query = query.Where(x => criteria.Statuses.Contains(x.Status));
                }
                if (criteria.StoreIds != null && criteria.StoreIds.Any())
                {
                    query = query.Where(x => criteria.StoreIds.Contains(x.StoreId));
                }
                if (criteria.EmployeeId != null)
                {
                    query = query.Where(x => x.EmployeeId == criteria.EmployeeId);
                }
                if (criteria.StartDate != null)
                {
                    query = query.Where(x => x.CreatedDate >= criteria.StartDate);
                }

                if (criteria.EndDate != null)
                {
                    query = query.Where(x => x.CreatedDate <= criteria.EndDate);
                }

                var sortInfos = criteria.SortInfos;
                if (sortInfos.IsNullOrEmpty())
                {
                    sortInfos = new[] { new SortInfo { SortColumn = ReflectionUtility.GetPropertyName<CustomerOrderEntity>(x => x.CreatedDate), SortDirection = SortDirection.Descending } };
                }
                query = query.OrderBySortInfos(sortInfos);

                retVal.TotalCount = query.Count();

                var orderCustomerIds = query.Select(x => x.Id).Skip(criteria.Skip).Take(criteria.Take).ToArray();
                var orders = GetByIds(orderCustomerIds, criteria.ResponseGroup);
                retVal.Results = orders.AsQueryable<CustomerOrder>().OrderBySortInfos(sortInfos).ToList();
               
                return retVal;
            }
        }

        protected override void EnsureThatAllOperationsHaveNumber(CustomerOrder order) {
            var store = StoreService.GetById(order.StoreId);

            foreach (var operation in order.GetFlatObjectsListWithInterface<VirtoCommerce.Domain.Commerce.Model.IOperation>()) {
                if (operation.Number == null) {
                    if (operation.OperationType == "CustomerOrder") {
                        CustomerOrderSearchCriteria criteria = new CustomerOrderSearchCriteria() { Sort = "createdDate:desc", Take = 1, ResponseGroup = "default" };
                        var lastOrder = _orderSearchService.SearchCustomerOrders(criteria).Results.First();
                        if (lastOrder != null) {
                            operation.Number = (Convert.ToInt32(lastOrder.Number) + 1).ToString();
                        }
                    } else {
                        var objectTypeName = operation.OperationType;
                        // take uppercase chars to form operation type, or just take 2 first chars. (CustomerOrder => CO, PaymentIn => PI, Shipment => SH)
                        var opType = string.Concat(objectTypeName.Select(c => char.IsUpper(c) ? c.ToString() : ""));
                        if (opType.Length < 2) {
                            opType = objectTypeName.Substring(0, 2).ToUpper();
                        }
                        var numberTemplate = opType + "{0:yyMMdd}-{1:D5}";
                        if (store != null) {
                            numberTemplate = store.Settings.GetSettingValue("Order." + objectTypeName + "NewNumberTemplate", numberTemplate);
                        }
                        operation.Number = UniqueNumberGenerator.GenerateNumber(numberTemplate);
                    }
                }
            }
        }
    }
}
