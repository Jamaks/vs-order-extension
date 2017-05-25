using OrderExtension.Web.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.CartModule.Data.Repositories;
using VirtoCommerce.Platform.Data.Infrastructure.Interceptors;

namespace OrderExtension.Web
{
    public class ShopCartExtensionRepository : CartRepositoryImpl, IShopCartExtensionRepository
    {
        public ShopCartExtensionRepository()
        {
        }

        public ShopCartExtensionRepository(string nameOrConnectionString, params IInterceptor[] interceptors)
            : base(nameOrConnectionString, interceptors)
        {
        }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {

            #region ShipmentExtension
            modelBuilder.Entity<CartShipmentExtensionEntity>().HasKey(x => x.Id)
            .Property(x => x.Id);
            modelBuilder.Entity<CartShipmentExtensionEntity>().ToTable("CartShipmentExtension");
            #endregion

            base.OnModelCreating(modelBuilder);
        }
        public IQueryable<CartShipmentExtensionEntity> CartShipmentExtended
        {
            get { return GetAsQueryable<CartShipmentExtensionEntity>(); }
        }
    }
}
