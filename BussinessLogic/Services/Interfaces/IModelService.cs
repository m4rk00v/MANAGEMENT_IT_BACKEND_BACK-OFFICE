using Domain.Entities;

namespace BussinessLogic.Services.Interfaces;

public interface IModelService
{
    Task<Model> AddModelService(Model model);
}

