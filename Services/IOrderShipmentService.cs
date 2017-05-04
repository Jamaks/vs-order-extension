using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Domain.Commerce.Model.Search;
using VirtoCommerce.Domain.Order.Model;
using VirtoCommerce.Domain.Order.Services;

namespace OrderExtension.Web.Services
{
    public interface IOrderShipmentService
    {
        void Delete(string[] ids);
        CustomerOrder[] GetByIds(string[] orderIds, string responseGroup = null);
        void SaveChanges(CustomerOrder[] orders);
        GenericSearchResult<CustomerOrder> SearchCustomerOrdersExt(CustomerOrderSearchCriteria criteria);
    }
}
