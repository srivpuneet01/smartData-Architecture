'use strict';
angular
    .module('app.core')
    .controller('addScreenController', ['$scope', '$rootScope', 'ScreenService', 'storage', '$location', 'NgTableParams', '$route', '$modal', '$modalInstance', 'roleparam',
        function ($scope, $rootScope, ScreenService, storage, $location, NgTableParams, $route, $modal, $modalInstance, roleparam) {
            $rootScope.SucessMessage = "";
            $rootScope.ErrorMessage = "";
            $scope.close = function () {
                $modalInstance.dismiss('cancel');
            };
            $scope.title = roleparam.title;
            console.log(roleparam);

            $scope.moduledata = [];
            if (roleparam.id > 0) {
                $scope.screenname = roleparam.name;
                $scope.moduleid = roleparam.ModuleId;
            }

            $scope.getmodules = function () {
                ScreenService.getmodule(null)
                .then(function (data) {
                    $scope.moduledata = data.data;                    
                });
            }

            $scope.getmodules();

            
            $scope.insertscreen = function () {
                $rootScope.SucessMessage = "";
                $rootScope.ErrorMessage = "";
                if (roleparam.id > 0) {
                    console.log("Edit Start");
                    ScreenService.editscreen($scope.screenname, $scope.moduleid, roleparam.id)
                  .then(function (data) {
                      $scope.roledata = data.data;
                      //console.log($scope.roledata);
                      $modalInstance.dismiss('cancel');
                      $location.path("/screen");
                      $route.reload();
                      if (data.data.ResponseStatus === true) {
                          $rootScope.SucessMessage = data.data.ResponseText;
                      } else {
                          $rootScope.ErrorMessage = data.data.ResponseText;
                      }


                  });
                }
                else {

                    ScreenService.insertscreen($scope.screenname, $scope.moduleid)
                  .then(function (data) {
                      $modalInstance.dismiss('cancel');
                      $location.path("/screen");
                      $route.reload();
                      if (data.data.ResponseStatus === true) {
                          $rootScope.SucessMessage = data.data.ResponseText;
                      } else {
                          $rootScope.ErrorMessage = data.data.ResponseText;
                      }


                  });
                }
            };

        }]);