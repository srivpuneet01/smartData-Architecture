'use strict';
angular
    .module('app.core')
    .controller('LoginController', ['$scope', '$rootScope', 'AccountService', 'storage', '$location', function ($scope, $rootScope, AccountService, storage, $location) {
        storage.removeItem('user');
        console.log(storage.getAllItems());
        $scope.validateUser = function () {
            AccountService.authenticate($scope.user.username, $scope.user.password)
            .then(function (data) {
                $rootScope.hasLoggedIn = true;
                $rootScope.userName = data.Email;
                console.log(angular.toJson(data));
                storage.setItem('user', angular.toJson(data));
                
                $location.path('/home');
            });
        }
        if ($rootScope.rememberMe) {

        }

        $scope.forgetPassword = function ()
        {    
            //$scope.forgetPasswordEmail
            //validate the user with email 
            //afte validationg user Send email to user 
            //Then user will redirect to forget password from email
            angular.element('.modal-backdrop').removeClass('in');
            $location.path('/forgetpassword');
            
        }
    }]);