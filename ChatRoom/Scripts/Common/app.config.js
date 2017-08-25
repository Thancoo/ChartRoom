chatApp.config(['$stateProvider', '$urlRouterProvider', '$filterProvider', function ($stateProvider, $urlRouterProvider, $filterProvider) {
    $stateProvider
        .state('Message',
            {
                url: "Message/Index",
                templateUrl: '../Message/Message.html',
                controller: 'messageController'
            })
        .state('Message.Index',
            {
                url: "Message/Index",
                templateUrl: '../Message/Message.html',
                controller: 'messageController'
            })
        .state('Login',
            {
                url: "User/Login",
                templateUrl: function () {
                    return '../User/login.html';
                },
                controller: 'loginController'
            })
        .state('Register',
            {
                url: "User/Register",
                templateUrl: '../User/Register.html',
                controller: 'registerController'
            });
    $urlRouterProvider.otherwise('/User/Login');
    $filterProvider.register('name', function(value) {
        return function() {

        };
    });
}]);