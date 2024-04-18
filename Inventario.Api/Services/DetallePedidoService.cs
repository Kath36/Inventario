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
            // Validar que los campos no sean nulos o vacíos
            if (detallePedidoDto == null)
            {
                throw new ArgumentNullException(nameof(detallePedidoDto), "Los datos del detalle de pedido no pueden ser nulos.");
            }

            if (detallePedidoDto.Pedido_ID <= 0)
            {
                throw new ArgumentException("El Pedido_ID debe ser mayor que cero.");
            }

            if (detallePedidoDto.Material_ID <= 0)
            {
                throw new ArgumentException("El Material_ID debe ser mayor que cero.");
            }

            if (detallePedidoDto.Cantidad <= 0)
            {
                throw new ArgumentException("La Cantidad debe ser mayor que cero.");
            }

            // Resto del código para guardar el detalle del pedido en la base de datos...
        
            var detallePedido = new DetallePedido
            {
                Pedido_ID = detallePedidoDto.Pedido_ID,
                Material_ID = detallePedidoDto.Material_ID,
                Cantidad = detallePedidoDto.Cantidad,
                CreatedBy = "Kath",
                CreatedDate = DateTime.Now,
                UpdatedBy = "Kath",
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

            if (detallePedidoDto.Pedido_ID <= 0)
            {
                throw new ArgumentException("El Pedido_ID debe ser mayor que cero.");
            }

            if (detallePedidoDto.Material_ID <= 0)
            {
                throw new ArgumentException("El Material_ID debe ser mayor que cero.");
            }

            if (detallePedidoDto.Cantidad <= 0)
            {
                throw new ArgumentException("La Cantidad debe ser mayor que cero.");
            }

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
            // Validar si el detalle de pedido existe antes de eliminarlo
            var detallePedidoExistente = await _detallePedidoRepository.GetById(id);
            if (detallePedidoExistente == null)
            {
                throw new ArgumentException($"No se encontró ningún detalle de pedido con el id {id}.");
            }

            return await _detallePedidoRepository.DeleteAsync(id);
        }

        public async Task<DetallePedidoDto> GetById(int id)
        {
            // Validar si el detalle de pedido existe antes de obtenerlo
            var detallePedido = await _detallePedidoRepository.GetById(id);
            if (detallePedido == null)
            {
                throw new ArgumentException($"No se encontró ningún detalle de pedido con el id {id}.");
            }
            
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
