define(['app'], function (app) {
    //defining service using factory method
    app.factory('runDetailService', function ($http, utility) {
        var serviceurl = utility.baseAddress + "RunDetail/";
        return {
            getRunDetailList: function (clientId,appId,status,pageNumber, pageSize) {
                var url = serviceurl + "Get?clientId=" + clientId + "&appId=" + appId + "&status=" + status + "&pageNumber=" + pageNumber + "&pageSize=" + pageSize;
                return $http.get(url);
            },
            getRunStatus: function () {
                var url = serviceurl + "RunStatus";
                return $http.get(url);
            },
            getRunDetailCount: function() {
                var url = serviceurl + "Count";
                return $http.get(url);
            }
        };
    });
});