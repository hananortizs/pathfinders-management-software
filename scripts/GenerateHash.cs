using System;
using BCrypt.Net;

class Program
{
    static void Main()
    {
        string password = "Hanan123!@";
        string hash = BCrypt.HashPassword(password, BCrypt.GenerateSalt(11));
        
        Console.WriteLine($"Senha: {password}");
        Console.WriteLine($"Hash: {hash}");
    }
}
