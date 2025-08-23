
IF "%1"=="init-context" dotnet ef migrations add InitialCatalogMigration -o \NextGen.Modules.Catalogs\Shared\Data\Migrations\Catalogs --project .\NextGen.Modules.Catalogs\NextGen.Modules.Catalogs.csproj -c CatalogDbContext --verbose & goto exit
IF "%1"=="update-context" dotnet ef database update -c CatalogDbContext --verbose --project .\NextGen.Modules.Catalogs\NextGen.Modules.Catalogs.csproj & goto exit

:exit
