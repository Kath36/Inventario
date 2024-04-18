using Inventario.Core.Entities;

namespace Inventario.Api.Dto
{
    public class UsuarioDto : DtoBase
    {
        public string Email { get; set; }
        public string Contraseña { get; set; }

        public UsuarioDto()
        {
            
        }

        public UsuarioDto(Usuario usuario)
        {
            id = usuario.id;
            Email = usuario.Email;
            Contraseña = usuario.Contraseña;
            // Puedes agregar más propiedades si es necesario, como nombre, apellido, etc.
        }
    }
}