'use strict';
angular
    .module('app.core')
    .controller('NavController', ['$scope', '$rootScope', 'storage', function ($scope, $rootScope, storage) {

        $scope.headerText = "Login";
        $scope.hasLoggedIn = angular.isDefined(storage.getItem('user-token')) && storage.getItem('user-token') !== "" && storage.getItem('user-token') !== null;

    }]);