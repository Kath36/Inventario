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
    public class MaterialesController : ControllerBase
    {
        private readonly IMaterialService _materialService;

        public MaterialesController(IMaterialService materialService)
        {
            _materialService = materialService;
        }

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

        [HttpPost]
        public async Task<ActionResult<Response<MaterialDto>>> Post([FromBody] MaterialDtoSinId materialDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var response = new Response<MaterialDto>();

                // Aquí puedes realizar cualquier validación adicional necesaria antes de guardar el material

                var materialDtoWithId = new MaterialDto
                {
                    Nombre = materialDto.Nombre,
                    Descripcion = materialDto.Descripcion,
                    Precio = materialDto.Precio,
                    Unidad = materialDto.Unidad
                };

                response.Data = await _materialService.SaveAsync(materialDtoWithId);

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

                response.Data = await _materialService.UpdateAsync(materialDto);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "No existe el ID ingresado"});
            }
        }

        [HttpDelete]
        [Route("{id:int}")]
        public async Task<ActionResult<Response<bool>>> Delete(int id)
        {
            try
            {
                var response = new Response<bool>();

                if (!await _materialService.DeleteAsync(id))
                {
                    response.Errors.Add("Material no sepudo eliminar verifica si el ID existe");
                    return NotFound(response);
                }

                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "No existe el ID ingresado" + ex.Message });
            }
        }
    }
}
