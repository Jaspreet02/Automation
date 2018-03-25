define(['app'], function (app) {
    //defining service using factory method
    app.factory('clientService', function ($http, utility) {
        var serviceurl = utility.baseAddress + "Client/";
        return {
            getClientList: function (pageNumber, pageSize, fetchAll) {
                var url = serviceurl + "Get?pageNumber=" + pageNumber + "&pageSize=" + pageSize + "&fetchAll=" + fetchAll;
                return $http.get(url);
            },
            getClient: function (client) {
                var url = serviceurl + "Get/" + client.ClientId;
                return $http.get(url);
            },
            addClient: function (client) {
                var url = serviceurl + "Post";
                return $http.post(url, client);
            },
            updateClient: function (id, client) {
                var url = serviceurl + "Put/" + id;
                return $http.put(url, client);
            },
            deleteClient: function (client) {
                var url = serviceurl + "Delete/" + client.ClientId;
                return $http.delete(url);
            }
        };
    });
});