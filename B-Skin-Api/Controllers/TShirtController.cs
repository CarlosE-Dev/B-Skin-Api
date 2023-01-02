using B_Skin_Api.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace B_Skin_Api.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TShirtController : Controller
    {
        private readonly IUnitOfWork _uow;
        private readonly ITShirtRepository _repo;
        public TShirtController(IUnitOfWork uow, ITShirtRepository repo)
        {
            _uow = uow;
            _repo = repo;
        }
        [Route("list")]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _repo.GetAll());
        }
    }
}
