using OrderExtension.Web.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using VirtoCommerce.Domain.Order.Model;
using VirtoCommerce.OrderModule.Data.Model;
using VirtoCommerce.OrderModule.Data.Repositories;
using VirtoCommerce.Platform.Data.Infrastructure.Interceptors;

namespace OrderExtension.Web
{
    public class OrderExtensionRepository : OrderRepositoryImpl, IOrderExtensionRepository
    {
        public OrderExtensionRepository()
        {
        }

        public OrderExtensionRepository(string nameOrConnectionString, params IInterceptor[] interceptors)
            : base(nameOrConnectionString, interceptors)
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {

            #region ShipmentExtension
            modelBuilder.Entity<ShipmentExtensionEntity>().HasKey(x => x.Id)
                .Property(x => x.Id);
            modelBuilder.Entity<ShipmentExtensionEntity>().ToTable("OrderShipmentExtension");

           // modelBuilder.Entity<CartShipmentExtensionEntity>().ToTable("CartShipmentExtension2").HasKey(x => x.Id)
           //.Property(x => x.Id);
            #endregion

            base.OnModelCreating(modelBuilder);
        }

        public IQueryable<ShipmentExtensionEntity> ShipmentExtended
        {
            get { return GetAsQueryable<ShipmentExtensionEntity>(); }
        }

        
    }
}