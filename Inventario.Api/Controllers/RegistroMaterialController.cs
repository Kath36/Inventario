using System;
using System.Collections.Generic;
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
    public class RegistrosMaterialController : ControllerBase
    {
        private readonly IRegistroMaterialService _registroMaterialService;

        public RegistrosMaterialController(IRegistroMaterialService registroMaterialService)
        {
            _registroMaterialService = registroMaterialService;
        }

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
                response.Errors.Add("Error al obtener los registros de material: ");
                return StatusCode(500, response);
            }
        }

        [HttpPost]
        public async Task<ActionResult<Response<RegistroMaterialDto>>> Post([FromBody] RegistroMaterialDtoSinId registroMaterialDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var response = new Response<RegistroMaterialDto>();

                // Aquí puedes realizar cualquier validación adicional necesaria antes de guardar el registro de material

                var registroMaterialDtoWithId = new RegistroMaterialDto
                {
                    MaterialId = registroMaterialDto.MaterialId,
                    Cantidad = registroMaterialDto.Cantidad,
                    FechaRegistro = DateTime.Now // Puedes asignar la fecha actual o la fecha proporcionada en el DTO
                };

                response.Data = await _registroMaterialService.SaveAsync(registroMaterialDtoWithId);

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
                response.Errors.Add("El ID ");
                return StatusCode(500, response);
            }
        }

        [HttpPut]
        public async Task<ActionResult<Response<RegistroMaterialDto>>> Update([FromBody] RegistroMaterialDto registroMaterialDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var response = new Response<RegistroMaterialDto>();

            if (!await _registroMaterialService.RegistroMaterialExists(registroMaterialDto.id))
            {
                response.Errors.Add("Registro de material no encontrado");
                return NotFound(response);
            }

            try
            {
                response.Data = await _registroMaterialService.UpdateAsync(registroMaterialDto);
                return Ok(response);
            }
            catch (Exception ex)
            {
                response.Errors.Add("Error al actualizar el registro de material: ");
                return StatusCode(500, response);
            }
        }

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
