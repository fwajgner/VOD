namespace VOD.Infrastructure.SchemaDefinitions
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using VOD.Domain.Entities;

    public class UserEntitySchemaDefinitions : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.Property(p => p.Name)
                .HasMaxLength(30);

            builder.Property(p => p.CreationDate);

            builder.Property(p => p.ModificationDate);
        }
    }
}
