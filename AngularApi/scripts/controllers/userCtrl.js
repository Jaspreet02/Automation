define(['app', 'userService'], function (app) {
    app.controller("userCtrl", function ($scope, userService) {
        $scope.user = null;
        $scope.editMode = false;
        $scope.users = [];
        $scope.role = "";
        $scope.GetGender = GetGender;
        $scope.maxSize = 5;     // Limit number for pagination display number.  
        $scope.totalCount = 0;  // Total number of items in all pages. initialize as a zero  
        $scope.pageIndex = 1;   // Current page number. First page is 1.-->  
        $scope.pageSizeSelected = 5; // Maximum number of items per page.

        //get User
        $scope.get = function () {
            $scope.user = this.user;
            $("#viewModal").modal('show');
        };

        // initialize your users data
        (function () {
            UserList();
        })();

        function UserList() {
            userService.getUserList($scope.pageIndex, $scope.pageSizeSelected).success(function (data) {
                $scope.users = data.Result;
                $scope.totalCount = data.Count;
            }).error(function (data) {
                $scope.error = data.ExceptionMessage;
            });
        };

        // add User
        $scope.add = function () {
            var current = this.user;
            if (current != null) {
                userService.addUser(current,this.role).success(function (data) {
                    $scope.addMode = false;
                    current.UserId = data;
                    $scope.users.push(current);

                    //reset form
                    $scope.user = null;
                    // $scope.adduserform.$setPristine(); //for form reset

                    angular.element('#userModel').modal('hide');
                }).error(function (data) {
                    $scope.error = data.ExceptionMessage;
                });
            }
        };

        //edit user
        $scope.edit = function () {
            $scope.user = this.user;
            $scope.editMode = true;
            $("#userModel").modal('show');
        };

        //update user
        $scope.update = function (id) {
            var current = this.user;
            userService.updateUser(id,current).success(function (data) {
                current.editMode = false;
                $('#userModel').modal('hide');
            }).error(function (data) {
                $scope.error =  data.ExceptionMessage;
            });
        };

        // delete User
        $scope.delete = function () {
            var current = $scope.user;
            userService.deleteUser(current).success(function (data) {
                var index = $scope.users.indexOf(current);
                $scope.users.splice(index, 1);
                $('#confirmModal').modal('hide');
                // $scope.users.pop(currentUser);

            }).error(function (data) {
                $scope.error = data.ExceptionMessage;
                angular.element('#confirmModal').modal('hide');
            });
        };

        //Model popup events
        $scope.showadd = function () {
            $scope.user = null;
            $scope.editMode = false;
            $("#userModel").modal('show');
        };

        $scope.showedit = function () {
            $('#userModel').modal('show');
        };

        $scope.showconfirm = function (data) {
            $scope.user = data;
            $("#confirmModal").modal('show');
        };

        $scope.cancel = function () {
            $scope.user = null;
            $("#userModel").modal('hide');
        };

        function GetGender(id) {
            var item = $.grep($scope.genderList, function (b) { return b.id === id; });
            return item.length ? item[0].name : "";
        };

        $scope.statusList = [{ id: false, name: "False" }, { id: true, name: "True" }];

        $scope.genderList = [{ id: 'M', name: "Male" }, { id: 'F', name: "Female" }, { id: 'O', name: "Other" }];

        $scope.roleList = [{ id: 'superAdmin', name: "Super Admin" }, { id: 'admin', name: "Admin" }, { id: 'user', name: "User" }];

        //This method is calling from pagination number  
        $scope.pageChanged = function () {
            UserList();
        };

        //This method is calling from dropDown  
        $scope.changePageSize = function () {
            $scope.pageIndex = 1;
            UserList();
        };

    });
});
