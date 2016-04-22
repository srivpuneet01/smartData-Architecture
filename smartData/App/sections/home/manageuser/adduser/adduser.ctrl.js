'use strict';
angular
 .module('app.core')
.controller('AddUserController', ['$scope', '$rootScope', 'UserService', '$location', '$routeParams', function ($scope, $rootScope, UserService, $location, $routeParams) {
    $rootScope.headerText = "Add User";
    $scope.ErrorMessage = "";
    $scope.array = [];
    $scope.RegisterUser = function () {
        $scope.ErrorMessage = "";
        if ($scope.PasswordValidationError != '') {
            $scope.ErrorMessage = "Password is incorrect.";
            return;
        }
        if ($scope.Password != $scope.ConfirmPassword) {
            $scope.ErrorMessage = "Password and Confirm Password doesnot Match.";
            return;
        }

        UserService.adduser($scope.firstname, $scope.lastname, $scope.Email, $scope.Password, $scope.example14model)
            .then(function (data) {
                //console.log(angular.toJson(data));
                //storage.setItem('user', angular.toJson(data));                
                $location.path('/user');
            });


    }
    UserService.getuserrole()
    .then(function (data) {
        $scope.roles = data;
        //console.log(data);
        var log = [];
        angular.forEach(data, function (value, key) {
            //this.push(key + ': ' + value.RoleName);
            log.push({ label: value.RoleName, id: value.RoleId })
        }, log);

        console.log(log);
        $scope.example14data = log;
    });
    $scope.example14model = [];
    $scope.example14settings = {
        scrollableHeight: '200px',
        scrollable: true,
        enableSearch: true
    }

    $scope.example2settings = {
        displayProp: 'id'
    };

}]);
