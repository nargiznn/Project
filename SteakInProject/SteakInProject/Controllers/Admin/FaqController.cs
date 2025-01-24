using System;
using Microsoft.AspNetCore.Mvc;
using Repository.Exceptions;
using Service.Helpers.DTOs.Faq;
using Service.Helpers.Faqs;
using Service.Services.Interfaces;

namespace SteakInProject.Controllers.Admin
{
	public class FaqController:MainController
	{
        private readonly IFaqService _faqService;
        public FaqController(IFaqService faqService)
        {
            _faqService = faqService;
        }
        [ProducesResponseType(typeof(FaqDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(FaqDto), StatusCodes.Status404NotFound)]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            try
            {
                return Ok(await _faqService.GetByIdAsync(id));

            }
            catch (NotFoundException)
            {
                return NotFound();
            }
        }
        [ProducesResponseType(typeof(FaqDto), StatusCodes.Status201Created)]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] FaqCreateDto request)
        {
            await _faqService.CreateAsync(request);
            return CreatedAtAction(nameof(Create), "Succesfully");
        }
        [ProducesResponseType(typeof(FaqDto), StatusCodes.Status200OK)]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {

            return Ok(await _faqService.GetAllAsync());
        }
        [ProducesResponseType(typeof(FaqDto), StatusCodes.Status200OK)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            try
            {

                await _faqService.DeleteAsync(id);
                return Ok();
            }
            catch (NotFoundException)
            {
                return NotFound();
            }

        }
        [HttpGet("search")]
        public async Task<IActionResult> Search([FromQuery] string searchString)
        {
            var categories = await _faqService.SearchAsync(searchString);
            return Ok(categories);
        }
        [ProducesResponseType(typeof(FaqDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(FaqDto), StatusCodes.Status404NotFound)]
        [HttpPut("{id}")]
        public async Task<IActionResult> Edit([FromRoute] int id, [FromBody] FaqEditDto request)
        {
            try
            {
                await _faqService.EditAsync(id, request);
                return Ok("Faq updated successfully");
            }
            catch (NotFoundException)
            {
                return NotFound();
            }
        }
    }
}

