using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.CartModule.Data.Model;
using VirtoCommerce.Domain.Cart.Model;

namespace CartExtModule.Model
{
    public class CartShipmentExtension: Shipment
    {
        public DateTime shipmentDate { get; set; }
    }
}
