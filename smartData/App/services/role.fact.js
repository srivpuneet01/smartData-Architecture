'use strict';
angular
    .module('app.services')
    .constant('API_KEY', '87de9079e74c828116acce677f6f255b')
    .constant('BASE_URL', 'http://localhost:55038/api')
    .factory('RoleService', ['$http', 'BASE_URL', '$log', function ($http, BASE_URL, $log) {
        function makePOSTRequest(url, params) {
            var requestUrl = BASE_URL + '/' + url;

            return $http.post(requestUrl, JSON.stringify(params))
                  .then(dataResponse, dataServiceError);
        }
        function makeGetRequest(url, params) {
            var requestUrl = BASE_URL + '/' + url;
            return $http.get(requestUrl)
                  .then(dataResponse, dataServiceError);

        }
        function dataResponse(response) { return response; }
        function dataServiceError(errorResponse) {
            //$log.error('XHR Failed for AccountService');
            if (errorResponse.status == 404) {
                $log.error(errorResponse);
                //alert("User Not Found");
            }

            return errorResponse;
        }

        return {
            'getrole': function (role) {
                return makePOSTRequest('RoleAPI/GetRole/', { RoleName: role }).then(function (data) {
                    return data
                });
            }
        };


    }]);