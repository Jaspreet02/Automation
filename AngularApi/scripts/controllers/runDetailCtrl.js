define(['app', 'runDetailService', 'applicationService', 'clientService'], function (app) {
    app.controller("runDetailCtrl", function ($scope, runDetailService, applicationService, clientService, $timeout) {
        $scope.runDetails = [];
        $scope.applications = [];
        $scope.clients = [];
        $scope.application = {};
        $scope.client = {};
        $scope.runStatus = [];
        $scope.status = {};
        $scope.GetApplicationbyId = GetApplicationbyId;
        $scope.GetRunStatusbyId = GetRunStatusbyId;
        $scope.maxSize = 5;     // Limit number for pagination display number.  
        $scope.totalCount = 0;  // Total number of items in all pages. initialize as a zero  
        $scope.pageIndex = 1;   // Current page number. First page is 1.-->  
        $scope.pageSizeSelected = 5; // Maximum number of items per page.



        // initialize rundetail data
        (function () {
            RunDetailList();
        })();

        function RunDetailList() {
            var clientId = $scope.client != null ? ~~$scope.client.ClientId : 0;
            var applicationId = ~~$scope.application.ApplicationId;
            var statusId = $scope.runStatus.indexOf($scope.status);
            runDetailService.getRunDetailList(clientId, applicationId, statusId, $scope.pageIndex, $scope.pageSizeSelected).success(function (data) {
                $scope.runDetails = data.Result;
                $scope.totalCount = data.Count;
            }).error(function (data) {
                $scope.error = data.ExceptionMessage;
            });

            $timeout(function () {
                // RunDetailList();
            }, 30000)
        };

        // initialize client data
        (function () {
            clientService.getClientList().success(function (data) {
                $scope.clients = data.Result;
            }).error(function (data) {
                $scope.error = data.ExceptionMessage;
            });
        })();

        // initialize application data
        (function () {
            applicationService.getApplicationList(1,10,true).success(function (data) {
                $scope.applications = data.Result;
            }).error(function (data) {
                $scope.error = data.ExceptionMessage;
            });
        })();

        // initialize run status data
        (function () {
            runDetailService.getRunStatus().success(function (data) {
                $scope.runStatus = data;
            }).error(function (data) {
                $scope.error = data.ExceptionMessage;
            });
        })();

        function GetApplicationbyId(id) {
            var item = $.grep($scope.applications, function (b) { return b.ApplicationId === id; });
            return item.length ? item[0].Name : "";
        };

        function GetRunStatusbyId(id) {            
            return $scope.runStatus.length ? $scope.runStatus[id] : "";
        };

        $scope.filterExpression = function (item) {
            return ($scope.client !== null &&  item.ClientId === $scope.client.ClientId);
        };

        $scope.statusList = [{ id: false, name: "False" }, { id: true, name: "True" }];

        $scope.dropdownEvent = function (item) {
            if (item == "client") {
                $scope.application = {};
            }
            RunDetailList();
        };

        //This method is calling from pagination number  
        $scope.pageChanged = function () {
            RunDetailList();
        };

        //This method is calling from dropDown  
        $scope.changePageSize = function () {
            $scope.pageIndex = 1;
            RunDetailList();
        };

        $scope.expandAll = function (expanded) {
            // $scope is required here, hence the injection above, even though we're using "controller as" syntax
            $scope.$broadcast('onExpandAll', {
                expanded: expanded
            });
        };

    })
        .directive('expand', function () {
            function link(scope, element, attrs) {
                scope.$on('onExpandAll', function (event, args) {
                    scope.expanded = args.expanded;
                });
            }
            return {
                link: link
            };
        });
});
