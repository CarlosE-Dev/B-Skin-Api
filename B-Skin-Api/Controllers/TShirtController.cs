﻿using B_Skin_Api.Domain.Interfaces;
using B_Skin_Api.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace B_Skin_Api.Web.Controllers
{
    [Route("t-shirts")]
    [ApiController]
    public class TShirtsController : ControllerBase
    {
        private readonly ITShirtRepository _repo;
        public TShirtsController(ITShirtRepository repo)
        {
            _repo = repo;
        }

        [HttpPost("list")]
        public async Task<IActionResult> GetAll(bool onlyActives, TShirtFilterModel filter)
        {
            return Ok(
            await _repo.GetAll(
                onlyActives,
                filter
            ));
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

        [HttpPost("create")]
        public async Task<IActionResult> Create(TShirtModel entity)
        {
            return Created("", await _repo.Create(entity));
        }

        [HttpPut("update/{id:long}")]
        public async Task<IActionResult> Update(long id, TShirtModel entity)
        {
            await _repo.Update(id, entity);
            return NoContent();
        }

        [HttpPost("search/{query}")]
        public async Task<IActionResult> SearchByKeyWord([FromRoute] string query, int resultLimit)
        {
            return Ok(
            await _repo.SearchTShirtsByKeyWords(
                query,
                resultLimit
            ));
        }

        [HttpPut("image/update/{id:long}")]
        public async Task<IActionResult> UpdateImage(long id, string imgUrl)
        {
            await _repo.UpdateImage(id, imgUrl);
            return NoContent();
        }

        [HttpDelete("exclude/{id:long}")]
        public async Task<IActionResult> ExcludePermanently(long id)
        {
            await _repo.ExcludePermanently(id);
            return NoContent();
        }
    }
}
