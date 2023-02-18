using B_Skin_Api.Domain.Enums;
using B_Skin_Api.Domain.Interfaces;
using B_Skin_Api.Domain.Models;
using B_Skin_Api.Domain.Models.Commands;
using B_Skin_Api.Domain.Models.Commands.ProviderCommands;
using B_Skin_Api.Domain.Models.Queries.ProviderQueries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace B_Skin_Api.Web.Controllers
{
    [Route("providers")]
    [ApiController]
    public class ProvidersController : ControllerBase
    {
        private readonly IProviderRepository _repo;
        private readonly IMediator _mediator;
        public ProvidersController(IProviderRepository repo, IMediator mediator)
        {
            _repo = repo;
            _mediator = mediator;
        }

        [HttpGet("list")]
        public async Task<IActionResult> GetAll(bool onlyActives)
        {
            return Ok(await _mediator.Send(new GetAllProvidersQuery { IncludeInactives = onlyActives}));
        }

        [HttpGet("details/{id:long}")]
        public async Task<IActionResult> GetById(long id, bool onlyActives)
        {
            return Ok(await _mediator.Send(new GetProviderByIdQuery { Id = id, IncludeInactives = onlyActives}));
        }

        [HttpPut("update/{id:long}")]
        public async Task<IActionResult> Update([FromRoute] long id, [FromBody] UpdateProviderCommand command)
        {
            command.Id = id;
            await _mediator.Send(command);
            return NoContent();
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create(CreateProviderCommand command)
        {
            return Created("", await _mediator.Send(command));
        }

        [HttpPut("image/update/{id:long}")]
        public async Task<IActionResult> UpdateImage([FromRoute] long id, [FromBody] UpdateProviderImageUrlCommand command)
        {
            command.Id = id;
            await _mediator.Send(command);
            return NoContent();
        }

        [HttpPut("remove/{id:long}")]
        public async Task<IActionResult> InactivateById(long id)
        {
            await _mediator.Send(new UpdateProviderStatusCommand { Id = id, OperationType = EProviderStatusOperationType.Inactivate });
            return NoContent();
        }

        [HttpPut("reactivate/{id:long}")]
        public async Task<IActionResult> ReactivateById(long id)
        {
            await _mediator.Send(new UpdateProviderStatusCommand { Id = id, OperationType = EProviderStatusOperationType.Reactivate });
            return NoContent();
        }

        [HttpDelete("exclude/{id:long}")]
        public async Task<IActionResult> ExcludePermanently(long id)
        {
            await _mediator.Send(new DeleteProviderCommand { Id = id });
            return NoContent();
        }
    }
}
