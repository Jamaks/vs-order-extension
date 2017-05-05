using OrderExtension.Web.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using VirtoCommerce.Domain.Order.Model;

namespace OrderExtension.Web.Controllers.Api
{
    [RoutePrefix("api/order/delivery")]
    public class DeliveryModuleController : ApiController
    {
        private readonly IOrderShipmentService _orderShipmenyService;
        public DeliveryModuleController(IOrderShipmentService orderShipmenyService)
        {
            _orderShipmenyService = orderShipmenyService;
        }
        /// <summary>
        /// Search customer orders by given criteria
        /// </summary>
        /// <param name="criteria">criteria</param>
        [HttpPost]
        [Route("search")]
        public IHttpActionResult Search(CustomerOrderSearchCriteria criteria)
        {
            //Scope bound ACL filtration
            //criteria = FilterOrderSearchCriteria(HttpContext.Current.User.Identity.Name, criteria);

            // var result = _searchService.SearchCustomerOrders(criteria);
            //var retVal = new webModel.CustomerOrderSearchResult
            //{
            //    CustomerOrders = result.Results.ToList(),
            //    TotalCount = result.TotalCount
            //};
            var retVal = _orderShipmenyService.SearchCustomerOrdersExt(criteria);
            return Ok(retVal);
        }
    }
}
