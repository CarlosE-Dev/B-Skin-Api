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
    public class GetProviderByIdQuery : IRequest<ProviderDTO>
    {
        [JsonIgnore]
        public long Id { get; set; }

        [JsonIgnore]
        public bool IncludeInactives { get; set; }
    }

    public class GetProviderByIdQueryHandler : IRequestHandler<GetProviderByIdQuery, ProviderDTO>
    {
        private readonly IProviderRepository _providerRepository;
        public GetProviderByIdQueryHandler(IProviderRepository providerRepository)
        {
            _providerRepository = providerRepository;
        }

        public async Task<ProviderDTO> Handle(GetProviderByIdQuery request, CancellationToken cancellationToken)
        {
            return await _providerRepository.GetById(request.Id, request.IncludeInactives);
        }
    }
}
