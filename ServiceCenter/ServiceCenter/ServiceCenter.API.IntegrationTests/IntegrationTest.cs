using Microsoft.EntityFrameworkCore;
using ServiceCenter.Infrastructure.Data.SqlServer.EfCore.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace ServiceCenter.API.IntegrationTests;

[TestCaseOrderer("Beta.API.IntegrationTests.AlphabeticalOrderer", "Beta.API.IntegrationTests")]
public class IntegrationTest
{
    private readonly DbContextOptions<ApplicationDbContext> _optionsDb;
    private readonly IServiceProvider _serviceProvider;

    protected readonly CustomWebApplicationFactory<Startup> appFactory;

    protected readonly HttpClient testClient;
    protected readonly ApplicationDbContext dbContext;

    protected string version = "1";
    protected string area = "Common";
    protected string controller = "Account";
    protected string subFolderUrl = null;

    protected ApplicationDbContext DbContext => new(_optionsDb);
    protected IServiceProvider ServiceProvider => _serviceProvider;

    protected IntegrationTest(CustomWebApplicationFactory<Startup> webApplicationFactory)
    {
        appFactory = webApplicationFactory;
        testClient = appFactory.CreateClient();
        dbContext = appFactory.DbContext;

        if (!webApplicationFactory.isDataInitialized)
        {
            //TestDataInitializer.InitializeData(dbContext);
            webApplicationFactory.isDataInitialized = true;
        }

        _serviceProvider = appFactory.Services;

        _optionsDb = new DbContextOptionsBuilder<ApplicationDbContext>()
                      .UseSqlServer("Data Source=.;Initial Catalog=TestDb;Integrated Security=true")
                      .Options;
    }


    protected string GetUrl(string webServiceName, params string[] values)
    {
        subFolderUrl = subFolderUrl is not null && !subFolderUrl.Contains('/') ? subFolderUrl + "/" : subFolderUrl;
        webServiceName = webServiceName is not null ? "/" + webServiceName : null;

        string url = "api/" + "v" + version + "/" + area + "/" + subFolderUrl + controller + webServiceName;

        if (values.Length != 0)
        {
            url += "?";

            for (int i = 0; i < values.Length; i += 2)
            {
                url = url + values[i] + "=" + values[i + 1] + "&";
            }
        }

        return url;
    }

    protected string GetOtherUrl(string version, string area, string controller, string action, string subFolderUrl = null)
    {
        subFolderUrl = subFolderUrl is not null && !subFolderUrl.Contains('/') ? subFolderUrl + "/" : subFolderUrl;
        return "api/" + "v" + version + "/" + area + "/" + subFolderUrl + controller + "/" + action;
    }
}
