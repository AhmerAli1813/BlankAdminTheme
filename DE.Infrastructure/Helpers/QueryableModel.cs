using System.Collections.Generic;

namespace DE.Infrastructure.Helpers
{
    public class QueryableModel
    {
        public IEnumerable<dynamic> Results { get; set; }
        public int Count { get; set; }
    }
}
