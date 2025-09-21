#### Migration Scripts

```bash
dotnet ef migrations add InitialPartiesMigration -o Shared/Data/Migrations/Party -c PartiesDbContext
dotnet ef database update -c PartiesDbContext
```
```using PMC
Add-Migration -Name InitialPartiesMigration -o Shared/Data/Migrations/Party -c PartiesDbContext
Update-Database -context PartiesDbContext
Remove-Migration -context PartiesDbContext
```
