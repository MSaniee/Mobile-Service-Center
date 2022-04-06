using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Linq;

namespace ServiceCenter.WebFramework.API.StartupClassConfigurations.Swagger
{
    public class RemoveVersionParameters : Swashbuckle.AspNetCore.SwaggerGen.IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            // Remove version parameter from all Operations
            var versionParameter = operation.Parameters.SingleOrDefault(p => p.Name == "version");
            if (versionParameter != null)
                operation.Parameters.Remove(versionParameter);
        }
    }
}
