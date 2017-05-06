ShipmentModule.controller('Jamak.ShipmentModule.shipmentsListController', ['$scope', '$http', function ($scope, $http) {
    var blade = $scope.blade;

    $scope.data = "Shipment content";
    blade.title = "Shipment module";
    blade.isLoading = true;
    $http.post("/api/order/delivery/search", { "sort": "createdDate:desc", "skip": 0, "take": 20 })
        .then(function success(resp) {
            console.log(resp.data)
            blade.isLoading = false;
        }, function error(resp) {
            blade.isLoading = false;
        });
}]);
