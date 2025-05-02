using ExpenseTracker.Models;
using Microsoft.EntityFrameworkCore;

namespace ExpenseTracker.Services;

public class ExpenseService : IExpenseService
{
    private readonly AppDbContext _context;

    public ExpenseService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Expense>> GetAllAsync()
        => await _context.Expenses.Include(e => e.Category).ToListAsync();

    public async Task<Expense?> GetByIdAsync(int id)
        => await _context.Expenses.Include(e => e.Category).FirstOrDefaultAsync(e => e.ExpenseId == id);

    public async Task<Expense> CreateAsync(Expense expense)
    {
        _context.Expenses.Add(expense);
        await _context.SaveChangesAsync();
        return expense;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var exp = await _context.Expenses.FindAsync(id);
        if (exp == null) return false;

        _context.Expenses.Remove(exp);
        await _context.SaveChangesAsync();
        return true;
    }
}
