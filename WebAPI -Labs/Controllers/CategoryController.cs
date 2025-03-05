using Context;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Repository;
using WebAPI_Labs.Model;

namespace WebAPI_Labs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoryController(ICategoryRepository _categoryRepository)
        {
            this._categoryRepository = _categoryRepository;
        }

        [HttpGet]
        public ActionResult<GeneralResponse> GetAll()
        {
            var allCategories = _categoryRepository.GetAll();

            if (allCategories == null || !allCategories.Any())
            {
                return NotFound(new GeneralResponse { Success = false, Data = "No categories found" });
            }

            return Ok(new GeneralResponse { Success = true, Data = allCategories });
        }

        [HttpGet("{id:int}")]
        public ActionResult<GeneralResponse> GetByID(int id)
        {
            var category = _categoryRepository.GetById(id);

            if (category == null)
            {
                return NotFound(new GeneralResponse { Success = false, Data = "No category found" });
            }

            return Ok(new GeneralResponse { Success = true, Data = category });
        }

        [HttpPost]
        public IActionResult Add(Category cat)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _categoryRepository.Insert(cat);
            _categoryRepository.Save();

            return CreatedAtAction(nameof(GetByID), new { id = cat.ID }, new GeneralResponse { Success = true, Data = cat });
        }

        [HttpDelete("{id:int}")]
        public ActionResult<GeneralResponse> Delete(int id)
        {
            var category = _categoryRepository.GetById(id);
            if (category == null)
            {
                return NotFound(new GeneralResponse { Success = false, Data = "No category found" });
            }
            _categoryRepository.Delete(id);
            _categoryRepository.Save();

            return Ok(new GeneralResponse { Success = true, Data = "Category deleted successfully" });
        }

        [HttpPut("{id:int}")]
        public ActionResult<GeneralResponse> Update([FromBody]Category cat,int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var existingCategory = _categoryRepository.GetById(id);
            if (existingCategory == null)
            {
                return NotFound(new GeneralResponse { Success = false, Data = "No category found" });
            }

            _categoryRepository.Update(cat);
            _categoryRepository.Save();

            return Ok(new GeneralResponse { Success = true, Data = "Category updated successfully" });
        }
    }
}
