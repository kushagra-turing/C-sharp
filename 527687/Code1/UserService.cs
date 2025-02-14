// UserService.cs
using System.Collections.Generic;

namespace UserApi.Services
{
    public class UserService : IUserService
    {
        // Simulate a data store (replace with a real database)
        private static readonly List<User> _users = new List<User>()
        {
            new User { Id = 1, Name = "Alice", Email = "alice@example.com" },
            new User { Id = 2, Name = "Bob", Email = "bob@example.com" }
        };

        private static readonly UserProfile _defaultProfile = new UserProfile {Bio = "Default Bio", Interests = new List<string>{"coding"}};

        public User? GetUserById(int id)
        {
            return _users.FirstOrDefault(u => u.Id == id);
        }

        public UserProfile? GetUserProfile()
        {
            return _defaultProfile;
        }
    }
}