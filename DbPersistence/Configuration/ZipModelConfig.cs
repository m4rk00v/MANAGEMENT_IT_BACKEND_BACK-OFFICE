using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Domain.Entities;

namespace DbPersistence.Configuration;

public class ZipModelConfig : IEntityTypeConfiguration<ZipModel>
{
    public void Configure(EntityTypeBuilder<ZipModel> builder)
    {
        builder.ToTable(nameof(ZipModel));
        builder.HasKey(x => x.Id);

        builder.Property(x => x.CreatedAt)
            .IsRequired();

        builder.HasOne(x => x.Model)
            .WithOne(x => x.ZipModel);

    }
}