'use strict';
app.controller('StudentsCtr',
    [
        "$scope",
        "$rootScope",
        "$timeout",
        "$q",
        "urlService",
        "newGridService",
        "$window",
        function ($scope, $rootScope, $timeout, $q, urlService, newGridService, $window) {
            //var url = urlService.getUrlPrams();

            $scope.init = function ()
            {
                console.log("Welcome Student Controller Anguler")
                GetStudents();
            }
            function GetStudents()
            {
                $scope.ajaxGet('api/StudentsApi/GetStudentsList', null, function (resp) {

                    console.log(resp);
                    if (resp.Success) {
                        $scope.StdList = resp.StudentsLists;
                    }

                });
            }
          
            $scope.FilterStudent = function (data) {
                console.log('Filtering',data);
                $scope.ajaxGet('api/StudentsApi/GetStudentsList',data, function (resp) {

                    
                    console.log('Filter Data ',resp);
                    if (resp.Success) {
                        $scope.StdList = resp.StudentsLists;
                    }

                });
            }
            $scope.AddStudent = function (data) {
                console.log(data);
                if (data == null || data == undefined || data == '') {
                    toastr.error('All filed is reqried');
                    return false;
                }
                if (data.name == null || data.name == undefined || data.name == '') {
                    toastr.error('Please Enter Name');
                    return false;
                }
                if (data.rollNo == null || data.rollNo == undefined || data.rollNo == '') {
                    toastr.error('Please Enter rollNo');
                    return false;
                }
                if (data.age == null || data.age == undefined || data.age == '') {
                    toastr.error('Please Enter age');
                    return false;
                }
                $scope.ajaxPost('api/StudentsApi/AddStudentRec', data, function (response) {
                    console.log(response)
                    if (response.IsTure == false) {
                        toastr.error(response.message)
                    } else { 
                   
                        toastr.success('Studenet registered successful');
                        $timeout(() => { $window.location.href = '/Students' }, 2000);
                        //$timeout($window.location.href = '/account/RegisterUser', 5000);
                    }

                });
            }
            $scope.EditStudent = function (StudData) {
                console.log(StudData);
                if (StudData == null || StudData == undefined || StudData == '') {
                    toastr.error('All filed is reqried');
                    return false;
                }
                if (StudData.name == null || StudData.name == undefined || StudData.name == '') {
                    toastr.error('Please Enter Name');
                    return false;
                }
                if (StudData.rollNo == null || StudData.rollNo == undefined || StudData.rollNo == '') {
                    toastr.error('Please Enter rollNo');
                    return false;
                }
                if (StudData.age == null || StudData.age == undefined || StudData.age == '') {
                    toastr.error('Please Enter age');
                    return false;
                }
                $scope.ajaxPost('api/StudentsApi/StudentsInformationUpdate', { data: StudData }, function (response) {
                    console.log(response)
                    if (response.IsTure == true) {
                        toastr.success("Updated Successfully");
                        $timeout($window.location.href = '/Students/Index', 2000);
                    }
                    else {
                        toastr.error(response.message);
                    }
                });
            }
            $scope.EditHref = function (sId) {
                $window.open('/Students/Edit?Id=' + sId, '_self');
            }

            $scope.initEdit = function (data) {
                var id = urlService.getUrlPrams();
                console.log(`Welcome edit init angular ID: ${id.Id}`)
                $scope.ajaxGet('api/StudentsApi/getStudentDetails', { Id: id.Id }, function (resp) {
                    if (resp.Success) {
                        console.log("IntRequest");
                        console.log(resp);
                        $scope.data = resp;
                        $scope.std = resp;
                    }
                })

            }

                
            
        }

     ]
);
