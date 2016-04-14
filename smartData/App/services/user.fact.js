'use strict';
angular
    .module('app.services')
    .constant('API_KEY', '87de9079e74c828116acce677f6f255b')
    .constant('BASE_URL', 'http://localhost:55038/api')
    .factory('UserService', ['$http', 'BASE_URL', '$log', function ($http, BASE_URL, $log) {
        function makePostRequest(url, params) {
            var requestUrl = BASE_URL + '/' + url;
            return $http.post(requestUrl, params)
                  .then(dataResponse, dataServiceError);

        }

        function makeGetRequest(url, params) {
            var requestUrl = BASE_URL + '/' + url;
            return $http.get(requestUrl, params)
                  .then(dataResponse, dataServiceError);

        }
        function dataResponse(response) { return response.data; }
        function dataServiceError(errorResponse) {
            //$log.error('XHR Failed for AccountService');
            if (errorResponse.status == 404) {
                $log.error(errorResponse);
            }

            return errorResponse;
        }

        return {
            'getallUser': function (firstname, lastname, email) {
                return makePostRequest('UserAPI/GetAllUser', { FirstName: firstname, LastName: lastname, Email: email }).then(function (data) { return data });
            }
            ,
            'getuserrole': function () {
                return makeGetRequest('AccountAPI/GetUserRole').then(function (data) { return data });
            }
        };

    }]);