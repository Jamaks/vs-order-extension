//Call this to register our module to main application
var moduleName = "virtoCommerce.samples.order2";

if (AppDependencies != undefined) {
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

	    //contactInfo.detailBlade.knownChildrenOperations.push('Shipment');


	    //var foundTemplate = knownOperations.getOperation('CustomerOrder');
	    //if (foundTemplate) {
	    //    foundTemplate.detailBlade.metaFields.push(UER
	    //        {
	    //            name: 'newField',
	    //            title: "New field",
	    //            valueType: "ShortText"
	    //        });

	    //    foundTemplate.detailBlade.knownChildrenOperations.push('Invoice');
	    //}

	    //var invoiceOperation = {
	    //    type: 'Invoice',
	    //    description: 'Sample Invoice document',
	    //    treeTemplateUrl: 'invoiceOperation.tpl.html',
	    //    detailBlade: {
	    //        template: 'Modules/OrderExtension/Scripts/blades/invoice-detail.tpl.html',
	    //        metaFields: [
	    //            {
	    //                name: 'number',
	    //                isRequired: true,
	    //                title: "Invoice number",
	    //                valueType: "ShortText"
	    //            },
	    //            {
	    //                name: 'createdDate',
	    //                isReadonly: true,
	    //                title: "created",
	    //                valueType: "DateTime"
	    //            },
	    //            {
	    //                name: 'customerId',
	    //                title: "Customer",
	    //                templateUrl: 'customerSelector.html'
	    //            }
	    //        ]
	    //    }
	    //};
	    //knownOperations.registerOperation(invoiceOperation);

	    //$http.get('Modules/OrderExtension/Scripts/tree-template.html').then(function (response) {
	    //    // compile the response, which will put stuff into the cache
	    //    $compile(response.data);
	    //});
	}]);