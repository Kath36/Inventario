using Inventario.Core.Entities;
using Inventario.Api.Dto;
using Inventario.Api.Repositories.Interfecies;
using Inventario.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Inventario.Api.Services
{
    public class DetallePedidoService : IDetallePedidoService
    {
        private readonly IDetallePedidoRepository _detallePedidoRepository;
        
        public DetallePedidoService(IDetallePedidoRepository detallePedidoRepository)
        {
            _detallePedidoRepository = detallePedidoRepository;
        }
        
        public async Task<bool> DetallePedidoExists(int id)
        {
            var detallePedido = await _detallePedidoRepository.GetById(id);
            return (detallePedido != null);
        }

        public async Task<DetallePedidoDto> SaveAsync(DetallePedidoDto detallePedidoDto)
        {
            var detallePedido = new DetallePedido
            {
                Pedido_ID = detallePedidoDto.Pedido_ID,
                Material_ID = detallePedidoDto.Material_ID,
                Cantidad = detallePedidoDto.Cantidad,
                CreatedBy = "",
                CreatedDate = DateTime.Now,
                UpdatedBy = "",
                UpdatedDate = DateTime.Now
            };
            detallePedido = await _detallePedidoRepository.SaveAsync(detallePedido);
            detallePedidoDto.id = detallePedido.id;
            return detallePedidoDto;
         }

        public async Task<DetallePedidoDto> UpdateAsync(DetallePedidoDto detallePedidoDto)
        {
            var detallePedido = await _detallePedidoRepository.GetById(detallePedidoDto.id);

            if (detallePedido == null)
                throw new Exception("DetallePedido not found");

            detallePedido.Pedido_ID = detallePedidoDto.Pedido_ID;
            detallePedido.Material_ID = detallePedidoDto.Material_ID;
            detallePedido.Cantidad = detallePedidoDto.Cantidad;
            detallePedido.UpdatedBy = "";
            detallePedido.UpdatedDate = DateTime.Now;

            await _detallePedidoRepository.UpdateAsync(detallePedido);
            return detallePedidoDto;
        }

        public async Task<List<DetallePedidoDto>> GetAllAsync()
        {
            var detallesPedidos = await _detallePedidoRepository.GetAllAsync();
            var detallesPedidosDto = detallesPedidos.Select(d => new DetallePedidoDto
            {
                id = d.id,
                Pedido_ID = d.Pedido_ID,
                Material_ID = d.Material_ID,
                Cantidad = d.Cantidad
            }).ToList();
            return detallesPedidosDto;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            return await _detallePedidoRepository.DeleteAsync(id);
        }

        public async Task<DetallePedidoDto> GetById(int id)
        {
            var detallePedido = await _detallePedidoRepository.GetById(id);
            if (detallePedido == null)
                throw new Exception("DetallePedido not found");
            
            var detallePedidoDto = new DetallePedidoDto
            {
                id = detallePedido.id,
                Pedido_ID = detallePedido.Pedido_ID,
                Material_ID = detallePedido.Material_ID,
                Cantidad = detallePedido.Cantidad
            };
            return detallePedidoDto;
        }
    }
}
