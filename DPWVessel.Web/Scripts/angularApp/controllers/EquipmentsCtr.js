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
        function ($scope, $rootScope, $timeout, $q, urlService, newGridService, $window) {
            //var url = urlService.getUrlPrams();

            $scope.init = function ()
            {
                console.log("Welcome Equipment Controller Anguler")
                GetEquipments();
            }
            function GetEquipments()
            {
                $scope.ajaxGet('api/EquipmentsApi/GetEquipmentsList', null, function (resp) {

                    console.log(resp);
                    if (resp.Success) {
                        $scope.EqtyList = resp.EquipmentsLists;
                    }

                });
            }
          
            $scope.FilterEquipments = function (data) {
                console.log('Filtering',data);
                $scope.ajaxGet('api/EquipmentsApi/GetEquipmentsList',data, function (resp) {

                    
                    console.log('Filter Data ',resp);
                    if (resp.Success) {
                        $scope.EqtyList = resp.EquipmentsLists;
                    }

                });
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
            $scope.EditEquipments = function (Data) {
                console.log(Data);
                if (Data == null || Data == undefined || Data == '') {
                    toastr.error('All filed is reqried');
                    return false;
                }
                if (Data.name == null || Data.name == undefined || Data.name == '') {
                    toastr.error('Please Enter Name');
                    return false;
                }
              
                $scope.ajaxPost('api/EquipmentsApi/EquipmentsInformationUpdate', { data: Data }, function (response) {
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
            $scope.EditHref = function (sId) {
                $window.open('/Equipments/Edit?Id=' + sId, '_self');
            }

            $scope.initEdit = function (data) {
                var id = urlService.getUrlPrams();
                console.log(`Welcome edit init angular ID: ${id.Id}`)
                $scope.ajaxGet('api/EquipmentsApi/getEquipmentsDetails', { id: id.Id }, function (resp) {
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
