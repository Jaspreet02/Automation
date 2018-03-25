define(['app'], function (app) {
    //defining service using factory method
    app.factory('applicationService', function ($http, utility) {
        var serviceurl = utility.baseAddress + "application/";
        return {
            getApplicationList: function (pageNumber, pageSize, fetchAll) {
                var url = serviceurl + "Get?pageNumber=" + pageNumber + "&pageSize=" + pageSize + "&fetchAll=" + fetchAll;
                return $http.get(url);
            },
            getApplication: function (application) {
                var url = serviceurl + "Get/" + application.ApplicationId;
                return $http.get(url);
            },
            addApplication: function (application) {
                var url = serviceurl + "Post";
                return $http.post(url, application);
            },
            updateApplication: function (id, application) {
                var url = serviceurl + "Put/" + id;
                return $http.put(url, application);
            },
            deleteApplication: function (application) {
                var url = serviceurl + "Delete/" + application.ApplicationId;
                return $http.delete(url);
            },
            applicationFiles: function (applicationId) {
                var url = serviceurl + "ApplicationFiles?applicationId=" + applicationId;
                return $http.get(url);
            },
            addApplicationFile: function (applicationFile) {
                var url = serviceurl + "AddApplicationFile";
                return $http.post(url, applicationFile);
            },
            deleteApplicationFile: function (applicationFile) {
                var url = serviceurl + "DeleteApplicationFile?applicationFileId=" + applicationFile.ApplicationFileId;
                return $http.delete(url);
            }
        };
    });
});