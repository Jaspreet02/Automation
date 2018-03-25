define(['ngRoute', 'ngStorage', 'ngCookies', 'ngResource', 'ngResponsive', 'ngPagination'], function () {

    //defining angularjs module
    var app = angular.module("app", ['ngRoute', 'ngCookies', 'ngStorage', 'ngResource', 'wt.responsive', 'ui.bootstrap']);

    var roles = {
        superUser: 0,
        admin: 1,
        user: 2
    };

    app.constant("roles", roles);

    var routeForUnauthorizedAccess = '/login';

    app.constant("routeForUnauthorizedAccess", routeForUnauthorizedAccess);

    //global service
    app.constant("utility",
        {
            baseAddress: "http://127.0.0.1:8001/api/"
        });

    //manual bootstrap
    app.init = function () {
        angular.bootstrap(document, ['app']);

        // The following is required if you want AngularJS Scenario tests to work
        $('html').addClass('ng-app: app');
    };

    //defining routes
    app.config(function ($routeProvider) {
        $routeProvider
        .when('/user', {
            templateUrl: 'views/user/user.html', //path of the view/template of route
            caseInsensitiveMatch: true,
            controller: 'userCtrl'
        })
        .when('/client', {
            templateUrl: 'views/client/client.html', //path of the view/template of route
            caseInsensitiveMatch: true,
            controller: 'clientCtrl'
        })
        .when('/application', {
            templateUrl: 'views/application/application.html', //path of the view/template of route
            caseInsensitiveMatch: true,
            controller: 'applicationCtrl'
        })
        .when('/appComponent', {
            templateUrl: 'views/application/appComponent.html', //path of the view/template of route
            caseInsensitiveMatch: true,
            controller: 'appComponentCtrl'
        })
        .when('/inputLocation', {
            templateUrl: 'views/application/inputLocation.html', //path of the view/template of route
            caseInsensitiveMatch: true,
            controller: 'inputLocationCtrl'
        })
        .when('/outputLocation', {
            templateUrl: 'views/application/outputLocation.html', //path of the view/template of route
            caseInsensitiveMatch: true,
            controller: 'outputLocationCtrl'
        })
        .when('/applicationFile', {
            templateUrl: 'views/application/applicationFile.html', //path of the view/template of route
            caseInsensitiveMatch: true,
            controller: 'applicationFileCtrl'
        })
        .when('/component', {
            templateUrl: 'views/component/component.html', //path of the view/template of route
            caseInsensitiveMatch: true,
            controller: 'componentCtrl'
        })
        .when('/runDetail', {
            templateUrl: 'views/runDetail/runDetail.html', //path of the view/template of route
            caseInsensitiveMatch: true,
            controller: 'runDetailCtrl'
        })
        .when('/logout', {
            templateUrl: 'views/login/login.html', //path of the view/template of route
            caseInsensitiveMatch: true,
            controller: 'logoutCtrl'
        })
        .when('/changePassword', {
            templateUrl: 'views/account/changePassword.html', //path of the view/template of route
            caseInsensitiveMatch: true,
            controller: 'accountCtrl'
        })
        .when('/about', {
            templateUrl: 'views/about/about.html',
            caseInsensitiveMatch: true,
            controller: 'aboutCtrl'
            //resolve: {
            //    //Here we would use all the hardwork we have done
            //    //above and make call to the authorization Service
            //    //resolve is a great feature in angular, which ensures that a route
            //    //controller (in this case superUserController ) is invoked for a route
            //    //only after the promises mentioned under it are resolved.
            //    permission: function (authorizationService, $route) {
            //        debugger;
            //        return authorizationService.permissionCheck([roles.superUser]);
            //    },
            //}
        })
       .when('/fileTransfer', {
           templateUrl: 'views/fileTransfer/fileTransfer.html',
           caseInsensitiveMatch: true,
           controller: 'fileTransferCtrl'
       })
       .when('/uploadFile', {
           templateUrl: 'views/uploadFile/uploadFile.html',
           caseInsensitiveMatch: true,
           controller: 'uploadFileCtrl',
           resolve: {
               action: function () { return 'list'; }
           }
       })
    .when('/uploadFile/new', {
        templateUrl: 'views/uploadFile/manage.html',
        caseInsensitiveMatch: true,
        controller: 'uploadFileCtrl',
        resolve: {
            action: function () { return 'new'; }
        }
    })
    .when('/uploadFile/:uploadFileId/edit', {
        templateUrl: 'views/uploadFile/manage.html',
        caseInsensitiveMatch: true,
        controller: 'uploadFileCtrl',
        resolve: {
            action: function () { return 'edit'; }
        }
    })
       .when('/email', {
           templateUrl: 'views/email/email.html',
           caseInsensitiveMatch: true,
           controller: 'emailCtrl',
           resolve: {
               action: function () { return 'list'; }
           }
       })
    .when('/email/new', {
        templateUrl: 'views/email/manage.html',
        caseInsensitiveMatch: true,
        controller: 'emailCtrl',
        resolve: {
            action: function () { return 'new'; }
        }
    })
    .when('/email/:emailTemplateId/edit', {
        templateUrl: 'views/email/manage.html',
        caseInsensitiveMatch: true,
        controller: 'emailCtrl',
        resolve: {
            action: function () { return 'edit'; }
        }
    })
       .when(routeForUnauthorizedAccess, {
           templateUrl: 'views/login/login.html',
           caseInsensitiveMatch: true,
           controller: 'loginCtrl'
       })
        // route for unauthorized access (when permission is not given to visit a page)
        .otherwise({ redirectTo: '/login' });
    });

    app.config(function ($httpProvider) {
        $httpProvider.interceptors.push('authInterceptorService');
    });

    //app.run(['$rootScope', function ($rootScope) {
    //  // you can detect change in route

    //$rootScope.$on('$routeChangeStart', function(event, next, current) {
    //    if (!current) {
    //        // insert segment you want here
    //    }
    //});
    //}]);

    return app;
});