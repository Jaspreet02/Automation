define(['app', 'appComponentService'], function (app) {
    app.controller("inputLocationCtrl", function ($scope, $routeParams, appComponentService) {
        $scope.inputLocation = null;
        $scope.editMode = false;
        $scope.inputLocations = [];
        var applicationId = $routeParams.applicationId;
        var componentId = $routeParams.componentId;

        //get User
        $scope.get = function () {
            $scope.inputLocation = this.inputLocation;
            $("#viewModal").modal('show');
        };

        // initialize application data
        (function () {
            appComponentService.inputLocation(applicationId,componentId).success(function (data) {
                $scope.inputLocations = data;
            }).error(function (data) {
                $scope.error = data.ExceptionMessage;
            });
        })();
        
        // add application
        $scope.add = function () {
            var current = this.inputLocation;
            if (current != null) {
                current.ApplicationId = applicationId;
                current.ComponentId = componentId;
                appComponentService.addInputLocation(current).success(function (data) {
                    $scope.addMode = false;
                    current.ComponentInputLocationId = data;
                    $scope.inputLocations.push(current);
                    //reset form
                    $scope.inputLocation= null;
                    // $scope.adduserform.$setPristine(); //for form reset

                    angular.element('#inputLocationModel').modal('hide');
                }).error(function (data) {
                    $scope.error = data.ExceptionMessage;
                });
            }
        };

        //edit application
        $scope.edit = function () {
            $scope.inputLocation = this.inputLocation;
            $scope.editMode = true;
            $("#inputLocationModel").modal('show');
        };

        //update application
        $scope.update = function (id) {
            var current = this.inputLocation;
            current.ApplicationId = applicationId;
            current.ComponentId = componentId;
            current.ComponentInputLocationId = id;
            appComponentService.addInputLocation(current).success(function (data) {
                current.editMode = false;
                $('#inputLocationModel').modal('hide');
            }).error(function (data) {
                $scope.error = data.ExceptionMessage;
            });
        };

        // delete application
        $scope.delete = function () {
            var current = $scope.inputLocation;
            appComponentService.deleteInputLocation(current).success(function (data) {
                var index = $scope.inputLocations.indexOf(current);
                $scope.inputLocations.splice(index, 1);
                $('#confirmModal').modal('hide');
                // $scope.appComponents.pop(current);

            }).error(function (data) {
                $scope.error = data.ExceptionMessage;
                angular.element('#confirmModal').modal('hide');
            });
        };

        //Model popup events
        $scope.showadd = function () {
            $scope.inputLocation = null;
            $scope.editMode = false;
            $("#inputLocationModel").modal('show');
        };

        $scope.showedit = function () {
            $('#inputLocationModel').modal('show');
        };

        $scope.showconfirm = function (data) {
            $scope.inputLocation = data;
            $("#confirmModal").modal('show');
        };

        $scope.cancel = function () {
            $scope.inputLocation = null;
            $("#inputLocationModel").modal('hide');
        };
        
        $scope.statusList = [{ id: false, name: "False" }, { id: true, name: "True" }];
        
    });
});
