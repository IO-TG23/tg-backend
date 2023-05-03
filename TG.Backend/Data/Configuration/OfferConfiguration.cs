using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace TG.Backend.Data.Configuration;

public class OfferConfiguration : IEntityTypeConfiguration<Offer>
{
    public void Configure(EntityTypeBuilder<Offer> builder)
    {
        builder.HasKey(o => o.Id);

        builder.Property(o => o.Price)
            .HasPrecision(14, 2);

        builder.Property(o => o.Description)
            .IsRequired();

        builder.Property(o => o.ContactEmail)
            .IsRequired();

        builder.Property(o => o.ContactPhoneNumber)
            .IsRequired();

        builder.HasOne(o => o.Vehicle)
            .WithOne(v => v.Offer)
            .HasForeignKey<Offer>(o => o.VehicleId);

        builder.HasMany(o => o.Blobs)
            .WithOne()
            .HasForeignKey(b => b.OfferId);
    }
}