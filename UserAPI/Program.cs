// Copyright (c) Microsoft. All Rights Reserved.
// License under the Apache License, Version 2.0.
// API with mongodb, SQL server database and more.
// Owner: Pham Hong Phuc

using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using UserAPI.Services.ElasticsearchService;
using UserAPI.Services.IpfsService;
using UserAPI.Services.MongoService;
using UserAPI.Services.SQLServerService;

namespace UserAPI
{
    public class Program
    {
        public static MongoConnecter mongoConnecter { get; private set; }
        public static SQLConnecter sqlConnecter { get; private set; }
        public static IpfsConnecter IpfsConnecter { get; private set; }
        public static ElasticsearchConnecter elasConnecter { get; private set; }

        public static BudgetService budgetService { get; private set; }
        public static CategoryService categoryService { get; private set; }
        public static CurrencyService currencyService { get; private set; }
        public static IconService iconService { get; private set; }
        public static TransactionService transactionService { get; private set; }
        public static UserService userService { get; private set; }
        public static WalletService walletService { get; private set; }
        public static EmployeeService employeeService { get; private set; }
        public static Services.IpfsService.BaseService ipfsService { get; private set; }
        public static Services.ElasticsearchService.BaseService elasService { get; private set; }

        public static void Main(string[] args)
        {
            IConfigurationRoot config = new ConfigurationBuilder().AddJsonFile("appsettings.json", optional: false).Build();
            IConfigurationSection mongoSetting = config.GetSection("MongoSetting");
            IConfigurationSection sqlSetting = config.GetSection("SQLSetting");
            IConfigurationSection ipfsSetting = config.GetSection("IpfsSetting");
            IConfigurationSection elasSetting = config.GetSection("ElasticsearchSetting");

            //Create new connections to database
            mongoConnecter = new MongoConnecter(mongoSetting);
            sqlConnecter = new SQLConnecter(sqlSetting);
            IpfsConnecter = new IpfsConnecter(ipfsSetting);
            elasConnecter = new ElasticsearchConnecter(elasSetting);

            //Init mongo service
            budgetService = new BudgetService("Budget");
            categoryService = new CategoryService("Category");
            currencyService = new CurrencyService("Currency");
            iconService = new IconService("Icon");
            transactionService = new TransactionService("Transaction");
            userService = new UserService("User");
            walletService = new WalletService("Wallet");

            //Init sql server service
            employeeService = new EmployeeService();

            //Init ipfs service
            ipfsService = new Services.IpfsService.BaseService();

            //Init elasticsearch service
            elasService = new Services.ElasticsearchService.BaseService();

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
