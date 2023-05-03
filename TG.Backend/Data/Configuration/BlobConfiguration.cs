using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace TG.Backend.Data.Configuration;

public class BlobConfiguration : IEntityTypeConfiguration<Blob>
{
    public void Configure(EntityTypeBuilder<Blob> builder)
    {
        builder.HasKey(b => b.Id);

        builder.Property(b => b.Name)
            .IsRequired();
    }
}