'use strict';
angular
    .module('app.core')
    .controller('NavController', ['$scope', '$rootScope', function ($scope, $rootScope) {
        $scope.hasLoggedIn = angular.isDefined($rootScope.loginId) && $rootScope.loginId != null;
    }]);