define(['app'], function (app) {
    //defining service using factory method
    app.factory('appComponentService', function ($http, utility) {
        var serviceurl = utility.baseAddress + "AppComponent/";
        return {
            getAppComponentList: function (applicationId) {
                var url = serviceurl + "GetApplication?applicationId=" + applicationId;
                return $http.get(url);
            },
            getAppComponent: function (entity) {
                var url = serviceurl + "Get/" + entity.ApplicationComponentId;
                return $http.get(url);
            },
            addAppComponent: function (entity) {
                var url = serviceurl + "Post";
                return $http.post(url, entity);
            },
            updateAppComponent: function (id, entity) {
                var url = serviceurl + "Put/" + id;
                return $http.put(url, entity);
            },
            deleteAppComponent: function (entity) {
                var url = serviceurl + "Delete/" + entity.ApplicationComponentId;
                return $http.delete(url);
            },
            inputLocation: function (appId,compId) {
                var url = serviceurl + "InputLocations?appId=" + appId + "&compId=" + compId;
                return $http.get(url);
            },
            addInputLocation: function (entity) {
                var url = serviceurl + "AddInputLocation";
                return $http.post(url, entity);
            },            
            deleteInputLocation: function (entity) {
                var url = serviceurl + "DeleteInputLocation/" + entity.ComponentInputLocationId;
                return $http.delete(url);
            },
            outputLocation : function(appId,compId)
            {
                var url = serviceurl + "OutputLocations?appId=" + appId + "&compId=" + compId;
                return $http.get(url);
            },
            addOutputLocation: function (entity) {
                var url = serviceurl + "AddOutputLocation";
            return $http.post(url, entity);
            },
            deleteOutputLocation: function (entity) {
                var url = serviceurl + "DeleteOutputLocation/" + entity.ComponentOutputLocationId;
                return $http.delete(url);
            }
        };
    });
});