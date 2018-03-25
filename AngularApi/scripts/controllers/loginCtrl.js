define(['app', 'authorizationService'], function (app) {
    app.controller("loginCtrl", function ($scope, authorizationService) {

        $scope.authorizationService = authorizationService;

        $scope.loginData = {
            userName: "",
            password: ""
        };


        $scope.login = function () {
            authorizationService.login($scope.loginData).then(function (response) {
                window.location = "#runDetail";
            }, function (error) {
                $scope.error = error.data.error_description;
            });
        };
    });
});