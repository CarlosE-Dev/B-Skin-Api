using AutoMapper;
using B_Skin_Api.Domain.Interfaces;
using B_Skin_Api.Domain.Models.Dtos;
using MediatR;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Threading;

namespace B_Skin_Api.Domain.Models.Commands.ProviderCommands
{
    public class DeleteProviderCommand : IRequest<Unit>
    {
        [JsonIgnore]
        public long Id { get; set; }
    }

    public class DeleteProviderCommandHandler : IRequestHandler<DeleteProviderCommand, Unit>
    {
        private readonly IMapper _mapper;
        private readonly IProviderRepository _providerRepository;
        public DeleteProviderCommandHandler(IMapper mapper, IProviderRepository providerRepository)
        {
            _mapper = mapper;
            _providerRepository = providerRepository;
        }
        public async Task<Unit> Handle(DeleteProviderCommand request, CancellationToken cancellationToken)
        {
            await _providerRepository.ExcludePermanently(request.Id);

            return await Unit.Task;
        }
    }
}
