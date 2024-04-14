using Microsoft.AspNetCore.Mvc;
using Inventario.Core.Entities;
using Inventario.Core.Http;
using Inventario.Api.Dto;
using Inventario.Services.Interfaces;

namespace Inventario.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PedidosController : ControllerBase
    {
        private readonly IPedidoService _pedidoService;

        public PedidosController(IPedidoService pedidoService)
        {
            _pedidoService = pedidoService;
        }

        [HttpGet]
        public async Task<ActionResult<Response<List<PedidoDto>>>> GetAll()
        {
            var response = new Response<List<PedidoDto>>
            {
                Data = await _pedidoService.GetAllAsync()
            };
            return Ok(response);
        }

        [HttpPost]
        public async Task<ActionResult<Response<PedidoDto>>> Post([FromBody] PedidoDto pedidoDto)
        {
            var response = new Response<PedidoDto>()
            {
                Data = await _pedidoService.SaveAsync(pedidoDto)
            };
            return Created($"/api/[controller]/{response.Data.id}", response);
        }

        [HttpGet]
        [Route("{id:int}")]
        public async Task<ActionResult<Response<PedidoDto>>> GetById(int id)
        {
            var response = new Response<PedidoDto>();

            if (!await _pedidoService.PedidoExists(id))
            {
                response.Errors.Add("Pedido not found");
                return NotFound(response);
            }

            response.Data = await _pedidoService.GetById(id);
            return Ok(response);
        }

        [HttpPut]
        public async Task<ActionResult<Response<PedidoDto>>> Update([FromBody] PedidoDto pedidoDto)
        {
            var response = new Response<PedidoDto>();

            if (!await _pedidoService.PedidoExists(pedidoDto.id))
            {
                response.Errors.Add("Pedido not found");
                return NotFound(response);
            }

            response.Data = await _pedidoService.UpdateAsync(pedidoDto);
            return Ok(response);
        }

        [HttpDelete]
        [Route("{id:int}")]
        public async Task<ActionResult<Response<bool>>> Delete(int id)
        {
            var response = new Response<bool>();

            if (!await _pedidoService.DeleteAsync(id))
            {
                response.Errors.Add("Pedido not found");
                return NotFound(response);
            }

            return Ok(response);
        }
    }
}
