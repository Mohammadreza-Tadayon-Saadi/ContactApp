using Infrastructure.Entities.Contacts;
using Infrastructure.Entities.Groups;
using Infrastructure.Settings;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.FluentAPIs;

public sealed class GroupFluent : IEntityTypeConfiguration<Group>
{
    public void Configure(EntityTypeBuilder<Group> builder)
    {
        builder.HasKey(u => u.Id);
        builder.Property(ur => ur.Id).HasColumnName($"{nameof(Contact)}Id");

        builder.Property(u => u.Name)
            .IsRequired(false)
            .HasMaxLength(MaxLength.Common.Name);

        builder.HasMany(u => u.Contacts)
            .WithOne(ur => ur.GroupContact)
            .HasForeignKey(ur => ur.GroupId);
    }
}
