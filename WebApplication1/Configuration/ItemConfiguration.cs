using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebApplication1.Models.Authentication;
using WebApplication1.Models.Inventory;

namespace WebApplication1.Configuration
{
    public class ItemConfiguration : IEntityTypeConfiguration<ItemModel>
    {
        public void Configure(EntityTypeBuilder<ItemModel> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).ValueGeneratedOnAdd().HasColumnName("Id").HasColumnType("uniqueidentifier");
            builder.Property(x => x.inventoryId).HasColumnName("inventoryId").HasColumnType("uniqueidentifier");
            builder.Property(x => x.Title).HasMaxLength(255).HasColumnType("varchar").HasColumnName("Title");
            builder.Property(x => x.Price).HasMaxLength(255).HasColumnType("varchar").HasColumnName("Price");
            builder.Property(x => x.Description).HasMaxLength(1000).HasColumnType("varchar").HasColumnName("Description");

            builder.HasOne<InventoryModel>().WithMany().HasForeignKey(u => u.inventoryId).OnDelete(DeleteBehavior.Cascade);

        }
    }
}
