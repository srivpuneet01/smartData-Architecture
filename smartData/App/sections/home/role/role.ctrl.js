'use strict';
angular
    .module('app.core')
    .controller('RoleController', ['$scope', '$rootScope', 'RoleService', 'storage', '$location', 'NgTableParams', '$route', '$modal',
        function ($scope, $rootScope, RoleService, storage, $location, NgTableParams, $route, $modal) {
            $scope.rolename = "";
            $scope.getrole = function () {
                RoleService.getrole($scope.rolename)
                .then(function (data) {
                    $scope.roledata = data.data;
                    console.log($scope.roledata);
                    $scope.customConfigParams = createUsingFullOptions();

                });
            }
            $scope.getrole();
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
                    dataset: $scope.roledata
                };

                return new NgTableParams(initialParams, initialSettings);
            }

            $scope.openmodel = function () {
                var modalInstance = $modal.open({
                    templateUrl: 'app/sections/home/role/addrole.tpl.html',
                    controller: 'addrolecontroller',
                });
            }

        }]);