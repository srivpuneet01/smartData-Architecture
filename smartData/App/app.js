'use strict';
angular.module('app', ['ngRoute', 'ngAnimate', 'angularMoment', 'angular-preload-image', 'truncate', 'app.routes', 'app.core', 'app.services', 'app.helpers', 'app.config', 'ngTable'])
    .run(['$rootScope', 'storage', '$location', function ($rootScope, storage, $location) {
        $rootScope.$on('$routeChangeStart', function (evt) {
            if ($location.path() !== '/login') {
                if (!storage.isloggedIn()) {
                    console.log('DENY');
                    evt.preventDefault();
                    $location.path('/login');
                }
            }
        });

        $rootScope.test = 'test';
        console.log(angular.toJson(storage.getAllItems()));
        console.log(angular.toJson(storage.getItem('user')));
    }]);

