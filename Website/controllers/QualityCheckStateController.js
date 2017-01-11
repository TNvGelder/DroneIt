app.controller("QualityCheckStateController", function ($scope, $http, $location) {
	// contains all data needed for this page
	$scope.ActiveQualitycheck = null;

	$scope.GetActiveQualitycheck = function() {
		$http.get("http://localhost:62553/api/QualityCheck/GetQualityCheck")
		.then(
			function successCallback(response) {				
		        console.log(response);	
		        if(response.data == "null"){
					$location.path("qualitycheck");
		        }else{
		       	 	$scope.ActiveQualitycheck = JSON.parse(response.data);	        	
		        }
		    }, 
		    function errorCallback(response) {
		    	alert("Geen verbinding met de API");
		    	console.log(response);
		    }
	    );
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

	$scope.GetActiveQualitycheck();
	setInterval(function(){
		$scope.GetActiveQualitycheck();
	}, 1000);
});

