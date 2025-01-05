using BussinessLogic.Repositories.Interfaces;
using BussinessLogic.Services.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Domain.Entities;

namespace BussinessLogic.Services.Implementations;

public class ModelService : IModelService
{
    private readonly IModelRepository _modelRepository;
    private readonly IConfiguration _configuration;

    public ModelService(IModelRepository modelRepository, IConfiguration configuration)
    {
        _modelRepository = modelRepository;
        _configuration = configuration;
    }

    public async Task<Model> AddModelService(Model model)
    {
        // Buscar el usuario por email
        var response = await  _modelRepository.AddModelByUser(model);

        return response;
    }
}