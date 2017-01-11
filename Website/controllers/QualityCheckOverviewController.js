app.controller("QualityCheckOverviewController", function ($scope, $http, $location) {
	$scope.warehouses= null;
	$scope.selectedWarehouse= null;
	$scope.products = [];
	$scope.selectedproduct = null;
	$scope.selectedrow = null;
	$scope.view = null;

	$scope.ExecutedQualitycheck = null;
	$scope.getNumber = function(num) {
	    return new Array(num);   
	}


	$scope.showProductFrame = function(District,column) {	
		$scope.selectedrow = $scope.getProductColumns(District,column);
		console.log($scope.selectedrow);
	}
	$scope.getProductColumns = function(District,column) {
	    var products = [];
	    for (var i = 0; i < District.ProductLocations.length; i++) {
	    	if(District.ProductLocations[i].Column == column){
	    		products.push(District.ProductLocations[i]);
	    	}
	    }
	    return products;
	}

	$( window ).resize(function() {
 		$scope.ResizeCanvas();
	});

	$scope.ResizeCanvas = function() {
		var width = $("#warehousecanvas").width();
  		var w = $scope.selectedWarehouse.Width;
  		var h = $scope.selectedWarehouse.Height;
  		
  		var re = (100/w) * h;

  		var Height = width / 100 * re;
		console.log(Height);
  		$("#warehousecanvas").height(Height);
  		$(".producttr").parent().parent().parent().parent().find(".productbutton").show(); 	

	}



	$scope.LoadWarehouses = function() {
		$http.get("http://localhost:62553/api/Warehouse/GetWarehouses")
		.then(
			function successCallback(response) {				
		        $scope.warehouses = JSON.parse(response.data);
		        console.log($scope.warehouses);
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
		$scope.selectedrow = null;
	}

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

	$scope.setListView = function() {
		$(".graphicviewmenu").removeClass("active");
		$(".listviewmenu").addClass("active");
		$(".listview").removeClass("hide");
		$(".graphicview").addClass("hide");
		$scope.view = "list";
		$scope.selectedproduct = null;	
		$scope.selectedrow = null;
	}

	$scope.drawWarehouse = function() {		
		console.log($scope.selectedWarehouse);
		console.log("drawing warehouse");				
	}

	$scope.UpdateWarehouseDrawing = function() {
		$scope.selectedproduct = null;	

		$scope.products = $scope.GetProductsFromWarehouse($scope.selectedWarehouse);
		console.log($scope.products);

		$scope.drawWarehouse();
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
		    	//alert("Something went wrong.")
				$location.path("QualityCheckState");
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

