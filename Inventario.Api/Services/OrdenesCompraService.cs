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
    public class OrdenCompraService : IOrdenCompraService
    {
        private readonly IOrdenCompraRepository _ordenCompraRepository;

        public OrdenCompraService(IOrdenCompraRepository ordenCompraRepository)
        {
            _ordenCompraRepository = ordenCompraRepository;
        }

        public async Task<bool> OrdenCompraExists(int id)
        {
            var ordenCompra = await _ordenCompraRepository.GetById(id);
            return (ordenCompra != null);
        }

        public async Task<OrdenCompraDto> SaveAsync(OrdenCompraDto ordenCompraDto)
        {
            var ordenCompra = new OrdenCompra
            {
                Material_ID = ordenCompraDto.MaterialId,
                Cantidad = ordenCompraDto.Cantidad,
                Proveedor_ID = ordenCompraDto.ProveedorId,
                Fecha_Orden = ordenCompraDto.FechaOrden,
                CreatedBy = "",
                CreatedDate = DateTime.Now,
                UpdatedBy = "",
                UpdatedDate = DateTime.Now
            };
            ordenCompra = await _ordenCompraRepository.SaveAsync(ordenCompra);
            ordenCompraDto.id = ordenCompra.id;
            return ordenCompraDto;
        }

        public async Task<OrdenCompraDto> UpdateAsync(OrdenCompraDto ordenCompraDto)
        {
            var ordenCompra = await _ordenCompraRepository.GetById(ordenCompraDto.id);

            if (ordenCompra == null)
                throw new Exception("OrdenCompra not found");

            ordenCompra.Material_ID = ordenCompraDto.MaterialId;
            ordenCompra.Cantidad = ordenCompraDto.Cantidad;
            ordenCompra.Proveedor_ID = ordenCompraDto.ProveedorId;
            ordenCompra.Fecha_Orden = ordenCompraDto.FechaOrden;
            ordenCompra.UpdatedBy = "";
            ordenCompra.UpdatedDate = DateTime.Now;

            await _ordenCompraRepository.UpdateAsync(ordenCompra);
            return ordenCompraDto;
        }

        public async Task<List<OrdenCompraDto>> GetAllAsync()
        {
            var ordenesCompra = await _ordenCompraRepository.GetAllAsync();
            var ordenesCompraDto = ordenesCompra.Select(o => new OrdenCompraDto(o)).ToList();
            return ordenesCompraDto;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            return await _ordenCompraRepository.DeleteAsync(id);
        }

        public async Task<OrdenCompraDto> GetById(int id)
        {
            var ordenCompraEntity = await _ordenCompraRepository.GetById(id);
            if (ordenCompraEntity == null)
            {
                // Manejo de caso cuando no se encuentra la orden de compra
                // Puedes lanzar una excepción específica, devolver nulo, etc.
                throw new Exception($"OrdenCompra with id {id} not found");
            }
    
            // Utilizar el constructor que acepta una entidad OrdenCompra para inicializar el DTO
            var ordenCompraDto = new OrdenCompraDto(ordenCompraEntity);
    
            return ordenCompraDto;
        }



        public async Task<OrdenCompraDto> GetByIdAsync(int id)
        {
            var ordenCompra = await _ordenCompraRepository.GetById(id);
            if (ordenCompra == null)
                throw new Exception("OrdenCompra not found");
            var ordenCompraDto = new OrdenCompraDto(ordenCompra);
            return ordenCompraDto;
        }
    }
}
