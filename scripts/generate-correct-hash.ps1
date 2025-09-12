# Gerar hash BCrypt correto para "Hanan123!@"
Add-Type -AssemblyName System.Web

# Usar BCrypt.Net para gerar hash
$password = "Hanan123!@"
$salt = [BCrypt.Net.BCrypt]::GenerateSalt(11)
$hash = [BCrypt.Net.BCrypt]::HashPassword($password, $salt)

Write-Host "Senha: $password"
Write-Host "Hash: $hash"
