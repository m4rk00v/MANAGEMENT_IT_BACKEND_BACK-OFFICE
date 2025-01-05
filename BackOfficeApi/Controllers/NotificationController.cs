using System.ComponentModel.DataAnnotations;
using BussinessLogic.Services.Interfaces;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace BackOfficeApi.Controllers;


[ApiController]
[Route("api/[controller]")]
public class NotificationController : ControllerBase
{
    private readonly INotificationService _notificationService;

    public NotificationController(INotificationService notificationService)
    {
        _notificationService = notificationService;
    }
    
    [HttpPost]
    public async Task<IActionResult> AddMNotification([FromBody] NotificationDto model)
    {
        try
        {
            await _notificationService.AddNotificationByUser(model.GetModel(model));
            
            //get notificatioon list 
            var notificationLists = await _notificationService.GetNotificationsByUserId(model.UserId);
            
            return Ok(ApiResponseFactory.Success(notificationLists, "Notification created"));
        }
        catch (ValidationException ex)
        {
            return BadRequest(ApiResponseFactory.Error<object>("Validation error", ex.Message));
        }
        catch (Exception ex)
        {
            return StatusCode(500, ApiResponseFactory.Error<object>("Unexpected error"));
        }
    }

    [HttpGet("health")]
    public async Task<IActionResult> GetMessage()
    {
        object obj = new object();
        return Ok(ApiResponseFactory.Success(obj, "it works"));
    }
    
    [HttpPost("SendNotification")]
    public async Task<IActionResult> ProcessNotification([FromBody] NotificationDto dto)
    {
        try
        {
            var notification = dto.GetModel(dto);
            // Validate that the notification object is not null
            if (notification == null || string.IsNullOrEmpty(notification.Message))
            {
                return BadRequest(new { message = "Invalid notification object." });
            }

            // Add the new notification
            notification.IsReaded = false; // Mark as unread
            await _notificationService.AddNotificationByUser(notification);

            
            // Retrieve all unread notifications for the user
            var unreadNotifications = await _notificationService.GetNotificationsByUserId(notification.UserId);

            return Ok(new
            {
                message = "Notification processed successfully.",
                unreadNotifications = unreadNotifications
            });
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error processing notification: {ex.Message}");
            return StatusCode(500, new { message = "An error occurred while processing the notification.", error = ex.Message });
        }
    }

}