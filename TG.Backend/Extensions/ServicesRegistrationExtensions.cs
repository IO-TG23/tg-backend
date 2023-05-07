using Azure.Storage.Blobs;
using FluentValidation;
using Google.Authenticator;
using Lib.AspNetCore.ServerSentEvents;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using TG.Backend.Data.SSE;
using TG.Backend.Email;
using TG.Backend.Features.Behaviours;
using TG.Backend.Filters;
using TG.Backend.Middlewares;
using TG.Backend.Repositories.Blob;
using TG.Backend.Repositories.Client;
using TG.Backend.Repositories.Offer;
using TG.Backend.Services;

namespace TG.Backend.Extensions
{
    /// <summary>
    /// Rejestracja wszystkich serwisow - wyodrebniona - lepsza czytelnosc
    /// </summary>
    public static class ServicesRegistrationExtensions
    {
        public static void ConfigureServices(this WebApplicationBuilder builder)
        {
            IServiceCollection services = builder.Services;

            #region auth
            services.AddDbContext<AppDbContext>(opts =>
            {
                opts.UseNpgsql(builder.Configuration.GetConnectionString("MainConn"), pgOpts =>
                {
                    pgOpts.SetPostgresVersion(new Version("9.6"));
                });
            })
                .AddIdentity<AppUser, IdentityRole>(opts =>
                {
                    opts.Password.RequiredLength = 9;
                    opts.User.RequireUniqueEmail = true;
                    opts.SignIn.RequireConfirmedEmail = true;
                    opts.Lockout.MaxFailedAccessAttempts = 3;
                    opts.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(10);
                }).AddRoles<IdentityRole>()
                .AddDefaultTokenProviders().AddEntityFrameworkStores<AppDbContext>();

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(o =>
            {
                o.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidIssuer = builder.Configuration["Jwt:Issuer"],
                    ValidAudience = builder.Configuration["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey
                    (Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])),
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true
                };
            });

            services.AddAuthorization();

            services.AddSingleton<JwtSecurityTokenHandler>();
            services.AddScoped<ValidateAccountNotLockedFilter>();
            services.AddScoped<GetCurrentUserFromTheHeaderFilter>();

            #endregion auth

            #region libraries
            services.AddAutoMapper(typeof(Program).Assembly);
            services.AddMediatR(opts => opts.RegisterServicesFromAssemblyContaining<Program>());
            services.AddValidatorsFromAssemblyContaining<Program>();
            #endregion

            #region services-and-repositories
            services.AddServerSentEvents();
            services.AddServerSentEvents<INotificationsServerSentEventsService, NotificationsServerSentEventsService>();
            services.AddSingleton<TwoFactorAuthenticator>();
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));
            services.AddScoped<IOfferRepository, OfferRepository>();
            services.AddScoped<IClientRepository, ClientRepository>();
            services.AddTransient<IBlobRepository, BlobRepository>();

            if (!builder.Environment.IsProduction())
            {
                services.AddScoped<IEmailSender, DevEmailSender>();
                services.AddScoped<ISendPasswordTokenService, DevSendPasswordTokenService>();
            }
            else
            {
                services.AddScoped<IEmailSender, FluentEmailSender>();

                //IMPORTANT!! TO DELETE LATER ONE - WHEN PROPER SEND TOKEN SERVICE 
                services.AddScoped<ISendPasswordTokenService, DevSendPasswordTokenService>();

            }

            builder.Services.AddScoped<IAzureBlobStorageService, AzureBlobStorageService>();
            builder.Services.AddScoped(x =>
                new BlobServiceClient(builder.Configuration.GetValue<string>("ConnectionStrings:AzureBlobContainer")));

            #endregion

            #region cors
            services.AddCors(opts =>
            {
                opts.AddPolicy("AllowAnyone", policyOpts =>
                {
                    policyOpts.AllowAnyMethod().AllowAnyHeader().AllowAnyOrigin();
                });
            });
            #endregion

            #region middlewares
            services.AddScoped<ErrorHandlingMiddleware>();
            #endregion

            #region aspnet
            services.AddControllers();
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();
            #endregion
        }
    }
}
