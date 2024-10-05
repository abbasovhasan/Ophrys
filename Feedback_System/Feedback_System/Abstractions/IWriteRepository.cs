using Feedback_System.Models;

namespace Feedback_System.Abstractions;

public interface IWriteRepository<T> : IRepository<T> where T : BaseEntities
{
    void Create(T entity);
    void Update(T entity);
    void Delete(T entity);
}
