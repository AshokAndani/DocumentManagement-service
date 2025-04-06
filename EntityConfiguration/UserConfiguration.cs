using DocumentManagement.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DocumentManagement.EntityConfiguration
{
    /// <summary>
    /// Configuration regarding the User Entity for EF.
    /// </summary>
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            // Table Name
            builder.ToTable("User");

            // Primary Key
            builder.HasKey(u => u.Id);

            // Identity Column
            builder.Property(u => u.Id)
                   .ValueGeneratedOnAdd()
                   .UseIdentityColumn();

            // Column Constraints
            builder.Property(u => u.Username)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.Property(u => u.PasswordHash)
                   .IsRequired();

            builder.Property(u => u.Email)
                   .IsRequired()
                   .HasMaxLength(255);

            // Boolean Field
            builder.Property(u => u.IsDeleted)
                   .HasDefaultValue(false);

            // Timestamps
            builder.Property(u => u.CreateTimestamp)
                   .HasColumnType("datetime")
                   .HasDefaultValueSql("GETUTCDATE()")
                   .ValueGeneratedOnAdd();

            builder.Property(u => u.ModifyTimestamp)
                   .HasColumnType("datetime")
                   .HasDefaultValueSql("GETUTCDATE()")
                   .ValueGeneratedOnAddOrUpdate();
        }
    }

}
