using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebApplication1.Models.Inventory;

namespace WebApplication1.Configuration
{
    public class ApiTokenConfiguration : IEntityTypeConfiguration<ApiTokenModel>
    {
        public void Configure(EntityTypeBuilder<ApiTokenModel> builder)
        {
            builder.HasKey(x => x.Id);
            
            builder.Property(x => x.Token)
                .IsRequired()
                .HasMaxLength(500);
                
            builder.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(200);
                
            builder.Property(x => x.CreatedByUserId)
                .IsRequired()
                .HasMaxLength(450);
                
            builder.Property(x => x.CreatedAt)
                .IsRequired();
                
            builder.Property(x => x.IsActive)
                .IsRequired()
                .HasDefaultValue(true);
                
            builder.HasOne(x => x.Inventory)
                .WithMany()
                .HasForeignKey(x => x.InventoryId)
                .OnDelete(DeleteBehavior.Cascade);
                
            builder.HasIndex(x => x.Token)
                .IsUnique();
                
            builder.HasIndex(x => x.InventoryId);
        }
    }
}

