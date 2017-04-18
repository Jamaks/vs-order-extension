using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VirtoCommerce.Domain.Order.Services;
using VirtoCommerce.Domain.Store.Services;
using VirtoCommerce.OrderModule.Data.Services;
using orderModel = VirtoCommerce.Domain.Order.Model;
using cartModel = VirtoCommerce.Domain.Cart.Model;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Domain.Commerce.Model;

namespace OrderExtension.Web.Services {

    /// <summary>
    /// Override the original CustomerOrderBuilderImpl to copy extended fields which added in this module
    /// </summary>
    public class CustomerOrderBuilderExtImpl : CustomerOrderBuilderImpl
    {
        private ICustomerOrderService _customerOrderService;
        private IStoreService _storeService;

        public CustomerOrderBuilderExtImpl(ICustomerOrderService customerOrderService, IStoreService storeService) : base(customerOrderService, storeService)
        {
            _storeService = storeService;
            _customerOrderService = customerOrderService;
        }

        protected override orderModel.CustomerOrder ConvertCartToOrder(cartModel.ShoppingCart cart)
        {
            var retVal = base.ConvertCartToOrder(cart);
            //TODO: Invoices 
            return retVal;
        }

        protected override orderModel.Shipment ToOrderModel(cartModel.Shipment shipment)
        {
            var shipment2 = base.ToOrderModel(shipment) as Model.ShipmentExtension;
            if (shipment2 != null)
            {
                shipment2.HasLoadingDock = shipment2.HasLoadingDock;
                shipment2.IsCommercial = shipment2.IsCommercial;
                shipment2.Comment = shipment2.Comment;
            }
            return shipment2;
        }
        
    }
}
