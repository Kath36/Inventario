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
            try
            {
                var response = new Response<List<PedidoDto>>
                {
                    Data = await _pedidoService.GetAllAsync()
                };
                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "No hay datos"  });
            }
        }

        [HttpPost]
        public async Task<ActionResult<Response<PedidoDto>>> Post([FromBody] PedidoDto pedidoDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var response = new Response<PedidoDto>()
                {
                    Data = await _pedidoService.SaveAsync(pedidoDto)
                };
                return Created($"/api/[controller]/{response.Data.id}", response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Verifica que hayas llenado correctamente los campos " });
            }
            }

            [HttpGet]
        [Route("{id:int}")]
        public async Task<ActionResult<Response<PedidoDto>>> GetById(int id)
        {
            var response = new Response<PedidoDto>();

            if (!await _pedidoService.PedidoExists(id))
            {
                return StatusCode(500, new { message = "El ID ingresado no existe" });
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
                return StatusCode(500, new { message = "El ID ingresado no existe" });
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
                return StatusCode(500, new { message = "El ID ingresado no existe" });
                return NotFound(response);
            }

            return Ok(response);
        }
    }
}
