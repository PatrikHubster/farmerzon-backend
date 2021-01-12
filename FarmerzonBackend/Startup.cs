using System.Text;
using FarmerzonBackend.GraphControllerType;
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
        private const string CorsPolicy = "allowedOrigins";
        private IConfiguration Configuration { get; }
        
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(c => 
            {
                c.AddPolicy(CorsPolicy, options =>
                {
                    options.WithOrigins(Configuration.GetSection("AllowedOrigins").Get<string[]>());
                });
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
            services.AddScoped<IAddressManager, AddressManager>();
            services.AddScoped<IArticleManager, ArticleManager>();
            services.AddScoped<ICityManager, CityManager>();
            services.AddScoped<ICountryManager, CountryManager>();
            services.AddScoped<IPersonManager, PersonManager>();
            services.AddScoped<IStateManager, StateManager>();
            services.AddScoped<IUnitManager, UnitManager>();
            services.AddScoped<ITokenManager, TokenManager>();
            
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

            services.AddScoped<AddressOutputType>();
            services.AddScoped<ArticleOutputType>();
            services.AddScoped<CityOutputType>();
            services.AddScoped<CountryOutputType>();
            services.AddScoped<PersonOutputType>();
            services.AddScoped<StateOutputType>();
            services.AddScoped<UnitOutputType>();
            
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
            app.UseCors(CorsPolicy);

            // It is important to use app.UseAuthentication(); before app.UseAuthorization();
            // Otherwise authentication with json web tokens doesn't work.
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}