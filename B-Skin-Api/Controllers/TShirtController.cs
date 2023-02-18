using B_Skin_Api.Domain.Enums;
using B_Skin_Api.Domain.Interfaces;
using B_Skin_Api.Domain.Models.Commands.TShirtCommands;
using B_Skin_Api.Domain.Models.Queries.TShirtQueries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace B_Skin_Api.Web.Controllers
{
    [Route("t-shirts")]
    [ApiController]
    public class TShirtsController : ControllerBase
    {
        private readonly ITShirtRepository _repo;
        private readonly IMediator _mediator;
        public TShirtsController(ITShirtRepository repo, IMediator mediator)
        {
            _repo = repo;
            _mediator= mediator;
        }

        [HttpPost("list")]
        public async Task<IActionResult> GetAll(bool onlyActives, GetAllTShirtsQuery query)
        {
            query.OnlyActives = onlyActives;
            return Ok(await _mediator.Send(query));
        }

        [HttpGet("details/{id:long}")]
        public async Task<IActionResult> GetById(long id, bool onlyActives)
        {
            return Ok(await _mediator.Send(new GetTShirtByIdQuery { Id = id, OnlyActives = onlyActives}));
        }

        [HttpPut("remove/{id:long}")]
        public async Task<IActionResult> InactivateById(long id)
        {
            await _mediator.Send(new UpdateTShirtStatusCommand { Id = id, OperationType = EStatusOperationType.Inactivate });
            return NoContent();
        }

        [HttpPut("reactivate/{id:long}")]
        public async Task<IActionResult> ReactivateById(long id)
        {
            await _mediator.Send(new UpdateTShirtStatusCommand { Id = id, OperationType = EStatusOperationType.Reactivate });
            return NoContent();
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create(CreateTShirtCommand command)
        {
            return Created("", await _mediator.Send(command));
        }

        [HttpPut("update/{id:long}")]
        public async Task<IActionResult> Update(long id, UpdateTShirtCommand command)
        {
            command.Id = id;
            await _mediator.Send(command);
            return NoContent();
        }

        [HttpPost("search/{query}")]
        public async Task<IActionResult> SearchByKeyWord([FromRoute] string query, int resultsLimit)
        {
            return Ok(await _mediator.Send(new GetTShirtsByKeyWordsQuery { Query = query, ResultsLimit = resultsLimit }));
        }

        [HttpPut("image/update/{id:long}")]
        public async Task<IActionResult> UpdateImage([FromRoute] long id, [FromBody] UpdateTShirtImageUrlCommand command)
        {
            command.Id = id;
            await _mediator.Send(command);
            return NoContent();
        }

        [HttpDelete("exclude/{id:long}")]
        public async Task<IActionResult> ExcludePermanently(long id)
        {
            await _mediator.Send(new DeleteTShirtCommand { Id = id });
            return NoContent();
        }

        [HttpGet("sizes/{shirtId:long}")]
        public async Task<IActionResult> GetAvaiableSizes(long shirtId)
        {
            return Ok(await _mediator.Send(new GetTShirtAvaiableSizesQuery { TShirtId = shirtId }));
        }
    }
}
