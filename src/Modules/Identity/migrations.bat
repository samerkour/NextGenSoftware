
IF "%1"=="init-context" dotnet ef migrations add InitialIdentityServerMigration -o \NextGen.Modules.Identity\Shared\Data\Migrations\Identity --project .\NextGen.Modules.Identity\NextGen.Modules.Identity.csproj -c IdentityContext --verbose & goto exit
IF "%1"=="update-context" dotnet ef database update -c IdentityContext --verbose --project .\NextGen.Modules.Identity\NextGen.Modules.Identity.csproj & goto exit
:exit
