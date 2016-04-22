/// <reference path="sections/home/role/role.tpl.html" />
/// <reference path="sections/home/role/role.tpl.html" />
'use strict';

angular
    .module('app.routes', ['ngRoute'])
    .config(config);

function config($routeProvider) {
    $routeProvider.
        when('/', {
            //templateUrl: 'app/helpers/app.tpl.html',
            //controller: 'AppController as app'
            templateUrl: 'app/sections/account/login/login.tpl.html',
            controller: 'LoginController as login'
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
            templateUrl: 'app/sections/home/manageuser/user.tpl.html',
            controller: 'UserController as user'
        })
        .when('/adduser', {
            templateUrl: 'app/sections/home/manageuser/adduser/adduser.tpl.html',
            controller: 'AddUserController as Adduser'
        })
        .when('/edituser/:id', {
            templateUrl: 'app/sections/home/manageuser/adduser/edituser.tpl.html',
            controller: 'EditUserController as Edituser'
        })
        .when('/forgetpassword', {
            templateUrl: 'app/sections/account/ForgetPassword/forgetPassword.tpl.html',
            controller: 'ForgetPasswordController as forgetPassword'
        })
         .when('/changepassword', {
             templateUrl: 'app/sections/account/ChangePassword/changePassword.tpl.html',
             controller: 'ChangePasswordController as changepassword'
         })
        .when('/role', {
            templateUrl: 'app/sections/home/role/role.tpl.html',
            controller: 'RoleController as role'
        })
        .when('/module', {
            templateUrl: 'app/sections/home/module/module.tpl.html',
            controller: 'ModuleController as module'
        })
        .when('/screen', {
            templateUrl: 'app/sections/home/screen/screen.tpl.html',
            controller: 'screenController as screen'
        })

        .otherwise({
            redirectTo: '/'
        });
}