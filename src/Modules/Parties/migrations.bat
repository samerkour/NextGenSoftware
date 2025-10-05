
IF "%1"=="init-context" dotnet ef migrations add InitialPartiesMigration -o \NextGen.Modules.Party\Shared\Data\Migrations\Party --project .\NextGen.Modules.Parties\NextGen.Modules.Parties.csproj -c PartiesDbContext --verbose & goto exit
IF "%1"=="update-context" dotnet ef database update -c PartiesDbContext --verbose --project .\NextGen.Modules.Parties\NextGen.Modules.Parties.csproj & goto exit

:exit
