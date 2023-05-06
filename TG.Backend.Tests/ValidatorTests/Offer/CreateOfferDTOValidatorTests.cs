using FluentValidation.TestHelper;
using TG.Backend.Features.Offer.CreateOffer;
using TG.Backend.Features.Offer.Validation;
using TG.Backend.Models.Offer;
using TG.Backend.Models.Vehicle;

namespace TG.Backend.Tests.ValidatorTests.Offer;

public class CreateOfferDTOValidatorTests
{
    private class CreateOfferDtoWithErrorMessage
    {
        public CreateOfferDTO CreateOfferDto { get; set; }
        public string ExpectedErrorMessage { get; set; }

        public CreateOfferDtoWithErrorMessage(CreateOfferDTO createOfferDto, string expectedErrorMessage)
        {
            CreateOfferDto = createOfferDto;
            ExpectedErrorMessage = expectedErrorMessage;
        }
    }

    [Theory]
    [MemberData(nameof(GetInvalidModelsForName))]
    public void Validate_ForInvalidModel_ReturnsFailureForVehicleName(CreateOfferDTO createOfferDto)
    {
        // arrange
        var createOfferCommand = new CreateOfferCommand(createOfferDto);
        var validator = new CreateOfferDTOValidator();

        // act
        var result = validator.TestValidate(createOfferCommand);

        // assert
        result
            .ShouldHaveValidationErrorFor(e => e.CreateOfferDto.Vehicle.Name)
            .WithErrorMessage("Name of vehicle cannot be empty!")
            .Only();
    }

    [Theory]
    [MemberData(nameof(GetInvalidModelsForProductionStartYear))]
    public void Validate_ForInvalid_Model_ReturnsFailureForProductionStartYear(CreateOfferDTO createOfferDto,
        string expectedErrorMessage)
    {
        // arrange
        var createOfferCommand = new CreateOfferCommand(createOfferDto);
        var validator = new CreateOfferDTOValidator();

        // act
        var result = validator.TestValidate(createOfferCommand);

        // assert
        result
            .ShouldHaveValidationErrorFor(e => e.CreateOfferDto.Vehicle.ProductionStartYear)
            .WithErrorMessage(expectedErrorMessage)
            .Only();
    }

    [Theory]
    [MemberData(nameof(GetInvalidModelsForProductionEndYear))]
    public void Validate_ForInvalid_Model_ReturnsFailureForProductionEndYear(CreateOfferDTO createOfferDto,
        string expectedErrorMessage)
    {
        // arrange
        var createOfferCommand = new CreateOfferCommand(createOfferDto);
        var validator = new CreateOfferDTOValidator();

        // act
        var result = validator.TestValidate(createOfferCommand);

        // assert
        result
            .ShouldHaveValidationErrorFor(e => e.CreateOfferDto.Vehicle.ProductionEndYear)
            .WithErrorMessage(expectedErrorMessage)
            .Only();
    }

    [Theory]
    [MemberData(nameof(GetInvalidModelsForDoorsSeatsCapacity))]
    public void Validate_ForInvalidModel_ReturnsFailureForNumberOfDoorsAndSeatsAndBootCapacity(
        CreateOfferDTO createOfferDto)
    {
        // arrange
        var createOfferCommand = new CreateOfferCommand(createOfferDto);
        var validator = new CreateOfferDTOValidator();

        // act
        var result = validator.TestValidate(createOfferCommand);

        // assert
        result.ShouldHaveValidationErrorFor(e => e.CreateOfferDto.Vehicle.NumberOfDoors);
        result.ShouldHaveValidationErrorFor(e => e.CreateOfferDto.Vehicle.NumberOfSeats);
        result.ShouldHaveValidationErrorFor(e => e.CreateOfferDto.Vehicle.BootCapacity);
    }

    [Theory]
    [MemberData(nameof(GetInvalidModelsForLength))]
    public void Validate_ForInvalidModel_ReturnsFailureForLength(CreateOfferDTO createOfferDto)
    {
        // arrange
        var createOfferCommand = new CreateOfferCommand(createOfferDto);
        var validator = new CreateOfferDTOValidator();

        // act
        var result = validator.TestValidate(createOfferCommand);

        // assert
        result
            .ShouldHaveValidationErrorFor(e => e.CreateOfferDto.Vehicle.Length)
            .WithErrorMessage("Length of vehicle must be positive!")
            .Only();
    }

    [Theory]
    [MemberData(nameof(GetInvalidModelsForGearBoxAndDrive))]
    public void Validate_ForInvalidModel_ReturnsFailureForGearBoxAndDrive(CreateOfferDTO createOfferDto)
    {
        // arrange
        var createOfferCommand = new CreateOfferCommand(createOfferDto);
        var validator = new CreateOfferDTOValidator();

        // act
        var result = validator.TestValidate(createOfferCommand);

        // assert
        result.ShouldHaveValidationErrorFor(e => e.CreateOfferDto.Vehicle.Gearbox);
        result.ShouldHaveValidationErrorFor(e => e.CreateOfferDto.Vehicle.Drive);
    }

