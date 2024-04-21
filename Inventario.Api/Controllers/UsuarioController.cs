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
            try
            {
                // Verificar si los campos no son nulos
                if (loginRequest == null || string.IsNullOrEmpty(loginRequest.Email) ||
                    string.IsNullOrEmpty(loginRequest.Contraseña))
                {
                    return BadRequest(
                        new { message = "Los campos de correo electrónico y contraseña son obligatorios" });
                }

                // Continuar con la autenticación
                var usuario = await _usuarioService.AuthenticateAsync(loginRequest.Email, loginRequest.Contraseña);
                if (usuario == null)
                    return BadRequest(new { message = "Correo electrónico o contraseña incorrectos" });

                return Ok(usuario);
            }
            catch (Exception ex)
            {
                // Loguea la excepción para futura referencia
                Console.WriteLine($"Error en el método Authenticate: {ex}");

                // Retorna un código de estado 500 junto con un mensaje de error genérico
                return StatusCode(500, new { message = "Ocurrió un error al procesar la solicitud." });
            }
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
        public async Task<ActionResult<Response<UsuarioDto>>> Post([FromBody] UsuarioDtoSinId usuarioDto)
        {
            try
            {
                // Verificar si los campos no son nulos
                if (usuarioDto == null || string.IsNullOrEmpty(usuarioDto.Email) ||
                    string.IsNullOrEmpty(usuarioDto.Contraseña))
                {
                    return BadRequest(
                        new { message = "Los campos de correo electrónico y contraseña son obligatorios" });
                }

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var response = new Response<UsuarioDto>();

                // Aquí puedes realizar cualquier validación adicional necesaria antes de guardar el usuario

                var usuarioDtoWithId = new UsuarioDto
                {
                    Email = usuarioDto.Email,
                    Contraseña = usuarioDto.Contraseña
                };

                response.Data = await _usuarioService.RegistrarUsuarioAsync(usuarioDtoWithId);

                // Agregar mensaje de éxito a la respuesta
                response.Message = "El usuario se agregó correctamente";

                return Created($"/api/[controller]/{response.Data.id}", response);
            }
            catch (Exception ex)
            {
                // Loguea la excepción para futura referencia
                Console.WriteLine($"Error en el método Post: {ex}");

                // Retorna un código de estado 500 junto con un mensaje de error genérico
                return StatusCode(500, new { message = "Por seguridad, crea un correo y una contraseña" });
            }
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
            try
            {
                if (!await _usuarioService.UsuarioExists(usuarioDto.id))
                {
                    response.Errors.Add("Usuario no encontrado");
                    return NotFound(response);
                }

                response.Data = await _usuarioService.ActualizarUsuarioAsync(usuarioDto);

                // Agregar mensaje de éxito a la respuesta
                response.Message = "El usuario se actualizó correctamente";

                return Ok(response);
            }
            catch (Exception ex)
            {
                // Loguea la excepción para futura referencia
                Console.WriteLine($"Error en el método Update: {ex}");

                // Retorna un código de estado 500 junto con un mensaje de error genérico
                return StatusCode(500, new { message = "Este correo ya existe, ingrese otro" });
            }
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

            // Eliminar el usuario
            response.Data = await _usuarioService.EliminarUsuarioAsync(id);

            // Agregar mensaje de éxito
            response.Message = "Usuario eliminado correctamente";

            return Ok(response);
        }
    }
}
