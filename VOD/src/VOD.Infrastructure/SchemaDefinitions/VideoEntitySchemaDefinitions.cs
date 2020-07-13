namespace VOD.Infrastructure.SchemaDefinitions
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using VOD.Domain.Entities;

    public class VideoEntitySchemaDefinitions : IEntityTypeConfiguration<Video>
    {
        public void Configure(EntityTypeBuilder<Video> builder)
        {
            builder.ToTable("Videos", VODContext.DEFAULT_SCHEMA);
            builder.HasKey(k => k.Id);

            //builder.Property(p => p.Id)
            //    .ValueGeneratedOnAdd();

            builder.Property(p => p.Title)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(p => p.AltTitle)
               .IsRequired()
               .HasMaxLength(150);

            builder.Property(p => p.Description)
               .IsRequired()
               .HasMaxLength(1000);

            builder.Property(p => p.CreationDate)
               .ValueGeneratedOnAdd();

            builder.Property(p => p.ModificationDate);

            builder
                .HasIndex(i => i.AltTitle)
                .IsUnique();

            builder
               .HasOne(e => e.Kind)
               .WithMany(c => c.Videos)
               .OnDelete(DeleteBehavior.Restrict);

            builder
               .HasOne(e => e.Genre)
               .WithMany(c => c.Videos)
               .OnDelete(DeleteBehavior.Restrict);

            //builder
            //    .HasMany(v => v.GenreLinks)
            //    .WithOne(vg => vg.Video)
            //    .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
