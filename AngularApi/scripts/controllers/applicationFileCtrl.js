define(['app', 'applicationService'], function (app) {
    app.controller("applicationFileCtrl", function ($scope, $routeParams, applicationService) {
        $scope.applicationFiles = [];
        var applicationId = $routeParams.applicationId;
        $scope.applicationName = $routeParams.applicationName;
        $scope.selected = {};
        
        // initialize application data
        (function () {
            applicationService.applicationFiles(applicationId).success(function (data) {
                $scope.applicationFiles = data;
            }).error(function (data) {
                $scope.error = data.ExceptionMessage;
            });
        })();
      
        $scope.getTemplate = function (applicationFile) {
            if (applicationFile.ApplicationFileId === $scope.selected.ApplicationFileId) {
                return 'edit';
            }
            else return 'display';
        };

        $scope.showadd = function () {
            $scope.applicationFiles.push({});
        };

        $scope.edit = function (applicationFile) {
            $scope.selected = angular.copy(applicationFile);
        };

        $scope.delete = function (applicationFile) {
            applicationService.deleteApplicationFile(applicationFile).success(function (data) {
                var index = $scope.applicationFiles.indexOf(applicationFile);
                $scope.applicationFiles.splice(index, 1);
            }).error(function (data) {
                $scope.error = data.ExceptionMessage;
            });
        };
        
        $scope.update = function (applicationFile) {
            if (applicationFile != null) {
                applicationFile.ApplicationId = applicationId;
               applicationService.addApplicationFile(applicationFile).success(function (data) {
                   applicationFile.ApplicationFileId = data;
                   $scope.applicationFiles.push(applicationFile);

                    //reset form
                   $scope.selected = {};
                    // $scope.adduserform.$setPristine(); //for form reset

                }).error(function (data) {
                    $scope.error = data.ExceptionMessage;
                });
            }
        };

        $scope.reset = function () {
            $scope.selected = {};
        };

        $scope.statusList = [{ id: false, name: "False" }, { id: true, name: "True" }];

    });
});
