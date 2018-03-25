define(['app'], function (app) {
    //defining service using factory method
    app.factory('uploadFileService', function ($http, utility) {
        var serviceurl = utility.baseAddress + "UploadFile/";
        return {
            getUploadFileList: function (pageNumber, pageSize, fetchAll) {
                var url = serviceurl + "Get?pageNumber=" + pageNumber + "&pageSize=" + pageSize + "&fetchAll=" + fetchAll;
                return $http.get(url);
            },
            getUploadFile: function (id) {
                var url = serviceurl + "Get/" + id;
                return $http.get(url);
            },
            addUploadFile: function (uploadFile) {
                var url = serviceurl + "Post";
                return $http.post(url, uploadFile);
            },
            updateUploadFile: function (id, uploadFile) {
                var url = serviceurl + "Put/" + id;
                return $http.put(url, uploadFile);
            },
            deleteUploadFile: function (uploadFile) {
                var url = serviceurl + "Delete/" + uploadFile.UploadFileId;
                return $http.delete(url);
            }
        };
    });
});