using AutoMapper;
using MeetingsApplication.DAL;
using MeetingsApplication.Services;
using MeetingsApplication.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<UserService>();
builder.Services.AddSingleton<IRepository<User>, InMemoryRepository<User>>();
builder.Services.AddScoped<MeetingsService>();
builder.Services.AddSingleton<IRepository<Meeting>, InMemoryRepository<Meeting>>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
