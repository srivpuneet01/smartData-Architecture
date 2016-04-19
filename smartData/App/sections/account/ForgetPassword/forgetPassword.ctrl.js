'use strict';
angular
    .module('app.core')
    .controller('ForgetPasswordController', ['$scope', '$rootScope', 'AccountService', 'storage', '$location', function ($scope, $rootScope, AccountService, storage, $location) {
       
        $scope.forgetPassword = function () {
            AccountService.forgetpassword($scope.password.oldPassword, $scope.password.newPassword, $scope.password.confNewPassword)
            .then(function (data) {
                console.log(angular.toJson(data));
                $location.path('/home');
            });
        }
       
    }]);