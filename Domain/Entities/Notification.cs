using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Domain.Entities;

public class Notification
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    public string Message { get; set; }

    public int UserId { get; set; }

    public bool IsReaded { get; set; }

    public string Link { get; set; }

    public virtual User User { get; set; }

    // Método para convertir un DTO a una entidad
    public static Notification FromDto(NotificationDto notificationDto)
    {
        return new Notification
        {
            Id = notificationDto.Id,
            Message = notificationDto.Message,
            UserId = notificationDto.UserId,
            IsReaded = notificationDto.IsReaded,
            Link = notificationDto.Link
        };
    }
}