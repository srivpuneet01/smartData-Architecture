﻿'use strict';
(function (ng) {
    ng.module('app.core')
        .controller('HomeController', ['$scope', '$rootScope', function ($scope, $rootScope) {
            $rootScope.headerText = "Dashboard";
        }]);
})(angular);