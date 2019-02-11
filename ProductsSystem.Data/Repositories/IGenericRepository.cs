using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductsSystem.Data.Repositories
{
    public interface IGenericRepository<T> where T : class
    {
        IEnumerable<T> GetAll();

        Task<IEnumerable<T>> GetAllAsync();

        Task<T> GetByIdAsync(int id);

        Task Add(T entity);

        Task UpdateAsync(int id, T entity);

        Task DeleteByIdAsync(int id);

        void DeleteById(int id);
    }
}
