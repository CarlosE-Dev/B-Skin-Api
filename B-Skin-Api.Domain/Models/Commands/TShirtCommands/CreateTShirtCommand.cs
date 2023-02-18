using AutoMapper;
using B_Skin_Api.Domain.Interfaces;
using B_Skin_Api.Domain.Models.Dtos;
using MediatR;
using System.Threading.Tasks;
using System.Threading;
using System.ComponentModel.DataAnnotations;

namespace B_Skin_Api.Domain.Models.Commands.TShirtCommands
{
    public class CreateTShirtCommand : IRequest<TShirtDTO>
    {
        [Required(ErrorMessage = "The field {0} cannot be empty")]
        [StringLength(100, ErrorMessage = "The length of the field {0} must be {2} to {1} characters", MinimumLength = 3)]
        public string ModelName { get; set; }

        [Required(ErrorMessage = "The field {0} cannot be empty")]
        [StringLength(500, ErrorMessage = "The length of the field {0} must be {2} to {1} characters", MinimumLength = 10)]
        public string ModelDescription { get; set; }

        [Required(ErrorMessage = "The field {0} cannot be empty")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "The field {0} cannot be empty")]
        public int QuantityInStock { get; set; }

        [Required(ErrorMessage = "The field {0} cannot be empty")]
        public bool? IsActive { get; set; }

        [Required(ErrorMessage = "The field {0} cannot be empty")]
        [StringLength(1, ErrorMessage = "The length of the field {0} must be {2} character", MinimumLength = 1)]
        public string Gender { get; set; }

        [Required(ErrorMessage = "The field {0} cannot be empty")]
        [StringLength(50, ErrorMessage = "The length of the field {0} must be {2} to {1} characters", MinimumLength = 2)]
        public string Color { get; set; }

        [Required(ErrorMessage = "The field {0} cannot be empty")]
        public long ProviderId { get; set; }

        [Required(ErrorMessage = "The field {0} cannot be empty")]
        [StringLength(500, ErrorMessage = "The length of the field {0} must be {2} to {1} characters", MinimumLength = 5)]
        public string ImageUrl { get; set; }

        /// <summary>
        /// fields must be separated by a comma
        /// example: "1,3,6"
        /// </summary>
        [Required(ErrorMessage = "The field {0} cannot be empty")]
        [StringLength(20, ErrorMessage = "The length of the field {0} must be {2} to {1} characters", MinimumLength = 1)]
        public string SizeIds { get; set; }
    }
    public class CreateTShirtCommandHandler : IRequestHandler<CreateTShirtCommand, TShirtDTO>
    {
        private readonly IMapper _mapper;
        private readonly ITShirtRepository _tshirtRepository;
        public CreateTShirtCommandHandler(IMapper mapper, ITShirtRepository tshirtRepository)
        {
            _mapper = mapper;
            _tshirtRepository = tshirtRepository;
        }
        public async Task<TShirtDTO> Handle(CreateTShirtCommand request, CancellationToken cancellationToken)
        {
            return await _tshirtRepository.Create(_mapper.Map<TShirtModel>(request), request.SizeIds);
        }
    }
}
