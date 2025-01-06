using Microsoft.AspNetCore.SignalR;

public class NotificationHub : Hub
{
    // Método que los clientes pueden llamar
    public async Task SendNotification(string userId, object notification)
    {
        // Envía la notificación al usuario específico
        // await Clients.All(userId).SendAsync("ReceiveNotification", notification);
        await Clients.All.SendAsync("ReceiveNotification", notification);
    }
}