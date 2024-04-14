namespace Inventario.Core.Entities
{
    public class Pedido : EntityBase
    {
        public string Cliente { get; set; }
        public DateTime Fecha_Pedido { get; set; }
        public string Estado { get; set; }
    }
}