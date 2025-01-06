using System;
using Microsoft.AspNetCore.Mvc;
using Service.Helpers.DTOs.Logo;
using Service.Services.Interfaces;

namespace SteakInProject.Controllers
{
	public class AwardLogoController : BaseController
	{
        private readonly IAwardLogoService _logoService;

        public AwardLogoController(IAwardLogoService logoService)
        {
            _logoService = logoService;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromForm] LogoCreateDto request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var response = await _logoService.CreateAsync(request);

            if (response != "Success") return BadRequest(response);

            return Ok(response);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> Edit([FromRoute] int id, [FromForm] LogoEditDto request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var response = await _logoService.EditAsync(id, request);

            if (response != "Success")
            {
                return BadRequest(response);
            }

            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var datas = await _logoService.GetAllAsync();
            return Ok(datas);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            return Ok(await _logoService.GetById(id));
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var response = await _logoService.DeleteAsync(id);

            if (response == "Data not found")
            {

                return NotFound(response);
            }
            return Ok(response);
        }
    }
}

