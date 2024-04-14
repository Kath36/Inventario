using System.Collections.Generic;
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
    public class ProveedoresController : ControllerBase
    {
        private readonly IProveedorService _proveedorService;

        public ProveedoresController(IProveedorService proveedorService)
        {
            _proveedorService = proveedorService;
        }

        [HttpGet]
        public async Task<ActionResult<Response<List<ProveedorDto>>>> GetAll()
        {
            var response = new Response<List<ProveedorDto>>
            {
                Data = await _proveedorService.GetAllAsync()
            };
            return Ok(response);
        }

        [HttpPost]
        public async Task<ActionResult<Response<ProveedorDto>>> Post([FromBody] ProveedorDto proveedorDto)
        {
            var response = new Response<ProveedorDto>()
            {
                Data = await _proveedorService.SaveAsync(proveedorDto)
            };
            return Created($"/api/[controller]/{response.Data.id}", response);
        }

        [HttpGet]
        [Route("{id:int}")]
        public async Task<ActionResult<Response<ProveedorDto>>> GetById(int id)
        {
            var response = new Response<ProveedorDto>();

            if (!await _proveedorService.ProveedorExists(id))
            {
                response.Errors.Add("Proveedor not found");
                return NotFound(response);
            }

            response.Data = await _proveedorService.GetByIdAsync(id);
            return Ok(response);
        }

        [HttpPut]
        public async Task<ActionResult<Response<ProveedorDto>>> Update([FromBody] ProveedorDto proveedorDto)
        {
            var response = new Response<ProveedorDto>();

            if (!await _proveedorService.ProveedorExists(proveedorDto.id))
            {
                response.Errors.Add("Proveedor not found");
                return NotFound(response);
            }

            response.Data = await _proveedorService.UpdateAsync(proveedorDto);
            return Ok(response);
        }

        [HttpDelete]
        [Route("{id:int}")]
        public async Task<ActionResult<Response<bool>>> Delete(int id)
        {
            var response = new Response<bool>();

            if (!await _proveedorService.DeleteAsync(id))
            {
                response.Errors.Add("Proveedor not found");
                return NotFound(response);
            }

            return Ok(response);
        }
    }
}
