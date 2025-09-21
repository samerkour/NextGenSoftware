
IF "%1"=="init-context" dotnet ef migrations add InitialInventoryMigration -o \NextGen.Modules.Inventories\Shared\Data\Migrations\Inventories --project .\NextGen.Modules.Inventories\NextGen.Modules.Inventories.csproj -c InventoryDbContext --verbose & goto exit
IF "%1"=="update-context" dotnet ef database update -c InventoryDbContext --verbose --project .\NextGen.Modules.Inventories\NextGen.Modules.Inventories.csproj & goto exit

:exit
