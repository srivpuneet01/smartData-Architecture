'use strict';

angular
    .module('app.routes', ['ngRoute'])
    .config(config);

function config ($routeProvider) {
    $routeProvider.
        when('/', {
            templateUrl: 'app/helpers/app.tpl.html',
            controller: 'AppController as app'
        })
        .when('/home', {
            templateUrl: 'app/sections/home/user-graph.tpl.html',
            controller: 'HomeController as login'
        })
        .when('/login', {
            templateUrl: 'app/sections/account/login/login.tpl.html',
            controller: 'LoginController as login'
        })
        .when('/register', {
            templateUrl: 'app/sections/account/register/register.tpl.html',
            controller: 'RegisterController as reg'
        })
        .when('/user', {
            templateUrl: 'app/sections/user/user.tpl.html',
            controller: 'UserController as user'
        })
        .otherwise({
            redirectTo: '/'
        });
}