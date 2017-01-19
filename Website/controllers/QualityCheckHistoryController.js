/*
	@Author : Harmen Hilvers
	Controller used for the qualitycheck history page
*/
app.controller("QualityCheckHistoryController", function ($scope, $http, $location) {
	$scope.QualityChecks = [];
	$scope.selectedqualitycheck = null;
	$scope.maxpage = $scope.QualityChecks.length;
	$scope.PageSize = 10;
	$scope.currentpage = 0;

	// Set the current selected qualitycheck
	$scope.SelectQualitycheck = function(QualityCheck) {
		$scope.selectedqualitycheck = QualityCheck;
		console.log(QualityCheck);
	}

	// method used to set the current page of the pagination
	$scope.SetPage = function(num) {
		$scope.currentpage = num;
	}

	// get the current ammount of pages 
	$scope.getPaginationNumber = function(num) {
		var s = Math.ceil( num / $scope.PageSize);
		if(s == 1){
			s=0;
		}
	    return new Array(s);   
	}

	// get all the current finished qualitychecks
	$scope.GetQualitychecks = function() {
		$http.get("http://localhost:62553/api/QualityCheck/GetQualityChecks")
		.then(
			function successCallback(response) {	
		        $scope.QualityChecks = JSON.parse(response.data);
		        console.log($scope.QualityChecks);
		        
				$scope.maxpage = $scope.QualityChecks.length
				console.log($scope.maxpage);
		    }, 
		    function errorCallback(response) {
		    	alert("Geen verbinding met de API");
		    	console.log(response);
		    }
	    );
	}

	$scope.GetQualitychecks();
});