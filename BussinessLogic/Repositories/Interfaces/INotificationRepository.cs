using Domain.Entities;

namespace BussinessLogic.Repositories.Interfaces;

public interface INotificationRepository
{
    public Task<Notification> AddNotificationByUser(Notification notification);
    public Task<List<Notification>> GetNotificationsByUserId(int userId);

}