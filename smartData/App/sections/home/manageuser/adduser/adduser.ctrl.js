'use strict';
angular
 .module('app.core')
.controller('AddUserController', ['$scope', '$rootScope', 'UserService', function ($scope, $rootScope, UserService) {
    $scope.ErrorMessage = "";
    $scope.array = [];
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
        
    }
    UserService.getuserrole()
    .then(function (data) {
        $scope.roles = data;
        console.log(data);
    });


    $scope.example14model = [];
    $scope.example14settings = {
        scrollableHeight: '200px',
        scrollable: true,
        enableSearch: true
    }
    $scope.example14data = [{
        "label": "Alabama",
        "id": "AL",
        "desc": "xyx"
    }, {
        "label": "Alaska",
        "id": "AK"
        ,
        "desc": "xyx"
    }, {
        "label": "American Samoa",
        "id": "AS"
    }, {
        "label": "Arizona",
        "id": "AZ"
    }, {
        "label": "Arkansas",
        "id": "AR"
    }, {
        "label": "California",
        "id": "CA"
    }, {
        "label": "Colorado",
        "id": "CO"
    }, {
        "label": "Connecticut",
        "id": "CT"
    }, {
        "label": "Delaware",
        "id": "DE"
    }, {
        "label": "District Of Columbia",
        "id": "DC"
    }, {
        "label": "Federated States Of Micronesia",
        "id": "FM"
    }, {
        "label": "Florida",
        "id": "FL"
    }, {
        "label": "Georgia",
        "id": "GA"
    }, {
        "label": "Guam",
        "id": "GU"
    }, {
        "label": "Hawaii",
        "id": "HI"
    }, {
        "label": "Idaho",
        "id": "ID"
    }, {
        "label": "Illinois",
        "id": "IL"
    }, {
        "label": "Indiana",
        "id": "IN"
    }, {
        "label": "Iowa",
        "id": "IA"
    }, {
        "label": "Kansas",
        "id": "KS"
    }, {
        "label": "Kentucky",
        "id": "KY"
    }, {
        "label": "Louisiana",
        "id": "LA"
    }, {
        "label": "Maine",
        "id": "ME"
    }, {
        "label": "Marshall Islands",
        "id": "MH"
    }, {
        "label": "Maryland",
        "id": "MD"
    }, {
        "label": "Massachusetts",
        "id": "MA"
    }, {
        "label": "Michigan",
        "id": "MI"
    }, {
        "label": "Minnesota",
        "id": "MN"
    }, {
        "label": "Mississippi",
        "id": "MS"
    }, {
        "label": "Missouri",
        "id": "MO"
    }, {
        "label": "Montana",
        "id": "MT"
    }, {
        "label": "Nebraska",
        "id": "NE"
    }, {
        "label": "Nevada",
        "id": "NV"
    }, {
        "label": "New Hampshire",
        "id": "NH"
    }, {
        "label": "New Jersey",
        "id": "NJ"
    }, {
        "label": "New Mexico",
        "id": "NM"
    }, {
        "label": "New York",
        "id": "NY"
    }, {
        "label": "North Carolina",
        "id": "NC"
    }, {
        "label": "North Dakota",
        "id": "ND"
    }, {
        "label": "Northern Mariana Islands",
        "id": "MP"
    }, {
        "label": "Ohio",
        "id": "OH"
    }, {
        "label": "Oklahoma",
        "id": "OK"
    }, {
        "label": "Oregon",
        "id": "OR"
    }, {
        "label": "Palau",
        "id": "PW"
    }, {
        "label": "Pennsylvania",
        "id": "PA"
    }, {
        "label": "Puerto Rico",
        "id": "PR"
    }, {
        "label": "Rhode Island",
        "id": "RI"
    }, {
        "label": "South Carolina",
        "id": "SC"
    }, {
        "label": "South Dakota",
        "id": "SD"
    }, {
        "label": "Tennessee",
        "id": "TN"
    }, {
        "label": "Texas",
        "id": "TX"
    }, {
        "label": "Utah",
        "id": "UT"
    }, {
        "label": "Vermont",
        "id": "VT"
    }, {
        "label": "Virgin Islands",
        "id": "VI"
    }, {
        "label": "Virginia",
        "id": "VA"
    }, {
        "label": "Washington",
        "id": "WA"
    }, {
        "label": "West Virginia",
        "id": "WV"
    }, {
        "label": "Wisconsin",
        "id": "WI"
    }, {
        "label": "Wyoming",
        "id": "WY"
    }];
    $scope.example2settings = {
        displayProp: 'id'
    };

}]);
