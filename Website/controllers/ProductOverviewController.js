app.controller("ProductOverviewController", function ($scope, $http, $location) {
    $scope.products = [];

    $scope.LoadProducts = function() {
		$http.get("http://localhost:62553/api/Products")
		.then(
			function successCallback(response) {
		        $scope.products = response.data;
		    }, 
		    function errorCallback(response) {
		    	alert("Geen verbinding met de API");
		    }
	    );
	}

	$scope.EditProduct = function(Id) {
		$location.path("/edit/" + Id);
	}

    $scope.DeleteProduct = function(Id) {
    	if(confirm("Weet u zeker dat u dit product wilt verwijderen?")) {
	   		$http.delete("http://localhost:62553/api/Products/" + Id).then(function() {
		   		$scope.LoadProducts();
	   		});
    	}
    }

    $scope.LoadProducts();
});