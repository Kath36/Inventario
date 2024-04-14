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
        public async Task<ActionResult<Response<DetallePedidoDto>>> Post([FromBody] DetallePedidoDto detallePedidoDto)
        {
            var response = new Response<DetallePedidoDto>()
            {
                Data = await _detallePedidoService.SaveAsync(detallePedidoDto)
            };
            return Created($"/api/[controller]/{response.Data.id}", response);
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
