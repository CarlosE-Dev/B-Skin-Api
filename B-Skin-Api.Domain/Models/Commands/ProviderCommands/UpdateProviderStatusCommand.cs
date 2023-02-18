using B_Skin_Api.Domain.Enums;
using B_Skin_Api.Domain.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;

namespace B_Skin_Api.Domain.Models.Commands.ProviderCommands
{
    public class UpdateProviderStatusCommand : IRequest<Unit>
    {
        [JsonIgnore]
        public long Id { get; set; }

        [StringLength(500, ErrorMessage = "The length of the field {0} must be {2} to {1} characters", MinimumLength = 5)]
        public EStatusOperationType OperationType { get; set; }
    }

    public class UpdateProviderStatusCommandHandler : IRequestHandler<UpdateProviderStatusCommand, Unit>
    {
        private readonly IProviderRepository _providerRepository;
        public UpdateProviderStatusCommandHandler(IProviderRepository providerRepository)
        {
            _providerRepository = providerRepository;
        }

        public async Task<Unit> Handle(UpdateProviderStatusCommand request, CancellationToken cancellationToken)
        {
            if (request.OperationType == EStatusOperationType.Inactivate)
                await _providerRepository.InactivateById(request.Id);

            if(request.OperationType == EStatusOperationType.Reactivate)
                await _providerRepository.ReactivateById(request.Id);

            return await Unit.Task;
        }
    }
}
