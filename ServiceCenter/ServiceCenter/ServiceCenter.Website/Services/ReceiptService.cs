using ServiceCenter.Application.Dtos.Receipts;
using ServiceCenter.Domain.Core.Utilities.PagesSettings;
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

    public async Task<List<ReceiptDto>> GetReceipts()
    {
        Pagable pagable = new()
        {
            Page = 1,
            PageSize = 20,
            Search = null
        };

        var response = await _client.PostAsJsonAsync("v1/Users/Receipts/GetAll", pagable);

        //if(response.StatusCode == HttpStatusCode.OK)

        var result = await response.Content.ReadAsAsync<ApiResult<List<ReceiptDto>>>();

        if (!response.IsSuccessStatusCode)
        {
            throw new ApplicationException(response.Content.ToString());
        }

        return result.Data;
    }
}

