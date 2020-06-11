using System;
using System.Text;
using FarmerzonBackend.GraphControllerType;
using FarmerzonBackend.GraphInputType;
using FarmerzonBackend.GraphOutputType;
using FarmerzonBackendManager.Implementation;
using FarmerzonBackendManager.Interface;
using GraphQL;
using GraphQL.DataLoader;
using GraphQL.Http;
using GraphQL.Server;
using GraphQL.Types;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;

namespace FarmerzonBackend
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        readonly string allowOrigins = "allowOrigins";
        
        public IConfiguration Configuration { get; }
        
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(c => 
            {
                c.AddPolicy(allowOrigins,
                    options =>
                    {
                        options.AllowAnyHeader().AllowAnyOrigin().AllowAnyMethod();
                    });
            });
            
            // Adding the micrservices like described on:
            // https://docs.microsoft.com/en-us/aspnet/core/fundamentals/http-requests?view=aspnetcore-3.1
            services.AddHttpClient("FarmerzonArticles", c =>
            {
                c.BaseAddress = new Uri($"http://{Configuration["BaseUrls:FarmerzonArticles:Host"]}:" +
                    $"{Configuration["BaseUrls:FarmerzonArticles:Port"]}");
            });
            
            // serialization for GraphQL error responses was not able. The following solution was found on stackoverflow
            // under the following url: https://stackoverflow.com/questions/59199593/net-core-3-0-possible-object-cycle
            // -was-detected-which-is-not-supported
            services.AddControllersWithViews()
                .AddNewtonsoftJson(options =>
                    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
                );
            
            services.AddControllers();
            
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = Configuration["Jwt:Issuer"],
                    ValidAudience = Configuration["Jwt:Issuer"],
                    RequireExpirationTime = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Jwt:Secret"]))
                };
            });

            // manager DI container
            services.AddScoped<IArticleManager, ArticleManager>();
            services.AddScoped<IPersonManager, PersonManager>();
            services.AddScoped<IUnitManager, UnitManager>();
            
            // graphQL DI container
            services.AddSingleton<IDataLoaderContextAccessor, DataLoaderContextAccessor>();
            services.AddSingleton<DataLoaderDocumentListener>();
            services.AddSingleton<IDocumentExecuter, DocumentExecuter>();
            services.AddSingleton<IDocumentWriter, DocumentWriter>();
            services.AddScoped<ISchema, RootSchema>();
            services.AddScoped<IDependencyResolver>(provider =>
                new FuncDependencyResolver(provider.GetRequiredService));
            
            services.AddScoped<RootQuery>();
            services.AddScoped<RootMutation>();

            services.AddScoped<ArticleOutputType>();
            services.AddScoped<PersonOutputType>();
            services.AddScoped<UnitOutputType>();
            
            services.AddScoped<UnitInputType>();
            services.AddGraphQL(o => o.ExposeExceptions = true).AddGraphTypes(ServiceLifetime.Scoped);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();
            app.UseCors(allowOrigins);

            // It is important to use app.UseAuthentication(); before app.UseAuthorization();
            // Otherwise authentication with json web tokens doesn't work.
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}