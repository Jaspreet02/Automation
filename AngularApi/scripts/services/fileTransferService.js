define(['app'], function (app) {
    //defining service using factory method
    app.factory('fileTransferService', function ($http, utility) {
        var serviceurl = utility.baseAddress + "FileTransfer/";
        return {
            getFileTransferList: function (pageNumber, pageSize, fetchAll) {
                var url = serviceurl + "Get?pageNumber=" + pageNumber + "&pageSize=" + pageSize + "&fetchAll=" + fetchAll;
                return $http.get(url);
            },
            getFileTransfer: function (fileTransfer) {
                var url = serviceurl + "Get/" + fileTransfer.FileTransferSettingId;
                return $http.get(url);
            },
            addFileTransfer: function (fileTransfer) {
                var url = serviceurl + "Post";
                return $http.post(url, fileTransfer);
            },
            updateFileTransfer: function (id, fileTransfer) {
                var url = serviceurl + "Put/" + id;
                return $http.put(url, fileTransfer);
            },
            deleteFileTransfer: function (fileTransfer) {
                var url = serviceurl + "Delete/" + fileTransfer.FileTransferSettingId;
                return $http.delete(url);
            }
        };
    });
});