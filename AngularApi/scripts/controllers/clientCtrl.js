define(['app', 'clientService'], function (app) {
    app.controller("clientCtrl", function ($scope, clientService) {
        $scope.client = null;
        $scope.editMode = false;
        $scope.clients = [];
        $scope.password = false;
        $scope.realName = false;

        //get User
        $scope.get = function () {
            $scope.client = this.client;
            $("#viewModal").modal('show');
        };

        // initialize your users data
        (function () {
            clientService.getClientList().success(function (data) {
                $scope.clients = data.Result;
            }).error(function (data) {
                $scope.error = data.ExceptionMessage;
            });
        })();

        // add client
        $scope.add = function () {
            var current = this.client;
            if (current != null) {
                clientService.addClient(current).success(function (data) {
                    $scope.addMode = false;
                    current.ClientId = data;
                    $scope.clients.push(current);

                    //reset form
                    $scope.client = null;
                    // $scope.adduserform.$setPristine(); //for form reset

                    angular.element('#clientModel').modal('hide');
                }).error(function (data) {
                    $scope.error = data.ExceptionMessage;
                });
            }
        };

        //edit client
        $scope.edit = function () {
            $scope.client = this.client;
            $scope.editMode = true;
            $scope.dropdownChange($scope.client.ProofFormat);
            $("#clientModel").modal('show');
        };

        //update client
        $scope.update = function (id) {
            var current = this.client;
            clientService.updateClient(id, current).success(function (data) {
                current.editMode = false;
                $('#clientModel').modal('hide');
            }).error(function (data) {
                $scope.error = data.ExceptionMessage;
            });
        };

        // delete client
        $scope.delete = function () {
            var current = $scope.client;
            clientService.deleteClient(current).success(function (data) {
                var index = $scope.clients.indexOf(current);
                $scope.clients.splice(index, 1);
                $('#confirmModal').modal('hide'); 
                // $scope.users.pop(currentUser);

            }).error(function (data) {
                $scope.error = data.ExceptionMessage;
                angular.element('#confirmModal').modal('hide');
            });
        };

        //Model popup events
        $scope.showadd = function () {
            $scope.client = null;
            $scope.editMode = false;
            $scope.dropdownChange(null);
            $("#clientModel").modal('show');
        };

        $scope.showedit = function () {
            $('#clientModel').modal('show');
        };

        $scope.showconfirm = function (data) {
            $scope.client = data;
            $("#confirmModal").modal('show');
        };

        $scope.cancel = function () {
            $scope.client = null;
            $("#clientModel").modal('hide');
        };

        $scope.statusList = [{ id: false, name: "False" }, { id: true, name: "True" }];

        $scope.formatList = [{ id: 'ZIP', name: "ZIP" }, { id: 'PGP', name: "PGP" }];

        $scope.dropdownChange = function (data) {
            if (data == "ZIP") {
                $scope.password = true;
                $scope.realName = false;
            }
            else if (data == "PGP") {
                $scope.password = true;
                $scope.realName = true;
            }
            else {
                $scope.password = false;
                $scope.realName = false;
            }
        }

    });
});
