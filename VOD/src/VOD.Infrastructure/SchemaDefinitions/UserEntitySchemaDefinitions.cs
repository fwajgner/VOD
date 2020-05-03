namespace VOD.Infrastructure.SchemaDefinitions
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using VOD.Domain.Entities;

    public class UserEntitySchemaDefinitions : IEntityTypeConfiguration<ApplicationUser>
    {
        public void Configure(EntityTypeBuilder<ApplicationUser> builder)
        {
            builder.ToTable("Users", VODContext.DEFAULT_SCHEMA);
            builder.HasKey(k => k.Id);

            //builder.Property(p => p.Id)
            //    .ValueGeneratedOnAdd();

            builder.Property(p => p.UserName)
                .IsRequired()
                .IsUnicode(false)
                .HasMaxLength(30);

            builder.Property(p => p.Email)
                .IsRequired()
                .IsUnicode(false)
                .HasMaxLength(256);

            builder.Property(p => p.CreationDate)
                .ValueGeneratedOnAdd();

            builder.Property(p => p.ModificationDate);

            builder
               .HasIndex(i => i.UserName)
               .IsUnique();
        }
    }
}
