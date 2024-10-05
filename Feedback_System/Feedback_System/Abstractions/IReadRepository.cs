using Feedback_System.Models;

namespace Feedback_System.Abstractions;

public interface IReadRepository<T> : IRepository<T> where T : BaseEntities
{
    T GetById(int id);
    List<T> GetAll();
}
