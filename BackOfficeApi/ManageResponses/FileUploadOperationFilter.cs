using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

public class FileUploadOperationFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        if (operation.RequestBody?.Content.ContainsKey("multipart/form-data") == true)
        {
            var content = operation.RequestBody.Content["multipart/form-data"];
            content.Schema.Properties["file"] = new OpenApiSchema
            {
                Type = "string",
                Format = "binary",
                Description = "Upload a .obj file"
            };
        }
    }
}