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
    public class RegistroMaterialService : IRegistroMaterialService
    {
        private readonly IRegistroMaterialRepository _registroMaterialRepository;

        public RegistroMaterialService(IRegistroMaterialRepository registroMaterialRepository)
        {
            _registroMaterialRepository = registroMaterialRepository;
        }

        public async Task<bool> RegistroMaterialExists(int id)
        {
            var registroMaterial = await _registroMaterialRepository.GetById(id);
            return (registroMaterial != null);
        }

        public async Task<RegistroMaterialDto> SaveAsync(RegistroMaterialDto registroMaterialDto)
        {
            var registroMaterial = new RegistroMaterial
            {
                Material_ID = registroMaterialDto.MaterialId,
                Cantidad = registroMaterialDto.Cantidad,
                Fecha_Registro = registroMaterialDto.FechaRegistro,
                CreatedBy = "",
                CreatedDate = DateTime.Now,
                UpdatedBy = "",
                UpdatedDate = DateTime.Now
            };
            registroMaterial = await _registroMaterialRepository.SaveAsync(registroMaterial);
            registroMaterialDto.id = registroMaterial.id;
            return registroMaterialDto;
        }

        public async Task<RegistroMaterialDto> UpdateAsync(RegistroMaterialDto registroMaterialDto)
        {
            var registroMaterial = await _registroMaterialRepository.GetById(registroMaterialDto.id);

            if (registroMaterial == null)
                throw new Exception("Registro Material not found");

            registroMaterial.Material_ID = registroMaterialDto.MaterialId;
            registroMaterial.Cantidad = registroMaterialDto.Cantidad;
            registroMaterial.Fecha_Registro = registroMaterialDto.FechaRegistro;
            registroMaterial.UpdatedBy = "";
            registroMaterial.UpdatedDate = DateTime.Now;

            await _registroMaterialRepository.UpdateAsync(registroMaterial);
            return registroMaterialDto;
        }

        public async Task<List<RegistroMaterialDto>> GetAllAsync()
        {
            var registrosMaterial = await _registroMaterialRepository.GetAllAsync();
            var registrosMaterialDto = registrosMaterial.Select(r => new RegistroMaterialDto(r)).ToList();
            return registrosMaterialDto;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            return await _registroMaterialRepository.DeleteAsync(id);
        }

        public async Task<RegistroMaterialDto> GetByIdAsync(int id)
        {
            var registroMaterial = await _registroMaterialRepository.GetById(id);
            if (registroMaterial == null)
                throw new Exception("Registro Material not found");
            var registroMaterialDto = new RegistroMaterialDto(registroMaterial);
            return registroMaterialDto;
        }
    }
}
