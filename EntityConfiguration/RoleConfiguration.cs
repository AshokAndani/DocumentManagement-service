using DocumentManagement.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DocumentManagement.EntityConfiguration
{

    /// <summary>
    /// Configuration regarding the Role Entity for EF.
    /// </summary>
    public class RoleConfiguration : IEntityTypeConfiguration<Role>
    {
        public void Configure(EntityTypeBuilder<Role> builder)
        {
            builder.ToTable("Role");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .ValueGeneratedOnAdd();

            builder.Property(x => x.Name)
                .IsRequired()
                .HasColumnType("varchar")
                .HasMaxLength(255)
                .HasColumnName("Name");

            builder.Property(x => x.IsDeleted)
                .HasColumnName("IsDeleted")
                .HasColumnType("bit")
                .HasDefaultValue(false)
                .ValueGeneratedOnAdd();

            builder.Property(x=> x.CreateTimestamp)
                .HasColumnType("datetime")
                .HasDefaultValueSql("GETUTCDATE()")
                .ValueGeneratedOnAdd();

            builder.Property(x => x.ModifyTimestamp)
                .HasColumnType("datetime")
                .HasDefaultValueSql ("GETUTCDATE()")
                .ValueGeneratedOnAddOrUpdate();

        }
    }
}
