using ServiceCenter.Application.Dtos.Receipts;
using ServiceCenter.Domain.Core.Utilities.PagesSettings;
using ServiceCenter.Website.Features;
using ServiceCenter.Website.Interfaces;
using System.Net.Http.Json;
using System.Text.Json;

namespace ServiceCenter.Website.Services;

public class ReceiptService : IReceiptService
{
    private readonly HttpClient _client;
    private readonly JsonSerializerOptions _options;
    public ReceiptService(HttpClient client)
    {
        _client = client;
        _options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
    }

    public async Task<PagingResponse<ReceiptDto>> GetReceipts(Pagable pagable)
    {
        HttpResponseMessage response = await _client.PostAsJsonAsync("v1/Users/Receipts/GetAll", pagable);

        if (!response.IsSuccessStatusCode)
        {
            throw new ApplicationException(response.Content.ToString());
        }

        var result = await response.Content.ReadAsAsync<ApiResult<List<ReceiptDto>>>();

        if (!result.IsSuccess)
        {
            throw new ApplicationException(result.Message);
        }

        PagingResponse<ReceiptDto> pagingResponse = new()
        {
            Items = result.Data,
            MetaData = JsonSerializer.Deserialize<MetaData>(response.Headers.GetValues("X-Pagination").First(), _options)
        };

        return pagingResponse;
    }

    public async Task CreateReceipt(ReceiptDto dto)
    {
        HttpResponseMessage response = await _client.PostAsJsonAsync("v1/Users/Receipts", dto);

        if (!response.IsSuccessStatusCode)
        {
            throw new ApplicationException(response.Content.ToString());
        }

        var postResult = await response.Content.ReadAsAsync<ApiResult<ReceiptDto>>();

        if (!postResult.IsSuccess)
        {
            throw new ApplicationException(postResult.Message);
        }
    }
}

