namespace VOD.Infrastructure.SchemaDefinitions
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using VOD.Domain.Entities;

    public class KindEntitySchemaDefinitions : IEntityTypeConfiguration<Kind>
    {
        public void Configure(EntityTypeBuilder<Kind> builder)
        {
            builder.ToTable("Kinds", VODContext.DEFAULT_SCHEMA);
            builder.HasKey(k => k.Id);

            //builder.Property(p => p.Id)
            //    .ValueGeneratedOnAdd();

            builder.Property(p => p.Name)
               .IsRequired()
               .HasMaxLength(30);

            builder.Property(p => p.CreationDate);

            builder.Property(p => p.ModificationDate);

            builder
                .HasIndex(i => i.Name)
                .IsUnique();
        }
    }
}
