define(['app', 'accountService'], function (app) {
    app.controller("accountCtrl", function ($scope, accountService) {
        $scope.changePassword = null;

        // add User
        $scope.reset = function () {
            var current = this.changePassword;
            if (current != null) {
                accountService.changePassword(current).success(function (data) {

                    //reset form
                    $scope.changePassword = null;
                    // $scope.adduserform.$setPristine(); //for form reset

                    window.location = "#logout";

                }).error(function (data) {
                    $scope.error = data.ExceptionMessage;
                });
            }
        };

    });
});
