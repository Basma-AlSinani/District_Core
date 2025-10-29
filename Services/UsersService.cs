﻿using Crime.DTOs;
using Crime.Models;
using Crime.Repositories;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;


namespace Crime.Services
{
    public class UsersService : IUsersService
    {
        private readonly IUsersRepo _userRepository;

        public UsersService(IUsersRepo userRepository)
        {
            _userRepository = userRepository;
        }


        // Create a new user
        public async Task<Users> CreateAsync(Users user)
        {
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

            await _userRepository.UpdateAsync(user);
            await _userRepository.SaveChangesAsync();
        }

        // Delete a user by ID
        public async Task DeleteAsync(int id)
        {
            var user = await _userRepository.GetByIdAsync(id);

            if (user == null)
                throw new Exception("User not found.");

            await _userRepository.DeleteAsync(user);
            await _userRepository.SaveChangesAsync();

        }

        public string HashPassword(string password)
        {
            using var sha256 = SHA256.Create();
            var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(bytes);
        }
    }
}

