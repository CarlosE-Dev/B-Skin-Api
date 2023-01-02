using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace B_Skin_Api.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : Controller
    {
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }
    }
}
