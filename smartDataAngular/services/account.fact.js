'use strict';
angular
    .module('app.services')
    .constant('API_KEY', '87de9079e74c828116acce677f6f255b')
    .constant('BASE_URL', 'http://localhost:55038/')
    .factory('AccountService', ['$http', 'BASE_URL', '$log', function ($http, BASE_URL, $log) {
        function makeRequest(url, type, params) {
            var requestUrl = BASE_URL + '/' + url;
            //angular.forEach(params, function (value, key) {
            //    requestUrl = requestUrl + '&' + key + '=' + value;
            //});
            return $http({
                'url': requestUrl,
                'method': type,
                'data': JSON.stringify(params),
                'headers': {
                    'Content-Type': 'application/json'
                },
                'cache': true
            }).then(function (response) {
                return response.data;
            }).catch(dataServiceError);
        }
        function dataServiceError(errorResponse) {
            $log.error('XHR Failed for ShowService');
            $log.error(errorResponse);
            return errorResponse;
        }

        return {
            'authenticate': function (user, pwd) {
                return makeRequest('Account/Login', 'POST', { UserName: user, Password: pwd })
                .then(function (data) { return data.results});
            }
        };
    }]);