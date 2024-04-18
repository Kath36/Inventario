using System;
using Inventario.Core.Entities;

namespace Inventario.Core.Entities
{
    public class Usuario : EntityBase
    {
        public string Email { get; set; }
        public string Contraseña { get; set; }
    }
}