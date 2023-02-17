using AutoMapper;
using B_Skin_Api.Domain.Models;
using B_Skin_Api.Domain.Models.Commands;

namespace B_Skin_Api.Domain.AutoMapper
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Provider, CreateProviderCommand>().ReverseMap();
        }
    }
}
