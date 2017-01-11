app.controller("QualityCheckStateController", function ($scope, $http, $location) {
	$scope.ActiveQualitycheck = null;
	$scope.activeWarehouse= null;
	$scope.loaded= null;

	$scope.GetActiveQualitycheckState = function() {
		$http.get("http://localhost:62553/api/QualityCheck/GetQualityCheck")
		.then(
			function successCallback(response) {				
		        console.log(response);	
		        if(response.data == "null"){
					$location.path("qualitycheck");
		        }else{

		       	 	$scope.ActiveQualitycheck = JSON.parse(response.data);	         	
		       	 	console.log($scope.activeWarehouse);
		       	 	if( $scope.activeWarehouse == null){
						$scope.LoadWarehouse();

		       	 	}
		       	 	$scope.ResizeCanvas();
		        }
		    }, 
		    function errorCallback(response) {
		    	alert("Geen verbinding met de API");
		    	console.log(response);
		    }
	    );
	}

	$scope.ResizeCanvas = function() {
		var width = $("#warehousecanvas").width();
  		var w = $scope.activeWarehouse.Width;
  		var h = $scope.activeWarehouse.Height;
  		var re = (100/w) * h;
  		var Height = width / 100 * re;
		console.log(Height);
  		$("#warehousecanvas").height(Height);
  		$("#hiddencanvas").height(Height);	
  		$("#hiddencanvas").width(width);	
  		var canvas = document.getElementById('hiddencanvas');
		canvas.width = width;
		canvas.height = Height;
		$scope.DrawLines();
		if($scope.loaded == null){
			
			$scope.loaded= 1;
		}

	}

	$scope.LoadWarehouse = function() {
		
		$http.get("http://localhost:62553/api/Warehouse/GetWarehouse?id="+ $scope.ActiveQualitycheck.ProductLocation.District.Warehouse.Id)
		.then(
			function successCallback(response) {				
		        $scope.activeWarehouse = JSON.parse(response.data);
		        console.log($scope.activeWarehouse);
		        console.log("ho222i");
		    }, 
		    function errorCallback(response) {
		    	alert("Geen verbinding met de API");
		    	console.log(response);
		    }
	    );
	}

	$scope.getNumber = function(num) {
	    return new Array(num);   
	}

	$scope.getActiveProduct = function(District,column) {
	    var products = [];
	    for (var i = 0; i < District.ProductLocations.length; i++) {
	    	if(District.ProductLocations[i].Column == column && $scope.ActiveQualitycheck.ProductLocation.Id ==District.ProductLocations[i].Id ){
	    		products.push(District.ProductLocations[i]);
	    	}
	    }
	    return products;
	}
	
	$scope.CancelQualityCheck = function() {
		$scope.ActiveQualitycheck.Status = "Done";
		$http.put("http://localhost:62553/api/QualityCheck/PutQualityCheck", $scope.ActiveQualitycheck)
		.then(
			function successCallback(response) {				
		        $location.path("qualitycheck");
		    }, 
		    function errorCallback(response) {
		    	alert("Geen verbinding met de API");		 

		    }
	    );
	}

	$scope.DrawLines = function() {
		var c=document.getElementById("hiddencanvas");

		var ctx=c.getContext("2d");
		ctx.beginPath();
		ctx.moveTo(0,0);
		ctx.lineTo(100,100);
		ctx.stroke();

	}

	$scope.GetActiveQualitycheckState();
	setInterval(function(){
		$scope.GetActiveQualitycheckState();
	}, 1000);

});

