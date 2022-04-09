using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ServiceCenter.WebFramework.API.Filters;

namespace ServiceCenter.WebFramework.API.Bases;

[ApiController]
[Authorize]
[ApiResultFilter]
//[ApiVersion("1")]
[Route("api/v{version:apiVersion}/[area]/[controller]/[action]")]
public class BaseController : ControllerBase
{
}

