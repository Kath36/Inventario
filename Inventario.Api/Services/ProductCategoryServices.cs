using Inventario.Core.Entities;
using Inventario.Api.Dto;
using Inventario.Api.Repositories.Interfecies;
using Inventario.Services.Interfaces;

namespace Inventario.Api.Services;

public class ProductCategoryServices : IProductCategoryService
{
    private readonly IProductCategoryRepository _productCategoryRepository;
    

    public ProductCategoryServices(IProductCategoryRepository productCategoryRepository)
    {
        _productCategoryRepository = productCategoryRepository;
    }
    
    public async Task<bool> ProductCategoryExist(int id)
    {
        var category = await _productCategoryRepository.GetById(id);
        return (category != null);
    }

    public async Task<ProductCategoryDto> SaveAsycn(ProductCategoryDto categoryDto)
    {
        var catetegory = new ProductCategory
        {
            Name = categoryDto.Name,
            Description = categoryDto.Description,
            CreatedBy = "Kath",
            CreatedDate = DateTime.Now,
            UpdatedBy = "Kath",
            UpdatedDate = DateTime.Now
        };
        catetegory = await _productCategoryRepository.SaveAsycn(catetegory);
        categoryDto.id = catetegory.id;
        return categoryDto;
    }

    public async Task<ProductCategoryDto> UpdateAsync(ProductCategoryDto categoryDto)
    {
        var category = await _productCategoryRepository.GetById(categoryDto.id);

        if (category == null)
            throw new Exception("Product Category Not founf");

        category.Name = categoryDto.Name;
        category.Description = categoryDto.Description;
        category.UpdatedBy = "Kath";
        category.UpdatedDate = DateTime.Now;
        await _productCategoryRepository.UpdateAsync(category);
        return categoryDto;
    }

    public async Task<List<ProductCategoryDto>> GetAllAsync()
    {
        var categories = await _productCategoryRepository.GetAllAsync();
        var categoriesDto = categories.Select(c => new ProductCategoryDto(c)).ToList();
        return categoriesDto;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        return await _productCategoryRepository.DeleteAsync(id);
    }

    public async Task<ProductCategoryDto> GetById(int id)
    {
        var category = await _productCategoryRepository.GetById(id);
        if (category == null)
            throw new Exception("Product category not Found");
        var categoryDto = new ProductCategoryDto(category);
        return categoryDto;
    }
}