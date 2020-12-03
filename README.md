### API đơn giản kết nối với cơ sở dữ liệu mongo và SQL Server
### Công nghệ sử dụng
    C# Core ASP.NET API
    Visual studio 2019
    

###### setup service swagger
thêm đoạn code dưới đây vào file <project>.csproj để enabled XML Comments

    <PropertyGroup>
        <GenerateDocumentationFile>true</GenerateDocumentationFile>
        <NoWarn>$(NoWarn);1591</NoWarn>
    </PropertyGroup>
thông tin chi tiết ở [đây](https://docs.microsoft.com/en-us/aspnet/core/tutorials/getting-started-with-swashbuckle?view=aspnetcore-3.1&tabs=visual-studio)
    
### chạy chương trình
###### chạy bằng .NET CLI trên visual studio 2019(cd /UserAPI)
    cài đặt các thư viện cần thiết, gọi lệnh: make install
    chạy chương trình, gọi lệnh: make run
    chạy chương trình với profile UserAPI, gọi lệnh: make runuser

### những lưu ý
    chắc chắn máy tính đã cài make

### phát triển trong tương lai
- chạy swagger bằng một custom swagger.json
- chạy chương trình trên docker
- viết unittest cho API

### tài liệu tham khảo
###### mongo
- https://docs.mongodb.com/drivers/csharp
- https://github.com/mongodb/mongo-csharp-driver
- https://www.mongodb.com/blog/post/quick-start-csharp-and-mongodb--update-operation#:~:text=Set(%22class_id%22%2C%20483,no%20documents%20will%20be%20updated.
###### logger
- https://docs.microsoft.com/en-us/aspnet/core/fundamentals/logging/?view=aspnetcore-3.1
###### config file
- https://stackoverflow.com/questions/31453495/how-to-read-appsettings-values-from-a-json-file-in-asp-net-core
- https://stackoverflow.com/questions/46364293/automatically-set-appsettings-json-for-dev-and-release-environments-in-asp-net-c
- https://dotnettutorials.net/lesson/asp-net-core-launchsettings-json-file/
###### docker
- https://github.com/TechMaster/DockerizeDotNetCoreConsoleApp
- https://stackoverflow.com/questions/51769324/how-to-create-run-net-core-console-app-in-docker
###### swagger
- https://docs.microsoft.com/vi-vn/aspnet/core/tutorials/getting-started-with-swashbuckle?view=aspnetcore-3.1&tabs=visual-studio
- https://github.com/hinault/SwaggerDemo
###### JWT
- https://stackoverflow.com/questions/51943722/how-to-validate-jwt-token-in-aspnet-core-web-api
###### middleware
- https://stackoverflow.com/questions/36711068/call-controllers-action-method-from-middleware
- https://docs.microsoft.com/en-us/aspnet/core/fundamentals/middleware/?view=aspnetcore-3.1#:~:text=Middleware%20is%20software%20that's%20assembled,next%20component%20in%20the%20pipeline.
###### SQL Server
- https://medium.com/@chathuranga94/connect-database-to-asp-net-core-web-api-63a53e8da1ca
