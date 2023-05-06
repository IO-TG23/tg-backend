using FluentValidation.TestHelper;
using TG.Backend.Features.Auth.Validation;
using TG.Backend.Features.Command;
using TG.Backend.Models.Auth;

namespace TG.Backend.Tests.ValidatorTests.Auth;

public class AppUserLoginDTOValidatorTests
{
    [Theory]
    [MemberData(nameof(GetInvalidModels))]
    public void Validate_ForInvalidModel_ReturnsFailure(AppUserLoginDTO appUserChangePasswordDto)
    {
        // arrange
        var command = new LoginUserCommand(appUserChangePasswordDto);
        var validator = new AppUserLoginDTOValidator();

        // act
        var result = validator.TestValidate(command);

        // assert
        result.ShouldHaveAnyValidationError();
    }
    
    [Theory]
    [MemberData(nameof(GetValidModels))]
    public void Validate_ForValidModel_ReturnsFailure(AppUserLoginDTO appUserChangePasswordDto)
    {
        // arrange
        var command = new LoginUserCommand(appUserChangePasswordDto);
        var validator = new AppUserLoginDTOValidator();

        // act
        var result = validator.TestValidate(command);

        // assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    public static IEnumerable<object[]> GetValidModels()
    {
        var models = new List<AppUserLoginDTO>()
        {
            new()
            {
                Email = "test@test.com",
                Password = "password",
                Code = "token"
            },
        };

        return models.Select(q => new object[] { q });
    }
    
    public static IEnumerable<object[]> GetInvalidModels()
    {
        var models = new List<AppUserLoginDTO>()
        {
            new()
            {
                Email = null!,
                Password = null!,
                Code = null!
            },
            new()
            {
                Email = "",
                Password = "  ",
                Code = "dsf"
            },
            new()
            {
                Email = "test@test.com",
                Password = "csdf",
                Code = "  "
            },
            new()
            {
                Email = "fdsf",
                Password = "fsdfsdf",
                Code = ""
            }
        };

        return models.Select(q => new object[] { q });
    }
}