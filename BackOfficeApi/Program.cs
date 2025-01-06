using BussinessLogic.Repositories.Implementations;
using BussinessLogic.Repositories.Interfaces;
using BussinessLogic.Services.Implementations;
using BussinessLogic.Services.Interfaces;
using DbPersistence;
using Microsoft.AspNetCore.SignalR;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.WebHost.ConfigureKestrel(serverOptions =>
{
    serverOptions.Limits.MaxRequestBodySize = 838860800; // 800 MB en bytes
});


builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigins", policy =>
    {
        policy.WithOrigins("http://localhost:4200") // Agrega la URL de tu frontend
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials(); // Permite el uso de credenciales
    });
});
builder.Services.AddPersistence(builder.Configuration); 

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IModelRepository, ModelRepostory>();
builder.Services.AddScoped<INotificationRepository, NotificationRepository>();
builder.Services.AddScoped<IModelService, ModelService>();
builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();
builder.Services.AddScoped<INotificationService, NotificationService>();

builder.Services.AddSignalR(); // <-- Mover aquí antes de app.MapHub

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "API Documentation",
        Version = "v1"
    });

    // Agregar el filtro para soportar subida de archivos
    c.OperationFilter<FileUploadOperationFilter>();
});

var app = builder.Build();

app.UseCors("AllowSpecificOrigins");

app.Urls.Add("http://*:5159");  // Puerto para HTTP
app.Urls.Add("https://*:7291"); // Puerto para HTTPS

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}



app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapHub<NotificationHub>("/notificationHub"); // Ruta para SignalR

app.Run();