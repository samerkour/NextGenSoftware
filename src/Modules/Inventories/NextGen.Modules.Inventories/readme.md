#### Migration Scripts

```bash
dotnet ef migrations add InitialInventoryMigration -o Shared/Data/Migrations/Inventories -c InventoryDbContext
dotnet ef database update -c InventoryDbContext
```
```using PMC
Add-Migration -Name InitialInventoryMigration -o Shared/Data/Migrations/Inventories -c InventoryDbContext
Update-Database -context InventoryDbContext
Remove-Migration -context InventoryDbContext
```
