using B_Skin_Api.Domain.Interfaces;
using MediatR;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;

namespace B_Skin_Api.Domain.Models.Commands.ProviderCommands
{
    public class UpdateProviderImageUrlCommand : IRequest<Unit>
    {
        [JsonIgnore]
        public long Id { get; set; }

        [Required(ErrorMessage = "The field {0} cannot be empty")]
        [StringLength(500, ErrorMessage = "The length of the field {0} must be {2} to {1} characters", MinimumLength = 5)]
        public string ImageUrl { get; set; }
    }
    public class UpdateProviderImageUrlCommandHandler : IRequestHandler<UpdateProviderImageUrlCommand, Unit>
    {
        private readonly IProviderRepository _providerRepository;
        public UpdateProviderImageUrlCommandHandler(IProviderRepository providerRepository)
        {
            _providerRepository = providerRepository;
        }

        public async Task<Unit> Handle(UpdateProviderImageUrlCommand request, CancellationToken cancellationToken)
        {
            await _providerRepository.UpdateImage(request.Id, request.ImageUrl);

            return await Unit.Task;
        }
    }
}
