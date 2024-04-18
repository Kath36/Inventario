// IUsuarioService.cs
using Inventario.Api.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Inventario.Services.Interfaces
{
    public interface IUsuarioService
    {
        Task<bool> UsuarioExists(int id);
        Task<UsuarioDto> RegistrarUsuarioAsync(UsuarioDto usuario);
        Task<UsuarioDto> ActualizarUsuarioAsync(UsuarioDto usuario);
        Task<List<UsuarioDto>> GetAllUsuariosAsync();
        Task<bool> EliminarUsuarioAsync(int id);
        Task<UsuarioDto> GetUsuarioByIdAsync(int id);
    }
}