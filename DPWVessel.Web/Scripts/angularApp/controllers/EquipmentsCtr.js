'use strict';
app.controller('EquipmentsCtr',
    [
        "$scope",
        "$rootScope",
        "$timeout",
        "$q",
        "urlService",
        "newGridService",
        "$window",
        "$filter",
        function ($scope, $rootScope, $timeout, $q, urlService, newGridService, $window, $filter) {
            //var url = urlService.getUrlPrams();
       
            $scope.init = function ()
            {
                console.log("Welcome Equipment Controller Anguler")
                GetEquipments();
                GetEquipmentTypes();
            }
            function GetEquipments()
            {
                var TypeId = urlService.getUrlPrams();
                $scope.ajaxGet('api/EquipmentsApi/GetEquipmentsList',null , function (resp) {
                    
                    console.log(resp);
                    if (resp.Success) {
                        $scope.EqList = resp.EquipmentsLists;
                    }

                });
            }
            function GetEquipmentTypes() {
                $scope.ajaxGet('api/EquipmentsApi/GetAllEquipmentsTypesOpt', null, function (resp) {
                    console.log("GetAll Equipments Types Opt",resp);
                    if (resp.Success) {
                        $scope.EqtyList = resp.EquipmentTypesOpt;
                    }

                });
            }

            $scope.FilterEquipments = function (data) {
                console.log("ddata", data);
                if (data == undefined) {
                    data = {};
                }
                var st = $("#startDate").val();
                var et = $("#endDate").val();
                if (st != "" && st != undefined) {
                    var StartDate = moment(stringToDate($("#startDate").val(), "dd/MM/yyyy", "/")).format('YYYY-MM-DDT00:00:00');
                   
                  /*  $("#startDate").val('');*/

                }
                if (et != "" && et != undefined) {
                    var EndDate = moment(stringToDate($("#endDate").val(), "dd/MM/yyyy", "/")).format('YYYY-MM-DDT00:00:00');
                   
                /*    $("#endDate").val('');*/
                }
                console.log(data);
              

                $scope.ajaxGet('api/EquipmentsApi/GetEquipmentsList',
                    {
                        id: data.id,
                        name: data.name,
                        equipmentTypeId: data.equipmentTypeId,
                        startDate: StartDate,
                        endDate: EndDate
                    }
                    , function (resp) {
                        data = {};
                    console.log('Filter Data', resp);
                    if (resp.Success) {
                        
                        $scope.EqList = resp.EquipmentsLists;
                        console.log('Filter scope EqList', $scope.EqList);
                    }

                });
            }
            $scope.ToExportExcel = function (data) {
                console.log("ddata", data);
                if (data == undefined) {
                    data = {};
                }
                var st = $("#startDate").val();
                var et = $("#endDate").val();
                if (st != "" && st != undefined) {
                    var StartDate = moment(stringToDate($("#startDate").val(), "dd/MM/yyyy", "/")).format('YYYY-MM-DDT00:00:00');

                    /*  $("#startDate").val('');*/

                }
                if (et != "" && et != undefined) {
                    var EndDate = moment(stringToDate($("#endDate").val(), "dd/MM/yyyy", "/")).format('YYYY-MM-DDT00:00:00');

                    /*    $("#endDate").val('');*/
                }
                console.log(data);


                var queryString = $.param({
                    id: data.id,
                    name: data.name,
                    equipmentTypeId: data.equipmentTypeId,
                    startDate: StartDate,
                    endDate: EndDate
                });

                $window.open('/Equipments/ToExportExcelEquipment?' + queryString, '_self');

            }
            
         
            $scope.AddEquipments = function (data) {
                console.log(data);
                if (data == null || data == undefined || data == '') {
                    toastr.error('All filed is reqried');
                    return false;
                }
                if (data.name == null || data.name == undefined || data.name == '') {
                    toastr.error('Please Enter Name');
                    return false;
                }
                if (data.equipmentsTypeId == null || data.equipmentsTypeId == undefined || data.equipmentsTypeId == '') {
                    toastr.error('Please Select  euipments types ');
                    return false;
                }
                
                $scope.ajaxPost('api/EquipmentsApi/AddEquipmentsRec', data, function (response) {
                    console.log(response)
                    if (response.IsTure == false) {
                        toastr.error(response.message)
                    } else { 
                   
                        toastr.success('Records registered successful');
                        $timeout(() => { $window.location.href = '/Equipments' }, 2000);
                        //$timeout($window.location.href = '/account/RegisterUser', 5000);
                    }

                });
            }
            $scope.EditEquipments = function (data) {
                console.log(data);
                //data.equipmentsTypeId = document.getElementById("equipmentsTypeId").value;
                if (data == null || data == undefined || data == '') {
                    toastr.error('All filed is reqried');
                    return false;
                }
                if (data.name == null || data.name == undefined || data.name == '') {
                    toastr.error('Please Enter Name');
                    return false;
                }
                if (data.equipmentTypeId == null || data.equipmentTypeId == undefined || data.equipmentTypeId == '') {
                    toastr.error('Please Select  euipments types ');
                    return false;
                }
                console.log(data,'---');
                $scope.ajaxPost('api/EquipmentsApi/EquipmentsInformationUpdate', { data: data }, function (response) {
                    console.log(response)
                    if (response.IsTure == true) {
                        toastr.success("Updated Successfully");
                        $timeout($window.location.href = '/Equipments/Index', 2000);
                    }
                    else {
                        toastr.error(response.message);
                    }
                });
            }
            $scope.EditHref = function (Id) {
                $window.open('/Equipments/Edit?Id=' + Id, '_self');
            }
            $scope.PrintHref = function (Id) {
                $window.open('/Equipments/Print?Id=' + Id, '_self');
            }
            
            $scope.initEdit = function () {
                var Url = urlService.getUrlPrams();
                GetEquipmentTypes();
                console.log(`Welcome edit init angular ID: ${Url.Id}`)
                $scope.ajaxGet('api/EquipmentsApi/getEquipmentsDetails', { id: Url.Id }, function (resp) {
                    if (resp.Success) {
                        console.log("IntRequest");
                        console.log(resp);
                        $scope.eq = resp;
                        
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
