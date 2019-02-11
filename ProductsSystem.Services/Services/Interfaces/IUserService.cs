using ProductsSystem.Models.EntityModels;
using ProductsSystem.ViewModels.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProductsSystem.Services.Services.Interfaces
{
    public interface IUserService
    {
        User Authenticate(string username, string password);
        IEnumerable<User> GetAll();
        Task<User> GetById(int id);
        User Create(UserViewModel userviewModel);
        void Update(User user, string password = null);
        void Delete(int id);
    }
}
