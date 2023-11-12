'use strict';
app.controller('UsersCtr',
    [
        "$scope",
        "$rootScope",
        "$timeout",
        "$q",
        "urlService",
        "newGridService",
        "$window",
        function ($scope, $rootScope, $timeout, $q, urlService, newGridService, $window) {
            var url = urlService.getUrlPrams();
   
            $scope.init = function () {

                $scope.ajaxGet('api/UsersApi/GetUsersListRequest', null, function (resp) {
                    
                    console.log(resp);
                    if (resp.Success) {
                        $scope.UserList = resp.UsersList;
                        GetRoles();
                        $scope.AppList();
                    }
                });

            
             
            }
            $scope.AppList =function (data)
            {
                
                $scope.ajaxGet('api/UsersApi/GetApplicationList', null, function (resp) {

                    console.log(resp);
                    if (resp.Success) {
                        $scope.AppList = resp.ApplicationsList;
                       

                    }
                });
            }
            $scope.ResetHref =function (UserId)
            {
                    $window.open('/Users/ResetPassword?Id=' + UserId, '_self');
            }
            $scope.EditHref = function (UserId)
            { 
                    $window.open('/Users/Edit?Id=' + UserId, '_self');
            }
            $scope.initEdit = function () {
                
                GetRoles();
                var id = urlService.getUrlPrams();
          
               
                    $scope.ajaxGet('api/AccountApi/getUserDetails', { Id: id.Id }, function (resp) {
                        if (resp.Success) {
                            console.log("IntRequest");
                            console.log(resp);
                            $scope.data = resp;
                            $scope.User = resp;
                        }
                    }); //userdetails end
   
            
               }
           
      


            function GetRoles() {
                 
                $scope.ajaxGet('api/AccountApi/GetUserRole', null, function (response) {
                    console.log(response);
                    if (response.Success) {
                        $scope.Roles = response.Options;
                        console.log("Roles");
                        console.log($scope.Roles);

                    }
                    else {
                        toastr.error($scope.GetLanguageContent('Insertion_error'));
                    }
                })


            }

            $scope.resets = function () {
                window.location.href = 'index';
            }

                   $scope.ChangeRole = function (Id, UserType) {
                       console.log(UserType);
                       var params = {
                           Id: Id,
                           Role: UserType
                       };
                       $scope.ajaxPost('api/AccountApi/RoleChange', params, function (resp) {
                           if (resp.Success) {
                               // $scope.LocationInfo();
                               toastr.success("Role Change Successfully");
                               $window.location.reload();
                           }
                           else
                               toastr.error("Failed");

                       });

                   };



            $scope.ImageUpload = function (fileuploadid, property_to_update) {
                $scope.ImageUploadPromise(fileuploadid).then(
                    function (response) {
                        $timeout(function () {
                            $scope.User[property_to_update] = response;
                        }, 100);
                    });
            }

            $scope.ResetPassword = function (User) {
                User.Email = $scope.data.Email;
                User.Username = $scope.data.Username;
                $scope.ajaxPost('api/AccountApi/UsersPasswordResetRequest', User, function (response) {
                    if (response.Succeeded) {
                        toastr.success('Password Reset Successfully');
                        $timeout($window.location.href = '/Users', 3000);
                    }
                    else {
                        toastr.error('Password Is Not Reset');
                    }
                })
            }
          

        }]);
