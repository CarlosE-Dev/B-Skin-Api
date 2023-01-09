using B_Skin_Api.Domain.Interfaces;
using B_Skin_Api.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace B_Skin_Api.Web.Controllers
{
    [Route("providers")]
    [ApiController]
    public class ProvidersController : ControllerBase
    {
        private readonly IProviderRepository _repo;
        public ProvidersController(IProviderRepository repo)
        {
            _repo = repo;
        }

        [HttpGet("list")]
        public async Task<IActionResult> GetAll(bool onlyActives)
        {
            return Ok(await _repo.GetAll(onlyActives));
        }

        [HttpGet("details/{id:long}")]
        public async Task<IActionResult> GetById(long id, bool onlyActives)
        {
            return Ok(await _repo.GetById(id, onlyActives));
        }

        [HttpPut("remove/{id:long}")]
        public async Task<IActionResult> InactivateById(long id)
        {
            await _repo.InactivateById(id);
            return NoContent();
        }

        [HttpPut("reactivate/{id:long}")]
        public async Task<IActionResult> ReactivateById(long id)
        {
            await _repo.ReactivateById(id);
            return NoContent();
        }

        [HttpPut("update/{id:long}")]
        public async Task<IActionResult> Update(long id, Provider entity)
        {
            await _repo.Update(id, entity);
            return NoContent();
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create(Provider entity)
        {
            return Created("", await _repo.Create(entity));
        }
    }
}
