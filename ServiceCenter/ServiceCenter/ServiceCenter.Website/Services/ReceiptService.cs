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

    public async Task<string> UploadImage(MultipartFormDataContent content)
    {
        var response = await _client.PostAsync("v1/Common/Upload", content);
        
        if (!response.IsSuccessStatusCode)
        {
            throw new ApplicationException(response.Content.ToString());
        }

        var postResult = await response.Content.ReadAsAsync<ApiResult<string>>();

        if(postResult.IsSuccess)
        {
            var imgUrl = Path.Combine("https://localhost:5011/", postResult.Data);
            return postResult.Data;
        }
        else
        {
            throw new ApplicationException(postResult.Message);
        }
    }

    public async Task<ReceiptDto> GetReceipt(long id)
    {
        var response = await _client.GetAsync($"v1/Users/Receipts?id={id}");

        if (!response.IsSuccessStatusCode)
        {
            throw new ApplicationException(response.Content.ToString());
        }

        var postResult = await response.Content.ReadAsAsync<ApiResult<ReceiptDto>>();

        if (postResult.IsSuccess)
        {
            return postResult.Data;
        }
        else
        {
            throw new ApplicationException(postResult.Message);
        }
    }

    public async Task UpdateReceipt(ReceiptDto dto)
    {
        HttpResponseMessage response = await _client.PutAsJsonAsync("v1/Users/Receipts", dto);

        if (!response.IsSuccessStatusCode)
        {
            throw new ApplicationException(response.Content.ToString());
        }

        var postResult = await response.Content.ReadAsAsync<ApiResult>();

        if (!postResult.IsSuccess)
        {
            throw new ApplicationException(postResult.Message);
        }
    }

    public async Task DeleteReceipt(long id)
    {
        var response = await _client.DeleteAsync($"v1/Users/Receipts?id={id}");

        if (!response.IsSuccessStatusCode)
        {
            throw new ApplicationException(response.Content.ToString());
        }

        var postResult = await response.Content.ReadAsAsync<ApiResult>();

        if (!postResult.IsSuccess)
        {
            throw new ApplicationException(postResult.Message);
        }
    }
}

