using Feedback_System.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace Feedback_System.Abstractions;

public interface IRepository<T> where T : BaseEntities
{
    DbSet<T> Table { get; }

}
