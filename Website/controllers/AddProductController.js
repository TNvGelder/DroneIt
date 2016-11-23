app.controller("AddProductController", function ($scope, $http, $location) {
	$scope.ProductName; 

	//Saves a product by submitting it to the API
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

	//Goes back to the overview page.
	$scope.Cancel = function() {
		$location.path("/");
	}
});