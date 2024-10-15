using begywebsapi;
using begywebsapi.Filtro;
using begywebsapi.Models;
using begywebsapi.Utilidades;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.




builder.Services.AddAutoMapper(typeof(Program));

//Cuando esté listo la cuenta azure, solo cambiar AlmacenadorArchivosLocal por AlmacenadorAzureStorage
builder.Services.AddTransient<IAlmacenadorArchivos, AlmacenadorArchivosLocal>();
builder.Services.AddHttpContextAccessor();

builder.Services.AddDbContext<VentasbegyContext>(options =>
options.UseSqlServer(builder.Configuration.GetConnectionString("defaultConnection")));

// Configurar CORS si el frontend estará en otro servidor
builder.Services.AddCors(options =>
{
    var frontendURL = builder.Configuration.GetValue<string>("frontend_url");
    options.AddDefaultPolicy(builder =>
    {
        builder.WithOrigins(frontendURL)
               .AllowAnyMethod()
               .AllowAnyHeader()
               .WithExposedHeaders(new string[] { "cantidadtotalregistros" });
    });
});

builder.Services.AddIdentity<IdentityUser, IdentityRole>()
    .AddEntityFrameworkStores<VentasbegyContext>()
    .AddDefaultTokenProviders();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(opciones => 
       opciones.TokenValidationParameters = new TokenValidationParameters   
       {
           ValidateIssuer = false,
           ValidateAudience = false,
           ValidateLifetime = true,
           ValidateIssuerSigningKey = true,
           IssuerSigningKey = new SymmetricSecurityKey(
               Encoding.UTF8.GetBytes(builder.Configuration["llaveJwt"])),
           ClockSkew = TimeSpan.Zero
      
       } 
       
);


builder.Services.AddAuthorization(opciones =>
{
    opciones.AddPolicy("EsAdmin", policy =>
    policy.RequireClaim("http://schemas.microsoft.com/ws/2008/06/identity/claims/role", "admin"));
});


builder.Services.AddControllers(option =>
      { option.Filters.Add(typeof(FiltroDeExcepcion));
        option.Filters.Add(typeof(ParsearBadRequets));

});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseCors();

app.UseStaticFiles();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
