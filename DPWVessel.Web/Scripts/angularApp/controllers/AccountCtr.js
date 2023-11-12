
'use strict';
app.controller('AccountCtr',
    [
        "$scope",
        "$rootScope",
        "$timeout",
        "$q",
        "urlService",
        "newGridService",
        "$window",
        function($scope, $rootScope, $timeout, $q, urlService, newGridService, $window) {
           $scope.UserAppDiv=false;
           $scope.init = function ()
           {
                GetRoles();
                GetUsersAppList();
                $scope.UserAppDiv = false;
            }
         
            //$scope.Register;
            var roles = {};
            $scope.Org = {};
            $scope.User = {};
            $scope.checkrole = function (data)
            {
            
                if(data!=1)
                {
                    $scope.UserAppDiv = true;
                }
                else
                {
                    $scope.UserAppDiv = false;
                }
            }

            function GetRoles()
            {
                $scope.ajaxGet('api/AccountApi/GetUserRole', null, function (response) {
                    console.log(response);
                    if (response.Success) {
                        $scope.Roles = response.Options;

                    }
                    else {
                        toastr.error('');
                    }
                });
            }
            function GetUsersAppList()
            {
                
                $scope.ajaxGet('api/UsersApi/GetUsersApplicationListRequest', null, function (response) {
                    console.log(response);
                    console.log("Response Up...");
                    if (response.Success) {
                        $scope.AppList = response.UsersApplicationList;

                    }
                    else {
                        toastr.error('');
                    }
                });
            }
            //int edit start
              $scope.initEdit = function () {
          
                // GetEqTypes();
                  GetRoles();
                  GetUsersAppList();
                var id = urlService.getUrlPrams();

                $scope.ajaxGet('api/AccountApi/getUserDetails', { Id: id.Id }, function (resp) {

                    if (resp.Success) {
                    
                        $scope.data = resp;
                        $scope.User = resp;
                        console.log($scope.User);

                        if ($scope.User.UsersApplication.includes('1')) {
                            $scope.RolesDiv = ['1'];
                        }
                        else {
                            if ($scope.User.UsersApplication.includes('2')) {
                                $scope.RolesDiv = ['2'];
                            }
                            else {
                                $scope.RolesDiv = [];
                            }
                        }
                        if ($scope.User.UsersApplication.includes('2')) {
                            $scope.RolesDiv = ['2'];
                        }
                        else {
                            if ($scope.User.UsersApplication.includes('1')) {
                                $scope.RolesDiv = ['1'];
                            }
                            else {
                                $scope.RolesDiv = [];
                            }
                        }
                        //Remaing Work
                        if ($scope.User.UsersApplication.includes('3')) {
                            $scope.RolesDiv = ['3'];
                        }
                        //Remaing Work End
                        if ($scope.User.UsersApplication.includes('1') && $scope.User.UsersApplication.includes('2') && $scope.User.UsersApplication.includes('3')) {
                            $scope.RolesDiv = ['1', '2','3'];
                        }

                    
                        $(document).ready(function () {

                            var o = new SlimSelect({
                                select: '#UserAppId',
                                placeholder: 'Select Application Access',
                                allowDeselectOption: true,
                                hideSelectedOption: true
                                //deselectLabel: '<span class="red">✖</span>',
                            });

                            var v = new SlimSelect({
                                select: '#UserRoleId',
                                placeholder: 'Select Role',
                                allowDeselectOption: true,
                                hideSelectedOption: true
                                //deselectLabel: '<span class="red">✖</span>',
                            });

                            
                        });
                      
                    }
                });

              }
              
            //int edit end
            $scope.RolesDi;
            //Check App Access
             $scope.CheckAccess = function (data)
            {
                
                 console.log(data,"Check ");
                //$("#test").val($scope.Register.UserType).trigger("change");

                if(data.includes('1'))
                {
                    $scope.RolesDiv = ["1"];
                  
                }
                else
                {
                    if ($scope.RolesDiv!=undefined && $scope.RolesDiv.includes('2'))
                    {
                        $scope.RolesDiv = ["2"];
                        
                    }
                    else
                    {
                       
                        $scope.RolesDiv = [];
                       
                    }
                    
                }
                if (data.includes('2'))
                {
                    $scope.RolesDiv = ["2"];
                    
                }
                else
                {
                    if ($scope.RolesDiv.includes('1')) {
                        $scope.RolesDiv = ["1"];
                        
                        
                    }
                    else {
                        $scope.RolesDiv = [];
                       
                    }
                }
                if (data.includes('3')) {
                    $scope.RolesDiv = ["3"];

                }
                if (data.includes('1') && data.includes('2')) {
                    $scope.RolesDiv = ["1", "2"];

                }
                if (data.includes('1') && data.includes('3')) {
                    $scope.RolesDiv = ["1", "3"];

                }
                if (data.includes('2') && data.includes('3')) {
                    $scope.RolesDiv = ["2", "3"];

                }
                if (data.includes('1') && data.includes('2') && data.includes('3'))
                {
                    $scope.RolesDiv = ["1", "2","3"];
                   
                }
              
             }
             $scope.CheckAccessEdit = function (data) {

                 if (data == null || data.length == 0) {
                     $scope.RolesDiv;
                    
                 }

                 //$("#test").val($scope.Register.UserType).trigger("change");

                 if (data.includes('1')) {
                     $scope.RolesDiv = ["1"];

                 }
                 else {
                     if ($scope.RolesDiv.includes('2')) {
                        
                         $scope.RolesDiv = ["2"];
                        

                     }
                     else {
                         $scope.RolesDiv = [];
                        
                     }

                 }
                 if (data.includes('2')) {
                     $scope.RolesDiv = ["2"];
                    
                 }
                 else {
                     if ($scope.RolesDiv.includes('1')) {
                         $scope.RolesDiv = ["1"];
                        
                     }
                     else {

                         $scope.RolesDiv = [];
                         
                     }
                 }
                 //Remaing Work
                 if (data.includes('3')) {
                     $scope.RolesDiv = ["3"];

                 }
                 //End
                 if (data.includes('1') && data.includes('2')) {
                     $scope.RolesDiv = ["1", "2"];

                 }
                 if (data.includes('1') && data.includes('3')) {
                     $scope.RolesDiv = ["1", "3"];

                 }
                 if (data.includes('2') && data.includes('3')) {
                     $scope.RolesDiv = ["2", "3"];

                 }
                 if (data.includes('1') && data.includes('2') && data.includes('3')) {
                     $scope.RolesDiv = ["1", "2","3"];

                 }

             }
            //Check App Access End

            //Check Register Role Limit  Start
             $scope.RoleCount = function (Roles)
             {
                 console.log(Roles);
                 var rlength = Roles.length;
                 console.log(rlength);
                 
                     if (Roles.includes('1') || Roles.includes('2'))
                     {
                         if (rlength > 1)
                         {
                             toastr.error('only one Common role allows!');
                            
                             return false;
                         }
                     }
                 else
                     {
                         var App = $scope.Register.UsersApplication;
                         if(App.length!=0)
                         {
                             
                             if(App.includes('1'))
                             {
                                 
                                     console.log(Roles);
                                     if(Roles.includes('4')||Roles.includes('6') || Roles.includes('7') || Roles.includes('8') || Roles.includes('9'))
                                     {
                                        
                                             if (App.includes('2')) {
                                                 if (Roles.includes('3')) {
                                                      if (rlength > 2)
                                                      {
                                                          toastr.error('only one role allows for JIP');
                                                          return false;
                                                      }
                                                 }
                                                 else
                                                 {
                                                     if (rlength > 1) {
                                                         toastr.error('only one role allows for JIP');
                                                         return false;
                                                     }
                                                 }
                                             }
                                             else
                                             {
                                                 if (rlength > 1) {
                                                     toastr.error('only one role allows for JIP');
                                                     return false;
                                                 }
                                             }
                                         
                                        
                                        
                                     }
                                 
                                 
                             }
                         }
                     }
                 
             }
            //Check Register Role Limit End

            //Check Edit Role Limit  Start
             $scope.RoleCountEdit = function (Roles) {
                 console.log(Roles);
                 var rlength = Roles.length;
                 console.log(rlength);

                 if (Roles.includes('1') || Roles.includes('2')) {
                     if (rlength > 1) {
                         toastr.error('only one Common role allows!');
                         return false;
                     }
                 }
                 else {
                     var App = $scope.User.UsersApplication;
                     if (App.length != 0) {

                         if (App.includes('1')) {

                             console.log(Roles);
                             if (Roles.includes('4') || Roles.includes('6') || Roles.includes('7') || Roles.includes('8') || Roles.includes('9')) {

                                 if (App.includes('2')) {
                                     if (Roles.includes('3')) {
                                         if (rlength > 2) {
                                             toastr.error('only one role allows for JIP');
                                             return false;
                                         }
                                     }
                                     else {
                                         if (rlength > 1) {
                                             toastr.error('only one role allows for JIP');
                                             return false;
                                         }
                                     }
                                 }
                                 else {
                                     if (rlength > 1) {
                                         toastr.error('only one role allows for JIP');
                                         return false;
                                     }
                                 }



                             }


                         }
                     }
                 }

             }
            //Check Edit Role Limit End

            // update user start
             
            $scope.SaveUser = function (User) {

              
                var id = urlService.getUrlPrams();
              
                console.log(User);
         
                //var params = {
                //    Id: parseInt(id.Id),
                //    UserId: parseInt(id.Id),
                //    FullName: User.FullName,
                //    Phone: User.Phone,
                //    Status: User.Status,
                //    UserType: User.UserTypeId,
                //    UsersApplicationEditReq: User.UsersApplication,
                //};
              
    
                if ($scope.RoleCountEdit(User.UserType) !=false)
                {
                    $scope.ajaxPost('api/AccountApi/UserInformationUpdate', { data: User }, function (response) {
                        if (response.Success) {
                            toastr.success("Updated Successfully");
                            $timeout($window.location.href = '/Users/Index', 2000);
                        }
                        else {
                            toastr.error('Error in Update');
                        }
                    });
                }
             

            }
            // end update user
            $scope.ImageUpload = function (fileuploadid, property_to_update) {
                $scope.ImageUploadPromise(fileuploadid).then(
                    function (response) {
                        $timeout(function () {
                            $scope.Register[property_to_update] = response;
                        }, 100);
                    });
            }
          
            $scope.RegisterUser = function (Register) {
               
                console.log(Register);
                if ($scope.RoleCount(Register.UserType) == false)
                {
                    return false;
                }
                if (Register == null || Register.Email == null || Register.FullName ==null || Register.Phone ==null || Register.UserType ==null|| Register.Status==null)
                {
                    toastr.error("Please fill all required fields");
                    return false;
                }
                if (Register.UsersApplication == null)
                {
                    if (Register.UserType != 1)
                    {console.log(Register.UserType);
                         toastr.error("Please Select User Application Access!");
                          return false;
                    }
                }
             
                $scope.ajaxPost('api/AccountApi/Register', Register, function (response) {
                    console.log(response);
                    if (response.Success) {
                        toastr.success('User registered successful');
                        $timeout(() => { $window.location.href = '/Users' }, 2000);
                        //$timeout($window.location.href = '/account/RegisterUser', 5000);
                    }

                });
            }

            $scope.Change = function () {
                $scope.open = "true";
                if ($scope.Register.UserType == 2 || $scope.Register.UserType == 3 || $scope.Register.UserType == 4 || $scope.Register.UserType == 6) {
                    GetOrganization();
                   
                    $scope.ut = $scope.Register.UserType;
                } else {
                    $scope.organization = "";
                    $scope.suppliers = "";
                    $scope.locations = "";
                    $scope.departments = "";
                }
                if ($scope.Register.UserType == 5) {
                    GetSupplier();
                    $scope.ut = $scope.Register.UserType;
                } else {
                    $scope.organization = "";
                    $scope.suppliers = "";
                    $scope.locations = "";
                    $scope.departments = "";
                }
            }
            $scope.Enable = function () {
                $scope.open = "true";
                if (($scope.ut == 3 || $scope.ut == 4 || $scope.ut == 6) && $scope.Register.OrganizationId) {

                    GetLocation($scope.Register.OrganizationId);
                    GetDepartment($scope.Register.OrganizationId);

                } else {
                    $scope.locations = "";
                    $scope.departments = "";
                }
            }

            
            $scope.ChangePassword = function (ChangeP) {
                console.log(ChangeP);
                if (ChangeP === undefined) {
                    toastr.error("Please Fill the require Fields");
                    return false;
                }

                if (ChangeP.OldPassword != null && ChangeP.NewPassword && ChangeP.ConfirmPassword) {
                    $scope.ajaxPost('api/AccountApi/ChangePassword', ChangeP, function (response) {
                        console.log(response);
                        if (response.Succeeded) {
                            toastr.success("Password Reset Successfully");
                        }
                    })
                } else {
                    toastr.error("Please Fill the require Fields");
                }

            }



            $scope.InitEditProfile = function () {

                $scope.ajaxGet('api/AccountApi/GetUserProfileDetails', null, function (response) {
                    console.log(response);
                    if (response.Success) {
                        $scope.Contracts = response;
                        console.log($scope.Contracts);
                    }


                })
            }

            $scope.EditProfile = function (profile) {
                console.log(profile);
                if (profile == null && profile == undefined) {
                    toastr.error($scope.GetLanguageContent("all_fields_required"));
                    return false;
                }

                $scope.ajaxPost('api/AccountApi/UpdateProfile', profile, function (response) {
                    console.log(response);
                    if (response.Success) {
                        toastr.success($scope.GetLanguageContent('update_successful'));
                        $timeout(() => { $window.location.href = '/Home' }, 2000);
                    }
                    else {
                        toastr.error($scope.GetLanguageContent('Edit_update_failed'));
                    }
                });
            }   
            $scope.SelectAll = function (selectall) {
                console.log(selectall);
                console.log($scope.Register.SelectedLocation);
                $scope.Register.SelectedLocation=
                console.log();
            }
        }



    ]);
