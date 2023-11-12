'use strict';

app.factory('urlService', function () {
    var ImageServiceUrl = 'https://fileserver2.salomi.com.sa/api/uploadapi/uploadfile';
   // var ImageServiceUrl = 'https://fileserver.dpworld.sa/api/uploadapi/uploadfile';

	var LocalServiceUrl = '/';
	return {
	    getImageServiceUrl: function(){
	        return ImageServiceUrl;
	    },        
	    compose: function (url) {
	        return LocalServiceUrl + url;
	    },
	    getUrlPrams: function () {
	        // This function is anonymous, is executed immediately and 
	        // the return value is assigned to QueryString!
	        var query_string = {};
	        var query = window.location.search.substring(1);

	        if (query == '') {
	            return {};
	        }

	        var vars = query.split("&");
	        for (var i = 0; i < vars.length; i++) {
	            var pair = vars[i].split("=");

                //skip the autologin token
	            if (pair[0] == 'token')
	                return;

	            // If first entry with this name
	            if (typeof query_string[pair[0]] === "undefined") {
	                query_string[pair[0]] = decodeURIComponent(pair[1]);
	                // If second entry with this name
	            } else if (typeof query_string[pair[0]] === "string") {
	                var arr = [query_string[pair[0]], decodeURIComponent(pair[1])];
	                query_string[pair[0]] = arr;
	                // If third or later entry with this name
	            } else {
	                query_string[pair[0]].push(decodeURIComponent(pair[1]));
	            }
	        }
	        return query_string;
	    }
	}
});