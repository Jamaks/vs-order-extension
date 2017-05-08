ShipmentModule.controller('Jamak.ShipmentModule.shipmentsListController',
    ['$scope', '$localStorage', '$http', 'platformWebApp.bladeUtils', 'dateFilter', 'uiGridConstants', 'platformWebApp.uiGridHelper', 'platformWebApp.bladeNavigationService', 'platformWebApp.ui-grid.extension',
function ($scope, $localStorage, $http, bladeUtils, dateFilter, uiGridConstants, uiGridHelper, bladeNavigationService, gridOptionExtension) {

        var blade = $scope.blade;
        $scope.uiGridConstants = uiGridConstants;


        $scope.data = "Shipment content";
        blade.title = "Shipment module";
       
        blade.refresh = function () {
            load();
        };
        $scope.list = [];
        function load() {
            blade.isLoading = true;
            var criteria = {
                keyword: filter.keyword,
                sort: uiGridHelper.getSortExpression($scope),
                skip: ($scope.pageSettings.currentPage - 1) * $scope.pageSettings.itemsPerPageCount,
                take: $scope.pageSettings.itemsPerPageCount
            };
            $http.post("/api/order/delivery/search", criteria)
                .then(function success(resp) {
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
        $scope.selectNode = function (node) {
            $scope.selectedNodeId = node.id;
            var newBlade = {
                id: 'shipmentOrder',
                // controller name must be unique in Application. Use prefix like 'um-'.
                controller: 'Jamak.ShipmentModule.shipmentOrderController',
                template: 'Modules/OrderExtension/Scripts/blades/shipmentDetail.tpl.html',
                isClosingDisabled: false,
                isExpandable: false,
                customerOrder: node,
                metaFields: [
            
                ]
            };
            bladeNavigationService.showBlade(newBlade, blade);
        };


        // ui-grid
        $scope.setGridOptions = function (gridId, gridOptions) {
            debugger
            // TODO: refactor this
            var createdDateColumn = _.findWhere(gridOptions.columnDefs, { name: 'createdDate' });
            if (createdDateColumn) { // custom tooltip
                createdDateColumn.cellTooltip = function (row, col) { return dateFilter(row.entity.createdDate, 'medium'); }
            }

            $scope.gridOptions = gridOptions;
            gridOptionExtension.tryExtendGridOptions(gridId, gridOptions);

            uiGridHelper.initialize($scope, gridOptions, function (gridApi) {
                uiGridHelper.bindRefreshOnSortChanged($scope);
            });

             bladeUtils.initializePagination($scope);

            return gridOptions;
        };
        // simple and advanced filtering
        var filter = blade.filter = $scope.filter = {};
        $scope.$localStorage = $localStorage;
        if (!$localStorage.orderSearchFilters) {
            $localStorage.orderSearchFilters = [{ name: 'orders.blades.customerOrder-list.labels.new-filter' }]
        }
        if ($localStorage.orderSearchFilterId) {
            filter.current = _.findWhere($localStorage.orderSearchFilters, { id: $localStorage.orderSearchFilterId });
        }
        filter.criteriaChanged = function () {
            if ($scope.pageSettings.currentPage > 1) {
                $scope.pageSettings.currentPage = 1;
            } else {
                blade.refresh();
            }
        };

    //load data
        //setTimeout(load, 500);
    }]);
