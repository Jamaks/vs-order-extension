//Call this to register our module to main application
var moduleName = "virtoCommerce.samples.order2";

if (AppDependencies !== undefined) {
    AppDependencies.push(moduleName);
}

angular.module(moduleName, [])
.run(
  ['virtoCommerce.orderModule.knownOperations', '$http', '$compile',
	function (knownOperations, $http, $compile) {
	    var contactInfo = knownOperations.getOperation("Shipment");
	    if (contactInfo) {
	        contactInfo.detailBlade.metaFields.push(
           {
               name: 'shipmentDate',
               title: "Date of shipment",
               valueType: "DateTime"
           });
	        contactInfo.detailBlade.metaFields.push({
	            name: 'isCommercial',
	            title: "isCommercial",
	            valueType: "ShortText"
	        });
	        contactInfo.detailBlade.metaFields.push({
	            name: 'hasLoadingDock',
	            title: "hasLoadingDock",
	            valueType: "ShortText"
	        });

	    }
	}]);