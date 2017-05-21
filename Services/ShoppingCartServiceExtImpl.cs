using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.CartModule.Data.Repositories;
using VirtoCommerce.CartModule.Data.Services;
using VirtoCommerce.Domain.Cart.Events;
using VirtoCommerce.Platform.Core.DynamicProperties;
using VirtoCommerce.Platform.Core.Events;

namespace OrderExtension.Web.Services
{
    public class ShoppingCartServiceExtImpl : ShoppingCartServiceImpl
    {
        public ShoppingCartServiceExtImpl(Func<ICartRepository> repositoryFactory, IEventPublisher<CartChangeEvent> changingEventPublisher, IDynamicPropertyService dynamicPropertyService, IEventPublisher<CartChangedEvent> changedEventPublisher) : base(repositoryFactory, changingEventPublisher, dynamicPropertyService, changedEventPublisher)
        {
        }
    }
}
