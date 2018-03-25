define(['app'], function (app) {
    //defining service using factory method
    app.factory('authorizationService', function ($http, $resource, $q, $rootScope, $location, $localStorage, utility, roles, routeForUnauthorizedAccess) {
      
        var userInfo = {
            isAuth: false,
            userName: "",
            userId: "",
            userType:""
        };

        return {
            // We would cache the permission for the session,
            //to avoid roundtrip to server
            //for subsequent requests            

            permissionModel: {
                permission: {},
                isPermissionLoaded: false
            },

            permissionCheck: function (roleCollection) {
                // we will return a promise .
                var deferred = $q.defer();

                //this is just to keep a pointer to parent scope from within promise scope.
                var parentPointer = this;

                //Checking if permission object(list of roles for logged in user) 
                //is already filled from service
                if (this.permissionModel.isPermissionLoaded) {
                    //Check if the current user has required role to access the route
                    this.getPermission(this.permissionModel, roleCollection, deferred);
                } else {
                    //if permission is not obtained yet, we will get it from  server.
                    // 'api/permissionService' is the path of server web service , used for this example.

                    var url = serviceurl + "GetPermissionList";

                    $resource(url).get().$promise.then(function (response) {
                        //when server service responds then we will fill the permission object
                        parentPointer.permissionModel.permission = response;

                        //Indicator is set to true that permission object is filled and 
                        //can be re-used for subsequent route request for the session of the user
                        parentPointer.permissionModel.isPermissionLoaded = true;

                        //Check if the current user has required role to access the route
                        parentPointer.getPermission(parentPointer.permissionModel, roleCollection, deferred);
                    });
                }
                return deferred.promise;
            },

            //Method to check if the current user has required role to access the route
            //'permissionModel' has permission information obtained from server for current user
            //'roleCollection' is the list of roles which are authorized to access route
            //'deferred' is the object through which we shall resolve promise
            getPermission: function (permissionModel, roleCollection, deferred) {
                var ifPermissionPassed = false;
                angular.forEach(roleCollection, function (role) {
                    switch (role) {
                        case roles.superUser:
                            if (permissionModel.permission.isSuperUser) {
                                ifPermissionPassed = true;
                            }
                            break;
                        case roles.admin:
                            if (permissionModel.permission.isAdministrator) {
                                ifPermissionPassed = true;
                            }
                            break;
                        case roles.user:
                            if (permissionModel.permission.isUser) {
                                ifPermissionPassed = true;
                            }
                            break;
                        default:
                            ifPermissionPassed = false;
                    }
                });

                if (!ifPermissionPassed) {
                    //If user does not have required access, 
                    //we will route the user to unauthorized access page
                    $location.path(routeForUnauthorizedAccess);
                    //As there could be some delay when location change event happens, 
                    //we will keep a watch on $locationChangeSuccess event
                    // and would resolve promise when this event occurs.
                    $rootScope.$on('$locationChangeSuccess', function (next, current) {
                        deferred.resolve();
                    });
                } else {
                    deferred.resolve();
                }
            },

            login: function login(loginData) {
                var data = "grant_type=password&username=" + loginData.userName + "&password=" + loginData.password;

                var deferred = $q.defer();
                
                $http.post('http://127.0.0.1:8001/token', data, { headers: { 'Content-Type': 'application/x-www-form-urlencoded' } })
                    .then(function (result) {
                        $localStorage.authorizationData = { token: result.data.access_token, userName: result.data.userName, type: result.data.role };
                       deferred.resolve(result);
                    }, function (error) {
                        deferred.reject(error);
                    });

                return deferred.promise;
            },

            logout: function logout() {
                userInfo.isAuth = false;
                userInfo.userName = "";
                 $localStorage.$reset();
                 $location.path('/')
            },


            getUserInfo: function getUserInfo() {
                var authData = $localStorage.authorizationData;
                if (authData) {
                    userInfo.isAuth = true;
                    userInfo.userName = authData.userName;
                    userInfo.userType = authData.type;
                }
                else
                    userInfo.userType = "header";
                return userInfo;
            }
        };
    });
});