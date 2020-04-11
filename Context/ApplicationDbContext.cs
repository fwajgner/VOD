using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Entities;

namespace Context
{
    public class ApplicationDbContext : DbContext
    {
        #region Consctructors

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {

        }

        #endregion

        #region Properties

        public DbSet<ApplicationUser> Users { get; set; }

        public DbSet<Genre> Genres { get; set; }

        public DbSet<Entities.Type> Types { get; set; }

        public DbSet<Video> Videos { get; set; }

        public DbSet<VideoGenre> VideosGenres { get; set; }

        #endregion

        #region Methods

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            #region Users

            modelBuilder.Entity<ApplicationUser>()
                .ToTable("Users");

            modelBuilder.Entity<ApplicationUser>()
                .Property(u => u.Id)
                .ValueGeneratedOnAdd();

            modelBuilder.Entity<ApplicationUser>()
                .HasIndex(u => u.UserName)
                .IsUnique();

            modelBuilder.Entity<ApplicationUser>()
                .Property(u => u.UserName)
                .IsRequired()
                .IsUnicode(false)
                .HasMaxLength(30);

            modelBuilder.Entity<ApplicationUser>()
                .Property(u => u.Email)
                .IsRequired()
                .IsUnicode(false)
                .HasMaxLength(256);

            modelBuilder.Entity<ApplicationUser>()
                .Property(u => u.CreationDate)
                .ValueGeneratedOnAdd()
                .HasDefaultValue(DateTime.Now);

            modelBuilder.Entity<ApplicationUser>()
                .Property(u => u.ModificationDate);

            #endregion

            #region Genre

            modelBuilder.Entity<Genre>()
                .HasMany(g => g.VideoLinks)
                .WithOne(vg => vg.Genre)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Genre>()
                .Property(g => g.Id)
                .ValueGeneratedOnAdd();

            modelBuilder.Entity<Genre>()
                .HasIndex(g => g.Name)
                .IsUnique();

            modelBuilder.Entity<Genre>()
               .Property(g => g.Name)
               .IsRequired()
               .HasMaxLength(30);

            modelBuilder.Entity<Genre>()
                .Property(g => g.CreationDate)
                .ValueGeneratedOnAdd()
                .HasDefaultValue(DateTime.Now);

            modelBuilder.Entity<Genre>()
                .Property(g => g.ModificationDate);

            #endregion

            #region Type

            modelBuilder.Entity<Entities.Type>()
                .HasMany(v => v.Videos)
                .WithOne(t => t.Type)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Entities.Type>()
                .Property(t => t.Id)
                .ValueGeneratedOnAdd();

            modelBuilder.Entity<Entities.Type>()
                .HasIndex(t => t.Name)
                .IsUnique();

            modelBuilder.Entity<Entities.Type>()
               .Property(t => t.Name)
               .IsRequired()
               .HasMaxLength(30);

            modelBuilder.Entity<Entities.Type>()
                .Property(t => t.CreationDate)
                .ValueGeneratedOnAdd()
                .HasDefaultValue(DateTime.Now);

            modelBuilder.Entity<Entities.Type>()
                .Property(t => t.ModificationDate);

            #endregion

            #region Video

            modelBuilder.Entity<Video>()
                .HasOne(v => v.Type)
                .WithMany(t => t.Videos)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Video>()
                .HasMany(v => v.GenreLinks)
                .WithOne(vg => vg.Video)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Video>()
                .Property(v => v.Id)
                .ValueGeneratedOnAdd();

            modelBuilder.Entity<Video>()
                .Property(v => v.Title)
                .IsRequired()
                .HasMaxLength(100);

            modelBuilder.Entity<Video>()
                .HasIndex(v => v.AltTitle)
                .IsUnique();

            modelBuilder.Entity<Video>()
               .Property(v => v.AltTitle)
               .IsRequired()
               .HasMaxLength(150);

            modelBuilder.Entity<Video>()
               .Property(v => v.Description)
               .IsRequired()
               .HasMaxLength(1000);

            modelBuilder.Entity<Video>()
               .Property(v => v.CreationDate)
               .ValueGeneratedOnAdd()
               .HasDefaultValue(DateTime.Now);

            modelBuilder.Entity<Video>()
                .Property(v => v.ModificationDate);

            #endregion

            #region VideoGenre

            modelBuilder.Entity<VideoGenre>()
                .HasKey(v => new { v.VideoId, v.GenreId });

            modelBuilder.Entity<VideoGenre>()
                .HasOne(vg => vg.Genre)
                .WithMany(g => g.VideoLinks)
                .HasForeignKey(vg => vg.GenreId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<VideoGenre>()
                .HasOne(vg => vg.Video)
                .WithMany(v => v.GenreLinks)
                .HasForeignKey(vg => vg.VideoId)
                .OnDelete(DeleteBehavior.Restrict);

            #endregion
        }

        #endregion
    }
}
