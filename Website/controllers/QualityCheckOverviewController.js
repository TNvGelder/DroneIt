app.controller("QualityCheckOverviewController", function ($scope, $http, $location) {
	$scope.warehouses= null;
	$scope.selectedWarehouse= null;
	$scope.products = [];
	$scope.selectedproduct = null;

	$scope.ExecutedQualitycheck = null;

	$scope.LoadWarehouses = function() {
		$http.get("http://localhost:62553/api/Warehouse/GetWarehouses")
		.then(
			function successCallback(response) {				
		        $scope.warehouses = JSON.parse(response.data);
		        console.log($scope.warehouses);
		        $scope.selectedWarehouse = $scope.warehouses[0];
		        $scope.UpdateWarehouseDrawing()
		    }, 
		    function errorCallback(response) {
		    	alert("Geen verbinding met de API");
		    	console.log(response);
		    }
	    );
	}

	$scope.GetActiveQualitycheck = function() {
		$http.get("http://localhost:62553/api/QualityCheck/GetQualityCheck")
		.then(
			function successCallback(response) {				
		        console.log(response);	             
		  
                if(response.data != "null"){
					$location.path("QualityCheckState");
		        }else{		       
		        	console.log(JSON.parse(response.data));	
		        }
		    }, 
		    function errorCallback(response) {
		    	alert("Geen verbinding met de API");
		    	console.log(response);

		    }
	    );
	}

	$scope.SelectProduct = function(selectedproduct) {
		$scope.selectedproduct = selectedproduct;
	}

	$scope.setGraphicView = function() {
		$(".listviewmenu").removeClass("active");
		$(".graphicviewmenu").addClass("active");
		$(".graphicview").removeClass("hide");
		$(".listview").addClass("hide");

	}

	$scope.setListView = function() {
		$(".graphicviewmenu").removeClass("active");
		$(".listviewmenu").addClass("active");
		$(".listview").removeClass("hide");
		$(".graphicview").addClass("hide");
	}

	$scope.UpdateWarehouseDrawing = function() {
		$scope.selectedproduct = null;

		console.log($scope.selectedWarehouse);
		console.log("drawing warehouse");	

		$scope.products = $scope.GetProductsFromWarehouse($scope.selectedWarehouse);
		console.log($scope.products);

		$scope.setListView();
	}

	$scope.ExecuteQualityCheck = function() {
		console.log("executing qualitycheck");
		$http.post( "http://localhost:62553/api/QualityCheck/PostQualityCheck",  {id: $scope.selectedproduct.Id} )
		.then(
			function successCallback(response) {
				console.log(response);
				$location.path("QualityCheckState");
		    }, 
		    function errorCallback(response) {
		    	alert("Something went wrong.")
		    	console.log(response);		    
		    }
		);
	
	}

	$scope.GetProductsFromWarehouse = function(warehouse) {
		var products = [];
		for (var i = 0; i < warehouse.Districts.length; i++) {
			for (var x = 0; x < warehouse.Districts[i].ProductLocations.length; x++) {
				warehouse.Districts[i].ProductLocations[x].District = warehouse.Districts[i].Name;
				products.push(warehouse.Districts[i].ProductLocations[x]);

			}
		}
		return products;
	}


	$scope.GetActiveQualitycheck();
	$scope.LoadWarehouses();
});

