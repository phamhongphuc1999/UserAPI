## File Config in C# .NET Core
1. [Cấu hình trong ASP.NET Core](#section1)
    1. [Load cấu hình](#section1.1)
        1. [Load cấu hình từ file json](#section1.1.1)
        2. [Load từ User Secrets](#section1.1.2)
        3. [Load cấu hình từ biến môi trường](#Section1.1.3)
        4. [Đọc từ Command Line Arguments](#section1.1.4)
    2. [Đọc cấu hình](#section1.2)
2. [Middleware/Pipeline trong C#](#section2)
    1. [Tổng quan](#section2.1)
    2. [Custom Middleware](#section2.2)
    3. [Map](#section2.3)
    4. [Truyền dữ liệu từ Startup vào middleware](#section2.4)

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

### Middleware/Pipeline trong C#<a name="section2"></a>
#### Tổng quan<a name="section2.1"></a>
Một Middleware là một module code nó nhận yêu cầu gửi đến Request và trả về Response. Cụ thể trong ASP.NET Core, middlewarre có thể:
- Nhận một HTTP Request gửi đến và phát sinh ra HTTP Response để trả về
-	Nhận một HTTP Request gửi đến, thi hành một số tác vụ (có thể là sửa đổi HTTP Request), sau đó chuyển đến một middleware khác.
-	Nhận HTTP Response, sửa nó và chuyển đến một Middleware khác
###### Pipeline: Trong ứng dụng ASP.NET Core, các middlware kết nối lại với nhau thành một xích, middleware đầu tiên nhận HTTP Request, xử lý nó và có thể chuyển cho middleware tiếp theo hoặc trả về ngay HTTP Response. Chuỗi các middleware theo thứ tự như vậy gọi là pipeline.
![image](https://raw.githubusercontent.com/xuanthulabnet/learn-cs-netcore/master/imgs/cs074.png)
<br/><br/>Các middleware như là các dịch vụ nhỏ, đăng ký vào ứng dụng bằng cách sử dụng đối tượng IApplicationBuilder, sau đó ứng dụng căn sẽ xây dựng lên các pipeline (luồng xử lý) cho các truy vấn gửi đến
<br/>Trong ASP.NET Core, middleware được định nghĩa trong Configure lớp Startup, đồng thời ASP.NET Core cung cấp nhiều middleware có sẵn

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
            app.UseDatabaseErrorPage();
        }
        else
        {
            app.UseExceptionHandler("/Error");
            app.UseHsts();
        }
        app.UseHttpsRedirection();
        app.UseStaticFiles();
        app.UseCookiePolicy();
        app.UseRouting();
        app.UseAuthentication();
        app.UseAuthorization();
        app.UseSession();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapRazorPages();
        });
    }
    
hình trên thể hiện một số middleware có sẵn được gọi: UseHttpRedirection(), UseStaticFile(), UseCookiePolicy(), UseRouting(), UseAuthentication(), UseAuthorization(), UseSession()
#### Custom Middleware<a name="section2.2"></a>
Có hai cách chính để tạo và thực thi một middleware
-	Tạo inline middleware ngay trong Startup.Configure()
-	Tạo class middleware
###### Inline Middleware
    public void Configure(IApplicationBuilder app)
    {
        // use inline middleware
        app.Use(async (context, next) =>
        {
            // if specific condition does not meet
            if (context.Request.Path.ToString().Equals("/foo"))
            {
                context.Response.Redirect("path/to/controller/action");
            }
            else
            {
                await next.Invoke();
            }
        });
        // or use a middleware class
        app.UseMiddleware<RedirectMiddleware>();
        app.UseMvc();
    }
    
khối code trên có dòng code app.UseMiddleware<RedirectMiddleware>() chính là cách gọi đến một class middleware
###### Class Middleware
    public class RedirectMiddleware
    {
        private readonly RequestDelegate _next;
        public RedirectMiddleware(RequestDelegate next)
        {
            _next = next;
        }
        public async Task Invoke(HttpContext context)
        {
            // if specific condition does not meet
            if (context.Request.Path.ToString().Equals("/bar"))
            {
                context.Response.Redirect("path/to/controller/action");
            }
            else
            {
                await _next.Invoke(context);
            }
        }
    }
Như đã nói từ đầu phần 2, middleware có thể gọi đến một middleware khác, để làm việc đấy chỉ cần dòng code: await _next(context)
<br/>Nếu muốn gọi controller trong middleware chỉ cần dòng code:  await _next.Invoke(context)
#### Map
Map được sử dụng để có thể rẽ nhánh trong pipeline, ví dụ bạn chỉ muốn middleware tác động đến một phần chứ không phải tất cả thì bạn cần dùng đến middle
###### VD1
    public void Configure(IApplicationBuilder app)
    {
        app.Map("/map1", HandleMapTest1);
        app.Map("/map2", HandleMapTest2);
        app.Run(async context =>
        {
            await context.Response.WriteAsync("Hello from non-Map delegate. <p>");
        });
    }
###### VD2
    app.Map("/level1", level1App => {
        level1App.Map("/level2a", level2AApp => {
            // "/level1/level2a" processing
        });
        level1App.Map("/level2b", level2BApp => {
            // "/level1/level2b" processing
        });
    });
#### Truyền dữ liệu từ startup vào middleware<a name="section2.4"></a>    
để truyền dữ liệu vào middleware, ta thay đổi một chút ở hàm UseMiddleware
    
    string data = "data for middleware"
    app.UseMiddleware<RedirectMiddleware>(data);
và contructor của RedirectMiddleware sẽ được sửa lại như sau

    public RedirectMiddleware(RequestDelegate next, string data)
    {
        _next = next;
    }
