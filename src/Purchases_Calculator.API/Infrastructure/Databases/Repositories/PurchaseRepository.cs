using Microsoft.EntityFrameworkCore;
using Purchases_Calculator.API.Domain;

namespace Purchases_Calculator.API.Infrastructure.Databases.Repositories;

public class PurchaseRepository : IPurchaseRepository
{
    private readonly AppDbContext _appDbContext;

    public PurchaseRepository(AppDbContext appDbContext)
    {
        _appDbContext = appDbContext;
    }
    public async Task CreateAsync(Purchase entity)
    {
        await _appDbContext.Set<Purchase>().AddAsync(entity);
        await _appDbContext.SaveChangesAsync();
    }

    public async Task<IEnumerable<Purchase>> GetAllAsync()
    {
        return await _appDbContext.Set<Purchase>().ToListAsync();
    }

    public async Task<Purchase?> GetByIdAsync(int id)
    {
        return await _appDbContext.Set<Purchase>().FirstOrDefaultAsync(e => e.Id == id);
    }
}