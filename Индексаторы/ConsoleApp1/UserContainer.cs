using System;

public class UserContainer
{
    public string Name { get; set; }
    public string Email { get; set; }
    public string Role { get; set; }

    public UserContainer(string name, string email, string role)
    {
        Name = name;
        Email = email;
        Role = role;
    }

    public string this[string field]
    {
        get
        {
            switch (field.ToLower())
            {
                case "name": return Name;
                case "email": return Email;
                case "role": return Role;
                default: throw new ArgumentException($"Неизвестное поле: {field}", nameof(field));
            }
        }
        set
        {
            switch (field.ToLower())
            {
                case "name": Name = value; break;
                case "email": Email = value; break;
                case "role": Role = value; break;
                default: throw new ArgumentException($"Неизвестное поле: {field}", nameof(field));
            }
        }
    }
}
