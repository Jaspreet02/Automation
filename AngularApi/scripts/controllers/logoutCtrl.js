define(['app', 'authorizationService'], function (app) {
    app.controller("logoutCtrl", function ($scope, authorizationService) {

        authorizationService.logout();

    });
});