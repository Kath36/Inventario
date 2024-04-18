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
    public class MaterialService : IMaterialService
    {
        private readonly IMaterialRepository _materialRepository;

        public MaterialService(IMaterialRepository materialRepository)
        {
            _materialRepository = materialRepository;
        }

        public async Task<bool> MaterialExists(int id)
        {
            var material = await _materialRepository.GetById(id);
            return material != null;
        }

        public async Task<MaterialDto> SaveAsync(MaterialDto materialDto)
        {
            var material = new Material
            {
                Nombre = materialDto.Nombre,
                Descripcion = materialDto.Descripcion,
                Precio = materialDto.Precio,
                Unidad = materialDto.Unidad,
                CreatedBy = "Kath",
                CreatedDate = DateTime.Now,
                UpdatedBy = "Kath",
                UpdatedDate = DateTime.Now
            };

            material = await _materialRepository.SaveAsync(material);
            materialDto.id = material.id;
            return materialDto;
        }

        public async Task<MaterialDto> UpdateAsync(MaterialDto materialDto)
        {
            var material = await _materialRepository.GetById(materialDto.id);

            if (material == null)
                throw new Exception("Material not found");

            material.Nombre = materialDto.Nombre;
            material.Descripcion = materialDto.Descripcion;
            material.Precio = materialDto.Precio;
            material.Unidad = materialDto.Unidad;
            material.UpdatedBy = "Kath";
            material.UpdatedDate = DateTime.Now;

            await _materialRepository.UpdateAsync(material);
            return materialDto;
        }

        public async Task<List<MaterialDto>> GetAllAsync()
        {
            var materials = await _materialRepository.GetAllAsync();
            var materialsDto = materials.Select(m => new MaterialDto(m)).ToList();
            return materialsDto;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            return await _materialRepository.DeleteAsync(id);
        }

        public async Task<MaterialDto> GetByIdAsync(int id)
        {
            var material = await _materialRepository.GetById(id);
            if (material == null)
                throw new Exception("Material not found");
            var materialDto = new MaterialDto(material);
            return materialDto;
        }
    }
}
