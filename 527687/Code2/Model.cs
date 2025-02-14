namespace UserApi.Models
{
    public class User
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
    }

    public class Profile
    {
        public string? DisplayName { get; set; }
        public string? Bio { get; set; }
    }
}