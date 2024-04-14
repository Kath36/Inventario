using Inventario.Core.Entities;

namespace Inventario.Api.Dto
{
    public class ProveedorDto : DtoBase
    {
        public string Nombre { get; set; }
        public string Direccion { get; set; }
        public string Telefono { get; set; }

        public ProveedorDto()
        {
            
        }

        public ProveedorDto(Proveedor proveedor)
        {
            id = proveedor.id;
            Nombre = proveedor.Nombre;
            Direccion = proveedor.Direccion;
            Telefono = proveedor.Telefono;
        }
    }
}