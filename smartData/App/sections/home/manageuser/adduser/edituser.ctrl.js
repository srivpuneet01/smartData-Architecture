'use strict';
angular
 .module('app.core')
.controller('EditUserController', ['$scope', '$rootScope', 'UserService', '$location', '$routeParams', function ($scope, $rootScope, UserService, $location, $routeParams) {
    $scope.ErrorMessage = "";
    $scope.array = [];
$rootScope.headerText = "Edit User";
    Edit();
    function Edit() {
        UserService.getuserbyid($routeParams.id).then(function (data) {
            $scope.firstname = data.FirstName;
            $scope.lastname = data.LastName;
            $scope.Email = data.Email;
            console.log(data.RolesList);
        });
    }
    GetUserRoleByID();
    function GetUserRoleByID()
    {
        UserService.getuserrolebyid($routeParams.id).then(function (data) {
            console.log(data);
            angular.forEach(data, function (key, value) {
                $scope.example14model.push({ "id": key.RoleId })
            });
            
        });
    }

    $scope.UpdateUser = function () {
        $scope.ErrorMessage = "";
        UserService.updateuser($routeParams.id, $scope.firstname, $scope.lastname, $scope.Email, $scope.example14model)
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
