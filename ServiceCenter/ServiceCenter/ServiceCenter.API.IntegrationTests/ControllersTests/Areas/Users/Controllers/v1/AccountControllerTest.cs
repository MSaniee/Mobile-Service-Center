using FluentAssertions;
using ServiceCenter.Application.Dtos.ActivationCodes;
using ServiceCenter.Domain.Entities.UserAggregate;
using ServiceCenter.WebFramework.API.Bases;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Xunit;

namespace ServiceCenter.API.IntegrationTests.ControllersTests.Areas.Users.Controllers.v1;

[Collection("BetaAPI - Full Integration Test #1")]
public class AccountControllerTest : IntegrationTest
{
    public AccountControllerTest(CustomWebApplicationFactory<Startup> webApplicationFactory)
        : base(webApplicationFactory)
    {
        version = "1";
        area = "Users";
        controller = "Account";
    }

    #region SendActivationCode
    [Fact]
    public async Task SendActivationCode_WhenCall_ReturnSuccess()
    {
        //Arrange
        //Act
        SendActivationCodeDto dto = new()
        {
            PhoneNumber = "09121112233"
        };

        var response = await testClient.PostAsJsonAsync(GetUrl("SendActivationCode"), dto);

        //Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var result = await response.Content.ReadAsAsync<ApiResult<SendActivationCodeResultDto>>();

        result.Data.CodeExpirationRemainingTime.Should().Contain(":");

        ActivationCode activationCode = DbContext.Set<ActivationCode>()
            .Where(u => u.PhoneNumber == dto.PhoneNumber)
            .First();

        activationCode.Step.Should().Be(0);
        activationCode.Used.Should().BeFalse();
        activationCode.ExpireDate.Should().BeBefore(DateTime.Now.AddSeconds(60));
        activationCode.ExpireDate.Should().BeAfter(DateTime.Now);
        activationCode.Code.Should().Be(1234);
    }
    #endregion
}
