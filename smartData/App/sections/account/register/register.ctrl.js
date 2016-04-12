'use strict';
angular
    .module('app.core')
    .controller('RegisterController', ['$scope', '$rootScope', 'AccountService', function ($scope, $rootScope, AccountService) {
        $scope.ErrorMessage = "";
        //$scope.user.Email = "Dev@test.in";
        //$scope.user.Password = "123456";
        //$scope.user.ConfirmPassword = "123456";
        $scope.RegisterUser = function () {
            $scope.ErrorMessage = "";
            if ($scope.user.Password != $scope.user.ConfirmPassword) {
                $scope.ErrorMessage = "Password and Confirm Password doesnot Match.";
                return;
            }
            AccountService.registeruser($scope.user.Email, $scope.user.Password)
            .then(function (data) {
                alert("Logged in Successfully");
            });
        }
    }]);
