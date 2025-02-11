using Infrastructure.Entities.Contacts;
using Infrastructure.Settings;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.FluentAPIs;

public sealed class ContactFluent : IEntityTypeConfiguration<Contact>
{
    public void Configure(EntityTypeBuilder<Contact> builder)
    {
        builder.HasKey(u => u.Id);
        builder.Property(ur => ur.Id).HasColumnName($"{nameof(Contact)}Id");

        builder.Property(u => u.Mobile)
            .HasMaxLength(MaxLength.Common.PhoneNumber)
            .IsFixedLength()
            .IsRequired();

        builder.Property(u => u.Photo)
            .HasDefaultValue("UserAvatar.png")
            .HasMaxLength(MaxLength.Common.Avatar)
            .IsRequired();

        builder.Property(u => u.Email)
            .IsRequired()
            .HasMaxLength(MaxLength.Common.Email);

        builder.Property(u => u.Job)
            .IsRequired()
            .HasMaxLength(MaxLength.Common.Name);

        builder.Property(u => u.FullName)
            .IsRequired(false)
            .HasMaxLength(MaxLength.Common.Name);

        builder.HasOne(u => u.GroupContact)
            .WithMany(ur => ur.Contacts)
            .HasForeignKey(ur => ur.GroupId);
    }
}
