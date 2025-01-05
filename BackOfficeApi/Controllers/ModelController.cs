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