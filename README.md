### API đơn giản kết nối với cơ sở dữ liệu mongo
    ngôn ngữ: C# ASP.NET Core
    trình biên dịch sử sụng: Visual Studio 2019

### cấu trúc chương trình
##### chương trình có hai project
- Model: class core, có chức năng tương tác với mongodb
- UserAPI: ASP.NET API Core, tạo API tương tác với mongodb thông qua Model

### cách chạy chương trình
#####: chạy bằng .NET CLI trên visual studio 2019
    để chạy chương trình gọi lệnh: make run
    cài đặt thư viện gọi lệnh: make install
#####: chạy bằng docker
    để chạy chương trình, gọi lệnh: docker-compose up

### những lưu ý
    chắc chắn máy tính đã cài make

### tài liệu tham khảo
- https://docs.mongodb.com/drivers/csharp
- https://github.com/mongodb/mongo-csharp-driver
