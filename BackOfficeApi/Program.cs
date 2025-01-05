using BussinessLogic.Repositories.Implementations;
using BussinessLogic.Repositories.Interfaces;
using BussinessLogic.Services.Implementations;
using BussinessLogic.Services.Interfaces;
using DbPersistence;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigins", policy =>
    {
        policy.WithOrigins("http://localhost:4200") // Agrega la URL de tu frontend
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});
builder.Services.AddPersistence(builder.Configuration); 

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IModelRepository, ModelRepostory>();
builder.Services.AddScoped<INotificationRepository, NotificationRepository>();
builder.Services.AddScoped<IModelService, ModelService>();
builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();
builder.Services.AddScoped<INotificationService, NotificationService>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseCors("AllowSpecificOrigins");

app.Urls.Add("http://*:5159");  // Puerto para HTTP
app.Urls.Add("https://*:7291"); // Puerto para HTTPS

// app.Urls.Add("http://*:7291"); // Cambia el puerto si es necesario



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



app.Run();

