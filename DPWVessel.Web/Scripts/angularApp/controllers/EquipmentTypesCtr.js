'use strict';
app.controller('EquipmentTypesCtr',
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
                console.log("Welcome EquipmentType Controller Anguler")
                GetEquipmentTypes();
            }

            function GetEquipmentTypes()
            {
                
                $scope.ajaxGet('api/EquipmentTypesApi/GetEquipmentTypesList',null, function (resp) {

                    console.log(resp);
                    if (resp.Success) {
                        $scope.EqtyList = resp.EquipmentTypesLists;
                        $timeout(function () {
                            initDataTable();
                        });
                    }

                });
            }
            function initDataTable() {
                // Assuming you have a table with id 'equipmentTable'
                $('#equipmenttypeTable').DataTable({
                    // Add DataTable options here
                    "paging": true,
                    "ordering": true,
                    "info": true,
                    "searching": true,
                    // Add more options as needed
                });
            }

            $scope.FilterEquipmentType = function (data) {
                console.log("ddata", data);
                if (data == undefined) {
                    data = {};
                }
                
                var st = $("#startDate").val();
                var et = $("#endDate").val();
                if (st != "" && st != undefined) {
                    var StartDate = moment(stringToDate($("#startDate").val(), "dd/MM/yyyy", "/")).format('YYYY-MM-DDT00:00:00');
                    $("#startDate").val('');

                }
                if (et != "" && et != undefined) {
                    var EndDate = moment(stringToDate($("#endDate").val(), "dd/MM/yyyy", "/")).format('YYYY-MM-DDT00:00:00');
                    $("#endDate").val('');
                }
                console.log(data);                                                                                             


                $scope.ajaxGet('api/EquipmentTypesApi/GetEquipmentTypesList',
                    {
                        id: data.id,
                        name: data.name,
                        startDate: StartDate,
                        endDate: EndDate
                    }
                    , function (resp) {
                        data = {}
                        console.log('Filter Data', resp);
                        if (resp.Success) {
                            $scope.EqtyList = resp.EquipmentTypesLists;
                        }

                    });
            }
            $scope.AddEquipmentType = function (data) {
                console.log(data);
                if (data == null || data == undefined || data == '') {
                    toastr.error('All filed is reqried');
                    return false;
                }
                if (data.name == null || data.name == undefined || data.name == '') {
                    toastr.error('Please Enter Name');
                    return false;
                }
              
                $scope.ajaxPost('api/EquipmentTypesApi/AddEquipmentTypesRec', data, function (response) {
                    console.log(response)
                    if (response.IsTure == false) {
                        toastr.error(response.message)
                    } else { 
                   
                        toastr.success('Records registered successful');
                        $timeout(() => { $window.location.href = '/EquipmentTypes' }, 2000);
                        //$timeout($window.location.href = '/account/RegisterUser', 5000);
                    }

                });
            }
            $scope.EditEquipmentType = function (Data) {
                console.log(Data);
                if (Data == null || Data == undefined || Data == '') {
                    toastr.error('All filed is reqried');
                    return false;
                }
                if (Data.name == null || Data.name == undefined || Data.name == '') {
                    toastr.error('Please Enter Name');
                    return false;
                }
              
                $scope.ajaxPost('api/EquipmentTypesApi/EquipmentTypesInformationUpdate', { data: Data }, function (response) {
                    console.log(response)
                    if (response.IsTure == true) {
                        toastr.success("Updated Successfully");
                        $timeout($window.location.href = '/EquipmentTypes/Index', 2000);
                    }
                    else {
                        toastr.error(response.message);
                    }
                });
            }
            $scope.EditHref = function (sId) {
                $window.open('/EquipmentTypes/Edit?Id=' + sId, '_self');
            }
            $scope.EqListHref = function (sId) {
                $window.open('/Equipment/Index?TypeId=' + sId, '_self');
            }
            
            $scope.initEdit = function () {
                var id = urlService.getUrlPrams();
                console.log(`Welcome edit init angular ID: ${id.Id}`)
                $scope.ajaxGet('api/EquipmentTypesApi/getEquipmentTypesDetails', { id: id.Id }, function (resp) {
                    if (resp.Success) {
                        console.log("IntRequest");
                        console.log(resp);
                        $scope.item = resp;
                        
                    }
                })

            }
            function stringToDate(_date, format, delimiter) {
                var formatLowerCase = format.toLowerCase();
                var formatItems = formatLowerCase.split(delimiter);
                var dateItems = _date.split(delimiter);
                var monthIndex = formatItems.indexOf("mm");
                var dayIndex = formatItems.indexOf("dd");
                var yearIndex = formatItems.indexOf("yyyy");
                var month = parseInt(dateItems[monthIndex]);
                month -= 1;
                var formatedDate = new Date(dateItems[yearIndex], month, dateItems[dayIndex]);
                return formatedDate;
            }
                
            
        }

     ]
);
