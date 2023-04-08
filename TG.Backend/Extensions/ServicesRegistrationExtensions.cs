using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using TG.Backend.Email;
using TG.Backend.Features.Behaviours;
using TG.Backend.Filters;
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
                opts.UseNpgsql(builder.Configuration.GetConnectionString("MainConn"));
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
                    ValidateLifetime = false,
                    ValidateIssuerSigningKey = true
                };
            });

            services.AddAuthorization();

            services.AddScoped<ValidateAccountNotLockedFilter>();

            #endregion auth

            #region libraries
            services.AddAutoMapper(typeof(Program).Assembly);
            services.AddMediatR(opts => opts.RegisterServicesFromAssemblyContaining<Program>());
            services.AddValidatorsFromAssemblyContaining<Program>();
            #endregion

            #region services-and-repositories
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));

            if (builder.Environment.IsDevelopment())
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

            #endregion

            #region aspnet
            services.AddControllers();
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();
            #endregion
        }
    }
}
