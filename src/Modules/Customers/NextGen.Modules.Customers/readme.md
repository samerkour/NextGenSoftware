#### Migration Scripts

```bash
dotnet ef migrations add InitialCustomersMigration -o Shared/Data/Migrations/Customer -c CustomersDbContext
dotnet ef database update -c CustomersDbContext
```
```using PMC
Add-Migration -Name InitialCustomersMigration -o Shared/Data/Migrations/Customer -c CustomersDbContext
Update-Database -context CustomersDbContext
Remove-Migration -context CustomersDbContext
```
