'use strict';
angular
    .module('app.core')
    .controller('NavController', ['$scope', '$rootScope', 'storage', function ($scope, $rootScope, storage) {
        $scope.hasLoggedIn = angular.isDefined(storage.getItem('user')) && storage.getItem('user') !== "" && storage.getItem('user') !== null;
        $rootScope.userName = $rootScope.userName;
        console.log($scope.hasLoggedIn);
        $scope.hideAlert = function () {
            $rootScope.SucessMessage = "";
            $rootScope.ErrorMessage = "";
        }
    }]);