using AutoMapper;
using B_Skin_Api.Domain.Models;
using B_Skin_Api.Domain.Models.Commands.ProviderCommands;

namespace B_Skin_Api.Domain.AutoMapper
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            #region Providers

            CreateMap<Provider, CreateProviderCommand>().ReverseMap();
            CreateMap<Provider, UpdateProviderCommand>().ReverseMap();

            #endregion

            #region TShirts


            #endregion
        }
    }
}
