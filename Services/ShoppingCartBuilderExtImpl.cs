using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.CartModule.Data.Services;
using VirtoCommerce.Domain.Cart.Services;
using VirtoCommerce.Domain.Customer.Services;
using VirtoCommerce.Domain.Store.Services;

namespace OrderExtension.Web.Services
{
    public class ShoppingCartBuilderExtImpl : ShoppingCartBuilderImpl
    {
        private IStoreService _storeService;
        private IShoppingCartService _shoppingShoppingCartService;
        private IShoppingCartSearchService _shoppingCartSearchService;
        public ShoppingCartBuilderExtImpl(IStoreService storeService, IShoppingCartService shoppingShoppingCartService, IShoppingCartSearchService shoppingCartSearchService, IMemberService memberService) : base(storeService, shoppingShoppingCartService, shoppingCartSearchService, memberService)
        {
            _storeService = storeService;
            _shoppingShoppingCartService = shoppingShoppingCartService;
            _shoppingCartSearchService = shoppingCartSearchService;
        }
        public  override  T
    }
}
