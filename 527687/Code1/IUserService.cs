// IUserService.cs
namespace UserApi.Services
{
    public interface IUserService
    {
        User? GetUserById(string id);
        UserProfile? GetUserProfile();
    }
}