using Newtonsoft.Json;
using ServiceCenter.Application.Services.Base;
using ServiceCenter.Common.EnumTools;
using ServiceCenter.Domain.Enums;

namespace ServiceCenter.Website.Services;

public class ApiResult
{
    public bool IsSuccess { get; set; }
    public ApiResultStatusCode StatusCode { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public string Message { get; set; }

    public ApiResult(bool isSuccess, ApiResultStatusCode statusCode, string message = null)
    {
        IsSuccess = isSuccess;
        StatusCode = statusCode;
        Message = message ?? statusCode.ToDisplay();
    }

    #region Implicit Operators

    public static implicit operator ApiResult(SResult result)
    {
        return new ApiResult(result.IsSuccess, (ApiResultStatusCode)result.StatusCode, result.Message);
    }


    #endregion Implicit Operators
}

public class ApiResult<TData> : ApiResult where TData : class
{
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public TData Data { get; set; }

    public ApiResult(bool isSuccess, ApiResultStatusCode statusCode, TData data, string message = null)
        : base(isSuccess, statusCode, message)
    {
        Data = data;
    }

    #region Implicit Operators

    public static implicit operator ApiResult<TData>(TData data)
    {
        return new ApiResult<TData>(true, ApiResultStatusCode.Success, data);
    }

    public static implicit operator ApiResult<TData>(SResult<TData> result)
    {
        return new ApiResult<TData>(result.IsSuccess,
                                     result.IsSuccess ? ApiResultStatusCode.Success : ApiResultStatusCode.BadRequest,
                                     result.Data,
                                     result.Message);
    }

    #endregion Implicit Operators
}

