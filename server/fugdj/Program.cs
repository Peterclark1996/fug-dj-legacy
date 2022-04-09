using fugdj;
using fugdj.Hubs;
using fugdj.Integration;
using fugdj.Repositories;
using fugdj.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;

const string corsDomainPolicy = "CorsDomainPolicy";

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
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
    
    app.UseCors(corsPolicyBuilder =>
    {
        corsPolicyBuilder.WithOrigins("http://localhost:3000")
            .AllowAnyHeader()
            .WithMethods("GET", "POST", "DELETE")
            .AllowCredentials();
    });
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseCors(corsDomainPolicy);

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