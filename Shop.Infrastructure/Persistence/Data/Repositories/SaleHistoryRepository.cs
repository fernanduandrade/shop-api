using Microsoft.EntityFrameworkCore;
using Shop.Domain.SalesHistory;

namespace Shop.Infrastructure.Persistence.Data.Repositories;

public class SaleHistoryRepository : ISaleHistoryRepository
{
    private readonly AppDbContext _context;

    public SaleHistoryRepository(AppDbContext context)
        => (_context) = (context);

    public IQueryable<SaleHistory> GetAllPaginated()
    {
        var result = _context.SalesHistory
            .AsNoTracking()
            .Include(x => x.Product)
            .OrderByDescending(x => x.Created);

        return result;
    }

    public async Task<SaleHistory> FindByIdAsync(Guid id)
    {
        var saleHistory = await _context.SalesHistory
            .Include(x => x.Product)
            .FirstOrDefaultAsync(x => x.Id == id);
        return saleHistory;
    }

    public void Add(SaleHistory saleHistory)
        => _context.SalesHistory.Add(saleHistory);

    public void AddMany(List<SaleHistory> saleHistories)
    {
        _context.SalesHistory.AddRange(saleHistories);
    }

    public void Update(SaleHistory saleHistory)
        => _context.SalesHistory.Update(saleHistory);

    public async Task Remove(Guid id)
    {
        var saleHistory = await _context.SalesHistory.FirstOrDefaultAsync(x => x.Id == id);
        _context.SalesHistory.Remove(saleHistory);
    }

    public decimal TodaySales()
    {
        DateTime compareDate = DateTime.UtcNow;
        var todaySales = _context.SalesHistory
            .Where(x => x.Date.Date == compareDate.Date)
            .ToList()
            .Sum(x => x.TotalPrice);
        
        return todaySales;
    }

    public decimal MonthSales()
    {
        DateTime compareDate = DateTime.UtcNow;
        var todaySales = _context.SalesHistory
            .Where(x => x.Date.Month == compareDate.Month)
            .ToList()
            .Sum(x => x.TotalPrice);
        
        return todaySales;
    }

    public virtual void SetEntityStateModified(SaleHistory entity)
    {
        _context.SalesHistory.Entry(entity).State = EntityState.Modified;
    }

    public void DeleteBulk(List<Guid> ids)
    {
        var sales = _context.SalesHistory.Where(x => ids.Contains(x.Id)).ToList();
        _context.SalesHistory.RemoveRange(sales);
    }
}