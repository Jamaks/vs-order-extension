using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Domain.Cart.Model;
using VirtoCommerce.CartModule.Data.Model;
using VirtoCommerce.Platform.Core.Common;

namespace OrderExtension.Web.Model
{
    public class CartShipmentExtensionEntity: ShipmentEntity
    {
        public DateTime shipmentDate { get; set; }
        public override Shipment ToModel(Shipment shipment)
        {
            var result = base.ToModel(shipment);
            var shipment2 = result as CartShipmentExtension;
            shipment2.shipmentDate = this.shipmentDate;
            shipment2.Items = this.Items.Select(x => x.ToModel(AbstractTypeFactory<ShipmentItem>.TryCreateInstance())).ToList();

            return shipment2;
        }
        public override ShipmentEntity FromModel(Shipment shipment, PrimaryKeyResolvingMap pkMap)
        {
            base.FromModel(shipment, pkMap);
            CartShipmentExtension shipment2 = new CartShipmentExtension();
            shipment2 = (CartShipmentExtension)shipment;
            this.shipmentDate = shipment2.shipmentDate;
            return this;
           
        }
        public override void Patch(ShipmentEntity target)
        {
            base.Patch(target);
            var shipmentExtensionEntity = target as CartShipmentExtensionEntity;
            shipmentExtensionEntity.shipmentDate = this.shipmentDate;
        }
    }
}
