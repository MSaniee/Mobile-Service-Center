using Mapster;
using ServiceCenter.Application.Dtos.Receipts;
using ServiceCenter.Application.Features.ReveiptsAggregate.Receipts.Commands;
using ServiceCenter.Application.Features.ReveiptsAggregate.Receipts.Queries;
using ServiceCenter.Common.IdentityTools;
using ServiceCenter.Domain.Core.Utilities.PagesSettings;
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
    public async Task<ApiResult> Post(ReceiptDto dto, CancellationToken cancellationToken)
    {
        dto.UserId = User.Identity.GetGuidUserId();

        CreateReceiptCommand command = dto.Adapt<CreateReceiptCommand>();

        return await _mediator.Send(command, cancellationToken);
    }

    /// <summary>
    /// دریافت قبض های تعمیر
    /// </summary>
    /// <param name="pagable"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<ApiResult<List<ReceiptDto>>> GetAll(Pagable pagable, CancellationToken cancellationToken)
    {
        Guid? userId = User.Identity.GetGuidUserId();

        GetReceiptsQuery query = new(pagable, (Guid)userId);

        return await _mediator.Send(query, cancellationToken);
    }
}

