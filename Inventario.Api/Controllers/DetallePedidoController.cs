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
            var response = new Response<List<DetallePedidoDto>>
            {
                Data = await _detallePedidoService.GetAllAsync()
            };
            return Ok(response);
        }
   [HttpPost]
public async Task<ActionResult<Response<DetallePedidoDto>>> Post([FromBody] DetallePedidoDtoSinId detallePedidoDtoSinId)
{
    var response = new Response<DetallePedidoDto>();

    try
    {
        // Validar que los campos no sean nulos o vacíos
        if (detallePedidoDtoSinId == null)
        {
            response.Errors.Add("Los datos del detalle de pedido no pueden estar vacíos.");
            return BadRequest(response);
        }

        // Validar que los campos requeridos no estén vacíos
        if (detallePedidoDtoSinId.Pedido_ID <= 0)
        {
            response.Errors.Add("El ID del pedido no es válido.");
            return BadRequest(response);
        }

        if (detallePedidoDtoSinId.Material_ID <= 0)
        {
            response.Errors.Add("El ID del material no es válido.");
            return BadRequest(response);
        }

        if (detallePedidoDtoSinId.Cantidad <= 0)
        {
            response.Errors.Add("La cantidad debe ser mayor que cero.");
            return BadRequest(response);
        }

        // Verificar que el ID del pedido existe en la tabla de pedidos
        try
        {
            var pedidoExists = await _pedidoService.PedidoExists(detallePedidoDtoSinId.Pedido_ID);
            if (!pedidoExists)
            {
                response.Errors.Add("El ID del pedido no existe en la tabla de pedidos.");
                return BadRequest(response);
            }
        }
        catch (Exception ex)
        {
            response.Errors.Add($"Error al verificar la existencia del ID del pedido: {ex.Message}");
            return StatusCode(500, response);
        }

        // Verificar que el ID del material existe en la tabla de materiales
        try
        {
            var materialExists = await _materialService.MaterialExists(detallePedidoDtoSinId.Material_ID);
            if (!materialExists)
            {
                response.Errors.Add("El ID del material no existe en la tabla de materiales.");
                return BadRequest(response);
            }
        }
        catch (Exception ex)
        {
            response.Errors.Add($"Error al verificar la existencia del ID del material: {ex.Message}");
            return StatusCode(500, response);
        }

        // Convertir DetallePedidoDtoSinId a DetallePedidoDto
        var detallePedidoDto = new DetallePedidoDto
        {
            Pedido_ID = detallePedidoDtoSinId.Pedido_ID,
            Material_ID = detallePedidoDtoSinId.Material_ID,
            Cantidad = detallePedidoDtoSinId.Cantidad
        };

        // Guardar el detalle del pedido utilizando el servicio
        response.Data = await _detallePedidoService.SaveAsync(detallePedidoDto);

        // Devolver una respuesta de éxito con el detalle del pedido guardado
        return Created($"/api/DetallePedidos/{response.Data.Pedido_ID}", response);
    }
    catch (Exception ex)
    {
        // Manejar cualquier excepción y devolver una respuesta de error
        response.Errors.Add($"Error al procesar la solicitud: {ex.Message}");
        return StatusCode(500, response);
    }
}


        
        [HttpGet]
        [Route("{id:int}")]
        public async Task<ActionResult<Response<DetallePedidoDto>>> GetById(int id)
        {
            var response = new Response<DetallePedidoDto>();

            if (!await _detallePedidoService.DetallePedidoExists(id))
            {
                response.Errors.Add("DetallePedido not found");
                return NotFound(response);
            }

            response.Data = await _detallePedidoService.GetById(id);
            return Ok(response);
        }

        [HttpPut]
        public async Task<ActionResult<Response<DetallePedidoDto>>> Update([FromBody] DetallePedidoDto detallePedidoDto)
        {
            var response = new Response<DetallePedidoDto>();

            if (!await _detallePedidoService.DetallePedidoExists(detallePedidoDto.id))
            {
                response.Errors.Add("DetallePedido not found");
                return NotFound(response);
            }

            response.Data = await _detallePedidoService.UpdateAsync(detallePedidoDto);
            return Ok(response);
        }

        [HttpDelete]
        [Route("{id:int}")]
        public async Task<ActionResult<Response<bool>>> Delete(int id)
        {
            var response = new Response<bool>();

            if (!await _detallePedidoService.DeleteAsync(id))
            {
                response.Errors.Add("DetallePedido not found");
                return NotFound(response);
            }

            return Ok(response);
        }
    }
}
