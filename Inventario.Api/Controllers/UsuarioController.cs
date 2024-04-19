// UsuariosController.cs
using Microsoft.AspNetCore.Mvc;
using Inventario.Api.Dto;
using Inventario.Services.Interfaces;
using System.Threading.Tasks;
using Inventario.Core.Http;

namespace Inventario.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IUsuarioService _usuarioService;

        public AuthController(IUsuarioService usuarioService)
        {
            _usuarioService = usuarioService;
        }

        [HttpPost("authenticate")]
        public async Task<ActionResult<UsuarioDto>> Authenticate([FromBody] LoginRequestDto loginRequest)
        {
            var usuario = await _usuarioService.AuthenticateAsync(loginRequest.Email, loginRequest.Contraseña);
            if (usuario == null)
                return BadRequest(new { message = "Correo electrónico o contraseña incorrectos" });

            return Ok(usuario);
        }

        [HttpGet]
        public async Task<ActionResult<Response<List<UsuarioDto>>>> GetAll()
        {
            var response = new Response<List<UsuarioDto>>
            {
                Data = await _usuarioService.GetAllUsuariosAsync()
            };
            return Ok(response);
        }

        [HttpPost]
        public async Task<ActionResult<Response<UsuarioDto>>> Post([FromBody] UsuarioDto usuarioDto)
        {
            var response = new Response<UsuarioDto>
            {
                Data = await _usuarioService.RegistrarUsuarioAsync(usuarioDto)
            };
            return Created($"/api/[controller]/{response.Data.id}", response);
        }

        [HttpGet]
        [Route("{id:int}")]
        public async Task<ActionResult<Response<UsuarioDto>>> GetById(int id)
        {
            var response = new Response<UsuarioDto>();
            if (!await _usuarioService.UsuarioExists(id))
            {
                response.Errors.Add("Usuario no encontrado");
                return NotFound(response);
            }
            response.Data = await _usuarioService.GetUsuarioByIdAsync(id);
            return Ok(response);
        }

        [HttpPut]
        public async Task<ActionResult<Response<UsuarioDto>>> Update([FromBody] UsuarioDto usuarioDto)
        {
            var response = new Response<UsuarioDto>();
            if (!await _usuarioService.UsuarioExists(usuarioDto.id))
            {
                response.Errors.Add("Usuario no encontrado");
                return NotFound(response);
            }
            response.Data = await _usuarioService.ActualizarUsuarioAsync(usuarioDto);
            return Ok(response);
        }

        [HttpDelete]
        [Route("{id:int}")]
        public async Task<ActionResult<Response<bool>>> Delete(int id)
        {
            var response = new Response<bool>();
            if (!await _usuarioService.UsuarioExists(id))
            {
                response.Errors.Add("Usuario no encontrado");
                return NotFound(response);
            }
            response.Data = await _usuarioService.EliminarUsuarioAsync(id);
            return Ok(response);
        }
    }
}
