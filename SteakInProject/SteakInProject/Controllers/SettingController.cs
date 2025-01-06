using System;
using Microsoft.AspNetCore.Mvc;
using Repository.Exceptions;
using Service.Helpers.DTOs.Setting;
using Service.Services.Interfaces;

namespace SteakInProject.Controllers
{
	public class SettingController:BaseController
	{
        private readonly ISettingService _settingService;
        public SettingController(ISettingService tagService)
        {
            _settingService = tagService;
        }
        [ProducesResponseType(typeof(SettingDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(SettingDto), StatusCodes.Status404NotFound)]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            try
            {
                return Ok(await _settingService.GetByIdAsync(id));

            }
            catch (NotFoundException)
            {
                return NotFound();
            }
        }
        [ProducesResponseType(typeof(SettingDto), StatusCodes.Status200OK)]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {

            return Ok(await _settingService.GetAllAsync());
        }

        [ProducesResponseType(typeof(SettingDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(SettingDto), StatusCodes.Status404NotFound)]
        [HttpPut("{id}")]
        public async Task<IActionResult> Edit([FromRoute] int id, [FromBody] SettingEditDto request)
        {
            try
            {
                await _settingService.EditAsync(id, request);
                return Ok("Setting updated successfully");
            }
            catch (NotFoundException)
            {
                return NotFound();
            }
        }
    }
}

