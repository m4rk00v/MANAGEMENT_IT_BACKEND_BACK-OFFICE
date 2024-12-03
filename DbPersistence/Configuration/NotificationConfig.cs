using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Domain.Entities;

namespace DbPersistence.Configuration;

public class NotificationConfig : IEntityTypeConfiguration<Notification>
{
    public void Configure(EntityTypeBuilder<Notification> builder)
    {
        builder.ToTable(nameof(Notification));
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Message)
            .HasMaxLength(250) 
            .IsRequired();

        builder.HasOne(x => x.User)
            .WithMany(x => x.Notifications) 
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}