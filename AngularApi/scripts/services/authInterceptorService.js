define(['app'], function (app) {
    //defining service using factory method
    app.factory('authInterceptorService', function ($q, $injector, $location, $localStorage) {

        return {

            request: function (config) {

                config.headers = config.headers || {};
                var authData = $localStorage.authorizationData;
                if (authData) {
                    config.headers.Authorization = 'Bearer ' + authData.token;
                }

                return config;
            },

            responseError: function (rejection) {
                if (rejection.status === 401) {
                    var authService = $injector.get('authorizationService');                    
                    authService.logout();
                    $location.path('/login');
                }
                return $q.reject(rejection);
            }
        };
    });
});