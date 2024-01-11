'use strict';

app.controller('baseCtrl', ["$scope", "ajaxService", "$q", "$window","$timeout", function ($scope, ajaxService, $q, $window,$timeout) {

	$scope.activeService = 0;	
	$scope.moment = window.moment;
	$scope.MeterType = "Start";
	
	$scope.IsNotExist = true;

$scope._currentUser = _currentUser;
//	$scope._languageContents = _languageContents;

	$scope.initializeBase = function () {
	    if (localStorage.getItem('refreshed') === null)
	    {
          localStorage['refreshed'] = true;
        }
	    else
	    {
               localStorage.removeItem('refreshed');
        }
		console.log($scope._currentUser);
		
		$scope.CheckAccess();
	    $scope.responsive = $window.outerWidth < 641
	}

   //DataTable Js
    $scope.initDataTable = function (TargetTable, additionalOptions) {
        // Check if DataTable instance exists
        var table = $.fn.DataTable.fnIsDataTable($(TargetTable)) && $(TargetTable).DataTable();

        // Destroy existing DataTable instance
        if (table) {
            table.destroy();
        }

        // Default DataTable options
        var defaultOptions = {
            "paging": false,
            "ordering": true,
            "info": false,
            "searching": false,
            "language": {
                "emptyTable": "No data Found"
            },
            "order": [[0, 'desc']],
            "columnDefs": [
                { "defaultContent": "", "targets": "_all" }, // Set default content for all columns
                { "orderable": false, "targets": $(TargetTable).find("th.no-sort").map(function () { return $(this).index(); }).toArray() } // Disable sorting for columns with class "no-sort"
            ]
            // Add more default options as needed
        };
		//how to call function 
		//  $timeout(function () {$scope.initDataTable("#Table");}, 1000);
		//how delete instance if already created code here
		//    $("#Table").DataTable().clear().destroy();
        // Merge default options with additional options
        var options = angular.merge({}, defaultOptions, additionalOptions);

        // Create a new DataTable instance
        $(TargetTable).DataTable(options);
    };
//Datatable  end here

    $scope.CheckAccess =function ()
	{
	    
	    $scope.ajaxGetBackground('api/AccountApi/CheckAccess', null, function (response) {
	        console.log(response);
	        if (response.Success) {
	            if(response.Return==false)
	            {
	                toastr.warning('Somethings Went Wrong!', 'Contact to Administrator');
	                $timeout(() => { $window.open('/Account/LogOff', '_self'); }, 2000);
	            }
	        }

	    });
	 
	}
    // Comment End
	$scope.ExternalAjaxBegin = function () {
	    $scope.activeService++;
	}

	$scope.ExternalAjaxEnd = function () {
	    $scope.activeService--;
	}

	$scope.ResetServiceCounter = function(){
		$scope.activeService = 0;
	}
    $scope.BackButton = function () {
        $window.history.back();
    }
    $scope.getcontent = function (value) {
       var values= $scope.GetLanguageContent(value);
       return values;
    }
    $scope.GetLanguageContent = function (key) {
        return _languageContents[key];
    }
	$scope.showUTCDateTime = function (date) {
	    var utcDate = moment.utc(date);
	    if (utcDate.isValid())
	        return utcDate.format("DD-MMM-YY HH:mm");
	    return '';
	}
    //start for show comas
	$scope.showPrice = function (value) {
	    return value.toString().replace(/(\d)(?=(\d{3})+(?!\d))/g, "$1,");
	}
    //Only display value
	$scope.Display = function (value) {
	    return value;
	}
    //end
	$scope.showDate = function (date) {
	    var utcDate = moment(date);
	    if (utcDate.isValid())
	        return utcDate.format("DD-MMM-YY");
	    return '';
	}
	$scope.BackButton = function () {
	    $window.history.back();
	}

	$scope.ajaxGet = function(url, data, successCallback, errorCallback){
		ajaxService.Get(url, data, successCallback, errorCallback, $scope);
	}
	$scope.ajaxGetBackground = function (url, data, successCallback, errorCallback) {
	    ajaxService.GetBackground(url, data, successCallback, errorCallback, $scope);
	}
	$scope.ajaxPost = function(url, data, successCallback, errorCallback){
		ajaxService.Post(url, data, successCallback, errorCallback, $scope);
	}
	$scope.ImageUploadPromise = function (fileuploadid) {
	    return ajaxService.UploadImage(fileuploadid , $scope);
	}
	$scope.ImageDataUploadPromise = function (fileData, fileName) {
	    return ajaxService.UploadImageData(fileData, fileName, $scope);
	}
    	$scope.SignatureUploadPromise = function (SignData, IsReplace,fileName) {
		return ajaxService.UploadSignature(SignData, IsReplace, fileName,$scope);
	}

	$scope.changeSelectedClass = function(selector) {
		$('.current').removeClass();
    	$('#' + selector).addClass('current');	
	}

	$scope.showToastrMsg = function (type) {
	    switch (type) {
	        case 'delete':
	            toastr.info('DELETE SUCCESSFULLY');
	            break;
	        default:
	            return false;
	    }
	}

	$scope.hidePanel = function (id) {
	    $('#' + id).find('.fa-chevron-up').click()
	}

	$scope.showPanel = function (id) {
	    $('#' + id).find('.fa-chevron-down').click()
	}

	$scope.togglePanelExcept = function (id) {
	    $('.x_title').find('.fa-chevron-up').click()
	    $('#' + id).find('.fa-chevron-down').click()
	}

	$scope.GetImageUrl = function (idForGet) {
	    var file = document.getElementById(idForGet).files[0];
       
	    if (file) {
	        console.log(file);
	        var reader = new FileReader();
	        reader.onload = function (readerEvt) {
	            var imageUrl = uploadImage(readerEvt.target.result, file.name);
	            $scope.$broadcast("ImageUrlGenerated", imageUrl);
	        };           
	        reader.readAsDataURL(file);
	    }

	}

	function uploadImage(imageData, fileName) {
	    var folderName = "united-admin";
	    var fileData = imageData;
	    var postedFileData =
            {
                folder: folderName,
                filename: fileName,
                verifyToken: CryptoJS.MD5(folderName + fileName).toString(),
                filedata: fileData
            };
	    var resp = $.ajax({
	        async: false,
	        type: 'POST',
	        url: 'https://fileserver2.DPWVessel.com.sa/api/uploadapi/uploadfile',
	        data: JSON.stringify(postedFileData),
	        success: function (response) {
	            return response;
	        },
	        contentType: "application/json",
	        dataType: 'json'
	    });
	    return resp.responseJSON;

	}


}]);
