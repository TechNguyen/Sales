
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Sale.Domain;
using Sale.Domain.Entities;
using Sale.Repository.Core;
using Sale.Service.Core;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddSwaggerGen(option =>
{
	option.CustomSchemaIds(type => type.ToString());
	option.SwaggerDoc("v1", new OpenApiInfo { Title = "API", Version = "v1" });
	option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
	{
		In = ParameterLocation.Header,
		Description = "Please enter a valid token",
		Name = "Authorization",
		Type = SecuritySchemeType.Http,
		BearerFormat = "JWT",
		Scheme = "Bearer"
	});
	option.AddSecurityRequirement(new OpenApiSecurityRequirement
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

builder.Services.AddDbContext<SaleContext>(options =>
{
	options.UseSqlServer(builder.Configuration.GetConnectionString("ConStr"));
});


builder.Services.AddIdentity<AppUser, AppRole>(options =>
{
	options.Password.RequireDigit = true;
	options.Password.RequireLowercase = true;
	options.Password.RequiredLength = 8;
	options.Password.RequireUppercase = true;
	options.Password.RequireNonAlphanumeric = true;

	// User setting
	options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-_";

}).AddEntityFrameworkStores<SaleContext>().AddDefaultTokenProviders();



builder.Services.AddAuthentication(option =>
{
	option.DefaultSignOutScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
	options.TokenValidationParameters = new TokenValidationParameters
	{
		ValidateIssuer = true,
		ValidIssuer = builder.Configuration["JWT:ValidIssuer"],
		ValidateAudience = true,
		ValidAudience = builder.Configuration["JWT:ValidAudience"],
		ValidateIssuerSigningKey = true,
		IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(builder.Configuration["JWT:Secret"])),

	};
});

// repo setting
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

var repositoryTypes = typeof(IRepository<>).Assembly.GetTypes().Where(x => !string.IsNullOrEmpty(x.Namespace) && x.Namespace.StartsWith("Sale.Repository") && x.Name.EndsWith("Repository"));
foreach (var repo in repositoryTypes.Where(t => t.IsInterface))
{
	var impl = repositoryTypes.FirstOrDefault(c => c.IsClass && repo.Name.Substring(1) == c.Name);
	if (impl != null) builder.Services.AddScoped(repo, impl); 
}

builder.Services.AddScoped(typeof(IService<>), typeof(Service<>));
var serviceTypes = typeof(IService<>).Assembly.GetTypes().Where(x => !string.IsNullOrEmpty(x.Namespace) && x.Namespace.StartsWith("Sale.Service") && x.Name.EndsWith("Service"));
foreach (var serc in serviceTypes.Where(t => t.IsInterface))
{
	var impl = repositoryTypes.FirstOrDefault(c => c.IsClass && serc.Name.Substring(1) == c.Name);
	if (impl != null) builder.Services.AddScoped(serc, impl);
}

builder.Services.AddHttpContextAccessor();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseHttpsRedirection();


app.UseAuthentication();

app.UseRouting();

app.UseAuthorization();


app.UseCors(x => x
	 .AllowAnyMethod()
	 .AllowAnyHeader()
	 .AllowCredentials()
	 .SetIsOriginAllowed(origin => true));


app.MapControllers();

app.Run();
