'use strict';
angular
    .module('app.core')
    .controller('AppController', ['$scope', '$location', 'storage', function ($scope, $location, storage) {
        console.log("reached app");
        console.log($location.path());
        if (storage.getItem('user-token') !== undefined && storage.getItem('user-token') !== ""&& storage.getItem('user-token') !== null) $location.path('/home');
        else $location.path('/login');
        console.log(storage.getItem('user-token') !== undefined && storage.getItem('user-token') !== "");
    }]);