using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProductsSystem.Services.Services.Interfaces;
using ProductsSystem.ViewModels.ViewModels;
using System.Threading.Tasks;

namespace ProductsSystem.WebApi.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    public class CategoryController : Controller
    {
        private ICategoryService _service;

        public CategoryController(ICategoryService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult> Get()
        {
            var entities = await _service.GetAll();
            return Ok(entities);
        }

        [HttpGet("getByName")]
        public async Task<ActionResult> GetByName(string searchName)
        {
            var result = await _service.GetByName(searchName);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> Get(int id)
        {
            if (id == 0)
            {
                return BadRequest();
            }

            var result = await _service.GetbyId(id);
            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult> Post(CategoryViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var result = await _service.Create(viewModel);

            if (!result)
            {
                return BadRequest();
            }

            return Ok();
        }

        // PUT api/values/5
        [HttpPut]
        public async Task<ActionResult> Edit(CategoryViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var result = await _service.Edit(viewModel);

            if (!result)
            {
                return BadRequest();
            }

            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            if (id == 0)
            {
                return BadRequest();
            }

            var result = await _service.Delete(id);

            if (!result)
            {
                return BadRequest();
            }
            return Ok(new { Success = result });
        }
    }
}