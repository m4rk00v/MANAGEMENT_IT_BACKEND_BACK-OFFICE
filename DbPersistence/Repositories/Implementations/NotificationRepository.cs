using System.Reflection.Metadata.Ecma335;
using BussinessLogic.Repositories.Interfaces;
using DbPersistence.Context;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace BussinessLogic.Repositories.Implementations;

public class NotificationRepository: INotificationRepository
{
    private readonly ApplicationDbContext _context;

    public NotificationRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Notification> AddNotificationByUser(Notification notification)
    {
        var entity = await _context.Notifications.AddAsync(notification);
        await _context.SaveChangesAsync();
        return entity.Entity;

    }
    
    public async Task<List<Notification>> GetNotificationsByUserId(int userId)
    {
        return await _context.Notifications
            .Where(n => n.UserId == userId && !n.IsReaded) 
            .ToListAsync();  
    }
}