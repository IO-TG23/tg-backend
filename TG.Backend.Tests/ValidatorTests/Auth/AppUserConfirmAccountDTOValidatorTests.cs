using FluentValidation.TestHelper;
using TG.Backend.Features.Auth.Validation;
using TG.Backend.Features.Command;
using TG.Backend.Models.Auth;

namespace TG.Backend.Tests.ValidatorTests.Auth;

public class AppUserConfirmAccountDTOValidatorTests
{
    [Theory]
    [MemberData(nameof(GetInvalidModels))]
    public void Validate_ForInvalidModel_ReturnsFailure(AppUserConfirmAccountDTO appUserChangePasswordDto)
    {
        // arrange
        var command = new ConfirmAccountCommand(appUserChangePasswordDto.Token, appUserChangePasswordDto.Email);
        var validator = new AppUserConfirmAccountDTOValidator();

        // act
        var result = validator.TestValidate(command);

        // assert
        result.ShouldHaveAnyValidationError();
    }

    [Theory]
    [MemberData(nameof(GetValidModels))]
    public void Validate_ForValidModel_ReturnsFailure(AppUserConfirmAccountDTO appUserChangePasswordDto)
    {
        // arrange
        var command = new ConfirmAccountCommand(appUserChangePasswordDto.Token, appUserChangePasswordDto.Email);
        var validator = new AppUserConfirmAccountDTOValidator();

        // act
        var result = validator.TestValidate(command);

        // assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    public static IEnumerable<object[]> GetValidModels()
    {
        var models = new List<AppUserConfirmAccountDTO>()
        {
            new()
            {
                Email = "test@test.com",
                Token = "token"
            },
        };

        return models.Select(q => new object[] { q });
    }

    public static IEnumerable<object[]> GetInvalidModels()
    {
        var models = new List<AppUserConfirmAccountDTO>()
        {
            new()
            {
                Email = null!,
                Token = null!
            },
            new()
            {
                Email = "",
                Token = "dsf"
            },
            new()
            {
                Email = "test@test.com",
                Token = "  "
            },
            new()
            {
                Email = "fdsf",
                Token = ""
            }
        };

        return models.Select(q => new object[] { q });
    }
}