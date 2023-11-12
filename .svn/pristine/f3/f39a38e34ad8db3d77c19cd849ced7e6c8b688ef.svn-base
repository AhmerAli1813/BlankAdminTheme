'use strict';

var app = angular.module('DPWVesselApp', []);


toastr.options.positionClass = "toast-bottom-right";


Array.prototype.sum = function (prop) {
    var total = 0
    for ( var i = 0, _len = this.length; i < _len; i++ ) {
        total += this[i][prop]
    }
    return total
}

//Adding X-Request-With for Glimpse Ajax Calls
app.config(['$httpProvider', function ($httpProvider) {
    $httpProvider.defaults.headers.common = { 'X-Requested-With': 'XMLHttpRequest' };
}]);
