using Crime.Models;
using Crime.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;



namespace Crime.Services
{
    public class UsersService : IUsersService
    {
        private readonly IUsersRepo _userRepository;

        public UsersService(IUsersRepo userRepository)
        {
            _userRepository = userRepository;
        }

        // Get all users
        public async Task<Users> GetByIdAsync(int id)
        {
            return await _userRepository.GetByIdAsync(id);
        }

        // Create a new user
        public async Task<Users> CreateAsync(Users user)
        {
            await _userRepository.AddAsync(user);
            await _userRepository.SaveChangesAsync();
            return user;
        }

        // Update an existing user
        public async Task UpdateAsync(Users user)
        {
            await _userRepository.UpdateAsync(user);
            await _userRepository.SaveChangesAsync();
        }

        // Delete a user by ID
        public async Task DeleteAsync(int id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user != null)
            {
                await _userRepository.DeleteAsync(user);
                await _userRepository.SaveChangesAsync();
            }
        }

        // Get user by email
        public async Task<Users> GetByEmailAsync(string email)
        {
            return await _userRepository.GetByEmailAsync(email);
        }

        // Get user by username
        public async Task<Users> GetByUsernameAsync(string username)
        {
            return await _userRepository.GetByUsernameAsync(username);
        }
    }
}

