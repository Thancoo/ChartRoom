var chatApp = angular.module('chatApp', [
        'ngCookies',
        'angularFileUpload',
        'ngSanitize',
        'angularMoment',
        'ngRoute',
        'ui.router'
]);
chatApp.run(['$rootScope', 'chatBuiness', 'chatHub', '$state', function ($rootScope, chatBuiness) {
    //记录ui-router的错误！
    $rootScope.$on('$stateChangeError',
        function (event, toState, toParams, fromState, fromParams, error) {
            console.error({ 'EventType': '$stateChangeError', 'From': fromState, 'To': toState, 'Error': error });
        });
    //记录改变视图的信息；
    //$rootScope.$on('$stateChangeStart',
    //    function(event, toState, toParams, fromState, fromParams, options) {
    //        console.info({ 'EventType':'$stateChangeStart','From': fromState, 'To': toState});
    //    });
    //记录找不到视图的错误！
    $rootScope.$on('$stateNotFound',
        function(event, unfoundState, fromState, fromParams) {
            console.error({ 'EventType': '$stateNotFound', 'From': fromState, 'To': unfoundState.to });
        });
    ////记录成功唤起的信息。
    //$rootScope.$on('$stateChangeSuccess',
    //    function(event, toState, toParams, fromState, fromParams) {
    //        console.info({ 'EventType': '$stateChangeSuccess', 'From': fromState, 'To': toState });
    //    });
    chatBuiness.InitChatRoom();
}]);