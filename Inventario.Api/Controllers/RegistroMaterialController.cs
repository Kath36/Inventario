using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Inventario.Core.Http;
using Inventario.Api.Dto;
using Inventario.Services.Interfaces;

namespace Inventario.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RegistrosMaterialController : ControllerBase
    {
        private readonly IRegistroMaterialService _registroMaterialService;
        private readonly IMaterialService _materialService;

        public RegistrosMaterialController(IRegistroMaterialService registroMaterialService, IMaterialService materialService)
        {
            _registroMaterialService = registroMaterialService;
            _materialService = materialService;
        }
        /// ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////7777
        [HttpGet]
        public async Task<ActionResult<Response<List<RegistroMaterialDto>>>> GetAll()
        {
            var response = new Response<List<RegistroMaterialDto>>();

            try
            {
                response.Data = await _registroMaterialService.GetAllAsync();
                return Ok(response);
            }
            catch (Exception ex)
            {
                response.Errors.Add("Error al obtener los registros de material: " + ex.Message);
                return StatusCode(500, response);
            }
        }
/// ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////7777

[HttpPost]
public async Task<ActionResult<Response<RegistroMaterialDtoSinId>>> Post([FromBody] RegistroMaterialDtoSinId registroMaterialDto)
{
    try
    {
        // Validar si el modelo recibido es válido
        if (!ModelState.IsValid)
        {
            // Recopilar todos los errores de validación del modelo en una lista
            var validationErrors = new List<string>();
            foreach (var value in ModelState.Values)
            {
                foreach (var error in value.Errors)
                {
                    validationErrors.Add(error.ErrorMessage);
                }
            }

            // Devolver BadRequest con la lista de errores de validación
            return BadRequest(new { errors = validationErrors });
        }

        // Validar que los valores necesarios no sean nulos o inválidos
        if (registroMaterialDto.MaterialId <= 0)
        {
            ModelState.AddModelError(nameof(registroMaterialDto.MaterialId), "El MaterialId es obligatorio.");
        }

        if (registroMaterialDto.Cantidad <= 0)
        {
            ModelState.AddModelError(nameof(registroMaterialDto.Cantidad), "La Cantidad debe ser mayor que cero.");
        }

        // Validar que el ID material exista en la tabla de materiales
        if (!await _materialService.MaterialExists(registroMaterialDto.MaterialId))
        {
            ModelState.AddModelError(nameof(registroMaterialDto.MaterialId), "El MaterialId no existe.");
        }

        // Verificar si hay errores después de la validación adicional
        if (!ModelState.IsValid)
        {
            // Recopilar todos los errores de validación en una lista
            var validationErrors = new List<string>();
            foreach (var value in ModelState.Values)
            {
                foreach (var error in value.Errors)
                {
                    validationErrors.Add(error.ErrorMessage);
                }
            }

            // Devolver BadRequest con la lista de errores de validación
            return BadRequest(new { errors = validationErrors });
        }

        // Crear un nuevo DTO con los datos recibidos
        var registroMaterialDtoWithId = new RegistroMaterialDto
        {
            MaterialId = registroMaterialDto.MaterialId,
            Cantidad = registroMaterialDto.Cantidad,
            Fecha_Registro = DateTime.Now // Puedes establecer la fecha de registro aquí o recibir la fecha como parte del DTO
        };

        // Guardar el registro de material
        await _registroMaterialService.SaveAsync(registroMaterialDtoWithId);

        // Devolver un mensaje de éxito
        return Ok(new { message = "El registro de material se agregó correctamente." });
    }
    catch (Exception ex)
    {
        // Devolver una respuesta 500 Internal Server Error junto con un mensaje de error
        return StatusCode(500, new { message = "Ocurrió un error al procesar la solicitud: " + ex.Message });
    }
}
/// ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////7777
        [HttpGet]
        [Route("{id:int}")]
        public async Task<ActionResult<Response<RegistroMaterialDto>>> GetById(int id)
        {
            var response = new Response<RegistroMaterialDto>();

            if (!await _registroMaterialService.RegistroMaterialExists(id))
            {
                response.Errors.Add("Registro de material no encontrado");
                return NotFound(response);
            }

            try
            {
                response.Data = await _registroMaterialService.GetByIdAsync(id);
                return Ok(response);
            }
            catch (Exception ex)
            {
                response.Errors.Add("Registro de material no encontrado");
                return StatusCode(500, response);
            }
        } 
/// ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////7777
[HttpPut]
public async Task<ActionResult<Response<RegistroMaterialDto>>> Update([FromBody] RegistroMaterialDto registroMaterialDto)
{
    try
    {
        // Validar si el modelo recibido es válido
        if (!ModelState.IsValid)
        {
            // Si hay errores de validación en el modelo, devolver BadRequest con los errores
            return BadRequest(ModelState);
        }

        // Validar si el registro de material existe
        if (!await _registroMaterialService.RegistroMaterialExists(registroMaterialDto.id))
        {
            // Si no existe, agregar un error al response y devolver NotFound
            var response = new Response<RegistroMaterialDto>();
            response.Errors.Add("Registro de material no encontrado");
            return NotFound(response);
        }

        // Validar que el ID del material existe
        if (!await _materialService.MaterialExists(registroMaterialDto.MaterialId))
        {
            // Si no existe, agregar un error al response y devolver BadRequest
            var response = new Response<RegistroMaterialDto>();
            response.Errors.Add("El MaterialId no existe.");
            return BadRequest(response);
        }

        // Validar que la cantidad no sea nula o negativa
        if (registroMaterialDto.Cantidad <= 0)
        {
            // Si es nula o negativa, agregar un error al response y devolver BadRequest
            var response = new Response<RegistroMaterialDto>();
            response.Errors.Add("La Cantidad debe ser mayor que cero.");
            return BadRequest(response);
        }

        // Realizar la actualización del registro de material
        var updatedRegistroMaterial = await _registroMaterialService.UpdateAsync(registroMaterialDto);

        // Devolver Ok con la respuesta y un mensaje de éxito
        var okResponse = new Response<RegistroMaterialDto>();
        okResponse.Data = updatedRegistroMaterial;
        okResponse.Message = "El registro de material se actualizó correctamente.";
        return Ok(okResponse);
    }
    catch (Exception ex)
    {
        // Loguear la excepción para futura referencia
        Console.WriteLine($"Error en el método Update: {ex}");

        // Devolver una respuesta 500 Internal Server Error junto con un mensaje de error genérico
        return StatusCode(500, new { message = "Ocurrió un error al procesar la solicitud." });
    }
}

/// ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////7777
    [HttpDelete]
        [Route("{id:int}")]
        public async Task<ActionResult<Response<bool>>> Delete(int id)
        {
            var response = new Response<bool>();

            if (!await _registroMaterialService.DeleteAsync(id))
            {
                response.Errors.Add("Registro de material no encontrado");
                return NotFound(response);
            }

            return Ok(response);
        }
    }
}
/// ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////7777