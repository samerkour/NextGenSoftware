#### Migration Scripts

```bash
dotnet ef migrations add InitialSalesMigration -o Shared/Data/Migrations/Sale -c SalesDbContext
dotnet ef database update -c SalesDbContext
```
```using PMC
Add-Migration -Name InitialSalesMigration -o Shared/Data/Migrations/Sale -c SalesDbContext
Update-Database -context SalesDbContext
Remove-Migration -context SalesDbContext
```
