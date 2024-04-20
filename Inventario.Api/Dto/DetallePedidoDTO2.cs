using Inventario.Core.Entities;

namespace Inventario.Api.Dto
{
    public class DetallePedidoDtoSinId
    {
        public int Pedido_ID { get; set; }
        public int Material_ID { get; set; }
        public int Cantidad { get; set; }

        public DetallePedidoDtoSinId()
        {

        }

        public DetallePedidoDtoSinId(DetallePedido detallePedido)
        {
            Pedido_ID = detallePedido.Pedido_ID;
            Material_ID = detallePedido.Material_ID;
            Cantidad = detallePedido.Cantidad;
        }
    }
}