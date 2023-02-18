using AutoMapper;
using B_Skin_Api.Domain.Interfaces;
using MediatR;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;

namespace B_Skin_Api.Domain.Models.Commands.ProviderCommands
{
    public class UpdateProviderCommand : IRequest<Unit>
    {
        [JsonIgnore]
        public long? Id { get; set; }

        [Required(ErrorMessage = "The field {0} cannot be empty")]
        [StringLength(100, ErrorMessage = "The length of the field {0} must be {2} to {1} characters", MinimumLength = 3)]
        public string Name { get; set; }

        [Required(ErrorMessage = "The field {0} cannot be empty")]
        [StringLength(500, ErrorMessage = "The length of the field {0} must be {2} to {1} characters", MinimumLength = 10)]
        public string Description { get; set; }

        [Required(ErrorMessage = "The field {0} cannot be empty")]
        [StringLength(2, ErrorMessage = "The length of the field {0} must be {1} characters", MinimumLength = 2)]
        public string Country { get; set; }

        [Required(ErrorMessage = "The field {0} cannot be empty")]
        [StringLength(100, ErrorMessage = "The length of the field {0} must be {2} to {1} characters", MinimumLength = 5)]
        public string Email { get; set; }

        [Required(ErrorMessage = "The field {0} cannot be empty")]
        [StringLength(20, ErrorMessage = "The length of the field {0} must be {2} to {1} characters", MinimumLength = 5)]
        public string Phone { get; set; }

        [Required(ErrorMessage = "The field {0} cannot be empty")]
        [StringLength(14, ErrorMessage = "The length of the field {0} must be {1} characters", MinimumLength = 14)]
        public string Document { get; set; }

        [Required(ErrorMessage = "The field {0} cannot be empty")]
        public bool IsActive { get; set; }

        [Required(ErrorMessage = "The field {0} cannot be empty")]
        public long ProviderTypeId { get; set; }

        [StringLength(500, ErrorMessage = "The length of the field {0} must be {2} to {1} characters", MinimumLength = 5)]
        public string ImageUrl { get; set; }
    }

    public class UpdateProviderCommandHandler : IRequestHandler<UpdateProviderCommand, Unit>
    {
        private readonly IMapper _mapper;
        private readonly IProviderRepository _providerRepository;
        public UpdateProviderCommandHandler(IMapper mapper, IProviderRepository providerRepository)
        {
            _mapper = mapper;
            _providerRepository = providerRepository;
        }
        public async Task<Unit> Handle(UpdateProviderCommand request, CancellationToken cancellationToken)
        {
            await _providerRepository.Update(_mapper.Map<Provider>(request));

            return Unit.Task.Result;
        }
    }
}
