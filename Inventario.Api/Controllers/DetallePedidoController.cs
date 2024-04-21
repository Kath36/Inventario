using Microsoft.AspNetCore.Mvc;
using Inventario.Core.Http;
using Inventario.Api.Dto;
using Inventario.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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

        public DetallePedidosController(
            IDetallePedidoService detallePedidoService, 
            IMaterialService materialService, 
            IPedidoService pedidoService)
        {
            _detallePedidoService = detallePedidoService;
            _materialService = materialService;
            _pedidoService = pedidoService;
        }
// OPTENER TODOOOO //////////////////////////////////////////////////////////////////////////////////////////////////////////////

        [HttpGet]
        public async Task<ActionResult<Response<List<DetallePedidoDto>>>> GetAll()
        {
            var response = new Response<List<DetallePedidoDto>>
            {
                Data = await _detallePedidoService.GetAllAsync()
            };
            return Ok(response);
        }
        // Agregar //////////////////////////////////////////////////////////////////////////////////////////////////////////////

        
        
 [HttpPost]
public async Task<ActionResult<Response<DetallePedidoDto>>> Post([FromBody] DetallePedidoDtoSinId detallePedidoDtoSinId)
{
    var response = new Response<DetallePedidoDto>();

    try
    {
        // Validar que los datos recibidos no sean nulos
        if (detallePedidoDtoSinId == null)
        {
            response.Errors.Add("Los datos del detalle de pedido no pueden estar vacíos.");
            return BadRequest(response);
        }

        // Validar que los campos requeridos no estén vacíos
        if (detallePedidoDtoSinId.Pedido_ID <= 0)
        {
            response.Errors.Add("El ID del pedido no es válido.");
        }

        if (detallePedidoDtoSinId.Material_ID <= 0)
        {
            response.Errors.Add("El ID del material no es válido.");
        }

        if (detallePedidoDtoSinId.Cantidad <= 0)
        {
            response.Errors.Add("La cantidad debe ser mayor que cero.");
        }

        // Verificar si hay errores de validación hasta este punto
        if (response.Errors.Any())
        {
            return BadRequest(response);
        }

        // Verificar que el ID del pedido existe en la tabla de pedidos
        var pedidoExists = await _pedidoService.PedidoExists(detallePedidoDtoSinId.Pedido_ID);
        if (!pedidoExists)
        {
            response.Errors.Add("El ID del pedido no existe en la tabla de pedidos.");
        }

        // Verificar que el ID del material existe en la tabla de materiales
        var materialExists = await _materialService.MaterialExists(detallePedidoDtoSinId.Material_ID);
        if (!materialExists)
        {
            response.Errors.Add("El ID del material no existe en la tabla de materiales.");
        }

        // Verificar si hay errores de existencia de IDs
        if (response.Errors.Any())
        {
            return BadRequest(response);
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

// obtener por id //////////////////////////////////////////////////////////////////////////////////////////////////////////////

        [HttpGet]
        [Route("{id:int}")]
        public async Task<ActionResult<Response<DetallePedidoDto>>> GetById(int id)
        {
            var response = new Response<DetallePedidoDto>();

            if (!await _detallePedidoService.DetallePedidoExists(id))
            {
                response.Errors.Add("DetallePedido no existe");
                return NotFound(response);
            }

            response.Data = await _detallePedidoService.GetById(id);
            return Ok(response);
        }
// Actualizar   //////////////////////////////////////////////////////////////////////////////////////////////////////////////
        

[HttpPut]
public async Task<ActionResult<Response<DetallePedidoDto>>> Update([FromBody] DetallePedidoDto detallePedidoDto)
{
    var response = new Response<DetallePedidoDto>();
    var errors = new List<string>();

    try
    {
        // Validar que los campos no sean nulos o vacíos
        if (detallePedidoDto == null)
        {
            errors.Add("Los datos del detalle de pedido no pueden estar vacíos.");
        }

        // Validar que el ID del detalle de pedido sea válido
        if (detallePedidoDto.id <= 0)
        {
            errors.Add("El ID del detalle de pedido no es válido.");
        }

        // Verificar si el detalle de pedido a actualizar existe en la tabla de detalles de pedido
        if (!await _detallePedidoService.DetallePedidoExists(detallePedidoDto.id))
        {
            errors.Add("El detalle de pedido a actualizar no fue encontrado.");
        }

        // Verificar que el ID del pedido existe en la tabla de pedidos
        try
        {
            var pedidoExists = await _pedidoService.PedidoExists(detallePedidoDto.Pedido_ID);
            if (!pedidoExists)
            {
                errors.Add("El ID del pedido no existe en la tabla de pedidos.");
            }
        }
        catch (Exception ex)
        {
            errors.Add($"Error al verificar la existencia del ID del pedido: {ex.Message}");
        }

        // Verificar que el ID del material existe en la tabla de materiales
        try
        {
            var materialExists = await _materialService.MaterialExists(detallePedidoDto.Material_ID);
            if (!materialExists)
            {
                errors.Add("El ID del material no existe en la tabla de materiales.");
            }
        }
        catch (Exception ex)
        {
            errors.Add($"Error al verificar la existencia del ID del material: {ex.Message}");
        }

        // Si hay errores, agregarlos a la respuesta y devolver BadRequest
        if (errors.Any())
        {
            response.Errors.AddRange(errors);
            return BadRequest(response);
        }

        // Realizar la actualización del detalle del pedido
        response.Data = await _detallePedidoService.UpdateAsync(detallePedidoDto);

        // Agregar un mensaje indicando que se actualizó correctamente
        response.Message = "El detalle de pedido se actualizó correctamente.";

        // Devolver una respuesta de éxito con el detalle del pedido actualizado
        return Ok(response);
    }
    catch (Exception ex)
    {
        // Manejar cualquier excepción y devolver una respuesta de error
        response.Errors.Add($"Coloque una cantidad viable {ex.Message}");
        return StatusCode(500, response);
    }
}

// Eliminar     //////////////////////////////////////////////////////////////////////////////////////////////////////////////

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
