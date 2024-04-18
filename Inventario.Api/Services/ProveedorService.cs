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
    public class ProveedorService : IProveedorService
    {
        private readonly IProveedorRepository _proveedorRepository;

        public ProveedorService(IProveedorRepository proveedorRepository)
        {
            _proveedorRepository = proveedorRepository;
        }

        public async Task<bool> ProveedorExists(int id)
        {
            var proveedor = await _proveedorRepository.GetById(id);
            return (proveedor != null);
        }

        public async Task<ProveedorDto> SaveAsync(ProveedorDto proveedorDto)
        {
            var proveedor = new Proveedor
            {
                Nombre = proveedorDto.Nombre,
                Direccion = proveedorDto.Direccion,
                Telefono = proveedorDto.Telefono,
                CreatedBy = "Kath",
                CreatedDate = DateTime.Now,
                UpdatedBy = "Kath",
                UpdatedDate = DateTime.Now
            };
            proveedor = await _proveedorRepository.SaveAsync(proveedor);
            proveedorDto.id = proveedor.id;
            return proveedorDto;
        }

        public async Task<ProveedorDto> UpdateAsync(ProveedorDto proveedorDto)
        {
            var proveedor = await _proveedorRepository.GetById(proveedorDto.id);

            if (proveedor == null)
                throw new Exception("Proveedor not found");

            proveedor.Nombre = proveedorDto.Nombre;
            proveedor.Direccion = proveedorDto.Direccion;
            proveedor.Telefono = proveedorDto.Telefono;
            proveedor.UpdatedBy = "";
            proveedor.UpdatedDate = DateTime.Now;

            await _proveedorRepository.UpdateAsync(proveedor);
            return proveedorDto;
        }

        public async Task<List<ProveedorDto>> GetAllAsync()
        {
            var proveedores = await _proveedorRepository.GetAllAsync();
            var proveedoresDto = proveedores.Select(p => new ProveedorDto(p)).ToList();
            return proveedoresDto;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            return await _proveedorRepository.DeleteAsync(id);
        }

        public async Task<ProveedorDto> GetByIdAsync(int id)
        {
            var proveedor = await _proveedorRepository.GetById(id);
            if (proveedor == null)
                throw new Exception("Proveedor not found");
            var proveedorDto = new ProveedorDto(proveedor);
            return proveedorDto;
        }
    }
}