    [Theory]
    [MemberData(nameof(GetInvalidModelsForEmail))]
    public void Validate_ForInvalidModel_ReturnsFailureForEmail(CreateOfferDTO createOfferDto,
        string expectedErrorMessage)
    {
        // arrange
        var createOfferCommand = new CreateOfferCommand(createOfferDto);
        var validator = new CreateOfferDTOValidator();

        // act
        var result = validator.TestValidate(createOfferCommand);

        // assert
        result
            .ShouldHaveValidationErrorFor(e => e.CreateOfferDto.ContactEmail)
            .WithErrorMessage(expectedErrorMessage)
            .Only();
    }

    [Theory]
    [MemberData(nameof(GetInvalidModelsForPhoneNumber))]
    public void Validate_ForInvalidModel_ReturnsFailureForPhoneNumber(CreateOfferDTO createOfferDto,
        string expectedErrorMessage)
    {
        // arrange
        var createOfferCommand = new CreateOfferCommand(createOfferDto);
        var validator = new CreateOfferDTOValidator();

        // act
        var result = validator.TestValidate(createOfferCommand);

        // assert
        result
            .ShouldHaveValidationErrorFor(e => e.CreateOfferDto.ContactPhoneNumber)
            .WithErrorMessage(expectedErrorMessage)
            .Only();
    }

