using CartExtModule.Migrations;
using CartExtModule.Model;
using Microsoft.Practices.Unity;
using CartExtModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VirtoCommerce.CartModule.Data.Repositories;
using VirtoCommerce.Domain.Commerce.Model;
using VirtoCommerce.Domain.Order.Model;
using VirtoCommerce.Domain.Order.Services;

using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.Modularity;
using VirtoCommerce.Platform.Data.Infrastructure;
using VirtoCommerce.Platform.Data.Infrastructure.Interceptors;

namespace CartExtModule
{
    public class Module : ModuleBase
    {
        private const string _connectionStringName = "VirtoCommerce";
        private readonly IUnityContainer _container;

        public Module(IUnityContainer container)
        {
            _container = container;
        }
        public override void SetupDatabase()
        {
            using (var db = new ShopCartExtensionRepository(_connectionStringName, _container.Resolve<AuditableInterceptor>()))
            {
                var initializer = new SetupDatabaseInitializer<ShopCartExtensionRepository, Configuration>();
                initializer.InitializeDatabase(db);
            }



        }
        public override void Initialize()
        {
            base.Initialize();

            _container.RegisterType<ICartRepository>(new InjectionFactory(c => new ShopCartExtensionRepository(_connectionStringName, _container.Resolve<AuditableInterceptor>(), new EntityPrimaryKeyGeneratorInterceptor())));

        }

        public override void PostInitialize()
        {
            base.PostInitialize();


            AbstractTypeFactory<VirtoCommerce.Domain.Cart.Model.Shipment>.OverrideType<VirtoCommerce.Domain.Cart.Model.Shipment, CartShipmentExtension>();
            AbstractTypeFactory<VirtoCommerce.CartModule.Data.Model.ShipmentEntity>.OverrideType<VirtoCommerce.CartModule.Data.Model.ShipmentEntity, CartShipmentExtensionEntity>();
        }

    }

}