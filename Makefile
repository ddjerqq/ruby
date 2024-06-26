testa:
	echo hello $(name)

add-migration:
	dotnet ef migrations add --project src\Infrastructure\Infrastructure.csproj --startup-project src\Presentation\Presentation.csproj --context Infrastructure.Persistence.AppDbContext --configuration Debug --verbose $(name) --output-dir Persistence\Migrations