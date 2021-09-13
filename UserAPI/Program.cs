// -------------------- SIMPLE API -------------------- 
//
//
// Copyright (c) Microsoft. All Rights Reserved.
// License under the Apache License, Version 2.0.
//
//
// Product by: Pham Hong Phuc
//
//
// ----------------------------------------------------

using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using UserAPI.Connecter;
using UserAPI.Services.MongoService;
using UserAPI.Services.SQLServerService;

namespace UserAPI
{
    public class Program
    {
        public static APIConnection APIConnecter { get; private set; }

        public static UserService userService { get; private set; }
        public static ProductService productService { get; private set; }
        public static EmployeeService employeeService { get; private set; }

        public static void Main(string[] args)
        {
            IConfigurationRoot config = new ConfigurationBuilder().AddJsonFile("appsettings.json", optional: false).Build();
            IConfigurationSection mongoSetting = config.GetSection("MongoSetting");
            IConfigurationSection sqlSetting = config.GetSection("SQLSetting");
            IConfigurationSection sqliteSetting = config.GetSection("SQLiteSetting");

            //Create new connections to database
            APIConnecter = new APIConnection(mongoSetting, sqlSetting, sqliteSetting);

            //Init mongo service
            userService = new UserService("Users");
            productService = new ProductService("Products");

            //Init sql server service
            employeeService = new EmployeeService();

            IHostBuilder builder = CreateHostBuilder(args);
            builder.Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
                .ConfigureLogging(logging =>
                {
                    logging.ClearProviders();
                    logging.AddConsole();
                })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
        }
    }
}
