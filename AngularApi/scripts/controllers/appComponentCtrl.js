define(['app', 'appComponentService', 'componentService'], function (app) {
    app.controller("appComponentCtrl", function ($scope, $routeParams, appComponentService, componentService) {
        $scope.appComponent = null;
        $scope.editMode = false;
        $scope.appComponents = [];
        $scope.components = [];
        $scope.GetComponentbyId = GetComponentbyId;
        var applicationId = $routeParams.applicationId;
        $scope.applicationName = $routeParams.applicationName;
        $scope.orders = [];

        //get User
        $scope.get = function () {
            $scope.appComponent = this.appComponent;
            $("#viewModal").modal('show');
        };

        // initialize application data
        (function () {
            appComponentService.getAppComponentList(applicationId).success(function (data) {
                $scope.appComponents = data;
                OrderList(data.length);
            }).error(function (data) {
                $scope.error = data.ExceptionMessage;
            });
        })();

        // initialize application data
        (function () {
            componentService.getComponentList().success(function (data) {
                $scope.components = data.Result;
            }).error(function (data) {
                $scope.error = data.ExceptionMessage;
            });
        })();

        // add application
        $scope.add = function () {
            var current = this.appComponent;
            if (current != null) {
                current.ApplicationId = applicationId;
                appComponentService.addAppComponent(current).success(function (data) {
                    $scope.addMode = false;
                    current.ApplicationComponentId = data;
                    $scope.appComponents.push(current);
                    OrderList($scope.orders.length);
                    //reset form
                    $scope.appComponent = null;
                    // $scope.adduserform.$setPristine(); //for form reset

                    angular.element('#appComponentModel').modal('hide');
                }).error(function (data) {
                    $scope.error = data.ExceptionMessage;
                });
            }
        };

        //edit application
        $scope.edit = function () {
            $scope.appComponent = this.appComponent;
            $scope.editMode = true;
            $("#appComponentModel").modal('show');
        };

        //update application
        $scope.update = function (id) {
            var current = this.appComponent;
            current.ApplicationId = applicationId;
            appComponentService.updateAppComponent(id, current).success(function (data) {
                current.editMode = false;
                $('#appComponentModel').modal('hide');
            }).error(function (data) {
                $scope.error = data.ExceptionMessage;
            });
        };

        // delete application
        $scope.delete = function () {
            var current = $scope.appComponent;
            appComponentService.deleteAppComponent(current).success(function (data) {
                var index = $scope.appComponents.indexOf(current);
                $scope.appComponents.splice(index, 1);
                $('#confirmModal').modal('hide');
               // $scope.appComponents.pop(current);

            }).error(function (data) {
                $scope.error = data.ExceptionMessage;
                angular.element('#confirmModal').modal('hide');
            });
        };

        //Model popup events
        $scope.showadd = function () {
            $scope.appComponent = null;
            $scope.editMode = false;
            $("#appComponentModel").modal('show');
        };

        $scope.showedit = function () {
            $('#appComponentModel').modal('show');
        };

        $scope.showconfirm = function (data) {
            $scope.appComponent = data;
            $("#confirmModal").modal('show');
        };

        $scope.cancel = function () {
            $scope.appComponent = null;
            $("#appComponentModel").modal('hide');
        };

        function GetComponentbyId(id) {
            var item = $.grep($scope.components, function (b) { return b.ComponentId === id; });
            return item.length ? item[0].Name : "";
        };

        $scope.$watch('appComponent.ComponentId', function () {
            var item = $.grep($scope.components, function (b) { return b.ComponentId === $scope.appComponent.ComponentId; });
            $scope.isEnabled = item.length ? item[0].IsOptional : false;
        });

        $scope.statusList = [{ id: false, name: "False" }, { id: true, name: "True" }];

        
        function OrderList(length) {
            $scope.orders.length = 0;
                for (var i = 0; i <= length; i++) {
                    $scope.orders.push(i + 1);
                }
        };

    });
});
