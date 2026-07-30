using AutoMapper;
using CatalogService.Application.DTOs;
using CatalogService.Domain.Aggregates.ProductAggregate;

namespace CatalogService.Application.Common.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Đăng ký map từ lớp DTO sang lớp Domain Gốc
            CreateMap<VariantAttributeDto, VariantAttribute>();

            // (Tùy chọn) Nếu sau này bạn cần map ngược lại từ Domain sang DTO cho các API lấy dữ liệu:
            // CreateMap<VariantAttribute, VariantAttributeDto>();
        }
    }
}
