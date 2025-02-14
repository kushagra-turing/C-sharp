// IUserService.cs
namespace UserApi.Services
{
    public interface IUserService
    {
        User? GetUserById(int id);
        UserProfile? GetUserProfile();
    }
}