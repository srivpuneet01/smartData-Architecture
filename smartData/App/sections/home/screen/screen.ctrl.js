'use strict';
angular
    .module('app.core')
    .controller('screenController', ['$scope', '$rootScope', 'ScreenService', 'storage', '$location', 'NgTableParams', '$route', '$modal',
        function ($scope, $rootScope, ScreenService, storage, $location, NgTableParams, $route, $modal, $dialog) {
            $rootScope.headerText = "Manage Screen";
            $scope.getscreens = function () {
                ScreenService.getscreen($scope.screenname)
                .then(function (data) {
                    $scope.roledata = data.data;
                    console.log($scope.roledata);
                    $scope.customConfigParams = createUsingFullOptions();

                });
            }


            $scope.getscreens();
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

            $scope.openmodel = function (title, id, screenname, moduleid) {
                var modalInstance = $modal.open({
                    templateUrl: 'app/sections/home/screen/addscreen.tpl.html',
                    controller: 'addScreenController',
                    resolve: {
                        roleparam: function () {
                            return { id: id, title: title, name: screenname, ModuleId: moduleid };
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
            $scope.deletescreen = function (id) {

                $rootScope.SucessMessage = "";
                $rootScope.ErrorMessage = "";
                ScreenService.deletescreen(id)
                .then(function (data) {
                    $scope.getscreens();
                    if (data.data.ResponseStatus === true) {
                        $rootScope.SucessMessage = data.data.ResponseText;
                    } else { $rootScope.ErrorMessage == data.data.ResponseText; }
                });
            }

            $scope.openActionModel = function (screenname, screenid) {
                var modalInstance = $modal.open({
                    templateUrl: 'app/sections/home/screen/addscreenaction.tpl.html',
                    controller: 'addScreenActionController',
                    resolve: {
                        roleparam: function () {
                            return { id: screenid, name: screenname };
                        }
                    }
                });

            }


        }]);