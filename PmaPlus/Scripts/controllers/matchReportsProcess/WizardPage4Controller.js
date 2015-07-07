﻿var app = angular.module('MainApp');

app.controller('WizardPage4Controller', ['$scope', '$http', '$q', '$location', '$rootScope', 'toaster', 'WizardHandler', function ($scope, $http, $q, $location, $rootScope, toaster, WizardHandler) {

    var pathArray = $location.$$absUrl.split("/");
    $scope.currId = pathArray[pathArray.length - 1];

    
    
    var confInvitet = angular.element('#confInvitet');
    $scope.playerAdd = {};

    $scope.openEdit1 = function (player) {     
        $scope.player = player;
        $scope.modalTitle = "Edit";
        confInvitet.modal('show');
    };

    $scope.closeDetails = function () {
        confInvitet.modal('hide');
    };

    $scope.getTable = function () {
    $http.get('/api/PlayerMatchStatistic/' + $scope.currId).success(function (result) {
        $scope.playersStat = result;       
    });
    };


    $scope.$on('moveEvent', function () {
        if (WizardHandler.wizard().currentStepNumber() == 4) {
            $scope.getTable();
            $scope.nav.canNext = true;
            $scope.nav.canBack = true;
            $scope.nav.last = false;
        }
    });

    //ADD==========================================
    $scope.addPlayerStat = function () {
        
        $scope.loginLoading = true;
        $scope.myform.form_Submitted = !$scope.myform.$valid;    
        $scope.loginLoading = false;
        if ($scope.myform.$valid) {
            $http.post('/api/PlayerMatchStatistic/', $scope.player).success(function () {

                $scope.playerAdd = {};
                confInvitet.modal('hide');
            }).error(function (data, status, headers, config) {
                if (status == 400) {
                    console.log(data);
                    toaster.pop({
                        type: 'error',
                        title: 'Error', bodyOutputType: 'trustedHtml',
                        body: 'Please comptite compulsory fields'
                    });
                }
            });
        } else {       
            toaster.pop({
            type: 'error',
            title: 'Error', bodyOutputType: 'trustedHtml',
            body: 'Please comptite compulsory fields'
        });
        
        }
        
        
    };
    //ADD=========================================
}]);