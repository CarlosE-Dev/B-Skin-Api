using AutoMapper;
using B_Skin_Api.Domain.Models;
using B_Skin_Api.Domain.Models.Commands.ProviderCommands;
using B_Skin_Api.Domain.Models.Commands.TShirtCommands;

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

            CreateMap<TShirtModel, CreateTShirtCommand>().ReverseMap();
            CreateMap<TShirtModel, UpdateTShirtCommand>().ReverseMap();

            #endregion
        }
    }
}
