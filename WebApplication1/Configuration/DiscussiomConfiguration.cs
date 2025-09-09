using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebApplication1.Models.Authentication;
using WebApplication1.Models.Inventory;

namespace WebApplication1.Configuration
{
    public class DiscussiomConfiguration: IEntityTypeConfiguration<DiscussionModel>
    {
        public void Configure(EntityTypeBuilder<DiscussionModel> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).ValueGeneratedOnAdd().HasColumnName("Id").HasColumnType("uniqueidentifier");
            builder.Property(x => x.inventoryId).HasColumnName("inventoryId").HasColumnType("uniqueidentifier");
            builder.Property(x => x.userId).HasColumnType("nvarchar(450)").HasColumnName("userId").IsRequired();
            builder.Property(x => x.Text).HasMaxLength(1000).HasColumnType("varchar").HasColumnName("Text");

            builder.HasOne<InventoryModel>().WithMany().HasForeignKey(u => u.inventoryId).OnDelete(DeleteBehavior.NoAction);
            builder.HasOne<User>().WithMany().HasForeignKey(x => x.userId).OnDelete(DeleteBehavior.Cascade);
        }
    }
}
