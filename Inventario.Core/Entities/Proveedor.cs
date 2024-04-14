using System;

namespace Inventario.Core.Entities
{
    public class Proveedor : EntityBase
    {
        public string Nombre { get; set; }
        public string Direccion { get; set; }
        public string Telefono { get; set; }
    }
}