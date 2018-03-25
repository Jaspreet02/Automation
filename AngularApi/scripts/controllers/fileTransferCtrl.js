define(['app', 'fileTransferService','masterService'], function (app) {
    app.controller("fileTransferCtrl", function ($scope, fileTransferService, masterService) {
        $scope.fileTransfer = {};
        $scope.fileTransfers = [];
        $scope.queueList = [];
        $scope.GetQueuebyId = GetQueuebyId;
        
        // initialize File Transfer data
        (function () {
            fileTransferService.getFileTransferList().success(function (data) {
                $scope.fileTransfers = data.Result;
            }).error(function (data) {
                $scope.error =  data.ExceptionMessage;
            });
        })();

        // initialize Queue Type data
        (function () {
            masterService.getQueueType().success(function (data) {
                $scope.queueList = data;
            }).error(function (data) {
                $scope.error = data.ExceptionMessage;
            });
        })();
      
        $scope.getTemplate = function (item) {
            if (item.FileTransferSettingId === $scope.fileTransfer.FileTransferSettingId) {
                return 'edit';
            }
            else return 'display';
        };

        $scope.showadd = function () {
            $scope.fileTransfers.push({});
        };

        $scope.edit = function (item) {
            $scope.fileTransfer = angular.copy(item);
        };

        $scope.delete = function (item) {
            fileTransferService.deleteFileTransfer(item).success(function (data) {
                var index = $scope.fileTransfers.indexOf(item);
                $scope.fileTransfers.splice(index, 1);
            }).error(function (data) {
                $scope.error = data.ExceptionMessage;
            });
        };
        
        $scope.update = function (item) {
            if (item != null) {
                fileTransferService.addFileTransfer(item).success(function (data) {
                    item.FileTransferSettingId = data;
                   $scope.fileTransfers.push(item);

                    //reset form
                   $scope.fileTransfer = {};
                    // $scope.adduserform.$setPristine(); //for form reset

                }).error(function (data) {
                    $scope.error = data.ExceptionMessage;
                });
            }
        };

        $scope.reset = function () {
            $scope.fileTransfer = {};
        };

        function GetQueuebyId(id) {
            var item = $.grep($scope.queueList, function (b) { return b.QueueTypeId === id; });
            return item.length ? item[0].Status : "";
        };

        $scope.statusList = [{ id: false, name: "False" }, { id: true, name: "True" }];

    });
});

