using B_Skin_Api.Domain.Interfaces;
using B_Skin_Api.Domain.Models.Dtos;
using MediatR;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Threading;

namespace B_Skin_Api.Domain.Models.Queries.TShirtQueries
{
    public class GetTShirtByIdQuery : IRequest<TShirtDTO>
    {
        [JsonIgnore]
        public bool OnlyActives { get; set; }

        [JsonIgnore]
        public long Id { get; set; }
    }

    public class GetTShirtByIdQueryHandler : IRequestHandler<GetTShirtByIdQuery, TShirtDTO>
    {
        private readonly ITShirtRepository _tshirtRepository;
        public GetTShirtByIdQueryHandler(ITShirtRepository tshirtRepository)
        {
            _tshirtRepository = tshirtRepository;
        }

        public async Task<TShirtDTO> Handle(GetTShirtByIdQuery query, CancellationToken cancellationToken)
        {
            return await _tshirtRepository.GetById(query.Id, query.OnlyActives);
        }
    }
}
