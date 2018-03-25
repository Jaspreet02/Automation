/// <reference path="app.js" />
require.config({
    baseurl: '/scripts/',
    paths: {
        'angular': 'libs/angular',
        'ngStorage': 'libs/ngStorage',
        'ngCookies': 'libs/angular-cookies',
        'ngRoute': 'libs/angular-route',
        'ngResource': 'libs/angular-resource',
        'ngResponsive': 'libs/angular-responsive-tables',
        'ngPagination': 'libs/ui-bootstrap-tpls-0.13.4.min',
        'jquery': 'libs/jquery-1.10.2',
        'bootstrap': 'libs/bootstrap',
        'userService': 'services/userService',
        'emailService': 'services/emailService',
        'accountService': 'services/accountService',
        'clientService': 'services/clientService',
        'appComponentService': 'services/appComponentService',
        'authorizationService': 'services/authorizationService',
        'applicationService': 'services/applicationService',
        'fileTransferService': 'services/fileTransferService',
        'componentService': 'services/componentService',
        'runDetailService': 'services/runDetailService',
        'masterService': 'services/masterService',
        'uploadFileService': 'services/uploadFileService',
        'authInterceptorService': 'services/authInterceptorService',
        'loginCtrl': "controllers/loginCtrl",
        'logoutCtrl': "controllers/logoutCtrl",
        'aboutCtrl': "controllers/aboutCtrl",
        'runDetailCtrl': "controllers/runDetailCtrl",
        'filter': "filters/filter",
        'userCtrl': "controllers/userCtrl",
        'clientCtrl': "controllers/clientCtrl",
        'appComponentCtrl': "controllers/appComponentCtrl",
        'inputLocationCtrl': "controllers/inputLocationCtrl",
        'outputLocationCtrl': "controllers/outputLocationCtrl",
        'applicationCtrl': "controllers/applicationCtrl",
        'componentCtrl': "controllers/componentCtrl",
        'applicationFileCtrl': "controllers/applicationFileCtrl",
        'fileTransferCtrl': "controllers/fileTransferCtrl",
        'uploadFileCtrl': "controllers/uploadFileCtrl",
        'emailCtrl': "controllers/emailCtrl",
        'accountCtrl': "controllers/accountCtrl",
        'directive': "directives/myDirective"
    },
    shim: {
        ngStorage: {
            deps: ['angular'],
            exports: 'angular'
        },
        ngCookies: {
            deps: ['angular'],
            exports: 'angular'
        },
        ngRoute: {
            deps: ['angular'],
            exports: 'angular'
        },
        ngResource: {
            deps: ['angular'],
            exports: 'angular'
        },
        ngPagination: {
            deps: ['angular'],
            exports: 'angular'
        },
        ngResponsive: {
            deps: ['angular'],
            exports: 'angular'
        },
        angular: {
            exports: 'angular'
        },
        bootstrap: {
            deps: ['jquery']
        }
    },
    deps: ['app']
});

require([
    "app",
    "filter",
    "directive",
    "bootstrap",
    "loginCtrl",
    "logoutCtrl",
    "aboutCtrl",
    "userCtrl",
    "clientCtrl",
    "componentCtrl",
    "applicationCtrl",
    "runDetailCtrl",
    "appComponentCtrl",
    "inputLocationCtrl",
    "outputLocationCtrl",
    "applicationFileCtrl",
    "fileTransferCtrl",
    "uploadFileCtrl",
    "emailCtrl",
    "accountCtrl",
    "authInterceptorService"
],

    function (app) {
        //bootstrapping app
        app.init();
    }
);