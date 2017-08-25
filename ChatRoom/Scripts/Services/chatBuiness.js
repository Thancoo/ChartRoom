chatApp.factory('chatBuiness', ['$http', '$rootScope', '$state', 'chatHub', 'dataFactory',
    function ($http, $rootScope, $state, chatHub, dataFactory) {
        $rootScope.GroupList = [];//群列表，目前不支持群创建
        $rootScope.FriendList = [];//好友列表，暂时不开放
        $rootScope.GroupFriendList = {};//群成员信息表,以{groupId:{userId:{name:"name",headImg:"img"}}}的形式储存。
        $rootScope.UserState = "offline";
        $rootScope.UserInfo = {};
        $rootScope.Messages = {};//以{GroupId:[{message}]}的形式储存
        $rootScope.ChatSessionContext = {};
        //上下线事件通知
        var server = {};
        server.InitChatRoom = function () {
            dataFactory.Auth.AuthConnection().then(function (response) {
                if (response.data.State && response.data.StateCode === 1000) {
                    $rootScope.UserState = 'online';
                    dataFactory.User.GetAllFirends().then(function (rep) {
                        if (rep.data.State)
                            $rootScope.FriendList = rep.data.Data;
                        else
                            alert(rep.data.Message);
                    });
                    dataFactory.User.GetUserDetail(parseInt(response.data.Data)).then(function (rep) {
                        if (rep.data) {
                            $rootScope.UserInfo = rep.data;
                        } else {
                            console.log(rep);
                            alert('获取用户详情接口调用失败！');
                        }
                    });
                    dataFactory.Message.GetGroupChatContext().then(function (rep) {
                        if (rep.data.State) {
                            $rootScope.GroupList = rep.data.Data;
                            $rootScope.ChatSessionContext = $rootScope.GroupList[0];
                            rep.data.Data.forEach(function(itm) {
                                if ($rootScope.GroupFriendList[itm.Id] == undefined)
                                    $rootScope.GroupFriendList[itm.Id] = {};
                            });
                        } else
                            alert(rep.data.Message);
                    });
                    //心跳检测
                    chatHub.BeginHeardBeatAuth(function (re) {
                        if (re.status !== 200 || !re.data.State)
                            $rootScope.UserState = "offline";
                        else
                            $rootScope.UserState = "online";
                        $rootScope.$broadcast('HeardBeatAuthEvent', re.data);
                    });
                    chatHub.RegisterHubEventListener();
                    chatHub.Reciver(function (dt) {
                        if (!$rootScope.Messages[dt.RelayToId])
                            $rootScope.Messages[dt.RelayToId] = [];
                        $rootScope.Messages[dt.RelayToId].push(dt);
                        $rootScope.$applyAsync();
                        if (dt.RelayFromId&&dt.RelayFromId !== $rootScope.UserInfo.Id && dt.RelayToId) {
                            if (!$rootScope.GroupFriendList[dt.RelayToId])
                                $rootScope.GroupFriendList[dt.RelayToId] = {};
                            if (!$rootScope.GroupFriendList[dt.RelayToId][dt.RelayFromId]) {
                                dataFactory.User.GetUserDetail(dt.RelayFromId).then(function(__rep) {
                                    if (__rep != undefined && __rep.status === 200) {
                                        $rootScope.GroupFriendList[dt.RelayToId][dt.RelayFromId] = __rep.data;
                                    } else {
                                        console.warn(["用户信息获取异常", "UserId", dt.RelayFromId, "GroupId", dt.RelayToId, "Respose", __rep]);
                                    }
                                });
                            }
                        }
                        $rootScope.$broadcast('MessageEvent', dt);
                    });
                    chatHub.connect().done(function () {
                        //Do something On Connectioning.
                    });
                    $state.go('Message');
                } else {
                    $state.go('Login');
                }
            }
            );
        }
        return server;
    }])