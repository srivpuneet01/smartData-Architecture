'use strict';
angular.module('app', ['ngRoute', 'ngAnimate', 'angularMoment', 'angular-preload-image', 'truncate', 'app.routes', 'app.core', 'app.services', 'app.helpers', 'app.config'])
    .run(function ($rootScope) {
        $rootScope.test = 'test';
    });

