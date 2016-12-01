app.controller("ProductOverviewController", function ($scope, $http, $location) {
    $scope.products = [];

    //This methods loads the products from the API and puts them into the products array.
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

	//Go to the edit page.
	$scope.EditProduct = function(Id) {
		$location.path("/edit/" + Id);
	}

	//Delete a product after confirmation.
    $scope.DeleteProduct = function(Id) {
    	if(confirm("Weet u zeker dat u dit product wilt verwijderen?")) {
	   		$http.delete("http://localhost:62553/api/Products/" + Id).then(function() {
		   		$scope.LoadProducts();
	   		});
    	}
    }

    $scope.LoadProducts();
});