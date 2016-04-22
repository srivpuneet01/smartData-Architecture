'use strict';
angular
    .module('app.services')
    .constant('API_KEY', '87de9079e74c828116acce677f6f255b')
    .constant('BASE_URL', 'http://localhost:55038/api')
    .factory('ScreenService', ['$http', 'BASE_URL', '$log', function ($http, BASE_URL, $log) {
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
            'getmodule': function (module) {
                return makePOSTRequest('ModuleAPI/GetModule/', { ModuleName: module }).then(function (data) {
                    return data
                });
            }, 'getscreen': function (module) {
                return makePOSTRequest('ScreenAPI/GetScreen/', { screenName: module }).then(function (data) {
                    return data
                });
            }
            , 'insertscreen': function (moduleName, moduleid) {
                return makePOSTRequest('ScreenAPI/CreateScreen/', { ScreenName: moduleName, ModuleId: moduleid }).then(function (data) {
                    return data
                });
            }, 'deletescreen': function (id) {
                return makeGetRequest('ScreenAPI/Deletescreen/' + id).then(function (data) {
                    return data
                });
            }
            , 'editscreen': function (moduleName, moduleid, id) {
                return makePOSTRequest('ScreenAPI/EditScreen/', { ScreenName: moduleName, ModuleID: moduleid, screenId: id }).then(function (data) {
                    return data
                });
            }
             , 'getAction': function () {
                 return makeGetRequest('ScreenAPI/GetAction/').then(function (data) {
                     return data
                 });
             }
               , 'addScreenAction': function (actionName, id, actionType) {
                   return makePOSTRequest('ScreenAPI/AddScreenAction/', { ActionName: actionName, FK_screenId: id, ActionType: actionType }).then(function (data) {
                       return data
                   });
               }
        };


    }]);