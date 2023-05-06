using AutoMapper;
using Google.Authenticator;
using Microsoft.AspNetCore.Identity.UI.Services;
using TG.Backend.Models.Client;
using TG.Backend.Repositories.Client;

namespace TG.Backend.Features.Handler
{
    public class RegisterUserHandler : IRequestHandler<RegisterUserCommand, AuthResponseModel>
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IMapper _mapper;
        private readonly IEmailSender _sender;
        private readonly TwoFactorAuthenticator _authenticator;
        private readonly IConfiguration _configuration;
        private readonly IClientRepository _clientRepository;

        public RegisterUserHandler(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager,
            IMapper mapper, IEmailSender sender, TwoFactorAuthenticator authenticator, IConfiguration configuration,
            IClientRepository clientRepository)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _mapper = mapper;
            _sender = sender;
            _authenticator = authenticator;
            _configuration = configuration;
            _clientRepository = clientRepository;
        }

        public async Task<AuthResponseModel> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
        {
            AppUser user = _mapper.Map<AppUser>(request.AppUser);

            user.UserName = request.AppUser.Email;

            IdentityResult res = await _userManager.CreateAsync(user, request.AppUser.Password);

            if (!res.Succeeded)
            {
                return new()
                {
                    IsSuccess = false,
                    StatusCode = HttpStatusCode.Unauthorized,
                    Messages = res.Errors.Select(e => e.Description).ToArray()
                };
            }

            //TODO - do przeniesienia
            await CreateRoles();

            await _userManager.AddToRoleAsync(user, "Client");

            string token = await _userManager.GenerateEmailConfirmationTokenAsync(user);

            //TODO - wiadomosc mailowa - wyglad
            await _sender.SendEmailAsync(user.Email, "Potwierdz adres email", token);

            SetupCode setupInfo = _authenticator.GenerateSetupCode(_configuration["2FA:Issuer"], user.Email, _configuration["2FA:Key"], false);
            string qrCode = setupInfo.QrCodeSetupImageUrl;

            await _clientRepository.CreateClient(new CreateClientDTO() { AppUserId = Guid.Parse(user.Id), AppUser = user });

            return new()
            {
                IsSuccess = true,
                StatusCode = HttpStatusCode.OK,
                Messages = new[] { qrCode }
            };
        }

        /// <summary>
        /// Pomocnicza metoda tworzaca role - do przeniesienia
        /// </summary>
        /// <returns></returns>
        private async Task CreateRoles()
        {
            bool rolesNotExist = !await _roleManager.RoleExistsAsync("Client") &&
                !await _roleManager.RoleExistsAsync("Employee") &&
                !await _roleManager.RoleExistsAsync("Admin");

            if (rolesNotExist)
            {
                IdentityRole clientRole = new()
                {
                    Name = "Client"
                };

                IdentityRole employeeRole = new()
                {
                    Name = "Employee"
                };

                IdentityRole adminRole = new()
                {
                    Name = "Admin"
                };

                await _roleManager.CreateAsync(clientRole);
                await _roleManager.CreateAsync(employeeRole);
                await _roleManager.CreateAsync(adminRole);
            }
        }
    }
}
