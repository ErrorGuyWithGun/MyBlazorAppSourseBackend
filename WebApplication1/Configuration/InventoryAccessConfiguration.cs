using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebApplication1.Models.Authentication;
using WebApplication1.Models.Inventory;

namespace WebApplication1.Configuration
{
    public class InventoryAccessConfiguration : IEntityTypeConfiguration<InventoryAccessModel>
    {
        public void Configure(EntityTypeBuilder<InventoryAccessModel> builder)
        {
            builder.HasKey(x => new {x.inventoryId,x.userId});
            builder.Property(x => x.inventoryId).HasColumnName("inventoryId").HasColumnType("uniqueidentifier");
            builder.Property(x => x.userId).HasColumnType("nvarchar(450)").HasColumnName("userId").IsRequired();

            builder.HasOne<InventoryModel>().WithMany().HasForeignKey(x => x.inventoryId).OnDelete(DeleteBehavior.Cascade);
            builder.HasOne<User>().WithMany().HasForeignKey(x => x.userId).HasPrincipalKey(u => u.Id).OnDelete(DeleteBehavior.NoAction);
        }
    }
}

