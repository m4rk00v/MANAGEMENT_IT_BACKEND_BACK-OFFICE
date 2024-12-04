using BussinessLogic.Interfaces;
using BussinessLogic.Repositories.Interfaces;
using DbPersistence.Context;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace BussinessLogic.Repositories.Implementations;

public class UserRepository : IUserRepository
{
    private readonly ApplicationDbContext _context;

    public UserRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<User> GetUserByEmail(string email)
    {
        return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
    }

    public async Task<User> GetUserById(int id)
    {
        return await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
    }
}