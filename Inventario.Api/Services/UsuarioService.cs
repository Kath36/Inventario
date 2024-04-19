using Inventario.Api.Dto;
using Inventario.Api.Repositories.Interfaces;
using Inventario.Services.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Inventario.Core.Entities;

namespace Inventario.Api.Services
{
    public class UsuarioService : IUsuarioService
    {
        private readonly IUsuarioRepository _usuarioRepository;

        public UsuarioService(IUsuarioRepository usuarioRepository)
        {
            _usuarioRepository = usuarioRepository;
        }
        public async Task<UsuarioDto> AuthenticateAsync(string email, string contraseña)
        {
            var usuario = await _usuarioRepository.GetByEmailAsync(email);
            // Verificar si el usuario existe y si la contraseña es correcta
            if (usuario == null || usuario.Contraseña != contraseña)
                return null;

            // Autenticación exitosa, devolver el usuario DTO
            return new UsuarioDto(usuario);
        }
        public async Task<bool> UsuarioExists(int id)
        {
            return await _usuarioRepository.UsuarioExistsByIdAsync(id);
        }
        public async Task<bool> UsuarioExistsByEmailAsync(string email)
        {
            return await _usuarioRepository.UsuarioExistsByEmailAsync(email);
        }

        public async Task<UsuarioDto> RegistrarUsuarioAsync(UsuarioDto usuarioDto)
        {
            if (await UsuarioExistsByEmailAsync(usuarioDto.Email))
            {
                throw new Exception("El correo electrónico ya está registrado.");
            }

            var usuario = new Usuario
            {
                Email = usuarioDto.Email,
                Contraseña = usuarioDto.Contraseña,
                CreatedBy = "Kath",
                CreatedDate = DateTime.Now,
                UpdatedBy = "Kath",
                UpdatedDate = DateTime.Now
            };

            // Guardar el usuario en la base de datos
            var usuarioGuardado = await _usuarioRepository.AddAsync(usuario);

            // Asignar el ID generado al DTO de usuario
            usuarioDto.id = usuarioGuardado.id;

            return usuarioDto;
        }

        public async Task<List<UsuarioDto>> GetAllUsuariosAsync()
        {
            var usuarios = await _usuarioRepository.GetAllAsync();
            return usuarios.Select(u => new UsuarioDto(u)).ToList();
        }

        public async Task<UsuarioDto> GetUsuarioByIdAsync(int id)
        {
            var usuario = await _usuarioRepository.GetByIdAsync(id);
            if (usuario == null)
            {
                throw new Exception("Usuario no encontrado.");
            }
            return new UsuarioDto(usuario);
        }

        public async Task<UsuarioDto> ActualizarUsuarioAsync(UsuarioDto usuarioDto)
        {
            // Verificar si el usuario existe
            var usuarioExistente = await _usuarioRepository.GetByIdAsync(usuarioDto.id);
            if (usuarioExistente == null)
            {
                throw new Exception("Usuario no encontrado.");
            }

            // Actualizar los datos del usuario
            usuarioExistente.Email = usuarioDto.Email;
            usuarioExistente.Contraseña = usuarioDto.Contraseña;
            usuarioExistente.UpdatedBy = "Kath";
            usuarioExistente.UpdatedDate = DateTime.Now;

            // Guardar los cambios en la base de datos
            var actualizado = await _usuarioRepository.UpdateAsync(usuarioExistente);
    
            // Aquí podrías cargar de nuevo el usuario actualizado desde el repositorio si lo necesitas,
            // o simplemente devolver el DTO pasado como parámetro
            return usuarioDto;
        }


        public async Task<bool> EliminarUsuarioAsync(int id)
        {
            // Verificar si el usuario existe
            var usuarioExistente = await _usuarioRepository.GetByIdAsync(id);
            if (usuarioExistente == null)
            {
                throw new Exception("Usuario no encontrado.");
            }

            // Eliminar el usuario de la base de datos
            return await _usuarioRepository.DeleteAsync(id);
        }
    }
}
