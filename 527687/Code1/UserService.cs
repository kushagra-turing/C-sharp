// UserService.cs
using System;
using UserApi.Models; // Make sure to include the Models namespace

namespace UserApi.Services
{
    public class UserService : IUserService
    {
        // In-memory data for demonstration purposes
        private static readonly List<User> _users = new()
        {
            new User { Id = "1", Name = "Alice", Email = "alice@example.com" },
            new User { Id = "2", Name = "Bob", Email = "bob@example.com" }
        };

        private static readonly UserProfile _userProfile = new()
        {
            FirstName = "Default",
            LastName = "User",
            City = "Anytown"
        };

        public User? GetUserById(string id)
        {
            return _users.FirstOrDefault(u => u.Id == id);
        }

        public UserProfile? GetUserProfile()
        {
            return _userProfile;
        }
    }
}