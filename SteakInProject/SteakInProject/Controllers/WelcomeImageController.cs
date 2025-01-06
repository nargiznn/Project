using System;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Repository.Exceptions;
using Service.Helpers.DTOs.WelcomeImage;
using Service.Services.Interfaces;

namespace SteakInProject.Controllers
{
	public class WelcomeImageController:BaseController
	{
        private readonly IWelcomeImageService _welcomeImageService;

        public WelcomeImageController(IWelcomeImageService welcomeImageService)
        {
            _welcomeImageService = welcomeImageService;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromForm] WelcomeImageCreateDto request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var response = await _welcomeImageService.CreateAsync(request);

            if (response != "Success") return BadRequest(response);

            return Ok(response);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> Edit([FromRoute] int id, [FromForm] WelcomeImageEditDto request)
        {
            var result = await _welcomeImageService.EditAsync(id, request);

            if (result == "Data not found")
            {
                return NotFound(result); 
            }

            return Ok(result); 
        }


        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var datas = await _welcomeImageService.GetAllAsync();
            return Ok(datas);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            return Ok(await _welcomeImageService.GetById(id));
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var response = await _welcomeImageService.DeleteAsync(id);

            if (response == "Data not found")
            {

                return NotFound(response);
            }
            return Ok(response);
        }

    }
}

