using Api.Middlewares;
using Api.Services;
using Core.Abstraction;
using Core.Domain;
using DataAccess;
using DataAccess.Repositories;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Converters;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers().AddMvcOptions(x =>
    x.SuppressAsyncSuffixInActionNames = false).AddNewtonsoftJson(options =>
    {
        options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
        options.SerializerSettings.Converters.Add(new StringEnumConverter());
    }
);
builder.Services.AddCors();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<WeatherDbContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("PostgresDB"));
});
builder.Services.AddScoped<WeatherRepository>();
builder.Services.AddScoped<ExcelReader>();


var app = builder.Build();

app.UseExceptionHandlerMiddleware();
app.UseStaticFiles();
app.MapControllers();

app.UseCors(x => x.WithOrigins("http://localhost:8080") // путь к нашему SPA клиенту
    .AllowCredentials()
    .AllowAnyMethod()
    .AllowAnyHeader());

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// app.UseHttpsRedirection();
app.UseForwardedHeaders(new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
});


app.Run();