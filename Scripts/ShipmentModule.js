//Call this to register our module to main application
var moduleName = "Jamak.ShipmentModule";

if (AppDependencies != undefined) {
	AppDependencies.push(moduleName);
}

var ShipmentModule= angular.module(moduleName, [
    
])
.config(
	['$stateProvider',
		function ($stateProvider) {
			$stateProvider
				.state('workspace.shipment', {
					url: '/shipment',
					templateUrl: 'Scripts/common/templates/home.tpl.html',
					controller: [
						'$scope', 'platformWebApp.bladeNavigationService', function ($scope, bladeNavigationService) {
							var blade = {
								id: 'shipmentsList',
								// controller name must be unique in Application. Use prefix like 'um-'.
								controller: 'Jamak.ShipmentModule.shipmentsListController',
								template: 'Modules/Order.Shipment/Scripts/blades/shipmentsList.tpl.html',
								isClosingDisabled: true
							};
							bladeNavigationService.showBlade(blade);
						}
					]
				});
		}
	]
)
.run(
	['$rootScope', 'platformWebApp.mainMenuService','$state', function ($rootScope, mainMenuService,$state) {
 var menuItem = {
            path: 'browse/shipment',
            icon: 'fa fa-truck',
            title: 'Shipment',
            priority: 90,
            action: function () { $state.go('workspace.shipment'); },
            permission: 'order:access'
        };
        mainMenuService.addMenuItem(menuItem);
	}])

