using System;
using System.Threading.Tasks;
using fugdj;
using fugdj.Hubs;
using fugdj.Integration;
using fugdj.Repositories;
using fugdj.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddControllers(options => options.Filters.Add(new HttpExceptionFilter()));
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
    {
        options.Authority = builder.Configuration["Auth0:Domain"];
        options.Audience = builder.Configuration["Auth0:Audience"];
        options.Events = new JwtBearerEvents
        {
            OnMessageReceived = context =>
            {
                if (context.Request.Path.ToString().StartsWith("/hub/"))
                    context.Token = context.Request.Query["access_token"];
                return Task.CompletedTask;
            }
        };
    });

builder.Services.AddScoped<IRoomService, RoomService>();
builder.Services.AddScoped<IRoomRepository, RoomRepository>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IDataSourceClient, DataSourceClient>();
builder.Services.AddScoped<IYoutubeClient, YoutubeClient>();

builder.Services.AddHttpClient();
builder.Services.AddSignalR();

var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    Console.WriteLine("Starting in development mode");
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();

    app.UseCors(corsPolicyBuilder =>
    {
        corsPolicyBuilder.WithOrigins("http://localhost:3000")
            .WithMethods("GET", "POST", "DELETE", "PUT", "PATCH")
            .AllowAnyHeader()
            .AllowCredentials();
    });
}
if (app.Environment.IsProduction())
{
    Console.WriteLine("Starting in production mode");
    app.UseCors(corsPolicyBuilder =>
    {
        corsPolicyBuilder.WithOrigins("https://fug-dj.herokuapp.com")
            .WithMethods("GET", "POST", "DELETE", "PUT", "PATCH")
            .AllowAnyHeader()
            .AllowCredentials();
    });
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseCors();
app.UseDefaultFiles();
app.UseStaticFiles();
app.UseAuthentication();
app.UseAuthorization();
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
        "default",
        "{controller}/{action=Index}/{id?}");
});
app.MapControllers();
app.MapHub<RoomHub>("/hub/room");
app.Run();