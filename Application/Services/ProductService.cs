using AutoMapper;
using InventoryAPI.Application.DTO;
using InventoryAPI.Domain.Repositories;
using MassTransit;

namespace InventoryAPI.Application.Services
{
    public interface IProductService
    {
        Task<IEnumerable<Product>> GetProductsAsync();
        Task<Product?> GetProductByIdAsync(Guid id);
        Task<Product> CreateProductAsync(CreateProductDTO createProductDTO);
        Task<bool> UpdateProductAsync(Guid id, UpdateProductDTO updateProductDTO);
        Task<bool> DeleteProductAsync(Guid id);
    }

    public class ProductService : IProductService
    {
        private readonly IProductRepository _repository;
        private readonly IBus _bus;
        private readonly IMapper _mapper;

        public ProductService(IProductRepository repository, IBus bus, IMapper mapper)
        {
            _repository = repository;
            _bus = bus;
            _mapper = mapper;
        }

        public async Task<IEnumerable<Product>> GetProductsAsync()
        {
            return await _repository.GetProductsAsync();
        }

        public async Task<Product?> GetProductByIdAsync(Guid id)
        {
            return await _repository.GetProductByIdAsync(id);
        }

        public async Task<Product> CreateProductAsync(CreateProductDTO createProductDTO)
        {
            var product = _mapper.Map<Product>(createProductDTO);
            await _repository.AddProductAsync(product);
            await _repository.SaveChangesAsync();

            await SendMessage("Create", product);

            return product;
        }

        public async Task<bool> UpdateProductAsync(Guid id, UpdateProductDTO updateProductDTO)
        {
            var product = await _repository.GetProductByIdAsync(id);
            if (product == null)
                return false;

            _mapper.Map(updateProductDTO, product);
            await _repository.UpdateProductAsync(product);
            await _repository.SaveChangesAsync();

            await SendMessage("Update", product);

            return true;
        }

        public async Task<bool> DeleteProductAsync(Guid id)
        {
            var product = await _repository.GetProductByIdAsync(id);
            if (product == null)
                return false;

            await _repository.DeleteProductAsync(product);
            await _repository.SaveChangesAsync();

            await SendMessage("Delete", product);

            return true;
        }

        private async Task SendMessage(string action, Product product)
        {
            var message = new ProductMessage
            {
                Action = action,
                Product = product
            };

            await _bus.Publish(message);
        }
    }
}