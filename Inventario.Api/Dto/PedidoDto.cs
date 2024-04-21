using System.Text.Json.Serialization;
using Inventario.Core.Entities;

namespace Inventario.Api.Dto
{
    public class PedidoDto : DtoBase
    {
        public string Cliente { get; set; }
        [JsonIgnore]
        public DateTime Fecha_Pedido { get; set; }
        public string Estado { get; set; }

        public PedidoDto()
        {

        }

        public PedidoDto(Pedido pedido)
        {
            id = pedido.id;
            Cliente = pedido.Cliente;
            Fecha_Pedido = pedido.UpdatedDate;
            Estado = pedido.Estado;
        }
    }
}