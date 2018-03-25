define(['app', 'appComponentService'], function (app) {
    app.controller("outputLocationCtrl", function ($scope, $routeParams, appComponentService) {
        $scope.outputLocation = null;
        $scope.editMode = false;
        $scope.outputLocations = [];
        var applicationId = $routeParams.applicationId;
        var componentId = $routeParams.componentId;

        //get User
        $scope.get = function () {
            $scope.outputLocation = this.outputLocation;
            $("#viewModal").modal('show');
        };

        // initialize application data
        (function () {
            appComponentService.outputLocation(applicationId, componentId).success(function (data) {
                $scope.outputLocations = data;
            }).error(function (data) {
                $scope.error = data.ExceptionMessage;
            });
        })();

        // add application
        $scope.add = function () {
            var current = this.outputLocation;
            if (current != null) {
                current.ApplicationId = applicationId;
                current.ComponentId = componentId;
                appComponentService.addOutputLocation(current).success(function (data) {
                    $scope.addMode = false;
                    current.ComponentOutputLocationId = data;
                    $scope.outputLocations.push(current);
                    //reset form
                    $scope.outputLocation = null;
                    // $scope.adduserform.$setPristine(); //for form reset

                    angular.element('#outputLocationModel').modal('hide');
                }).error(function (data) {
                    $scope.error = data.ExceptionMessage;
                });
            }
        };

        //edit application
        $scope.edit = function () {
            $scope.outputLocation = this.outputLocation;
            $scope.editMode = true;
            $("#outputLocationModel").modal('show');
        };

        //update application
        $scope.update = function (id) {
            var current = this.outputLocation;
            current.ApplicationId = applicationId;
            current.ComponentId = componentId;
            current.ComponentOutputLocationId = id;
            appComponentService.addOutputLocation(current).success(function (data) {
                current.editMode = false;
                $('#outputLocationModel').modal('hide');
            }).error(function (data) {
                $scope.error = data.ExceptionMessage;
            });
        };

        // delete application
        $scope.delete = function () {
            var current = $scope.outputLocation;
            appComponentService.deleteOutputLocation(current).success(function (data) {
                var index = $scope.outputLocations.indexOf(current);
                $scope.outputLocations.splice(index, 1);
                $('#confirmModal').modal('hide');
                // $scope.appComponents.pop(current);

            }).error(function (data) {
                $scope.error = data.ExceptionMessage;
                angular.element('#confirmModal').modal('hide');
            });
        };

        //Model popup events
        $scope.showadd = function () {
            $scope.outputLocation = null;
            $scope.editMode = false;
            $("#outputLocationModel").modal('show');
        };

        $scope.showedit = function () {
            $('#outputLocationModel').modal('show');
        };

        $scope.showconfirm = function (data) {
            $scope.outputLocation = data;
            $("#confirmModal").modal('show');
        };

        $scope.cancel = function () {
            $scope.outputLocation = null;
            $("#outputLocationModel").modal('hide');
        };

        $scope.statusList = [{ id: false, name: "False" }, { id: true, name: "True" }];

    });
});
