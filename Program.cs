using ChatApp.Data;
using ChatApp.Hubs;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSignalR();
builder.Services.AddControllers();
builder.Services.AddDbContext<ChatContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Configure CORS
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.WithOrigins("http://localhost:3000")
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
    });
});

var app = builder.Build();

// Enable Swagger for both development and production environments
app.UseSwagger();
app.UseSwaggerUI();

app.UseCors(); 
app.UseExceptionHandler("/Error");
app.UseHsts();
app.UseHttpsRedirection();
app.UseStaticFiles(); // Serve static files from wwwroot
app.UseRouting();
app.MapControllers();
app.MapHub<ChatHub>("/chathub");

// Handle SPA routing in production
if (!app.Environment.IsDevelopment())
{
    app.MapFallbackToFile("index.html");
}

// Add detailed logging to diagnose issues
app.Use(async (context, next) =>
{
    app.Logger.LogInformation($"Request Path: {context.Request.Path}");
    app.Logger.LogInformation($"Request QueryString: {context.Request.QueryString}");
    await next();
    app.Logger.LogInformation($"Response Status Code: {context.Response.StatusCode}");
});

app.Run();