using Xunit;

namespace ServiceCenter.API.IntegrationTests;

[CollectionDefinition("BetaAPI - Full Integration Test #1")]
public class WebApplicationFactoryCollection : ICollectionFixture<CustomWebApplicationFactory<Startup>>
{
    // This class has no code, and is never created. Its purpose is simply
    // to be the place to apply [CollectionDefinition] and all the
    // ICollectionFixture<> interfaces.
}
