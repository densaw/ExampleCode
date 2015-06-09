﻿var app = angular.module('MainApp');

app.controller('TalentIdentificationController', ['$scope', '$http', 'toaster', '$q', '$routeParams', '$location', '$rootScope', function ($scope, $http, toaster, $q, $routeParams, $location, $rootScope) {

    var pathArray = $location.$$absUrl.split("/");
    $scope.currId = pathArray[pathArray.length - 1];
    $scope.profileTalents = [];

    function getParentCurr() {
        $http.get('/api/TalentIdentification/Detail/' + $scope.currId).success(function (result) {
            $scope.profileTalents = result;
            console.log($scope.profileTalents);
        });
    }
    var toggleAttendance = angular.element('#toggleAttendance');
    var toggleInvite = angular.element('#toggleInvite');
    var toggleJoined = angular.element('#toggleJoined');
    
    
    //Variable section
    var date = new Date();

    
    var needToDelete = -1;

    var urlTail = '/api/TalentIdentification';
    var target = angular.element('#addScoutP');
    var confDelete = angular.element('#confDelete');
    
    var sortArray = [];
    //get scout
    

    $scope.scouts = [];
    
    $http.get('/api/Scouts/List').success(function(result) {
        $scope.scouts = result;
    });

   //end
   

    $scope.newScoutP = {};

    $scope.curriculumTypesList = [];
       
    
    function createTail(pageNumber) {
        if (sortArray.length > 0) {
            return urlTail + '/' + $scope.itemsPerPage + '/' + pageNumber + '/' + sortArray[0] + '/' + sortArray[1];
        } else {
            return urlTail + '/' + $scope.itemsPerPage + '/' + pageNumber;
        }
    }

    function getResultsPage(pageNumber) {
        $http.get(createTail(pageNumber))
            .success(function (result) {
                console.log(result);
                $scope.items = result.items;
                $scope.totalItems = result.count;
        
            });
    }

    $rootScope.$watchGroup(['orderField', 'revers'], function (newValue, oldValue, scope) {
        sortArray = newValue;
        $http.get(createTail($scope.pagination.current))
            .success(function (result) {
                $scope.items = result.items;
                $scope.totalItems = result.count;
            });
    });

    $scope.items = [];
    $scope.totalItems = 0;
    $scope.itemsPerPage = 20;


    $scope.pagination = {
        current: 1
    };

    getResultsPage($scope.pagination.current);

    $scope.pageChanged = function (newPage) {
        getResultsPage(newPage);
        $scope.pagination.current = newPage;
    };

    getParentCurr();
    $scope.open = function () {
        $scope.modalTitle = 'Add Scouted Player';
        target.modal('show');
    };

    $scope.cancel = function () {
        target.modal('hide');
        confDelete.modal('hide');
        needToDelete = -1;
    };


    $scope.openEdit = function (id) {
        console.log(id);
        $http.get(urlTail + '/' + id)
            .success(function (result) {
                $scope.newScoutP = result;
                $scope.myform.form_Submitted = false;
                $scope.modalTitle = "Update Scouted Player";
                target.modal('show');
            });
    };

    $scope.ok = function (id) {
        $scope.loginLoading = true;
        $scope.myform.form_Submitted = !$scope.myform.$valid;
        if (id != null) {

            //PUT it now have no url to Update date
            $http.put(urlTail + '/' + id, $scope.newScoutP).success(function (result) {
                getResultsPage($scope.pagination.current);
                console.log(id);
                $scope.loginLoading = false;
                $scope.newScoutP = {};
                target.modal('hide');
            }).error(function (data, status, headers, config) {
                $scope.loginLoading = false;
            });
        } else {
            //POST
            $http.post(urlTail, $scope.newScoutP).success(function (result) {
                getResultsPage($scope.pagination.current);
                target.modal('hide');
                $scope.newScoutP = {};
                $scope.loginLoading = false;
            }).error(function (data, status, headers, config) {
                $scope.loginLoading = false;
            });
        }
    };

    $scope.openDelete = function (id) {
        confDelete.modal('show');
        console.log(id);
        needToDelete = id;
    };
    $scope.delete = function () {
        $http.delete(urlTail + '/' + needToDelete).success(function () {
            getResultsPage($scope.pagination.current);
            needToDelete = -1;
            confDelete.modal('hide');
        });
    };

  


    $scope.check = function (playerObj) {
        $http.put(urlTail + '/Invite/' + playerObj.id, !playerObj.isLive).success(function (result) {
            getResultsPage($scope.pagination.current);
        }).error(function (data, status, headers, config) {

        });
    }
    
    function getClubName() {
        $http.get('/api/ClubAdminDashboard/ClubName').success(function (result) {
            $scope.clubName = result;
        }).error(function (data, status, headers, config) {

        });
    }
    getClubName();
    

}]);