namespace VOD.Infrastructure.SchemaDefinitions
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using VOD.Domain.Entities;

    public class GenreEntitySchemaDefinitions : IEntityTypeConfiguration<Genre>
    {
        public void Configure(EntityTypeBuilder<Genre> builder)
        {
            builder.ToTable("Genres", VODContext.DEFAULT_SCHEMA);
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

            //builder
            //    .HasMany(e => e.VideoLinks)
            //    .WithOne(c => c.Genre)
            //    .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
