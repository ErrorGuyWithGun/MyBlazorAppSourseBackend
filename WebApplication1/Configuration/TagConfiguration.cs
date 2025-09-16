using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebApplication1.Models.Inventory;

namespace WebApplication1.Configuration
{
    public class TagConfiguration : IEntityTypeConfiguration<TagsModel>
    {
        public void Configure(EntityTypeBuilder<TagsModel> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .HasMaxLength(50)
                .IsRequired()
                .HasColumnName("Id")
                .HasColumnType("nvarchar(50)");

            builder.Property(x => x.Name)
                .HasMaxLength(255)
                .IsRequired()
                .HasColumnName("Name")
                .HasColumnType("nvarchar(255)");

            builder.ToTable("Tags");
 
            builder.HasIndex(x => x.Name)
                .HasDatabaseName("IX_Tags_Name")
                .IsUnique();
        }
    }
}


