using System;

namespace Inventario.Core.Entities
{
    public class Material : EntityBase
    {
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public decimal Precio { get; set; }
        public string Unidad { get; set; }
    }
}