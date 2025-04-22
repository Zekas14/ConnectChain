using Microsoft.EntityFrameworkCore;
using ConnectChain.Data.Context;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using MediatR;
using Microsoft.AspNetCore.Identity;
using ConnectChain.Models;
using ConnectChain.Data.Repositories.UserRepository;
using ConnectChain.Helpers;
using AutoMapper;
using System.Reflection;
using Autofac.Extensions.DependencyInjection;
using Autofac;
using ConnectChain.Config;
using ConnectChain.Settings;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.AspNetCore.Mvc;
using ConnectChain.ViewModel;
using ConnectChain.Features.SupplierManagement.Common.Queries;
using ConnectChain.Middlewares;
using ConnectChain.Filters;

var builder = WebApplication.CreateBuilder(args);

var config = builder.Configuration;
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddScoped<AuthorizationAttribute>();
builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
builder.Host.ConfigureContainer<ContainerBuilder>(container =>
{
    container.RegisterModule(new ApplicationModule());
});

builder.Services.Configure<MailSetting>(builder.Configuration.GetSection("MailSetting"));
builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.InvalidModelStateResponseFactory = context =>
    {
        var errors = context.ModelState
            .Where(e => e.Value!.Errors.Count > 0)
            .SelectMany(e => e.Value!.Errors.Select(err => err.ErrorMessage))
            .ToList();
        return new BadRequestObjectResult(new { ErrorCode.InvalidInput, errors});
    };
});

builder.Services.AddIdentity<User, IdentityRole>()
    .AddEntityFrameworkStores<ConnectChainDbContext>()
    .AddDefaultTokenProviders();
;
builder.Services.AddDbContext<ConnectChainDbContext>(optionsBuilder =>
{
    optionsBuilder.UseSqlServer(
        config.GetConnectionString("DefaultConnection"),
        options => options.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery)
        ).UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
}
);
builder.Services.AddMemoryCache();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IMailServices, MailServices>();
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly()));
builder.Services.AddAutoMapper(typeof(Profile));

builder.Services.Configure<CloudinarySettings>(builder.Configuration.GetSection("CloudinarySettings"));
builder.Services.AddScoped<CloudinaryService>();

builder.Services.AddSwaggerGen();
builder.Services.AddAuthentication(
    options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    }
    ).AddJwtBearer(options =>
    {
        options.SaveToken = true;
        options.RequireHttpsMetadata = true;
        options.TokenValidationParameters = new()
        {
            ValidateIssuer = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = config["JWT:Issuer"],
            ValidateAudience = true,
            ValidAudience = config["JWT:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["JWT:SecretKey"]!))
        };
    });
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigins",
        policy =>
        {
            policy.WithOrigins(
                "https://connectchain.runasp.net" 
            )
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials();
        });
});
var app = builder.Build();

//if (app.Environment.IsDevelopment())
//{

app.UseSwagger();
    app.UseSwaggerUI();
//}
app.UseCors("AllowSpecificOrigins");
AutoMapperServices.Mapper = app.Services.GetRequiredService<IMapper>();

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.UseMiddleware<GlobalErrorHandlerMiddleware>();
//app.UseMiddleware<GlobalErrorHandlerMiddleware>();

app.Run();
