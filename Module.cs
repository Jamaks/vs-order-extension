using Microsoft.Practices.Unity;
using OrderExtension.Web;
using OrderExtension.Web.Migrations;
using OrderExtension.Web.Model;
using OrderExtension.Web.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VirtoCommerce.Domain.Commerce.Model;
using VirtoCommerce.Domain.Order.Model;
using VirtoCommerce.Domain.Order.Services;
using VirtoCommerce.OrderModule.Data.Model;
using VirtoCommerce.OrderModule.Data.Repositories;
using VirtoCommerce.OrderModule.Data.Services;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.Modularity;
using VirtoCommerce.Platform.Data.Infrastructure;
using VirtoCommerce.Platform.Data.Infrastructure.Interceptors;

namespace OrderExtension.Web
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
            using (var db = new OrderExtensionRepository(_connectionStringName, _container.Resolve<AuditableInterceptor>()))
            {
                var initializer = new SetupDatabaseInitializer<OrderExtensionRepository, Configuration>();
                initializer.InitializeDatabase(db);
            }


        }
        public override void Initialize()
        {
            base.Initialize();

            _container.RegisterType<IOrderRepository>(new InjectionFactory(c => new OrderExtensionRepository(_connectionStringName, _container.Resolve<AuditableInterceptor>(), new EntityPrimaryKeyGeneratorInterceptor())));
            //Override ICustomerOrderBuilder default implementation
            _container.RegisterType<ICustomerOrderBuilder, CustomerOrderBuilderExtImpl>();
           // _container.RegisterType<ICustomerOrderService, CustomerOrderServiceExtImpl>();

        }

        public override void PostInitialize()
        {
            base.PostInitialize();


            AbstractTypeFactory<Shipment>.OverrideType<Shipment, ShipmentExtension>().WithFactory(()=>new ShipmentExtension() { OperationType="Shipment"});
            AbstractTypeFactory<ShipmentEntity>.OverrideType<ShipmentEntity, ShipmentExtensionEntity>();

            //Thats need for PolymorphicOperationJsonConverter for API deserialization
            AbstractTypeFactory<IOperation>.RegisterType<ShipmentExtension>();
        }

    }

}