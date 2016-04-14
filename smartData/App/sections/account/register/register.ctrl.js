﻿'use strict';
angular
    .module('app.core').directive('checkStrength', function () {
        return {
            replace: false,
            restrict: 'EACM',
            link: function (scope, iElement, iAttrs, ctrl) {

                var strength = {
                    colors: ['#F00', '#F90', '#FF0', '#9F0', '#0F0'],
                    mesureStrength: function (p) {
                        var _force = 0;
                        var _regex = /[$-/:-?{-~!"^_`\[\]]/g;
                        var _lowerLetters = /[a-z]+/.test(p);
                        var _upperLetters = /[A-Z]+/.test(p);
                        var _numbers = /[0-9]+/.test(p);
                        var _symbols = _regex.test(p);
                        var _flags = [_lowerLetters, _upperLetters, _numbers, _symbols];
                        var _passedMatches = $.grep(_flags, function (el) { return el === true; }).length;
                        _force += 2 * p.length + ((p.length >= 10) ? 1 : 0);
                        _force += _passedMatches * 10;
                        // penality (short password)
                        _force = (p.length <= 6) ? Math.min(_force, 10) : _force;
                        // penality (poor variety of characters)
                        _force = (_passedMatches == 1) ? Math.min(_force, 10) : _force;
                        _force = (_passedMatches == 2) ? Math.min(_force, 20) : _force;
                        _force = (_passedMatches == 3) ? Math.min(_force, 40) : _force;

                        return _force;

                    },
                    getColor: function (s) {
                        scope.PasswordValidationError = "";
                        var idx = 0;
                        if (s <= 10) { idx = 0; scope.PasswordValidationError = "Error"; }
                        else if (s <= 20) { idx = 1; }
                        else if (s <= 30) { idx = 2; }
                        else if (s <= 40) { idx = 3; }
                        else { idx = 4; }

                        return { idx: idx + 1, col: this.colors[idx] };

                    }
                };

                scope.$watch(iAttrs.checkStrength, function () {
                    //alert(scope.Password);
                    if (scope.Password === undefined || scope.Password === '') {
                        scope.PasswordValidationError = '';
                        iElement.css({ "display": "none" });
                    } else {
                        var c = strength.getColor(strength.mesureStrength(scope.Password));
                        iElement.css({ "display": "inline" });
                        iElement.children('li')
                            .css({ "background": "#DDD" })
                            .slice(0, c.idx)
                            .css({ "background": c.col });
                    }
                });

            },
            template: '<li class="point"></li><li class="point"></li><li class="point"></li><li class="point"></li><li class="point"></li>'
        };
    })
    .controller('RegisterController', ['$scope', '$rootScope', 'AccountService', function ($scope, $rootScope, AccountService) {
        $scope.ErrorMessage = "";        
        $scope.RegisterUser = function () {
            $scope.ErrorMessage = "";
            if ($scope.PasswordValidationError != '') {
                $scope.ErrorMessage = "Password is incorrect.";
                return;
            }
            if ($scope.Password != $scope.ConfirmPassword) {
                $scope.ErrorMessage = "Password and Confirm Password doesnot Match.";
                return;
            }
            
            AccountService.registeruser($scope.Email, $scope.Password)
            .then(function (data) {
                $scope.ErrorMessage = data;
            });
        }
    }]);
