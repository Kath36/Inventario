using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Inventario.Core.Entities;
using Inventario.Core.Http;
using Inventario.Api.Dto;
using Inventario.Api.Repositories.Interfecies;
using Inventario.Services.Interfaces;

namespace Inventario.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MaterialesController : ControllerBase
    {
        private readonly IMaterialService _materialService;

        public MaterialesController(IMaterialService materialService)
        {
            _materialService = materialService;
        }

        [HttpGet]
        public async Task<ActionResult<Response<List<MaterialDto>>>> GetAll()
        {
            var response = new Response<List<MaterialDto>>
            {
                Data = await _materialService.GetAllAsync()
            };
            return Ok(response);
        }

        [HttpPost]
        public async Task<ActionResult<Response<MaterialDto>>> Post([FromBody] MaterialDto materialDto)
        {
            var response = new Response<MaterialDto>()
            {
                Data = await _materialService.SaveAsync(materialDto)
            };
            return Created($"/api/[controller]/{response.Data.id}", response);
        }

        [HttpGet]
        [Route("{id:int}")]
        public async Task<ActionResult<Response<MaterialDto>>> GetById(int id)
        {
            var response = new Response<MaterialDto>();

            if (!await _materialService.MaterialExists(id))
            {
                response.Errors.Add("Material not found");
                return NotFound(response);
            }

            response.Data = await _materialService.GetByIdAsync(id);
            return Ok(response);
        }

        [HttpPut]
        public async Task<ActionResult<Response<MaterialDto>>> Update([FromBody] MaterialDto materialDto)
        {
            var response = new Response<MaterialDto>();

            if (!await _materialService.MaterialExists(materialDto.id))
            {
                response.Errors.Add("Material not found");
                return NotFound(response);
            }

            response.Data = await _materialService.UpdateAsync(materialDto);
            return Ok(response);
        }

        [HttpDelete]
        [Route("{id:int}")]
        public async Task<ActionResult<Response<bool>>> Delete(int id)
        {
            var response = new Response<bool>();

            if (!await _materialService.DeleteAsync(id))
            {
                response.Errors.Add("Material not found");
                return NotFound(response);
            }

            return Ok(response);
        }
    }
}
