using Domain.Entities;

namespace BussinessLogic.Interfaces;

public interface ISecurityProxyService
{
    Task<User> GetUserById(int id);
    Task<User> GetUserByEmail(string email);
    Task<User> GetUserTokenById(int id, string bearerToken);
}