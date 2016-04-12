'use strict';

angular
    .module('app.routes', ['ngRoute'])
    .config(config);

function config ($routeProvider) {
    $routeProvider.
        when('/', {
            templateUrl: 'sections/account/login.tpl.html',
            controller: 'AccountController as acc'
        })
        .when('/login', {
            templateUrl: 'sections/account/login.tpl.html',
            controller: 'AccountController as acc'
        })
        .otherwise({
            redirectTo: '/'
        });
}