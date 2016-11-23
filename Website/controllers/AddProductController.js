app.controller("AddProductController", function ($scope, $http, $location) {
	$scope.ProductName; 

	$scope.SaveProduct = function() {
		$http.post( "http://localhost:62553/api/Products", { Name: $scope.ProductName })
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