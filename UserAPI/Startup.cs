using System;
using System.IO;
using System.Reflection;
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

        public Startup(IWebHostEnvironment env)
        {
            _env = env;
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            Configuration = builder.Build();
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //add swagger service
            services.AddSwaggerGen(c =>
            {
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });

            //connect SQL Server
            services.AddDbContext<SQLData>();
            services.AddScoped<SQLData>();

            services.AddControllers();
            services.AddHttpContextAccessor();
            services.AddMvcCore();
            services.Configure<JWTConfig>(Configuration.GetSection("JWT"));
            if (_env.IsDevelopment()) services.Configure<DevelopmentConfig>(Configuration.GetSection("Develop"));
            else if (_env.IsProduction()) services.Configure<ProductionConfig>(Configuration.GetSection("Product"));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app)
        {
            if (_env.IsDevelopment()) app.UseDeveloperExceptionPage();
            else if (_env.IsProduction()) app.UseExceptionHandler();

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
                c.RoutePrefix = string.Empty;
            });

            app.UseRouting();
            app.UseAuthorization();

            //check authorization
            string secretKey = Configuration.GetSection("JWT").GetValue<string>("SecretKey");
            string mainUrl = Configuration.GetValue<string>("Develop:ApplicationUrl");

            app.UseMiddleware<AuthorizedMiddleware>(secretKey, mainUrl);

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
