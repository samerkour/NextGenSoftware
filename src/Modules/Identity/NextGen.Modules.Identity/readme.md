#### Migration Scripts

```bash
dotnet ef migrations add InitialIdentityServerMigration -o Shared/Data/Migrations/Identity -c IdentityContext
dotnet ef database update -c IdentityContext
```

```using PMC
Add-Migration -Name InitialIdentityServerMigration -o Shared/Data/Migrations/Identity -c IdentityContext
Update-Database -context IdentityContext
Remove-Migration -context IdentityContext
```
