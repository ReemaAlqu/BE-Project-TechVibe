using AutoMapper;
using src.DTO;
using src.Entity;
using static src.DTO.CategoryDTO;
using static src.DTO.OrderDetailDTO;
using static src.DTO.ProductDTO;
using static src.DTO.UserDTO;

namespace src.Utils
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            // Product Mappings
            CreateMap<Product, ProductReadDto>();
            CreateMap<ProductCreateDto, Product>();
            CreateMap<ProductUpdateDto, Product>()
                .ForAllMembers(opts =>
                    opts.Condition((src, dest, srcProperty) => srcProperty != null)
                );

            // User Mappings
            CreateMap<User, UserReadDto>();
            CreateMap<UserCreateDto, User>();
            CreateMap<UserUpdateDto, User>()
                .ForAllMembers(opts =>
                    opts.Condition((src, dest, srcProperty) => srcProperty != null)
                );
            // Category Mappings
            CreateMap<Category, CategoryReadDto>();
            CreateMap<CategoryCreateDto, Category>();
            CreateMap<CategoryUpdateDto, Category>()
                .ForAllMembers(opts =>
                    opts.Condition((src, dest, srcProperty) => srcProperty != null)
                );

            //Order Mappings
            CreateMap<OrderDTO.Create, Order>()
                .ForMember(dest => dest.ID, opt => opt.Ignore());

            CreateMap<OrderDTO.Update, Order>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
            CreateMap<Order, OrderDTO.Get>()
                .ForMember(dest => dest.OrderDetails, opt => opt.MapFrom(src => src.OrderDetails));

            // Map from OrderDetail to OrderDetailCreateDto
            CreateMap<OrderDetails, OrderDetailDTO.OrderDetailCreateDto>()
                .ForMember(dest => dest.ProductId, opt => opt.MapFrom(src => src.ProductId))
                .ForMember(dest => dest.Quantity, opt => opt.MapFrom(src => src.Quantity));

            // You might also need to map from OrderDetailCreateDto to OrderDetails (for Create operations)
            CreateMap<OrderDetailDTO.OrderDetailCreateDto, OrderDetails>()
                .ForMember(dest => dest.ProductId, opt => opt.MapFrom(src => src.ProductId))
                .ForMember(dest => dest.Quantity, opt => opt.MapFrom(src => src.Quantity));

            CreateMap<OrderDetails, OrderDetailReadDto>();
            CreateMap<OrderDetailCreateDto, OrderDetails>()
                .ForAllMembers(opts =>
                    opts.Condition((src, dest, srcProperty) => srcProperty != null)
                );

            CreateMap<OrderDetails, OrderDetailReadDto>();
            CreateMap<OrderDetailCreateDto, OrderDetails>()
                .ForAllMembers(opts =>
                    opts.Condition((src, dest, srcProperty) => srcProperty != null)
                );
        }
    }
}
