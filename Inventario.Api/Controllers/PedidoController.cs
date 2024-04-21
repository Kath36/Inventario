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
                // Verificar si los campos cliente y estado son nulos o vacíos
                if (string.IsNullOrEmpty(pedidoDto.Cliente) || string.IsNullOrEmpty(pedidoDto.Estado))
                {
                    return BadRequest(
                        new { message = "Los campos de cliente y estado son obligatorios" });
                }

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var response = new Response<PedidoDto>();

                // Aquí puedes realizar cualquier validación adicional necesaria antes de guardar el pedido

                var pedidoDtoWithId = new PedidoDto
                {
                    Cliente = pedidoDto.Cliente,
                    Fecha_Pedido = DateTime.Now,
                    Estado = pedidoDto.Estado
                };

                response.Data = await _pedidoService.SaveAsync(pedidoDtoWithId);

                // Devolver un mensaje indicando que el pedido se agregó correctamente
                response.Message = "Pedido agregado correctamente";

                return Created($"/api/[controller]/{response.Data.id}", response);
            }
            catch (Exception ex)
            {
                // Loguea la excepción para futura referencia
                Console.WriteLine($"Error en el método Post: {ex}");

                // Retorna un código de estado 500 junto con un mensaje de error genérico
                return StatusCode(500,
                    new { message = "Tienes que colocar el cliente correcto y/o el Estado en que esta ese pedido." });
            }
        }

        [HttpGet]
        [Route("{id:int}")]
        public async Task<ActionResult<Response<PedidoDto>>> GetById(int id)
        {
            var response = new Response<PedidoDto>();

            // Verificar si el pedido existe antes de intentar obtenerlo
            if (!await _pedidoService.PedidoExists(id))
            {
                response.Errors.Add("No existe");
                return NotFound(response);
            }

            // Si el pedido existe, obtenerlo y devolverlo en la respuesta
            response.Data = await _pedidoService.GetById(id);
            return response;
        }

        [HttpPut]
        public async Task<ActionResult<Response<PedidoDto>>> Update([FromBody] PedidoDto pedidoDto)
        {
            var response = new Response<PedidoDto>();

            if (!await _pedidoService.PedidoExists(pedidoDto.id))
            {
                response.Errors.Add("No existe");
                return NotFound(response);
            }

            // Crear un nuevo PedidoDto sin incluir la propiedad Fecha_Pedido
            var pedidoDtoToUpdate = new PedidoDto
            {
                id = pedidoDto.id,
                Cliente = pedidoDto.Cliente,
                Estado = pedidoDto.Estado
            };

            response.Data = await _pedidoService.UpdateAsync(pedidoDtoToUpdate);

            // Agregar un mensaje de éxito al objeto de respuesta
            response.Message = "El pedido se actualizó correctamente";

            return Ok(response);
        }


        [HttpDelete]
        [Route("{id:int}")]
        public async Task<ActionResult<Response<bool>>> Delete(int id)
        {
            var response = new Response<bool>();

            if (!await _pedidoService.DeleteAsync(id))
            {
                response.Errors.Add("El ID ingresado no existe");
                return NotFound(response);
            }

            // Agregar un mensaje de éxito al objeto de respuesta
            response.Message = "El pedido se eliminó correctamente";

            return Ok(response);
        }
    }
}
