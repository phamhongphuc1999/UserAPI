### API đơn giản kết nối với cơ sở dữ liệu mongo
    ngôn ngữ sử : C# ASP.NET Core
    trình biên dịch sử sụng: Visual Studio 2019

### cấu trúc chương trình
##### chương trình có hai project
- MongoDB: class core, có chức năng tương tác với mongodb
- UserAPI: ASP.NET API Core, tạo API tương tác với mongodb thông qua Model

### cách chạy chương trình
##### chạy bằng .NET CLI trên visual studio 2019
    cài đặt thư viện gọi lệnh: make install
    để chạy chương trình gọi lệnh: make run
    để chạy chương trình với profile UserAPI gọi lệnh: make runuser
    để chạy chương trình với profile Docker gọi lệnh: make rundocker
##### chạy bằng docker
    để chạy chương trình, gọi lệnh: docker-compose up

### những lưu ý
    chắc chắn máy tính đã cài make
    muốn biết chương trình có những profile nào đọc trrong file: launchSettings.json

##### Collection postman: https://www.getpostman.com/collections/1276a5fd32ba82f3f967

### tài liệu tham khảo
##### mongo
- https://docs.mongodb.com/drivers/csharp
- https://github.com/mongodb/mongo-csharp-driver
- https://www.mongodb.com/blog/post/quick-start-csharp-and-mongodb--update-operation#:~:text=Set(%22class_id%22%2C%20483,no%20documents%20will%20be%20updated.
##### logger
- https://docs.microsoft.com/en-us/aspnet/core/fundamentals/logging/?view=aspnetcore-3.1
##### config file
- https://stackoverflow.com/questions/31453495/how-to-read-appsettings-values-from-a-json-file-in-asp-net-core
- https://stackoverflow.com/questions/46364293/automatically-set-appsettings-json-for-dev-and-release-environments-in-asp-net-c
- https://dotnettutorials.net/lesson/asp-net-core-launchsettings-json-file/
##### docker
- https://github.com/TechMaster/DockerizeDotNetCoreConsoleApp
- https://stackoverflow.com/questions/51769324/how-to-create-run-net-core-console-app-in-docker
##### swagger
- https://docs.microsoft.com/vi-vn/aspnet/core/tutorials/getting-started-with-swashbuckle?view=aspnetcore-3.1&tabs=visual-studio
- https://github.com/hinault/SwaggerDemo
##### JWT
- https://stackoverflow.com/questions/51943722/how-to-validate-jwt-token-in-aspnet-core-web-api
##### middleware
- https://stackoverflow.com/questions/36711068/call-controllers-action-method-from-middleware
- https://docs.microsoft.com/en-us/aspnet/core/fundamentals/middleware/?view=aspnetcore-3.1#:~:text=Middleware%20is%20software%20that's%20assembled,next%20component%20in%20the%20pipeline.
