var wileblue = false;
chatApp.controller('messageController', ['dataFactory', '$scope', '$routeParams', 'chatHub','$rootScope','$state','$filter',
        function (dataFactory, $scope, $routeParams, chatHub, $rootScope, $state, $filter) {
            $scope.title = "Home";
            $scope.heardImageClicked = false;
            $scope.messages = [];
            $scope.message = {
                RelayFromId: $rootScope.UserInfo.Id,
                RelayToId: $rootScope.ChatSessionContext.Id,
                EventType: 'chat',
                MsgType: "group",
                ContentType: "text",
                Content: null
            };
            $scope.pre_message = angular.copy($scope.message);
            //表情控件初始化
            $('#chat_enjoy').qqFace({
                id: 'chat_enjoy_cnt',
                path: '../../Images/emjoy/',
                assign: 'container_input',
                tip: 'emj_'
            });
            //滚动条初始化
            $('.heard_image').click(function (e) {
                if (wileblue) {
                    e.stopPropagation();
                    return;
                }
                $('.heard_hover_action_list').show();
                wileblue = true;
                e.stopPropagation();
            });
            $(document).on('click', "*", function (e) {
                if (!wileblue) {
                    return;
                }
                if ($('.heard_hover_action_list').length>0 && (e.pageX > $(".heard_hover_action_list").offset().left + parseInt($(".heard_hover_action_list").css("width").substring(0, $(".heard_hover_action_list").css("width").length - 2)) || e.pageX < $(".heard_hover_action_list").offset().left || e.pageY > $(".heard_hover_action_list").offset().top + parseInt($(".heard_hover_action_list").css("height").substring(0, $(".heard_hover_action_list").css("height").length - 2)) || e.pageY < $(".heard_hover_action_list").offset().top)) {
                    $('.heard_hover_action_list').hide();
                    wileblue = false;
                }
                e.stopPropagation();
            });
            $('.chat_image').click(function (e) {
                e.stopPropagation();
                $('.hide_file_ele').click();
            });
            $('.chat_cate_list').off('click', '.list-group-item');
            $('.chat_cate_list').on('click', '.list-group-item', function (eve) {
                var _this = $(this);
                $('.chat_cate_list>.list-group-item').each(function () {
                    if ($(this).is(_this)) {
                        if(!$(this).hasClass('cate_selected'))
                            $(this).addClass('cate_selected');
                    } else {
                        if ($(this).hasClass('cate_selected'))
                            $(this).removeClass('cate_selected');
                    }
                });
            });
            $scope.PasteEncode = function(event) {
                console.log(event);
                console.log($scope.$event);
                return false;
            };
            $scope.ChangeChatContext = function (type, id) {
                $rootScope.ChatSessionContext = $rootScope.GroupList.filter(function (itm) {
                    return itm.Id === id;
                })[0];
                $scope.message.MsgType = type;
                $scope.message.RelayToId = id;
            }
            $scope.newMessage = function () {
                if ($rootScope.UserInfo.UserType === "visitor") {
                    alert("游客无法群聊，请先注册！");
                    return;
                }
                if ($('#container_input').html().length > 0) {
                    $scope.message.RelayFromId = $rootScope.UserInfo.Id;
                    $scope.message.RelayToId = $rootScope.ChatSessionContext.Id;
                    $scope.message.Content = $filter('EncodeEnjoy')($('#container_input').html());
                    chatHub.Send(angular.copy($scope.message));
                    $scope.message.Content = '';
                    $('#container_input').html("");
                } else {
                    alert("发送内容不可为空！");
                }
            }

            $scope.upload = function (file) {
                if (file) {
                    dataFactory.upload($rootScope.UserInfo.Id, file).then(function (response) {
                        var mes=angular.copy($scope.message);
                        mes.RelayFromId = $rootScope.UserInfo.Id;
                        mes.RelayToId = $rootScope.ChatSessionContext.Id;
                        mes.Content = response.data;
                        mes.ContentType = "image";
                        chatHub.Send(mes);
                        $scope.fileStyle = { "background-color": "#A9F5A9" }
                    }, function (error) {
                        console.log(error);
                        $scope.fileStyle = { "background-color": "#F5A9A9" }
                    });
                }
            };
            $scope.LoginOut = function () {
                dataFactory.User.LoginOut().then(function (response) {
                    if (response.status === 200 && response.data.State) {
                        $state.go('Login');
                        $rootScope.UserState = 'offline';
                        chatHub.StopHeardBeatAuth();
                    } else {
                        $state.go('Login');
                        $rootScope.UserState = 'offline';
                        chatHub.StopHeardBeatAuth();
                    }
                });
            }
            $rootScope.$on('FirendLoginEvent',
                function(event,data) {
                    //处理用户上下线通知
                });
            $rootScope.$on('MessageEvent',
                function(event,args) {
                    //处理消息接收事件
                    var grpct = $rootScope.GroupList.filter(function (itm) { return itm.Id === args.RelayToId; })[0];
                    grpct.LastChatTime = args.CreatedOn;
                });
            $rootScope.$on('HeardBeatAuthEvent', function (event,data) {
                //处理登录状态推送
               
            });
        }]);