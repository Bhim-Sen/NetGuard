using BLL.IService;
using BLL.Service;
using DAL.Entity;
using DAL.UnitOfWork;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using NetGuard.Environment;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddTransient<ICommonService, CommonService>();
builder.Services.AddTransient<IUnitOfWork, UnitOfWork>();
builder.Services.AddTransient<HttpClient>();

string connectionStringKey;
string connectionString;
if (ApiEnvironment.IsProduction())
{
	connectionStringKey = "NetGuard.Production.SqlConnectionString";
}
else if (ApiEnvironment.IsLocal())
{
	connectionStringKey = "NetGuard.Local.SqlConnectionString";
}
else
{
	connectionStringKey = "NetGuard.Dev.SqlConnectionString";
}
connectionString = builder.Configuration.GetRequiredSection($"ConnectionStrings:{connectionStringKey}").Get<string>()!;
builder.Services.AddDbContext<NetGuardDbContext>(options =>
{
	options.UseSqlServer(connectionString, sqlServerOptionsAction: builder => { });
});


//Swagger
builder.Services.AddSwaggerGen(opt =>
{
	opt.SwaggerDoc("v1", new OpenApiInfo { Title = "MyAPI", Version = "v1" });
	opt.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
	{
		In = ParameterLocation.Header,
		Description = "Please enter token",
		Name = "Authorization",
		Type = SecuritySchemeType.Http,
		BearerFormat = "JWT",
		Scheme = "bearer"
	});

	opt.AddSecurityRequirement(new OpenApiSecurityRequirement
	{
		{
			new OpenApiSecurityScheme
			{
				Reference = new OpenApiReference
				{
					Type=ReferenceType.SecurityScheme,
					Id="Bearer"
				}
			},
			new string[]{}
		}
	});
});
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
		  .AddJwtBearer("Bearer", cfg =>
		  {
			  cfg.TokenValidationParameters = new TokenValidationParameters()
			  {
				  ValidateIssuer = true,
				  ValidateAudience = false,
				  ValidateLifetime = true,
				  ValidateIssuerSigningKey = true,
				  ValidIssuer = builder.Configuration["Jwt:Issuer"],
				  ValidAudience = builder.Configuration["Jwt:Audience"],
				  IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!)),
			  };
		  });

builder.Services.AddCors(options =>
{
	options.AddPolicy("AllowSpecificOrigin",
		builder =>
		{
			builder.WithOrigins("*")
				   .AllowAnyHeader()
				   .AllowAnyMethod();
		});
});


var app = builder.Build();
app.UseCors("AllowSpecificOrigin");

app.UseSwagger();
app.UseSwaggerUI();
app.UseGlobalExceptionHandler();
app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
