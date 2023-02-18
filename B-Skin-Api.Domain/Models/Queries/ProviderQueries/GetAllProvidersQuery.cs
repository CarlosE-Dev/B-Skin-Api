using AutoMapper;
using B_Skin_Api.Domain.Interfaces;
using B_Skin_Api.Domain.Models.Commands.ProviderCommands;
using B_Skin_Api.Domain.Models.Dtos;
using MediatR;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;

namespace B_Skin_Api.Domain.Models.Queries.ProviderQueries
{
    public class GetAllProvidersQuery : IRequest<IEnumerable<ProviderDTO>>
    {
        [JsonIgnore]
        public bool IncludeInactives { get; set; }
    }

    public class GetAllProvidersQueryHandler : IRequestHandler<GetAllProvidersQuery, IEnumerable<ProviderDTO>>
    {
        private readonly IProviderRepository _providerRepository;
        public GetAllProvidersQueryHandler(IProviderRepository providerRepository)
        {
            _providerRepository = providerRepository;
        }

        public async Task<IEnumerable<ProviderDTO>> Handle(GetAllProvidersQuery request, CancellationToken cancellationToken)
        {
            return await _providerRepository.GetAll(request.IncludeInactives);
        }
    }
}
