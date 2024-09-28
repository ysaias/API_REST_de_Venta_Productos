using AutoMapper;
using begywebsapi.DTOs;
using begywebsapi.Models;
using Microsoft.Extensions.Options;

namespace begywebsapi.Utilidades
{
    public class AutoMapperProfiles
    {

        public class ProductoProfile : Profile
        {
            public ProductoProfile()
            {
                // Aquí defines el mapeo entre CreacionCategoriaDto y Producto
                CreateMap<CreacionCategoriaDto, Producto>()
                    .ForMember(x => x.ImagenUrl, option => option.Ignore());

                // Si tienes otros mapeos, puedes añadirlos aquí también
                CreateMap<Producto, ProductoDto>();  // Para el GET de productos, si lo necesitas
                
                CreateMap<Categoria, CategoriaDto>();
                
                CreateMap<CreacionCategoriaDto, Categoria>();

                CreateMap<Role, RoleDto>();

                CreateMap<CreacionRoleDto, Role>();

                CreateMap<Usaurio, UsuarioDto>();

                CreateMap<CreacionUsuarioDto, Usaurio>();

                CreateMap<Cart, CartDto>()
                    .ForMember(dest => dest.CartItems, opt => opt.MapFrom(src => src.CartItems));

                CreateMap<CartItem, CartItemDto>()
                    .ForMember(dest => dest.ProductoNombre, opt => opt.MapFrom(src => src.Producto.Nombre))
                    .ForMember(dest => dest.ProductoId, opt => opt.MapFrom(src => src.Producto.Id)); // Ajustar según el modelo de producto

                CreateMap<CreacionCartItemDto, CartItem>();
            }
        }

        
    }
}
