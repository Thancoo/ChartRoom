
chatApp.factory('chatHub', ['$http', '$rootScope', function ($http, $rootScope) {
    var chatFactory = {};
    chatFactory.RegisterHubEventListener= function() {
        //获取hub链接
        chatFactory.connection = $.connection.hub;
        //打印Log开关
        chatFactory.connection.logging = true;
        //出现网络异常监听事件
        chatFactory.connection.connectionSlow(function () {
            console.log("connectionSlow");
        });
        //断开链接监听事件
        chatFactory.connection.disconnected(function () {
            console.log("disconnected");
            if ($rootScope.UserState == 'online') {
                console.log("用户已然在线，开始链接重试！");
                chatFactory.connect().then(function() {
                    console.log("重连成功！");
                });
            }
        });
        //链接出现错误监听事件
        chatFactory.connection.error(function (error) {
            console.error(error);
        });
        //重新链接成功监听事件
        chatFactory.connection.reconnected(function () {
            console.log("reconnected");
        });
        //正在重新链接监听事件
        chatFactory.connection.reconnecting(function () {
            console.log("reconnecting");
        });
        //正在链接开始监听事件
        chatFactory.connection.starting(function () {
            console.log("starting");
        });
        //注册状态更改监听事件
        chatFactory.connection.stateChanged(function (state) {
            console.log("stateChanged " + state.oldState + " => " + state.newState);
        });
    }
    chatFactory.connect = function () {
        return chatFactory.connection.start();
    }

    chatFactory.stop = function () {
        return chatFactory.connection.stop();
    }
    chatFactory.Send = function (data) {
        return $.connection.chatHub.server.sendMessage(data);
    }
    chatFactory.Reciver = function (callback) {
        $.connection.chatHub.client.broadcastMessage = function (data) {
            callback(data);
        }
    }
    chatFactory.HeartBeatScheduler = function () {
        return $http.post('/api/Auth/HeartBeatAuth');
    }
    chatFactory.BeginHeardBeatAuth = function (callback) {
        if (window.customerData == undefined)
            window.customerData = {};
        window.customerData.HeardBeatInterval=setInterval(function() {
            chatFactory.HeartBeatScheduler().then(function (response) {
                if (response.data.State) {
                    //chatFactory.connection.state
                }
                callback && callback(response);
            });
        },1000*60*2);
    }
    chatFactory.StopHeardBeatAuth = function () {
        if (!window.customerData.HeardBeatInterval)
            return;
        clearInterval(window.customerData.HeardBeatInterval);
    }
    return chatFactory;
}]);

