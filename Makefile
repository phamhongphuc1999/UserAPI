run:
	dotnet run -p ./UserAPI

install:
	dotnet add ./Model package MongoDB.Bson
	dotnet add ./Model package MongoDB.Driver
	dotnet add ./UserAPI package Newtonsoft.Json
