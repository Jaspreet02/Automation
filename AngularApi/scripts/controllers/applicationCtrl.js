define(['app', 'applicationService','clientService','fileTransferService','masterService'], function (app) {
    app.controller("applicationCtrl", function ($scope, applicationService, clientService, fileTransferService, masterService) {
        $scope.application = null;
        $scope.editMode = false;
        $scope.applications = [];
        $scope.password = false;
        $scope.realName = false;
        $scope.clientList = [];
        $scope.GetClientbyId = GetClientbyId;
        $scope.fileTransferList = [];
        $scope.GetFileTransferbyId = GetFileTransferbyId;
        $scope.fileKeyword = [];
        $scope.maxSize = 5;     // Limit number for pagination display number.  
        $scope.totalCount = 0;  // Total number of items in all pages. initialize as a zero  
        $scope.pageIndex = 1;   // Current page number. First page is 1.-->  
        $scope.pageSizeSelected = 5; // Maximum number of items per page.

        //get User
        $scope.get = function () {
            $scope.application = this.application;
            $("#viewModal").modal('show');
        };

        // initialize application data
        (function () {
            ApplicationList();
        })();

        function ApplicationList() {
            applicationService.getApplicationList($scope.pageIndex, $scope.pageSizeSelected,false).success(function (data) {
                $scope.applications = data.Result;
                $scope.totalCount = data.Count;
            }).error(function (data) {
                $scope.error = data.ExceptionMessage;
            });
        };

        // initialize client data
        (function () {
            clientService.getClientList(1,10,true).success(function (data) {
                $scope.clientList = data.Result;
            }).error(function (data) {
                $scope.error = data.ExceptionMessage;
            });
        })();

        // initialize Queue Type data
        (function () {
            masterService.getFileKeyword().success(function (data) {
                $scope.fileKeyword = data;
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
            var current = this.application;
            if (current != null) {
                applicationService.addApplication(current).success(function (data) {
                    $scope.addMode = false;
                    current.ApplicationId = data;
                    $scope.applications.push(current);

                    //reset form
                    $scope.application = null;
                    // $scope.adduserform.$setPristine(); //for form reset

                    angular.element('#applicationModel').modal('hide');
                }).error(function (data) {
                    $scope.error = data.ExceptionMessage;
                });
            }
        };

        //edit application
        $scope.edit = function () {
            $scope.application = this.application;
            $scope.editMode = true;
            $("#applicationModel").modal('show');
        };

        //update application
        $scope.update = function (id) {
            var current = this.application;
            applicationService.updateApplication(id, current).success(function (data) {
                current.editMode = false;
                $('#applicationModel').modal('hide');
            }).error(function (data) {
                $scope.error = data.ExceptionMessage;
            });
        };

        // delete application
        $scope.delete = function () {
            var current = $scope.application;
            applicationService.deleteApplication(current).success(function (data) {
                var index = $scope.applications.indexOf(current);
                $scope.applications.splice(index, 1);
                $('#confirmModal').modal('hide');
                // $scope.users.pop(currentUser);

            }).error(function (data) {
                $scope.error = data.ExceptionMessage;
                angular.element('#confirmModal').modal('hide');
            });
        };

        //Model popup events
        $scope.showadd = function () {
            $scope.application = null;
            $scope.editMode = false;
            $("#applicationModel").modal('show');
        };

        $scope.showedit = function () {
            $('#applicationModel').modal('show');
        };

        $scope.showconfirm = function (data) {
            $scope.application = data;
            $("#confirmModal").modal('show');
        };

        $scope.cancel = function () {
            $scope.application = null;
            $("#applicationModel").modal('hide');
        };

        function GetClientbyId(id) {
            var client = $.grep($scope.clientList, function (b) { return b.ClientId === id; });
            return client.length ? client[0].Name : "";
        };

        function GetFileTransferbyId(id) {
            var fileTransfer = $.grep($scope.fileTransferList, function (b) { return b.FileTransferSettingId === id; });
            return fileTransfer.length ? fileTransfer[0].Name : "";
        };

        $scope.statusList = [{ id: false, name: "False" }, { id: true, name: "True" }];
          
        //This method is calling from pagination number  
        $scope.pageChanged = function () {
            ApplicationList();
        };

        //This method is calling from dropDown  
        $scope.changePageSize = function () {
            $scope.pageIndex = 1;
            ApplicationList();
        };

    });
});
