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

//e////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
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

//e///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////        
        [HttpPost]
        public async Task<ActionResult<Response<OrdenCompraDtoSinId>>> Post(
            [FromBody] OrdenCompraDtoSinId ordenCompraDto)
        {
            try
            {
                // Validar si el modelo recibido es válido
                if (!ModelState.IsValid)
                {
                    // Si hay errores de validación en el modelo, devolver BadRequest con los errores
                    return BadRequest(ModelState);
                }

                // Validar que los valores necesarios no sean nulos o inválidos
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
                    FechaOrden =
                        DateTime.Now // Puedes establecer la fecha de la orden aquí o recibir la fecha como parte del DTO
                };

                // Guardar la orden de compra
                await _ordenCompraService.SaveAsync(ordenCompraDtoWithId);

                // Devolver un mensaje de éxito
                return Ok(new { message = "La orden de compra se agregó correctamente." });
            }
            catch (Exception ex)
            {
                // Loguear la excepción para futura referencia
                Console.WriteLine($"Error en el método Post: {ex}");

                // Devolver una respuesta 500 Internal Server Error junto con un mensaje de error genérico
                return StatusCode(500, new { message = "Verifica que el ID material y/o ID proveedor existan." });
            }
        }
//e/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        [HttpGet]
        [Route("{id:int}")]
        public async Task<ActionResult<Response<OrdenCompraDto>>> GetById(int id)
        {
            try
            {
                var response = new Response<OrdenCompraDto>();

                if (!await _ordenCompraService.OrdenCompraExists(id))
                {
                    return StatusCode(500, new { message = "El ID ingresado no existe" });
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

//e///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////        


        [HttpPut]
        public async Task<ActionResult<Response<OrdenCompraDto>>> Update([FromBody] OrdenCompraDto ordenCompraDto)
        {
            var response = new Response<OrdenCompraDto>();

            try
            {
                // Verificar si la orden de compra existe
                if (!await _ordenCompraService.OrdenCompraExists(ordenCompraDto.id))
                {
                    response.Errors.Add("Orden de compra no encontrada");
                    return NotFound(response);
                }

                // Validar que la cantidad no sea nula
                if (ordenCompraDto.Cantidad == 0)
                {
                    ModelState.AddModelError(nameof(ordenCompraDto.Cantidad), "La Cantidad es obligatoria.");
                    return BadRequest(ModelState);
                }

                // Crear un nuevo objeto OrdenCompraDto sin incluir la propiedad FechaOrden
                var ordenCompraDtoToUpdate = new OrdenCompraDto
                {
                    id = ordenCompraDto.id,
                    MaterialId = ordenCompraDto.MaterialId,
                    Cantidad = ordenCompraDto.Cantidad, // Accede al valor de cantidad usando .Value
                    ProveedorId = ordenCompraDto.ProveedorId
                };

                // Actualizar la orden de compra en la base de datos
                response.Data = await _ordenCompraService.UpdateAsync(ordenCompraDtoToUpdate);

                // Agregar un mensaje de éxito al objeto de respuesta
                response.Message = "La orden de compra se actualizó correctamente";

                return Ok(response);
            }
            catch (Exception ex)
            {
                // Loguear la excepción para futura referencia
                Console.WriteLine($"Error en el método Update: {ex}");

                // Devolver una respuesta 500 Internal Server Error junto con un mensaje de error genérico
                return StatusCode(500, new { message = "Verifica que el ID material y/o ID proveedor existan." });
            }
        }

//e///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////        

        [HttpDelete]
        [Route("{id:int}")]
        public async Task<ActionResult<Response<bool>>> Delete(int id)
        {
            try
            {
                var response = new Response<bool>();

                if (!await _ordenCompraService.DeleteAsync(id))
                {
                    return NotFound(new { message = "El ID ingresado no existe" });
                }

                // Agregar un mensaje de éxito al objeto de respuesta
                response.Message = "El elemento fue eliminado correctamente";

                return Ok(response);
            }
            catch (Exception ex)
            {
                // Loguear la excepción para futura referencia
                Console.WriteLine($"Error en el método Delete: {ex}");

                return StatusCode(500, new { message = "Ocurrió un error al procesar la solicitud." });
            }
        }
    }
}
//e///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////        
