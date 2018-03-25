define(['app'], function (app) {
    //defining service using factory method
    app.factory('accountService', function ($http, utility) {
        var serviceurl = utility.baseAddress + "Account/";
        return {
            changePassword: function (entity) {
                var url = serviceurl + "ChangePassword";
                return $http.post(url, entity);
            }
        };
    });
});