using FluentAssertions;
using ServiceCenter.Domain.Entities.UserAggregate;
using System;
using Xunit;

namespace ServiceCenter.UnitTests.Domain.Entities.UserAggregate;

public class ActivationCodeTest
{
    [InlineData(0, 60)]
    [InlineData(1, 90)]
    [Theory]
    public void SetExpireDate_WhenCall_ReturnExpectedValue(int step, int expectedAddSeconds)
    {
        //Arrange
        //Action
        ActivationCode code = new();
        code.SetExpireDate(step);

        //Assert
        code.ExpireDate.Should().BeSameDateAs(DateTime.Now.AddSeconds(expectedAddSeconds));
    }
}

