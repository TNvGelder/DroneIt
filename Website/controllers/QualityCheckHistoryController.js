app.controller("QualityCheckHistoryController", function ($scope, $http, $location) {
	$scope.QualityChecks = null;
	$scope.selectedqualitycheck = null;

	$scope.SelectQualitycheck = function(QualityCheck) {
		$scope.selectedqualitycheck = QualityCheck;
	}

	$scope.GetQualitychecks = function() {
		$http.get("http://localhost:62553/api/QualityCheck/GetQualityChecks")
		.then(
			function successCallback(response) {	
		        $scope.QualityChecks = JSON.parse(response.data);
		        console.log($scope.QualityChecks);
		    }, 
		    function errorCallback(response) {
		    	alert("Geen verbinding met de API");
		    	console.log(response);
		    }
	    );
	}

	$scope.GetQualitychecks();
});