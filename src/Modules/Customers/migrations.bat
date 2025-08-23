
IF "%1"=="init-context" dotnet ef migrations add InitialCustomersMigration -o \NextGen.Modules.Customer\Shared\Data\Migrations\Customer --project .\NextGen.Modules.Customers\NextGen.Modules.Customers.csproj -c CustomersDbContext --verbose & goto exit
IF "%1"=="update-context" dotnet ef database update -c CustomersDbContext --verbose --project .\NextGen.Modules.Customers\NextGen.Modules.Customers.csproj & goto exit

:exit
