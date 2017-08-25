
chatApp.controller('loginController', ['dataFactory', '$scope', '$rootScope','$state','chatBuiness',
        function (dataFactory, $scope, $rootScope, $state, chatBuiness) {
            $scope.title = "login";
            $scope.Name = '';
            $scope.edit = "";
            $scope.currentUser = {};

            $scope.ToLogin = function() {
                $state.go('Login');
            }
            $scope.ToRegister = function() {
                $state.go('Register');
            }

            $scope.Login = function () {
                dataFactory.User.Login($scope.userfrom).then(function(response) {
                    if (response.data.State) {
                        chatBuiness.InitChatRoom();
                    } else {
                        alert(response.data.Message);
                    }
                });
            }
        }]);

chatApp.controller('registerController', ['dataFactory', '$scope', '$rootScope', '$state',
    function (dataFactory, $scope, $rootScope, $state) {
        $scope.currentUser = {};
        $scope.ToLogin = function () {
            $state.go('Login');
        }
        $('.edit_icon').click(function() {
            $('.login_file_image').click();
        });
        $scope.upload = function (file) {
            if (file) {
                dataFactory.upload($scope.userinfo.Name, file).then(function (response) {
                    $scope.message.Content = response.data;
                    $scope.fileStyle = { "background-color": "#A9F5A9" }
                }, function (error) {
                    console.log(error);
                    $scope.fileStyle = { "background-color": "#F5A9A9" }
                });
            }
        };
        $scope.Register = function () {
            if (!$scope.userregisterfrom.UserName) {
                alert('必须填写用户名。');
                return;
            }
            if (!$scope.userregisterfrom.Email) {
                alert('必须填写Email。');
                return;
            }
            if (!$scope.userregisterfrom.Sex) {
                alert('必须填写选择性别。');
                return;
            }
            if (!$scope.userregisterfrom.Password) {
                alert('必须填写密码。');
                return;
            }
            if (!$scope.userregisterfrom.ConfirmPassword) {
                alert('必须填写确认密码。');
                return;
            }
            if ($scope.userregisterfrom.ConfirmPassword != $scope.userregisterfrom.Password) {
                alert('密码不一致。');
                return;
            }
            return dataFactory.User.Register($scope.userregisterfrom).then(function (response) {
                if (response.data.State) {
                    $state.go('Login');
                } else {
                    alert(response.data.Message);
                }
            }, function (error) {
                console.log(error);
            });
        }
    }]);