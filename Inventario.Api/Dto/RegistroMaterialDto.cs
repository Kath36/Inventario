using System.Text.Json.Serialization;
using Inventario.Core.Entities;

namespace Inventario.Api.Dto
{
    public class RegistroMaterialDto : DtoBase
    {
        public int MaterialId { get; set; }
        public int Cantidad { get; set; }

        [JsonIgnore]
        public DateTime Fecha_Registro { get; set; }

        public RegistroMaterialDto()
        {
            
        }

        public RegistroMaterialDto(RegistroMaterial registroMaterial)
        {
            id = registroMaterial.id;
            MaterialId = registroMaterial.Material_ID;
            Cantidad = registroMaterial.Cantidad;
            Fecha_Registro = registroMaterial.UpdatedDate;
        }
    }
}