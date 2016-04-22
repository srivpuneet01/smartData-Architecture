'use strict';
angular
    .module('app.core')
    .controller('addScreenActionController', ['$scope', '$rootScope', 'ScreenService', 'storage', '$location', 'NgTableParams', '$route', '$modal', '$modalInstance', 'roleparam',
        function ($scope, $rootScope, ScreenService, storage, $location, NgTableParams, $route, $modal, $modalInstance, roleparam) {
            $rootScope.SucessMessage = "";
            $rootScope.ErrorMessage = "";
            $scope.close = function () {
                $modalInstance.dismiss('cancel');
            };
            $scope.title = roleparam.name + " " + "Action";
            $scope.screenid = roleparam.id;

            $scope.getActions = function () {
                ScreenService.getAction()
                .then(function (data) {
                    $scope.actiondata = data.data;
                });
            }

            $scope.getActions();


            $scope.insertscreenaction = function () {
                $rootScope.SucessMessage = "";
                $rootScope.ErrorMessage = "";
                console.log("Error");
                ScreenService.addScreenAction($scope.actionname, roleparam.id, $scope.actionid)
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


        }]);