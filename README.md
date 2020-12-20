# UserAPI

## A simple API to connect database

## setup service swagger
add below code in file <project>.csproj to enabled XML Comments

    <PropertyGroup>
        <GenerateDocumentationFile>true</GenerateDocumentationFile>
        <NoWarn>$(NoWarn);1591</NoWarn>
    </PropertyGroup>
detail [here](https://docs.microsoft.com/en-us/aspnet/core/tutorials/getting-started-with-swashbuckle?view=aspnetcore-3.1&tabs=visual-studio)
    
## usage
###### .NET CLI
    cd /UserAPI
    to install libraries: make install
    to run program: make run

## note
    make sure your computer has installed make

## reference
###### mongo
[github_library](https://github.com/mongodb/mongo-csharp-driver)      
[document](https://docs.mongodb.com/drivers/csharp)

###### logger
[document](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/logging/?view=aspnetcore-3.1)

###### swagger
[document](https://docs.microsoft.com/vi-vn/aspnet/core/tutorials/getting-started-with-swashbuckle?view=aspnetcore-3.1&tabs=visual-studio)

###### middleware
[document](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/middleware/?view=aspnetcore-3.1#:~:text=Middleware%20is%20software%20that's%20assembled,next%20component%20in%20the%20pipeline.)

###### ipfs
[github](https://github.com/richardschneider/net-ipfs-http-client)