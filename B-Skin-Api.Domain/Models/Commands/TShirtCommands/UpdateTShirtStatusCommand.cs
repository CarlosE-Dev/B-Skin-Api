using B_Skin_Api.Domain.Enums;
using B_Skin_Api.Domain.Interfaces;
using MediatR;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;

namespace B_Skin_Api.Domain.Models.Commands.TShirtCommands
{
    public class UpdateTShirtStatusCommand : IRequest<Unit>
    {
        [JsonIgnore]
        public long Id { get; set; }

        [StringLength(500, ErrorMessage = "The length of the field {0} must be {2} to {1} characters", MinimumLength = 5)]
        public EStatusOperationType OperationType { get; set; }
    }

    public class UpdateTShirtStatusCommandHandler : IRequestHandler<UpdateTShirtStatusCommand, Unit>
    {
        private readonly ITShirtRepository _tshirtRepository;
        public UpdateTShirtStatusCommandHandler(ITShirtRepository tshirtRepository)
        {
            _tshirtRepository = tshirtRepository;
        }

        public async Task<Unit> Handle(UpdateTShirtStatusCommand request, CancellationToken cancellationToken)
        {
            if (request.OperationType == EStatusOperationType.Inactivate)
                await _tshirtRepository.InactivateById(request.Id);

            if (request.OperationType == EStatusOperationType.Reactivate)
                await _tshirtRepository.ReactivateById(request.Id);

            return await Unit.Task;
        }
    }
}
