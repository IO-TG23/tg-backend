using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace TG.Backend.Data.Configuration;

public class VehicleConfiguration : IEntityTypeConfiguration<Vehicle>
{
    public void Configure(EntityTypeBuilder<Vehicle> builder)
    {
        builder.HasKey(v => v.Id);
        
        builder.Property(v => v.Name)
            .IsRequired();
        
        builder.Property(v => v.ProductionStartYear)
            .IsRequired();

        builder.Property(v => v.NumberOfDoors)
            .IsRequired();

        builder.Property(v => v.NumberOfSeats)
            .IsRequired();

        builder.Property(v => v.BootCapacity)
            .IsRequired();

        builder.Property(v => v.Length)
            .IsRequired();

        builder.Property(v => v.Height)
            .IsRequired();

        builder.Property(v => v.Width)
            .IsRequired();

        builder.Property(v => v.WheelBase)
            .IsRequired();

        builder.Property(v => v.BackWheelTrack)
            .IsRequired();

        builder.Property(v => v.FrontWheelTrack)
            .IsRequired();

        builder.Property(v => v.Gearbox)
            .IsRequired();

        builder.Property(v => v.Drive)
            .IsRequired();
    }
}