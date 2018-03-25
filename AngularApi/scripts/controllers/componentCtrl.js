define(['app', 'componentService'], function (app) {
    app.controller("componentCtrl", function ($scope, componentService) {
        $scope.component = null;
        $scope.editMode = false;
        $scope.components = [];
        $scope.triggerandStatusFile = null;

        //get User
        $scope.get = function (data) {
            if (data == null) {
                $scope.triggerandStatusFile = [];
            }
            else {
                $scope.triggerandStatusFile = data;
            }
            $scope.triggerandStatusFile.ComponentId = this.component.ComponentId;
            $("#viewModal").modal('show');
        };

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
            var current = this.component;
            if (current != null) {
                componentService.addComponent(current).success(function (data) {
                    $scope.addMode = false;
                    current.ComponentId = data;
                    $scope.components.push(current);

                    //reset form
                    $scope.component = null;
                    // $scope.adduserform.$setPristine(); //for form reset

                    angular.element('#componentModel').modal('hide');
                }).error(function (data) {
                    $scope.error = data.ExceptionMessage;
                });
            }
        };

        //edit application
        $scope.edit = function () {
            $scope.component = this.component;
            $scope.editMode = true;
            $("#componentModel").modal('show');
        };

        //update application
        $scope.update = function (id) {
            var current = this.component;
            componentService.updateComponent(id, current).success(function (data) {
                current.editMode = false;
                $('#componentModel').modal('hide');
            }).error(function (data) {
                $scope.error = data.ExceptionMessage;
            });
        };

        // delete application
        $scope.delete = function () {
            var current = $scope.component;
            componentService.deleteComponent(current).success(function (data) {
                var index = $scope.components.indexOf(current);
                $scope.components.splice(index, 1);
                $('#confirmModal').modal('hide');
                // $scope.users.pop(currentUser);

            }).error(function (data) {
                $scope.error = data.ExceptionMessage;
                angular.element('#confirmModal').modal('hide');
            });
        };

        // add application
        $scope.triggerAdd = function () {
            var current = this.triggerandStatusFile;
            if (current != null) {
                componentService.updateTriggerandStatus(current).success(function (data) {
                   
                    //reset form
                    $scope.triggerandStatusFile = null;
                    // $scope.adduserform.$setPristine(); //for form reset

                    angular.element('#viewModal').modal('hide');
                }).error(function (data) {
                    $scope.error = data.ExceptionMessage;
                });
            }
        };

        //Model popup events
        $scope.showadd = function () {
            $scope.component = null;
            $scope.editMode = false;
            $("#componentModel").modal('show');
        };

        $scope.showedit = function () {
            $('#componentModel').modal('show');
        };

        $scope.showconfirm = function (data) {
            $scope.component = data;
            $("#confirmModal").modal('show');
        };

        $scope.cancel = function () {
            $scope.component = null;
            $("#componentModel").modal('hide');
        };
        
        $scope.statusList = [{ id: false, name: "False" }, { id: true, name: "True" }];

    });
});
