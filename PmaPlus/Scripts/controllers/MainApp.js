﻿(function () {
    var module = angular.module('MainApp', ['tc.chartjs', 'charts.ng.sparkline.seriesbar']);

    module.controller('ChartController', function ($scope, $http) {
        var monthNames = ['January',
            'February',
            'March',
            'April',
            'May',
            'June',
            'July',
            'August',
            'September',
            'October',
            'November',
            'December'];

        $http.get('api/dashboard/active/players').success(function (data) {
            
            var monthArray = new Array;
            var playerCountArray = new Array;
            data.forEach(function(val) {
                monthArray.push(monthNames[val.month-1]);
                playerCountArray.push(val.activePlayers);
            });

            $scope.data = {
                labels: monthArray,
                datasets: [
                    {
                        label: "Example dataset",
                        fillColor: "rgba(26,179,148,0.5)",
                        strokeColor: "rgba(26,179,148,0.7)",
                        pointColor: "rgba(26,179,148,1)",
                        pointStrokeColor: "#fff",
                        pointHighlightFill: "#fff",
                        pointHighlightStroke: "rgba(26,179,148,1)",
                        data: playerCountArray
                    }
                ]
            };

            $scope.options = {
                scaleShowGridLines: true,
                scaleGridLineColor: "rgba(0,0,0,.05)",
                scaleGridLineWidth: 1,
                bezierCurve: true,
                bezierCurveTension: 0.4,
                pointDot: true,
                pointDotRadius: 4,
                pointDotStrokeWidth: 1,
                pointHitDetectionRadius: 20,
                datasetStroke: true,
                datasetStrokeWidth: 2,
                datasetFill: true,
                responsive: true
            };


        });
    });
    module.controller('PlayersBoxController', function($scope, $http) {
        $http.get('api/dashboard/logged/players').success(function(data) {
            $scope.amountPlayers = data.amount;
            $scope.progressPlayer = data.progress;
            $scope.percentage = data.percentage;
        });
    });
    module.controller('CoachsBoxController', function($scope, $http) {
        $http.get('api/dashboard/logged/coaches').success(function(data) {
            $scope.amountCoaches = data.amount;
            $scope.progressCoach = data.progress;
            $scope.percentage = data.percentage;
        });
    });
    module.controller('ClubsBoxController', function($scope, $http) {
        $http.get('api/dashboard/logged/clubs').success(function(data) {
            $scope.amountClubs = data.amount;
            $scope.progressClub = data.progress;
            $scope.percentage = data.percentage;
        });
    });
    module.controller('UsersBoxController', function($scope, $http) {
        $http.get('api/dashboard/logged/users').success(function(data) {
            $scope.amountUsers = data.amount;
            $scope.progressUser = data.progress;
            $scope.percentage = data.percentage;
        });
    });
    module.controller('PlayerLoginHistoryController', function($scope, $http) {
        $http.get('api/dashboard/logged/players/10/weeks').success(function (data) {
            $scope.data = {
                //labels: ["January", "February", "March", "April", "May", "June", "July"],
                labels: ["", "", "", "", "", "", ""],
                //labels: new Array(7),
                datasets: [
                    {
                        label: "My First dataset",
                        fillColor: "rgba(26,179,148,1)",
                        strokeColor: "rgba(26,179,148,1)",
                        highlightFill: "#fff",
                        highlightStroke: "#fff",
                        data: [65, 59, 80, 81, 56, 55, 40]
                    }
                ]
            };
            $scope.options = {
                scaleBeginAtZero: true,
                scaleShowGridLines: false,
                scaleGridLineColor: "rgba(0,0,0,.00)",
                scaleGridLineWidth: 1,
                scaleShowHorizontalLines: false,
                scaleShowVerticalLines: false,
                barShowStroke: false,
                barStrokeWidth: 2,
                barValueSpacing: 5,
                barDatasetSpacing: 1,
                showXLabels: 2
            };

        });
    });
    module.controller('AllPlayerController', function($scope, $http) {
        $http.get('api/dashboard/active/players/all').success(function (data) {
            $scope.playerCount = data;
        });
    });

    
})();