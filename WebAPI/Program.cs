using AutoMapper;
using Business.Abstract;
using Business.Concrete;
using Business.Mapping;
using DataAccess;
using DataAccess.Abstract;
using DataAccess.Concrete;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo{Title = "File Management API", Version = "v1"});
});

builder.Services.AddSingleton<FileManagementContext>();
builder.Services.AddDbContext<FileManagementDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("FileManagementDbContext")));


var mapperConfiguration = new MapperConfiguration(mc =>
{
    mc.AddMaps("Business");
});

var mapper = mapperConfiguration.CreateMapper();
builder.Services.AddSingleton(mapper);

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<FileRepository>();
builder.Services.AddScoped<IFileService, FileManager>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins", policy =>
    {
        policy
            .AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

builder.Services.AddHttpContextAccessor();


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("AllowAllOrigins");


app.UseAuthorization();

app.MapControllers();

app.Run();

