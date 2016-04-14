'use strict';
angular
    .module('app.core')
    .controller('AppController', ['$scope', '$location', 'storage', function ($scope, $location, storage) {
        console.log("reached app");
        console.log($location.path());
        if (storage.getItem('user') !== undefined && storage.getItem('user') !== ""&& storage.getItem('user') !== null) $location.path('/home');
        else $location.path('/login');
        console.log(storage.getItem('user') !== undefined && storage.getItem('user') !== "");
    }]);