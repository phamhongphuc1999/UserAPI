# Common

## Run project.
run:
	dotnet run --launch-profile UserAPI --project ./UserAPI

## Run project with specified launch.
runl:
	dotnet run --launch-profile ${launch} --profile ./UserAPI
