using Feedback_System.Abstractions;

using Feedback_System.Models;
using Microsoft.EntityFrameworkCore;

namespace Feedback_System.Concretes
{
    public class WriteRepository<T> : IWriteRepository<T> where T : BaseEntities
    {
        private readonly AppDbContext _context;

        public WriteRepository(AppDbContext context)
        {
            _context = context;
        }

        // Provide access to the DbSet for the entity type T
        public DbSet<T> Table => _context.Set<T>();

        // Method to create (add) a new entity
        public void Create(T entity)
        {
            Table.Add(entity); // Adds the entity to the context
            _context.SaveChanges(); // Persists changes to the database
        }

        // Method to delete an entity
        public void Delete(T entity)
        {
            Table.Remove(entity); // Marks the entity for deletion
            _context.SaveChanges(); // Persists changes to the database
        }

        // Method to update an existing entity
        public void Update(T entity)
        {
            Table.Update(entity); // Marks the entity as modified
            _context.SaveChanges(); // Persists changes to the database
        }
    }
}
