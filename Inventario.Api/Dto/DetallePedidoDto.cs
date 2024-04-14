using Inventario.Core.Entities;

namespace Inventario.Api.Dto
{
    public class DetallePedidoDto : DtoBase
    {
        public int Pedido_ID { get; set; }
        public int Material_ID { get; set; }
        public int Cantidad { get; set; }

        public DetallePedidoDto()
        {

        }

        public DetallePedidoDto(DetallePedido detallePedido)
        {
            id = detallePedido.id;
            Pedido_ID = detallePedido.Pedido_ID;
            Material_ID = detallePedido.Material_ID;
            Cantidad = detallePedido.Cantidad;
        }
    }
}