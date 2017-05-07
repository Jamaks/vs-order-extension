ShipmentModule.controller('Jamak.ShipmentModule.shipmentsListController', ['$scope', '$http', function ($scope, $http,bladeUtils, dateFilter,uiGridConstants, uiGridHelper, gridOptionExtension) {
    var blade = $scope.blade;
    $scope.uiGridConstants = uiGridConstants;


    $scope.data = "Shipment content";
    blade.title = "Shipment module";
    load();
    blade.refresh = function () {
        load();
    };
    $scope.list=[];
    function load() {
        blade.isLoading = true;
        $http.post("/api/order/delivery/search", { "sort": "createdDate:desc", "skip": 0, "take": 20 })
            .then(function success(resp) {
                debugger
                $scope.objects = resp.data.results;
                blade.isLoading = false;
            }, function error(resp) {
                blade.isLoading = false;
            });
    }

    blade.toolbarCommands = [
          {
              name: "platform.commands.refresh", icon: 'fa fa-refresh',
              executeMethod: blade.refresh,
              canExecuteMethod: function () {
                  return true;
              }
          }]
    // ui-grid
    $scope.setGridOptions = function (gridId, gridOptions) {
        // TODO: refactor this
        var createdDateColumn = _.findWhere(gridOptions.columnDefs, { name: 'createdDate' });
        if (createdDateColumn) { // custom tooltip
            createdDateColumn.cellTooltip = function (row, col) { return dateFilter(row.entity.createdDate, 'medium'); }
        }

        $scope.gridOptions = gridOptions;
        //gridOptionExtension.tryExtendGridOptions(gridId, gridOptions);

        //uiGridHelper.initialize($scope, gridOptions, function (gridApi) {
        //    uiGridHelper.bindRefreshOnSortChanged($scope);
        //});

       // bladeUtils.initializePagination($scope);

        return gridOptions;
    };

}]);
