chatApp.directive("session-show", function() {
    return {
        restrict: 'E',
        scope: {
            message: "=",
            userinfo: "="
        },
        template: function () {
            var oo = [];
            return oo.join(',');
        },
        link: function(scope, ele,attr) {
            //跟据消息的内容来产生结果
        }
    };
})