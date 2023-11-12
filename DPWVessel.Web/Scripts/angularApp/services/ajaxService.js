'use strict';

app.factory('ajaxService', ["urlService", "$http", "$q", function(urlService, $http, $q){
    return {
		Get: function(url, data, successCallback, errorCallback, baseScope){
			baseScope.activeService++;
			var promise = $http.get(urlService.compose(url), { params: data, headers: { 'Accept': 'application/json' } });
			promise.then(
				function(response) {
					baseScope.activeService--;
					if (!response.data.Success) {
					    angular.forEach(response.data.ValidationErrors, function (error) {
					        toastr.error(error.Message)
					    })
					}
			     	if(successCallback)
			    		successCallback(response.data)
			  	}, function(response) {
			    	baseScope.activeService--;
		    		toastr.error(response.status + " : " + response.statusText)     
			    	if(errorCallback)
			    		errorCallback(response)
			  	}
		  	)
		}, GetBackground: function (url, data, successCallback, errorCallback, baseScope) {

		    var promise = $http.get(urlService.compose(url), { params: data, headers: { 'Accept': 'application/json' } });
		    promise.then(
                function (response) {

                    if (!response.data.Success) {
                        angular.forEach(response.data.ValidationErrors, function (error) {
                            toastr.error(error.Message)
                        })
                    }
                    if (successCallback)
                        successCallback(response.data)
                }, function (response) {

                    toastr.error(response.status + " : " + response.statusText)
                    if (errorCallback)
                        errorCallback(response)
                }
            )
		},
		Post: function(url, data, successCallback, errorCallback, baseScope){
			baseScope.activeService++;
			var promise = $http.post(urlService.compose(url), data, { headers: { 'Accept': 'application/json' } });
			promise.then(
				function(response) {
					baseScope.activeService--;
					if (!response.data.Success) {
					    angular.forEach(response.data.ValidationErrors, function (error) {
                            toastr.error(error.Message)
					    })
					}
			     	if(successCallback)
			     	    successCallback(response.data)
			  	}, function(response) {
			    	baseScope.activeService--;
			    	console.log(response);
			    	if (errorCallback) {
			    	    errorCallback(response)
			    	}
			    	else {
			    	    toastr.error(response.status + " : " + response.statusText)
			    	}
			  	}
		  	)	
		},
		GetSync: function (url, data) {
		    var re = $.ajax(
                {
                    async: false,
                    url: url,
                    data: data,
                    contentType: "application/json",
                    type: 'get',
                    success: function (response) {
                        return response;
                    },
                    error: function (response) {
                        toastr.error(response.status + " : " + response.statusText)
                        return response;
                    }
                });
		    return re.responseJSON;
		},
		UploadImage: function (fileuploadid , baseScope) {
		    var file = document.getElementById(fileuploadid).files[0];
		    var d = $q.defer();
		    if (file) {
		        //console.log(file);
		        var reader = new FileReader();
		        reader.onload = function (readerEvt) {
		            var fileData = readerEvt.target.result;
		            var fileName = file.name;
		            var folderName = "DPWVesselOPS";
		            var data =
                        {
                            folder: folderName,
                            filename: fileName,
                            verifyToken: CryptoJS.MD5(folderName + fileName).toString(),
                            filedata: fileData
                        };
		            baseScope.activeService++;
		            var promise = $http.post(urlService.getImageServiceUrl(), data,
                        {
                            headers: { 'Accept': 'application/json' }
                        });

		            promise.then(
                        function (response) {
                        baseScope.activeService--;
                            //console.log('resolving');
                            d.resolve(response.data);
                        }, function (response) {
                        baseScope.activeService--;
                            //console.log('rejecting');
                            toastr.error(response.status + " : " + response.statusText)
                            d.reject(response);
                        }
                    )
		        };
		        reader.readAsDataURL(file);
		    }
		    else {
		        d.reject('no file chosen');
		    }
		    return d.promise;
		},
		UploadImageData: function (fileData,fileName, baseScope) {
			var file = fileName;
			var FileData = fileData;
			var d = $q.defer();
			if (FileData) {
				//console.log(file);
				
					var fileData = FileData;
					var fileName = file;
					var folderName = "DPWVesselOPS";
					var data =
					{
						folder: folderName,
						filename: fileName,
						verifyToken: CryptoJS.MD5(folderName + fileName).toString(),
						filedata: fileData
					};
					baseScope.activeService++;
					var promise = $http.post(urlService.getImageServiceUrl(), data,
						{
							headers: { 'Accept': 'application/json' }
						});

					promise.then(
						function (response) {
							baseScope.activeService--;
							//console.log('resolving');
							d.resolve(response.data);
						}, function (response) {
							baseScope.activeService--;
							//console.log('rejecting');
							toastr.error(response.status + " : " + response.statusText)
							d.reject(response);
						}
					)
			}
			else {
				d.reject('no file chosen');
			}
			return d.promise;
		},
        UploadSignature: function (signData, IsReplace, FileName, baseScope) {			var d = $q.defer();			if (signData!=null) {								var fileData = signData;				var fileName = 'Form-Signature.png';				if (IsReplace==true) {					fileName = FileName;				}				var folderName = "DPWVesselOPS";				var isReplace = IsReplace;				var data =				{					folder: folderName,					filename: fileName,					verifyToken: CryptoJS.MD5(folderName + fileName).toString(),					filedata: fileData,					IsReplace: isReplace				};				baseScope.activeService++;				var promise = $http.post(urlService.getImageServiceUrl(), data,					{						headers: { 'Accept': 'application/json' }					});				promise.then(					function (response) {						baseScope.activeService--;						//console.log('resolving');						d.resolve(response.data);					}, function (response) {						baseScope.activeService--;						//console.log('rejecting');						toastr.error(response.status + " : " + response.statusText)						d.reject(response);					}				)			}			else {				d.reject('no file chosen');			}			return d.promise;		}
	}
}]);
