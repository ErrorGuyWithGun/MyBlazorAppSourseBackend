using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebApplication1.Models.Inventory;

namespace WebApplication1.Configuration
{
    public class ItemTagConfiguration : IEntityTypeConfiguration<ItemTagModel>
    {
        public void Configure(EntityTypeBuilder<ItemTagModel> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .ValueGeneratedOnAdd()
                .HasColumnName("Id")
                .HasColumnType("uniqueidentifier");

            builder.Property(x => x.ItemId)
                .IsRequired()
                .HasColumnName("ItemId")
                .HasColumnType("uniqueidentifier");

            builder.Property(x => x.TagId)
                .IsRequired()
                .HasMaxLength(50)
                .HasColumnName("TagId")
                .HasColumnType("nvarchar(50)");

            builder.ToTable("ItemTags");
 
            builder.HasOne(x => x.Item)
                .WithMany(x => x.ItemTags)
                .HasForeignKey(x => x.ItemId)
                .OnDelete(DeleteBehavior.Cascade);
            
            builder.HasOne(x => x.Tag)
                .WithMany(x => x.ItemTags)
                .HasForeignKey(x => x.TagId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(x => new { x.ItemId, x.TagId })
                .HasDatabaseName("IX_ItemTags_ItemId_TagId")
                .IsUnique();
        }
    }
}
