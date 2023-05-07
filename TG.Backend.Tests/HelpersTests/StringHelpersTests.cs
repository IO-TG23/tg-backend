using FluentAssertions;
using TG.Backend.Data;
using TG.Backend.Helpers;

namespace TG.Backend.Tests.HelpersTests;

public class StringHelpersTests
{
    [Theory]
    [MemberData(nameof(GetInvalidGearboxNames))]
    public void BeValidGearBox_ForInvalidGearbox_ReturnsFalse(string value)
    {
        // act
        var result = value.BeValidGearbox();
        
        // assert
        result.Should().BeFalse();
    }
    
    [Theory]
    [MemberData(nameof(GetValidGearboxNames))]
    public void BeValidGearBox_ForValidGearbox_ReturnsTrue(string value)
    {
        // act
        var result = value.BeValidGearbox();
        
        // assert
        result.Should().BeTrue();
    }
    
    [Theory]
    [MemberData(nameof(GetInvalidDriveNames))]
    public void BeValidDrive_ForInvalidDrive_ReturnsFalse(string value)
    {
        // act
        var result = value.BeValidDrive();
        
        // assert
        result.Should().BeFalse();
    }
    
    [Theory]
    [MemberData(nameof(GetValidDriveNames))]
    public void BeValidDrive_ForValidDrive_ReturnsTrue(string value)
    {
        // act
        var result = value.BeValidDrive();
        
        // assert
        result.Should().BeTrue();
    }

    [Fact]
    public void GetGearbox_ForValidGearboxName_ReturnsCorrectGearboxEnum()
    {
        // arrange
        var value = "manual";
        
        // act
        var result = value.GetGearbox();
        
        // assert
        result.Should().Be(Gearbox.Manual);
    }
    
    [Fact]
    public void GetGearbox_ForInvalidGearboxName_ThrowsException()
    {
        // arrange
        var value = "dasd";
        
        // act
        var action = () => value.GetGearbox();
        
        // assert
        action.Should().Throw<Exception>();
    }
    
    [Fact]
    public void GetDrive_ForValidDriveName_ReturnsCorrectDriveEnum()
    {
        // arrange
        var value = "Fwd";
        
        // act
        var result = value.GetDrive();
        
        // assert
        result.Should().Be(Drive.FWD);
    }
    
    [Fact]
    public void GetDrive_ForInvalidDriveName_ThrowsException()
    {
        // arrange
        var value = "dasd";
        
        // act
        var action = () => value.GetDrive();
        
        // assert
        action.Should().Throw<Exception>();
    }
    
    public static IEnumerable<object[]> GetInvalidGearboxNames()
    {
        
        var models = new[]
        {
            "",
            null,
            "   ",
            "fsdfsdf"
        };

        return models.Select(q => new object[] { q });
    }
    
    public static IEnumerable<object[]> GetValidGearboxNames()
    {
        
        var models = new[]
        {
            "Automatic",
            "Manual",
            "AUTOMATIC",
            "manual"
        };

        return models.Select(q => new object[] { q });
    }
    
    public static IEnumerable<object[]> GetInvalidDriveNames()
    {
        
        var models = new[]
        {
            "PTE",
            null,
            "   ",
            ""
        };

        return models.Select(q => new object[] { q });
    }
    
    public static IEnumerable<object[]> GetValidDriveNames()
    {
        
        var models = new[]
        {
            "fwd",
            "Fwd",
            "FWD",
            "RwD"
        };

        return models.Select(q => new object[] { q });
    }
}