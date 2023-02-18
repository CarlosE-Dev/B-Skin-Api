using B_Skin_Api.Domain.Interfaces;
using MediatR;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;

namespace B_Skin_Api.Domain.Models.Queries.TShirtQueries
{
    public class GetTShirtAvaiableSizesQuery : IRequest<IEnumerable<SizeModel>>
    {
        [JsonIgnore]
        public long TShirtId { get; set; }
    }

    public class GetTShirtAvaiableSizesQueryHandler : IRequestHandler<GetTShirtAvaiableSizesQuery, IEnumerable<SizeModel>>
    {
        private readonly ITShirtRepository _tshirtRepository;
        public GetTShirtAvaiableSizesQueryHandler(ITShirtRepository tshirtRepository)
        {
            _tshirtRepository = tshirtRepository;
        }

        public async Task<IEnumerable<SizeModel>> Handle(GetTShirtAvaiableSizesQuery query, CancellationToken cancellationToken)
        {
            return await _tshirtRepository.GetAvaiableSizes(query.TShirtId);
        }
    }
}
