using Domain.Entities;

namespace BussinessLogic.Services.Interfaces;

public interface INotificationService
{
    Task<Notification> AddNotificationByUser(Notification notification);
    
    public Task<List<Notification>> GetNotificationsByUserId(int userId);
}