using BCrypt.Net;
using Microsoft.EntityFrameworkCore;
using Practica.Application.Interfaces;
using Practica.Domain.Entities;
using Practica.Infrastructure.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Practica.Application.Services
{
    public class UserService : IUserService
    {
        private readonly AppDBContext _context;

        public UserService(AppDBContext context)
        {
            _context = context;
        }


        public async Task AddUserAsync(User user)
        {
            var isEmailRegistered = await _context.Users.SingleOrDefaultAsync(u => u.Email == user.Email);
            if (isEmailRegistered != null) throw new InvalidOperationException("Email already exists.");

            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(user.PasswordHash);

            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<User>> GetUsersAsync()
        {
            var users = await _context.Users.ToListAsync();
            return users;
        }

        public async Task RemoveUserAsync(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                throw new KeyNotFoundException("User not found.");
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateUserByIdAsync(int id, User user)
        {
           var findUser = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
            if (findUser == null) {
                throw new KeyNotFoundException($"User with ID {id} not found.");
            }

            findUser.Name = user.Name;
            findUser.Email = user.Email;

            await _context.SaveChangesAsync();


        }
    }
}
