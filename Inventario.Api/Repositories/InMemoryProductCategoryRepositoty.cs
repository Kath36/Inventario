using Inventario.Core.Entities;
using Inventario.Api.Repositories.Interfecies;

namespace Inventario.Api.Repositories;

public class InMemoryProductCategoryRepositoty : IProductCategoryRepository
{

    private readonly List<ProductCategory> _categories;

    public InMemoryProductCategoryRepositoty()
    {
        _categories = new List<ProductCategory>();
    }
    public async Task<ProductCategory> SaveAsycn(ProductCategory category)
    {
        category.id = _categories.Count + 1;
        _categories.Add(category);

        return category;
    }

    public async Task<ProductCategory> UpdateAsync(ProductCategory category)
    {
        var index = _categories.FindIndex(x => x.id == category.id);
        if (index != -1)
            _categories[index] = category;
        return await Task.FromResult(category);
    }

    public async Task<List<ProductCategory>> GetAllAsync()
    {
        return _categories;
    }

    public  async Task<bool> DeleteAsync(int id)
    {
        _categories.RemoveAll(x => x.id == id);
        return true;
    }

    public async Task<ProductCategory> GetById(int id)
    {
        var category = _categories.FirstOrDefault(x => x.id == id);
        return await Task.FromResult(category); 
    }
}