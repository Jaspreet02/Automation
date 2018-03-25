define(['app'], function (app) {
    //defining service using factory method
    app.factory('masterService', function ($http, utility) {
        var serviceurl = utility.baseAddress + "Master/";
        return {
            getRunNumberStatus: function () {
                var url = serviceurl + "RunNumberStatus";
                return $http.get(url);
            },
            getQueueType: function () {
                var url = serviceurl + "QueueType";
                return $http.get(url);
            },
            getFileKeyword: function () {
                var url = serviceurl + "FileKeyword";
                return $http.get(url);
            }
        };
    });
});