using FluentValidation.TestHelper;
using TG.Backend.Features.Auth.Validation;
using TG.Backend.Features.Command;
using TG.Backend.Models.Auth;

namespace TG.Backend.Tests.ValidatorTests.Auth;

public class AppUserChangepasswordDTOValidatorTests
{
    [Theory]
    [MemberData(nameof(GetInvalidModels))]
    public void Validate_ForInvalidModel_ReturnsFailure(AppUserChangePasswordDTO appUserChangePasswordDto)
    {
        // arrange
        var command = new ChangePasswordCommand(appUserChangePasswordDto);
        var validator = new AppUserChangePasswordDTOValidator();

        // act
        var result = validator.TestValidate(command);

        // assert
        result.ShouldHaveAnyValidationError();
    }
    
    [Theory]
    [MemberData(nameof(GetValidModels))]
    public void Validate_ForValidModel_ReturnsFailure(AppUserChangePasswordDTO appUserChangePasswordDto)
    {
        // arrange
        var command = new ChangePasswordCommand(appUserChangePasswordDto);
        var validator = new AppUserChangePasswordDTOValidator();

        // act
        var result = validator.TestValidate(command);

        // assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    public static IEnumerable<object[]> GetValidModels()
    {
        var models = new List<AppUserChangePasswordDTO>()
        {
            new AppUserChangePasswordDTO()
            {
                Email = "test@test.com",
                NewPassword = "password",
                Token = "token"
            },
        };

        return models.Select(q => new object[] { q });
    }
    
    public static IEnumerable<object[]> GetInvalidModels()
    {
        var models = new List<AppUserChangePasswordDTO>()
        {
            new AppUserChangePasswordDTO()
            {
                Email = null!,
                NewPassword = null!,
                Token = null!
            },
            new AppUserChangePasswordDTO()
            {
                Email = "",
                NewPassword = "  ",
                Token = "dsf"
            },
            new AppUserChangePasswordDTO()
            {
                Email = "test@test.com",
                NewPassword = "csdf",
                Token = "  "
            },
            new AppUserChangePasswordDTO()
            {
                Email = "fdsf",
                NewPassword = "fsdfsdf",
                Token = ""
            }
        };

        return models.Select(q => new object[] { q });
    }
}