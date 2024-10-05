using Feedback_System.Abstractions;
using Feedback_System.Models;
using Microsoft.EntityFrameworkCore;

namespace Feedback_System.Concretes
{
    public class ReadRepository<T> : IReadRepository<T> where T : BaseEntities
    {
        private readonly AppDbContext _context;

        public ReadRepository(AppDbContext context)
        {
            _context = context;
        }

        // Provide access to the DbSet for the entity type T
        public DbSet<T> Table => _context.Set<T>();

        // Method to get all records from the table
        public List<T> GetAll()
        {
            return Table.ToList();
        }

        // Method to get a single record by ID
        public T GetById(int id)
        {
            T? entity = Table.Find(id);
            if (entity == null)
            {
                throw new Exception($"Entity not found with this id {id}");
            }

            return entity;
        }
    }
}
