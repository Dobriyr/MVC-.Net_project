using AutoMapper;
using Business.Models;
using Data.Entities;
using System.Linq;

namespace Business
{
    public class AutomapperProfile : Profile 
    {
        public AutomapperProfile()
        {
            CreateMap<Receipt, ReceiptModel>()
                .ForMember(rm => rm.ReceiptDetailsIds, r => r.MapFrom(x => x.ReceiptDetails.Select(rd => rd.Id)))
                .ReverseMap();

            CreateMap<Product, ProductModel>()
                .ForMember(pm => pm.Id, p => p.MapFrom(p => p.Id))
                .ForMember(pm => pm.ProductCategoryId, p => p.MapFrom(p => p.ProductCategoryId))
                .ForMember(pm => pm.CategoryName, p => p.MapFrom(p => p.Category.CategoryName))
                .ForMember(pm => pm.ProductName, p => p.MapFrom(p => p.ProductName))
                .ForMember(pm => pm.Price, p => p.MapFrom(p => p.Price))
                .ForMember(pm => pm.ReceiptDetailIds, r => r.MapFrom(x => x.ReceiptDetails.Select(rd => rd.Id)))
                .ReverseMap();
            CreateMap<ProductModel, Product>();

            CreateMap<ReceiptDetail, ReceiptDetailModel>().ReverseMap();

            CreateMap<Customer, CustomerModel>()
                 .ForMember(cm => cm.Id, c => c.MapFrom(x => x.Person.Id))
                 .ForMember(cm => cm.Name, c => c.MapFrom(x => x.Person.Name))
                 .ForMember(cm => cm.Surname, c => c.MapFrom(x => x.Person.Surname))
                 .ForMember(cm => cm.BirthDate, c => c.MapFrom(x => x.Person.BirthDate))
                 .ForMember(cm => cm.DiscountValue, c => c.MapFrom(x => x.DiscountValue))
                 .ForMember(cm => cm.ReceiptsIds, c => c.MapFrom(x => x.Receipts.Select(r => r.Id)))
                 .ReverseMap();

            CreateMap<ProductCategory, ProductCategoryModel>()
                .ForMember(pcm=>pcm.CategoryName, pc=>pc.MapFrom(pc=>pc.CategoryName))
                .ForMember(pcm=>pcm.Id, pc=>pc.MapFrom(pc=>pc.Id))
                .ReverseMap();
        }
    }
}