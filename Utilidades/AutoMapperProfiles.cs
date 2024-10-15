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
                // Aquí se define el mapeo 

               

                CreateMap<Producto, ProductoDto>();

                CreateMap<CreacionProductoDto, Producto>()
                 .ForMember(dest => dest.Id, opt => opt.Ignore())
                 .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                 .ForMember(dest => dest.ImagenUrl, opt => opt.Ignore() );


                CreateMap<Producto, CreacionProductoDto>();

                CreateMap<Categoria, CategoriaDto>();
                
                CreateMap<CreacionCategoriaDto, Categoria>();

                CreateMap<Role, RoleDto>();

                CreateMap<CreacionRoleDto, Role>();

                CreateMap<Usuario, UsuarioDto>();

                CreateMap<CreacionUsuarioDto, Usuario>();

                CreateMap<Cart, CartDto>()
                    .ForMember(x => x.CartItems, opt => opt.MapFrom(src => src.CartItems));

                CreateMap<CartItem, CartItemDto>()
                    .ForMember(x => x.ProductoNombre, opt => opt.MapFrom(src => src.Producto.Nombre))
                    .ForMember(x => x.ProductoId, opt => opt.MapFrom(src => src.Producto.Id)); 

                CreateMap<CreacionCartItemDto, CartItem>();

                CreateMap<Pedido, PedidoDto>()
                    .ForMember(x => x.PedidoItems, opt => opt.MapFrom(src => src.PedidoItems))
                    .ForMember(x => x.Pagos, opt => opt.MapFrom(src => src.Pagos));

                CreateMap<CreacionPedidoDto, Pedido>();

                CreateMap<PedidoItem, PedidoItemDto>()
                    .ForMember(x => x.NombreProducto, opt => opt.MapFrom(src => src.Producto.Nombre))
                    .ForMember(x => x.Precio, opt => opt.MapFrom(src => src.Precio));

                CreateMap<CreacionPedidoItemDto, PedidoItem>();

                CreateMap<Pago, PagoDto>();
                CreateMap<CreacionPagoDto, Pago>();

            }
        }

        
    }
}
