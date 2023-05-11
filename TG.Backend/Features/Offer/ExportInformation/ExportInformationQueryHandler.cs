using Microsoft.AspNetCore.Identity.UI.Services;
using QRCoder;
using TG.Backend.Exceptions;
using TG.Backend.Models.Offer;
using TG.Backend.Repositories.Offer;

namespace TG.Backend.Features.Offer.ExportInformation;

public class ExportInformationQueryHandler : IRequestHandler<ExportInformationQuery, OfferResponse>
{
    private readonly IEmailSender _emailSender;
    private readonly IOfferRepository _offerRepository;
    private readonly IConfiguration _configuration;

    public ExportInformationQueryHandler(IEmailSender emailSender, IOfferRepository offerRepository,
        IConfiguration configuration)
    {
        _emailSender = emailSender;
        _offerRepository = offerRepository;
        _configuration = configuration;
    }

    public async Task<OfferResponse> Handle(ExportInformationQuery request, CancellationToken cancellationToken)
    {
        var offer = await _offerRepository.GetOfferByIdAsync(request.Id);

        if (offer is null)
            throw new OfferNotFoundException(request.Id);

        var qrcodeGenerator = new QRCodeGenerator();
        var qrCodeData = qrcodeGenerator.CreateQrCode($"{_configuration["Website:Url"]}Offer/{request.Id}",
            QRCodeGenerator.ECCLevel.Q);
        var qrCode = new BitmapByteQRCode(qrCodeData);
        var qrCodeImage = qrCode.GetGraphic(20);

        // TO DO
        // musimy uściślić jak to wysyłamy bo nie wiem czy to ma być HTML czy co
        // i trzeba link ustalic skoro go chcemy wyslac w kodzie qr
        await _emailSender.SendEmailAsync(request.Email, $"{offer.Vehicle.Name} - Tanie Graty", $"data:image/png;base64,{Convert.ToBase64String(qrCodeImage)}");

        return new OfferResponse
        {
            IsSuccess = true,
            StatusCode = HttpStatusCode.OK
        };
    }
}