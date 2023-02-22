using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AspNet.Security.OpenIdConnect.Primitives;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Newtonsoft;
using PropertyMataaz.DataContext;
using PropertyMataaz.Filters;
using PropertyMataaz.Models;
using PropertyMataaz.Models.SeederModels;
using PropertyMataaz.Models.UtilityModels;
using PropertyMataaz.Repositories;
using PropertyMataaz.Repositories.Interfaces;
using PropertyMataaz.Services;
using PropertyMataaz.Services.Interfaces;
using PropertyMataaz.Utilities;
using PropertyMataaz.Utilities.Abstrctions;
using PropertyMataaz.Utilities.Constants;
using SendGrid.Extensions.DependencyInjection;

namespace PropertyMataaz
{
    public class Startup
    {
        public Startup(IWebHostEnvironment env,IConfiguration configuration)
        {
            Configuration = configuration;
            CurrentEnvironment = env;
        }

        private IWebHostEnvironment CurrentEnvironment{ get; set; }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            string envName = CurrentEnvironment.EnvironmentName;
            var AppSettingsSection = Configuration.GetSection("AppSettings");
            services.Configure<Globals>(AppSettingsSection);
            services.Configure<PagingOptions>(Configuration.GetSection("DefaultPagingOptions"));

            var AppSettings = AppSettingsSection.Get<Globals>();
            var Key = Encoding.ASCII.GetBytes(AppSettings.Secret);

            services.AddSingleton<IConfiguration>(provider => Configuration);

            services.AddDbContext<PMContext>(options =>
            {
                // options.UseSqlServer(Configuration.GetConnectionString("PMConnection"));
                var ConnectionString = "";
                // if (envName == "Development")
                // {
                //     // ConnectionString = Configuration.GetConnectionString("PMConnectionP");
                //     // Logger.Info(ConnectionString);
                //     ConnectionString = GetHerokuConnectionString(Configuration.GetConnectionString("herokudb"));
                // }
                // else
                // {
                //     var currentDeploymentEnvironment = Environment.GetEnvironmentVariable("ENVIRONMENT");
                //     if (currentDeploymentEnvironment == "Production")
                //     {
                //         ConnectionString = GetHerokuConnectionString(Environment.GetEnvironmentVariable("DATABASE_URL"));
                //     }
                //     else
                //     {
                //     }
                //     Logger.Info(ConnectionString);
                // }
                ConnectionString = UtilityConstants.ConnectionString;
                options.UseNpgsql(ConnectionString,
                npgsqlOptionsAction: sqlOptions =>
                {
                    sqlOptions.EnableRetryOnFailure(
                        maxRetryCount: 10,
                        maxRetryDelay: TimeSpan.FromSeconds(5),
                        errorCodesToAdd: null
                    );
                });
                options.UseOpenIddict<int>();
            });

            var context = services.BuildServiceProvider().GetRequiredService<PMContext>();
            if(envName != "Development")
            {
                context.Database.Migrate();
            }
                // context.Database.Migrate();
            

