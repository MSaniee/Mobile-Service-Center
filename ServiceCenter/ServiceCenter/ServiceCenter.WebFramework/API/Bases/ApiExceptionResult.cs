using Newtonsoft.Json;
using ServiceCenter.Common.EnumTools;
using ServiceCenter.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceCenter.WebFramework.API.Bases
{
    public class ApiExceptionResult
    {
        public ApiExceptionResult(bool isSuccess, ApiResultStatusCode statusCode, string message = null, string stackTrace = null)
        {
            this.isSuccess = isSuccess;
            this.statusCode = statusCode;
            this.message = message ?? statusCode.ToDisplay();
            this.stackTrace = stackTrace ?? "";
        }

        public bool isSuccess { get; set; }
        public ApiResultStatusCode statusCode { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string message { get; set; }

        public string stackTrace { get; set; }
    }
}
