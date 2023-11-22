
'use strict';

app.controller('EquipmentsCtr', [
    "$scope",
    "$rootScope",
    "$timeout",
    "$q",
    "urlService",
    "newGridService",
    "$window",
    "$filter",
    "$http",
    function ($scope, $rootScope, $timeout, $q, urlService, newGridService, $window, $filter, $http) {
        $scope.init = function () {
            console.log("Welcome Equipment Controller Angular");
            GetEquipments();
            GetEquipmentTypes();
        }

        function GetEquipments() {
            $scope.ajaxGet('api/EquipmentsApi/GetEquipmentsList', null, function (resp) {
                console.log(resp);
                if (resp.Success) {
                    $scope.EqList = resp.EquipmentsLists;
                    $timeout(function () {
                        initDataTable();
                    });
                }
            });
        }
     
        function initDataTable() {
            // Assuming you have a table with id 'equipmentTable'
            var table = $('#equipmentTable').DataTable();

            // Destroy existing DataTable instance
            table.destroy();

            // Create a new DataTable instance
            $('#equipmentTable').DataTable({
                // Add DataTable options here
                "paging": true,
                "ordering": true,
                "info": true,
                "searching": true,
                // Add more options as needed
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
                if (EndDate < StartDate) {
                    toastr.error("Start date always greate then ya equal to end Date ")

                } else {

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
        $scope.ToExportExcelTemplate = function (data) {
           
            $window.open('/Equipments/ExcelTemplate');

        }
        $scope.ToImportExcel = function () {
            var fileInput = $("#ExcelFile")[0]; // Get the file input element
            var files = fileInput.files; // Get the selected files

            if (!files.length) {
                toastr.error("Please choose a file");
                return false;
            }

            var file = files[0]; // Get the first selected file

            // Check the file extension
            var fileName = file.name;
            var extension = fileName.substr(fileName.lastIndexOf('.') + 1).toLowerCase();

            if (extension !== 'xlsx' && extension !== 'xls') {
                toastr.error('Please select an Excel file with the extension .xlsx or .xls.');
                return false;
            }

            var formData = new FormData();
            formData.append('file', file);

            $.ajax({
                url: '/Equipments/Upload',
                type: 'POST',
                contentType: false,
                processData: false,
                data: formData,
                error: function (err) {
                    console.log(err);
                }
            }).done(function (res) {
                console.log(res);
                if (res.Isture == false) {
                    toastr.error(res.msg);
                    if (res.msg2 === 'File') {
                        // Convert byte array to Uint8Array
                        var bytes = new Uint8Array(res.ErrorByt);

                        // Create a Blob from the Excel file bytes
                        var blob = new Blob([bytes], { type: 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet' });

                        // Create a link element and trigger a click to download the file
                        var link = document.createElement('a');
                        link.href = window.URL.createObjectURL(blob);
                        var currentDate = new Date().toLocaleDateString();


                        link.download = `ErrorFileEquipments_${currentDate}.xlsx`;
                        document.body.appendChild(link);
                        link.click();
                        document.body.removeChild(link);

                    }
                }
                else {
                    toastr.success(res.msg);

                    
                }
               
                $timeout(() => {
                    window.reload();
                }, 2500)
                
                console.log(res);
            });
        };




            
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
                if (data.equipmentTypeId == null || data.equipmentTypeId == undefined || data.equipmentTypeId == '' || data.equipmentTypeId == 0) {
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
                if (data.equipmentTypeId == null || data.equipmentTypeId == undefined || data.equipmentTypeId == '' || data.equipmentTypeId == 0) {
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
