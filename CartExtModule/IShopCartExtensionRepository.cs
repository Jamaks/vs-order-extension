using CartExtModule.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.CartModule.Data.Repositories;

namespace CartExtModule
{
    public interface IShopCartExtensionRepository: ICartRepository
    {
        IQueryable<CartShipmentExtensionEntity> CartShipmentExtended { get; }
    }
}
