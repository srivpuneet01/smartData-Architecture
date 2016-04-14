'use strict';
angular
    .module('app.core')
    .controller('UserController', ['$scope', '$rootScope', 'UserService', 'NgTableParams', function ($scope, $rootScope, UserService, NgTableParams) {
        $scope.validateUser = function () {
            UserService.getallUser($scope.FirstName, $scope.LastName, $scope.Email)
            .then(function (data) {
                console.log(data.data);
            });
        }
        var self = this;
        var data = [{ name: "Moroni", age: 50 }, { name: "Moroni", age: 50 }];
        self.tableParams = new NgTableParams({}, { dataset: data });
        console.log(data);
    }]);