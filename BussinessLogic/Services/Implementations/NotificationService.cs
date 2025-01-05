using BussinessLogic.Repositories.Interfaces;
using BussinessLogic.Services.Interfaces;
using Domain.Entities;
using Microsoft.Extensions.Configuration;

namespace BussinessLogic.Services.Implementations;

public class NotificationService : INotificationService
{
    private readonly INotificationRepository _notificationRepository;
    private readonly IConfiguration _configuration;

    public NotificationService(INotificationRepository notificationRepository, IConfiguration configuration)
    {
        _notificationRepository = notificationRepository;
        _configuration = configuration;
    }

    public async Task<Notification> AddNotificationByUser(Notification notification)
    {
        var response = await  _notificationRepository.AddNotificationByUser(notification);
        return response;
    }

    public async Task<List<Notification>> GetNotificationsByUserId(int userId)
    {
        var notificationList = await _notificationRepository.GetNotificationsByUserId(userId);
        return notificationList;
    }

}