    [Theory]
    [MemberData(nameof(GetValidModels))]
    public void Validate_ForValidModel_ReturnsSuccess(CreateOfferDTO createOfferDto)
    {
        // arrange
        var createOfferCommand = new CreateOfferCommand(createOfferDto);
        var validator = new CreateOfferDTOValidator();

        // act
        var result = validator.TestValidate(createOfferCommand);

        // assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    public static IEnumerable<object[]> GetValidModels()
    {
        var models = new List<CreateOfferDTO>
        {
            GetValidCreateOfferDto(),
            GetValidCreateOfferDto(createOfferDto => createOfferDto.Vehicle.Length = 5),
            GetValidCreateOfferDto(createOfferDto => createOfferDto.Vehicle.Gearbox = "Manual"),
            GetValidCreateOfferDto(createOfferDto =>
            {
                createOfferDto.Vehicle.Gearbox = "Manual";
                createOfferDto.Price = 1950;
                createOfferDto.Description = "dfsdfs";
                createOfferDto.Vehicle.NumberOfDoors = 5;
            })
        };

        return models.Select(q => new object[] { q });
    }

    public static IEnumerable<object[]> GetInvalidModelsForPhoneNumber()
    {
        var models = new List<CreateOfferDtoWithErrorMessage>
        {
            new(GetValidCreateOfferDto(createOfferDto => createOfferDto.ContactPhoneNumber = null),
                "Contact phone number cannot be empty!"),
            new(GetValidCreateOfferDto(createOfferDto => createOfferDto.ContactPhoneNumber = ""),
                "Contact phone number cannot be empty!"),
            new(GetValidCreateOfferDto(createOfferDto => createOfferDto.ContactPhoneNumber = "   "),
                "Contact phone number cannot be empty!"),
            new(GetValidCreateOfferDto(createOfferDto => createOfferDto.ContactPhoneNumber = "12345g789"),
                "Invalid phone number!"),
            new(GetValidCreateOfferDto(createOfferDto => createOfferDto.ContactPhoneNumber = "12345678"),
                "Invalid phone number!"),
            new(GetValidCreateOfferDto(createOfferDto => createOfferDto.ContactPhoneNumber = "1234567891"),
                "Invalid phone number!"),
        };

        return models.Select(q => new object[] { q.CreateOfferDto, q.ExpectedErrorMessage });
    }

    public static IEnumerable<object[]> GetInvalidModelsForEmail()
    {
        var models = new List<CreateOfferDtoWithErrorMessage>
        {
            new(GetValidCreateOfferDto(createOfferDto => createOfferDto.ContactEmail = null),
                "Contact email cannot be empty!"),
            new(GetValidCreateOfferDto(createOfferDto => createOfferDto.ContactEmail = ""),
                "Contact email cannot be empty!"),
            new(GetValidCreateOfferDto(createOfferDto => createOfferDto.ContactEmail = "   "),
                "Contact email cannot be empty!"),
            new(GetValidCreateOfferDto(createOfferDto => createOfferDto.ContactEmail = "gdfgdfgdfg"),
                "This is not valid email address!"),
            new(GetValidCreateOfferDto(createOfferDto => createOfferDto.ContactEmail = "testtest"),
                "This is not valid email address!"),
        };

        return models.Select(q => new object[] { q.CreateOfferDto, q.ExpectedErrorMessage });
    }

    public static IEnumerable<object[]> GetInvalidModelsForGearBoxAndDrive()
    {
        var models = new List<CreateOfferDTO>
        {
            GetValidCreateOfferDto(createOfferDto =>
            {
                createOfferDto.Vehicle.Gearbox = null;
                createOfferDto.Vehicle.Drive = null;
            }),
            GetValidCreateOfferDto(createOfferDto =>
            {
                createOfferDto.Vehicle.Gearbox = "Auto";
                createOfferDto.Vehicle.Drive = "PWD";
            }),
            GetValidCreateOfferDto(createOfferDto =>
            {
                createOfferDto.Vehicle.Gearbox = "";
                createOfferDto.Vehicle.Drive = "";
            }),
            GetValidCreateOfferDto(createOfferDto =>
            {
                createOfferDto.Vehicle.Gearbox = "   ";
                createOfferDto.Vehicle.Drive = "   ";
            }),
        };

        return models.Select(q => new object[] { q });
    }

    public static IEnumerable<object[]> GetInvalidModelsForLength()
    {
        var models = new List<CreateOfferDTO>
        {
            GetValidCreateOfferDto(createOfferDto => createOfferDto.Vehicle.Length = default),
            GetValidCreateOfferDto(createOfferDto => createOfferDto.Vehicle.Length = -5),
            GetValidCreateOfferDto(createOfferDto => createOfferDto.Vehicle.Length = 0)
        };

        return models.Select(q => new object[] { q });
    }

    public static IEnumerable<object[]> GetInvalidModelsForDoorsSeatsCapacity()
    {
        var models = new List<CreateOfferDTO>
        {
            GetValidCreateOfferDto(createOfferDto =>
            {
                createOfferDto.Vehicle.NumberOfDoors = -1;
                createOfferDto.Vehicle.NumberOfSeats = default;
                createOfferDto.Vehicle.BootCapacity = 0;
            }),
            GetValidCreateOfferDto(createOfferDto =>
            {
                createOfferDto.Vehicle.NumberOfDoors = 0;
                createOfferDto.Vehicle.NumberOfSeats = -5;
                createOfferDto.Vehicle.BootCapacity = default;
            }),
            GetValidCreateOfferDto(createOfferDto =>
            {
                createOfferDto.Vehicle.NumberOfDoors = default;
                createOfferDto.Vehicle.NumberOfSeats = 0;
                createOfferDto.Vehicle.BootCapacity = -3;
            })
        };

        return models.Select(q => new object[] { q });
    }

    public static IEnumerable<object[]> GetInvalidModelsForProductionEndYear()
    {
        var models = new List<CreateOfferDtoWithErrorMessage>
        {
            new(GetValidCreateOfferDto(createOfferDto => createOfferDto.Vehicle.ProductionEndYear = 1850),
                "Production end year must be empty or greater than 1900!"),
            new(GetValidCreateOfferDto(createOfferDto => createOfferDto.Vehicle.ProductionEndYear = 1940),
                "Production end year must be greater or equal to production start year!")
        };

        return models.Select(q => new object[] { q.CreateOfferDto, q.ExpectedErrorMessage });
    }

    public static IEnumerable<object[]> GetInvalidModelsForProductionStartYear()
    {
        var models = new List<CreateOfferDtoWithErrorMessage>
        {
            new(GetValidCreateOfferDto(createOfferDto => createOfferDto.Vehicle.ProductionStartYear = 1800),
                "Production start year must be greater or equal to 1900!"),
            new(GetValidCreateOfferDto(createOfferDto => createOfferDto.Vehicle.ProductionStartYear = default),
                "Production start year must be greater or equal to 1900!")
        };

        return models.Select(q => new object[] { q.CreateOfferDto, q.ExpectedErrorMessage });
    }

    public static IEnumerable<object[]> GetInvalidModelsForName()
    {
        var models = new List<CreateOfferDTO>
        {
            GetValidCreateOfferDto(createOfferDto => createOfferDto.Vehicle.Name = string.Empty),
            GetValidCreateOfferDto(createOfferDto => createOfferDto.Vehicle.Name = null),
            GetValidCreateOfferDto(createOfferDto => createOfferDto.Vehicle.Name = "    "),
            GetValidCreateOfferDto(createOfferDto => createOfferDto.Vehicle.Name = default)
        };

        return models.Select(q => new object[] { q });
    }

    private static CreateOfferDTO GetValidCreateOfferDto(Action<CreateOfferDTO>? action = null)
    {
        var createOfferDto = new CreateOfferDTO
        {
            Vehicle = new VehicleDTO
            {
                Name = "Name",
                Description = "Description",
                ProductionStartYear = 1950,
                ProductionEndYear = 2010,
                NumberOfDoors = 4,
                NumberOfSeats = 4,
                BootCapacity = 1600,
                Length = 2000,
                Height = 2500,
                Width = 1500,
                WheelBase = 150,
                BackWheelTrack = 150,
                FrontWheelTrack = 150,
                Gearbox = "Automatic",
                Drive = "FWD",
                ClientId = default
            },
            Price = 1500,
            Description = "Description",
            ContactEmail = "test@test.com",
            ContactPhoneNumber = "123456789"
        };

        action?.Invoke(createOfferDto);

        return createOfferDto;
    }
}