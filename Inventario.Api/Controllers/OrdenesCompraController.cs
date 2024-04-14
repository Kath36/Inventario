using Inventario.Api.Dto;
using Inventario.Core.Http;
using Inventario.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Inventario.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrdenCompraController : ControllerBase
    {
        private readonly IOrdenCompraService _ordenCompraService;

        public OrdenCompraController(IOrdenCompraService ordenCompraService)
        {
            _ordenCompraService = ordenCompraService;
        }

        [HttpGet]
        public async Task<ActionResult<Response<List<OrdenCompraDto>>>> GetAll()
        {
            var response = new Response<List<OrdenCompraDto>>
            {
                Data = await _ordenCompraService.GetAllAsync()
            };
            return Ok(response);
        }

        [HttpPost]
        public async Task<ActionResult<Response<OrdenCompraDto>>> Post([FromBody] OrdenCompraDto ordenCompraDto)
        {
            var response = new Response<OrdenCompraDto>()
            {
                Data = await _ordenCompraService.SaveAsync(ordenCompraDto)
            };
            return Created($"/api/[controller]/{response.Data.id}", response);
        }

        [HttpGet]
        [Route("{id:int}")]
        public async Task<ActionResult<Response<OrdenCompraDto>>> GetById(int id)
        {
            var response = new Response<OrdenCompraDto>();

            if (!await _ordenCompraService.OrdenCompraExists(id))
            {
                response.Errors.Add("OrdenCompra not found");
                return NotFound(response);
            }

            response.Data = await _ordenCompraService.GetById(id);
            return Ok(response);
        }

        [HttpPut]
        public async Task<ActionResult<Response<OrdenCompraDto>>> Update([FromBody] OrdenCompraDto ordenCompraDto)
        {
            var response = new Response<OrdenCompraDto>();

            if (!await _ordenCompraService.OrdenCompraExists(ordenCompraDto.id))
            {
                response.Errors.Add("OrdenCompra not found");
                return NotFound(response);
            }

            response.Data = await _ordenCompraService.UpdateAsync(ordenCompraDto);
            return Ok(response);
        }

        [HttpDelete]
        [Route("{id:int}")]
        public async Task<ActionResult<Response<bool>>> Delete(int id)
        {
            var response = new Response<bool>();

            if (!await _ordenCompraService.DeleteAsync(id))
            {
                response.Errors.Add("OrdenCompra not found");
                return NotFound(response);
            }

            return Ok(response);
        }
    }
}
