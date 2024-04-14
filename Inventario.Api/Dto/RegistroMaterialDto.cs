using Inventario.Core.Entities;

namespace Inventario.Api.Dto
{
    public class RegistroMaterialDto : DtoBase
    {
        public int MaterialId { get; set; }
        public int Cantidad { get; set; }
        public DateTime FechaRegistro { get; set; }

        public RegistroMaterialDto()
        {
            
        }

        public RegistroMaterialDto(RegistroMaterial registroMaterial)
        {
            id = registroMaterial.id;
            MaterialId = registroMaterial.Material_ID;
            Cantidad = registroMaterial.Cantidad;
            FechaRegistro = registroMaterial.Fecha_Registro;
        }
    }
}