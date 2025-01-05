using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class NotificationConfig : IEntityTypeConfiguration<Notification>
{
    public void Configure(EntityTypeBuilder<Notification> builder)
    {
        builder.ToTable(nameof(Notification));
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Message)
            .HasMaxLength(250)
            .IsRequired();

        builder.Property(x => x.Link)
            .HasMaxLength(1000)
            .IsRequired();

        builder.Property(x => x.IsReaded)
            .IsRequired();

        // Configurar explícitamente la relación
        builder.HasOne(x => x.User)
            .WithMany(x => x.Notifications)
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}