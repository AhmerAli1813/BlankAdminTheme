'use strict';

app.factory('newGridService', ["$http", "urlService", "$timeout", "$q", function ($http, urlService, $timeout, $q) {

    var Grid = function (scope, options) {
        var self = this
        self.enablePaginationControls = true;
        self.autoFetchData = true;

        //append all properites form options into self object
        for (var k in options)
            self[k] = options[k];

        if (self.externalColumnFilters) {
            self.isActiveExternalFilters = true
        }

        self.paginationOptions = {
            PageNumber: 1,
            PageSize: 15,
            sort: null
        };

        self.externalFilters = {}
        self.params = {}

        //add delete column
        if (self.enableDelete && self.enableDelete == true) {
            self.columnDefs.push({
                name: ' ',
                width: '8%',
                enableCellEdit: false,
                cellTemplate: '<a ng-click="grid.appScope.markedRowForDelete(row.entity)"><span title="Delete Entry" style="font-size:large;color:#E00707;cursor:pointer;padding:6px" class="glyphicon glyphicon-remove"></span></a>',
            });
        }

        self.saveRowCallback = function (rowEntity) {
            var url = self.baseUrl + '/update';
            var promise = $q.defer()
            scope.gridApi.rowEdit.setSavePromise(rowEntity, promise.promise);
            scope.ajaxPost(url, rowEntity, function (response) {
                if (response.Success) {
                    promise.resolve()
                    rowEntity.Timestamp = response.Entity.Timestamp;
                    //self.reset();
                }
                else
                    promise.reject()
            })
        }
        self.entityToDelete = {};
        scope.markedRowForDelete = function (entity) {
        console.log(entity);
            self.entityToDelete = entity;
            $('#delete-confirmation').modal('show');
            console.log("Opened From NewGrid Function");
            $('#myModalDelete').modal('show');
            $('#myModalDelete').show();
            $("#myModalDelete").css("display", "block");
            $(".modal-backdrop").css("display", "block");
            $(".modal-backdrop").css("opacity", "0.5");
        }

        scope.deleteRow = function () {
           // console.log("this is Going to Delete the record");
            scope.ajaxPost(self.baseUrl + '/remove', { Id: self.entityToDelete.Id, Timestamp: self.entityToDelete.Timestamp }, function (response) {
                if (response.Success) {
                    var index = scope.gridOptions.data.indexOf(self.entityToDelete);
                    scope.gridOptions.data.splice(index, 1);
                    scope.showToastrMsg('delete');
                }
            }, function (response) {
                toastr.error('An error occurred while deleting');
            });
            $('#myModalDelete').modal('hide');
            $('#myModalDelete').hide();
             $(".modal-backdrop").css("display", "none");

        };

        self.init = function () {
            scope.gridOptions = {
                paginationPageSizes: [15, 30, 50, 100,500, 1000, 2000],
                paginationPageSize: 15,
                useExternalPagination: true,
                useExternalSorting: true,
                enableColumnResizing: true,
                CustomData: self.CustomData,
                columnDefs: self.columnDefs,
                /**/
                enableGridMenu: true,
                enableSelectAll: true,
                exporterMenuPdf: false,
                exporterMenuExcel: false,
                exporterMenuAllData: false,
                exporterMenuCsv:false,
                exporterCsvFilename: 'myFile.csv',
                exporterPdfDefaultStyle: { fontSize: 9, margin:[0,0,0,0] },
                exporterPdfTableStyle: { margin: [0, 10, 0, 0] },
                exporterPdfTableHeaderStyle: { fontSize: 10, bold: true, italics: true, color: '#000' },
                exporterPdfHeader: {
                    columns: [
                    { text: 'DPWVessel', alignment: 'center', margin: [0, 10, 0, 0], fontSize: 22 }
                    ],
                },
                exporterPdfFooter: {
                    columns: [
                        { text: '____________________________________________________________________________________________________________________________________ Shop # Z-55, Hannan Center, Dar-ul-Aman Society, Shahrah-e-Faisal, Karachi. Tel: +92 21 34320213-14-15', alignment: 'center', margin: [10, 0, 10, 0], fontSize: 10 }
                    ],
                },
                //exporterPdfFooter: function (currentPage, pageCount) {
                //    return { text: currentPage.toString() + ' of ' + pageCount.toString(), style: 'footerStyle' };
                //},
                exporterPdfCustomFormatter: function (docDefinition) {
                    docDefinition.styles.headerStyle = { fontSize: 22, bold: true, margin: [10, 10, 10, 0] };
                    docDefinition.styles.footerStyle = { fontSize: 10, bold: true };
                    
                    return docDefinition;
                },
                exporterPdfOrientation: 'portrait',
                exporterPdfPageSize: 'LETTER',
                exporterPdfMaxGridWidth: 500,
                exporterCsvLinkElement: angular.element(document.querySelectorAll(".custom-csv-link-location")),
                onRegisterApi: function (gridApi) {
                    scope.gridApi = gridApi;
                },
                gridMenuCustomItems: [
                    {
                        title: 'Export data as EXCEL',
                        action: function ($event) {
                            exportUiGridService.exportToExcel('sheet 1', scope.gridApi, 'visible', 'visible');
                        },
                        order: 111
                    }
                ],
                /**/

                enableFiltering: self.isActiveExternalFilters,
                useExternalFiltering: self.isActiveExternalFilters,
                enablePaginationControls: self.enablePaginationControls,
                onRegisterApi: function (gridApi) {
                    scope.gridApi = gridApi;
                    scope.gridApi.core.on.sortChanged(scope, function (grid, sortColumns) {
                        if (sortColumns.length == 0) {
                            self.paginationOptions.Sort = null;
                        } else {
                            self.paginationOptions.SortField = sortColumns[0].field;
                            self.paginationOptions.SortDirection = sortColumns[0].sort.direction;
                        }
                        self.getPage();
                    });

                    if (self.externalColumnFilters) {
                        scope.gridApi.core.on.filterChanged(scope, function () {
                            var externalFilters = []
                            angular.forEach(this.grid.columns, function (value) {
                                var filter = {};
                                for (var i = 0; i < value.filters.length; i++) {
                                    if (value.filters[i].term) {
                                        var type = self.GetFilterType(value.filters[i]);
                                        if (!filter[value.name])
                                            filter[value.name] = [];
                                        filter[value.name].push({ Type: type, Value: value.filters[i].term });
                                    }
                                }
                                externalFilters.push(filter)
                            });
                            self.externalFilters = {}
                            angular.forEach(externalFilters, function (value) {
                                for (var key in value)
                                    self.externalFilters[key] = value[key]
                            });

                            if (self.filterPromise)
                                $timeout.cancel(self.filterPromise)

                            self.filterPromise = $timeout(function () {
                                self.getPage();
                            }, 1000)
                        });
                    }

                    //register after cell edit callback
                    if (self.autoFetchData)
                        gridApi.rowEdit.on.saveRow(scope, self.saveRowCallback);

                    if (self.afterCellEdit)
                        gridApi.edit.on.afterCellEdit(scope, self.afterCellEdit);

                    gridApi.pagination.on.paginationChanged(scope, function (newPage, pageSize) {
                        self.paginationOptions.PageNumber = newPage;
                        self.paginationOptions.PageSize = pageSize;
                        self.getPage();
                    });
                }
            };

            if(self.autoFetchData)
                self.getPage();

        }

        self.GetFilterType = function (filter) {
            switch (filter.type) {
                case uiGridConstants.filter.LESS_THAN: return "LessThan";
                case uiGridConstants.filter.GREATER_THAN: return "GreaterThan";
                case uiGridConstants.filter.CONTAINS: return "Contains";
                case uiGridConstants.filter.EXACT: return "Equal";
                case uiGridConstants.filter.GREATER_THAN_OR_EQUAL: return "GreaterThanOrEqual";
                case uiGridConstants.filter.LESS_THAN_OR_EQUAL: return "LessThanOrEqual";
                case uiGridConstants.filter.NOT_EQUAL: return "NotEqual";
                case uiGridConstants.filter.SELECT: return "Equal";
                case uiGridConstants.filter.STARTS_WITH: return "StartsWith";
                case uiGridConstants.filter.ENDS_WITH: return "EndsWith";
                default: return "StartsWith";
            }
        }

        self.reset = function () {
            self.paginationOptions.PageNumber = 1;
            self.paginationOptions.PageSize = 15;
            self.paginationOptions.Sort = null;
            self.getPage();
        }

        self.getPage = function () {
            self.params = self.paginationOptions
            if (self.filters) {
                self.params.filters = angular.copy(self.filters);
            }
            else {
                self.filters = {};
                for (var key in urlService.getUrlPrams()) {
                    self.filters[key] = urlService.getUrlPrams()[key];
                }
                if (!$.isEmptyObject(self.filters)) {
                    self.params.filters = angular.copy(self.filters);
                }
            }
            if (self.CustomData) {
                self.params.CustomData = angular.copy(self.CustomData);
            }

            self.params.ExternalFilters = {}
            if (!$.isEmptyObject(self.externalFilters)) {
                angular.forEach(self.externalFilters, function (value, key) {
                    self.params.ExternalFilters[key] = value;
                })
                self.params.filters = null;
            }
            if (self.initialExternalFilter) {
                self.params.ExternalFilters = self.initialExternalFilter;
            }

            // get table rows from server
            console.log(self.baseUrl);
            scope.ajaxGet(self.baseUrl + '/list', self.params, function (data) {
                scope.gridOptions.totalItems = data.Payload.TotalItems;
                data = data.Payload.Data;
                scope.gridOptions.data = data;
                $timeout(self.setGridHeight)
            })
        }

        self.addRow = function (data, successCallback) {
            scope.ajaxPost(self.baseUrl + '/add', data, function (response) {
                scope.gridOptions.data.unshift(response.Entity)
                successCallback(response);
            });
        }

        self.setGridHeight = function () {
            if (scope.gridApi) {
                //console.log(scope.gridApi.grid.options.paginationPageSize);
                scope.gridApi.core.refresh();
                var row_height = 30;
                var header_height = 31;
                var visibleRowsCount = scope.gridApi.core.getVisibleRows().length;
                var height = row_height * visibleRowsCount + header_height;
                if (visibleRowsCount < 11 && visibleRowsCount > 8) {
                    height *= 1.23;
                    $('.grid').height(height);
                } else if (visibleRowsCount >= 11) {
                    if (visibleRowsCount < 16)
                        height *= 1.18;
                    $('.grid').height(height);
                }
                scope.gridApi.grid.handleWindowResize();
            }
        }

        self.setFilter = function (filter) {
            self.filters = filter;
        }

        self.setCustomData = function (CustomData) {
            self.filters = CustomData;
        }

        self.setExternalFilter = function (filter) {
            self.initialExternalFilter = filter;
        }

        self.init();
    }

    return {
        init: function (scope, options) {
            return new Grid(scope, options);
        },
    }
}]);