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
        private readonly IMaterialService _materialService;
        private readonly IPedidoService _pedidoService;

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
                // Validación de tipo de datos y valores de los campos
                try
                {
                    if (detallePedidoDto.Pedido_ID <= 0)
                    {
                        response.Errors.Add("El ID del pedido no es válido");
                    }

                    if (detallePedidoDto.Material_ID <= 0)
                    {
                        response.Errors.Add("El ID del material no es válido");
                    }

                    if (detallePedidoDto.Cantidad <= 0)
                    {
                        response.Errors.Add("La cantidad debe ser mayor que cero");
                    }
                }
                catch (Exception ex)
                {
                    response.Errors.Add($"Error al validar los campos del detalle del pedido: {ex.Message}");
                }

                // Validación de IDs de tablas relacionales
                try
                {
                    if (!await _pedidoService.PedidoExists(detallePedidoDto.Pedido_ID))
                    {
                        response.Errors.Add("El ID del pedido no existe en la tabla de pedidos");
                    }

                    if (!await _pedidoService.PedidoExists(detallePedidoDto.Material_ID))
                    {
                        response.Errors.Add("El ID del material no existe en la tabla de materiales");
                    }
                }
                catch (Exception ex)
                {
                    response.Errors.Add($"Error, no existe el ID pedido ni ID material");
                }

                if (response.Errors.Any())
                {
                    return BadRequest(response);
                }

                // Validación de la cantidad ingresada
                if (detallePedidoDto.Cantidad == 0)
                {
                    response.Errors.Add("La cantidad no puede ser cero");
                    return BadRequest(response);
                }

                // Si todas las validaciones pasan, guardamos el detalle del pedido
                response.Data = await _detallePedidoService.SaveAsync(detallePedidoDto);
                return Created($"/api/[controller]/{response.Data.id}", response);
            }
            catch (Exception ex)
            {
                response.Errors.Add($"Error al guardar el detalle del pedido: {ex.Message}");
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
