using Domain.Entities;

namespace BussinessLogic.Repositories.Interfaces;

public interface IModelRepository
{
    public Task<Model> AddModelByUser(Model model);
}