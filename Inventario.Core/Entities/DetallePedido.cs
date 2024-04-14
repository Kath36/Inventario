using System;

namespace Inventario.Core.Entities
{
    public class DetallePedido : EntityBase
    {
        public int Pedido_ID { get; set; }
        public int Material_ID { get; set; }
        public int Cantidad { get; set; }
    }
}