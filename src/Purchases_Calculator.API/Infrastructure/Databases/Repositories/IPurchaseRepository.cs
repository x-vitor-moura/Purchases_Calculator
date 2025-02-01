using Purchases_Calculator.API.Domain;

namespace Purchases_Calculator.API.Infrastructure.Databases.Repositories;

public interface IPurchaseRepository
{
    Task CreateAsync(Purchase entity);
    Task<Purchase?> GetByIdAsync(int id);
    Task<IEnumerable<Purchase>> GetAllAsync();
}