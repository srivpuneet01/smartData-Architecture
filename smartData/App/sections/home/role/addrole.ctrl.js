'use strict';
angular
    .module('app.core')
    .controller('addrolecontroller', ['$scope', '$rootScope', 'RoleService', 'storage', '$location', 'NgTableParams', '$route', '$modal', '$modalInstance', 'roleparam',
        function ($scope, $rootScope, RoleService, storage, $location, NgTableParams, $route, $modal, $modalInstance, roleparam) {
            $rootScope.SucessMessage = "";
            $rootScope.ErrorMessage = "";
            $scope.close = function () {
                $modalInstance.dismiss('cancel');
            };
            $scope.title = roleparam.title;
            console.log(roleparam);
            if (roleparam.id > 0) {
                $scope.rolename = roleparam.name;
                $scope.rolestatus = roleparam.rolestatus;
            }

            $scope.insertrole = function () {
                $rootScope.SucessMessage = "";
                $rootScope.ErrorMessage = "";
                if (roleparam.id > 0) {
                    console.log("Edit Start");
                    RoleService.editrole($scope.rolename, $scope.rolestatus, roleparam.id)
                  .then(function (data) {
                      $scope.roledata = data.data;
                      //console.log($scope.roledata);
                      $modalInstance.dismiss('cancel');
                      $location.path("/role");
                      $route.reload();
                      if (data.data.ResponseStatus === true) {
                          $rootScope.SucessMessage = data.data.ResponseText;
                      } else {
                          $rootScope.ErrorMessage = data.data.ResponseText;
                      }


                  });
                }
                else {
                    console.log($scope.rolestatus);
                    RoleService.insertrole($scope.rolename, $scope.rolestatus)
                  .then(function (data) {
                      $scope.roledata = data.data;
                      //console.log($scope.roledata);
                      $modalInstance.dismiss('cancel');
                      $location.path("/role");
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