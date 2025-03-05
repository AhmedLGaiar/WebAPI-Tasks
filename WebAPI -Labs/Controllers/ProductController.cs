using AutoMapper;
using Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Repository;
using WebAPI_Labs.DTOs;
using WebAPI_Labs.Model;

namespace WebAPI_Labs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;

        public ProductController(IProductRepository productRepository, IMapper mapper)
        {
            _productRepository = productRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public ActionResult<GeneralResponse> GetAll()
        {
            var allProd = _productRepository.GetAll();

            if (!allProd.Any())
            {
                return NotFound(new GeneralResponse { Success = false, Data = "No Products found" });
            }

            var productDtos = _mapper.Map<List<ProdWithCatDto>>(allProd);

            return Ok(new GeneralResponse { Success = true, Data = productDtos });
        }

        [HttpGet("{id:int}")]
        public ActionResult<GeneralResponse> GetById(int id)
        {
            var product = _productRepository.GetById(id);

            if (product == null)
            {
                return NotFound(new GeneralResponse { Success = false, Data = "No Product found" });
            }

            var productDto = _mapper.Map<ProdWithCatDto>(product);
            return Ok(new GeneralResponse { Success = true, Data = productDto });
        }

        [HttpPost]
        public IActionResult Add([FromBody] AddProductDto productDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new GeneralResponse { Success = false, Data = ModelState });
            }

            var product = _mapper.Map<Product>(productDto);

            _productRepository.Insert(product);
            _productRepository.Save();

            var savedProduct = _productRepository.GetById(product.ID);


            var responseDto = _mapper.Map<ProdWithCatDto>(savedProduct);

            return CreatedAtAction(nameof(GetById), new { id = product.ID },
                      new GeneralResponse { Success = true, Data = responseDto });
        }

        [HttpDelete("{id:int}")]
        public ActionResult<GeneralResponse> Delete(int id)
        {
            var product = _productRepository.GetById(id);
            if (product == null)
            {
                return NotFound(new GeneralResponse { Success = false, Data = "No Product found" });
            }

            _productRepository.Delete(id);
            _productRepository.Save();

            return Ok(new GeneralResponse { Success = true, Data = "Product deleted successfully" });
        }

        [HttpPut("{id:int}")]
        public ActionResult<GeneralResponse> Update(int id, [FromBody] AddProductDto updatedProductDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new GeneralResponse { Success = false, Data = ModelState });
            }

            var existingProduct = _productRepository.GetById(id);

            if (existingProduct == null)
            {
                return NotFound(new GeneralResponse { Success = false, Data = "No Product found" });
            }

            _mapper.Map(updatedProductDto, existingProduct);

            _productRepository.Update(existingProduct);
            _productRepository.Save();

            var updatedProductWithCategory = _productRepository.GetById(id);
            var productDto = _mapper.Map<ProdWithCatDto>(updatedProductWithCategory);

            return Ok(new GeneralResponse { Success = true, Data = productDto });
        }
    }
}
