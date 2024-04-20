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
    public class PedidoService : IPedidoService
    {
        private readonly IPedidoRepository _pedidoRepository;

        public PedidoService(IPedidoRepository pedidoRepository)
        {
            _pedidoRepository = pedidoRepository;
        }

        public async Task<bool> PedidoExists(int id)
        {
            var pedido = await _pedidoRepository.GetById(id);
            return (pedido != null);
        }

        public async Task<PedidoDto> SaveAsync(PedidoDto pedidoDto)
        {
            var pedido = new Pedido
            {
                Cliente = pedidoDto.Cliente,
                Fecha_Pedido = pedidoDto.Fecha_Pedido,
                Estado = pedidoDto.Estado,
                CreatedBy = "Kath",
                CreatedDate = DateTime.Now,
                UpdatedBy = "Kath",
                UpdatedDate = DateTime.Now
            };

            pedido = await _pedidoRepository.SaveAsync(pedido);
            pedidoDto.id = pedido.id;
            return pedidoDto;
        }

        public async Task<PedidoDto> UpdateAsync(PedidoDto pedidoDto)
        {
            var pedido = await _pedidoRepository.GetById(pedidoDto.id);

            if (pedido == null)
                throw new Exception("Pedido not found");

            pedido.Cliente = pedidoDto.Cliente;
            pedido.Fecha_Pedido = pedidoDto.Fecha_Pedido;
            pedido.Estado = pedidoDto.Estado;
            pedido.UpdatedBy = "Kath";
            pedido.UpdatedDate = DateTime.Now;

            await _pedidoRepository.UpdateAsync(pedido);
            return pedidoDto;
        }


        public async Task<List<PedidoDto>> GetAllAsync()
        {
            var pedidos = await _pedidoRepository.GetAllAsync();
            var pedidosDto = pedidos.Select(p => new PedidoDto(p)).ToList();
            return pedidosDto;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            return await _pedidoRepository.DeleteAsync(id);
        }

        public async Task<PedidoDto> GetById(int id)
        {
            var pedido = await _pedidoRepository.GetById(id);
            if (pedido == null)
                throw new Exception("Pedido not found");

            var pedidoDto = new PedidoDto(pedido);
            return pedidoDto;
        }
    }
}
