using System;

namespace Inventario.Core.Entities
{
    public class OrdenCompra : EntityBase
    {
        public int Material_ID { get; set; }
        public int Cantidad { get; set; }
        public int Proveedor_ID { get; set; }
        public DateTime Fecha_Orden { get; set; }
       
    }
}