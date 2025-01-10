# Blazor Basic CRUD APP

### CREATE DATABASE

```sql
CREATE DATABASE [db-BlazorCrudApp];
GO

CREATE TABLE [Personal] (
    [Id] INT IDENTITY NOT NULL,
    [FirstName] NVARCHAR(255) NOT NULL,
    [LastName] NVARCHAR(255),
    [DateOfBirth] DATETIME NOT NULL,
    [DateCreated] DATETIME NOT NULL CONSTRAINT DF_Personal_DateCreated DEFAULT GETUTCDATE(),
    [DateModified] DATETIME,
    [DateDeleted] DATETIME,
    CONSTRAINT [PK_Personal] PRIMARY KEY ([Id])
)
GO
```

### Initialize migrations for identity

```shell
dotnet ef migrations add InitialMigration -o Migrations --project BlazorCrudApp.Server.csproj --context ApplicationDbContext
```

### Update DB for migration

```shell
dotnet ef database update --project BlazorCrudApp.Server.csproj --context ApplicationDbContext
```

### DB Scaffolding

```shell
dotnet ef dbcontext scaffold "Data Source=localhost;Initial Catalog=db-BlazorCrudApp;User Id=sa;Password=Password;MultipleActiveResultSets=False;Application Name=BlazorCrudApp-App;TrustServerCertificate=True" Microsoft.EntityFrameworkCore.SqlServer -o Data --no-onconfiguring --context ApplicationDbContext --project BlazorCrudApp.Server/BlazorCrudApp.Server.csproj -f
```
