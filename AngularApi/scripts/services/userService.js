define(['app'], function (app) {
    //defining service using factory method
    app.factory('userService', function ($http, utility) {
        var serviceurl = utility.baseAddress + "User/";
        return {
            getUserList: function (pageNumber, pageSize) {
                var url = serviceurl + "Get?pageNumber=" + pageNumber + "&pageSize=" + pageSize;
                return $http.get(url);
            },
            getUser: function (user) {
                var url = serviceurl + "Get/" + user.Id;
                return $http.get(url);
            },
            addUser: function (user,role) {
                var url = serviceurl + "Post?roleName=" + role;
                return $http.post(url, user);
            },
            updateUser: function (id,user) {
                var url = serviceurl + "Put/" + id;
                return $http.put(url, user);
            },
            deleteUser: function (user) {
                var url = serviceurl + "Delete/" + user.Id;
                return $http.delete(url);
            }
        };
    });
});