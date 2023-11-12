using System;

namespace DE.Infrastructure.CodeContracts
{
    public static class DateTimeHelpers
    {
        public static DateTime GetDefaultDateTime()
        {
            return new DateTime(1901, 01, 01);
        }
    }
}
