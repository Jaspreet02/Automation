define(['app'], function (app) {
    //defining service using factory method
    app.factory('emailService', function ($http, utility) {
        var serviceurl = utility.baseAddress + "EmailTemplate/";
        return {
            getEmailList: function () {
                var url = serviceurl + "Get";
                return $http.get(url);
            },
            getEmail: function (id) {
                var url = serviceurl + "Get/" + id;
                return $http.get(url);
            },
            addEmail: function (email) {
                var url = serviceurl + "Post";
                return $http.post(url, email);
            },
            updateEmail: function (id, email) {
                var url = serviceurl + "Put/" + id;
                return $http.put(url, email);
            },
            deleteEmail: function (id) {
                var url = serviceurl + "Delete/" + id;
                return $http.delete(url);
            },
            emailToken: function () {
                var url = serviceurl + "EmailToken";
                return $http.get(url);
            }
        };
    });
});