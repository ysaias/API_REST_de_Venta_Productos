using AutoMapper;
using begywebsapi.DTOs;
using begywebsapi.Models;
using begywebsapi.Utilidades;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace begywebsapi.Controllers
{
    [Route("api/cuentas")]
    [ApiController]
    public class CuentasController : ControllerBase
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly SignInManager<IdentityUser> signInManager;
        private readonly IConfiguration configuration;
        private readonly VentasbegyContext context;
        private readonly IMapper mapper;

        public CuentasController(UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            IConfiguration configuration, VentasbegyContext context,
            IMapper mapper)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.configuration = configuration;
            this.context = context;
            this.mapper = mapper;
        }

        [HttpGet("listadoUsuarios")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "EsAdmin")]
        public async Task<ActionResult<List<UsuarioDto>>> ListadoUsuarios([FromQuery] PaginacionDTO paginacionDTO)
        {
            var queryable = context.Usaurios.AsQueryable();
            await HttpContext.InsertarParametrosPaginacionEnCabecera(queryable);
            var usuarios = await queryable.OrderBy(x => x.Email).Paginar(paginacionDTO).ToListAsync();
            return mapper.Map<List<UsuarioDto>>(usuarios);
        }

        [HttpPost("HacerAdmin")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "EsAdmin")]
        public async Task<ActionResult> HacerAdmin([FromBody] string usuarioId)
        {
            var usuario = await userManager.FindByIdAsync(usuarioId);
            await userManager.AddClaimAsync(usuario, new Claim("role", "admin"));
            return NoContent();
        }

        [HttpPost("RemoverAdmin")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "EsAdmin")]
        public async Task<ActionResult> RemoveAdmin([FromBody] string usuarioId)
        {
            var usuario = await userManager.FindByIdAsync(usuarioId);
            await userManager.RemoveClaimAsync(usuario, new Claim("role", "admin"));
            return NoContent();
        }



        [HttpPost("crear")]
        public async Task<ActionResult<RespuestaAutenticacion>> Crear([FromBody] CreacionCredencialUsuarioDto creacionUsuarioDto)
        {
            var usuario = new IdentityUser { UserName = creacionUsuarioDto.UserName, Email = creacionUsuarioDto.Email, PhoneNumber = creacionUsuarioDto.Telefono };
            var resultado = await userManager.CreateAsync(usuario, creacionUsuarioDto.Password);

            if (resultado.Succeeded)
            {
                return await ConstruirTokenPrimeraVez(creacionUsuarioDto);
            }
            else
            {
                return BadRequest(resultado.Errors);
            }

        }

        [HttpPost("login")]
        public async Task<ActionResult<RespuestaAutenticacion>> Login([FromBody] CredencialesUsuario credenciales)
        {
            var resultdo = await signInManager.PasswordSignInAsync(credenciales.Email,
                credenciales.Password, isPersistent: false, lockoutOnFailure: false);

            if (resultdo.Succeeded)
            {
                return await ConstruirToken(credenciales);
            }
            else if (resultdo.IsLockedOut)
            {
                return BadRequest("La cuenta está bloqueada.");
            }
            else if (resultdo.IsNotAllowed)
            {
                return BadRequest("No está permitido el acceso.");
            }
            else
            {
                return BadRequest("Login incorrecto.");
            }

        }

        private async Task<RespuestaAutenticacion> ConstruirToken(CredencialesUsuario credenciales)
        {
            var usuario = await userManager.FindByEmailAsync(credenciales.Email);
            var claims = new List<Claim>()
            {
                new Claim("email", credenciales.Email),
                new Claim("NombreUsuario", usuario.UserName) // Incluye el UserName como NombreUsuario en los claims
            };

            var claimsDB = await userManager.GetClaimsAsync(usuario);
            claims.AddRange(claimsDB);

            var llave = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["llavejwt"]));
            var creds = new SigningCredentials(llave, SecurityAlgorithms.HmacSha256);

            var expiracion = DateTime.UtcNow.AddYears(1);

            var token = new JwtSecurityToken(issuer: null, audience: null, claims: claims,
                expires: expiracion, signingCredentials: creds);

            return new RespuestaAutenticacion()
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                Expiracion = expiracion
            };
        }


        private async Task<RespuestaAutenticacion> ConstruirTokenPrimeraVez(CreacionCredencialUsuarioDto credenciales)
        {
            var claims = new List<Claim>()
            {
                new Claim("email", credenciales.Email),
                new Claim("NombreUsuario", credenciales.UserName)
            };

            var usuario = await userManager.FindByEmailAsync(credenciales.Email);
            var climaDB = await userManager.GetClaimsAsync(usuario);

            claims.AddRange(climaDB);

            var llave = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["llavejwt"]));
            var creds = new SigningCredentials(llave, SecurityAlgorithms.HmacSha256);

            var expiracion = DateTime.UtcNow.AddYears(1);

            var token = new JwtSecurityToken(issuer: null, audience: null, claims: claims,
                expires: expiracion, signingCredentials: creds);

            return new RespuestaAutenticacion()
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                Expiracion = expiracion
            };

        }
    }
}