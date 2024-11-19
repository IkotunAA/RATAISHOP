using Microsoft.EntityFrameworkCore;
using RATAISHOP.Context;
using RATAISHOP.Repositories.Interfaces;

namespace RATAISHOP.Repositories.Implementations
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly RataiDbContext _context;
        public UnitOfWork(RataiDbContext context)
        {
            _context = context;
        }
        public int SaveChanges()
        {
            return _context.SaveChanges();
        }
    }
}

