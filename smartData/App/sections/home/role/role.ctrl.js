'use strict';
angular
    .module('app.core')
    .controller('RoleController', ['$scope', '$rootScope', 'RoleService', 'storage', '$location', 'NgTableParams', '$route', '$modal',
        function ($scope, $rootScope, RoleService, storage, $location, NgTableParams, $route, $modal) {
            $rootScope.headerText = "Manage Role";
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

            $scope.openmodel = function (title, id, rolename, status) {
                var modalInstance = $modal.open({
                    templateUrl: 'app/sections/home/role/addrole.tpl.html',
                    controller: 'addrolecontroller',
                    resolve: {
                        roleparam: function () {
                            return { id: id, title: title, name: rolename, rolestatus: status };
                        }
                        //,
                        //id: function () {
                        //    return id;
                        //}, rolename: function () {
                        //    return rolename;
                        //}
                    }
                });
            }
            $scope.deleterole = function (id) {
                $rootScope.SucessMessage = "";
                $rootScope.ErrorMessage = "";
                RoleService.deleterole(id)
                .then(function (data) {
                    $scope.getrole();
                    console.log(angular.toJson(data.data));
                    if (data.data.ResponseStatus === true) {
                        $rootScope.SucessMessage = data.data.ResponseText;
                    } else { $rootScope.ErrorMessage == data.data.ResponseText; }
                });
            }


        }]);