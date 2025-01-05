using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities;

public class NotificationDto
{
    public int Id { get; set; }
    
    public string Message { get; set; }

    public int UserId { get; set; }

    public bool IsReaded { get; set; }

    public string Link { get; set; }
    
  

    public Notification GetModel(NotificationDto notificationDto)
    {
        Notification notification = new Notification();
        notification.Id = notificationDto.Id;
        notification.Message = notificationDto.Message;
        notification.UserId = notificationDto.UserId;
        notification.IsReaded = notificationDto.IsReaded;
        notification.Link = notificationDto.Link;
        return notification;
    }

}