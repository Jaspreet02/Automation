define(['app', 'emailService','clientService', 'applicationService','componentService'], function (app) {
    app.controller("emailCtrl", function ($scope, emailService, clientService, applicationService, componentService, action, $routeParams, $location) {
        $scope.email = null;
        $scope.editMode = false;
        $scope.emails = [];
        $scope.clientList = [];
        $scope.GetClientbyId = GetClientbyId;
        $scope.applicationList = [];
        $scope.GetApplicationbyId = GetApplicationbyId;
        $scope.componentList = [];
        $scope.GetComponentbyId = GetComponentbyId;
        $scope.emailToken = [];


        switch (action) {
            case 'new':
                $scope.editMode = false;
                EmailToken();
                break;
            case 'edit':
                $scope.editMode = true;
                EmailToken();
                Email();
                break;
            default:
                EmailList();
                break;
        }

        //get User
        function Email() {
            emailService.getEmail($routeParams.emailTemplateId).success(function (data) {
                $scope.email = data;
            }).error(function (data) {
                $scope.error = data.ExceptionMessage;
            });
        }

        function EmailList() {
            emailService.getEmailList().success(function (data) {
                $scope.emails = data;
            }).error(function (data) {
                $scope.error = data.ExceptionMessage;
            });
        }

        function EmailToken() {
            emailService.emailToken().success(function (data) {
                $scope.emailToken = data;
            }).error(function (data) {
                $scope.error = data.ExceptionMessage;
            });
        }

        // initialize application data
        (function () {
            clientService.getClientList().success(function (data) {
                $scope.clientList = data;
            }).error(function (data) {
                $scope.error = data.ExceptionMessage;
            });
        })();

        // initialize application data
        (function () {
            applicationService.getApplicationList().success(function (data) {
                $scope.applicationList = data;
            }).error(function (data) {
                $scope.error = data.ExceptionMessage;
            });
        })();

        // initialize component data
        (function () {
            componentService.getComponentList().success(function (data) {
                $scope.componentList = data;
            }).error(function (data) {
                $scope.error = data.ExceptionMessage;
            });
        })();
        
        // add application
        $scope.add = function () {
            var current = this.email;
            if (current != null) {
                emailService.addEmail(current).success(function (data) {
                    //reset form
                    $scope.email = null;
                    // $scope.adduserform.$setPristine(); //for form reset
                    $location.url("/email");
                }).error(function (data) {
                    $scope.error = data.ExceptionMessage;
                });
            }
        };

        //update application
        $scope.update = function (id) {
            var current = this.email;
            emailService.updateEmail(id, current).success(function (data) {
                $location.url("/email");
            }).error(function (data) {
                $scope.error = data.ExceptionMessage;
            });
        };

        // delete application
        $scope.delete = function () {
            var current = $scope.email;
            emailService.deleteEmail(current.EmailTemplateId).success(function (data) {
                var index = $scope.emails.indexOf(current);
                $scope.emails.splice(index, 1);
                $('#confirmModal').modal('hide');
                // $scope.users.pop(currentUser);

            }).error(function (data) {
                $scope.error = data.ExceptionMessage;
                angular.element('#confirmModal').modal('hide');
            });
        };


        $scope.showconfirm = function (data) {
            $scope.email = data;
            $("#confirmModal").modal('show');
        };

        $scope.cancel = function () {
            $scope.email = null;
            $location.url("/email");
        };

        function GetClientbyId(id) {
            var item = $.grep($scope.clientList, function (b) { return b.ClientId === id; });
            return item.length ? item[0].Name : "";
        };

        function GetApplicationbyId(id) {
            var item = $.grep($scope.applicationList, function (b) { return b.ApplicationId === id; });
            return item.length ? item[0].Name : "";
        };

        function GetComponentbyId(id) {
            var item = $.grep($scope.componentList, function (b) { return b.ComponentId === id; });
            return item.length ? item[0].Name : "";
        };

        $scope.statusList = [{ id: false, name: "False" }, { id: true, name: "True" }];

    });
});
