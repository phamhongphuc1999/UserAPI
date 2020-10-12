install:
	dotnet add ./UserAPI package MongoDB.Bson --version 2.11.1
	dotnet add ./UserAPI package MongoDB.Driver --version 2.11.1
	dotnet add ./UserAPI package Newtonsoft.Json --version 12.0.3
	dotnet add ./UserAPI package System.IdentityModel.Tokens.Jwt --version 6.7.1
	dotnet add ./UserAPI package Swashbuckle.AspNetCore --version 5.5.1
	dotnet add ./UserAPI package Microsoft.EntityFrameworkCore --version 3.1.7
	dotnet add ./UserAPI package Microsoft.EntityFrameworkCore.SqlServer --version 3.1.7
	dotnet add ./UserAPI package NEST --version 7.9.0

run:
	dotnet run --launch-profile UserAPI --project ./UserAPI

runuser:
	dotnet run --launch-profile UserAPI --project ./UserAPI

runiss:
	dotnet run --launch-profile IISExpress --project ./UserAPI

rundocker:
	dotnet run --launch-profile Docker --project ./UserAPI
