using B_Skin_Api.Domain.Interfaces;
using B_Skin_Api.Domain.Models.Dtos;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace B_Skin_Api.Domain.Models.Queries.TShirtQueries
{
    public class GetTShirtsByKeyWordsQuery : IRequest<IEnumerable<TShirtDTO>>
    {
        public string Query { get; set; }
        public int ResultsLimit { get; set; }
    }
    public class GetTShirtsByKeyWordsQueryHandler : IRequestHandler<GetTShirtsByKeyWordsQuery, IEnumerable<TShirtDTO>>
    {
        private readonly ITShirtRepository _tshirtRepository;
        public GetTShirtsByKeyWordsQueryHandler(ITShirtRepository tshirtRepository)
        {
            _tshirtRepository = tshirtRepository;
        }
        public async Task<IEnumerable<TShirtDTO>> Handle(GetTShirtsByKeyWordsQuery request, CancellationToken cancellationToken)
        {
            return await _tshirtRepository.SearchTShirtsByKeyWords(request.Query, request.ResultsLimit);
        }
    }
}
