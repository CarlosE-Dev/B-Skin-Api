using B_Skin_Api.Domain.Interfaces;
using B_Skin_Api.Domain.Models.Dtos;
using MediatR;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;

namespace B_Skin_Api.Domain.Models.Queries.TShirtQueries
{
    public class GetAllTShirtsQuery : IRequest<IEnumerable<TShirtDTO>>
    {
        public PaginationFilter Pagination { get; set; }

        public TShirtFilterModel Filters { get; set; }

        [JsonIgnore]
        public bool OnlyActives { get; set; }


    }
    public class GetAllProvidersQueryHandler : IRequestHandler<GetAllTShirtsQuery, IEnumerable<TShirtDTO>>
    {
        private readonly ITShirtRepository _tshirtRepository;
        public GetAllProvidersQueryHandler(ITShirtRepository tshirtRepository)
        {
            _tshirtRepository= tshirtRepository;
        }

        public async Task<IEnumerable<TShirtDTO>> Handle(GetAllTShirtsQuery query, CancellationToken cancellationToken)
        {
            return await _tshirtRepository.GetAll(query);
        }
    }
}
