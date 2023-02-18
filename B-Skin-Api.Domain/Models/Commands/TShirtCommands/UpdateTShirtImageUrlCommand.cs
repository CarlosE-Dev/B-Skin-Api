using B_Skin_Api.Domain.Interfaces;
using MediatR;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;

namespace B_Skin_Api.Domain.Models.Commands.TShirtCommands
{
    public class UpdateTShirtImageUrlCommand : IRequest<Unit>
    {
        [JsonIgnore]
        public long Id { get; set; }

        [Required(ErrorMessage = "The field {0} cannot be empty")]
        [StringLength(500, ErrorMessage = "The length of the field {0} must be {2} to {1} characters", MinimumLength = 5)]
        public string ImageUrl { get; set; }
    }
    public class UpdateTShirtImageUrlCommandHandler : IRequestHandler<UpdateTShirtImageUrlCommand, Unit>
    {
        private readonly ITShirtRepository _tshirtRepository;
        public UpdateTShirtImageUrlCommandHandler(ITShirtRepository tshirtRepository)
        {
            _tshirtRepository = tshirtRepository;
        }

        public async Task<Unit> Handle(UpdateTShirtImageUrlCommand request, CancellationToken cancellationToken)
        {
            await _tshirtRepository.UpdateImage(request.Id, request.ImageUrl);

            return await Unit.Task;
        }
    }
}
