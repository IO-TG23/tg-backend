using FluentValidation.TestHelper;
using TG.Backend.Features.Auth.Validation;
using TG.Backend.Features.Command;
using TG.Backend.Models.Auth;

namespace TG.Backend.Tests.ValidatorTests.Auth;

public class AppUserRegisterDTOValidatorTests
{
    [Theory]
    [MemberData(nameof(GetInvalidModels))]
    public void Validate_ForInvalidModel_ReturnsFailure(AppUserRegisterDTO appUserChangePasswordDto)
    {
        // arrange
        var command = new RegisterUserCommand(appUserChangePasswordDto);
        var validator = new AppUserRegisterDTOValidator();

        // act
        var result = validator.TestValidate(command);

        // assert
        result.ShouldHaveAnyValidationError();
    }
    
    [Theory]
    [MemberData(nameof(GetValidModels))]
    public void Validate_ForValidModel_ReturnsFailure(AppUserRegisterDTO appUserChangePasswordDto)
    {
        // arrange
        var command = new RegisterUserCommand(appUserChangePasswordDto);
        var validator = new AppUserRegisterDTOValidator();

        // act
        var result = validator.TestValidate(command);

        // assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    public static IEnumerable<object[]> GetValidModels()
    {
        var models = new List<AppUserRegisterDTO>()
        {
            new ()
            {
                Email = "test@test.com",
                Password = "password",
                ConfirmPassword = "password"
            },
        };

        return models.Select(q => new object[] { q });
    }
    
    public static IEnumerable<object[]> GetInvalidModels()
    {
        var models = new List<AppUserRegisterDTO>()
        {
            new ()
            {
                Email = null!,
                Password = null!,
                ConfirmPassword = null!
            },
            new ()
            {
                Email = "test@test.com",
                Password = "  ",
                ConfirmPassword = "  "
            },
            new ()
            {
                Email = "test@test.com",
                Password = "csdf",
                ConfirmPassword = "hfgh"
            },
            new ()
            {
                Email = "fdsf",
                Password = "fsdfsdf",
                ConfirmPassword = "fsdfsdf"
            }
        };

        return models.Select(q => new object[] { q });
    }
}