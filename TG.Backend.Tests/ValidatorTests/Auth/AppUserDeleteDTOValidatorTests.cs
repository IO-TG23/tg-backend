using FluentValidation.TestHelper;
using TG.Backend.Features.Auth.Validation;
using TG.Backend.Features.Command;
using TG.Backend.Models.Auth;

namespace TG.Backend.Tests.ValidatorTests.Auth;

public class AppUserDeleteDTOValidatorTests
{
    [Fact]
    public void Validate_For_EmptyEmail_ReturnsFailure()
    {
        // arrange
        var model = new AppUserDeleteDTO()
        {
            Email = ""
        };
        var command = new DeleteUserCommand(model);
        var validator = new AppUserDeleteDTOValidator();
        
        // act
        var result = validator.TestValidate(command);

        // assert
        result.ShouldHaveAnyValidationError();
    }
    
    [Fact]
    public void Validate_For_WhitespaceEmail_ReturnsFailure()
    {
        // arrange
        var model = new AppUserDeleteDTO()
        {
            Email = "  "
        };
        var command = new DeleteUserCommand(model);
        var validator = new AppUserDeleteDTOValidator();
        
        // act
        var result = validator.TestValidate(command);

        // assert
        result.ShouldHaveAnyValidationError();
    }
    
    [Fact]
    public void Validate_For_InvalidEmail_ReturnsFailure()
    {
        // arrange
        var model = new AppUserDeleteDTO()
        {
            Email = "testst"
        };
        var command = new DeleteUserCommand(model);
        var validator = new AppUserDeleteDTOValidator();
        
        // act
        var result = validator.TestValidate(command);

        // assert
        result.ShouldHaveAnyValidationError();
    }
    
    [Fact]
    public void Validate_For_ValidModel_ReturnsFailure()
    {
        // arrange
        var model = new AppUserDeleteDTO()
        {
            Email = "test@test.com"
        };
        var command = new DeleteUserCommand(model);
        var validator = new AppUserDeleteDTOValidator();
        
        // act
        var result = validator.TestValidate(command);

        // assert
        result.ShouldNotHaveAnyValidationErrors();
    }
}