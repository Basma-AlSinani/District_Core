using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Linq;

namespace CrimeManagment.Helpers
{
    public class EnumSchemaFilter : ISchemaFilter
    {
        public void Apply(OpenApiSchema schema, SchemaFilterContext context)
        {
            if (context.Type.IsEnum)
            {
                schema.Description += "<br><b>Available Values:</b><br>" +
                    string.Join("<br>", Enum.GetNames(context.Type)
                    .Select(name => $"{Convert.ToInt32(Enum.Parse(context.Type, name))} = {name}"));

                schema.Enum.Clear();
                foreach (var value in Enum.GetValues(context.Type))
                {
                    schema.Enum.Add(new OpenApiInteger(Convert.ToInt32(value)));
                }
                schema.Type = "integer";
            }
        }
    }
}
