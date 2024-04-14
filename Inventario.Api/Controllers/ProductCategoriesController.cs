using System.Data;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Inventario.Core.Entities;
using Inventario.Core.Http;
using Inventario.Api.Dto;
using Inventario.Api.Repositories.Interfecies;
using Inventario.Services.Interfaces;

namespace Inventario.Api.Controllers;


[ApiController]
[Route("api/[controller]")]
public class ProductCategoriesController : ControllerBase
{
    // private readonly IProductCategoryRepository _productCategoryRepository;
    private readonly IProductCategoryService _productCategoryService;
    
    public ProductCategoriesController(IProductCategoryService productCategoryService)
    {
        // _productCategoryRepository = productCategoryRepository;
        _productCategoryService = productCategoryService;
    }

    [HttpGet]
    public async Task<ActionResult<Response<List<ProductCategory>>>> GetAll()
    {
        var response = new Response<List<ProductCategoryDto>>
        {
            Data = await _productCategoryService.GetAllAsync()
        };
        return Ok(response);
        // var response = new Response<List<ProductCategoryDto>>();
        // var categories = await _productCategoryRepository.GetAllAsync();
        //
        // response.Data =
        //     categories.Select(c => new ProductCategoryDto(c)).ToList();
        //
        // return Ok(response);
    }

    [HttpPost]
    public async Task<ActionResult<Response<ProductCategory>>> Post([FromBody] ProductCategoryDto categoryDto)
    {
        var response = new Response<ProductCategoryDto>()
        {
            Data = await _productCategoryService.SaveAsycn(categoryDto)

        };
        return Created($"/api/[controller]/{response.Data.id}", response);
    }

    [HttpGet]
    [Route("{id:int}")]
    public async Task<ActionResult<Response<ProductCategoryDto>>> GetById(int id)
    {
        
        var response = new Response<ProductCategoryDto>();
        // var category = await _productCategoryRepository.GetById(id);

        if (!await _productCategoryService.ProductCategoryExist(id))
        {
            response.Errors.Add("Category not found");
            return NotFound(response);
        }


        response.Data = await _productCategoryService.GetById(id); 
        return Ok(response);
    }

    [HttpPut]
    public async Task<ActionResult<Response<ProductCategoryDto>>> Update([FromBody] ProductCategoryDto categoryDto)
    {
        var response = new Response<ProductCategoryDto>();
        if (!await _productCategoryService.ProductCategoryExist(categoryDto.id))
        {
            response.Errors.Add("Product Category not found");
            return NotFound(response);
        }

        response.Data = await _productCategoryService.UpdateAsync(categoryDto);
        return Ok(response);

    }
    

    [HttpDelete]
    [Route("{id:int}")]
    public async Task<ActionResult<Response<bool>>> Delete(int id)
    {
        var response = new Response<bool>();
        // var category = await _productCategoryRepository.GetById(id);

        if (!await _productCategoryService.DeleteAsync(id))
        {
            response.Errors.Add("id not found");
            return NotFound(response);
        }
        return Ok(response);
    }
}