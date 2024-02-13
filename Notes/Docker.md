# Staticaly

## Starting SQL Server on a docker container

```bash
docker run -e "ACCEPT_EULA=Y" -e "MSSQL_SA_PASSWORD=$sa_password" -e "MSSQL_PID=Evaluation" -p 1433:1433 -v sqlvolume:/var/opt/mssql --rm --name mssql --hostname sqlpreview -d mcr.microsoft.com/mssql/server:2022-preview-ubuntu-22.04
```

## Setting the conn string in secret Manager

```bash
sa_password="Passw0rd1"

dotnet ef migrations add InitialCreate --output-dir Models\Migrations

dotnet user-secrets set "ConnectionStrings:StaticalyContext" "Server=localhost; DataBase=Staticaly; User Id=sa; Password=$sa_password; TrustServerCertificate=True;"

dotnet ef database update --connection "Server=localhost; DataBase=Staticaly; User Id=sa; Password=$sa_password; TrustServerCertificate=True;"

"StaticalyContext" : "Server=localhost; Database=Staticaly; User Id=sa; Password=PASSWORD_GOES_HERE; TrustServerCertificate=True"

```

### usuarios

| clave | rol        |
| ----- | ---------- |
| 1     | Admin      |
| 2     | Estudiante |
| 3     | Docente    |
