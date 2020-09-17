using System.Linq;
using AutoMapper;
using CP.API.Dto;
using CP.API.Model;

namespace SAMMAPP.API.Helpers
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<CategoryRegisterDTO,Category>();
            CreateMap<Category,CategoryReturnDTO>()
            .ForMember(d => d.Sections, m => m.MapFrom(s => s.Sections));
            CreateMap<ProductForUpdateDTO,Product>();
            CreateMap<Product,ProductForUpdateDTO>();
            CreateMap<ProductRegisterDTO ,Product>();
            CreateMap< Product ,ProductReturnDTO>()
            .ForMember(dest=>dest.PhotoURL,opt=>{opt.MapFrom(src=>src.PhotoForProducts.FirstOrDefault(p=>p.IsMain).Url);})
            .ForMember(d => d.Photos, m => m.MapFrom(s => s.PhotoForProducts))
            .ForMember(d => d.SupplierName, m => m.MapFrom(s => s.Supplier))
            .ForMember(d => d.SectionName, m => m.MapFrom(s => s.Section));
            
            CreateMap<PaymentRegisterDTO,Payment>();
            CreateMap<Payment,PaymentReturnDTO>();
            CreateMap<ShipperRegisterDTO,Shipper>();
            CreateMap<Shipper,ShipperReturnDTO>();


            
            CreateMap<SupplierRegisterDTO,Supplier>();
            CreateMap<SupplierForUpdateDTO,Supplier>();
            CreateMap<Supplier,SupplierReturnDTO>()
            .ForMember(dest=>dest.PhotoURL,opt=>{opt.MapFrom(src=>src.PhotoForSuppliers.FirstOrDefault(p=>p.IsMain).Url);})
            .ForMember(d => d.Products, m => m.MapFrom(s => s.Products))
            .ForMember(d => d.Photos, m => m.MapFrom(s => s.PhotoForSuppliers));

            CreateMap<OrderDetailRegisterDTO , OrderDetail>();
            CreateMap<OrderDetail,OrderDetailReturnDTO>();
            
            // .ForMember(d => d.ProductName, m => m.MapFrom(s => s.Product.));

            CreateMap<OrderRegisterDTO,Order>();
            CreateMap<Order,OrderDetailReturnDTO>();

            CreateMap<PhotoForSupplier,PhotoForDetailsDto>();
            CreateMap<PhotoForSupplier,PhotoForReturnDto>();
            CreateMap<PhotoForCreateDto,PhotoForSupplier>();

            CreateMap<PhotoForProduct,PhotoForDetailsDto>();
            CreateMap<PhotoForProduct,PhotoForReturnDto>();
            CreateMap<PhotoForCreateDto,PhotoForProduct>();
            // .ForMember(x =>x.Products, opt => opt.Ignore());
            // .ForMember(dest => dest.PhotoUrl, opt => { opt.MapFrom(src => src.Photos.FirstOrDefault(p => p.IsMain).Url); }).ForMember(dest => dest.Age, opt => { opt.ResolveUsing(src => src.DateOfBirth.CalculateAge()); });
            //  CreateMap<User, UserForDetailsDTO>()
            // .ForMember(dest => dest.PhotoUrl, opt => { opt.MapFrom(src => src.Photos.FirstOrDefault(p => p.IsMain).Url); })
            // .ForMember(dest => dest.Age, opt => { opt.ResolveUsing(src => src.DateOfBirth.CalculateAge()); });
            //  CreateMap<Photo, PhotoForDetailsDTO>();

             CreateMap<SectionRegisterDTO,Section>();
            CreateMap<Section,SectionReturnDTO>();

            CreateMap<DiscountRegisterDTO,Discount>();
            CreateMap<Discount,DiscountReturnDTO>();


            CreateMap<SocialCommunicationRegisterDTO , SocialCommunication>();
            CreateMap<SocialCommunication,SocialCommunicationReturnDTO>();
        }
    }
}