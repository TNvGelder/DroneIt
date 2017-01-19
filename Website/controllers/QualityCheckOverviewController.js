/*
	@Author : Harmen Hilvers
	Controller used for the qualitycheck overview page
*/
app.controller("QualityCheckOverviewController", function ($scope, $http, $location) {
	$scope.warehouses= null;
	$scope.selectedWarehouse= null;
	$scope.products = [];
	$scope.selectedproduct = null;
	$scope.selectedrow = null;
	$scope.view = null;
	$scope.ExecutedQualitycheck = null;

	// method used to return array of the size of the given number
	$scope.getNumber = function(num) {
	    return new Array(num);   
	}

	// method used to set value of the selected product, this will trigger the product frame to show
	$scope.showProductFrame = function(District,column) {	
		$scope.selectedrow = $scope.getProductColumns(District,column);
		console.log($scope.selectedrow);
	}

	// Returns all products that are contained in the specific district and column
	$scope.getProductColumns = function(District,column) {
	    var products = [];
	    for (var i = 0; i < District.ProductLocations.length; i++) {
	    	if(District.ProductLocations[i].Column == column){
	    		products.push(District.ProductLocations[i]);
	    	}
	    }
	    return products;
	}

	// handles resizing of canvas when page size changes
	$( window ).resize(function() {
 		$scope.ResizeCanvas();
	});


	// method to resize the warehouse canvas to be responsive
	$scope.ResizeCanvas = function() {
		var width = $("#warehousecanvas").width();
  		var w = $scope.selectedWarehouse.Width;
  		var h = $scope.selectedWarehouse.Height;  		
  		var re = (100/w) * h;
  		var Height = width / 100 * re;

  		$("#warehousecanvas").height(Height);
  		$(".producttr").parent().parent().parent().parent().find(".productbutton").show(); 	

	}

	// method to get all warehouses from api
	$scope.LoadWarehouses = function() {
		$http.get("http://localhost:62553/api/Warehouse/GetWarehouses")
		.then(
			function successCallback(response) {				
		        $scope.warehouses = JSON.parse(response.data);	
		        $scope.selectedWarehouse = $scope.warehouses[0];
		        $scope.UpdateWarehouseDrawing();
 				$scope.ResizeCanvas();

		    }, 
		    function errorCallback(response) {
		    	alert("Geen verbinding met de API");
		    	console.log(response);
		    }
	    );
	}

	// method to get the current active qualitycheck, if there is a active quality check set page to qualitycheckstate
	$scope.GetActiveQualitycheck = function() {
		$http.get("http://localhost:62553/api/QualityCheck/GetQualityCheck")
		.then(
			function successCallback(response) {	        
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

	// method to set the the current sellected products
	$scope.SelectProduct = function(selectedproduct) {
		$scope.selectedproduct = selectedproduct;
		$scope.selectedrow = null;
	}

	// method to set the current view to graphic view
	$scope.setGraphicView = function() {
		$(".listviewmenu").removeClass("active");
		$(".graphicviewmenu").addClass("active");
		$(".graphicview").removeClass("hide");
		$(".listview").addClass("hide");
		$scope.view = "graphic";
		$scope.selectedproduct = null;
		$scope.selectedrow = null;	
 		$scope.ResizeCanvas();
	}

	// method to set the current view to list view
	$scope.setListView = function() {
		$(".graphicviewmenu").removeClass("active");
		$(".listviewmenu").addClass("active");
		$(".listview").removeClass("hide");
		$(".graphicview").addClass("hide");
		$scope.view = "list";
		$scope.selectedproduct = null;	
		$scope.selectedrow = null;
	}

	// method to update warehouse data to selected warehouse
	$scope.UpdateWarehouseDrawing = function() {
		$scope.selectedproduct = null;	
		$scope.products = $scope.GetProductsFromWarehouse($scope.selectedWarehouse);
		console.log($scope.products);
		
		$scope.setListView();
	}

	// method the execute a qualitycheck on the currend selected product
	$scope.ExecuteQualityCheck = function() {
		console.log("executing qualitycheck");
		$http.post( "http://localhost:62553/api/QualityCheck/PostQualityCheck",  {id: $scope.selectedproduct.Id} )
		.then(
			function successCallback(response) {
				console.log(response);
				$location.path("QualityCheckState");
		    }, 
		    function errorCallback(response) {
				$location.path("QualityCheckState");
		    	console.log(response);		    
		    }
		);
	
	}

	// method to get all products from warehouse as a list
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

