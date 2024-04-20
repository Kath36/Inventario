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
    public class ProveedoresController : ControllerBase
    {
        private readonly IProveedorService _proveedorService;

        public ProveedoresController(IProveedorService proveedorService)
        {
            _proveedorService = proveedorService;
        }

        [HttpGet]
        public async Task<ActionResult<Response<List<ProveedorDto>>>> GetAll()
        {
            var response = new Response<List<ProveedorDto>>();

            try
            {
                response.Data = await _proveedorService.GetAllAsync();
                return Ok(response);
            }
            catch (Exception ex)
            {
                response.Errors.Add("No hay datos para mostrar");
                return StatusCode(500, response);
            }
        }

       
        [HttpPost]
        public async Task<ActionResult<Response<ProveedorDto>>> Post([FromBody] ProveedorDtoSinId proveedorDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var response = new Response<ProveedorDto>();

                // Aquí puedes realizar cualquier validación adicional necesaria antes de guardar el proveedor

                var proveedorDtoWithId = new ProveedorDto
                {
                    Nombre = proveedorDto.Nombre,
                    Direccion = proveedorDto.Direccion,
                    Telefono = proveedorDto.Telefono
                };

                response.Data = await _proveedorService.SaveAsync(proveedorDtoWithId);

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
        public async Task<ActionResult<Response<ProveedorDto>>> GetById(int id)
        {
            var response = new Response<ProveedorDto>();

            if (!await _proveedorService.ProveedorExists(id))
            {
                response.Errors.Add("No existe el ID ingresado verifiquelo");
                return NotFound(response);
            }

            response.Data = await _proveedorService.GetByIdAsync(id);
            return Ok(response);
        }

        [HttpPut]
        public async Task<ActionResult<Response<ProveedorDto>>> Update([FromBody] ProveedorDto proveedorDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var response = new Response<ProveedorDto>();

            if (!await _proveedorService.ProveedorExists(proveedorDto.id))
            {
                response.Errors.Add("No existe el ID ingresado verifiquelo");
                return NotFound(response);
            }

            response.Data = await _proveedorService.UpdateAsync(proveedorDto);
            return Ok(response);
        }

        [HttpDelete]
        [Route("{id:int}")]
        public async Task<ActionResult<Response<bool>>> Delete(int id)
        {
            var response = new Response<bool>();

            if (!await _proveedorService.DeleteAsync(id))
            {
                response.Errors.Add("No existe el ID ingresado verifiquelo");
                return NotFound(response);
            }

            return Ok(response);
        }
    }
}
