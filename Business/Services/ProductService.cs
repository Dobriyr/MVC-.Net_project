using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Business.Interfaces;
using Business.Models;
using Data.Interfaces;
using Business.Validation;
using Data.Entities;
using System.Linq;

namespace Business.Services
{
    public class ProductService : IProductService
    {
        public readonly IUnitOfWork _unitOfWork;
        public readonly IMapper _mapper;

        public ProductService(IUnitOfWork uof, IMapper mapper)
        {
            this._unitOfWork = uof;
            this._mapper = mapper;
        }


        public async Task AddAsync(ProductModel model)
        {
            model.IsValid();

            var temp = _mapper.Map<ProductModel, Product>(model);
            await _unitOfWork.ProductRepository.AddAsync(temp);
            await _unitOfWork.SaveAsync();

        }

        public async Task AddCategoryAsync(ProductCategoryModel categoryModel)
        {
            if (categoryModel is null) {
                throw new MarketException("CategoryModel value is null");
            }
            if (categoryModel.CategoryName == string.Empty) 
            {
                throw new MarketException("CategoryModel value CategoryName is empty");
            }
          

            var temp = _mapper.Map<ProductCategoryModel, ProductCategory>(categoryModel);
            await _unitOfWork.ProductCategoryRepository.AddAsync(temp);
            await _unitOfWork.SaveAsync();
        }

        public async Task DeleteAsync(int modelId)
        {
            await _unitOfWork.ProductRepository.DeleteByIdAsync(modelId);
            await _unitOfWork.SaveAsync();
        }

        public async Task<IEnumerable<ProductModel>> GetAllAsync()
        {
            var products = await _unitOfWork.ProductRepository.GetAllWithDetailsAsync();
            List<ProductModel> pml = new List<ProductModel>();
            foreach (Product p in products) {
                pml.Add(_mapper.Map<Product, ProductModel>(p));
            }
            return pml;
        }

        public async Task<IEnumerable<ProductCategoryModel>> GetAllProductCategoriesAsync()
        {
            var productCategorys = await _unitOfWork.ProductCategoryRepository.GetAllAsync();
            List<ProductCategoryModel> pcml =  new List<ProductCategoryModel>();
            foreach (ProductCategory pc in productCategorys) {
            pcml.Add(_mapper.Map<ProductCategory,ProductCategoryModel>(pc));
            }
            return pcml;
        }

        public async Task<IEnumerable<ProductModel>> GetByFilterAsync(FilterSearchModel filterSearch)
        {
            var products = await _unitOfWork.ProductRepository.GetAllWithDetailsAsync();
            var filtred = products
                .Where(p => p.Price >= (filterSearch.MinPrice ?? decimal.MinValue) && p.Price <= (filterSearch.MaxPrice ?? decimal.MaxValue));
            if (filterSearch.CategoryId != null) { 
                filtred = filtred.Where(p=>p.Category.Id == filterSearch.CategoryId);
                 }

            List<ProductModel> pml = new List<ProductModel>(); 
            foreach(Product p in filtred)
            {
                pml.Add(_mapper.Map<Product, ProductModel>(p));
            }
            return pml;
        }

        public async Task<ProductModel> GetByIdAsync(int id)
        {
            var product = await _unitOfWork.ProductRepository.GetByIdWithDetailsAsync(id);
            var productModel = _mapper.Map<Product, ProductModel>(product);
            productModel.IsValid();
            return productModel;
        }

        public async Task RemoveCategoryAsync(int categoryId)
        {
            await _unitOfWork.ProductCategoryRepository.DeleteByIdAsync(categoryId);
            await _unitOfWork.SaveAsync();
        }

        public async Task UpdateAsync(ProductModel model)
        {
            var product = _mapper.Map<ProductModel, Product>(model);
            if (product.ProductName == String.Empty)
                throw new MarketException("Empty ProductName value");
            _unitOfWork.ProductRepository.Update(product);
            await _unitOfWork.SaveAsync();
        }

        public async Task UpdateCategoryAsync(ProductCategoryModel categoryModel)
        {
            var category = _mapper.Map<ProductCategoryModel, ProductCategory>(categoryModel);
            if (category.CategoryName == String.Empty)
                throw new MarketException("Empty CategoryName value");
            _unitOfWork.ProductCategoryRepository.Update(category);
            await _unitOfWork.SaveAsync();
        }
    }
}
