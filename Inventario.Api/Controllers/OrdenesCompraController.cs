using Inventario.Api.Dto;
using Inventario.Core.Http;
using Inventario.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
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
            try
            {
                var response = new Response<List<OrdenCompraDto>>
                {
                    Data = await _ordenCompraService.GetAllAsync()
                };
                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Noy hay datos" });
            }
        }

      [HttpPost]
        public async Task<ActionResult<Response<OrdenCompraDtoSinId>>> Post([FromBody] OrdenCompraDtoSinId ordenCompraDto)
        {
            try
            {
                // Validar si el modelo recibido es válido
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var response = new Response<OrdenCompraDto>();

                // Validar que los valores necesarios no sean nulos
                if (ordenCompraDto.MaterialId <= 0)
                {
                    ModelState.AddModelError(nameof(ordenCompraDto.MaterialId), "El MaterialId es obligatorio.");
                    return BadRequest(ModelState);
                }

                if (ordenCompraDto.Cantidad <= 0)
                {
                    ModelState.AddModelError(nameof(ordenCompraDto.Cantidad), "La Cantidad debe ser mayor que cero.");
                    return BadRequest(ModelState);
                }

                if (ordenCompraDto.ProveedorId <= 0)
                {
                    ModelState.AddModelError(nameof(ordenCompraDto.ProveedorId), "El ProveedorId es obligatorio.");
                    return BadRequest(ModelState);
                }

                // Crear un nuevo DTO con los datos recibidos
                var ordenCompraDtoWithId = new OrdenCompraDto
                {
                    MaterialId = ordenCompraDto.MaterialId,
                    Cantidad = ordenCompraDto.Cantidad,
                    ProveedorId = ordenCompraDto.ProveedorId,
                    FechaOrden = DateTime.Now // Puedes establecer la fecha de la orden aquí o recibir la fecha como parte del DTO
                };

                // Guardar la orden de compra y obtener la respuesta
                response.Data = await _ordenCompraService.SaveAsync(ordenCompraDtoWithId);

                // Devolver una respuesta 201 Created con el DTO de la orden de compra creada
                return Created($"/api/[controller]/{response.Data.id}", response);
            }
            catch (Exception ex)
            {
                // Loguear la excepción para futura referencia
                Console.WriteLine($"Error en el método Post: {ex}");

                // Devolver una respuesta 500 Internal Server Error junto con un mensaje de error genérico
                return StatusCode(500, new { message = "Ocurrió un error al procesar la solicitud." });
            }
        }

        [HttpGet]
        [Route("{id:int}")]
        public async Task<ActionResult<Response<OrdenCompraDto>>> GetById(int id)
        {
            try
            {
                var response = new Response<OrdenCompraDto>();

                if (!await _ordenCompraService.OrdenCompraExists(id))
                {
                    response.Errors.Add("Orden Compra no existe");
                    return NotFound(response);
                }

                response.Data = await _ordenCompraService.GetById(id);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "El ID ingresado no existe" });
            }
        }

        [HttpPut]
        public async Task<ActionResult<Response<OrdenCompraDto>>> Update([FromBody] OrdenCompraDto ordenCompraDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var response = new Response<OrdenCompraDto>();

                if (!await _ordenCompraService.OrdenCompraExists(ordenCompraDto.id))
                {
                    return StatusCode(500, new { message = "El ID ingresado no existe" });
                    return NotFound(response);
                }

                response.Data = await _ordenCompraService.UpdateAsync(ordenCompraDto);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "El ID ingresado no existe" });
            }
        }

        [HttpDelete]
        [Route("{id:int}")]
        public async Task<ActionResult<Response<bool>>> Delete(int id)
        {
            try
            {
                var response = new Response<bool>();

                if (!await _ordenCompraService.DeleteAsync(id))
                {
                    return StatusCode(500, new { message = "El ID ingresado no existe" });
                    return NotFound(response);
                }

                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "El ID ingresado no existe" });
            }
        }
    }
}
