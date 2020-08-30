## File Config in C# .NET Core
1. [Cấu hình trong ASP.NET Core](#section1)
    1. [Load cấu hình](#section1.1)
        1. [Load cấu hình từ file json](#section1.1.1)
        2. [Load từ User Secrets](#section1.1.2)
        3. [Load cấu hình từ biến môi trường](#Section1.1.3)
        4. [Đọc từ Command Line Arguments](#section1.1.4)
    2. [Đọc cấu hình](#section3)

### Cấu hình trong ASP.NET Core<a name="section1"></a>
Cấu hình trong ASP.NET Core được lưu dưới dạng key-value, người dùng có thể lưu cấu hình trong các file định dạng
- Json
- XML
- INI

         {  
            "option1": "value1",  
            "option2": 2
         }
         {
            "subsection": 
            {
               "suboption1": "subvalue1",
               "suboption2": "subvalue2"  
            }
        }
        
ASP.NET Core hỗ trợ đọc cấu hình từ các nguồn khác nhau và các định dạng khác nhau. Một vài nguồn được sử dụng phổ biến:
- Định dạng file: JSON, INI, XML
- Command line Arguments (tham số dòng lệnh)
- Environment variables (biến môi trường)
- Custom Provider (cái này là tự tạo ra provider riêng theo ý muốn)
### Load cấu hình<a name="section1.1"></a>
       public class Program
       {
           public static void Main(string[] args)
           {
               BuildWebHost(args).Run();
           }
           public static IWebHost BuildWebHost(string[] args) =>
               WebHost.CreateDefaultBuilder(args)
                   .UseStartup<Startup>()
                   .Build();
        }
phương thức Main gọi đến CreateDefaultBuilder là một Helper Class, một trong những công việc là load cấu hình từ các nguồn

        .ConfigureAppConfiguration((hostingContext, config) => {
        var env = hostingContext.HostingEnvironment;
        config.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
              .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: true);
 
        if (env.IsDevelopment()) {
            var appAssembly = Assembly.Load(new AssemblyName(env.ApplicationName));
            if (appAssembly != null) {
                config.AddUserSecrets(appAssembly, optional: true);
            }
        }
        config.AddEnvironmentVariables();
            if (args != null) {
               config.AddCommandLine(args);
             }
        })
Phương thức ConfigureAppConfiguration nhận 2 tham số
- Tham số thứ nhất: là thể hiện của WebHostBuilderContext
-	Tham số thứ hai:là thể hiện của IConfiguretionBuilder.WebHostBuilderContext đưa ra thuộc tính HostingEnvironment giúp chúng ta biết đang chạy ở môi trường nào(Development, Production, Staging)
<br/><b>IConfigurationBuilder</b> đưa ra một số phương thức để load file cấu hình
##### Load cấu hình từ file Json<a name="section1.1.1"></a>
       config.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
Phương thức AddJsonFile giúp load cấu hình từ file json, có ba tham số truyền vào:
-	Tham số thứ nhất: đường dẫn tương đối đến file cấu hình
-	Tham số thứ hai: nếu để true nó sẽ không sinh lỗi nếu không tìm thấy đường dẫn cung cấp bởi tham số đầu tiên
-	Tham số thứ ba: nếu để true, chương trình sẽ tự động load lại cấu hình nếu file cấu hình bị thay đổi
##### Load từ User Secrets<a name="section1.1.2"></a>
Đây là tính năng mới trong ASP.NET Core khi mà cấu hình được lưu trữ bên ngoài mã nguồn. Nó được gọi là user secrets và chỉ có thể áp dụng cho môi trường Development. Điều này hữu ích trong trường hợp bạn có những cấu hình chỉ muốn sử dụng trên máy dev thôi, và không muốn nó nằm trong mã nguồn khi đưa code lên source control chung.

      if (env.IsDevelopment()) {
         var appAssembly = Assembly.Load(new AssemblyName(env.ApplicationName));
         if (appAssembly != null) {
             config.AddUserSecrets(appAssembly, optional: true);
         }                   
      }
##### Load cấu hình từ biến môi trường<a name="section1.1.3"></a>
Phương thức AddEnvironmentVariables() đọc tất cả các user và system environment variable trong hệ điều hành ra.

      config.AddEnvironmentVariables();
##### Đọc từ Command Line Arguments<a name="section1.1.4"></a>
Phương thức AddCommandLine đọc tất cả các tham số của dòng lệnh khi các bạn gọi ứng dụng bằng command line mà truyền các tham số.

      if (args != null) {
        config.AddCommandLine(args);                    
      }
### Đọc cấu hình<a name="section1.2"></a>
Để đọc cấu hình, chúng ta cần thể hiện của IConfiguration. Chúng ta có thể dùng dependency injection để lấy thể hiện của nó trong constructor của Startup class. Bạn có thể dùng kỹ thuật này tương tự với Controller.

      public Startup(IConfiguration configuration)
      {
          Configuration = configuration;
      }
      public IConfiguration Configuration { get; }
Sau đó nhận giá trị với phương thức Configuration.GetSection:

      await context.Response.WriteAsync("<div>" + Configuration.GetSection("message").Value + "</div>");
   