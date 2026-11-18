public class UserEntity : Entity
{
    public string Email { get; set; }
    public string Role { get; set; }

    public UserEntity() : base(0, string.Empty)
    {
        Email = string.Empty;
        Role = string.Empty;
    }

    public UserEntity(int id, string name, string email, string role) : base(id, name)
    {
        Email = email;
        Role = role;
    }

    public override string Category => "Пользователи";

    public override string GetEntityDetails()
    {
        return $"Пользователь #{Id}: {Name}, Email: {Email}, Роль: {Role}";
    }
}
