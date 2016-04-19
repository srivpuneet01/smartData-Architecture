'use strict';
angular
    .module('app.core')
    .controller('ChangePasswordController', ['$scope', '$rootScope', 'AccountService', 'storage', '$location', function ($scope, $rootScope, AccountService, storage, $location) {

        $scope.changePassword = function () {
            AccountService.changepassword($scope.password.oldPassword, $scope.password.newPassword, $scope.password.confNewPassword)
            .then(function (data) {
                console.log(angular.toJson(data));
                $location.path('/home');
            });
        }

    }]);