            services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequiredLength = 6;
                options.Password.RequiredUniqueChars = 0;
                options.ClaimsIdentity.UserIdClaimType = OpenIdConnectConstants.Claims.Name;
                options.ClaimsIdentity.UserIdClaimType = OpenIdConnectConstants.Claims.Subject;
                options.ClaimsIdentity.RoleClaimType = OpenIdConnectConstants.Claims.Role;
                options.SignIn.RequireConfirmedEmail = true;
            });

            services.AddIdentity<User, Role>()
                .AddEntityFrameworkStores<PMContext>()
                .AddDefaultTokenProviders();

            services.AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

                })
                .AddJwtBearer(x =>
                {
                    x.RequireHttpsMetadata = false;
                    x.SaveToken = true;
                    x.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Key),
                        ValidateIssuer = false,
                        ValidateAudience = false
                    };
                });




            AddIdentityCoreServices(services);
            //Configure app dependencies
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<IPropertyRepository, PropertyRepository>();
            services.AddTransient<IPropertyService, PropertyService>();
            services.AddTransient<IRequestRepository, RequestRepository>();
            services.AddTransient<IRequestService, RequestService>();
            services.AddTransient<IMediaRepository, MediaRepository>();
            services.AddTransient<IMediaService, MediaService>();
            services.AddTransient<IEmailHandler, EmailHandler>();
            services.AddTransient<IUtilityMethods, UtilityMethods>();
            services.AddTransient<ICodeProvider, CodeProvider>();
            services.AddTransient<IReportRepository, ReportRepository>();
            services.AddTransient<IReportService, ReportService>();
            services.AddTransient<IPropertyRequestRepository, PropertyRequestRepository>();
            services.AddTransient<IPropertyRequestService, PropertyRequestService>();
            services.AddTransient<IUserEnquiryRepository, UserEnquiryRepository>();
            services.AddTransient<IApplicationRepository, ApplicationRepository>();
            services.AddTransient<IApplicationService, ApplicationService>();
            services.AddTransient<IPaymentRepository, PaymentRepository>();
            services.AddTransient<IPaymentService, PaymentService>();
            services.AddTransient<ICleanRepository, CleanRepository>();
            services.AddTransient<ICleaningService, CleaningService>();
            services.AddTransient<ILandSearchRepository, LandSearchRepository>();
            services.AddTransient<ILandSearchService, LandSearchService>();
            services.AddTransient<IComplaintsCategoryRepository, ComplaintsCategoryRepository>();
            services.AddTransient<IComplaintsCategoryService, ComplaintsCategoryService>();
            services.AddTransient<IReliefRepository, ReliefRepository>();
            services.AddTransient<IReliefService, ReliefService>();
            services.AddTransient<IComplaintsRepository, ComplaintsRepository>();
            services.AddTransient<IComplaintsService, ComplaintsService>();
            services.AddTransient<ITenancyService, TenancyService>();
            services.AddTransient<ITenancyRepository, TenancyRepository>();
            services.AddTransient<IPDFHandler, PDFHandler>();

            services.AddAuthorization();
            services.AddAutoMapper(typeof(Startup));
            services.AddControllers().AddNewtonsoftJson(options =>
                options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
            );



            services.AddMvc(options =>
            {
                options.Filters.Add<LinkRewritingFilter>();
            });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "PropertyMataaz", Version = "v1" });

                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme (Example: 'Bearer 12345abcdef')",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        Array.Empty<string>()
                    }
                });
            });

            services.AddHttpContextAccessor();

            services.AddSendGrid(options =>
            {
                options.ApiKey = UtilityConstants.SendGridApiKey;
            });

            services.AddCors(options =>
            {
                options.AddDefaultPolicy(policy =>
                {
                    policy.AllowAnyHeader();
                    policy.AllowAnyMethod();
                    // policy.AllowAnyOrigin();
                    policy.AllowCredentials();
                });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, PMContext context)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();

            }
            new SeedData(context).SeedInitialData();
            app.UseCors(x => x
                .AllowAnyMethod()
                .AllowAnyHeader()
                // .AllowAnyOrigin()
                .SetIsOriginAllowed(origin => true) // allow any origin
                .AllowCredentials());
            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "PropertyMataaz v1"));

            app.UseHttpsRedirection();


            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        private static void AddIdentityCoreServices(IServiceCollection services)
        {
            var builder = services.AddIdentityCore<User>();
            builder = new IdentityBuilder(
                builder.UserType,
                typeof(Role),
                builder.Services
            );
            builder.AddRoles<Role>()
                .AddEntityFrameworkStores<PMContext>()
                .AddDefaultTokenProviders()
                .AddSignInManager<SignInManager<User>>();
        }

        private string GetHerokuConnectionString(string connectionUrl)
        {
            // Get the connection string from the ENV variables
            // string connectionUrl = Environment.GetEnvironmentVariable("DATABASE_URL");

            // parse the connection string
            var databaseUri = new Uri(connectionUrl);

            string db = databaseUri.LocalPath.TrimStart('/');
            string[] userInfo = databaseUri.UserInfo.Split(':', StringSplitOptions.RemoveEmptyEntries);

            return $"User ID={userInfo[0]};Password={userInfo[1]};Host={databaseUri.Host};Port={databaseUri.Port};Database={db};Pooling=true;SSL Mode=Require;Trust Server Certificate=True;";
        }
    }
}
