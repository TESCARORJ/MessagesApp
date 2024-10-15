using MessagesApp.API.Extensions;
using MessagesApp.API.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddServicesConfig(builder.Configuration);

//Registrando um serviço de segundo plano
builder.Services.AddHostedService<MessageConsumerService>();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();
app.UseAuthorization();
app.MapControllers();
app.Run();
