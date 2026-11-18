public class Person
{
    public string Name { get; set; }
    public string Email { get; set; }
    public string Role { get; set; }

    public Person(string name, string email, string role)
    {
        Name = name;
        Email = email;
        Role = role;
    }

    public override string ToString() => $"{Name}, {Email}, {Role}";
}
