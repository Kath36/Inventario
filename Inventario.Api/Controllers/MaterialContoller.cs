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
    public class MaterialesController : ControllerBase
    {
        private readonly IMaterialService _materialService;

        public MaterialesController(IMaterialService materialService)
        {
            _materialService = materialService;
        }
// OPTENER TODOOOO //////////////////////////////////////////////////////////////////////////////////////////////////////////////
        [HttpGet]
        public async Task<ActionResult<Response<List<MaterialDto>>>> GetAll()
        {
            try
            {
                var response = new Response<List<MaterialDto>>
                {
                    Data = await _materialService.GetAllAsync()
                };
                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "No hay datos que mostrar "});
            }
        }
// AGREGAR //////////////////////////////////////////////////////////////////////////////////////////////////////////////

[HttpPost]
public async Task<ActionResult<Response<MaterialDto>>> Post([FromBody] MaterialDtoSinId materialDto)
{
    try
    {
        // Validar si el modelo recibido es válido
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var response = new Response<MaterialDto>();

        // Validar que ningún dato sea nulo
        if (string.IsNullOrEmpty(materialDto.Nombre))
        {
            ModelState.AddModelError(nameof(materialDto.Nombre), "El nombre del material es obligatorio.");
            return BadRequest(ModelState);
        }

        if (string.IsNullOrEmpty(materialDto.Descripcion))
        {
            ModelState.AddModelError(nameof(materialDto.Descripcion), "La descripción del material es obligatoria.");
            return BadRequest(ModelState);
        }

        if (materialDto.Precio <= 0)
        {
            ModelState.AddModelError(nameof(materialDto.Precio), "El precio del material debe ser mayor que cero.");
            return BadRequest(ModelState);
        }

        if (string.IsNullOrEmpty(materialDto.Unidad))
        {
            ModelState.AddModelError(nameof(materialDto.Unidad), "La unidad del material es obligatoria.");
            return BadRequest(ModelState);
        }

        // Crear un nuevo DTO con los datos recibidos
        var materialDtoWithId = new MaterialDto
        {
            Nombre = materialDto.Nombre,
            Descripcion = materialDto.Descripcion,
            Precio = materialDto.Precio,
            Unidad = materialDto.Unidad
        };

        // Guardar el material y obtener la respuesta
        response.Data = await _materialService.SaveAsync(materialDtoWithId);

        // Agregar un mensaje de éxito a la respuesta
        response.Message = "El material se agregó exitosamente.";

        // Devolver una respuesta 201 Created con el DTO del material creado
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


// OBTENER POR ID //////////////////////////////////////////////////////////////////////////////////////////////////////////////

        [HttpGet]
        [Route("{id:int}")]
        public async Task<ActionResult<Response<MaterialDto>>> GetById(int id)
        {
            try
            {
                var response = new Response<MaterialDto>();

                if (!await _materialService.MaterialExists(id))
                {
                    response.Errors.Add("No existe el ID ingresado");
                    return NotFound(response);
                }

                response.Data = await _materialService.GetByIdAsync(id);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "No existe el ID ingresado"});
            }
        }

        
// ACTUALIZAR  //////////////////////////////////////////////////////////////////////////////////////////////////////////////

        [HttpPut]
        public async Task<ActionResult<Response<MaterialDto>>> Update([FromBody] MaterialDto materialDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var response = new Response<MaterialDto>();

                if (!await _materialService.MaterialExists(materialDto.id))
                {
                    response.Errors.Add("No existe el ID ingresado");
                    return NotFound(response);
                }

                // Actualizar el material en la base de datos
                response.Data = await _materialService.UpdateAsync(materialDto);

// Agregar un mensaje de éxito al objeto de respuesta
                response.Message = "Material actualizado correctamente";

// Devolver una respuesta 200 OK con el DTO del material actualizado y el mensaje de éxito
                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "No existe el ID ingresado"});
            }
        }
// ELIMINAR //////////////////////////////////////////////////////////////////////////////////////////////////////////////

        [HttpDelete]
        [Route("{id:int}")]
        public async Task<ActionResult<Response<bool>>> Delete(int id)
        {
            try
            {
                var response = new Response<bool>();

                // Eliminar el material de la base de datos
                if (!await _materialService.DeleteAsync(id))
                {
                    response.Errors.Add("Material no se pudo eliminar, verifica si el ID existe");
                    return NotFound(response);
                }

// Agregar un mensaje de éxito al objeto de respuesta
                response.Message = "Material eliminado correctamente";

// Devolver una respuesta 200 OK con el mensaje de éxito
                return Ok(response);

            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "No existe el ID ingresado" + ex.Message });
            }
        }
    }
}
//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

