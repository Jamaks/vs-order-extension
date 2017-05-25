using OrderExtension.Web.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.CartModule.Data.Repositories;

namespace OrderExtension.Web
{
    public interface IShopCartExtensionRepository: ICartRepository
    {
        IQueryable<CartShipmentExtensionEntity> CartShipmentExtended { get; }
    }
}
