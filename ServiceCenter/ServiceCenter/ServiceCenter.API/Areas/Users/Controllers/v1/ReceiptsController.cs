using Mapster;
using Newtonsoft.Json;
using ServiceCenter.Application.Dtos.Receipts;
using ServiceCenter.Application.Features.ReveiptsAggregate.Receipts.Commands;
using ServiceCenter.Application.Features.ReveiptsAggregate.Receipts.Queries;
using ServiceCenter.Common.IdentityTools;
using ServiceCenter.Domain.Core.Utilities.PagesSettings;
using ServiceCenter.WebFramework.API.Utilities.PageSettings;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ServiceCenter.API.Areas.Users.Controllers.v1;

[Area("Users")]
[ApiVersion("1")]
[Authorize(Roles = "User")]
[ApiExplorerSettings(GroupName = "Users - Receipts")]
public class ReceiptsController : BaseController
{
    private readonly IMediator _mediator;

    public ReceiptsController(
        IMediator mediator)
    {
        _mediator = mediator.ThrowIfNull();
    }

    /// <summary>
    /// افزودن قبض تعمیر
    /// </summary>
    /// <param name="dto"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpPost]
    [ActionName("")]
    [AllowAnonymous]
    public async Task<ApiResult> Post(ReceiptDto dto, CancellationToken cancellationToken)
    {
        dto.UserId = User.Identity.GetGuidUserId();
        dto.UserId = Guid.Parse("617c75b4-86ba-ec11-9801-50e549189de0");

        CreateReceiptCommand command = dto.Adapt<CreateReceiptCommand>();

        return await _mediator.Send(command, cancellationToken);
    }

    /// <summary>
    /// دریافت یک قبض تعمیر
    /// </summary>
    /// <param name="id"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpGet]
    [ActionName("")]
    [AllowAnonymous]
    public async Task<ApiResult<ReceiptDto>> Get(long id, CancellationToken cancellationToken)
    {
        GetReceiptQuery query = new(id);

        return await _mediator.Send(query, cancellationToken);
    }

    /// <summary>
    /// ویرایش اطلاعات قبض
    /// </summary>
    /// <param name="dto"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpPut]
    [ActionName("")]
    [AllowAnonymous]
    public async Task<ApiResult> Put(ReceiptDto dto, CancellationToken cancellationToken)
    {
        UpdateReceiptCommad command = dto.Adapt<UpdateReceiptCommad>();

        return await _mediator.Send(command, cancellationToken);
    }

    /// <summary>
    /// دریافت قبض های تعمیر
    /// </summary>
    /// <param name="pagable"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpPost]
    [AllowAnonymous]
    public async Task<ApiResult<PagedList<ReceiptDto>>> GetAll(Pagable pagable, CancellationToken cancellationToken)
    {
        //Guid? userId = User.Identity.GetGuidUserId();
        Guid? userId = Guid.Parse("617c75b4-86ba-ec11-9801-50e549189de0");

        GetReceiptsQuery query = new(pagable, (Guid)userId);
        var result = await _mediator.Send(query, cancellationToken);

        Response.AddPaginationToHeader(result.Data.MetaData);

        return result;
    }
}

