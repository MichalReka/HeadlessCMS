using Azure.Storage.Blobs;
using HeadlessCMS.ApplicationCore;
using HeadlessCMS.ApplicationCore.Core.Configurations;
using HeadlessCMS.Domain;
using HeadlessCMS.Domain.Entities;
using HeadlessCMS.Persistence;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.OData;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Reflection;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var configuration = new ConfigurationBuilder()
               .SetBasePath(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location))
               .AddJsonFile("appsettings.json")
               .Build();

builder.Services.AddSQLDbContext(builder.Configuration);
builder.Services.AddDomainServices(builder.Configuration);
builder.Services.AddApplicationCoreServices(builder.Configuration);
builder.Services.AddAutoMapper(typeof(MapperProfile));
builder.Services.AddHttpContextAccessor();
builder.Services.AddControllers().AddOData(options => options.Select().Filter().OrderBy());
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(
        builder =>
        {
            builder.WithOrigins("http://localhost:4200");
            builder.AllowAnyMethod();
            builder.AllowAnyHeader();
        });
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
JwtBearerOptions options(JwtBearerOptions jwtBearerOptions)
{
    jwtBearerOptions.RequireHttpsMetadata = false;
    jwtBearerOptions.SaveToken = true;
    jwtBearerOptions.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        ValidIssuer = @"https://localhost:44316",
        ValidAudience = @"https://localhost:44316",
        ValidateAudience = true,
        ValidateIssuer = true,
        ValidateLifetime = true, //validate the expiration and not before values in the token
        ClockSkew = TimeSpan.FromMinutes(1), //1 minute tolerance for the expiration date
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration.GetSection("Token").Value)),
    };
    //if (audience == "access")
    //{
    //    jwtBearerOptions.Events = new JwtBearerEvents
    //    {
    //        OnAuthenticationFailed = context =>
    //        {
    //            if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
    //            {
    //                context.Response.Headers.Add("Token-Expired", "true");
    //            }
    //            return Task.CompletedTask;
    //        }
    //    };
    //}
    return jwtBearerOptions;
}

builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(jwtBearerOptions => options(jwtBearerOptions))
.AddJwtBearer("refresh", jwtBearerOptions => options(jwtBearerOptions));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "MyProject", Version = "v1.0.0" });

    var securitySchema = new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        Reference = new OpenApiReference
        {
            Type = ReferenceType.SecurityScheme,
            Id = "Bearer"
        }
    };

    c.AddSecurityDefinition("Bearer", securitySchema);

    var securityRequirement = new OpenApiSecurityRequirement
                {
                    { securitySchema, new[] { "Bearer" } }
                };

    c.AddSecurityRequirement(securityRequirement);
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.UseCors();

app.Run();
// Configure the HTTP request pipeline.