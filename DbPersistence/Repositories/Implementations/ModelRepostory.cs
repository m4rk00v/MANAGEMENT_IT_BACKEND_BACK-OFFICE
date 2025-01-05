using BussinessLogic.Repositories.Interfaces;
using DbPersistence.Context;
using Domain.Entities;

namespace BussinessLogic.Repositories.Implementations;

public class ModelRepostory : IModelRepository
{
    private readonly ApplicationDbContext _context;

    public ModelRepostory(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Model> AddModelByUser(Model model)
    {
        var entity = await _context.Models.AddAsync(model);
        await _context.SaveChangesAsync(); 
        return entity.Entity;
    }
}