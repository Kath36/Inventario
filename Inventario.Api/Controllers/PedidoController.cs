using System.Data;
using Microsoft.AspNetCore.Http.HttpResults;
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
        public async Task<ActionResult<Response<PedidoDto>>> Post([FromBody] PedidoDtoSinId pedidoDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var response = new Response<PedidoDto>();

                // Aquí puedes realizar cualquier validación adicional necesaria antes de guardar el pedido

                var pedidoDtoWithId = new PedidoDto
                {
                    Cliente = pedidoDto.Cliente,
                    Fecha_Pedido = pedidoDto.Fecha_Orden,
                    Estado = pedidoDto.Estado
                };

                response.Data = await _pedidoService.SaveAsync(pedidoDtoWithId);

                return Created($"/api/[controller]/{response.Data.id}", response);
            }
            catch (Exception ex)
            {
                // Loguea la excepción para futura referencia
                Console.WriteLine($"Error en el método Post: {ex}");

                // Retorna un código de estado 500 junto con un mensaje de error genérico
                return StatusCode(500, new { message = "Ocurrió un error al procesar la solicitud." });
            }
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
