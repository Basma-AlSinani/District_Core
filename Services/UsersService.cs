using CrimeManagment.DTOs;
using CrimeManagment.Models;
using CrimeManagment.Repositories;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using AutoMapper;


namespace CrimeManagment.Services
{
    public class UsersService : IUsersService
    {
        private readonly IUsersRepo _userRepository;
        private readonly IMapper _mapper;

        public UsersService(IUsersRepo userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        // Create a new user
        public async Task<Users> CreateAsync(Users user)
        {
            var existingUser = await _userRepository.GetByEmailAsync(user.Email);
            if (existingUser != null)
                throw new Exception("A user with this email already exists.");

            // Validate Clearance Level Restrictions based on Role
            //if (user.Role == UserRole.Investigator && user.ClearanceLevel > ClearanceLevel.High)
            //{
            //    throw new Exception("Investigator cannot have a Clearance Level higher than High (2).");
            //}

            //if (user.Role == UserRole.Officer && user.ClearanceLevel != ClearanceLevel.Low)
            //{
            //    throw new Exception("Officer can only have Clearance Level Low (0).");
            //}
            await _userRepository.AddAsync(user);
            await _userRepository.SaveChangesAsync();
            return user;
        }

        // Update an existing user
        public async Task UpdateAsync(int id, UpdateUserDto dto)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null)
                throw new Exception("User not found.");

            if (!string.IsNullOrWhiteSpace(dto.Password))
                user.PasswordHash = HashPassword(dto.Password);

            if (dto.Role.HasValue)
                user.Role = dto.Role.Value;

            if (dto.ClearanceLevel.HasValue)
                user.ClearanceLevel = dto.ClearanceLevel.Value;

            if (!string.IsNullOrWhiteSpace(dto.Email))
                user.Email = dto.Email;

            // Restrict role clearance relationships during update
            if (user.Role == UserRole.Investigator && user.ClearanceLevel > ClearanceLevel.High)
            {
                throw new Exception("Investigator cannot have Clearance Level higher than High (2).");
            }

            if (user.Role == UserRole.Officer && user.ClearanceLevel != ClearanceLevel.Low)
            {
                throw new Exception("Officer can only have Clearance Level Low (0).");
            }

            await _userRepository.UpdateAsync(user);
            await _userRepository.SaveChangesAsync();
        }

        // Delete a user by ID
        public async Task<bool> DeleteAsync(int id)
        {
            var user = await _userRepository.GetByIdAsync(id);

            if (user == null)
                return false;

            await _userRepository.DeleteAsync(user);
            await _userRepository.SaveChangesAsync();
            return true;
        }

        public string HashPassword(string password)
        {
            using var sha256 = SHA256.Create();
            var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(bytes);
        }

        public async Task<bool> ExistsByUsername(string username)
        {
            var user = await _userRepository.GetByUsernameAsync(username);
            return user != null;
        }
        public async Task<Users?> AuthenticateAsync(string email, string password)
        {
            var user = await _userRepository.GetByEmailAsync(email);
            if (user == null) return null;

            using var sha256 = SHA256.Create();
            var hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            var hashedPassword = Convert.ToBase64String(hashBytes);

            return user.PasswordHash == hashedPassword ? user : null;
        }

        public async Task<Users?> GetByEmailAsync(string email)
        {
            return await _userRepository.GetByEmailAsync(email);
        }

        public async Task<IEnumerable<UserDTO>> GetAllUsersAsync()
        {
            var users = await _userRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<UserDTO>>(users);
        }

        public async Task<UserDTO?> GetUserByIdAsync(int id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            return _mapper.Map<UserDTO?>(user);
        }
    }
}

