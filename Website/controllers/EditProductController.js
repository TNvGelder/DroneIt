app.controller("EditProductController", function ($scope, $http, $location, $routeParams) {
    $scope.ProductID = $routeParams.Id;

	$http.get( "http://localhost:62553/api/Products/" + $scope.ProductID)
	.then(
		function successCallback(response) {
			$scope.ProductName = response.data.Name;
	    }, 
	    function errorCallback(response) {
	    	alert("Geen verbinding met de API");
	    }
	);

	$scope.EditProduct = function() {
		$http.put("http://localhost:62553/api/Products/" + $scope.ProductID, { Id: $scope.ProductID, Name: $scope.ProductName })
		.then(
			function successCallback(response) {
				$location.path("/")
		    }, 
		    function errorCallback(response) {
		    	alert("Geen verbinding met de API");
		    }
		);
	}

	$scope.Cancel = function() {
		$location.path("/");
	}
});