#### Migration Scripts

```bash
dotnet ef migrations add InitialCatalogMigration -o Shared/Data/Migrations/Catalogs -c CatalogDbContext
dotnet ef database update -c CatalogDbContext
```
```using PMC
Add-Migration -Name InitialCatalogMigration -o Shared/Data/Migrations/Catalogs -c CatalogDbContext
Update-Database -context CatalogDbContext
Remove-Migration -context CatalogDbContext
```
