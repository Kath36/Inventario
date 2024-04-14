using Inventario.Core.Entities;

namespace Inventario.Api.Dto
{
    public class OrdenCompraDto : DtoBase
    {
        public int MaterialId { get; set; }
        public int Cantidad { get; set; }
        public int ProveedorId { get; set; }
        public DateTime FechaOrden { get; set; }

        public OrdenCompraDto()
        {
            
        }

        public OrdenCompraDto(OrdenCompra ordenCompra)
        {
            id = ordenCompra.id;
            MaterialId = ordenCompra.Material_ID;
            Cantidad = ordenCompra.Cantidad;
            ProveedorId = ordenCompra.Proveedor_ID;
            FechaOrden = ordenCompra.Fecha_Orden;
        }
    }
}