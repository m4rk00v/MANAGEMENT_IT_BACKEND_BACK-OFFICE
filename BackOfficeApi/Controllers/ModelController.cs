using System.ComponentModel.DataAnnotations;
using BussinessLogic.Services.Interfaces;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace BackOfficeApi.Controllers;


[ApiController]
[Route("api/[controller]")]
public class ModelController : ControllerBase
{
    private readonly IModelService _modelService;

    public ModelController(IModelService modelService)
    {
        _modelService = modelService;
    }
    
    [HttpPost]
    public async Task<IActionResult> AddModel([FromBody] ModelDto model)
    {
        try
        {
            var entityResponse = await _modelService.AddModelService(model.GetModel(model));
            return Ok(ApiResponseFactory.Success(entityResponse, "Model created"));
        }
        catch (ValidationException ex)
        {
            return BadRequest(ApiResponseFactory.Error<object>("Validation error", ex.Message));
        }
        catch (Exception ex)
        {
            return StatusCode(500, ApiResponseFactory.Error<object>("Unexpected error"));
        }
    }
    [HttpPost("objRegister")]
    
    public async Task<IActionResult> SaveObjFile()
    {
        try
        {
            using (var memoryStream = new MemoryStream())
            {
                // Copiar el cuerpo de la solicitud al stream
                await Request.Body.CopyToAsync(memoryStream);

                if (memoryStream.Length == 0)
                {
                    return BadRequest(new { message = "El archivo .obj está vacío o no es válido." });
                }

                // Obtener el nombre del archivo del encabezado (si está presente)
                var fileName = Request.Headers["X-File-Name"].ToString();
                // if (string.IsNullOrEmpty(fileName) || !fileName.EndsWith(".obj", StringComparison.OrdinalIgnoreCase))
                // {
                //     fileName = $"uploaded_{Guid.NewGuid()}.obj"; // Nombre genérico si no se proporciona
                // }

                // Definir la ruta final donde se guardará el archivo .obj
                var finalSavePath = Path.Combine("/Users/appleuser/Desktop/bordeaux/ManagementIT/models", fileName+".obj");

                // Guardar el archivo en la ubicación especificada
                await System.IO.File.WriteAllBytesAsync(finalSavePath, memoryStream.ToArray());

                return Ok(new { message = "Archivo .obj guardado exitosamente.", path = finalSavePath });
            }
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Ocurrió un error al procesar el archivo .obj.", error = ex.Message });
        }
    }



    [HttpGet("health")]
    public async Task<IActionResult> GetMessage()
    {
        object obj = new object();
        return Ok(ApiResponseFactory.Success(obj, "it works"));
    }

    [HttpPost("ZipReceiver")]
    public async Task<IActionResult> ProcessZipFile()
    {
        // Step 1: Read the request body into a memory stream
        using (var memoryStream = new MemoryStream())
        {
            await Request.Body.CopyToAsync(memoryStream);

            if (memoryStream.Length == 0)
            {
                return BadRequest(new { message = "Invalid ZIP file." });
            }

            // Log the size of the received ZIP file
            Console.WriteLine($"Received ZIP file size: {memoryStream.Length} bytes");

            // Extract the original file name from the headers
            var originalFileName = Request.Headers["X-File-Name"].ToString();
            if (string.IsNullOrEmpty(originalFileName))
            {
                return BadRequest(new { message = "Original filename is required." });
            }

            // Reset the stream position to the beginning
            memoryStream.Seek(0, SeekOrigin.Begin);

            // At this point, memoryStream contains the ZIP file ready to be processed or sent
            Console.WriteLine($"ZIP file '{originalFileName}' is ready for further processing.");

            return Ok(new { message = "ZIP file received successfully!", fileName = originalFileName });
        }
    }
}