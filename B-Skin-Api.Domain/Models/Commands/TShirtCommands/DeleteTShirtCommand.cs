using AutoMapper;
using B_Skin_Api.Domain.Interfaces;
using MediatR;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;

namespace B_Skin_Api.Domain.Models.Commands.TShirtCommands
{
    public class DeleteTShirtCommand : IRequest<Unit>
    {
        [JsonIgnore]
        public long Id { get; set; }
    }

    public class DeleteTShirtCommandHandler : IRequestHandler<DeleteTShirtCommand, Unit>
    {
        private readonly IMapper _mapper;
        private readonly ITShirtRepository _tshirtRepository;
        public DeleteTShirtCommandHandler(IMapper mapper, ITShirtRepository tshirtRepository)
        {
            _mapper = mapper;
            _tshirtRepository = tshirtRepository;
        }
        public async Task<Unit> Handle(DeleteTShirtCommand request, CancellationToken cancellationToken)
        {
            await _tshirtRepository.ExcludePermanently(request.Id);

            return await Unit.Task;
        }
    }
}
