using Microsoft.AspNetCore.Mvc;
using Inventario.Core.Http;
using Inventario.Api.Dto;
using Inventario.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Inventario.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DetallePedidosController : ControllerBase
    {
        private readonly IDetallePedidoService _detallePedidoService;

        public DetallePedidosController(IDetallePedidoService detallePedidoService)
        {
            _detallePedidoService = detallePedidoService;
        }

        [HttpGet]
        public async Task<ActionResult<Response<List<DetallePedidoDto>>>> GetAll()
        {
            var response = new Response<List<DetallePedidoDto>>();
            try
            {
                response.Data = await _detallePedidoService.GetAllAsync();
                return Ok(response);
            }
            catch (Exception ex)
            {
                response.Errors.Add("No hay datos");
                return StatusCode(500, response);
            }
        }

        [HttpPost]
        public async Task<ActionResult<Response<DetallePedidoDto>>> Post([FromBody] DetallePedidoDto detallePedidoDto)
        {
            var response = new Response<DetallePedidoDto>();

            try
            {
                if (detallePedidoDto == null)
                {
                    response.Errors.Add("La solicitud no puede estar vacía. Asegúrate de enviar los datos del detalle del pedido.");
                    return BadRequest(response);
                }

                // Validar que el campo pedido_ID no sea negativo
                if (detallePedidoDto.Pedido_ID < 0)
                {
                    response.Errors.Add("El ID del pedido no puede ser negativo. Por favor, proporciona un valor válido.");
                    return BadRequest(response);
                }

                // Validar que el campo material_ID no sea negativo y sea un número entero
                if (detallePedidoDto.Material_ID <= 0)
                {
                    response.Errors.Add("El ID del material debe ser un número entero positivo. Por favor, proporciona un valor válido.");
                    return BadRequest(response);
                }

                // Validar que la cantidad sea un número entero positivo
                if (detallePedidoDto.Cantidad <= 0)
                {
                    response.Errors.Add("La cantidad debe ser un número entero positivo. Por favor, proporciona un valor válido.");
                    return BadRequest(response);
                }

                // Guardar el detalle del pedido si todas las validaciones son exitosas
                var savedDetallePedido = await _detallePedidoService.SaveAsync(detallePedidoDto);
                response.Data = savedDetallePedido;
                return Created($"/api/DetallePedidos/{savedDetallePedido.id}", response);
            }
            catch (Exception ex)
            {
                response.Errors.Add("Se produjo un error al procesar la solicitud. Por favor, inténtalo de nuevo más tarde.");
                return StatusCode(500, response);
            }
        }


        [HttpGet]
        [Route("{id:int}")]
        public async Task<ActionResult<Response<DetallePedidoDto>>> GetById(int id)
        {
            var response = new Response<DetallePedidoDto>();
            try
            {
                var detallePedidoExists = await _detallePedidoService.DetallePedidoExists(id);
                if (!detallePedidoExists)
                {
                    response.Errors.Add("El detalle de pedido no se encontró.");
                    return NotFound(response);
                }

                response.Data = await _detallePedidoService.GetById(id);
                return Ok(response);
            }
            catch (Exception ex)
            {
                response.Errors.Add("Error, no existe el ID ingresado.");
                return StatusCode(500, response);
            }
        }

        [HttpPut]
        public async Task<ActionResult<Response<DetallePedidoDto>>> Update([FromBody] DetallePedidoDto detallePedidoDto)
        {
            var response = new Response<DetallePedidoDto>();
            try
            {
                if (detallePedidoDto == null)
                {
                    response.Errors.Add("Los datos del detalle de pedido no pueden estar vacíos.");
                    return BadRequest(response);
                }

                var detallePedidoExists = await _detallePedidoService.DetallePedidoExists(detallePedidoDto.id);
                if (!detallePedidoExists)
                {
                    response.Errors.Add("El ID especidicado no se encontró.");
                    return NotFound(response);
                }

                response.Data = await _detallePedidoService.UpdateAsync(detallePedidoDto);
                return Ok(response);
            }
            catch (Exception ex)
            {
                response.Errors.Add("El ID especidicado no se encontró.");
                return StatusCode(500, response);
            }
        }

        [HttpDelete]
        [Route("{id:int}")]
        public async Task<ActionResult<Response<bool>>> Delete(int id)
        {
            var response = new Response<bool>();
            try
            {
                var deleted = await _detallePedidoService.DeleteAsync(id);
                if (!deleted)
                {
                    response.Errors.Add("El ID especidicado no se encontró.");
                    return NotFound(response);
                }

                return Ok(response);
            }
            catch (Exception ex)
            {
                response.Errors.Add("El ID especidicado no se encontró.");
                return StatusCode(500, response);
            }
        }
    }
}
