using fugdj;
using fugdj.Integration;
using fugdj.Repositories;
using fugdj.Services;

const string corsDomainPolicy = "CorsDomainPolicy";

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddControllers(options => options.Filters.Add(new HttpExceptionFilter()));
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IRoomService, RoomService>();
builder.Services.AddScoped<IRoomRepository, RoomRepository>();

builder.Services.AddScoped<IDataSourceClient, DataSourceClient>();

builder.Services.AddCors(options =>
{
    options.AddPolicy(corsDomainPolicy,
        policy =>
        {
            policy.WithOrigins("http://localhost:3000")
                .AllowAnyHeader()
                .AllowAnyMethod();
        });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseCors(corsDomainPolicy);

app.UseAuthorization();

app.MapControllers();

app.Run();