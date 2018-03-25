define(['app', 'uploadFileService', 'applicationService', 'fileTransferService', 'componentService'], function (app) {
    app.controller("uploadFileCtrl", function ($scope, uploadFileService, applicationService, fileTransferService, componentService, action, $routeParams,$location) {
        $scope.uploadFile = null;
        $scope.editMode = false;
        $scope.uploadFiles = [];
        $scope.applicationList = [];
        $scope.GetApplicationbyId = GetApplicationbyId;
        $scope.fileTransferList = [];
        $scope.GetFileTransferbyId = GetFileTransferbyId;
        $scope.componentList = [];
        $scope.GetComponentbyId = GetComponentbyId;
        
        switch (action) {
            case 'new':
                $scope.editMode = false;
                break;
            case 'edit':
                $scope.editMode = true;
                UploadFile();
                break;
            default:
                UploadFileList();
                break;
        }

        //get User
        function UploadFile() {
            uploadFileService.getUploadFile($routeParams.uploadFileId).success(function (data) {
                $scope.uploadFile = data;
            }).error(function (data) {
                $scope.error = data.ExceptionMessage;
            });
        }

        function UploadFileList() {
            uploadFileService.getUploadFileList().success(function (data) {
                $scope.uploadFiles = data.Result;
            }).error(function (data) {
                $scope.error = data.ExceptionMessage;
            });
        }

        // initialize client data
        //(function () {
        //    uploadFileService.getUploadFileList().success(function (data) {
        //        $scope.uploadFiles = data;
        //    }).error(function (data) {
        //        $scope.error = "An Error has occured while Loading applications! " + data.ExceptionMessage;
        //    });
        //})();

        // initialize application data
        (function () {
            applicationService.getApplicationList(1,10,true).success(function (data) {
                $scope.applicationList = data.Result;
            }).error(function (data) {
                $scope.error = data.ExceptionMessage;
            });
        })();

        // initialize component data
        (function () {
            componentService.getComponentList(1,10,true).success(function (data) {
                $scope.componentList = data.Result;
            }).error(function (data) {
                $scope.error = data.ExceptionMessage;
            });
        })();

        // initialize filetransfer data
        (function () {
            fileTransferService.getFileTransferList(1,10,true).success(function (data) {
                $scope.fileTransferList = data.Result;
            }).error(function (data) {
                $scope.error = data.ExceptionMessage;
            });
        })();

        // add application
        $scope.add = function () {
            var current = this.uploadFile;
            if (current != null) {
                uploadFileService.addUploadFile(current).success(function (data) {                   
                    //reset form
                    $scope.uploadFile = null;
                    // $scope.adduserform.$setPristine(); //for form reset
                    $location.url("/uploadFile");
                }).error(function (data) {
                    $scope.error = data.ExceptionMessage;
                });
            }
        };

        //update application
        $scope.update = function (id) {
            var current = this.uploadFile;
            uploadFileService.updateUploadFile(id, current).success(function (data) {
                $location.url("/uploadFile");
            }).error(function (data) {
                $scope.error =  data.ExceptionMessage;
            });
        };

        // delete application
        $scope.delete = function () {
            var current = $scope.uploadFile;
            uploadFileService.deleteUploadFile(current).success(function (data) {
                var index = $scope.uploadFiles.indexOf(current);
                $scope.uploadFiles.splice(index, 1);
                $('#confirmModal').modal('hide');
                // $scope.users.pop(currentUser);

            }).error(function (data) {
                $scope.error = data.ExceptionMessage;
                angular.element('#confirmModal').modal('hide');
            });
        };

       
        $scope.showconfirm = function (data) {
            $scope.uploadFile = data;
            $("#confirmModal").modal('show');
        };

        $scope.cancel = function () {
            $scope.uploadFile = null;
            $location.url("/uploadFile");
        };

        function GetApplicationbyId(id) {
            var item = $.grep($scope.applicationList, function (b) { return b.ApplicationId === id; });
            return item.length ? item[0].Name : "";
        };

        function GetComponentbyId(id) {
            var item = $.grep($scope.componentList, function (b) { return b.ComponentId === id; });
            return item.length ? item[0].Name : "";
        };

        function GetFileTransferbyId(id) {
            var fileTransfer = $.grep($scope.fileTransferList, function (b) { return b.FileTransferSettingId === id; });
            return fileTransfer.length ? fileTransfer[0].Name : "";
        };

        $scope.statusList = [{ id: false, name: "False" }, { id: true, name: "True" }];

    });
});
