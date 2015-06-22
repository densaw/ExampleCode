﻿var app = angular.module('MainApp');

app.controller('CurrReportPeriodController', ['$scope', '$http', '$location', 'WizardHandler', function ($scope, $http, $location, WizardHandler) {

    var pathArray = $location.$$absUrl.split("/");
    $scope.currId = pathArray[pathArray.length - 1];

    $http.get('/api/CurriculumStatement/List').success(function (data) {
        $scope.statemants = data;
    });


    $http.get('/api/Curriculum/Wizard/Session/BlockObjectiveTable/' + $scope.currId + '/' + $scope.$parent.step.sessionId)
        .success(function (result) {
            $scope.items = result;

        });
    var savePeriod = function () {
        $http.post('/api/Curriculum/Wizard/Session/BlockObjectiveTable/' + $scope.currId + '/' + $scope.$parent.step.sessionId, $scope.items)
            .success(function () {

            });

    };

    $scope.$on('saveProgressEvent', function () {
        if (WizardHandler.wizard().currentStepNumber() == $scope.$parent.steps.indexOf($scope.$parent.step) + 1) {

            console.log("save objectives");
            savePeriod();

        }
    });

    $scope.addObjective = function (player) {
        angular.element('#objModal').appendTo("body").modal('show');
        $scope.player = player;

    }



    $scope.saveObjective = function () {
        angular.element('#objModal').modal('hide');
    }

}]);