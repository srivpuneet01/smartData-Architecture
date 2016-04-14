'use strict';
angular
    .module('app.core')
    .controller('LoginController', ['$scope', '$rootScope', 'AccountService', function ($scope, $rootScope, AccountService) {
        $scope.validateUser = function ()
        {
            AccountService.authenticate($scope.user.username, $scope.user.password)
            .then(function (data) {
                $rootScope.User = data.data;
                console.log(data.data);
            });
        }
        if ($rootScope.rememberMe)
        {

        }
    }]);