'use strict';
angular
    .module('app.core')
    .controller('addrolecontroller', ['$scope', '$rootScope', 'RoleService', 'storage', '$location', 'NgTableParams', '$route', '$modal','$modalInstance',
        function ($scope, $rootScope, RoleService, storage, $location, NgTableParams, $route, $modal,$modalInstance) {
           $scope.close = function () {
                $modalInstance.dismiss('cancel');
            };
        }]);