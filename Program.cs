using Microsoft.AspNetCore.Cors;
using Microsoft.EntityFrameworkCore;
using MoviesMinimalAPI;
using MoviesMinimalAPI.Endpoints;
using MoviesMinimalAPI.Repositories;
using MoviesMinimalAPI.Services;

var builder = WebApplication.CreateBuilder(args); // this permits the use of the WebApplication class

// db configuration
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer("name=DefaultConnection");
});

// cors configuration
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(configuration =>
    {
        configuration.WithOrigins(builder.Configuration.GetValue<string>("allowedOrigins")!)
            .AllowAnyHeader()
            .AllowAnyMethod();
    });

    options.AddPolicy("free", configuration =>
    {
        configuration.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
    });
});

// cache configuration
builder.Services.AddOutputCache();

// swagger configuration
builder.Services.AddEndpointsApiExplorer(); // this adds the endpoint api explorer for swagger recognize the endpoints
builder.Services.AddSwaggerGen(); // this adds the swagger generator

// add gender repository
builder.Services.AddScoped<IGenderRepository, GenderRepository>(); // means that service is created once per request
builder.Services.AddScoped<IActorRepository, ActorRepository>();

// add file storage service
builder.Services.AddScoped<IFileStorage, LocalFileStorage>();
builder.Services.AddHttpContextAccessor(); // this adds the http context accessor

builder.Services.AddAutoMapper(typeof(Program)); // this adds the automapper

var app = builder.Build(); // this creates the app instance

// swagger use
app.UseSwagger();
app.UseSwaggerUI();

app.UseStaticFiles(); // this permits the use of static files

// cors use
app.UseCors();

// cache use
app.UseOutputCache();

// this only endpoint use free policy
app.MapGet("/", [EnableCors(PolicyName = "free")] () => "Hello World!"); // this is a simple route

// map groups configurations
app.MapGroup("/genders").MapGenders();
app.MapGroup("/actors").MapActors();

app.Run(); // this starts the app
