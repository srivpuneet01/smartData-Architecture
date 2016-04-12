'use strict';

angular
    .module('app.routes', ['ngRoute'])
    .config(config);

function config ($routeProvider) {
    $routeProvider.
        when('/', {
            templateUrl: 'app/sections/account/login/login.tpl.html',
            controller: 'LoginController as login'
        })
        .when('/login', {
            templateUrl: 'app/sections/account/login/login.tpl.html',
            controller: 'LoginController as login'
        })
        .when('/register', {
            templateUrl: 'app/sections/account/register/register.tpl.html',
            controller: 'RegisterController as reg'
        })
        .otherwise({
            redirectTo: '/'
        });
}