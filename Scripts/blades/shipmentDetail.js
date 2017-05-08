ShipmentModule.controller('Jamak.ShipmentModule.shipmentOrderController',
    ['$scope', '$http', 'platformWebApp.bladeUtils', 'dateFilter', 'uiGridConstants', 'platformWebApp.uiGridHelper', 'platformWebApp.bladeNavigationService',
    function ($scope, $http, bladeUtils, dateFilter, uiGridConstants, uiGridHelper, bladeNavigationService) {

        var blade = $scope.blade;
        $scope.uiGridConstants = uiGridConstants;

        blade.title = "Order shipment";
        blade.isLoading = false;
        blade.headIcon = 'fa-file-text';
        blade.shipmentStatuses = [];

        //init
        $http.get('api/platform/settings/values/Shipment.Status').then(function (resp) {
            blade.shipmentStatuses = resp.data;
        });
        blade.currentEntity = angular.copy(blade.customerOrder);

        blade.save = function () {
            blade.isLoading = true;
            var req = {
                method: 'PUT',
                url: 'api/order/customerOrders',
                data: blade.customerOrder
            }
            $http(req).then(function (resp) {
                blade.isLoading = false;
            });
        }
        blade.toolbarCommands = [
            {
                name: "platform.commands.save", icon: 'fa fa-save',
                executeMethod: blade.save,
                canExecuteMethod: function () {
                    return true;
                }
            },
        ]
    }]);