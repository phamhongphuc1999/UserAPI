// Copyright (c) Microsoft. All Rights Reserved.
// License under the Apache License, Version 2.0.
// API with mongodb, SQL server database and more.
// Owner: Pham Hong Phuc

using UserAPI.Models.SQLServerModel;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace UserAPI
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        private readonly IWebHostEnvironment _env;

        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            Configuration = configuration;
            _env = env;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //config swagger service
            services.AddSwaggerGen(c =>
            {
                var xmlPath = "Properties/UserAPI.xml";
                c.IncludeXmlComments(xmlPath);
            });

            //config cors service
            services.AddCors(options =>
            {
                options.AddPolicy("MyCorsPolicy",
                    builder =>
                    {
                        builder.WithOrigins("http://localhost:3000")
                            .AllowAnyMethod().AllowAnyHeader().AllowCredentials();
                    });
            });

            //connect SQL Server
            services.AddDbContext<SQLData>();
            services.AddScoped<SQLData>();

            services.AddControllers();
            services.AddHttpContextAccessor();
            services.AddMvcCore();

            services.Configure<JWTConfig>(Configuration.GetSection("JWT"));
            services.Configure<MongoSetting>(Configuration.GetSection("MongoSetting"));
            if (_env.IsDevelopment()) services.Configure<DevelopmentConfig>(Configuration.GetSection("Develop"));
            else if (_env.IsProduction()) services.Configure<ProductionConfig>(Configuration.GetSection("Product"));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app)
        {
            if (_env.IsDevelopment()) app.UseDeveloperExceptionPage();
            else if (_env.IsProduction()) app.UseExceptionHandler();

            //swagger
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
                c.RoutePrefix = string.Empty;
            });

            app.UseRouting();
            app.UseAuthorization();

            app.UseCors("MyCorsPolicy");
            app.Use(async (context, next) =>
            {
                context.Response.Headers["Access-Control-Allow-Origin"] = "*";
                await next.Invoke();
            });

            //check authorization
            string secretKey = Configuration.GetSection("JWT").GetValue<string>("SecretKey");
            string mainUrl = Configuration.GetValue<string>("Develop:ApplicationUrl");

            app.UseMiddleware<LoggerMiddleware>(mainUrl);

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
            app.Run(async context =>
            {
                context.Response.StatusCode = StatusCodes.Status404NotFound;
                await context.Response.WriteAsync("Page not found");
            });
        }
    }
}
