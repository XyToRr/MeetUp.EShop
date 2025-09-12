using MeetUp.EShop.Core.Interfaces;
using MeetUp.EShop.Core.Models.User;
using DataAccess.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MeetUp.EShop.Business.Reposirories
{
    public class UserRepository : IUserRepository
    {
        private readonly EShopDbContext _context;

        public UserRepository(EShopDbContext context)
        {
            _context = context;
        }

        public User? Get(Guid guid)
        {
            return _context.Users
                .Include(u => u.Orders)
                .ThenInclude(o => o.Products)
                .FirstOrDefault(u => u.Id == guid);
        }

        public Guid? GetByName(string name)
        {
            var user = _context.Users.FirstOrDefault(u => u.Login == name);
            return user?.Id;
        }

        public IEnumerable<User> GetUsers()
        {
            return _context.Users
                .Include(u => u.Orders)
                .ThenInclude(o => o.Products)
                .ToList();
        }

        public async Task<Guid> Register(User user)
        {
            user.Id = Guid.NewGuid();
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return user.Id;
        }

        public async Task<bool> Update(User user)
        {
            var oldUser = _context.Users.FirstOrDefault(u => u.Id == user.Id);
            if (oldUser == null)
            {
                return false;
            }
            oldUser.FirstName = user.FirstName;
            oldUser.LastName = user.LastName;
            oldUser.Email = user.Email;
            if (!string.IsNullOrWhiteSpace(user.Password) && user.Password != oldUser.Password)
            {
                oldUser.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);
            }
            oldUser.Login = user.Login;
            _context.Users.Update(oldUser);
            return await _context.SaveChangesAsync() > 0;


        }

        public async Task<bool> UpdateTokens(User user)
        {
            var oldUser = _context.Users.FirstOrDefault(u => u.Id == user.Id);
            if (oldUser == null)
                return false;

            oldUser.RefreshToken = user.RefreshToken;
            oldUser.RefreshTokenExpire = user.RefreshTokenExpire;

            _context.Users.Update(oldUser);
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
