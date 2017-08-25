//获取hub链接
var connection = $.connection.hub;
//打印Log开关
connection.logging = true;
//出现网络异常监听事件
connection.connectionSlow(function () {
    console.log("connectionSlow");
});
//断开链接监听事件
connection.disconnected(function () {
    console.log("disconnected");
});
//链接出现错误监听事件
connection.error(function (error) {
    console.error(error);
});
//重新链接成功监听事件
connection.reconnected(function () {
    console.log("reconnected");
});
//正在重新链接监听事件
connection.reconnecting(function () {
    console.log("reconnecting");
});
//正在链接开始监听事件
connection.starting(function () {
    console.log("starting");
});
//注册状态更改监听事件
connection.stateChanged(function (state) {
    console.log("stateChanged " + state.oldState + " => " + state.newState);
});

//获取指定的hub
var hub = $.connection.chatHub;
//注册客户端事件响应程序
hub.client.sendMessageTest = function (dd) {
    alert(dd);
};
//创建供后端调用的回调函数
connection.start().done(function () {
    $('#test').click(function () {
        hub.server.test();
    });
    alert('链接成功！');
}).fail(function() {
    alert('链接失败！');
});
