global using API_CraftyOrnaments.Models;
global using Microsoft.EntityFrameworkCore;
using API_CraftyOrnaments.DAL.Contracts;
using API_CraftyOrnaments.DAL.Repository;
using API_CraftyOrnaments.Middlewares;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.FileProviders;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<CraftyOrnamentsContext>(options =>
options.UseSqlServer(builder.Configuration.GetConnectionString("mvcConnection")));
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

builder.Services.AddAuthentication();
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: "AllowOrigin",
        builder =>
        {
            builder.WithOrigins("http://localhost:4200")
                                .AllowAnyHeader()
                                .AllowAnyMethod();
        });
});
builder.Services.Configure<FormOptions>(o =>
{
    o.ValueLengthLimit = int.MaxValue;
    o.MultipartBodyLengthLimit = int.MaxValue;
    o.MemoryBufferThreshold = int.MaxValue;
});
builder.Services.AddControllers();

builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IPayments, PaymentsRepository>();
builder.Services.AddScoped<IAuthentication, Authentication>();
builder.Services.AddCors();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseMiddleware<ApiKeyMiddleware>();


app.MapControllers();
app.UseCors("AllowOrigin");
app.UseStaticFiles();
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), @"assets")),
    RequestPath = new PathString("/assets")
}) ;

app.Run();
