// UserProfile.cs
using System.Collections.Generic;

namespace UserApi
{
    public class UserProfile
    {
        public string? Bio { get; set; }
        public List<string>? Interests { get; set; }
    }
}