using DocumentManagement.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DocumentManagement.EntityConfiguration
{

    /// <summary>
    /// Configuration regarding the Document Entity for EF.
    /// </summary>
    public class DocumentConfiguration : IEntityTypeConfiguration<Document>
    {
        public void Configure(EntityTypeBuilder<Document> builder)
        {
            builder.ToTable("Document");

            builder.HasKey(e => e.Id);

            builder.Property(x=> x.Id)
                .UseIdentityColumn();

            builder.Property(x => x.FileName)
                .HasColumnName("FileName")
                .HasMaxLength(512)
                .IsRequired();


            builder.Property(x => x.IsDeleted)
               .HasColumnName("IsDeleted")
               .HasColumnType("bit")
               .HasDefaultValue(false)
               .ValueGeneratedOnAdd();

            builder.Property(x => x.CreateTimestamp)
                .HasColumnType("datetime")
                .HasDefaultValueSql("GETUTCDATE()")
                .ValueGeneratedOnAdd();

            builder.Property(x => x.ModifyTimestamp)
                .HasColumnType("datetime")
                .HasDefaultValueSql("GETUTCDATE()")
                .ValueGeneratedOnAddOrUpdate();

            builder.Property(x => x.UploadedBy)
                .HasColumnName("UploadedBy")
                .HasColumnType("int")
                .IsRequired()
                .ValueGeneratedNever();

            builder.Property(x => x.ExtractedText)
                .HasColumnType("NVARCHAR(MAX)")
                .IsRequired()
                .HasColumnName("ExtractedText");

        }
    }
}
