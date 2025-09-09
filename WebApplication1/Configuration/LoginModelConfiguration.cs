using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebApplication1.Models.Authentication.LogIn;
using WebApplication1.Models.Authentication.SignUp;

namespace WebApplication1.Configuration
{
    public class LoginModelConfiguration : IEntityTypeConfiguration<LoginModel>
    {
        public void Configure(EntityTypeBuilder<LoginModel> builder)
        {
            builder.Property(x => x.Id).ValueGeneratedOnAdd().HasColumnName("Id").HasColumnType("uniqueidentifier");
            builder.Property(x => x.Email).HasMaxLength(255).HasColumnType("varchar").HasColumnName("Email").IsRequired();
            builder.Property(x => x.Password).HasMaxLength(255).HasColumnName("Password").IsRequired();
            builder.HasIndex(x => x.Email).IsUnique();
        }
    }
}
