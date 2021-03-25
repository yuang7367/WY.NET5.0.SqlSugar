using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WY.WebApi.Filters
{
    public class JsonErrorResponse
    {
        public string Message { get; set; }

        public object DeveloperMessage { get; set; }
    }
}
