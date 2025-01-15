using System;
using Microsoft.AspNetCore.Mvc;
using Repository.Data;
using Repository.Exceptions;
using Service.Services;
using Service.Services.Interfaces;

namespace SteakInProject.Controllers
{
	public class FaqController: BaseController
    {
        private readonly IFaqService _faqService;
        private readonly AppDbContext _context;
        public FaqController(IFaqService faqService, AppDbContext context)
        {
            _faqService = faqService;
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _faqService.GetAllAsync());
        }
        [HttpGet("search")]
        public async Task<IActionResult> Search([FromQuery] string searchString)
        {
            if (searchString is null)
            {
                return Ok(await _faqService.GetAllAsync());
            }
            var faqs = await _faqService.SearchAsync(searchString);
            return Ok(faqs);
        }
    }
}

