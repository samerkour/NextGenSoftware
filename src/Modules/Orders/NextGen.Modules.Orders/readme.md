#### Migration Scripts

```bash
dotnet ef migrations add InitialOrdersMigration -o Shared/Data/Migrations/Order -c OrdersDbContext
dotnet ef database update -c OrdersDbContext
```
```using PMC
Add-Migration -Name InitialOrdersMigration -o Shared/Data/Migrations/Order -c OrdersDbContext
Update-Database -context OrdersDbContext
Remove-Migration -context OrdersDbContext
```
