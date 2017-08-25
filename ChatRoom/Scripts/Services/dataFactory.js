
chatApp.factory('dataFactory', ['$http', '$upload','$state','$rootScope',
function ($http, $upload,$state,$rootScope) {
            var dataFactory = {};
            //用户接口
            dataFactory.User = {
                GetAllFirends:function () {
                    return $http.post('/api/User/GetAllOnlineFirends');
                },
                GetUserDetail : function (id) {
                    return $http.post('/api/User/GetUserDetailInfo?userId=' + id);
                },
                Register : function (data) {
                    var request = $http({
                        method: 'POST',
                        url: '/api/User/Register',
                        data: data
                    });
                    return request;
                },
                Login : function (data) {
                    var request = $http({
                        method: 'POST',
                        url: '/api/User/Login',
                        data:data
                    });
                    return request;
                },
                LoginOut : function () {
                    return $http.post('/api/User/LoginOut');
                }
            };
            //分组接口
            dataFactory.Group=
            {
                GetAllGroups:function () {
                    var request = $http({
                        method: 'POST',
                        url: '/api/Group/GetAllGroups'
                    });
                    return request;
                }
            }
            //授权接口
            dataFactory.Auth = {
                AuthConnection: function() {
                    return $http.post('/api/Auth/AuthConnection');
                }
            };
            //工具类
            dataFactory.upload = function(name, file) {
                return $upload.upload({
                    url: '/api/fileUpload/upload',
                    fields: { 'username': name },
                    file: file
                });
            }
            //消息历史接口
            dataFactory.Message = {
                GetMessages: function() {
                    return $http.post('/api/Message/GetMessages');
                },
                GetGroupChatContext: function () {
                    return $http.post('/api/Message/GetGroupChatContext');
                }
            };
            return dataFactory;
        }
]);