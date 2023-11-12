﻿using System.Collections.Generic;

namespace DE.Infrastructure.Helpers
{
    public enum FilterType
    {
        Like,
        Equal,
        NotEqual,
        LessThan,
        LessThanOrEqual,
        GreaterThan,
        GreaterThanOrEqual,
        StartsWith,
        EndsWith,
        Contains,
        WhereIn
    };

    public class QueryFilter
    {
        public FilterType Type { get; set; }
        public string Field { get; set; }
        public object Value { get; set; }
    }

    public class ListingParams
    {
        public ListingParams()
        {
            this.QueryFilters = new List<QueryFilter>();
            this.CustomData = new Dictionary<string, string>(); ;
        }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public string SortDirection { get; set; }
        public string SortField { get; set; }
        public string[] Fields { get; set; }
        public List<QueryFilter> QueryFilters { get; private set; }
        public Dictionary<string, string> CustomData { get; set; }
    }
}
