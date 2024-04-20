namespace Inventario.Api.Dto
{
    public class RegistroMaterialDtoSinId
    {
        public int MaterialId { get; set; }
        public int Cantidad { get; set; }
        public DateTime FechaRegistro { get; set; }
    }
}