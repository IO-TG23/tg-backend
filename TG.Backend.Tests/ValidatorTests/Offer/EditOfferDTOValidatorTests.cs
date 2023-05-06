using FluentValidation.TestHelper;
using TG.Backend.Features.Offer.EditOffer;
using TG.Backend.Features.Offer.Validation;
using TG.Backend.Models.Offer;
using TG.Backend.Models.Vehicle;

namespace TG.Backend.Tests.ValidatorTests.Offer;

public class EditOfferDTOValidatorTests
{
    // edit offer dto contains CreateOfferDTO model - validator for this model is tested in separate file
    private class EditOfferDtoWithId
    {
        public EditOfferDTO EditOfferDto { get; set; }
        public Guid Id { get; set; }
        
        public EditOfferDtoWithId(EditOfferDTO editOfferDto, Guid id)
        {
            EditOfferDto = editOfferDto;
            Id = id;
        }
    }

    [Theory]
    [MemberData(nameof(GetInvalidModels))]
    public void Validate_ForInvalidModel_ReturnsFailureForIdAndOffer(EditOfferDTO editOfferDto, Guid id)
    {
        // arrange
        var editOfferCommand = new EditOfferCommand(editOfferDto, id);
        var validator = new EditOfferDTOValidator();

        // act
        var result = validator.TestValidate(editOfferCommand);

        // assert
        result.ShouldHaveAnyValidationError();
    }

    [Fact]
    public void Validate_ForValidModel_ReturnsSuccess()
    {
        // arrange
        var model = new EditOfferDTO
        {
            OfferDto = new CreateOfferDTO
            {
                Vehicle = new VehicleDTO
                {
                    Name = "name",
                    Description = "desc",
                    ProductionStartYear = 1950,
                    NumberOfDoors = 1,
                    NumberOfSeats = 1,
                    BootCapacity = 1,
                    Length = 1,
                    Height = 1,
                    Width = 1,
                    WheelBase = 1,
                    BackWheelTrack = 1,
                    FrontWheelTrack = 1,
                    Gearbox = "Automatic",
                    Drive = "FWD",
                    ClientId = default
                },
                Description = "desc",
                ContactEmail = "test@test.com",
                ContactPhoneNumber = "123456789",
                Price = 1950
            }
        };
        var editOfferCommand = new EditOfferCommand(model, Guid.NewGuid());
        var validator = new EditOfferDTOValidator();
        // act
        var result = validator.TestValidate(editOfferCommand);

        // assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    public static IEnumerable<object[]> GetInvalidModels()
    {
        var models = new List<EditOfferDtoWithId>
        {
            new (new EditOfferDTO
            {
                OfferDto = new CreateOfferDTO
                {
                    Vehicle = new VehicleDTO
                    {
                        Name = null,
                        Description = null,
                        ProductionStartYear = 0,
                        ProductionEndYear = null,
                        NumberOfDoors = 0,
                        NumberOfSeats = 0,
                        BootCapacity = 0,
                        Length = 0,
                        Height = 0,
                        Width = 0,
                        WheelBase = 0,
                        BackWheelTrack = 0,
                        FrontWheelTrack = 0,
                        Gearbox = null,
                        Drive = null,
                        ClientId = default
                    },
                    Price = 0,
                    Description = null,
                    ContactEmail = null,
                    ContactPhoneNumber = null
                }
            }, Guid.Empty),
        };

        return models.Select(q => new object[] { q.EditOfferDto, q.Id });
    }
}