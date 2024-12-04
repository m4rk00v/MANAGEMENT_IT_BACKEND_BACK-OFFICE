using Domain.Entities;

namespace BussinessLogic.Repositories.Interfaces;


public interface IUserRepository
{ 
    public Task<User> GetUserByEmail(string email);
    public Task<User> GetUserById(int id);
    
}
