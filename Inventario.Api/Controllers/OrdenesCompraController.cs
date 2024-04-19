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
        public async Task<ActionResult<Response<OrdenCompraDto>>> Post([FromBody] OrdenCompraDto ordenCompraDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var response = new Response<OrdenCompraDto>()
                {
                    Data = await _ordenCompraService.SaveAsync(ordenCompraDto)
                };
                return Created($"/api/[controller]/{response.Data.id}", response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Verifica que el ID proveedor y material existan y/o que hayas colocado una cantidad"});
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
