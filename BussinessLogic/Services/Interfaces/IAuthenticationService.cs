namespace BussinessLogic.Services.Interfaces;


public interface IAuthenticationService
{ 
    Task<string> LoginService(string email, string password);
}
