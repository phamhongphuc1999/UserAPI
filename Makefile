run:
	dotnet run -p ./UserAPI

install:
	dotnet add ./Model package MongoDB.Bson --version 2.11.1
	dotnet add ./Model package MongoDB.Driver --version 2.11.1
	dotnet add ./UserAPI package Newtonsoft.Json --version 12.0.3
