using OrderExtension.Web.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.OrderModule.Data.Repositories;

namespace OrderExtension.Web
{
    public interface IOrderExtensionRepository: IOrderRepository
    {
        IQueryable<ShipmentExtensionEntity> ShipmentExtended { get;  }
        IQueryable<CartShipmentExtensionEntity> CartShipmentExtended { get; }

    }
}
