using System;
using Microsoft.AspNetCore.Mvc;
using Service.Helpers.DTOs.Product;
using Service.Services.Interfaces;

namespace SteakInProject.Controllers
{
	public class ProductController: BaseController
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromForm] ProductCreateDto request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var response = await _productService.CreateAsync(request);

            if (response != "Success") return BadRequest(response);

            return Ok(response);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> Edit([FromRoute] int id, [FromForm] ProductEditDto request)
        {
            var result = await _productService.EditAsync(id, request);

            if (result == "Data not found")
            {
                return NotFound(result);
            }

            return Ok(result);
        }


        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var datas = await _productService.GetAllAsync();
            return Ok(datas);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            return Ok(await _productService.GetById(id));
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var response = await _productService.DeleteAsync(id);

            if (response == "Data not found")
            {

                return NotFound(response);
            }
            return Ok(response);
        }

    }
}

