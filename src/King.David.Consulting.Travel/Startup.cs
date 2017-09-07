using AutoMapper;
using FluentValidation.AspNetCore;
using King.David.Consulting.Common.AspNetCore;
using King.David.Consulting.Common.AspNetCore.Errors;
using King.David.Consulting.Common.AspNetCore.Security;
using King.David.Consulting.Common.AspNetCore.Security.Password;
using King.David.Consulting.Common.AspNetCore.Security.Token;
using King.David.Consulting.Travel.Web.Domain;
using King.David.Consulting.Travel.Web.Infrastructure;

using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SpaServices.Webpack;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Swashbuckle.AspNetCore.Swagger;


namespace King.David.Consulting.Travel.Web
{
    public class Startup
    {
        private IHostingEnvironment _hostingEnv;

        public Startup(IHostingEnvironment env)
        {
            _hostingEnv = env;

            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMediatR();

            // Add the Options framework.
            services.AddOptions();
            services.Configure<AppSettings>(options => Configuration.GetSection("AppSettings"));
            services.Configure<PasswordHasherOptions>(options => Configuration.GetSection("PasswordHasher").Bind(options));
            //services.Configure<PasswordHasher>(Configuration);

            services.AddEntityFrameworkSqlite()
                .AddDbContext<AppDbContext>(options =>
                                            options.UseSqlite(string.Format(Configuration.GetConnectionString("(default)"), _hostingEnv.ContentRootPath)));

            // Inject an implementation of ISwaggerProvider with defaulted settings applied
            services.AddSwaggerGen(x =>
            {
                x.SwaggerDoc("v1", new Info { Title = Configuration["AppSettings:ApplicationName"], Version = "v1" });
                x.CustomSchemaIds(y => y.FullName);
                x.DocInclusionPredicate((version, apiDescription) => true);
                x.TagActionsBy(y => y.GroupName);
            });

            services.AddCors();
            services.AddMvc(opt =>
            {
                opt.Conventions.Add(new GroupByApiRootConvention());
                opt.Filters.Add(typeof(ValidatorActionFilter));
            })
                .AddJsonOptions(options =>
                {
                    options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
                    options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                    options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                })
                .AddFluentValidation(cfg => { cfg.RegisterValidatorsFromAssemblyContaining<Startup>(); });

            services.AddAutoMapper();

            services.AddScoped<DbInitializer>();

            services.AddScoped<IPasswordHasher, PasswordHasher>();
            services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();
            services.AddScoped<ICurrentUserAccessor, CurrentUserAccessor>();
            //services.AddScoped<IProfileReader, ProfileReader>();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddJwt(Configuration);

            Mapper.AssertConfigurationIsValid();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            //loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            //loggerFactory.AddDebug();
            //loggerFactory.AddFile("Logs/ts-{Date}.txt");
            loggerFactory.AddSerilogLogging();

            app.UseMiddleware<ErrorHandlingMiddleware>();

            app.UseCors(builder =>
                      builder
                      .AllowAnyOrigin()
                      .AllowAnyHeader()
                      .AllowAnyMethod());
            app.UseJwt();

            if (env.IsDevelopment())
            {
                //app.UseDeveloperExceptionPage();
                app.UseWebpackDevMiddleware(new WebpackDevMiddlewareOptions {
                    HotModuleReplacement = true
                });
             
            }
            else
            { 
                app.UseExceptionHandler("/Home/Error");
            }
            
            app.UseDefaultFiles();

            app.UseStaticFiles();

            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS etc.), specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Travel.Web API V1");
            });

            app.MapWhen(x => !x.Request.Path.Value.StartsWith("/swagger"), builder =>
            {
                builder.UseMvc(routes =>
                {
                    routes.MapRoute(
                                    name: "default",
                                    template: "{controller=Home}/{action=Index}/{id?}");
                    routes.MapSpaFallbackRoute(
                              name: "spa-fallback",
                              defaults: new { controller = "Home", action = "Index" });
                });
            });

            app.ApplicationServices.GetRequiredService<AppDbContext>().Database.EnsureCreated();
            app.ApplicationServices.GetRequiredService<DbInitializer>().Seed().Wait();
        }
    }
}
