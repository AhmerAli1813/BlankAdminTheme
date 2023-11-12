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
                $scope.ajaxGet('api/EquipmentTypesApi/GetEquipmentTypesList', null, function (resp) {

                    console.log(resp);
                    if (resp.Success) {
                        $scope.EqtyList = resp.EquipmentTypesLists;
                    }

                });
            }
          
            $scope.FilterEquipmentType = function (data) {
                console.log('Filtering',data);
                $scope.ajaxGet('api/EquipmentTypesApi/GetEquipmentTypesList',data, function (resp) {

                    
                    console.log('Filter Data ',resp);
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

            $scope.initEdit = function (data) {
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

                
            
        }

     ]
);
