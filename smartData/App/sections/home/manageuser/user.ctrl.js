'use strict';
angular
    .module('app.core')
    .controller('UserController', ['$scope', '$rootScope', 'UserService', 'storage', '$location', 'NgTableParams', function ($scope, $rootScope, UserService, storage, $location, NgTableParams) {
    
        $scope.validateUser = function () {
            UserService.getallUser($scope.FirstName, $scope.LastName, $scope.Email)
            .then(function (data) {
                $scope.alluser = data;
                console.log($scope.alluser)
                $scope.customConfigParams = createUsingFullOptions();
            });
        }
        $scope.validateUser();


        function createUsingFullOptions() {
            var initialParams = {
                count: 5 // initial page size
            };
            var initialSettings = {
                // page size buttons (right set of buttons in demo)
                counts: [],
                // determines the pager buttons (left set of buttons in demo)
                paginationMaxBlocks: 13,
                paginationMinBlocks: 2,
                dataset: $scope.alluser
            };
            console.log($scope.alluser);
            return new NgTableParams(initialParams, initialSettings);
        }



    }]);