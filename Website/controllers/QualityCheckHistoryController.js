app.controller("QualityCheckHistoryController", function ($scope, $http, $location) {
	$scope.QualityChecks = [];
	$scope.selectedqualitycheck = null;
	$scope.maxpage = $scope.QualityChecks.length;
	$scope.PageSize = 10;
	$scope.currentpage = 0;

	$scope.SelectQualitycheck = function(QualityCheck) {
		$scope.selectedqualitycheck = QualityCheck;
		console.log(QualityCheck);
	}

	$scope.SetPage = function(num) {
		$scope.currentpage = num;
	}

	$scope.getPaginationNumber = function(num) {
		var s = Math.ceil( num / $scope.PageSize);
	    return new Array(s);   
	}

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