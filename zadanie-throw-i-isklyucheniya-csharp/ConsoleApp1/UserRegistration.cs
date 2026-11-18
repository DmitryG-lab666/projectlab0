using System;

public static class UserRegistration
{
    public static void RegisterUser(string username, int age)
    {
        if (username == null || username.Length < 2)
            throw new ArgumentException("Имя пользователя должно содержать не менее 2 символов");

        if (age < 18 || age > 120)
            throw new ArgumentOutOfRangeException(nameof(age), "Возраст должен быть в диапазоне от 18 до 120 лет");

        Console.WriteLine($"Пользователь {username} ({age}) успешно зарегистрирован");
    }
}
