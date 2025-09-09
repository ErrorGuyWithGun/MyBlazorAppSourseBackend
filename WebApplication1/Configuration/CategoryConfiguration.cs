using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebApplication1.Models.Authentication;
using WebApplication1.Models.Inventory;

namespace WebApplication1.Configuration
{
    public class CategoryConfiguration : IEntityTypeConfiguration<CategoryModel>
    {
        public void Configure(EntityTypeBuilder<CategoryModel> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).ValueGeneratedOnAdd().HasColumnName("Id").HasColumnType("uniqueidentifier");
            builder.Property(x => x.Name).HasMaxLength(255).HasColumnType("nvarchar").HasColumnName("Name").IsRequired();

        }
    }
}
