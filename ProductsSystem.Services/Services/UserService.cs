using ProductsSystem.Data.Core;
using ProductsSystem.Models.EntityModels;
using ProductsSystem.Services.Services.Interfaces;
using ProductsSystem.ViewModels.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductsSystem.Services
{
    public class UserService : IUserService
    {

        private IUnitOfWork _unitOfWork;
        public UserService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public User Authenticate(string username, string password)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
                return null;

            var allUsers = _unitOfWork.UserRepository.GetAll();
            var currentUser = allUsers.SingleOrDefault(x => x.Username == username);

            // check if username exists
            if (currentUser == null)
                return null;

            // check if password is correct
            if (!VerifyPasswordHash(password, currentUser.PasswordHash, currentUser.PasswordSalt))
                return null;

            // authentication successful
            return currentUser;
        }

        private static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            if (password == null) throw new ArgumentNullException("password");
            if (string.IsNullOrWhiteSpace(password)) throw new ArgumentException("Value cannot be empty or whitespace only string.", "password");

            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        private static bool VerifyPasswordHash(string password, byte[] storedHash, byte[] storedSalt)
        {
            if (password == null) throw new ArgumentNullException("password");
            if (string.IsNullOrWhiteSpace(password)) throw new ArgumentException("Value cannot be empty or whitespace only string.", "password");
            if (storedHash.Length != 64) throw new ArgumentException("Invalid length of password hash (64 bytes expected).", "passwordHash");
            if (storedSalt.Length != 128) throw new ArgumentException("Invalid length of password salt (128 bytes expected).", "passwordHash");

            using (var hmac = new System.Security.Cryptography.HMACSHA512(storedSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                for (int i = 0; i < computedHash.Length; i++)
                {
                    if (computedHash[i] != storedHash[i]) return false;
                }
            }

            return true;
        }

        public IEnumerable<User> GetAll()
        {
            return _unitOfWork.UserRepository.GetAll();
        }

        public async Task<User> GetById(int id)
        {
            var user = await _unitOfWork.UserRepository.GetByIdAsync(id);
            return user;
        }

        public User Create(UserViewModel userviewModel)
        {
            if (_unitOfWork.UserRepository.GetAll().Any(x => x.Username == userviewModel.Username))
                throw new Exception("Username \"" + userviewModel.Username + "\" is already taken");

            byte[] passwordHash, passwordSalt;
            CreatePasswordHash(userviewModel.Password, out passwordHash, out passwordSalt);
            var user = new User()
            {
                FirstName = userviewModel.FirstName,
                LastName = userviewModel.LastName,
                Username = userviewModel.Username
            };
            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;

            _unitOfWork.UserRepository.Add(user);
            _unitOfWork.SaveChanges();

            return user;
        }

        public void Update(User user, string password = null)
        {
            throw new NotImplementedException();
        }

        public void Delete(int id)
        {
            throw new NotImplementedException();
        }
    }
}
