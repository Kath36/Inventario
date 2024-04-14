using Inventario.Core.Entities;

namespace Inventario.Api.Dto
{
    public class MaterialDto : DtoBase
    {
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public decimal Precio { get; set; }
        public string Unidad { get; set; }

        public MaterialDto()
        {
            
        }

        public MaterialDto(Material material)
        {
            id = material.id;
            Nombre = material.Nombre;
            Descripcion = material.Descripcion;
            Precio = material.Precio;
            Unidad = material.Unidad;
        }
    }
}