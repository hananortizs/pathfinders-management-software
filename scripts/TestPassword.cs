using System;
using BCrypt.Net;

class Program
{
    static void Main()
    {
        string password = "Hanan123!@";
        string hash = BCrypt.HashPassword(password);
        
        Console.WriteLine($"Senha: {password}");
        Console.WriteLine($"Hash: {hash}");
        
        // Testar se o hash funciona
        bool isValid = BCrypt.Verify(password, hash);
        Console.WriteLine($"Hash válido: {isValid}");
    }
}
