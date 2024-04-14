namespace Inventario.Core.Entities
{
    public class RegistroMaterial : EntityBase
    {
        public int Material_ID { get; set; }
        public int Cantidad { get; set; }
        public DateTime Fecha_Registro { get; set; }
        public bool IsDeleted { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }
    }
}