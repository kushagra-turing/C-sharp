using System.Collections.Generic;

public class UserDetails
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public int Age { get; set; }
    public List<string> Hobbies { get; set; } = new List<string>();
}