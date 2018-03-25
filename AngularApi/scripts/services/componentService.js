define(['app'], function (app) {
    //defining service using factory method
    app.factory('componentService', function ($http, utility) {
        var serviceurl = utility.baseAddress + "Component/";
        return {
            getComponentList: function (pageNumber, pageSize, fetchAll) {
                var url = serviceurl + "Get?pageNumber=" + pageNumber + "&pageSize=" + pageSize + "&fetchAll=" + fetchAll;
                return $http.get(url);
            },
            getComponent: function (component) {
                var url = serviceurl + "Get/" + component.ComponentId;
                return $http.get(url);
            },
            addComponent: function (component) {
                var url = serviceurl + "Post";
                return $http.post(url, component);
            },
            updateComponent: function (id, component) {
                var url = serviceurl + "Put/" + id;
                return $http.put(url, component);
            },
            deleteComponent: function (component) {
                var url = serviceurl + "Delete/" + component.ComponentId;
                return $http.delete(url);
            },
            updateTriggerandStatus: function (entity) {
                var url = serviceurl + "AddUpdateTriggerandStatusFile";
                return $http.post(url, entity);
            }
        };
    });
});