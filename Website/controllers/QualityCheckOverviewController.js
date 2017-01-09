app.controller("QualityCheckOverviewController", function ($scope, $http, $location) {
	$scope.warehouses = [];

	$scope.LoadWarehouses = function() {
		$http.get("http://localhost:62553/api/qualitycheck")
		.then(
			function successCallback(response) {
		        $scope.warehouses = response.data;
		    }, 
		    function errorCallback(response) {
		    	//alert("Geen verbinding met de API");
		    }
	    );
	}

	$scope.UpdateWarehouseDrawing = function() {
		console.log("drawing warehouse");
	}

	$scope.ExecuteQualityCheck = function() {
		console.log("executing qualitycheck");
	}



	$scope.LoadWarehouses();
});

