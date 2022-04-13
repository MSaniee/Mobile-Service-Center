using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using ServiceCenter.Domain.Core.Utilities.PagesSettings;

namespace ServiceCenter.WebFramework.API.Utilities.PageSettings;

public static class PageSettingsExtensionMethods
{
    public static void AddPaginationToHeader(this HttpResponse response, MetaData metaData)
    {
        response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(metaData));
    }
}

