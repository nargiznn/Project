using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Repository.Data;
using Repository.Exceptions;
using Service.Helpers.DTOs.Chef;
using Service.Helpers.DTOs.Customer;
using Service.Services.Interfaces;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SteakInProject.Controllers
{
    public class ChefController : BaseController
    {
        private readonly IChefService _chefService;
        private readonly AppDbContext _context;

        public ChefController(IChefService chefService,AppDbContext context)
        {
            _chefService = chefService;
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _chefService.GetAllAsync());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var chef = await _chefService.GetByIdAsync(id);
            if (chef is null) return NotFound();
            return Ok(chef);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromForm] ChefCreateDto request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            await _chefService.CreateAsync(request);
            return CreatedAtAction(nameof(Create), "Successfully created");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            try
            {
                await _chefService.DeleteAsync(id);
                return Ok();
            }
            catch (NotFoundException)
            {
                return NotFound();

            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Edit([FromRoute] int id, [FromForm] ChefEditDto request)
        {
            try
            {
                await _chefService.EditAsync(id, request);
                return Ok();
            }
            catch (NotFoundException)
            {
                return NotFound();

            }
        }
        [HttpGet("positions")]
        public async Task<IActionResult> GetPositions()
        {
            var positions = await _context.Positions.ToListAsync();
            return Ok(positions);
        }




    }
}

