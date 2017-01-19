/*
	@Author : Harmen Hilvers & Gerhard Kroes
	Controller used for the Qualitycheck state page
*/
app.controller("QualityCheckStateController", function ($scope, $http, $location) {
	$scope.ActiveQualitycheck = null;
	$scope.activeWarehouse= null;
	$scope.loaded= null;
	$scope.currentWarehouseHeight= null;
	$scope.currentWarehouseWidth= null;

	// get the current active quality check, when there is no active quality check. Locate pageto the overviewpage
	$scope.GetActiveQualitycheckState = function() {
		$http.get("http://localhost:62553/api/QualityCheck/GetQualityCheck")
		.then(
			function successCallback(response) {	
		        if(response.data == "null" && response.data == 0){
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

	// method to resize the warehouse canvas to be responsive
	$scope.ResizeCanvas = function() {
		var width = $("#warehousecanvas").width();
  		var w = $scope.activeWarehouse.Width;
  		var h = $scope.activeWarehouse.Height;
  		var re = (100/w) * h;
  		var Height = width / 100 * re;
  		$("#warehousecanvas").height(Height);
  		$("#hiddencanvas").height(Height);	
  		$("#hiddencanvas").width(width);	
  		var canvas = document.getElementById('hiddencanvas');
		canvas.width = width;
		canvas.height = Height;

		$scope.DrawLines();
				
		$scope.loaded= 1;
	}

	// get the current warehouse data from the webapi
	$scope.LoadWarehouse = function() {		
		$http.get("http://localhost:62553/api/Warehouse/GetWarehouse?id="+ $scope.ActiveQualitycheck.ProductLocation.District.Warehouse.Id)
		.then(
			function successCallback(response) {				
		        $scope.activeWarehouse = JSON.parse(response.data);		 
		    }, 
		    function errorCallback(response) {
		    	alert("Geen verbinding met de API");
		    	console.log(response);
		    }
	    );
	}

	// method used to return array of the size of the given number
	$scope.getNumber = function(num) {
	    return new Array(num);   
	}

	// method to get the current active product location 
	$scope.getActiveProduct = function(District,column) {
	    var products = [];
	    for (var i = 0; i < District.ProductLocations.length; i++) {
	    	if(District.ProductLocations[i].Column == column && $scope.ActiveQualitycheck.ProductLocation.Id ==District.ProductLocations[i].Id ){
	    		products.push(District.ProductLocations[i]);
	    	}
	    }
	    return products;
	}
	
	// method to submit the cancilation of the quality check
	$scope.CancelQualityCheck = function() {
		$scope.ActiveQualitycheck.Status = "Done";
		$scope.loaded = 0;
		clearInterval(interval);
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

	// this method is used to calculate the correct positions from the warehouse coordinates to percentages for the view to be responsive
	$scope.calculateCorrectPosition = function(locations) {		
		for (var i = 0; i < locations.length; i++) {
			//flip y coordinates
			locations[i].Y = $scope.activeWarehouse.Height - locations[i].Y;

			// calculate y to percentages
			$scope.currentWarehouseHeight = $("#warehousecanvas").height();
			var heightpercentage = (100/$scope.activeWarehouse.Height ) * $scope.currentWarehouseHeight;
			locations[i].Y = ( heightpercentage/100 )* locations[i].Y;

			// calculate x to percentages
			$scope.currentWarehouseWidth = $("#warehousecanvas").width();
			var widthpercentage = (100/$scope.activeWarehouse.Width ) * $scope.currentWarehouseWidth;
			locations[i].X = ( widthpercentage/100 )* locations[i].X;

		}
		return locations;
	}

	// method to draw the path for the drone to take in the canvas
	$scope.DrawLines = function() {
	    if($scope.ActiveQualitycheck.JSONPath != null){

	    	var locations =  JSON.parse($scope.ActiveQualitycheck.JSONPath);
			var c=document.getElementById("hiddencanvas");	

			// calculate coordinates of product
	    	var DistrictX = $scope.ActiveQualitycheck.ProductLocation.District.X;
	    	var DistrictY = $scope.ActiveQualitycheck.ProductLocation.District.Y;

	    	// get productlocation
	    	var productpoint = $scope.CalculateBearingPoint(DistrictX,DistrictY,$scope.ActiveQualitycheck.ProductLocation.District.Orientation, ( $scope.ActiveQualitycheck.ProductLocation.Column *100)-50);

	    	// get coordinates in front of product location
	    	var frontproductlocation = $scope.CalculateBearingPoint(productpoint.X,productpoint.Y,90-$scope.ActiveQualitycheck.ProductLocation.District.Orientation, 200);
			locations.push(frontproductlocation);	    	
			locations.push(productpoint);
			locations = $scope.calculateCorrectPosition(locations);

			// loop through all locations and draw lines
	    	for (var i = 0; i < locations.length; i++) {	    		
	    		if(locations[i+1] == null) {continue;}	    		
				var ctx=c.getContext("2d");
		
				ctx.beginPath();
				ctx.moveTo(locations[i].X,locations[i].Y);
				ctx.lineTo(locations[i+1].X,locations[i+1].Y);
				console.log("line "+i+" from: X:"+locations[i].X+" Y: "+locations[i].Y +" to X:"+locations[i+1].X+" Y: "+locations[i+1].Y );
				ctx.stroke();
	    	}
	    }
	}

	// method to calculate the bearing point to determine the correct X and Y for the next point
	$scope.CalculateBearingPoint = function(X,Y, bearing, distance){
		point = {X: X, Y: Y};		
		angle =     90 - bearing-90;
		bearing =  bearing * Math.PI / 180;
		angle =    angle * Math.PI / 180;

		dist_x = distance * Math.cos(angle);
		dist_y = distance * Math.sin(angle)

		point.X = point.X + dist_x;
		point.Y =  point.Y + dist_y;

		return point;

	}

	$scope.GetActiveQualitycheckState();

	// set interval to refresh the qualitycheck state every second
	var interval = setInterval(function(){		
		$scope.GetActiveQualitycheckState();
		
	}, 1000);

});

