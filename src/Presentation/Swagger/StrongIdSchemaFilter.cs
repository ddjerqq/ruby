using System.ComponentModel;
using System.Reflection;
using Microsoft.OpenApi.Models;
using Presentation.Common;
using Ruby.Generated;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Presentation.Swagger;

[EditorBrowsable(EditorBrowsableState.Never)]
internal sealed class StrongIdSchemaFilter : ISchemaFilter
{
    public void Apply(OpenApiSchema schema, SchemaFilterContext context)
    {
        Assembly[] assemblies =
        [
            Domain.Domain.Assembly,
            Application.Application.Assembly,
            Infrastructure.Infrastructure.Assembly,
            Presentation.Assembly,
        ];

        var strongIdTypes = assemblies
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