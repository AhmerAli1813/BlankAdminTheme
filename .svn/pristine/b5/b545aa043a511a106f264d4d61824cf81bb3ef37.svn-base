using DE.Infrastructure.Helpers;
using Newtonsoft.Json;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Http.ModelBinding;

namespace DPWVessel.Web.Core.Model
{
    public class ExternalFilterModel
    {
        public FilterType Type { get; set; }
        public string Value { get; set; }
    }

    public class ListingParamModelBinder : IModelBinder
    {
        public bool BindModel(System.Web.Http.Controllers.HttpActionContext actionContext, ModelBindingContext bindingContext)
        {
            try
            {
                var model = new ListingParams();
                var values = actionContext.Request.GetQueryNameValuePairs()
                    .ToDictionary(pair => pair.Key, pair => pair.Value, StringComparer.InvariantCultureIgnoreCase);

                int pageNumber = 1;
                if (values.ContainsKey("PageNumber"))
                    int.TryParse(values["PageNumber"], out pageNumber);

                int pageSize = int.MaxValue;
                if (values.ContainsKey("PageSize"))
                    int.TryParse(values["PageSize"], out pageSize);

                if (values.ContainsKey("SortDirection"))
                    model.SortDirection = values["SortDirection"];

                if (values.ContainsKey("SortField"))
                    model.SortField = values["SortField"];

                model.PageNumber = pageNumber;
                model.PageSize = pageSize;

                if (values.ContainsKey("filters"))
                {
                    var filters = values["filters"];
                    if (filters != null && filters.Length > 3)
                    {
                        var dictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(filters);
                        foreach (var keyvalue in dictionary)
                        {
                            model.QueryFilters.Add(new QueryFilter { Type = FilterType.Equal, Field = keyvalue.Key, Value = keyvalue.Value });
                        }
                    }
                }

                if (values.ContainsKey("ExternalFilters"))
                {
                    var externalFilters = values["ExternalFilters"];
                    if (externalFilters != null && externalFilters.Length > 3)
                    {
                        var data = externalFilters;
                        var dictionary = JsonConvert.DeserializeObject<Dictionary<string, List<ExternalFilterModel>>>(externalFilters);
                        foreach (var keyvalue in dictionary)
                        {
                            foreach (var value in keyvalue.Value)
                            {
                                model.QueryFilters.Add(new QueryFilter { Type = value.Type, Field = keyvalue.Key, Value = value.Value });
                            }
                        }
                    }
                }

                if (values.ContainsKey("CustomData"))
                {
                    var filters = values["CustomData"];
                    if (filters != null && filters.Length > 3)
                    {
                        var dictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(filters);
                        model.CustomData = dictionary;
                    }
                }
                bindingContext.Model = model;
                return true;
            }
            catch (Exception e)
            {
                var _logger = LogManager.GetLogger("TMS");
                _logger.Error(e);
                bindingContext.Model = new ListingParams();
                return true;
            }
        }
    }
}