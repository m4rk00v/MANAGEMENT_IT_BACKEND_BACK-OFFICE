using Domain.Entities;
using Microsoft.AspNetCore.Http;

public class ModelWithFileDto
{
    public ModelDto Model { get; set; } // Información del modelo
    public IFormFile File { get; set; } // Archivo .obj
}