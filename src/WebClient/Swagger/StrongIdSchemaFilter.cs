using System.ComponentModel;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace WebClient.Swagger;

[EditorBrowsable(EditorBrowsableState.Never)]
internal sealed class StrongIdSchemaFilter : ISchemaFilter
{
    public void Apply(OpenApiSchema schema, SchemaFilterContext context)
    {
        var strongIdTypes = Ruby.Common.Ruby.Assemblies
            .SelectMany(assembly => assembly.GetTypes())
            .Where(type => type.Namespace == "Ruby.Generated")
            .Where(type => type.IsValueType)
            .Where(type => type.Name.EndsWith("id", StringComparison.InvariantCultureIgnoreCase))
            .ToList();

        if (strongIdTypes.Contains(context.Type))
        {
            schema.Type = "string";
        }
    }
}