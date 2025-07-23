using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using ToDoApi.Auth;
using ToDoApi.Data;

var builder = WebApplication.CreateBuilder(args);

// These config sources are added automatically:
// - appsettings.json
// - appsettings.{Environment}.json
// - Environment variables
// - Command line

var configuration = builder.Configuration;

// get DB connection
var connectionString = configuration.GetConnectionString("DefaultConnection");

//DBContext is not thread safe - use AddDbContext method instead of passing singleton!!!
builder.Services.AddDbContext<TodoDbContext>(options =>
    options.UseSqlServer(connectionString));


//Password Hash Context
builder.Services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();

// Add services to the container
builder.Services.AddControllers();

// Add Swagger support
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "ToDo API", Version = "v1" });

    // Add JWT support
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Enter: **Bearer your_token_here**"
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] { }
        }
    });
});

// Inject JwtService
builder.Services.AddSingleton<JwtService>();

// JWT Auth config
var jwtSettings = builder.Configuration.GetSection("jwt");
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtSettings["issuer"],
            ValidAudience = jwtSettings["audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["key"]!))
        };
    });


// Add global Validation Request Error message formatter
builder.Services.Configure<ApiBehaviorOptions> (options =>
{
    options.InvalidModelStateResponseFactory = context =>
    {
        var errors = context.ModelState
            .Where(e => e.Value?.Errors.Count > 0)
            .ToDictionary(
                e => e.Key,
                e => e.Value!.Errors.Select(err => err.ErrorMessage).ToArray()
            );

        return new BadRequestObjectResult(new
        {
            message = "Validation errors",
            errors
        });
    };
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

void SeedDatabase(WebApplication app)
{
    using var scope = app.Services.CreateScope() ;
    var dbContext = scope.ServiceProvider.GetRequiredService<TodoDbContext>();
    var passwordHasher = scope.ServiceProvider.GetRequiredService<IPasswordHasher<User>>();

    if (app.Environment.IsDevelopment() && !dbContext.Users.Any())
    {
        var adminUser = new User { Username = "admin", PasswordHash = "admin", Role = "ADMIN" };
        adminUser.PasswordHash = passwordHasher.HashPassword(adminUser, "admin");
        
        var userNayeem =  new User { Username = "nayeem", PasswordHash = "nayeem123", Role = "USER" };
        userNayeem.PasswordHash = passwordHasher.HashPassword(userNayeem, "nayeem123");
        
        dbContext.Users.AddRange(adminUser, userNayeem);
        dbContext.SaveChanges();
    }
}

SeedDatabase(app);
app.Run();
