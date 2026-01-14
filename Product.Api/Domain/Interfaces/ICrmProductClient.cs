using Product.Api.Application.Dtos;

namespace Product.Api.Domain.Interfaces
{
    public interface ICrmProductClient
    {
        Task<List<ProductDto>> GetAllAsync(CancellationToken ct = default);
        Task<ProductDto?> GetByIdAsync(Guid id, CancellationToken ct = default);
    }
}
