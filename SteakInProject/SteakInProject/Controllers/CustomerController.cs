using System;
using Microsoft.AspNetCore.Mvc;
using Service.Helpers.DTOs.Customer;
using Service.Services.Interfaces;

namespace SteakInProject.Controllers
{
	public class CustomerController:BaseController
	{
        private readonly ICustomerService _costumerService;

        public CustomerController(ICustomerService customerService)
        {
            _costumerService = customerService;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromForm] CustomerCreateDto request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var response = await _costumerService.CreateAsync(request);

            if (response != "Success") return BadRequest(response);

            return Ok(response);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> Edit([FromRoute] int id, [FromForm] CustomerEditDto request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var response = await _costumerService.EditAsync(id, request);

            if (response != "Success")
            {
                return BadRequest(response);
            }

            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var datas = await _costumerService.GetAllAsync();
            return Ok(datas);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            return Ok(await _costumerService.GetById(id));
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var response = await _costumerService.DeleteAsync(id);

            if (response == "Data not found")
            {

                return NotFound(response);
            }
            return Ok(response);
        }
    }
}

