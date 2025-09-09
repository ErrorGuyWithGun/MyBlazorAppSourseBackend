using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebApplication1.Models.Authentication;
using WebApplication1.Models.Inventory;

namespace WebApplication1.Configuration
{
    public class InventoryConfiguration : IEntityTypeConfiguration<InventoryModel>
    {
        public void Configure(EntityTypeBuilder<InventoryModel> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).ValueGeneratedOnAdd().HasColumnName("Id").HasColumnType("uniqueidentifier");
            builder.Property(x => x.userId).HasColumnType("nvarchar(450)").HasColumnName("userId").IsRequired();
            builder.Property(x => x.Name).HasMaxLength(255).HasColumnType("varchar").HasColumnName("Name").IsRequired();
            builder.Property(x => x.isPublic).HasColumnName("isPublic").HasColumnType("bit").HasDefaultValue(false);
            builder.Property(x => x.categoryId).HasColumnName("categoryId").HasColumnType("uniqueidentifier");
            
            builder.HasOne<User>().WithMany().HasForeignKey(x => x.userId).OnDelete(DeleteBehavior.Cascade);
        }
    }
}
