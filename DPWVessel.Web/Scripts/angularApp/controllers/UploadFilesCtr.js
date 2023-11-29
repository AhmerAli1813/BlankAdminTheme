'use strict';
app.controller('UploadFilesCtr',
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
                console.log("Welcome UploadFiles Controller Anguler")
                GetUploadFiles();
            }
            function GetUploadFiles()
            {
                
                $scope.ajaxGet('api/UploadFilesApi/GetUploadFilesList',null, function (resp) {

                    console.log(resp);
                    if (resp.Success) {
                        $scope.UpF = resp.GetAllUploadFilesLists;
                        console.log($scope.UpF);
                        $timeout(function () {
                            initDataTable();
                        });
                    }

                });
            }
            function initDataTable() {
                // Assuming you have a table with id 'UploadFiles'
                $('#UploadFilesTable').DataTable({
                    // Add DataTable options here
                    "paging": true,
                    "ordering": true,
                    "info": true,
                    "searching": true,
                    // Add more options as needed
                });
            }
            $scope.FilterUploadFiles = function (data) {
                console.log("ddata", data);
                if (data == undefined) {
                    data = {};
                }
                
                var st = $("#startDate").val();
                var et = $("#endDate").val();
                if (st != "" && st != undefined) {
                    var StartDate = moment(stringToDate($("#startDate").val(), "dd/MM/yyyy", "/")).format('YYYY-MM-DDT00:00:00');
                    /*$("#startDate").val('');*/

                }
                if (et != "" && et != undefined) {
                    var EndDate = moment(stringToDate($("#endDate").val(), "dd/MM/yyyy", "/")).format('YYYY-MM-DDT00:00:00');
                  /*  $("#endDate").val('');*/
                }
                console.log(data);                                                                                             


                $scope.ajaxGet('api/UploadFilesApi/GetUploadFilesList',
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
                            $scope.UpF = resp.GetAllUploadFilesLists;
                        }

                    });
            }
            $scope.EditHref = function (sId) {
                $window.open('/UploadFiles/Edit?Id=' + sId, '_self');
            }
            $scope.downloadHref = function (Id) {
                $window.open('/UploadFiles/Download?Id=' + Id, '_self');
            }
            $scope.initEdit = function () {
                var id = urlService.getUrlPrams();
                console.log(`Welcome edit init angular ID: ${id.Id}`)
                $scope.ajaxGet('api/UploadFilesApi/getUploadFilesDetails', { id: id.Id }, function (resp) {
                    if (resp.Success) {
                        console.log("IntRequest");
                        console.log(resp);
                        $scope.item = resp;

                    }
                });

            }

            $scope.selectedFiles = [];
            $scope.fileList = [];
            $scope.updateFileDetails = function () {
                var file = $("#File")[0].files

                console.log(file);
                console.log(file[0].lastModifiedDate);
                if (file.length>0) {
                    // You can update file details based on the selected file
                    $scope.item.name = file[0].name;
                    
                    $scope.item.createdAt = file[0].lastModifiedDate;
                    var newFile = {
                        originalFile: file[0],
                        name: file[0].name,
                        url: URL.createObjectURL(file[0])
                    }
                    $scope.fileList.push(newFile);
                    $scope.$apply(function () {
                        console.log("File update List:", $scope.fileList);
                    });
                    // Add other details as needed
                } else {
                    // Clear file details if no file is selected
                    $scope.item = {};
                    $("#File").val("");

                }
                $scope.$apply();
            };
            $scope.handleFileUpload = function () {
                var mfiles = $("#File")[0].files;
                var maxSizeMB = 2;
                console.log("hnadel", mfiles);

                for (var i = 0; i < mfiles.length; i++) {
                    var file = mfiles[i];
                    if (file.size <= maxSizeMB * 1024 * 1024) {
                        $scope.displayFile(file);
                    } else {
                        toastr.error("file can be selected if size less then 2Mbs!  ");
                    }

                    $scope.$apply(function () {
                        console.log("File List:", $scope.fileList);
                    });
                }

                // Reset the file input value to clear the previous selection
                 $("#File").val("");

            };
            $scope.displayFile = function (file) {
                var newFile = {
                    originalFile: file,
                    name: file.name,
                    url: URL.createObjectURL(file)
                }
                $scope.fileList.push(newFile);
                console.log("File List after displayFile:", $scope.fileList);
                // Store the original File object and create object URL for display
             
            
            };
            $scope.removeFile = function (index) {
              
                // Implement your logic to remove a file from the list
                $scope.fileList.splice(index, 1);
            };
            $scope.AddUploadFiles = function () {
       

                if ($scope.fileList.length > 0) {
                    var formData = new FormData();
                    for (var i = 0; i < $scope.fileList.length; i++) {

                        var originalFile = $scope.fileList[i].originalFile;
                        formData.append("files", originalFile);
                        // Perform any additional processing or upload logic with the originalFile
                        console.log("Uploading file:", originalFile);
                    }

                    $.ajax({
                        url: '/UploadFiles/AddUploadFilesRec',
                        type: 'POST',
                        contentType: false,
                        processData: false,
                        data: formData,
                        error: function (err) {
                            console.log(err);
                        }
                    }).done(function (res) {
                        console.log(res);
                        if (res.IsTure == false) {
                            toastr.error(res.msg)
                        } else {
                            toastr.success(res.msg)
                        }
                        $timeout(function () {
                            $window.location.href = '/UploadFiles';
                        }, 2000)
                    });
                } else {
                    toastr.error("Please select file !");
                }
               
              
            }
            $scope.EditUploadFiles = function (Data) {
                var Files = $("#File")[0];

                // Check if the file input exists
                if (Files && Files.files.length > 0) {
                    var file = Files.files[0];
                    var id = Data.id;
                    console.log(file, id);
                    var formData = new FormData();
                    formData.append("id", id);
                    formData.append("file", file);

                    console.log("form Data",formData);

                    $.ajax({
                        url: '/UploadFiles/Update',
                        type: 'POST',
                        processData: false,
                        contentType: false,
                        data: formData,
                        success: function (response) {
                            console.log(response);
                            if (response.IsTrue === false) {
                                toastr.error(response.msg);

                            } else {
                                toastr.success(response.msg);
                                $timeout(() => { $window.location.href = '/UploadFiles'; }, 2000)
                            }
                            // Redirect or perform other actions on success
                        },
                        error: function (error) {
                            console.error(error);
                        }
                    });
                } else {
                    console.error("File input is empty or not found.");
                }

            };
            $scope.openDeleteModal = function (recordId) {
                console.log("Delete Modal ID :", recordId)
                $scope.recordIdToDelete = recordId;
                $('#deleteModal').modal('show');
            };
            $scope.deleteRecord = function (id) {
                console.log("Delete Confirm Id :", id)
             // $window.open('/UploadFiles/DeleteUp?Id=' + id, '_self');
                $.ajax({
                    url: '/UploadFiles/DeleteUp',
                    type: 'POST',
                    data: {Id:id},
                    error: function (err) {
                        console.log(err);
                    }
                    
                }).done(function (res) {
                    console.log(res);
                    if (res.IsTure == false) {
                        toastr.error(res.msg)
                    } else {
                        toastr.success(res.msg)
                    }
                    $timeout(function () {
                        $window.location.href = '/UploadFiles';
                    }, 2000)
                });

                $('#deleteModal').modal('hide');
                
            };
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

     ]);
