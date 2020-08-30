install:
	dotnet add ./Model package MongoDB.Bson --version 2.11.1
	dotnet add ./Model package MongoDB.Driver --version 2.11.1
	dotnet add ./UserAPI package Newtonsoft.Json --version 12.0.3
	dotnet add ./UserAPI package System.IdentityModel.Tokens.Jwt --version 6.7.1
	dotnet add ./UserAPI package Swashbuckle.AspNetCore --version 5.5.0

run:
	dotnet run -p ./UserAPI

runuser:
	dotnet run -p ./UserAPI --launch-profile "UserAPI"

rundocker:
	dotnet run -p ./UserAPI --launch-profile "Docker"
