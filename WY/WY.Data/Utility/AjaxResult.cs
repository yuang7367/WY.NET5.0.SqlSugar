using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WY.Data.Utility
{
    public class AjaxResult : AjaxResult<object>
    {
    }

    public class AjaxResult<T> where T : new()
    {
        public AjaxResult()
        {
            Data = new T();
        }
        public AjaxResult(AjaxStatus status, string msg)
        {
            this.Message = msg;
            this.Status = status;
        }
        public string Message { get; set; }
        public string Location { get; set; }
        public T Data { get; set; }
        public AjaxStatus Status { get; set; }
    }
    public enum AjaxStatus
    {
        Normal = 1,
        Warn = 2,
        Error = 3,
        Redirect = 4
    }
}
