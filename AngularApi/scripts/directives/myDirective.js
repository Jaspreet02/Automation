define(['app', 'authorizationService'], function (app) {
    app.directive('isNumber', function () {
        return {
            restrict: 'A',
            require: '?ngModel',
            link: function (scope, element, attrs, modelCtrl) {
                modelCtrl.$parsers.push(function (inputValue) {
                    if (inputValue == undefined) return '';
                    var transformedInput = inputValue.replace(/[^0-9]/g, '');
                    if (transformedInput !== inputValue) {
                        modelCtrl.$setViewValue(transformedInput);
                        modelCtrl.$render();
                    }
                    return transformedInput;
                });
            }
        };
    });
    app.directive('myCustomer', function (authorizationService, $localStorage) {
        var headerPath = 'views/shared/_' + authorizationService.getUserInfo().userType + '.html';
        return {
            restrict: 'E',
            templateUrl: headerPath
        }
    });
    app.directive('pwCheck', function () {
        return {
            restrict: 'A',
            require: '?ngModel',
            link: function (scope, element, attrs, modelCtrl) {
                var firstPassword = '#' + attrs.pwCheck;
                element.add(firstPassword).on('keyup', function () {
                    scope.$apply(function () {
                        var v = element.val() === $(firstPassword).val();
                        modelCtrl.$setValidity('pwmatch', v);
                    });
                });
            }
        }
    });
    app.directive('loading', ['$http', function ($http) {
        return {
            restrict: 'A',
            link: function (scope, elm, attrs) {
                scope.isLoading = function () {
                    return $http.pendingRequests.length > 0;
                };

                scope.$watch(scope.isLoading, function (v) {
                    if (v) {
                        elm.show();
                    } else {
                        elm.hide();
                    }
                });
            }
        };

    }]);
});